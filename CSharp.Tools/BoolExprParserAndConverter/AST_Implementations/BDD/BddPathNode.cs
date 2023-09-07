namespace BddTools.AST_Implementations.BDD;

public struct BddPathNode {
    public Formula Formula;
    public bool Negation;

    public string PrintVariableAndNegation() {
        var negationExpr = Negation ? "!" : "";

        return $"{negationExpr}{Formula.Data}";
    }

    public override string ToString() =>
        PrintVariableAndNegation();

}