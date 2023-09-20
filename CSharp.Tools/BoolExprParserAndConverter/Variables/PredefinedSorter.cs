using System;
using System.Collections.Generic;

namespace BddTools.Variables {
    public class PredefinedSorter : IComparer<string> {
        private readonly Func<string, int> sortFunction;
        public PredefinedSorter(Func<string, int> sortFunction) => this.sortFunction = sortFunction;
        public int Compare(string? x, string? y) => sortFunction(x ?? "") - sortFunction(y ?? "");
    }
}