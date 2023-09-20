using System.Linq.Expressions;

namespace BddTools.AbstractSyntaxTrees {

    public class FormulaPrintOptions {
        public static FormulaPrintOptions Default => new();

        // ReSharper disable InconsistentNaming
        public FormulaPrintOptions(string OR = " | ", string AND = " & ", string NOT = " NOT ", bool encloseOR = true, bool encloseAND = true, bool encloseNOT = true, bool indentAndLineBreakIte = false) {
            this.OR = OR;
            this.AND = AND;
            this.NOT = NOT;
            EncloseOR = encloseOR;
            EncloseAND = encloseAND;
            EncloseNOT = encloseNOT;
            IndentAndLineBreakITE = indentAndLineBreakIte;
        }

        public string OR { get; set; }
        public string AND { get; set; }
        public string NOT { get; set; }
        public bool EncloseOR { get; set; }
        public bool EncloseAND { get; set; }
        public bool EncloseNOT { get; set; }

        public bool IndentAndLineBreakITE { get; set; }
        // ReSharper restore InconsistentNaming
    }
}