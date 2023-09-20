using System;
using System.Collections.Generic;
using System.Linq;

namespace BddTools.Variables {
    public class VarsBuilder {

        #region private fields

        private readonly Dictionary<string, VarInfo> vars;

        #endregion


        #region constructors

        public VarsBuilder()
            => this.vars = new Dictionary<string, VarInfo>();

        public VarsBuilder(IEnumerable<VarInfo> vars) {
            if (vars == null) throw new ArgumentNullException(nameof(vars));
            this.vars = vars.ToDictionary(v => v.Name, v => v);
        }

        #endregion

        
        public int AddNewOrGetExisting(string name, object? tag = null) {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (vars.TryGetValue(name, out var var)) {
                return var.Index;
            }

            var newIndex = vars.Any() ? vars.Values.Max(vi => vi.Index) + 1 : 0;
            var vi = new VarInfo(name, newIndex, tag);
            vars.Add(name, vi);
            return newIndex;
        }

        public ImmutableVarsBag Build()
            => new(vars.Values);

        public ImmutableVarsBag Build(IEnumerable<int> varIndexesToInclude) {
            if (varIndexesToInclude == null) throw new ArgumentNullException(nameof(varIndexesToInclude));
            var query = (from v in vars.Values
                join idx in varIndexesToInclude on v.Index equals idx
                select v).Distinct();
            return new ImmutableVarsBag(query);
        }

        public Dictionary<string, object?> Tags
            => vars.ToDictionary(kv => kv.Key, kv => kv.Value.Tag);

    }
}