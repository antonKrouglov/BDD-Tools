/* 
Simple ITE(if, then, else) representation of Boolean Decision Diagrams.
Boolean literals are True, False.
Literals and function names are NOT case sensitive.
Adapted from https://stackoverflow.com/a/30987142/2746150.
*/
grammar iteForBdd;
options {
	language = 'CSharp';
}

parse: expression EOF;

expression:
	op = ITE LPAREN ifcond = expression 
				COMMA thenexpr = expression
				COMMA elseexpr = expression
			 RPAREN	# iteExpr
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