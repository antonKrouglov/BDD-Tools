using System;
using BddTools.AbstractSyntaxTrees;
using BddTools.Util;
using DecisionDiagrams;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using BddTools.Variables;

namespace BddTools.Tests {
    [TestClass]
    [TestSubject(typeof(Formula))]
    public class FormulaTest
    {

        /// <summary>
        /// Test that different but equivalent (same truth table) formulas create same Reduced Ordered Binary Decision Diagram
        /// </summary>
        [TestMethod]
        public void RobbdTest()
        {
            var numVars = 3;
            var numTries = 100;
            var rnd = new Random(73);
            var ddm = new DDManager<BDDNode>();

            for (var j = 0; j < numTries; j++)
            {
                var f1 = Formula.CreateRandom(rnd, numVars, 1 << numVars);
                Console.WriteLine(f1.ToString());

                var truthTable = f1.EvaluateAll(numVars);
                Console.WriteLine(truthTable.ToBinaryString());

                Formula f2;

                while (true)
                {
                    f2 = Formula.CreateRandom(rnd, numVars, 1 << numVars);
                    if (truthTable.ToBinaryString() == f2.EvaluateAll(numVars).ToBinaryString())
                    {
                        break;
                    }
                }

                Console.WriteLine(f2.ToString());

                var vars = new VarBool<BDDNode>[numVars];
                for (var i = 0; i < numVars; i++)
                {
                    vars[i] = ddm.CreateBool();
                }

                var expected = f1.Evaluate(ddm, vars, f1);
                var actual = f2.Evaluate(ddm, vars, f2);
                Console.WriteLine(ddm.Display(expected));
                Assert.AreEqual(expected, actual);
            }

            //Assert.Fail("OK");
            ////IFF(EXISTS(VAR(0), 2), VAR(2))
        }


        [TestMethod]
        public void Formula_AsDnf_Works()
        {
            var vars = new ImmutableVarsBag("x,y,z,a,b,c");
            var x = Formula.Var(vars.GetIndex("x"));
            var y = Formula.Var(vars.GetIndex("y"));
            var z = Formula.Var(vars.GetIndex("z"));
            var f1 = Formula.Ite(x, Formula.True(), z);

            var a = Formula.Var(vars.GetIndex("a"));
            var b = Formula.Var(vars.GetIndex("b"));
            var c = Formula.Var(vars.GetIndex("c"));
            var f2 = Formula.Ite(a, b, f1);

            var f3 = Formula.Ite(c, f2, Formula.False());
            var f3Str = f3.ToStringWithVarNames(vars.IdxToNameDict);
            Console.WriteLine($"{f3Str}\n");
            Assert.AreEqual(f3Str, "ITE(c, ITE(a, b, ITE(x, TRUE, z)), FALSE)");

            var f3Bdd = BddMappedFormula.Adapt(f3, vars);
            var f3Str2 = f3Bdd.ToString();
            Console.WriteLine($"{f3Str2}\n");
            Assert.AreEqual(f3Str2, "ITE(c, ITE(a, ITE(b, TRUE, FALSE), ITE(x, TRUE, ITE(z, TRUE, FALSE))), FALSE)");

            var dnf = f3Bdd.AsDnf();
            Console.WriteLine($"{dnf}\n");
            Assert.AreEqual(dnf, "(c & a & b) | (c & !a & x) | (c & !a & !x & z)");
        }


        /// <summary> BitArray increment tests </summary>
        [TestMethod]
        public void BitArrayTest()
        {
            var b = new BitArray(8);
            Assert.AreEqual(b.ToUInt64(), 0ul);

            b.Increment();
            Console.WriteLine(b.ToBinaryString());
            Assert.AreEqual(b.ToBinaryString(), "00000001");
            Assert.AreEqual(b.ToUInt64(), 1ul);

            b.Increment();
            Console.WriteLine(b.ToBinaryString());
            Assert.AreEqual(b.ToBinaryString(), "00000010");
            Assert.AreEqual(b.ToUInt64(), 2ul);

            b.Increment();
            Console.WriteLine(b.ToBinaryString());
            Assert.AreEqual(b.ToBinaryString(), "00000011");
            Assert.AreEqual(b.ToUInt64(), 3ul);

            b.Increment();
            Console.WriteLine(b.ToBinaryString());
            Assert.AreEqual(b.ToBinaryString(), "00000100");
            Assert.AreEqual(b.ToUInt64(), 4ul);

            for (var j = b.ToUInt64(); j < 255; j++)
            {
                b.Increment();
            }

            Assert.AreEqual(b.ToUInt64(), 255ul);
        }

    }
}