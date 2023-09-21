using System.Collections.Generic;

namespace BddTools.BddReorder {
    public class ReorderResult {
        public int MinComplexityFound;
        public List<int> MinComplexityPermIndeхes;
        public bool AnyResultFound;

        public ReorderResult() {
        }

        public ReorderResult(bool anyResultFound, int minComplexityFound, List<int> minComplexityPermIndeхes) {
            MinComplexityPermIndeхes = minComplexityPermIndeхes;
            MinComplexityFound = minComplexityFound;
            AnyResultFound = anyResultFound;
        }
    }
}