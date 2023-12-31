﻿using System;
using System.Linq;
using System.Threading.Tasks;
using BddTools.Util;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BddTools.Tests {
    [TestClass]
    [TestSubject(typeof(HelperExtensions))]
    public class HelperExtensionsTest {

        [TestMethod]
        public void PermutationsTest() {
            var lst1 = new[] { "a", "b" };
            var perms = lst1.GetPermutationsSafe();
            var permsText = string.Join("", perms.Select(lst => string.Join("", lst)));
            Assert.AreEqual("ab ba".Replace(" ", ""), permsText);

            lst1 = new[] { "a", "b", "c" };
            perms = lst1.GetPermutationsSafe();
            permsText = string.Join("", perms.Select(lst => string.Join("", lst)));
            Assert.AreEqual("abc acb bac bca cab cba".Replace(" ", ""), permsText);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PermutationsTest2() {
            var lst1 = new[] { "a", "b" };
            lst1 = null;
            var perms = lst1.GetPermutationsSafe();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PermutationsTest3() {
            var lst1 = new[] { "a", "a" };
            var perms = lst1.GetPermutationsSafe();
        }


        [TestMethod]
        public void PermutationsFastTest() {
            var lst1 = new[] { "a", "b" };
            var perms = lst1.GetPermutationsFast().Select(v => v.CloneArray());
            var permsText = string.Join("", perms.Select(lst => string.Join("", lst)));
            Assert.AreEqual("ab ba".Replace(" ", ""), permsText);

            var lst2 = new[] { 1, 2, 3 };
            var perms2 = lst2.GetPermutationsFast().Select(v => v.CloneArray()).ToList();
            var permsText2 = string.Join(" ", perms2.Select(lst => "{" + $"{string.Join(',', lst)}" + "}"));
            Assert.AreEqual("{1,2,3} {1,3,2} {2,1,3} {2,3,1} {3,2,1} {3,1,2}", permsText2);

            var lst3 = new[] { 1, 2, 3, 4 };
            var perms3 = lst3.GetPermutationsFast().Select(v => v.CloneArray()).ToList();
            var permsText3 = string.Join(" ", perms3.Select(lst => "{" + $"{string.Join(',', lst)}" + "}"));
            Assert.AreEqual(
                "{1,2,3,4} {1,2,4,3} {1,3,2,4} {1,3,4,2} {1,4,3,2} {1,4,2,3} {2,1,3,4} {2,1,4,3} {2,3,1,4} {2,3,4,1} {2,4,3,1} {2,4,1,3} {3,2,1,4} {3,2,4,1} {3,1,2,4} {3,1,4,2} {3,4,1,2} {3,4,2,1} {4,2,3,1} {4,2,1,3} {4,3,2,1} {4,3,1,2} {4,1,3,2} {4,1,2,3}"
                , permsText3);

            var lst4 = new[] { 1, 2, 1 };
            var perms4 = lst4.GetPermutationsFast().Select(v => v.CloneArray()).ToList();
            var permsText4 = string.Join(" ", perms4.Select(lst => "{" + $"{string.Join(',', lst)}" + "}"));
            Assert.AreEqual("{1,2,1} {1,1,2} {2,1,1} {2,1,1} {1,2,1} {1,1,2}", permsText4);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PermutationsFastTest2() {
            var lst1 = new[] { "a", "b" };
            lst1 = null;
            var perms = lst1.GetPermutationsFast();
        }
#if NET6_0_OR_GREATER //Chunk requires NET Core 6


        [TestMethod]
        public void TestChunks() {
            var cnt = (int)6ul.Factorial();
            var chunkSize = (int)Math.Ceiling(cnt / 8.0);
            var lst1 = Enumerable.Range(0, cnt);
            Parallel.ForEach(lst1.Chunk(chunkSize), (chunk) => {
                //    Console.WriteLine(chunk.Count());
                Console.WriteLine(string.Join(",", chunk.Reverse()));
            });
        }


        [TestMethod]
        public void TestChunks2() {
            var varCount = 9;
            var varOrder = Enumerable.Range(0, varCount).ToList();
            var lst = varOrder.GetPermutationsSafe();
            var cnt = (int)((ulong)varCount).Factorial();
            var chunkSize = (int)Math.Ceiling(cnt / 8.0);
            Parallel.ForEach(lst.Chunk(chunkSize), (chunk) => {
                //    Console.WriteLine(chunk.Count());
                Console.WriteLine(string.Join(",", chunk.Select(pm => string.Join("", pm.Select(n => (int)Math.Pow(n, 1.1))))));
            });
        }
#endif //NET6_0_OR_GREATER

    }
}