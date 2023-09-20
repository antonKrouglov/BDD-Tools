using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using static System.StringSplitOptions;

namespace BddTools.Variables {


    public class ImmutableVarsBag {

        private readonly ImmutableDictionary<string, int> varNamesToIndexCache;

        public ImmutableVarsBag(string varNamesSortedCsv) : 
            this(varNamesSortedCsv
                    .Split(',', TrimEntries | RemoveEmptyEntries)) {
        }

        public ImmutableVarsBag(string[] varNamesSorted) {
            var j = 0;
            varNamesToIndexCache = ImmutableDictionary.CreateRange(
                varNamesSorted.Select(varName => new KeyValuePair<string, int>(varName, j++))
            );
        }

        public ImmutableVarsBag(IReadOnlyList<string> varNames, IReadOnlyList<int> varIndexes) {
            var j = 0;
            varNamesToIndexCache = ImmutableDictionary.CreateRange(
                varNames.Select(varName => new KeyValuePair<string, int>(varName, varIndexes[j++]))
            );
        }

        public ImmutableVarsBag(IEnumerable<VarInfo> varList) {
            varNamesToIndexCache = ImmutableDictionary.CreateRange(
                varList.Select(vi => new KeyValuePair<string, int>(vi.Name, vi.Index))
            );
        }

        public ImmutableVarsBag(ImmutableVarsBag src) : this(src.List) {
        }

        public ImmutableVarsBag(ImmutableVarsBag src, IReadOnlyList<int> varIndexes) 
            : this(src.SortedList, varIndexes) { }

        public string GetName(int index) => Get(index).Name;

        public int GetIndex(string name) => Get(name).Index;

        public VarInfo Get(int index) =>
            Contains(index, out var name)
                ? new VarInfo(name, index)
                : throw new ArgumentException($"Variable #{index} not found");

        public VarInfo Get(string name) =>
            Contains(name)
                ? new VarInfo(name, varNamesToIndexCache[name])
                : throw new ArgumentException($"Variable \"{name}\" not found");

        public bool Contains(int index) => Contains(index, out var _);

        public bool Contains(int index, out string name) {
            var kvFound = varNamesToIndexCache.Where(kv => kv.Value == index).Take(1).ToList();
            if (kvFound.Any()) {
                name = kvFound.First().Key;
                return true;
            }
            else {
                name = string.Empty;
                return false;
            }
        }

        public bool Contains(string name) => varNamesToIndexCache.ContainsKey(name ?? string.Empty);

        public string[] SortedList =>
            (new SortedSet<string>(varNamesToIndexCache.Keys, new PredefinedSorter(GetIndex))).ToArray();

        public IEnumerable<VarInfo> List =>
            varNamesToIndexCache.Select(kv => new VarInfo(kv));
        
        public ImmutableDictionary<int,string> IdxToNameDict =>
            varNamesToIndexCache
                .ToImmutableDictionary(kv => kv.Value
                    ,kv => kv.Key);

        public IReadOnlyDictionary<int, string> IdxToNameIDict =>
            new ReadOnlyDictionary<int, string>(IdxToNameDict);//IdxToNameDict.AsReadOnly();

        public override string ToString() =>
            string.Join(',', SortedList);

        public int Count =>
            varNamesToIndexCache.Count;

        public int MaxIndex =>
            varNamesToIndexCache.Values.Max();
    }
}