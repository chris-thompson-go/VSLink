namespace VsLink
{
    partial class LinkEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.scintilla = new ScintillaNET.Scintilla();
			this.txtLink = new VsLink.BorderedTextBox();
			this.txtTargetLabel = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.scintilla, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtLink, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtTargetLabel, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(654, 421);
			this.tableLayoutPanel1.TabIndex = 13;
			// 
			// scintilla
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.scintilla, 2);
			this.scintilla.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scintilla.HScrollBar = false;
			this.scintilla.Lexer = ScintillaNET.Lexer.Properties;
			this.scintilla.Location = new System.Drawing.Point(3, 28);
			this.scintilla.Name = "scintilla";
			this.scintilla.Size = new System.Drawing.Size(648, 390);
			this.scintilla.TabIndex = 16;
			this.scintilla.VScrollBar = false;
			// 
			// txtLink
			// 
			this.txtLink.BackColor = System.Drawing.SystemColors.ControlDark;
			this.txtLink.DefaultBorderColor = System.Drawing.SystemColors.ControlDark;
			this.txtLink.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtLink.FocusedBorderColor = System.Drawing.SystemColors.ControlDark;
			this.txtLink.Location = new System.Drawing.Point(53, 3);
			this.txtLink.Name = "txtLink";
			this.txtLink.Padding = new System.Windows.Forms.Padding(1);
			this.txtLink.Size = new System.Drawing.Size(598, 20);
			this.txtLink.TabIndex = 13;
			// 
			// txtTargetLabel
			// 
			this.txtTargetLabel.AutoSize = true;
			this.txtTargetLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtTargetLabel.Location = new System.Drawing.Point(3, 0);
			this.txtTargetLabel.Name = "txtTargetLabel";
			this.txtTargetLabel.Size = new System.Drawing.Size(44, 25);
			this.txtTargetLabel.TabIndex = 12;
			this.txtTargetLabel.Text = "Target:";
			this.txtTargetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LinkEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "LinkEditor";
			this.Size = new System.Drawing.Size(654, 421);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label txtTargetLabel;
        private BorderedTextBox txtLink;
        private ScintillaNET.Scintilla scintilla;
    }
}
