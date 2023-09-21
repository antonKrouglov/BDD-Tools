using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BddTools.Util;
using DecisionDiagrams;
//using Microsoft.Build.Framework;

namespace BddTools.BddReorder {
    /// <summary> Multithreaded version of ReordererBruteForce using Chunk
    /// Does not support GetPermutationsFast()
    /// </summary>
    public class ReordererParallel : IReorderer {

        #region fields

        private ReorderResult result;
        private ReorderParams paramz;

        public int numThreads = 8;
        public ReorderResult Result => result;
        public ReorderParams Params => paramz;

        #endregion


        #region constructors

        public ReordererParallel() {
            result = new();
            paramz = new();
        }

        public ReordererParallel(ReorderParams @params, ReorderResult result) {
            this.paramz = @params ?? throw new ArgumentNullException(nameof(@params));
            this.result = result ?? throw new ArgumentNullException(nameof(result));
        }

        #endregion


        #region interface implementation

        /// <summary> Slow brute force reordering </summary>
        public void Process() => result = Reorder(paramz);

        #endregion


        /// <summary> Slow brute force reordering </summary>
        public ReorderResult Reorder(ReorderParams data) {
            //paramz = data;
            var ret = new ReorderResult(
                anyResultFound: false
                , minComplexityFound: int.MaxValue
                , minComplexityPermIndeхes: new List<int>());
            //result = ret;
            var formula = data.bddFormula.FormulaInner;

#if NET6_0_OR_GREATER //Chunk requires NET Core 6

        var permutationsCount = ((ulong)data.varCount).Factorial();
        var chunkSize = (int)Math.Ceiling(permutationsCount / (double)numThreads);
        var chunks = data.orderPermutations.Chunk(chunkSize).ToArray();
        var workloads = chunks
            .Select<(IEnumerable<int> pm, int pmIndex)[], IReorderer>(chunk
                => new ReordererBruteForce(data, new ReorderResult()) {
                    Params = { orderPermutations = chunk }
                }).ToArray();

        Parallel.ForEach(workloads, workload => workload.Process());

        var minComplexity = workloads.Select(r => r.Result.MinComplexityFound).Min();

        ret.MinComplexityPermIndeхes = workloads
            .Where(r => r.Result.MinComplexityFound == minComplexity)
            .SelectMany(r => r.Result.MinComplexityPermIndeхes).ToList();
#endif //NET6_0_OR_GREATER

            return ret;
        }

    }
}