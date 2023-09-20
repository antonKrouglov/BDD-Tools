using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using static System.String;

namespace BddTools.AbstractSyntaxTrees {
    public class SyntaxError {
        public IRecognizer Recognizer { get; private set; }
        public IToken OffendingSymbol { get; private set; }
        public int Line { get; private set; }
        public int CharPositionInLine { get; private set; }
        public string Message { get; private set; }
        public RecognitionException Ex { get; private set; }

        public SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException ex) {
            this.Recognizer = recognizer;
            this.OffendingSymbol = offendingSymbol;
            this.Line = line;
            this.CharPositionInLine = charPositionInLine;
            this.Message = msg;
            this.Ex = ex;
        }

        public override string ToString() {
            List<String> stack = ((Antlr4.Runtime.Parser)Recognizer).GetRuleInvocationStack().Reverse().ToList();
            return $"line {Line}:{CharPositionInLine} at {OffendingSymbol}: rule stack: {Join("->", stack)}\n";
        }


    }
}