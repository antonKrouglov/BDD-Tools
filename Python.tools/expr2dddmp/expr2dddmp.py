#!/usr/bin/env python

import argparse
import sys
from argparse import RawDescriptionHelpFormatter

import sympy
from dd import cudd as _bdd
from sympy.logic.boolalg import is_dnf, to_dnf

if __name__ == '__main__':  # https://stackoverflow.com/a/27876800/2746150
    if (__package__ is None) | (__package__ == ''):
        import sys
        from os import path
        sys.path.append(path.dirname(path.dirname(path.abspath(__file__))))
        from common.common import (normalize_for_DD_Promela_bool_expr,
                                   parse_bool_expr_sympy)
    else:
        from ..common.common import (  # https://stackoverflow.com/a/42170807/2746150
            normalize_for_DD_Promela_bool_expr, parse_bool_expr_sympy)

PROGRAM_DESC = f"Converts boolean expression to BDD with DD(CUDD) and saves it using DDDMP format.\n\
    See (https://www.swi-prolog.org/pack/file_details/bddem/cudd-3.0.0/dddmp/doc/dddmp-2.0-A4.ps).\n\
    Supported syntax:\n\
        SymPy: (x1 & y) | ~ z | ITE(x2, True, False)\n\
        DD Promela: (x1 & y) | ! z | ite(x2, TRUE, FALSE)"


def expr_save_as_DDDMP(input_text, output_filename):
    """Converts input_text boolean expression to simplified Disjunctive Normal Form (DNF)

    Args:
        input_text (string): boolean expression in sympy format

    Returns:
        string: simplified DNF form of source boolean expression
    """
    
    # parse to SymPy 1st - we need to extract variables
    sympy_expr = parse_bool_expr_sympy(input_text)
    variables = sympy_expr.atoms(sympy.Symbol)
    assert len(variables) > 0, f"No variables can be parsed from:\n{input_text}"
    
    vars_str = [str(x) for x in variables]

    bdd = _bdd.BDD()

    bdd.declare(*vars_str)
    
    dd_expr_text = normalize_for_DD_Promela_bool_expr(input_text)
    cudd_bdd = bdd.add_expr(dd_expr_text)
    bdd.dump(output_filename, [cudd_bdd], "dddmp")
    
    return


EXIT_SUCCESS = 0
EXIT_FAILURE = 1
PROGRAM_DESC2 = '-----------------------------------------------------'


def main():  # adapted from https://stackoverflow.com/a/65971339/2746150
    """Parse arguments and call process function

    Returns:
        int: EXIT_SUCCESS = 0 or EXIT_FAILURE = 1
    """
    parser = argparse.ArgumentParser(
        description=PROGRAM_DESC,
        epilog=PROGRAM_DESC2,
        formatter_class=RawDescriptionHelpFormatter
    )

    parser.add_argument(
        'input',
        help='input file',
        type=argparse.FileType('r', encoding="utf-8"),
    )

    parser.add_argument(
        'output',
        help='output file',
        type=argparse.FileType('w', encoding="utf-8"),
    )

    args = parser.parse_args()

    if (not args.input) | (not args.output):
        parser.print_usage()
        return sys.exit(EXIT_FAILURE)

    with args.input as f:
        data = f.read()

    output_filename = args.output.name
    args.output.close()

    expr_save_as_DDDMP(data, output_filename)

    print("Expression saved to DDDMP successfully.")

    return sys.exit(EXIT_SUCCESS)


if __name__ == '__main__':
    main()
