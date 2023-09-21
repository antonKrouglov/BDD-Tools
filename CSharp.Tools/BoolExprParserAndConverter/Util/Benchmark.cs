using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;

namespace BddTools.Util {
    /// <summary> https://stackoverflow.com/a/37640207/2746150 </summary>
    public class Benchmark : IDisposable {
        private readonly Stopwatch timer = new Stopwatch();
        private readonly string benchmarkName;
        private readonly Action<string?> logResultAction;

        public Benchmark(string benchmarkName, Action<string?>? logResultDelegate = null) {
            logResultAction = logResultDelegate ?? Console.WriteLine;
            this.benchmarkName = benchmarkName;
            timer.Start();
        }

        public void Dispose() {
            timer.Stop();
            logResultAction($" {timer.Elapsed.Humanize(4)} for {benchmarkName}\n");
        }
    }
}