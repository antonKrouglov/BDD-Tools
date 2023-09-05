#!/usr/bin/env python

import argparse
import sys
from argparse import RawDescriptionHelpFormatter

from sympy.logic.boolalg import is_dnf, to_dnf

if __name__ == '__main__':  # https://stackoverflow.com/a/27876800/2746150
    if (__package__ is None) | (__package__ == ''):
        import sys
        from os import path
        sys.path.append(path.dirname(path.dirname(path.abspath(__file__))))
        from common.common import parse_bool_expr_sympy
    else:
        from ..common.common import \
            parse_bool_expr_sympy  # https://stackoverflow.com/a/42170807/2746150

PROGRAM_DESC = f"Converts boolean expression to [simplified] Disjunctive Normal Form (DNF).\n\
    See https://en.wikipedia.org/wiki/Disjunctive_normal_form.\n\
    Supported syntax:\n\
        SymPy: (x1 & y) | ~ z | ITE(x2, True, False)\n\
        DD Promela: (x1 & y) | ! z | ite(x2, TRUE, FALSE)\n\
        '#' line comments are supported inside expressions"


def expr_to_DNF(input_text, do_simplify):
    """Converts input_text boolean expression to simplified Disjunctive Normal Form (DNF)

    Args:
        input_text (string): boolean expression in sympy format
        
        do_simplify (bool): whether to simplify boolean expression or not

    Returns:
        string: simplified DNF form of source boolean expression
    """
    original_expr = parse_bool_expr_sympy(input_text)
    dnf_expr = to_dnf(original_expr, simplify=do_simplify, force=True)
    if ~is_dnf(original_expr):
        assert original_expr != dnf_expr, f"Something wrong with the source expression:\n{original_expr}"
    return f"{dnf_expr}"


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
        '-s',
        '--simplify',
        help='flag: whether to simplify boolean expression or not',
        action='store_true')

    parser.add_argument(
        'input',
        help='input file',
        type=argparse.FileType('r', encoding="utf-8"),
    )

    parser.add_argument(
        'output',
        help='output file, stdout if omitted',
        type=argparse.FileType('w', encoding="utf-8"),
        nargs='?',
    )

    args = parser.parse_args()

    if not args.input:
        parser.print_usage()
        return sys.exit(EXIT_FAILURE)

    with args.input as f:
        data = f.read()

    result = expr_to_DNF(data, args.simplify)

    if args.output:
        args.output.write(result)
        args.output.close()
        print("Expression converted to DNF successfully.")
    else:
        print(result)

    return sys.exit(EXIT_SUCCESS)


if __name__ == '__main__':
    main()
