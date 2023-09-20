using Antlr4.Runtime;
using System.Collections.Generic;
using System.IO;


namespace BddTools.AbstractSyntaxTrees {
    /// <summary>
    /// https://stackoverflow.com/a/51385877/2746150
    /// </summary>
    public class SyntaxErrorListener : BaseErrorListener {
        public List<SyntaxError> SyntaxErrors { get; private set; } = new();

        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e) 
            => SyntaxErrors.Add(new SyntaxError(recognizer, offendingSymbol, line, charPositionInLine, msg, e));

        public override string ToString() 
            => $"Errors:\n{string.Join("\n", SyntaxErrors)}";

        public bool HasErrors() =>
            SyntaxErrors.Count > 0;
        
    }


    ///// <summary>
    ///// Error listener recording all errors that Antlr parser raises during parsing.
    ///// </summary>
    //internal class ErrorListener : BaseErrorListener
    //{
    //    private const string Eof = "the end of formula";

    //    public ErrorListener()
    //    {
    //        ErrorMessages = new List<ErrorInfo>();
    //    }

    //    public bool ErrorOccured { get; private set; }
    //    public List<ErrorInfo> ErrorMessages { get; private set; }

    //    public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
    //    {
    //        ErrorOccured = true;

    //        if (e == null || e.GetType() != typeof(NoViableAltException))
    //        {
    //            ErrorMessages.Add(new ErrorInfo()
    //            {
    //                Message = ConvertMessage(msg),
    //                StartIndex = offendingSymbol.StartIndex,
    //                Column = offendingSymbol.Column + 1,
    //                Line = offendingSymbol.Line,
    //                Length = offendingSymbol.Text.Length
    //            });
    //            return;
    //        }

    //        ErrorMessages.Add(new ErrorInfo()
    //        {
    //            Message = string.Format("{0}{1}", ConvertToken(offendingSymbol.Text), " unexpected"),
    //            StartIndex = offendingSymbol.StartIndex,
    //            Column = offendingSymbol.Column + 1,
    //            Line = offendingSymbol.Line,
    //            Length = offendingSymbol.Text.Length
    //        });
    //    }

    //    public override void ReportAmbiguity(Antlr4.Runtime.Parser recognizer, DFA dfa, int startIndex, int stopIndex, bool exact, BitSet ambigAlts, ATNConfigSet configs)
    //    {
    //        ErrorOccured = true;
    //        ErrorMessages.Add(new ErrorInfo()
    //        {
    //            Message = "Ambiguity",
    //            Column = startIndex,
    //            StartIndex = startIndex
    //        });
    //        base.ReportAmbiguity(recognizer, dfa, startIndex, stopIndex, exact, ambigAlts, configs);
    //    }

    //    private string ConvertToken(string token)
    //    {
    //        return string.Equals(token, "<EOF>", StringComparison.InvariantCultureIgnoreCase)
    //            ? Eof
    //            : token;
    //    }

    //    private string ConvertMessage(string message)
    //    {
    //        StringBuilder builder = new StringBuilder(message);
    //        builder.Replace("<EOF>", Eof);
    //        return builder.ToString();
    //    }
    //}

}