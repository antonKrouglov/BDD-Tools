using System;
using System.Collections.Generic;
using System.Linq;
using BddTools.Util;
using DecisionDiagrams;

namespace BddTools.BddReorder {
    public class ReordererBruteForce : IReorderer {

        private ReorderResult result;
        private ReorderParams paramz;

        public ReordererBruteForce() {
            result = new();
            paramz = new();
        }

        public ReordererBruteForce(ReorderParams @params, ReorderResult result) {
            this.paramz = @params ?? throw new ArgumentNullException(nameof(@params));
            this.result = result ?? throw new ArgumentNullException(nameof(result));
        }

        public ReorderResult Result => result;

        public ReorderParams Params => paramz;

        /// <summary> Slow brute force reordering </summary>
        public void Process() => result = Reorder(paramz);

        /// <summary> Slow brute force reordering </summary>
        public ReorderResult Reorder(ReorderParams data) {
            //paramz = data;
            var ret = new ReorderResult(
                anyResultFound: false
                , minComplexityFound: int.MaxValue
                , minComplexityPermIndeões: new List<int>());
            //result = ret;
            var formula = data.bddFormula.FormulaInner;

            if (data.orderPermutations != null) {
                foreach (var (pm, pmIndex) in data.orderPermutations) {
                    if (data.indexSelector != null && !(data.indexSelector(pmIndex))) continue;

                    var ddm = new DDManager<BDDNode>();
                    var ddmVarsInitial = Enumerable.Range(0, data.varCount).Select(i => ddm.CreateBool()).ToArray();
                    var ddmVars = ddmVarsInitial.ReorderArray(pm.ToArray());
                    var dd = formula.Evaluate(ddm, ddmVars, formula);

                    var currentComplexity = ddm.NodeCount(dd);
                    if (currentComplexity > ret.MinComplexityFound) {
                        continue;
                    }

                    ret.AnyResultFound = true;

                    if (currentComplexity == ret.MinComplexityFound) {
                        ret.MinComplexityPermIndeões.Add(pmIndex);
                        continue;
                    }

                    //currentComplexity <= minComplexity
                    ret.MinComplexityFound = currentComplexity;
                    ret.MinComplexityPermIndeões = pmIndex.Yield().ToList();
                }
            }
            else {
                foreach (var (pm, pmIndex) in data.orderPermutationsAlt) {
                    if (data.indexSelector != null && !(data.indexSelector(pmIndex))) continue;

                    var ddm = new DDManager<BDDNode>();
                    var ddmVarsInitial = Enumerable.Range(0, data.varCount).Select(i => ddm.CreateBool()).ToArray();
                    var ddmVars = ddmVarsInitial.ReorderArray(pm.ToArray());
                    var dd = formula.Evaluate(ddm, ddmVars, formula);

                    var currentComplexity = ddm.NodeCount(dd);
                    if (currentComplexity > ret.MinComplexityFound) {
                        continue;
                    }

                    ret.AnyResultFound = true;

                    if (currentComplexity == ret.MinComplexityFound) {
                        ret.MinComplexityPermIndeões.Add(pmIndex);
                        continue;
                    }

                    //currentComplexity <= minComplexity
                    ret.MinComplexityFound = currentComplexity;
                    ret.MinComplexityPermIndeões = pmIndex.Yield().ToList();
                }
            }

            return ret;
        }
    }
}