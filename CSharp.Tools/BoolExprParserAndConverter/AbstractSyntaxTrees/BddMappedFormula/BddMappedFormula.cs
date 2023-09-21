using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using BddTools.BddReorder;
using BddTools.Parser;
using BddTools.Util;
using BddTools.Variables;
using DecisionDiagrams;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static BddTools.AbstractSyntaxTrees.Formula;
using BddPath = System.Collections.Generic.List<BddTools.AbstractSyntaxTrees.BddMappedFormulaNode>;
using BddPathsList = System.Collections.Generic.List<System.Collections.Generic.List<BddTools.AbstractSyntaxTrees.BddMappedFormulaNode>>;

namespace BddTools.AbstractSyntaxTrees {

    /// <summary>
    /// Formula limited to Binary Decision Diagram representation
    ///     BddFormula ::= ITE(condition_expression, then_expression, else_expression ) | boolLiteral | variableName
    ///     condition_expression ::= variableName
    ///     then_expression ::= BddFormula
    ///     else_expression ::= BddFormula
    /// Leaf nodes should be limited to boolLiteral.
    /// So Ite(x, y, z) should be rewritten as Ite(x,  Ite(y, true, false),  Ite(z, true, false))
    /// </summary>
    public class BddMappedFormula {
        public ImmutableVarsBag Variables { get; private set; } = null;

        public Formula FormulaInner { get; }

        private BddMappedFormula(Formula formulaInner, ImmutableVarsBag variables) {
            FormulaInner = formulaInner;
            Variables = variables;
        }


        /// <summary> Adapt formula as BDD formula; patch if needed; Throw exception if formula cannot be adapted </summary>
        /// <param name="formula"></param>
        /// <param name="predefinedVars">predefined variable list; If null will be rebuild from formula</param>
        /// <returns> ITE formula which can be mapped 1:1 to BDD </returns>
        public static BddMappedFormula Adapt(Formula formula, ImmutableVarsBag? predefinedVars) {
            BddMappedFormulaBuilder.CheckFormulaIsBddMappedAndPatchIfNeeded(formula);

            var vars = predefinedVars ?? //build default vars
                       new ImmutableVarsBag(formula
                           .DescendantsAndSelf
                           .Where(f => f is { IsVar: true, Data: int })
                           .Select(f => new VarInfo($"VAR{(int)f.Data}", (int)f.Data, f.Data)));

            return new BddMappedFormula(formula, vars);
        }


        /// <summary>
        /// Function traces back path from each TRUE leaf back to the root;
        /// We have ITE(Condition, Then, Else) for each formula non-leaf node.
        /// Conditions are mapped 1:1 to BDD nodes, so we add them as path members.
        /// BDD Node contains also Negation flag.
        /// Consider this formula: edge node->[Else]->child
        ///     Such node will appear negated (NOT x) in BDD path.
        ///     'x' here is the variable name in node's Condition expression
        /// </summary>
        /// <param name="leaf"> Leaf node; it has no children; </param>
        /// <returns>List of BDD nodes used to build DNF form of the entire soc Formula</returns>
        private BddPath GetPathBackToTheRoot(Formula leaf) {
            var lst = new BddPath();
            var node = leaf;

            while (!node.IsRoot) {
                BddMappedFormulaNode bf;
                bf.Formula = node.Parent.IteCondition;
                bf.Negation = node.ParentRefersToMeAs_ElseExpr;
                bf.vars = Variables.IdxToNameDict;
                lst.Add(bf);
                node = node.Parent;
            }

            return lst;
        }

        /// <summary> Convert formula into DNF expression parts </summary>
        /// <returns> ListOfOrExpressions ( ListOfAndExpressions () ) </returns>
        public BddPathsList DnfParts() {
            var lst = new BddPathsList();
            foreach (var path in FormulaInner.LeafsOfType_TRUE.Select(GetPathBackToTheRoot)) {
                path.Reverse();
                lst.Add(path);
            }

            return lst;
        }

        /// <summary> Convert formula into DNF expression </summary>
        /// <returns>DNF expression string; Example: (x1 &amp; x2) | (!x1 &amp; x3)</returns>
        public string AsDnf(string Bool_AND_OpSymbol = " & ", string Bool_OR_OpSymbol = " | ") {
            var src = DnfParts();

            //helper method
            string CombinePathAtomsWithBool_ANDs(BddPath bddPath) {
                var exprText = string.Join(Bool_AND_OpSymbol, bddPath);
                var ret = $"({exprText})";
                return ret;
            }

            var ors = string.Join(Bool_OR_OpSymbol, src.Select(CombinePathAtomsWithBool_ANDs));

            return ors;
        }

        public override string ToString()
            => ToString(false);


        public string ToString(bool indentAndLineBreakIte)
            => FormulaInner.ToStringWithVarNames(Variables.IdxToNameDict, new FormulaPrintOptions(indentAndLineBreakIte: indentAndLineBreakIte));


        public string ToStringWithVarIndexes()
            => FormulaInner.ToString();


        /// <summary> Amount of BDD tree nodes excluding TRUE and FALSE leafs </summary>
        /// <returns> BDD nodes count minus 2 </returns>
        public int NodesCount()
            => FormulaInner.DescendantsAndSelf.Count(f => f.IsIte);

        /// <summary> Amount of BDD tree nodes including TRUE and FALSE leafs </summary>
        /// <returns> BDD nodes count </returns>
        public int BddNodesCount()
            => NodesCount()+2;

        public BddMappedFormula MinimizeWithGivenOrder()
            => FormulaInner.MinimizeWithGivenOrder(Variables.IdxToNameIDict);


        /// <summary> Reorder variables in BDD to achieve minimal complexity (nodes count) </summary>
        /// <returns> 1st found minimal BDD-mapped Formula </returns>
        /// <remarks> performance is low as we are just iterating over all possible variable orders </remarks>
        public BddMappedFormula Reorder() {
            //prepare lists
            var initialSortInfo =
                Variables
                    .List
                    .OrderBy(vi => vi.Index)
                    .ToList();
            var initialSortIndexes =
                initialSortInfo
                    .Select(vi => vi.Index)
                    .ToArray();
            var varCount = initialSortIndexes.Count();

            //verify collections
            var expectedArray = Enumerable.Range(0, varCount).ToArray();
            CollectionAssert.AreEqual(expectedArray, initialSortIndexes);

            //prepare permutations
            var sortIndexes = initialSortIndexes.CloneArray(); //we need a clone as GetPermutationsFast changes array order
            var orderPermutations = sortIndexes
                .GetPermutationsFast()
                //.GetPermutationsSafe()
                .Select((pm, pmIndex) => (pm: (IEnumerable<int>)pm, pmIndex: pmIndex));

            //check counts - SLOWWW
            //var permutationsCount = (ulong)sortIndexes.CloneArray().GetPermutationsFast().LongCount();
            //Assert.AreEqual(permutationsCount, ((ulong)varCount).Factorial());

            IReorderer reorderer;
            reorderer = new
                //ReordererBruteForce
                //ReordererParallel
                ReordererParallelEx
                    (new ReorderParams(bddFormula: this, orderPermutations, varCount), new ReorderResult());
            reorderer.Process();

            if (!reorderer.Result.AnyResultFound) //nothing to do
                return this;

            var selectedPermutation = initialSortIndexes
                .CloneArray()
                .GetPermutationsFast()
                .Skip(reorderer.Result.MinComplexityPermIndeхes.First())
                .First();
            var permutedSortInfo = initialSortInfo
                .Select(vi => new VarInfo(vi.Name, selectedPermutation[vi.Index], vi.Tag));

            var newVarsOrder = new ImmutableVarsBag(permutedSortInfo).IdxToNameIDict;

            return TransformToNewOrder(newVarsOrder);
        }


        public BddMappedFormula TransformToNewOrder(IReadOnlyDictionary<int, string> newVarsOrder) {
            var newVarsText = string.Join(",", newVarsOrder.OrderBy(kv => kv.Key).Select(kv => kv.Value));

            ParserOfIteExpressions? parser;

            var newFormula = (parser = parseIteWithGivenOrder(this.ToString(), newVarsOrder))?
                             .BddMappedFormula
                             ?? throw new Exception(
                                 $"Internal error: parsing [{this.ToString()}] [{newVarsText}] failed [{parser?.SyntaxErrors?.First()}]");
            //we need to minimize two times
            return newFormula.MinimizeWithGivenOrder();
        }
    }
}