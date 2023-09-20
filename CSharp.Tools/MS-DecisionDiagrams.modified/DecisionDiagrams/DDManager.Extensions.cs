using System.Collections.Generic;

namespace DecisionDiagrams {
    /// <summary> My extensions. </summary>
    public sealed partial class DDManager<T> {
        /// <summary>
        /// Display with variable names and ITE(cond,thenExpr,elseExpr).
        /// </summary>
        /// <param name="value"></param>
        /// <param name="varNames"></param>
        /// <returns></returns>
        public string DisplayWithVarNames(DD value, IReadOnlyDictionary<int, string> varNames) {
            this.Check(value.ManagerId);
            return this.DisplayWithVarNames(value.Index, varNames);
        }

        internal string DisplayWithVarNames(DDIndex index, IReadOnlyDictionary<int, string> varNames) {
            return this.DisplayWithVarNames(index, false, varNames);
        }

        internal string DisplayWithVarNames(DDIndex index, bool negated, IReadOnlyDictionary<int, string> varNames) {
            if (index.IsOne()) {
                return negated ? "false" : "true";
            }

            if (index.IsZero()) {
                return negated ? "true" : "false";
            }

            negated ^= index.IsComplemented();
            var node = this.MemoryPool[index.GetPosition()];
            string ret;
            if (factory is BDDNodeFactory bddNodeFactory && node is BDDNode bddNode) {
                ret = bddNodeFactory.DisplayWithVarNames(bddNode, negated, varNames);
            }
            else {
                ret = factory.Display(node, negated);
            }
            return ret;
        }
    }
}