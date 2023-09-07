//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from D:\Projects\BDD\Antlr\BddTools\Grammar\iteForBdd.g4 by ANTLR 4.13.0

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace Parser.Ite {
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="iteForBddParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.0")]
[System.CLSCompliant(false)]
public interface IiteForBddVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="iteForBddParser.parse"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParse([NotNull] iteForBddParser.ParseContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>iteExpr</c>
	/// labeled alternative in <see cref="iteForBddParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIteExpr([NotNull] iteForBddParser.IteExprContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>boolLiteralExpr</c>
	/// labeled alternative in <see cref="iteForBddParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBoolLiteralExpr([NotNull] iteForBddParser.BoolLiteralExprContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>variableExpr</c>
	/// labeled alternative in <see cref="iteForBddParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariableExpr([NotNull] iteForBddParser.VariableExprContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="iteForBddParser.boolLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBoolLiteral([NotNull] iteForBddParser.BoolLiteralContext context);
}
} // namespace Parser.Ite