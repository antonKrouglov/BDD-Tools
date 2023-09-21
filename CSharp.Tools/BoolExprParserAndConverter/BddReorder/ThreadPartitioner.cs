using System;
using System.Diagnostics;

namespace BddTools.BddReorder {
    public class ThreadPartitioner {
        public readonly int numThreads; //1-infinity
        public readonly int curThread; // 0 to numThreads-1

        public ThreadPartitioner() {
        }

        public ThreadPartitioner(int numThreads, int curThread) {
            this.numThreads = numThreads;
            this.curThread = curThread;
            if (numThreads < 1) throw new ArgumentException(nameof(numThreads));
            if (curThread < 0 || curThread > numThreads - 1) throw new ArgumentException(nameof(curThread));
        }

        /// <summary>
        /// Partition permutation indexes {0,1,2,3,5,6,7,8,9,10...} into numThreads chunks
        /// </summary>
        /// <returns></returns>
        public bool CheckPermIndexBelongsToThisThread(int permIndex, int[] pma) {
            bool checkResult;
            if (numThreads == 1)
                checkResult = true;
            else {
                var reminder = permIndex % numThreads;

                checkResult = reminder == curThread;
            }

            Debug.Write($"thcheck: curThread={curThread} numThreads={numThreads} permIndex={permIndex} pma={{{string.Join(",", pma)}}} ");
            return checkResult;
        }


    }
}