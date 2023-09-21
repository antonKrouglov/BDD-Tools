using System;
using System.Collections.Generic;
using BddTools.AbstractSyntaxTrees;

namespace BddTools.BddReorder {
    public class ReorderParams
    {
        public int varCount;
        public IEnumerable<(IEnumerable<int> pm, int pmIndex)> orderPermutations;
        public IEnumerable<(int[] pm, int pmIndex)> orderPermutationsAlt;
        public BddMappedFormula bddFormula;
        //public Func<int, bool> indexSelector = null;
        public ThreadPartitioner partitioner;

        public ReorderParams() { }

        public ReorderParams(BddMappedFormula bddFormula, IEnumerable<(IEnumerable<int> pm, int pmIndex)> orderPermutations, int varCount)
        {
            this.bddFormula = bddFormula;
            this.orderPermutations = orderPermutations;
            this.varCount = varCount;
        }
    }
}