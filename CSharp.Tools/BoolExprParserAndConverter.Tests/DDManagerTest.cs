using System;
using System.Collections.Generic;
using BddTools.AbstractSyntaxTrees;
using BddTools.Variables;
using DecisionDiagrams;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BddTools.Tests {
    [TestClass]
    [TestSubject(typeof(DDManager<BDDNode>))]
    public class DDManagerExtensions_Test {

        #region helperMethods

        /// <summary> Get a new manager object. </summary>
        /// <param name="initialSize">The initial size.</param>
        private DDManager<BDDNode> GetManager(uint initialSize) => new(numNodes: initialSize, gcMinCutoff: (int)initialSize, printDebug: false);

        private static string idsToNames(string expr, List<string> varNames) {
            for (var j = 0; j < varNames.Count; j++) {
                expr = expr.Replace($"{j + 1}", varNames[j]);
            }

            return expr;
        }

        #endregion


        [TestMethod]
        public void DDManager_DisplayEx_works() {
            Formula x, y, z;
            var varNames = new ImmutableVarsBag($"{nameof(x)},{nameof(y)},{nameof(z)}");
            x = Formula.Var(varNames.GetIndex(nameof(x)));
            y = Formula.Var(varNames.GetIndex(nameof(y)));
            z = Formula.Var(varNames.GetIndex(nameof(z)));

            var f = Formula.Ite(Formula.Not(x), Formula.Not(y), z);

            var manager = GetManager(16);
            var numVars = varNames.Count;
            var variables = new VarBool<BDDNode>[numVars];
            for (var j = 0; j < numVars; j++) {
                variables[j] = manager.CreateBool();
            }

            var dd = f.Evaluate(manager, variables, f);

            var expr1 = manager.Display(dd);
            Console.WriteLine($"{expr1}\n");
            Assert.AreEqual(expr1, "(1 ? (3 ? true : false) : (2 ? false : true))");

            var expr2 = manager.DisplayWithVarNames(dd, varNames.IdxToNameIDict);
            Console.WriteLine($"{expr2}\n");
            Assert.AreEqual(expr2, "ITE(x, ITE(z, true, false), ITE(y, false, true))");
        }

        /// <summary>
        /// Test Reorder
        /// </summary>
        [TestMethod]
        public void TestReorder() {
            var m1 = this.GetManager(16);
            var x1 = m1.CreateBool();
            var x2 = m1.CreateBool();
            var x2_to1 = m1.CreateBool();
            var x1_to2 = m1.CreateBool();
            var f1 = m1.Ite(x1.Id(), x2.Id(), m1.True());

            var m2 = this.GetManager(16);
            var dummy1 = m2.CreateBool();
            var dummy2 = m2.CreateBool();
            var y2 = m2.CreateBool();
            var y1 = m2.CreateBool();
            var f2 = m2.Ite(y1.Id(), y2.Id(), m2.True());

            var m = m1.CreateVariableMap(new Dictionary<Variable<BDDNode>, Variable<BDDNode>> {
                { x1, x1_to2 }, { x2, x2_to1 },
            });

            var replaced = m1.Replace(f1, m);

            Assert.AreEqual(m1.Display(replaced), m2.Display(f2));
        }

        /// <summary>
        /// Test too many variables.
        /// </summary>
        [TestMethod]
        public void TestTooManyVariablesOk() {
            var manager = this.GetManager(6);

            for (int i = 0; i < 32767; i++) {
                manager.CreateBool();
            }
        }

        /// <summary>
        /// Test too many variables.
        /// </summary>
        [TestMethod]
        public void TestTooManyVariables2() {
            var manager = this.GetManager(6);

            for (int i = 0; i < short.MaxValue; i++) {
                manager.CreateBool();
                if (i % 1000 != 0) continue;
                GC.Collect();
                manager.GarbageCollect();
            }
        }

    }
}