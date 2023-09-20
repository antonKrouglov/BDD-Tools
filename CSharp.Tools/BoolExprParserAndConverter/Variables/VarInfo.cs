using System.Collections.Generic;

namespace BddTools.Variables {
    public struct VarInfo {
        public readonly string Name;
        public readonly int Index;

        public object? Tag; //any additional payload

        public VarInfo(string name, int index, object? tag = null) {
            Name = name;
            Index = index;
            Tag = tag;
        }

        public VarInfo(KeyValuePair<string, int> kv, object? tag = null) {
            Tag = tag;
            Name = kv.Key;
            Index = kv.Value;
        }

        public VarInfo(KeyValuePair<int, string> kv, object? tag = null) {
            Tag = tag;
            Name = kv.Value;
            Index = kv.Key;
        }


    }
}