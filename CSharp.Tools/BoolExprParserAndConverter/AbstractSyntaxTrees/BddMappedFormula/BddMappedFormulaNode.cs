using System.Collections.Immutable;

namespace BddTools.AbstractSyntaxTrees {
    /// <summary> Formula node + negation flag </summary>
    public struct BddMappedFormulaNode {
        public Formula Formula;
        public bool Negation;
        public ImmutableDictionary<int, string> vars;

        public string PrintVariableAndNegation() {
            var negationExpr = Negation ? "!" : "";
        
            if (Formula.Data is int intData) {
                return $"{negationExpr}{vars[intData]}";
            }
            return $"{negationExpr}{Formula.Data}";
        }

        public override string ToString() =>
            PrintVariableAndNegation();

    }
}