using System;
using System.Linq.Expressions;
using Antlr4.Runtime;
using BddTools.AST_Implementations.BDD;
using Parser.Ite;

namespace BddTools {
    internal class Program {
        static void Main(string[] args) {
            //TestParser();

            var x = Formula.Var("x");
            var y = Formula.Var("y");
            var z = Formula.Var("z");
            var f1 = Formula.Ite(x, Formula.True(), z);

            var a = Formula.Var("a");
            var b = Formula.Var("b");
            var c = Formula.Var("c");
            var f2 = Formula.Ite(a, b, f1);

            var f3 = Formula.Ite(c, f2, Formula.False());

            var bdd = BddFormula.Adapt(f3);

            Console.WriteLine($"{bdd}\n");

            Console.WriteLine($"{bdd.AsDnf()}\n");

        }

        private static void TestParser() {
            Dictionary<string, bool> variables = new() {
                { "x", true }, { "y", true }, { "z", false }, { "a", false }, { "b", true }, { "c", true }, { "d", false }, { "D", false },
            };

            string[] expressions = {
                "True", "FALSE", "x", "Ite(x, False, True)", "Ite(x, y, z)", "Ite(a, b, c)", "Ite(d, D, TRUE)", "Ite(x, Ite(d, D, TRUE), Ite(a, b, c))",
            };

            foreach (var expression in expressions) {
                iteForBddLexer lexer = new(new AntlrInputStream(expression));
                iteForBddParser parser = new(new CommonTokenStream(lexer));
                Object result = new EvalVisitorIte(variables).Visit(parser.parse());
                Console.WriteLine($"{expression} - {result}\n");
            }
        }
    }
}