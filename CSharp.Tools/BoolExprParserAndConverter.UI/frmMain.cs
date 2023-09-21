using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BddTools.AbstractSyntaxTrees;
using BddTools.Parser;
using BddTools.Util;
using BddTools.Variables;

namespace BddTools.UI {
    public partial class frmMain : Form {

        #region vars consts

        BindingList<string> variablesList;
        const string delim1 = "-^^^^-easy-processing-above=";
        const string delim2 = "-^^^^-normal------hard-vvvv=";

        #endregion


        #region form-related

        public frmMain() {
            InitializeComponent();
            InitOnFormCreation();
        }

        private void frmMain_Load(object sender, EventArgs e) => InitOnLoad();

        #endregion


        #region form init

        private void InitOnFormCreation() {
            variablesList = new BindingList<string>() { "var1", delim1, "var2", delim2, "var3" };
        }


        private void InitOnLoad() {
            lstVars.Items.Clear();
            lstVars.DataSource = variablesList;
            this.lstVars.AllowDrop = true;

            lstVars.DragDrop += lstVars_DragDrop;
            lstVars.DragOver += lstVars_DragOver;
            lstVars.MouseDown += lstVars_MouseDown;

            txtResult.HideSelection = false; //auto-scroll on append
            txtStatus.HideSelection = false; //auto-scroll on append
        }

        #endregion


        #region processing

        private void ProcessCommand(Action<string> action, string actionParam, string? actionName = null) {
            var statusText = actionName ?? "Command";
            logStatus($"{statusText}...");
            using (var b = new Benchmark($"{statusText} with {GetVariablesFromListbox().Count()} vars", logStatus)) {
                Cursor.Current = Cursors.WaitCursor; // Set cursor as hourglass
                try {
                    action(actionParam);
                }
                catch (Exception e) {
                    logError(e.ToString());
                }
                finally {
                    Cursor.Current = Cursors.Default; // Set cursor as default arrow
                }
            }
        }

        private void Verify(string booleanExpr) {
            //parse text
            if (!parseTextAndLogError(booleanExpr, out var parser)) return;
            logText($"[{booleanExpr}] - parsed successfully; {parser.Variables.Count} variables found.");

            //truth table of formula
            printTruthTable(parser.Formula);

            //fill variables list
            SetVariablesToListbox(parser.Variables.SortedList);
        }


        private void MinimizeWithGivenOrder(string booleanExpr) {
            //parse text
            if (!parseTextAndLogError(booleanExpr, out var parser, GetVariablesFromListbox())) return;

            //shortest possible formula with given variable order
            var minimalFormula = parser.Formula.MinimizeWithGivenOrder(parser.Variables.IdxToNameIDict);
            printBddFormula(minimalFormula);

            //disjunctive normal form of boolean expression
            printDNF(minimalFormula);

            //truth table of formula
            printTruthTable(minimalFormula.FormulaInner);
        }


        private void MinimizeWithReordering(string booleanExpr) {
            //parse text
            if (!parseTextAndLogError(booleanExpr, out var parser, GetVariablesFromListbox())) return;

            //get bdd-based formula to start with
            var bddFormula = parser.Formula.MinimizeWithGivenOrder(parser.Variables.IdxToNameIDict);

            //minimize with reordering
            var bestBddFormula = bddFormula.Reorder();

            //fill variables list using new order
            SetVariablesToListbox(bestBddFormula.Variables.SortedList);

            //print result
            printBddFormula(bestBddFormula);

            //disjunctive normal form of boolean expression
            printDNF(bestBddFormula);

            //truth table of formula
            printTruthTable(bestBddFormula.FormulaInner);
        }


        private bool parseTextAndLogError(string booleanExpr, out ParserOfBooleanExpressions parser, IEnumerable<VarInfo>? predefinedVars = null) {
            parser = new ParserOfBooleanExpressions(booleanExpr);
            var parsedOk = parser.Parse(predefinedVars);
            if (parsedOk) return true;
            logSyntaxError(parser.SyntaxErrors.FirstOrDefault());
            return false;
        }


        private void printBddFormula(BddMappedFormula minimalFormula, bool dividerBefore = true) {
            logText(
                minimalFormula.ToString(indentAndLineBreakIte: true)
                , dividerBefore: dividerBefore);
            logText(
                $"NODES: {minimalFormula.BddNodesCount()}"
                , dividerBefore: false);
        }


        private void printTruthTable(Formula? formula) {
            var truthTable = formula.EvaluateAll().ToBigInteger().ToString("X");
            logText($"Truth table = {truthTable}", dividerBefore: false);
        }


        private void printDNF(BddMappedFormula minimalFormula) {
            var dnfExpr = minimalFormula.AsDnf(Bool_OR_OpSymbol: "\n | ");
            logText($"DNF:\n{dnfExpr}", dividerBefore: false);
        }

        #endregion


        #region misc helpers

        private IEnumerable<string> GetNamesFromListbox()
            => variablesList.Where(s => s != delim1 && s != delim2);

        private IEnumerable<VarInfo> GetVariablesFromListbox()
            => GetNamesFromListbox().Select((t, j) => new VarInfo(t, j));

        private void SetVariablesToListbox(string[] variablesSortedList) {
            variablesList.Clear();
            var vars = variablesSortedList;
            for (var j = 0; j < vars.Count(); j++) {
                variablesList.Add(vars[j]);
            }

            variablesList.Add(delim1);
            variablesList.Add(delim2);
        }

        #endregion


        #region event handlers

        #region drag and drop setup for listbox

        private void lstVars_MouseDown(object sender, MouseEventArgs e) {
            if (this.lstVars.SelectedItem == null) return;
            this.lstVars.DoDragDrop(this.lstVars.SelectedItem, DragDropEffects.Move);
        }

        private void lstVars_DragOver(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.Move;
        }

        private void lstVars_DragDrop(object sender, DragEventArgs e) {
            Point point = lstVars.PointToClient(new Point(e.X, e.Y));
            int index = this.lstVars.IndexFromPoint(point);
            if (index < 0) index = this.lstVars.Items.Count - 1;
            object data = lstVars.SelectedItem;
            variablesList.Remove((string)data);
            //this.lstVars.Items.Remove(data);
            //this.lstVars.Items.Insert(index, data);
            variablesList.Insert(index, (string)data);
            this.lstVars.SelectedIndex = index;
        }

        #endregion

        private void cmdReadVerify_Click(object sender, EventArgs e) 
            => ProcessCommand(Verify, txtSrc.Text, nameof(Verify));

        private void cmdMinimizeWithGiven_Click(object sender, EventArgs e) 
            => ProcessCommand(MinimizeWithGivenOrder, txtSrc.Text, nameof(MinimizeWithGivenOrder));

        private void cmdMinimizeAndReorder_Click(object sender, EventArgs e) 
            => ProcessCommand(MinimizeWithReordering, txtSrc.Text, nameof(MinimizeWithReordering));

        #endregion


        #region logging

        private void logText(string message, bool newLineAfter = true, bool dividerBefore = true) {
            if (dividerBefore) logDivider();
            txtResult.AppendText($"{message}{(newLineAfter ? "\n" : "")}");
        }

        private void logStatus(string message) => txtStatus.AppendText($"{message}\n");

        private void logError(string message, bool newLineAfter = true, bool dividerBefore = true)
            => logText($"Error: {message}", newLineAfter, dividerBefore);

        private void logSyntaxError(SyntaxError? message, bool newLineAfter = true, bool dividerBefore = true)
            => logText($"Error: {message}", newLineAfter, dividerBefore);

        private void logDivider() {
            if (!string.IsNullOrEmpty(txtResult.Text)) AppendHorizontalBar(txtResult);
        }

        /// <summary> Appends a horizontal bar at the end of the specified Rich Text Box </summary>
        /// <param name="rtb">Rich Text Box to which horizontal bar is to be added</param>
        /// <remarks> https://stackoverflow.com/a/35445361/2746150 </remarks>
        private void AppendHorizontalBar(RichTextBox rtb) {
            // Position cursor at end of text
            rtb.Select(rtb.TextLength, 0);
            int selStart = rtb.TextLength;
            int selEnd = rtb.TextLength;

            // Textbox may transform chars, so (end-start) != text.Length
            rtb.Select(selStart, selEnd - selStart);

            // This is the RTF section to add.
            string horizontalBarRtf =
                @"{\pict\wmetafile8\picw12777\pich117\picwgoal7245\pichgoal60 0100090000035b00000004000800000000000400000003010800050000000b0200000000050000000c022100280e030000001e0008000000fa0200000300000000008000040000002d01000007000000fc020100000000000000040000002d010100080000002503020011001100170e110008000000fa0200000000000000000000040000002d01020007000000fc020000ffffff000000040000002d01030004000000f0010000040000002701ffff030000000000}";
            string centreText = ""; //"\\pard\\qc"; // set this to empty string to keep existing text alignment

            // Wrap to-add RTF section in RTF tag
            rtb.SelectedRtf = String.Format("{{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033\\uc1 {0} {1} \\line}}", centreText, horizontalBarRtf);

            // Leave no text selected
            rtb.SelectionLength = 0;
        }

        #endregion

    }
}