using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using BddTools.AbstractSyntaxTrees;
using BddTools.Grammar.Generated;
using BddTools.Variables;

namespace BddTools.Parser {

    public class ParserOfBooleanExpressions {

        #region input

        public string ExpressionText { get; private set; }

        #endregion


        #region output

        public List<SyntaxError> SyntaxErrors { get; private set; } = new();

        public SimpleBooleanParser.ParseContext? SyntaxTree { get; private set; }

        public Formula? Formula { get; private set; }

        public ImmutableVarsBag? Variables { get; private set; }

        #endregion


        #region constructors

        public ParserOfBooleanExpressions(string expressionText)
            => ExpressionText = expressionText ?? throw new ArgumentNullException(nameof(expressionText));

        #endregion


        /// <summary>
        /// Parse Simple Boolean expression:
        ///     (x | y) & !z
        ///     ITE (if, then, else) is supported as well.
        ///     Boolean literals are True, False.
        ///     Literals and function names are NOT case sensitive.
        ///     AND, OR binary operators have EQUAL priority.
        /// </summary>
        /// <returns>True if parsing was successful. False if not. SyntaxErrors list is filled with errors if any.
        /// Successful parsing fills SyntaxTree, Formula and Variables properties. </returns>
        // ReSharper disable once InconsistentNaming
        public bool Parse() => Parse(null);


        /// <summary>
        /// Parse Simple Boolean expression:
        ///     (x | y) & !z
        ///     ITE (if, then, else) is supported as well.
        ///     Boolean literals are True, False.
        ///     Literals and function names are NOT case sensitive.
        ///     AND, OR binary operators have EQUAL priority.
        /// </summary>
        /// <param name="predefinedVars">Variable names and their order. Null if order is determined from source expression.</param>
        /// <returns>True if parsing was successful. False if not. SyntaxErrors list is filled with errors if any.
        /// Successful parsing fills SyntaxTree, Formula and Variables properties. </returns>
        // ReSharper disable once InconsistentNaming
        public bool Parse(IEnumerable<VarInfo>? predefinedVars) {
            SimpleBooleanLexer lexer = new(new AntlrInputStream(ExpressionText));
            SimpleBooleanParser parser = new(new CommonTokenStream(lexer));

            parser.RemoveErrorListeners();
            var errListener = new SyntaxErrorListener();
            parser.AddErrorListener(errListener);

            SyntaxTree = parser.parse();

            if (errListener.HasErrors()) {
                SyntaxErrors = errListener.SyntaxErrors.ToList();
                Formula = null;
                Variables = null;
                return false;
            }

            SyntaxErrors = new List<SyntaxError>();

            var formulaBuilder = new ST_To_AST_Visitor_For_SimpleBoolean_Grammar(predefinedVars);
            Formula = formulaBuilder.Visit(SyntaxTree);
            Variables = formulaBuilder.VarsSort;

            return true;
        }

    }
}