using BddPath = System.Collections.Generic.List<BddTools.AST_Implementations.BDD.BddPathNode>;
using BddPathsList = System.Collections.Generic.List<System.Collections.Generic.List<BddTools.AST_Implementations.BDD.BddPathNode>>;

namespace BddTools.AST_Implementations.BDD;

/// <summary>
/// Formula limited to Binary Decision Diagram representation
///     BddFormula ::= ITE(condition_expression, then_expression, else_expression ) | boolLiteral | variableName
///     condition_expression ::= variableName
///     then_expression ::= BddFormula
///     else_expression ::= BddFormula
/// Leaf nodes should be limited to boolLiteral.
/// So Ite(x, y, z) should be rewritten as Ite(x,  Ite(y, true, false),  Ite(z, true, false))
/// </summary>
public class BddFormula {
    public Formula Formula { get; }

    private BddFormula(Formula formula) {
        Formula = formula;
    }

    public override string ToString() => Formula.ToString();

    /// <summary> Adapt formula as BDD formula; patch if needed; Throw exception if formula cannot be adapted </summary>
    /// <param name="formula"></param>
    /// <returns> ITE formula which can be mapped 1:1 to BDD </returns>
    public static BddFormula Adapt(Formula formula) {
        BddFormulaBuilder.VerifyAndPatch(formula);
        return new BddFormula(formula);
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
            BddPathNode bddPathNode;
            bddPathNode.Formula = node.Parent.IteCondition;
            bddPathNode.Negation = node.ParentRefersToMeAs_ElseExpr;
            lst.Add(bddPathNode);
            node = node.Parent;
        }

        return lst;
    }

    /// <summary> Convert formula into DNF expression parts </summary>
    /// <returns> ListOfOrExpressions ( ListOfAndExpressions () ) </returns>
    public BddPathsList DnfParts() {
        var lst = new BddPathsList();
        foreach (var path in Formula.LeafsOfType_TRUE.Select(GetPathBackToTheRoot)) {
            path.Reverse();
            lst.Add(path);
        }

        return lst;
    }

    /// <summary> Convert formula into DNF expression </summary>
    /// <returns>DNF expression string; Example: (x1 &amp; x2) | (!x1 &amp; x3)</returns>
    public string AsDnf() {
        const string Bool_AND_OpSymbol = " & ";
        const string Bool_OR_OpSymbol = " | ";
        var src = DnfParts();

        //helper method
        string CombinePathAtomsWithBool_AND_OpSymbols(BddPath bddPath) {
            var exprText = string.Join(Bool_AND_OpSymbol, bddPath);
            var ret = $"({exprText})";
            return ret;
        }

        var ors = string.Join(Bool_OR_OpSymbol, src.Select(CombinePathAtomsWithBool_AND_OpSymbols));

        return ors;
    }
}