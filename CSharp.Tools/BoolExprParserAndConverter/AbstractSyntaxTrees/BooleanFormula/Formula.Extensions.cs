using System;
using System.Collections.Generic;
using System.Linq;
using BddTools.Parser;
using BddTools.Util;
using DecisionDiagrams;
using System.Collections;
using BddTools.Variables;

namespace BddTools.AbstractSyntaxTrees {

    /// <summary> Abstract Syntax Tree representation for Boolean expressions.
    /// Main file is taken from https://github.com/microsoft/DecisionDiagrams/blob/master/DecisionDiagrams.Tests/Formula.cs </summary>
    public partial class Formula { //my extension methods

        /// <summary> True for the root node </summary>
        public bool IsRoot { get; set; }


        /// <summary> The formula's parent or Null for Root Formula. </summary>
        public Formula Parent { get; set; }


        public IEnumerable<Formula> Ancestors
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            => Parent == null ? Enumerable.Empty<Formula>() : Parent.Yield().Concat(Parent.Ancestors);


        // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
        private IEnumerable<Formula> ChildrenSafe => Children ?? Enumerable.Empty<Formula>();


        public IEnumerable<Formula> Descendants =>
            ChildrenSafe
                .Concat(ChildrenSafe
                    .SelectMany(f => f.Descendants));

        public IEnumerable<Formula> DescendantsAndSelf
            => this.Yield().Concat(Descendants);

        /// <summary> Is Leaf means no child nodes </summary>
        public bool IsLeaf => !ChildrenSafe.Any();

        public IEnumerable<Formula> Leafs
            => DescendantsAndSelf.Where(fc => fc.IsLeaf);

        // ReSharper disable once InconsistentNaming
        public IEnumerable<Formula> LeafsOfType_TRUE
            => Leafs.Where(fc => fc.IsBoolLiteralTRUE);

        public bool IsIte
            => Type == AstType.ITE;

        public bool IsVar
            => Type == AstType.VAR;

        // ReSharper disable once InconsistentNaming
        public bool IsBoolLiteralTRUE
            => Type == AstType.TRUE;

        // ReSharper disable once InconsistentNaming
        public bool IsBoolLiteralFALSE
            => Type == AstType.FALSE;

        public Formula IteCondition => Children[0];

        public Formula IteThen {
            get => Children[1];
            set => Children[1] = value;
        }

        public Formula IteElse {
            get => Children[2];
            set => Children[2] = value;
        }

        public void BuildCaches() {
            FillUpRoot(true);
            FillUpParents();
        }

        public void FillUpRoot(bool isRoot) {
            IsRoot = isRoot;
            foreach (var child in ChildrenSafe) {
                child.FillUpRoot(false);
            }
        }

        public void FillUpParents() {
            foreach (var child in ChildrenSafe) {
                child.Parent = this;
                child.FillUpParents();
            }
        }

        // ReSharper disable once InconsistentNaming
        public bool ParentRefersToMeAs_ThenExpr {
            get {
                if (IsRoot || !Parent.IsIte) return false;
                return ReferenceEquals(Parent.IteThen, this);
            }
        }

        // ReSharper disable once InconsistentNaming
        public bool ParentRefersToMeAs_ElseExpr {
            get {
                if (IsRoot || !Parent.IsIte) return false;
                return ReferenceEquals(Parent.IteElse, this);
            }
        }

        // ReSharper disable InconsistentNaming
        public string ToStringWithVarNames(IReadOnlyDictionary<int, string> varNames, FormulaPrintOptions? po = null) {
            // ReSharper restore InconsistentNaming

            po ??= FormulaPrintOptions.Default;

            var arguments = ChildrenSafe.Select(f => f.ToStringWithVarNames(varNames, po)).ToList();
            string expText;

            switch (Type) {
                case AstType.TRUE:
                case AstType.FALSE:
                    return $"{Type}";

                case AstType.VAR:
                    return Data is int varIndex
                        ? varNames[varIndex]
                        : $"VAR({Data})";

                case AstType.AND:
                    expText = $"{arguments[0]}{po.AND}{arguments[1]}";
                    if (po.EncloseAND) {
                        expText = $"({expText})";
                    }

                    return expText;

                case AstType.OR:
                    expText = $"{arguments[0]}{po.OR}{arguments[1]}";
                    if (po.EncloseOR) {
                        expText = $"({expText})";
                    }

                    return expText;

                case AstType.NOT:
                    expText = $"{po.NOT}{arguments[0]}";
                    if (po.EncloseNOT) {
                        expText = $"({expText})";
                    }

                    return expText;

                case AstType.IFF:

                case AstType.IMPLIES:

                case AstType.ITE:
                    string expr;
                    if (po.IndentAndLineBreakITE) {
                        var indents = Ancestors.Count();
                        var pad1 = string.Empty.PadRight(indents, '\t');
                        var pad2 = string.Empty.PadRight(indents + 1, '\t');
                        var cond = IteCondition.ToStringWithVarNames(varNames, po);
                        var then = IteThen.ToStringWithVarNames(varNames, po);
                        var els = IteElse.ToStringWithVarNames(varNames, po);
                        expr = $"ITE({cond},\n{pad2}{then},\n{pad2}{els}\n{pad1})";
                    }
                    else {
                        expr = $"ITE({string.Join(", ", arguments)})";
                    }

                    return expr;

                case AstType.REPLACE:

                case AstType.EXISTS:

                case AstType.FORALL:
                    // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                    if (Data != null) {
                        arguments.Add($"{Data}");
                    }

                    return arguments.Count == 0
                        ? $"{Type}"
                        : $"{Type}({string.Join(", ", arguments)})";

                default:
                    throw new ArgumentOutOfRangeException(nameof(Type));
            }
        }

        /// <summary> Calculate formula for all variable variants (truth table) </summary>
        /// <param name="numVars">Number of variables </param>
        /// <returns>Truth table</returns>
        public BitArray EvaluateAll(int numVars) {
            var f = this;
            var maxVariants = 1 << numVars; //2 power numVars
            var variants = new BitArray(numVars);
            var truthTable = new BitArray(maxVariants) {
                [0] = f.Evaluate(f, variants.ToPositionBitDict())
            };

            for (var j = 1; j < maxVariants; j++) {
                variants.Increment();
                truthTable[j] = f.Evaluate(f, variants.ToPositionBitDict());
            }

            return truthTable;
        }

        public BitArray EvaluateAll() =>
            EvaluateAll(this.VarIndexes().Max() + 1); //index is zero-based

        public BitArray EvaluateAll(ImmutableVarsBag vars) =>
            EvaluateAll(vars.MaxIndex + 1); //index is zero-based

        /// <summary> Create a random formula up to a maximum depth. Formula is limited to ITE expressions </summary>
        /// <param name="random">The random number generator.</param>
        /// <param name="numVars">The number of variables to use.</param>
        /// <param name="maxDepth">The maximum depth of the formula.</param>
        /// <returns></returns>
        public static Formula CreateRandomIte(Random random, int numVars, int maxDepth) {
            if (numVars < 0 || maxDepth < 0) {
                throw new ArgumentException("Invalid call to CreateRandom.");
            }

            return CreateRandomIte(random, numVars, maxDepth, 0);
        }

        /// <summary>
        /// Create a random formula up to a maximum depth.
        /// </summary>
        /// <param name="random">The random number generator.</param>
        /// <param name="numVars">The number of variables to use.</param>
        /// <param name="maxDepth">The maximum depth of the formula.</param>
        /// <param name="currentDepth">The current depth.</param>
        /// <returns></returns>
        private static Formula CreateRandomIte(Random random, int numVars, int maxDepth, int currentDepth) {
            var r = random.Next(10);
            r = currentDepth == maxDepth ? r % 3 : r;

            switch (r) {
                case 0:
                    return Formula.True();
                case 1:
                    return Formula.False();
                case 2:
                case 3:
                case 4:
                case 5:
                    return Formula.Var(random.Next(numVars));
                case 6:
                case 7:
                case 8:
                case 9:
                    return Formula.Ite(
                        CreateRandom(random, numVars, maxDepth, currentDepth + 1),
                        CreateRandom(random, numVars, maxDepth, currentDepth + 1),
                        CreateRandom(random, numVars, maxDepth, currentDepth + 1));
                default:
                    throw new Exception("impossible");
            }
        }

        /// <summary> Minimize Formula by converting to Binary Decision Diagram and back to Formula (BDD-mapped) </summary>
        /// <param name="variablesOrdered"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public BddMappedFormula MinimizeWithGivenOrder(IReadOnlyDictionary<int, string> variablesOrdered) {
            var ddm = new DDManager<BDDNode>();

            var numVars = variablesOrdered.Keys.Max() + 1; //zero indexed!
            var ddmVars = new VarBool<BDDNode>[numVars];
            for (var i = 0; i < numVars; i++) {
                ddmVars[i] = ddm.CreateBool();
            }

            var dd = Evaluate(ddm, ddmVars, this);

            //dd manager cant convert to Formula; so we use ugly text parsing
            var ddText = ddm.DisplayWithVarNames(dd, variablesOrdered);
            var parser = parseIteWithGivenOrder(ddText, variablesOrdered);
            return parser.BddMappedFormula;
        }

        internal static ParserOfIteExpressions? parseIteWithGivenOrder(string ddText, IReadOnlyDictionary<int, string> variablesOrdered) {
            var parser = new ParserOfIteExpressions(ddText);
            if ((!parser.ParseITE(variablesOrdered)) || (parser.BddMappedFormula == null)) {
                throw new Exception($"Failed to convert BDD to formula: [{ddText}]\nError: [{parser?.SyntaxErrors?.FirstOrDefault()}]");
            }

            return parser;
        }


        public IOrderedEnumerable<int> VarIndexes() =>
            DescendantsAndSelf
                .Where(f => f is { IsVar: true, Data: int })
                .Select(f => (int)f.Data)
                .Distinct()
                .OrderBy(i => Comparer<int>.Default);

    }
}