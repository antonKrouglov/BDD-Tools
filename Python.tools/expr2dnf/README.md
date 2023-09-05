# expr2dnf.py
Converts boolean expression to [simplified] Disjunctive Normal Form (DNF).

See https://en.wikipedia.org/wiki/Disjunctive_normal_form.

Supported syntax:
- SymPy: (x1 & y) | ~ z | ITE(x2, True, False)
- DD Promela: (x1 & y) | ! z | ite(x2, TRUE, FALSE)
- '#' line comments are supported inside expressions

## Usage:
    expr2dnf.py [-h] [-s] input [output]

    positional arguments:
      input       input file
      output      output file, stdout if omitted

    options:
      -h, --help  show this help message and exit
      -s, --simplify  flag: whether to simplify boolean expression or not