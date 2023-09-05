# expr2dddmp.py
Converts boolean expression to BDD with DD(CUDD) and saves it using DDDMP format.

See https://www.swi-prolog.org/pack/file_details/bddem/cudd-3.0.0/dddmp/doc/dddmp-2.0-A4.ps

Supported syntax:
        SymPy: (x1 & y) | ~ z | ITE(x2, True, False)
        DD Promela: (x1 & y) | ! z | ite(x2, TRUE, FALSE)


usage: expr2dddmp.py [-h] input output

positional arguments:
  input       input file
  output      output file

options:
  -h, --help  show this help message and exit