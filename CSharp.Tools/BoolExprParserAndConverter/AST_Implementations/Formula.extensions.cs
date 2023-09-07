namespace BddTools;

/// <summary> Helper extension https://stackoverflow.com/q/1577822/2746150 </summary>
file static class EnumerableExt {
    /// <summary>
    /// Wraps this object instance into an IEnumerable&lt;T&gt;
    /// consisting of a single item.
    /// </summary>
    /// <typeparam name="T"> Type of the object. </typeparam>
    /// <param name="item"> The instance that will be wrapped. </param>
    /// <returns> An IEnumerable&lt;T&gt; consisting of a single item. </returns>
    public static IEnumerable<T> Yield<T>(this T item) {
        yield return item;
    }
}

public partial class Formula {
    public static Formula Var(string varname) => new Formula(AstType.VAR, null, data: varname);

    /// <summary> True for the root node </summary>
    public bool IsRoot { get; set; }

    /// <summary> The formula's parent or Null for Root Formula. </summary>
    public Formula Parent { get; set; }


    // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
    private IEnumerable<Formula> ChildrenSafe => Children ?? Enumerable.Empty<Formula>();

    /// <summary> Is Leaf means no child nodes </summary>
    public bool IsLeaf => !ChildrenSafe.Any();


    public IEnumerable<Formula> Descendants =>
        ChildrenSafe
            .Concat(ChildrenSafe
                .SelectMany(f => f.Descendants));

    public IEnumerable<Formula> DescendantsAndSelf => this.Yield().Concat(Descendants);

    public IEnumerable<Formula> Leafs => this.DescendantsAndSelf.Where(fc => fc.IsLeaf);

    public IEnumerable<Formula> LeafsOfType_TRUE => this.Leafs.Where(fc => fc.IsBoolLiteralTRUE);

    public bool IsIte => this.Type == AstType.ITE;

    public bool IsVar => this.Type == AstType.VAR;

    public bool IsBoolLiteralTRUE => this.Type == AstType.TRUE;

    public bool IsBoolLiteralFALSE => this.Type == AstType.FALSE;

    public Formula IteCondition => this.Children[0];

    public Formula IteThen {
        get => this.Children[1];
        set => this.Children[1] = value;
    }

    public Formula IteElse {
        get => this.Children[2];
        set => this.Children[2] = value;
    }

    public void FillUp() {
        FillUpRoot(true);
        FillUpParents();
    }

    public void FillUpRoot(bool isRoot) {
        IsRoot = isRoot;
        foreach (var child in ChildrenSafe) {
            child.FillUpRoot(false);
        }
    }

    public void FillUpParents() {
        foreach (var child in ChildrenSafe) {
            child.Parent = this;
            child.FillUpParents();
        }
    }

    // ReSharper disable once InconsistentNaming
    public bool ParentRefersToMeAs_ThenExpr {
        get {
            if (IsRoot || !Parent.IsIte) return false;
            return ReferenceEquals(Parent.IteThen, this);
        }
    }

    // ReSharper disable once InconsistentNaming
    public bool ParentRefersToMeAs_ElseExpr {
        get {
            if (IsRoot || !Parent.IsIte) return false;
            return ReferenceEquals(Parent.IteElse, this);
        }
    }


    ///// <summary> The formula's parent at [0], grandparent at [1] and so on up to the root. </summary>
    //public List<Formula> Ancestors
    //{
    //    get
    //    {
    //        if (IsRoot) return new List<Formula>();
    //        ;
    //        var ret = Parent.Ancestors;
    //        ret.Insert(0, Parent);
    //        return ret;
    //    }
    //}

    ///// <summary> The formula's parent or Null for Root Formula. </summary>
    //public Formula BddParent
    //    => !isIteExpr ? null : Parent.Children[0];

    ///// <summary> The formula's parent at [0], grandparent at [1] and so on up to the root. </summary>
    //public List<Formula> BddAncestors
    //{
    //    get
    //    {
    //        if (IsRoot) return new List<Formula>();
    //        var ret = Parent.BddAncestors;
    //        ret.Insert(0, Parent);
    //        if (IsLeaf && Type == AstType.VAR)
    //            ret.Insert(0, this);
    //        return ret;
    //    }
    //}


    //public IEnumerable<Formula> IteDescendants
    //{
    //    get
    //    {
    //        if (Type != AstType.ITE) return Enumerable.Empty<Formula>();
    //        var iteChildren = Children.Skip(1).ToList();
    //        var ret = iteChildren
    //            .Concat(iteChildren
    //                .SelectMany(f => f.IteDescendants));
    //        return ret;
    //    }
    //}

    //public bool isIteExpr
    //{
    //    get
    //    {
    //        if (IsRoot) return false;
    //        return Parent.Type == AstType.ITE;
    //    }
    //}


    //public (Formula f, bool negation)[][] DnfParts()
    //{
    //    if (!IsRoot) return Enumerable.Empty<(Formula f, bool negation)[]>().ToArray();
    //    var iteLeafs = Descendants.Where(f => (f.IsLeaf && (f.isIteThenExpr || f.isIteElseExpr || f.Type == AstType.TRUE)));
    //    var pathsFromEachLeafToRoot = iteLeafs.Select(leaf => leaf.BddAncestors).ToList();
    //    var ret = new (Formula f, bool negation)[pathsFromEachLeafToRoot.Count][];
    //    for (var pathNo = 0; pathNo < pathsFromEachLeafToRoot.Count; pathNo++)
    //    {
    //        var path = pathsFromEachLeafToRoot[pathNo];
    //        var listOfAnds = new (Formula f, bool negation)[path.Count];
    //        var negation = false;
    //        for (var fNo = 0; fNo < path.Count; fNo++)
    //        {
    //            var f = path[fNo];
    //            (Formula f, bool negation) ff;
    //            ff.f = f;
    //            ff.negation = negation;
    //            negation = f.isIteElseExpr; //applies to parent
    //            listOfAnds[path.Count - fNo - 1] = ff;
    //        }

    //        ret[pathNo] = listOfAnds;
    //    }

    //    return ret;
    //}

    //public string AsDnf()
    //{
    //    var src = DnfParts();

    //    string PrintVariableAndNegation((Formula f, bool negation) f)
    //    {
    //        var negationExpr = f.negation ? "!" : "";
    //        return $"{negationExpr}{f.f.Data}";
    //    }

    //    string JoinAnds((Formula f, bool negation)[] ands)
    //        => $"({String.Join(" & ", ands.Select(PrintVariableAndNegation))})";

    //    var ors = String.Join(" | ", src.Select(JoinAnds));

    //    return ors;
    //}

}