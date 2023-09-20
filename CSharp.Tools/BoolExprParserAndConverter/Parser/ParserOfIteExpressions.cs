using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using BddTools.AbstractSyntaxTrees;
using BddTools.Grammar.Generated;
using BddTools.Variables;

namespace BddTools.Parser {

    public class ParserOfIteExpressions {

        #region input

        public string ExpressionText { get; private set; }

        #endregion


        #region output

        public List<SyntaxError> SyntaxErrors { get; private set; } = new();

        public iteForBddParser.ParseContext? SyntaxTreeIte { get; private set; }

        public BddMappedFormula? BddMappedFormula { get; private set; }

        #endregion


        #region constructors

        public ParserOfIteExpressions(string expressionText)
            => ExpressionText = expressionText ?? throw new ArgumentNullException(nameof(expressionText));

        #endregion


        /// <summary>
        /// Parse "ITE(if, then, else)" representation of Boolean Decision Diagrams.
        /// Boolean literals are True, False. Literals and function names are NOT case sensitive. </summary>
        /// <returns>True if parsing was successful. False if not. SyntaxErrors list is filled with errors if any.
        /// Successful parsing fills SyntaxTreeIte and BddMappedFormula properties. </returns>
        // ReSharper disable once InconsistentNaming
        public Boolean ParseITE() => ParseITE(predefinedVars: null);


        /// <summary>
        /// Parse "ITE(if, then, else)" representation of Boolean Decision Diagrams.
        /// Boolean literals are True, False. Literals and function names are NOT case sensitive. </summary>
        /// <param name="predefinedVars">Variable names and their order. Null if order is determined from source expression.</param>
        /// <returns>True if parsing was successful. False if not. SyntaxErrors list is filled with errors if any.
        /// Successful parsing fills SyntaxTreeIte and  BddMappedFormula properties. </returns>
        // ReSharper disable once InconsistentNaming
        public Boolean ParseITE(IEnumerable<VarInfo>? predefinedVars) {
            iteForBddLexer lexer = new(new AntlrInputStream(ExpressionText));
            iteForBddParser parser = new(new CommonTokenStream(lexer));

            parser.RemoveErrorListeners();
            var errListener = new SyntaxErrorListener();
            parser.AddErrorListener(errListener);

            SyntaxTreeIte = parser.parse();

            if (errListener.HasErrors()) {
                SyntaxErrors = errListener.SyntaxErrors.ToList();
                BddMappedFormula = null;
                return false;
            }

            SyntaxErrors = new();

            var formulaBuilder = new ST_To_AST_Visitor_For_iteForBdd_Grammar(predefinedVars);
            var formula = formulaBuilder.Visit(SyntaxTreeIte);
            BddMappedFormula = BddMappedFormula.Adapt(formula, formulaBuilder.VarsSort);

            return true;
        }

        // ReSharper disable once InconsistentNaming
        public bool ParseITE(IReadOnlyDictionary<int, string> predefinedVarsDict) {
            if (predefinedVarsDict == null) throw new ArgumentNullException(nameof(predefinedVarsDict));
            return ParseITE(predefinedVarsDict.Select(kv => new VarInfo(kv)));
        }


    }
    }