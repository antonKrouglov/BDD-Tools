namespace BddTools.AST_Implementations.BDD;

public static class BddFormulaBuilder
{
    /// <summary> Verify given formula meets the spec </summary>
    /// <param name="f">given formula</param>
    /// <param name="leafsNeedsRewrite">Minor problems with leaf nodes</param>
    /// <returns>true or false</returns>
    public static bool Verify(Formula f, out bool leafsNeedsRewrite)
    {
        bool hasVarLeafs = false, hasBadLeafs = false, hasBadNonLeafs = false, hasBadIteCoditions = false;
        foreach (var frm in f.DescendantsAndSelf)
        {
            if (frm.IsLeaf)
            {
                // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                switch (frm.Type)
                {
                    case AstType.TRUE:
                        break;
                    case AstType.FALSE:
                        break;
                    case AstType.VAR:
                        hasVarLeafs = true;
                        break;
                    default:
                        hasBadLeafs = true; //leafs must var or boolLiteral
                        break;
                }

                continue;
            }

            if (frm.IsIte)
            {
                hasBadIteCoditions = !frm.IteCondition.IsVar; //ITE conditions must be of VAR type
            }
            else
            {
                hasBadNonLeafs = true; //NonLeafs must be of ITE type
            }
        }

        leafsNeedsRewrite = hasVarLeafs; //VAR leafs must be expanded to Ite(VarName, true, false)

        var isBad = hasBadNonLeafs | hasBadIteCoditions | hasBadLeafs;

        return !isBad;
    }

    public static void VerifyAndPatch(Formula formula)
    {
        var formulaIsBdd = Verify(formula, out var leafsNeedRewrite);
        if (!formulaIsBdd)
        {
            throw new ArgumentException($"{formula} is not BDD formula.", nameof(formula));
        }

        formula.FillUp();

        if (!leafsNeedRewrite) return;

        //Replace VAR leafs with Ite(VarName, true, false)
        foreach (var leafOfVarType in formula.DescendantsAndSelf.Where(fc => fc.IsLeaf & fc.IsVar))
        {
            var replacementFormula = Formula.Ite(leafOfVarType, Formula.True(), Formula.False());
            if (leafOfVarType.ParentRefersToMeAs_ThenExpr)
            {
                leafOfVarType.Parent.IteThen = replacementFormula;
            }

            if (leafOfVarType.ParentRefersToMeAs_ElseExpr)
            {
                leafOfVarType.Parent.IteElse = replacementFormula;
            }
        }

        formula.FillUp();
    }
}