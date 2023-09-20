/*
Simple boolean grammar handling (x | y) & !z
ITE (if, then, else) is supported as well.
Boolean literals are True, False.
Literals and function names are NOT case sensitive.
AND, OR binary operators have EQUAL priority.
Adapted from https://stackoverflow.com/a/30987142/2746150.
*/

grammar SimpleBoolean;
options {
	language = 'CSharp';
}

parse: expression EOF;

expression:
	op = ITE LPAREN ifcond = expression 
				COMMA thenexpr = expression
				COMMA elseexpr = expression
			 RPAREN											# iteExpr
	| LPAREN exprInsideParenthesis = expression RPAREN		# parenExpr
	| NOT exprInsideNot = expression						# notExpr
	| left = expression op = binaryOp right = expression	# binaryExpr
	| boolLiteral											# boolLiteralExpr
	| IDENTIFIER											# variableExpr;

binaryOp: AND | OR;

boolLiteral: TRUE | FALSE;

ITE: [iI][tT][eE];
COMMA: ',';
AND: '&';
OR: '|';
NOT: '!';
TRUE: [tT][rR][uU][eE];
FALSE: [fF][aA][lL][sS][eE];
LPAREN: '(';
RPAREN: ')';
IDENTIFIER: [a-zA-Z_][a-zA-Z_0-9]*;
WS: [ \r\t\u000C\n]+ -> skip;