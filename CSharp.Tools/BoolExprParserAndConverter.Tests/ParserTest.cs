using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using BddTools.AbstractSyntaxTrees;
using BddTools.Grammar.Generated;
using BddTools.Parser;
using BddTools.Variables;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BddTools.Tests {
    [TestClass]
    [TestSubject(typeof(iteForBddParser))]
    [TestSubject(typeof(ParserOfIteExpressions))]
    [TestSubject(typeof(ParserOfBooleanExpressions))]
    public class ParserTest {


        /// <summary> Test direct text parsing and evaluation </summary>
        [TestMethod]
        public void IteParser_Works() {
            Dictionary<string, bool>
                variables = new() {
                    { "x", true }
                    , { "y", true }
                    , { "z", false }
                    , { "a", false }
                    , { "b", true }
                    , { "c", true }
                    , { "d", false }
                    , { "D", false },
                };

            string[] expressions = {
                "True"
                , "FALSE"
                , "x"
                , "Ite(x, False, True)"
                , "Ite(x, y, z)"
                , "Ite(a, b, c)"
                , "Ite(d, D, TRUE)"
                , "Ite(x, Ite(d, D, TRUE), Ite(a, b, c))",
            };

            foreach (var expression in expressions) {
                iteForBddLexer lexer = new(new AntlrInputStream(expression));
                iteForBddParser parser = new(new CommonTokenStream(lexer));

                parser.RemoveErrorListeners();
                var errListener = new SyntaxErrorListener();
                parser.AddErrorListener(errListener);

                var syntaxTree = parser.parse();
                Assert.IsFalse(errListener.HasErrors(), $"{errListener}");

                var formulaEvaluator = new Evaluation_Visitor_For_iteForBdd_Grammar(variables);
                var result = formulaEvaluator.Visit(syntaxTree);
                Console.WriteLine($"{expression} - {result}\n");
            }
        }

        /// <summary> Test bad syntax </summary>
        [TestMethod]
        public void IteParser_Works2() {
            string[] badSyntaxExpressions = {
                "ITE()"
                , "(FALSE)"
                , "ITE(a)"
                , "a b"
            };

            foreach (var expression in badSyntaxExpressions) {
                iteForBddLexer lexer = new(new AntlrInputStream(expression));
                iteForBddParser parser = new(new CommonTokenStream(lexer));

                parser.RemoveErrorListeners();
                var errListener = new SyntaxErrorListener();
                parser.AddErrorListener(errListener);

                var syntaxTree = parser.parse();
                Assert.IsTrue(errListener.HasErrors());
                Console.WriteLine($"{errListener}");
            }
        }


        /// <summary> Test ParserOfIteExpressions </summary>
        [TestMethod]
        public void ParserOfIteExpressions_Works() {
            string[] expressions = {
                "True"
                , "FALSE"
                , "x"
                , "Ite(x, False, True)"
                , "Ite(x, y, z)"
                , "ITE(a, b, c)"
                , "Ite(d, D, TRUE)"
                , "Ite(x, Ite(d, D, TRUE), Ite(a, b, c))",
            };

            foreach (var expression in expressions) {
                var parser = new ParserOfIteExpressions(expression);
                Assert.IsTrue(parser.ParseITE());
                Assert.IsNotNull(parser.SyntaxErrors);
                Assert.IsNotNull(parser.SyntaxTreeIte);
                Assert.IsNotNull(parser.BddMappedFormula);
                Assert.IsTrue(parser.SyntaxErrors.Count == 0);
                Assert.IsNotNull(parser.BddMappedFormula?.Variables);
                Console.WriteLine($"{expression} => {parser.BddMappedFormula}\n");
            }
        }

        /// <summary> Test ParserOfIteExpressions with variables</summary>
        [TestMethod]
        public void ParserOfIteExpressions_Works2() {
            (string expr, string vars)[] expressions = {
                ("x", "x")
                , ("Ite(x, False, True)", "x")
                , ("Ite(x, y, z)", "x,y,z")
                , ("ITE(a, b, c)", "a,c,b") //different order
                , ("Ite(d, D, TRUE)", "d,D")
                , ("Ite(x, Ite(d, D, TRUE), Ite(a, b, c))", "x,y,z,a,b,c,d,D") //extra vars
            };


            foreach (var (expression, vars) in expressions) {
                var parser = new ParserOfIteExpressions(expression);
                var varsList = new ImmutableVarsBag(vars).List;
                Assert.IsTrue(parser.ParseITE(varsList));
                Assert.IsNotNull(parser.SyntaxErrors);
                Assert.IsNotNull(parser.SyntaxTreeIte);
                Assert.IsNotNull(parser.BddMappedFormula);
                Assert.IsTrue(parser.SyntaxErrors.Count == 0);
                Assert.IsNotNull(parser.BddMappedFormula?.Variables);

                Assert.IsTrue(parser.BddMappedFormula?.Variables.Count > 0);
                Assert.IsTrue(varsList.Count() >= parser.BddMappedFormula?.Variables.Count);

                Console.WriteLine($"{expression} => {parser.BddMappedFormula} ≡ {parser.BddMappedFormula.ToStringWithVarIndexes()}\n");
            }
        }


        /// <summary> Test ParserOfBooleanExpressions </summary>
        [TestMethod]
        public void ParserOfBooleanExpressions_Works() {
            string[] expressions = {
                "True"
                , "FALSE"
                , "x"
                , "Ite(x, False, True)"
                , "Ite(x, y, z)"
                , "ITE(a, b, c)"
                , "Ite(d, D, TRUE)"
                , "Ite(x, Ite(d, D, TRUE), Ite(a, b, c))"
                , "a | b"
                , "c & d"
                , "(c) & !d"
                , "!c & (!d)"
                , "a | b & c"
                , "c & d | e"
                , "a | (b & c)"
                , "(c & d) | e"
                , "Ite(x, !y, z|x) | y & !z"
            };

            foreach (var expression in expressions) {
                var parser = new ParserOfBooleanExpressions(expression);
                Assert.IsTrue(parser.Parse());
                Assert.IsNotNull(parser.SyntaxErrors);
                Assert.IsNotNull(parser.SyntaxTree);
                Assert.IsNotNull(parser.Formula);
                Assert.IsTrue(parser.SyntaxErrors.Count == 0);
                Assert.IsNotNull(parser.Variables);
                var expressionParsedText = parser
                    .Formula?
                    .ToStringWithVarNames(parser.Variables.IdxToNameDict, new FormulaPrintOptions(NOT: "!"));
                Console.WriteLine($"{expression} => {expressionParsedText}\n");
            }
        }


        /// <summary> Test ParserOfBooleanExpressions with variables</summary>
        [TestMethod]
        public void ParserOfBooleanExpressions_Works2() {
            (string expr, string vars)[] expressions = {
                ("x", "x")
                , ("Ite(x, False, True)", "x")
                , ("Ite(x, y, z)", "x,y,z")
                , ("ITE(a, b, c)", "a,c,b") //different order
                , ("Ite(d, D, TRUE)", "d,D")
                , ("Ite(x, Ite(d, D, TRUE), Ite(a, b, c))", "x,y,z,a,b,c,d,D") //extra vars
                , ("a | b", "a,b")
                , ("c & d", "c,d")
                , ("(c) & !d", "c,d")
                , ("!c & (!d)", "c,d")
                , ("a | b & c", "c,b,a")
                , ("c & d | e", "c,d,e")
                , ("a | (b & c)", "a,b,c")
                , ("(c & d) | e", "c,d,e")
                , ("Ite(x, !y, z|x) | y & !z", "x,y,z,a,b,c,d,e") //extra vars
            };


            foreach (var (expression, vars) in expressions) {
                var parser = new ParserOfBooleanExpressions(expression);
                var varsList = new ImmutableVarsBag(vars).List;
                Assert.IsTrue(parser.Parse(varsList));
                Assert.IsNotNull(parser.SyntaxErrors);
                Assert.IsNotNull(parser.SyntaxTree);
                Assert.IsNotNull(parser.Formula);
                Assert.IsTrue(parser.SyntaxErrors.Count == 0);
                Assert.IsNotNull(parser.Variables);

                Assert.IsTrue(parser.Variables.Count > 0);
                Assert.IsTrue(varsList.Count() >= parser.Variables.Count);
                var expressionParsedText = parser
                    .Formula?
                    .ToStringWithVarNames(parser.Variables.IdxToNameDict,new FormulaPrintOptions(NOT: "!"));
                Console.WriteLine($"{expression} => {expressionParsedText} ≡ {parser.Formula}\n");
            }
        }


    }
}