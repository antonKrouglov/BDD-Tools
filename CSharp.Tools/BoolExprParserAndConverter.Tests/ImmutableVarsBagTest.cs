using System;
using System.Collections.Generic;
using System.Linq;
using BddTools.Variables;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BddTools.Tests {
    [TestClass]
    [TestSubject(typeof(ImmutableVarsBag))]
    public class ImmutableVarsBagTest {
        [TestMethod]
        public void TestCreate() {
            ImmutableVarsBag immutableVarsBag;

            immutableVarsBag = new ImmutableVarsBag(Enumerable.Empty<string>().ToArray());
            Assert.IsNotNull(immutableVarsBag);
            Assert.AreEqual(immutableVarsBag.Count, 0);

            immutableVarsBag = new ImmutableVarsBag(Enumerable.Empty<VarInfo>());
            Assert.IsNotNull(immutableVarsBag);
            Assert.AreEqual(immutableVarsBag.Count, 0);

            immutableVarsBag = new ImmutableVarsBag("");
            Assert.IsNotNull(immutableVarsBag);
            Assert.AreEqual(immutableVarsBag.Count, 0);
        }

        [TestMethod]
        public void TestCreate2() {
            ImmutableVarsBag immutableVarsBag;

            immutableVarsBag = new ImmutableVarsBag(new[] { "one" });
            Assert.IsNotNull(immutableVarsBag);
            Assert.AreEqual(immutableVarsBag.Count, 1);

            immutableVarsBag = new ImmutableVarsBag(new[] { new VarInfo("two", 1) });
            Assert.IsNotNull(immutableVarsBag);
            Assert.AreEqual(immutableVarsBag.Count, 1);

            immutableVarsBag = new ImmutableVarsBag("three");
            Assert.IsNotNull(immutableVarsBag);
            Assert.AreEqual(immutableVarsBag.Count, 1);

            immutableVarsBag = new ImmutableVarsBag(immutableVarsBag);
            Assert.IsNotNull(immutableVarsBag);
            Assert.AreEqual(immutableVarsBag.Count, 1);
        }

        [TestMethod]
        public void TestCounts() {
            var vb1 = new ImmutableVarsBag(new[] { "one", "two" });
            Assert.IsNotNull(vb1);

            var vb2 = new ImmutableVarsBag(new[] { new VarInfo("two", 2), new VarInfo("one", 1) });
            Assert.IsNotNull(vb2);

            var vb3 = new ImmutableVarsBag("one , two ,");
            Assert.IsNotNull(vb3);

            var vb4 = new ImmutableVarsBag(vb3);
            Assert.IsNotNull(vb4);

            var vb5 = new ImmutableVarsBag(vb4.SortedList.Reverse().ToArray(), new[] { 2, 1 });
            Assert.IsNotNull(vb5);

            Assert.AreEqual($"{vb1}", "one,two");
            Assert.AreEqual($"{vb1}", $"{vb2}");
            Assert.AreEqual($"{vb2}", $"{vb3}");
            Assert.AreEqual($"{vb3}", $"{vb4}");
            Assert.AreEqual($"{vb4}", $"{vb5}");

            Assert.AreEqual($"{vb1.GetName(0)}", "one");
            Assert.AreEqual($"{vb1.GetName(1)}", "two");

            Assert.AreEqual(vb1.GetIndex("one"), 0);
            Assert.AreEqual(vb1.GetIndex("two"), 1);

            Assert.AreEqual(vb1.Get(0).Name, "one");
            Assert.AreEqual(vb1.Get(1).Name, "two");

            Assert.AreEqual(vb1.Get("one").Index, 0);
            Assert.AreEqual(vb1.Get("two").Index, 1);

            Assert.IsTrue(vb1.Contains("one"));
            Assert.IsTrue(vb1.Contains("two"));
            Assert.IsTrue(vb1.Contains(0));
            Assert.IsTrue(vb1.Contains(1));
            Assert.IsFalse(vb1.Contains(99));
            Assert.IsFalse(vb1.Contains(""));
            Assert.IsFalse(vb1.Contains(null));
        }

        [TestMethod]
        public void TestBigCounts() {
            var lst = new List<string>();
            var idx = new List<int>();
            string vr = "x" + Guid.NewGuid().ToString().Substring(0, 8);
            for (int i = 1; i <= 10000; i++) {
                vr = "x" + Guid.NewGuid().ToString().Substring(0, 8);
                lst.Add(vr);
                idx.Add(i);
            }

            var vb1 = new ImmutableVarsBag(lst.ToArray());

            lst.Reverse();
            idx.Reverse();
            var vb2 = new ImmutableVarsBag(lst, idx);
            Assert.IsTrue(vb1.Contains(vr));
            Assert.AreEqual($"{vb1}", $"{vb2}");
        }
    }
}