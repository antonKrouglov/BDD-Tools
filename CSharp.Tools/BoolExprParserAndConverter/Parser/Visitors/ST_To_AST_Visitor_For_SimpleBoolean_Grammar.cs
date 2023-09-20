using System;
using System.Collections.Generic;
using BddTools.AbstractSyntaxTrees;
using BddTools.Grammar.Generated;
using BddTools.Variables;
using static BddTools.Grammar.Generated.SimpleBooleanParser;
namespace BddTools.Parser
{

    /// <summary>
    /// Implementation of Visitor pattern for:
    ///     antlr-generated Syntax Tree (ST) for boolean formula
    ///     Sample formula: (x | y) & !z
    ///         Complete grammar definition - see SimpleBoolean.g4
    /// This visitor builds Abstract Syntax Tree (AST); AST implemented as Formula class.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class ST_To_AST_Visitor_For_SimpleBoolean_Grammar : SimpleBooleanBaseVisitor<Formula>
    {

        #region private fields

        private VarsBuilder varsBuilder;

        #endregion


        #region public fields

        /// <summary> Get parsed variables name->order/index if needed; </summary>
        public ImmutableVarsBag VarsSort { get; private set; } = null!;

        /// <summary> Get variable tags after process completed </summary>
        public Dictionary<string, object?> VarsTags { get; private set; } = null!;

        /// <summary> Set predefined variables name->order/index if needed. This list is reset after each run </summary>
        public IEnumerable<VarInfo>? PredefinedVars;

        #endregion


        #region constructors

        public ST_To_AST_Visitor_For_SimpleBoolean_Grammar() : this(null)
        {
        }

        public ST_To_AST_Visitor_For_SimpleBoolean_Grammar(IEnumerable<VarInfo>? predefinedVars)
        {
            PredefinedVars = predefinedVars;
            varsBuilder = new VarsBuilder();
        }

        #endregion


        /// <summary> Processing </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Formula VisitParse(ParseContext context)
        {
            //init
            var vars = PredefinedVars;
            var filteringRequiredAfterBuild = false;
            if (vars == null)
            {
                vars = new List<VarInfo>();
                filteringRequiredAfterBuild = true;
            }
            varsBuilder = new VarsBuilder(vars);
            PredefinedVars = null; //PredefinedVars is single param

            //process
            var resultFormula = base.Visit(context.expression());
            resultFormula.BuildCaches();

            //build results
            VarsSort = filteringRequiredAfterBuild 
                ? varsBuilder.Build(resultFormula.VarIndexes())
                : varsBuilder.Build();

            VarsTags = varsBuilder.Tags;

            return resultFormula;
        }

        #region overrides for visiting every node type

        public override Formula VisitBoolLiteralExpr(BoolLiteralExprContext context)
            => bool.Parse(context.GetText().ToLower())
                ? Formula.True()
                : Formula.False();

        public override Formula VisitVariableExpr(VariableExprContext context)
        {
            var varIndex = varsBuilder.AddNewOrGetExisting(context.IDENTIFIER().GetText());
            return Formula.Var(varIndex);
        }

        public override Formula VisitIteExpr(IteExprContext context)
            => Formula.Ite(Visit(context.ifcond), Visit(context.thenexpr), Visit(context.elseexpr));

        public override Formula VisitNotExpr(NotExprContext context)
            => Formula.Not(Visit(context.exprInsideNot));

        public override Formula VisitBinaryExpr(BinaryExprContext context)
        {
            if (context.op.AND() != null)
            {
                return Formula.And(Visit(context.left), Visit(context.right));
            }

            if (context.op.OR() != null)
            {
                return Formula.Or(Visit(context.left), Visit(context.right));
            }

            throw new ApplicationException($"not implemented: binary operator '{context.op.GetText()}'");
        }

        public override Formula VisitParenExpr(ParenExprContext context)
            => Visit(context.exprInsideParenthesis);

        #endregion
    }
}