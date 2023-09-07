/* simple boolean grammar handling (x | y) & !z ITE (if, then, else) is supported as well boolean
 literals as True, False (case insensitive) Ite is also case insensitive Adapted from
 https://stackoverflow.com/a/30987142/2746150
 */
grammar SimpleBoolean;
options {
	language = 'CSharp';
}

parse: expression EOF;

expression:
	op = ITE LPAREN ifcond = expression COMMA thenexpr = expression COMMA elseexpr = expression
		RPAREN												# iteExpr
	| LPAREN expression RPAREN								# parenExpr
	| NOT expression										# notExpr
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