using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.X86;
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
                , minComplexityFound: data.bddFormula.BddNodesCount()//int.MaxValue
                , minComplexityPermIndeões: new List<int>());
            //result = ret;

            if (data.orderPermutations != null) {
                foreach (var (pm, pmIndex) in data.orderPermutations) {
                    var pma = pm.ToArray();
                    var shouldProcessThisPerm = data.partitioner?.CheckPermIndexBelongsToThisThread(pmIndex, pma) ?? true;
                    if (shouldProcessThisPerm) {
                        PermuteBDD(data, pma, ret, pmIndex);
                    }
                    else {
                        Debug.Write($"skipped");
                    }

                    Debug.WriteLine($"");
                }
            }
            else {
                foreach (var (pm, pmIndex) in data.orderPermutationsAlt) {
                    var pma = pm.ToArray();
                    var shouldProcessThisPerm = data.partitioner?.CheckPermIndexBelongsToThisThread(pmIndex, pma) ?? true;
                    if (shouldProcessThisPerm) {
                        PermuteBDD(data, pma, ret, pmIndex);
                    }
                    else {
                        Debug.Write($"skipped");
                    }

                    Debug.WriteLine($"");
                }
            }

            return ret;
        }

        private static void PermuteBDD(ReorderParams data, int[] pma, ReorderResult ret, int pmIndex) {
            var formula = data.bddFormula.FormulaInner;
            var ddm = new DDManager<BDDNode>();
            var ddmVarsInitial = Enumerable.Range(0, data.varCount).Select(i => ddm.CreateBool()).ToArray();
            var ddmVars = ddmVarsInitial.ReorderArray(pma);
            var dd = formula.Evaluate(ddm, ddmVars, formula);

            var currentComplexity = ddm.NodeCount(dd);
            Debug.Write($"complexity={currentComplexity}");
            if (currentComplexity > ret.MinComplexityFound) {
                return;
            }

            ret.AnyResultFound = true;

            if (currentComplexity == ret.MinComplexityFound) {
                ret.MinComplexityPermIndeões.Add(pmIndex);
                return;
            }

            //currentComplexity <= minComplexity
            ret.MinComplexityFound = currentComplexity;
            ret.MinComplexityPermIndeões = pmIndex.Yield().ToList();
        }
    }
}