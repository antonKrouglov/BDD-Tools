import re
from sympy import parse_expr



def replace_all(text, dic):
    """replaces whole words in text using substitution dict, case INSENSITIVE

    Args:
        text (string): text to search
        dic (dictinary): dictionary of search->replace tokens

    Returns:
        string: text with replacements
    """
    for i, j in dic.items():
        text = re.sub(i, j, text, flags=re.IGNORECASE)
    return text


def normalize_for_sympy_bool_expr(src_text):
    """converts case of expression for correct sympy parsing and converts '!' to '~'
        SymPy syntax: (x1 & y) | ~ z | ITE(x2, True, False)\n\

    Args:
        src_text (string): source expression text

    Returns:
        string: corrected expression text
    """
    repl1 = {r'\bTRUE\b': 'trux', r'\bFALSE\b': 'falsx', r'\bITE\b': 'itx', '\!': "~"}
    repl2 = {r'\btrux\b': 'True', r'\bfalsx\b': 'False', r'\bitx\b': 'ITE'}
    txt2 = replace_all(src_text, repl1)
    txt3 = replace_all(txt2, repl2)
    return txt3


def normalize_for_DD_Promela_bool_expr(src_text):
    """converts expression to correct DD Promela casing and converts '~' to '!'
        DD Promela syntax: (x1 & y) | ! z | ite(x2, TRUE, FALSE)"

    Args:
        src_text (string): source expression text

    Returns:
        string: corrected expression text
    """
    repl1 = {r'\bTRUE\b': 'trux', r'\bFALSE\b': 'falsx', r'\bITE\b': 'itx', '~': "!"}
    repl2 = {r'\btrux\b': 'TRUE', r'\bfalsx\b': 'FALSE', r'\bitx\b': 'ite'}
    txt2 = replace_all(src_text, repl1)
    txt3 = replace_all(txt2, repl2)
    return txt3


def parse_bool_expr_sympy(input_text, do_evaluate=False):
    """parse string to SymPy boolean expression

    Args:
        input_text (string): boolean expression in sympy format

    Returns:
        SymPy expression: expression
    """
    expr_text = normalize_for_sympy_bool_expr(input_text)
    original_expr = parse_expr(expr_text, evaluate=do_evaluate)
    return original_expr