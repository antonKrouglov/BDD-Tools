using System.Drawing;
using System.Windows.Forms;

namespace BddTools.UI
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitMain = new SplitContainer();
            splitLeft = new SplitContainer();
            txtSrc = new RichTextBox();
            cmdMinimizeWithGiven = new Button();
            cmdReadVerify = new Button();
            splitRight = new SplitContainer();
            lstVars = new ListBox();
            txtResult = new RichTextBox();
            cmdMinimizeAndReorder = new Button();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitLeft).BeginInit();
            splitLeft.Panel1.SuspendLayout();
            splitLeft.Panel2.SuspendLayout();
            splitLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitRight).BeginInit();
            splitRight.Panel1.SuspendLayout();
            splitRight.Panel2.SuspendLayout();
            splitRight.SuspendLayout();
            SuspendLayout();
            // 
            // splitMain
            // 
            splitMain.BorderStyle = BorderStyle.Fixed3D;
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 0);
            splitMain.Margin = new Padding(3, 4, 3, 4);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(splitLeft);
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(splitRight);
            splitMain.Size = new Size(914, 600);
            splitMain.SplitterDistance = 303;
            splitMain.SplitterWidth = 5;
            splitMain.TabIndex = 0;
            // 
            // splitLeft
            // 
            splitLeft.BorderStyle = BorderStyle.Fixed3D;
            splitLeft.Dock = DockStyle.Fill;
            splitLeft.Location = new Point(0, 0);
            splitLeft.Margin = new Padding(3, 4, 3, 4);
            splitLeft.Name = "splitLeft";
            splitLeft.Orientation = Orientation.Horizontal;
            // 
            // splitLeft.Panel1
            // 
            splitLeft.Panel1.Controls.Add(txtSrc);
            // 
            // splitLeft.Panel2
            // 
            splitLeft.Panel2.Controls.Add(cmdMinimizeAndReorder);
            splitLeft.Panel2.Controls.Add(cmdMinimizeWithGiven);
            splitLeft.Panel2.Controls.Add(cmdReadVerify);
            splitLeft.Size = new Size(303, 600);
            splitLeft.SplitterDistance = 434;
            splitLeft.SplitterWidth = 5;
            splitLeft.TabIndex = 0;
            // 
            // txtSrc
            // 
            txtSrc.Dock = DockStyle.Fill;
            txtSrc.Location = new Point(0, 0);
            txtSrc.Margin = new Padding(3, 4, 3, 4);
            txtSrc.Name = "txtSrc";
            txtSrc.Size = new Size(299, 430);
            txtSrc.TabIndex = 0;
            txtSrc.Text = "";
            txtSrc.WordWrap = false;
            // 
            // cmdMinimizeWithGiven
            // 
            cmdMinimizeWithGiven.Dock = DockStyle.Top;
            cmdMinimizeWithGiven.Location = new Point(0, 43);
            cmdMinimizeWithGiven.Margin = new Padding(3, 4, 3, 4);
            cmdMinimizeWithGiven.Name = "cmdMinimizeWithGiven";
            cmdMinimizeWithGiven.Size = new Size(299, 43);
            cmdMinimizeWithGiven.TabIndex = 1;
            cmdMinimizeWithGiven.Text = "Minimize with given variable order";
            cmdMinimizeWithGiven.UseVisualStyleBackColor = true;
            cmdMinimizeWithGiven.Click += cmdMinimizeWithGiven_Click;
            // 
            // cmdReadVerify
            // 
            cmdReadVerify.Dock = DockStyle.Top;
            cmdReadVerify.Location = new Point(0, 0);
            cmdReadVerify.Margin = new Padding(3, 4, 3, 4);
            cmdReadVerify.Name = "cmdReadVerify";
            cmdReadVerify.Size = new Size(299, 43);
            cmdReadVerify.TabIndex = 0;
            cmdReadVerify.Text = "Read | Verify";
            cmdReadVerify.UseVisualStyleBackColor = true;
            cmdReadVerify.Click += cmdReadVerify_Click;
            // 
            // splitRight
            // 
            splitRight.BorderStyle = BorderStyle.Fixed3D;
            splitRight.Dock = DockStyle.Fill;
            splitRight.Location = new Point(0, 0);
            splitRight.Margin = new Padding(3, 4, 3, 4);
            splitRight.Name = "splitRight";
            // 
            // splitRight.Panel1
            // 
            splitRight.Panel1.Controls.Add(lstVars);
            // 
            // splitRight.Panel2
            // 
            splitRight.Panel2.Controls.Add(txtResult);
            splitRight.Size = new Size(606, 600);
            splitRight.SplitterDistance = 201;
            splitRight.SplitterWidth = 5;
            splitRight.TabIndex = 0;
            // 
            // lstVars
            // 
            lstVars.Dock = DockStyle.Fill;
            lstVars.FormattingEnabled = true;
            lstVars.ItemHeight = 20;
            lstVars.Items.AddRange(new object[] { "a", "b", "c", "d" });
            lstVars.Location = new Point(0, 0);
            lstVars.Margin = new Padding(3, 4, 3, 4);
            lstVars.Name = "lstVars";
            lstVars.Size = new Size(197, 596);
            lstVars.TabIndex = 0;
            lstVars.DragDrop += lstVars_DragDrop;
            lstVars.DragOver += lstVars_DragOver;
            lstVars.MouseDown += lstVars_MouseDown;
            // 
            // txtResult
            // 
            txtResult.Dock = DockStyle.Fill;
            txtResult.Location = new Point(0, 0);
            txtResult.Margin = new Padding(3, 4, 3, 4);
            txtResult.Name = "txtResult";
            txtResult.Size = new Size(396, 596);
            txtResult.TabIndex = 1;
            txtResult.Text = "";
            txtResult.WordWrap = false;
            // 
            // cmdMinimizeAndReorder
            // 
            cmdMinimizeAndReorder.Dock = DockStyle.Top;
            cmdMinimizeAndReorder.Location = new Point(0, 86);
            cmdMinimizeAndReorder.Margin = new Padding(3, 4, 3, 4);
            cmdMinimizeAndReorder.Name = "cmdMinimizeAndReorder";
            cmdMinimizeAndReorder.Size = new Size(299, 43);
            cmdMinimizeAndReorder.TabIndex = 2;
            cmdMinimizeAndReorder.Text = "Minimize and reorder";
            cmdMinimizeAndReorder.UseVisualStyleBackColor = true;
            cmdMinimizeAndReorder.Click += cmdMinimizeAndReorder_Click;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 600);
            Controls.Add(splitMain);
            Margin = new Padding(3, 4, 3, 4);
            Name = "frmMain";
            Text = "Boolean tools";
            Load += frmMain_Load;
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            splitLeft.Panel1.ResumeLayout(false);
            splitLeft.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitLeft).EndInit();
            splitLeft.ResumeLayout(false);
            splitRight.Panel1.ResumeLayout(false);
            splitRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitRight).EndInit();
            splitRight.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitMain;
        private SplitContainer splitLeft;
        private RichTextBox txtSrc;
        private SplitContainer splitRight;
        private ListBox lstVars;
        private Button cmdReadVerify;
        private RichTextBox txtResult;
        private Button cmdMinimizeWithGiven;
        private Button cmdMinimizeAndReorder;
    }
}