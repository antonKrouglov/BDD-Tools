//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from D:\Projects\BDD\BDD-Tools.scratch\CS\BoolExprParserAndConverter\Grammar\SimpleBoolean.g4 by ANTLR 4.13.0

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace BddTools.Grammar.Generated {
using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.0")]
[System.CLSCompliant(false)]
public partial class SimpleBooleanParser : Parser {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		ITE=1, COMMA=2, AND=3, OR=4, NOT=5, TRUE=6, FALSE=7, LPAREN=8, RPAREN=9, 
		IDENTIFIER=10, WS=11;
	public const int
		RULE_parse = 0, RULE_expression = 1, RULE_binaryOp = 2, RULE_boolLiteral = 3;
	public static readonly string[] ruleNames = {
		"parse", "expression", "binaryOp", "boolLiteral"
	};

	private static readonly string[] _LiteralNames = {
		null, null, "','", "'&'", "'|'", "'!'", null, null, "'('", "')'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "ITE", "COMMA", "AND", "OR", "NOT", "TRUE", "FALSE", "LPAREN", "RPAREN", 
		"IDENTIFIER", "WS"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "SimpleBoolean.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static SimpleBooleanParser() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}

		public SimpleBooleanParser(ITokenStream input) : this(input, Console.Out, Console.Error) { }

		public SimpleBooleanParser(ITokenStream input, TextWriter output, TextWriter errorOutput)
		: base(input, output, errorOutput)
	{
		Interpreter = new ParserATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	public partial class ParseContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExpressionContext expression() {
			return GetRuleContext<ExpressionContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode Eof() { return GetToken(SimpleBooleanParser.Eof, 0); }
		public ParseContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_parse; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ISimpleBooleanListener typedListener = listener as ISimpleBooleanListener;
			if (typedListener != null) typedListener.EnterParse(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ISimpleBooleanListener typedListener = listener as ISimpleBooleanListener;
			if (typedListener != null) typedListener.ExitParse(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ISimpleBooleanVisitor<TResult> typedVisitor = visitor as ISimpleBooleanVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitParse(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ParseContext parse() {
		ParseContext _localctx = new ParseContext(Context, State);
		EnterRule(_localctx, 0, RULE_parse);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 8;
			expression(0);
			State = 9;
			Match(Eof);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ExpressionContext : ParserRuleContext {
		public ExpressionContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_expression; } }
	 
		public ExpressionContext() { }
		public virtual void CopyFrom(ExpressionContext context) {
			base.CopyFrom(context);
		}
	}
	public partial class VariableExprContext : ExpressionContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode IDENTIFIER() { return GetToken(SimpleBooleanParser.IDENTIFIER, 0); }
		public VariableExprContext(ExpressionContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ISimpleBooleanListener typedListener = listener as ISimpleBooleanListener;
			if (typedListener != null) typedListener.EnterVariableExpr(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ISimpleBooleanListener typedListener = listener as ISimpleBooleanListener;
			if (typedListener != null) typedListener.ExitVariableExpr(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ISimpleBooleanVisitor<TResult> typedVisitor = visitor as ISimpleBooleanVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitVariableExpr(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class NotExprContext : ExpressionContext {
		public ExpressionContext exprInsideNot;
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode NOT() { return GetToken(SimpleBooleanParser.NOT, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ExpressionContext expression() {
			return GetRuleContext<ExpressionContext>(0);
		}
		public NotExprContext(ExpressionContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ISimpleBooleanListener typedListener = listener as ISimpleBooleanListener;
			if (typedListener != null) typedListener.EnterNotExpr(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ISimpleBooleanListener typedListener = listener as ISimpleBooleanListener;
			if (typedListener != null) typedListener.ExitNotExpr(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ISimpleBooleanVisitor<TResult> typedVisitor = visitor as ISimpleBooleanVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitNotExpr(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class BinaryExprContext : ExpressionContext {
		public ExpressionContext left;
		public BinaryOpContext op;
		public ExpressionContext right;
		[System.Diagnostics.DebuggerNonUserCode] public ExpressionContext[] expression() {
			return GetRuleContexts<ExpressionContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public ExpressionContext expression(int i) {
			return GetRuleContext<ExpressionContext>(i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public BinaryOpContext binaryOp() {
			return GetRuleContext<BinaryOpContext>(0);
		}
		public BinaryExprContext(ExpressionContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ISimpleBooleanListener typedListener = listener as ISimpleBooleanListener;
			if (typedListener != null) typedListener.EnterBinaryExpr(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ISimpleBooleanListener typedListener = listener as ISimpleBooleanListener;
			if (typedListener != null) typedListener.ExitBinaryExpr(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ISimpleBooleanVisitor<TResult> typedVisitor = visitor as ISimpleBooleanVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitBinaryExpr(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class BoolLiteralExprContext : ExpressionContext {
		[System.Diagnostics.DebuggerNonUserCode] public BoolLiteralContext boolLiteral() {
			return GetRuleContext<BoolLiteralContext>(0);
		}
		public BoolLiteralExprContext(ExpressionContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ISimpleBooleanListener typedListener = listener as ISimpleBooleanListener;
			if (typedListener != null) typedListener.EnterBoolLiteralExpr(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ISimpleBooleanListener typedListener = listener as ISimpleBooleanListener;
			if (typedListener != null) typedListener.ExitBoolLiteralExpr(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ISimpleBooleanVisitor<TResult> typedVisitor = visitor as ISimpleBooleanVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitBoolLiteralExpr(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class ParenExprContext : ExpressionContext {
		public ExpressionContext exprInsideParenthesis;
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode LPAREN() { return GetToken(SimpleBooleanParser.LPAREN, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode RPAREN() { return GetToken(SimpleBooleanParser.RPAREN, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ExpressionContext expression() {
			return GetRuleContext<ExpressionContext>(0);
		}
		public ParenExprContext(ExpressionContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ISimpleBooleanListener typedListener = listener as ISimpleBooleanListener;
			if (typedListener != null) typedListener.EnterParenExpr(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ISimpleBooleanListener typedListener = listener as ISimpleBooleanListener;
			if (typedListener != null) typedListener.ExitParenExpr(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ISimpleBooleanVisitor<TResult> typedVisitor = visitor as ISimpleBooleanVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitParenExpr(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class IteExprContext : ExpressionContext {
		public IToken op;
		public ExpressionContext ifcond;
		public ExpressionContext thenexpr;
		public ExpressionContext elseexpr;
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode LPAREN() { return GetToken(SimpleBooleanParser.LPAREN, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode[] COMMA() { return GetTokens(SimpleBooleanParser.COMMA); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode COMMA(int i) {
			return GetToken(SimpleBooleanParser.COMMA, i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode RPAREN() { return GetToken(SimpleBooleanParser.RPAREN, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode ITE() { return GetToken(SimpleBooleanParser.ITE, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ExpressionContext[] expression() {
			return GetRuleContexts<ExpressionContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public ExpressionContext expression(int i) {
			return GetRuleContext<ExpressionContext>(i);
		}
		public IteExprContext(ExpressionContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ISimpleBooleanListener typedListener = listener as ISimpleBooleanListener;
			if (typedListener != null) typedListener.EnterIteExpr(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ISimpleBooleanListener typedListener = listener as ISimpleBooleanListener;
			if (typedListener != null) typedListener.ExitIteExpr(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ISimpleBooleanVisitor<TResult> typedVisitor = visitor as ISimpleBooleanVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitIteExpr(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ExpressionContext expression() {
		return expression(0);
	}

	private ExpressionContext expression(int _p) {
		ParserRuleContext _parentctx = Context;
		int _parentState = State;
		ExpressionContext _localctx = new ExpressionContext(Context, _parentState);
		ExpressionContext _prevctx = _localctx;
		int _startState = 2;
		EnterRecursionRule(_localctx, 2, RULE_expression, _p);
		try {
			int _alt;
			EnterOuterAlt(_localctx, 1);
			{
			State = 29;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case ITE:
				{
				_localctx = new IteExprContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;

				State = 12;
				((IteExprContext)_localctx).op = Match(ITE);
				State = 13;
				Match(LPAREN);
				State = 14;
				((IteExprContext)_localctx).ifcond = expression(0);
				State = 15;
				Match(COMMA);
				State = 16;
				((IteExprContext)_localctx).thenexpr = expression(0);
				State = 17;
				Match(COMMA);
				State = 18;
				((IteExprContext)_localctx).elseexpr = expression(0);
				State = 19;
				Match(RPAREN);
				}
				break;
			case LPAREN:
				{
				_localctx = new ParenExprContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;
				State = 21;
				Match(LPAREN);
				State = 22;
				((ParenExprContext)_localctx).exprInsideParenthesis = expression(0);
				State = 23;
				Match(RPAREN);
				}
				break;
			case NOT:
				{
				_localctx = new NotExprContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;
				State = 25;
				Match(NOT);
				State = 26;
				((NotExprContext)_localctx).exprInsideNot = expression(4);
				}
				break;
			case TRUE:
			case FALSE:
				{
				_localctx = new BoolLiteralExprContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;
				State = 27;
				boolLiteral();
				}
				break;
			case IDENTIFIER:
				{
				_localctx = new VariableExprContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;
				State = 28;
				Match(IDENTIFIER);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			Context.Stop = TokenStream.LT(-1);
			State = 37;
			ErrorHandler.Sync(this);
			_alt = Interpreter.AdaptivePredict(TokenStream,1,Context);
			while ( _alt!=2 && _alt!=global::Antlr4.Runtime.Atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( ParseListeners!=null )
						TriggerExitRuleEvent();
					_prevctx = _localctx;
					{
					{
					_localctx = new BinaryExprContext(new ExpressionContext(_parentctx, _parentState));
					((BinaryExprContext)_localctx).left = _prevctx;
					PushNewRecursionContext(_localctx, _startState, RULE_expression);
					State = 31;
					if (!(Precpred(Context, 3))) throw new FailedPredicateException(this, "Precpred(Context, 3)");
					State = 32;
					((BinaryExprContext)_localctx).op = binaryOp();
					State = 33;
					((BinaryExprContext)_localctx).right = expression(4);
					}
					} 
				}
				State = 39;
				ErrorHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(TokenStream,1,Context);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			UnrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	public partial class BinaryOpContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode AND() { return GetToken(SimpleBooleanParser.AND, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode OR() { return GetToken(SimpleBooleanParser.OR, 0); }
		public BinaryOpContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_binaryOp; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ISimpleBooleanListener typedListener = listener as ISimpleBooleanListener;
			if (typedListener != null) typedListener.EnterBinaryOp(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ISimpleBooleanListener typedListener = listener as ISimpleBooleanListener;
			if (typedListener != null) typedListener.ExitBinaryOp(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ISimpleBooleanVisitor<TResult> typedVisitor = visitor as ISimpleBooleanVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitBinaryOp(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public BinaryOpContext binaryOp() {
		BinaryOpContext _localctx = new BinaryOpContext(Context, State);
		EnterRule(_localctx, 4, RULE_binaryOp);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 40;
			_la = TokenStream.LA(1);
			if ( !(_la==AND || _la==OR) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class BoolLiteralContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode TRUE() { return GetToken(SimpleBooleanParser.TRUE, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode FALSE() { return GetToken(SimpleBooleanParser.FALSE, 0); }
		public BoolLiteralContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_boolLiteral; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ISimpleBooleanListener typedListener = listener as ISimpleBooleanListener;
			if (typedListener != null) typedListener.EnterBoolLiteral(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ISimpleBooleanListener typedListener = listener as ISimpleBooleanListener;
			if (typedListener != null) typedListener.ExitBoolLiteral(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ISimpleBooleanVisitor<TResult> typedVisitor = visitor as ISimpleBooleanVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitBoolLiteral(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public BoolLiteralContext boolLiteral() {
		BoolLiteralContext _localctx = new BoolLiteralContext(Context, State);
		EnterRule(_localctx, 6, RULE_boolLiteral);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 42;
			_la = TokenStream.LA(1);
			if ( !(_la==TRUE || _la==FALSE) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public override bool Sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 1: return expression_sempred((ExpressionContext)_localctx, predIndex);
		}
		return true;
	}
	private bool expression_sempred(ExpressionContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0: return Precpred(Context, 3);
		}
		return true;
	}

	private static int[] _serializedATN = {
		4,1,11,45,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,1,0,1,0,1,0,1,1,1,1,1,1,1,1,
		1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,1,30,8,1,1,1,
		1,1,1,1,1,1,5,1,36,8,1,10,1,12,1,39,9,1,1,2,1,2,1,3,1,3,1,3,0,1,2,4,0,
		2,4,6,0,2,1,0,3,4,1,0,6,7,45,0,8,1,0,0,0,2,29,1,0,0,0,4,40,1,0,0,0,6,42,
		1,0,0,0,8,9,3,2,1,0,9,10,5,0,0,1,10,1,1,0,0,0,11,12,6,1,-1,0,12,13,5,1,
		0,0,13,14,5,8,0,0,14,15,3,2,1,0,15,16,5,2,0,0,16,17,3,2,1,0,17,18,5,2,
		0,0,18,19,3,2,1,0,19,20,5,9,0,0,20,30,1,0,0,0,21,22,5,8,0,0,22,23,3,2,
		1,0,23,24,5,9,0,0,24,30,1,0,0,0,25,26,5,5,0,0,26,30,3,2,1,4,27,30,3,6,
		3,0,28,30,5,10,0,0,29,11,1,0,0,0,29,21,1,0,0,0,29,25,1,0,0,0,29,27,1,0,
		0,0,29,28,1,0,0,0,30,37,1,0,0,0,31,32,10,3,0,0,32,33,3,4,2,0,33,34,3,2,
		1,4,34,36,1,0,0,0,35,31,1,0,0,0,36,39,1,0,0,0,37,35,1,0,0,0,37,38,1,0,
		0,0,38,3,1,0,0,0,39,37,1,0,0,0,40,41,7,0,0,0,41,5,1,0,0,0,42,43,7,1,0,
		0,43,7,1,0,0,0,2,29,37
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace BddTools.Grammar.Generated
