namespace DecisionDiagrams {
    using System.Collections.Generic;

    /// <summary>
    /// Implementation of a factory for BDDNode objects.
    /// Calls back into DDManager recursively.
    /// </summary>
    internal partial struct BDDNodeFactory {
        /// <summary>
        /// How to display a node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="negated">Parity of negation.</param>
        /// <param name="varNames">Variable names.</param>
        /// <returns>The string representation.</returns>
        public string DisplayWithVarNames(BDDNode node, bool negated, IReadOnlyDictionary<int, string> varNames)
        {
            return string.Format(
                "ITE({0}, {1}, {2})",
                varNames[node.Variable - 1],
                this.Manager.DisplayWithVarNames(node.High, negated, varNames),
                this.Manager.DisplayWithVarNames(node.Low, negated, varNames));
        }
    }
}