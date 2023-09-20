using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using BddTools.AbstractSyntaxTrees;

namespace BddTools.Util {
    public static class HelperExtensions {

        #region IEnumerable extensions

        /// <summary> Wraps this object instance into an IEnumerable&lt;T&gt; consisting of a single item. </summary>
        /// <typeparam name="T"> Type of the object. </typeparam>
        /// <param name="item"> The instance that will be wrapped. </param>
        /// <returns> An IEnumerable&lt;T&gt; consisting of a single item. </returns>
        /// <remarks>  https://stackoverflow.com/q/1577822/2746150 </remarks>>
        public static IEnumerable<T> Yield<T>(this T item) {
            yield return item;
        }


        /// <summary> Check list for duplicates </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="list"></param>
        /// <param name="keySelector"></param>
        /// <returns>true if any duplicates found</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <remarks> https://stackoverflow.com/a/27233955/2746150 </remarks>>
        public static bool HaveDuplicates<TItem, TKey>(this IEnumerable<TItem> list, Func<TItem, TKey> keySelector)
            => (list ?? throw new ArgumentNullException(nameof(list)))
                .GroupBy(keySelector ?? throw new ArgumentNullException(nameof(keySelector)))
                .Any(g => g.Count() > 1);


        /// <summary> Check list for duplicates </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <remarks> https://stackoverflow.com/a/27233955/2746150 </remarks>>
        public static bool HaveDuplicates<T>(this IEnumerable<T> list, IEqualityComparer<T>? comparer = null)
            => (list ?? throw new ArgumentNullException(nameof(list)))
                .GroupBy(x => x, comparer ?? EqualityComparer<T>.Default)
                .Any(g => g.Count() > 1);


        /// <summary> inner permutation method </summary>
        private static IEnumerable<IEnumerable<T>> GetPermutationsSafe<T>(IEnumerable<T> list, int length) {
            if (length == 1) return list.Select(t => t.Yield());

            // ReSharper disable PossibleMultipleEnumeration
            return
                GetPermutationsSafe(list, length - 1)
                    .SelectMany(
                        t => list.Where(e => !t.Contains(e))
                        , (t1, t2) => t1.Concat(t2.Yield())
                    );
            // ReSharper restore PossibleMultipleEnumeration
        }


        /// <summary> Return permutations of a list; That is given {1,2,3) it will produce {1,2,3} {1,3,2} {2,1,3} {2,3,1} {3,1,2} {3,2,1} </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">list without duplicates</param>
        /// <returns>permutations</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <remarks> This implementation is not fast or memory effective as each permutation creates new collection;
        /// Still it is safe to store result IEnumerable into to list
        /// see https://stackoverflow.com/a/10630026/2746150 </remarks>>
        public static IEnumerable<IEnumerable<T>> GetPermutationsSafe<T>(this IList<T> list) {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (list.HaveDuplicates()) throw new ArgumentException($"duplicates found in {nameof(list)}");
            return GetPermutationsSafe(list, list.Count);
        }


        /// <summary>
        /// Return permutations of an array; That is given {1,2,3) it will produce {1,2,3} {1,3,2} {2,1,3} {2,3,1} {3,2,1} {3,1,2}
        ///     { 1, 2, 3, 4 } will produce {1,2,3,4} {1,2,4,3} {1,3,2,4} {1,3,4,2} {1,4,3,2} {1,4,2,3} {2,1,3,4} {2,1,4,3} {2,3,1,4} {2,3,4,1}
        ///         {2,4,3,1} {2,4,1,3} {3,2,1,4} {3,2,4,1} {3,1,2,4} {3,1,4,2} {3,4,1,2} {3,4,2,1} {4,2,3,1} {4,2,1,3} {4,3,2,1} {4,3,1,2} {4,1,3,2} {4,1,2,3}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">Array of values; duplicates are allowed </param>
        /// <returns>permutations</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <remarks> This implementation is fast but cannot be stored into a List;
        /// if you try to do something like GetPermutationsFast(vals).ToArray() then you end up with N references to the same array.
        /// If you want to be able to store the results you have to manually create a copy. E.g. GetPermutationsFast(values).Select(v =&gt; (T[])v.Clone())
        /// see https://stackoverflow.com/a/13022090/2746150 </remarks>>
        public static IEnumerable<T[]> GetPermutationsFast<T>(this T[] values) {
            if (values == null) throw new ArgumentNullException(nameof(values));
            return privGetPermutationsFast(values, 0);
        }

        /// <summary> inner permutation method </summary>
        private static IEnumerable<T[]> privGetPermutationsFast<T>(T[] values, int fromInd) {
            if (fromInd + 1 == values.Length)
                yield return values;
            else {
                foreach (var v in privGetPermutationsFast(values, fromInd + 1))
                    yield return v;

                for (var i = fromInd + 1; i < values.Length; i++) {
                    if (fromInd != i) {
                        //swap values
                        (values[fromInd], values[i]) = (values[i], values[fromInd]);
                    }

                    foreach (var v in privGetPermutationsFast(values, fromInd + 1))
                        yield return v;
                    if (fromInd != i) {
                        //swap values
                        (values[fromInd], values[i]) = (values[i], values[fromInd]);
                    }
                }
            }
        }

        //public static IEnumerable<T[]> Permutations<T>(T[] values, int fromInd = 0) {
        //    if (fromInd + 1 == values.Length)
        //        yield return values;
        //    else {
        //        foreach (var v in Permutations(values, fromInd + 1))
        //            yield return v;

        //        for (var i = fromInd + 1; i < values.Length; i++) {
        //            SwapValues(values, fromInd, i);
        //            foreach (var v in Permutations(values, fromInd + 1))
        //                yield return v;
        //            SwapValues(values, fromInd, i);
        //        }
        //    }
        //}

        //private static void SwapValues<T>(T[] values, int pos1, int pos2) {
        //    if (pos1 != pos2) {
        //        T tmp = values[pos1];
        //        values[pos1] = values[pos2];
        //        values[pos2] = tmp;
        //    }
        //}


        public static T[] CloneArray<T>(this T[] arr)
            => (T[])arr.Clone();

        public static T[] Reorder<T>(this T[] arr, int[] newOrder) {
            if (arr == null) throw new ArgumentNullException(nameof(arr));
            if (newOrder == null) throw new ArgumentNullException(nameof(newOrder));
            if (arr.Count() != newOrder.Count()) throw new ArgumentException($"Unmatched counts {arr.Count()} {newOrder.Count()}");

            var reordered = arr.Zip(newOrder, 
                (elem, replacementIndex) 
                    => arr[replacementIndex]).ToArray();

            return reordered;
        }

        #endregion


        #region bitArray extensions

        /// <summary> Treat bitArray as a big number; Increment that number </summary>
        /// <param name="bitArray"></param>
        /// <returns>Incremented bitArray</returns>
        public static BitArray Increment(this BitArray bitArray) {
            for (var i = 0; i < 32 && !(bitArray[i] = !bitArray[i++]);) {
            }

            return bitArray;
        }


        /// <summary> Treat bitArray as a big number; Return that number in binary format </summary>
        public static string ToBinaryString(this BitArray bitArray)
            => string
                .Join("", bitArray
                    .OfType<bool>()
                    .Reverse()
                    .Select(b => b ? "1" : "0"));


        public static BigInteger ToBigInteger(this BitArray bitArray) {
            BigInteger res = 0;
            foreach (var c in bitArray.ToBinaryString()) {
                res <<= 1;
                res += c == '1' ? 1 : 0;
            }

            return res;
        }


        public static ulong ToUInt64(this BitArray bitArray)
            => Convert.ToUInt64(bitArray.ToBinaryString(), 2);


        public static uint ToUInt32(this BitArray bitArray)
            => Convert.ToUInt32(bitArray.ToBinaryString(), 2);


        public static ushort ToUInt16(this BitArray bitArray)
            => Convert.ToUInt16(bitArray.ToBinaryString(), 2);


        public static byte ToUInt8(this BitArray bitArray)
            => Convert.ToByte(bitArray.ToBinaryString(), 2);


        /// <summary> Converts to [index]=bitValue dictionary </summary>
        /// <param name="bitArray"></param>
        /// <returns>ImmutableDictionary&lt;int, bool&gt;</returns>
        public static ImmutableDictionary<int, bool> ToPositionBitDict(this BitArray bitArray)
            => bitArray
                .OfType<bool>()
                .Select((elem, index) => new KeyValuePair<int, bool>(index, elem))
                .ToImmutableDictionary();

        #endregion

    }
}