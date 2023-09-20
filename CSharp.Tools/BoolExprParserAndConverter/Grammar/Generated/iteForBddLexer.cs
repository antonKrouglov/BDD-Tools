//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from D:\Projects\BDD\BDD-Tools.scratch\CS\BoolExprParserAndConverter\Grammar\iteForBdd.g4 by ANTLR 4.13.0

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
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.0")]
[System.CLSCompliant(false)]
public partial class iteForBddLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		ITE=1, COMMA=2, TRUE=3, FALSE=4, LPAREN=5, RPAREN=6, IDENTIFIER=7, WS=8;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"ITE", "COMMA", "TRUE", "FALSE", "LPAREN", "RPAREN", "IDENTIFIER", "WS"
	};


	public iteForBddLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public iteForBddLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, null, "','", null, null, "'('", "')'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "ITE", "COMMA", "TRUE", "FALSE", "LPAREN", "RPAREN", "IDENTIFIER", 
		"WS"
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

	public override string GrammarFileName { get { return "iteForBdd.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static iteForBddLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,8,52,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,6,
		2,7,7,7,1,0,1,0,1,0,1,0,1,1,1,1,1,2,1,2,1,2,1,2,1,2,1,3,1,3,1,3,1,3,1,
		3,1,3,1,4,1,4,1,5,1,5,1,6,1,6,5,6,41,8,6,10,6,12,6,44,9,6,1,7,4,7,47,8,
		7,11,7,12,7,48,1,7,1,7,0,0,8,1,1,3,2,5,3,7,4,9,5,11,6,13,7,15,8,1,0,12,
		2,0,73,73,105,105,2,0,84,84,116,116,2,0,69,69,101,101,2,0,82,82,114,114,
		2,0,85,85,117,117,2,0,70,70,102,102,2,0,65,65,97,97,2,0,76,76,108,108,
		2,0,83,83,115,115,3,0,65,90,95,95,97,122,4,0,48,57,65,90,95,95,97,122,
		3,0,9,10,12,13,32,32,53,0,1,1,0,0,0,0,3,1,0,0,0,0,5,1,0,0,0,0,7,1,0,0,
		0,0,9,1,0,0,0,0,11,1,0,0,0,0,13,1,0,0,0,0,15,1,0,0,0,1,17,1,0,0,0,3,21,
		1,0,0,0,5,23,1,0,0,0,7,28,1,0,0,0,9,34,1,0,0,0,11,36,1,0,0,0,13,38,1,0,
		0,0,15,46,1,0,0,0,17,18,7,0,0,0,18,19,7,1,0,0,19,20,7,2,0,0,20,2,1,0,0,
		0,21,22,5,44,0,0,22,4,1,0,0,0,23,24,7,1,0,0,24,25,7,3,0,0,25,26,7,4,0,
		0,26,27,7,2,0,0,27,6,1,0,0,0,28,29,7,5,0,0,29,30,7,6,0,0,30,31,7,7,0,0,
		31,32,7,8,0,0,32,33,7,2,0,0,33,8,1,0,0,0,34,35,5,40,0,0,35,10,1,0,0,0,
		36,37,5,41,0,0,37,12,1,0,0,0,38,42,7,9,0,0,39,41,7,10,0,0,40,39,1,0,0,
		0,41,44,1,0,0,0,42,40,1,0,0,0,42,43,1,0,0,0,43,14,1,0,0,0,44,42,1,0,0,
		0,45,47,7,11,0,0,46,45,1,0,0,0,47,48,1,0,0,0,48,46,1,0,0,0,48,49,1,0,0,
		0,49,50,1,0,0,0,50,51,6,7,0,0,51,16,1,0,0,0,3,0,42,48,1,6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace BddTools.Grammar.Generated
