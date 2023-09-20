using System.Collections.Generic;
using BddTools.Grammar.Generated;

namespace BddTools.Parser
{

    /// <summary>
    /// Implementation of Visitor pattern for:
    ///     antlr-generated Syntax Tree for If-then-else formula
    ///     Sample formula: Ite(condition, then, ite(condition2, true, false))
    /// Grammar definition - see iteForBdd.g4
    /// This visitor evaluates formula with concrete variable values
    /// adapted from https://stackoverflow.com/a/72423036/2746150
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class Evaluation_Visitor_For_iteForBdd_Grammar : iteForBddBaseVisitor<bool>
    {
        private readonly Dictionary<string, bool> variables;


        public Evaluation_Visitor_For_iteForBdd_Grammar(Dictionary<string, bool> variables)
            => this.variables = variables;

        public override bool VisitParse(iteForBddParser.ParseContext context)
            => base.Visit(context.expression());

        public override bool VisitIteExpr(iteForBddParser.IteExprContext context)
            => Visit(variables[context.ifcond.GetText()] ? context.thenexpr : context.elseexpr);

        public override bool VisitBoolLiteralExpr(iteForBddParser.BoolLiteralExprContext context)
            => bool.Parse(context.GetText().ToLower());

        public override bool VisitVariableExpr(iteForBddParser.VariableExprContext context)
            => variables[context.IDENTIFIER().GetText()];

    }
}