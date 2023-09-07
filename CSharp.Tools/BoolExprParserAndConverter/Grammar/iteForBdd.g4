/* simple ITE (if, then, else) representaion of Boolean Decison Diagrams boolean literals as True,
 False (case insensitive) Ite is also case insensitive Adapted from
 https://stackoverflow.com/a/30987142/2746150
 */
grammar iteForBdd;
options {
	language = 'CSharp';
}

parse: expression EOF;

expression:
	op = ITE LPAREN ifcond = IDENTIFIER COMMA thenexpr = expression COMMA elseexpr = expression
		RPAREN		# iteExpr
	| boolLiteral	# boolLiteralExpr
	| IDENTIFIER	# variableExpr;

boolLiteral: TRUE | FALSE;

ITE: [iI][tT][eE];
COMMA: ',';
TRUE: [tT][rR][uU][eE];
FALSE: [fF][aA][lL][sS][eE];
LPAREN: '(';
RPAREN: ')';
IDENTIFIER: [a-zA-Z_][a-zA-Z_0-9]*;
WS: [ \r\t\u000C\n]+ -> skip;