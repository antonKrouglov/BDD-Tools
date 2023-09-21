using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BddTools.Util;

namespace BddTools.BddReorder {
    /// <summary> Multithreaded version of ReordererBruteForce
    /// Manual chunking is used
    /// </summary>
    public class ReordererParallelEx : IReorderer {

        #region fields

        private ReorderResult result;
        private ReorderParams paramz;

        public ReorderResult Result => result;
        public ReorderParams Params => paramz;

        #endregion


        #region constructors

        public ReordererParallelEx() {
            result = new();
            paramz = new();
        }

        public ReordererParallelEx(ReorderParams @params, ReorderResult result) {
            this.paramz = @params ?? throw new ArgumentNullException(nameof(@params));
            this.result = result ?? throw new ArgumentNullException(nameof(result));
        }

        #endregion


        #region interface implementation

        /// <summary> Slow brute force reordering </summary>
        public void Process() => result = Reorder(paramz);

        #endregion


        public int numThreads = 8;

        /// <summary> Better brute force reordering </summary>
        public ReorderResult Reorder(ReorderParams data) {
            //paramz = data;
            var ret = new ReorderResult(
                anyResultFound: false
                , minComplexityFound: int.MaxValue
                , minComplexityPermIndeхes: new List<int>());
            //result = ret;
            var formula = data.bddFormula.FormulaInner;

            //build and partition permutations manually
            var src = Enumerable.Range(0, data.varCount).ToArray();
            var bufs = Enumerable.Range(0, numThreads).Select(thread => src.CloneArray()).ToArray();
            var workloads = Enumerable.Range(0, numThreads)
                .Select<int, IReorderer>(thread => new ReordererBruteForce() {
                    Params = {
                        varCount = data.varCount
                        //, indexSelector = i => (numThreads == 1 || ((i % numThreads) == thread))
                        , partitioner = new ThreadPartitioner(numThreads, thread)
                        , orderPermutations = null
                        , orderPermutationsAlt = bufs[thread].GetPermutationsFast().Select((pm, pmIndex) => (pm: pm, pmIndex: pmIndex))
                        , bddFormula = data.bddFormula
                    }
                }).ToArray<IReorderer>();

            Parallel.ForEach(workloads, workload => workload.Process());

            var results = workloads.Where(w => w.Result.AnyResultFound).Select(w => w.Result).ToList();
            if (results.Any()) {
                ret.AnyResultFound = true;
                ret.MinComplexityFound = results.Select(r => r.MinComplexityFound).Min();
                ret.MinComplexityPermIndeхes = results
                    .Where(r => r.MinComplexityFound == ret.MinComplexityFound)
                    .SelectMany(r => r.MinComplexityPermIndeхes).ToList();
            }
            else {
                ret.AnyResultFound = false;
            }

            return ret;
        }

    }
}