using Parser.Ite;

namespace BddTools {

    // adapted from https://stackoverflow.com/a/72423036/2746150
    public class EvalVisitorIte : iteForBddBaseVisitor<bool> {
        private readonly Dictionary<string, bool> _variables;


        public EvalVisitorIte(Dictionary<string, bool> variables) {
            _variables = variables;
        }

        public override bool VisitParse(iteForBddParser.ParseContext context) {
            return base.Visit(context.expression());
        }

        public override bool VisitIteExpr(iteForBddParser.IteExprContext context) {
            return Visit(_variables[context.ifcond.Text] ? context.thenexpr : context.elseexpr);
        }

        public override bool VisitBoolLiteralExpr(iteForBddParser.BoolLiteralExprContext context) {
            return bool.Parse(context.GetText().ToLower());
        }

        public override bool VisitVariableExpr(iteForBddParser.VariableExprContext context) {
            return _variables[context.IDENTIFIER().GetText()];
        }

    }
}