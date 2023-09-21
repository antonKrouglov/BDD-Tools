namespace BddTools.BddReorder {
    public interface IReorderer {
        ReorderResult Result { get; }
        ReorderParams Params { get; }

        /// <summary> Slow brute force reordering </summary>
        void Process();

        /// <summary> Slow brute force reordering </summary>
        ReorderResult Reorder(ReorderParams data);
    }
}