using System;
using System.Windows.Forms;
using SwissAcademic.Controls;

namespace SingleLineScrollGrid
{
    partial class GridScrollSettingsDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chkRefEnabled = new System.Windows.Forms.CheckBox();
            this.numRefLines = new System.Windows.Forms.NumericUpDown();
            this.lblRefLines = new System.Windows.Forms.Label();
            
            this.chkKnowEnabled = new System.Windows.Forms.CheckBox();
            this.numKnowLines = new System.Windows.Forms.NumericUpDown();
            this.lblKnowLines = new System.Windows.Forms.Label();
            
            this.chkTaskEnabled = new System.Windows.Forms.CheckBox();
            this.numTaskLines = new System.Windows.Forms.NumericUpDown();
            this.lblTaskLines = new System.Windows.Forms.Label();
            
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            
            ((System.ComponentModel.ISupportInitialize)(this.numRefLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKnowLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTaskLines)).BeginInit();
            this.SuspendLayout();
            
            // 
            // chkRefEnabled
            // 
            this.chkRefEnabled.AutoSize = true;
            this.chkRefEnabled.Location = new System.Drawing.Point(20, 20);
            this.chkRefEnabled.Name = "chkRefEnabled";
            this.chkRefEnabled.Size = new System.Drawing.Size(200, 17);
            this.chkRefEnabled.TabIndex = 0;
            this.chkRefEnabled.Text = "启用 Reference 列表滚动优化";
            this.chkRefEnabled.UseVisualStyleBackColor = true;
            
            // 
            // numRefLines
            // 
            this.numRefLines.Location = new System.Drawing.Point(130, 47);
            this.numRefLines.Minimum = new decimal(new int[] {1, 0, 0, 0});
            this.numRefLines.Maximum = new decimal(new int[] {10, 0, 0, 0});
            this.numRefLines.Name = "numRefLines";
            this.numRefLines.Size = new System.Drawing.Size(60, 20);
            this.numRefLines.TabIndex = 1;
            this.numRefLines.Value = new decimal(new int[] {1, 0, 0, 0});
            
            // 
            // lblRefLines
            // 
            this.lblRefLines.AutoSize = true;
            this.lblRefLines.Location = new System.Drawing.Point(40, 50);
            this.lblRefLines.Name = "lblRefLines";
            this.lblRefLines.Size = new System.Drawing.Size(65, 12);
            this.lblRefLines.TabIndex = 2;
            this.lblRefLines.Text = "滚动行数:";
            
            // 
            // chkKnowEnabled
            // 
            this.chkKnowEnabled.AutoSize = true;
            this.chkKnowEnabled.Location = new System.Drawing.Point(20, 90);
            this.chkKnowEnabled.Name = "chkKnowEnabled";
            this.chkKnowEnabled.Size = new System.Drawing.Size(200, 17);
            this.chkKnowEnabled.TabIndex = 3;
            this.chkKnowEnabled.Text = "启用 Knowledge 列表滚动优化";
            this.chkKnowEnabled.UseVisualStyleBackColor = true;
            
            // 
            // numKnowLines
            // 
            this.numKnowLines.Location = new System.Drawing.Point(130, 117);
            this.numKnowLines.Minimum = new decimal(new int[] {1, 0, 0, 0});
            this.numKnowLines.Maximum = new decimal(new int[] {10, 0, 0, 0});
            this.numKnowLines.Name = "numKnowLines";
            this.numKnowLines.Size = new System.Drawing.Size(60, 20);
            this.numKnowLines.TabIndex = 4;
            this.numKnowLines.Value = new decimal(new int[] {1, 0, 0, 0});
            
            // 
            // lblKnowLines
            // 
            this.lblKnowLines.AutoSize = true;
            this.lblKnowLines.Location = new System.Drawing.Point(40, 120);
            this.lblKnowLines.Name = "lblKnowLines";
            this.lblKnowLines.Size = new System.Drawing.Size(65, 12);
            this.lblKnowLines.TabIndex = 5;
            this.lblKnowLines.Text = "滚动行数:";
            
            // 
            // chkTaskEnabled
            // 
            this.chkTaskEnabled.AutoSize = true;
            this.chkTaskEnabled.Location = new System.Drawing.Point(20, 160);
            this.chkTaskEnabled.Name = "chkTaskEnabled";
            this.chkTaskEnabled.Size = new System.Drawing.Size(200, 17);
            this.chkTaskEnabled.TabIndex = 6;
            this.chkTaskEnabled.Text = "启用 Task 列表滚动优化";
            this.chkTaskEnabled.UseVisualStyleBackColor = true;
            
            // 
            // numTaskLines
            // 
            this.numTaskLines.Location = new System.Drawing.Point(130, 187);
            this.numTaskLines.Minimum = new decimal(new int[] {1, 0, 0, 0});
            this.numTaskLines.Maximum = new decimal(new int[] {10, 0, 0, 0});
            this.numTaskLines.Name = "numTaskLines";
            this.numTaskLines.Size = new System.Drawing.Size(60, 20);
            this.numTaskLines.TabIndex = 7;
            this.numTaskLines.Value = new decimal(new int[] {1, 0, 0, 0});
            
            // 
            // lblTaskLines
            // 
            this.lblTaskLines.AutoSize = true;
            this.lblTaskLines.Location = new System.Drawing.Point(40, 190);
            this.lblTaskLines.Name = "lblTaskLines";
            this.lblTaskLines.Size = new System.Drawing.Size(65, 12);
            this.lblTaskLines.TabIndex = 8;
            this.lblTaskLines.Text = "滚动行数:";
            
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(180, 230);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 25);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(270, 230);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 25);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(20, 230);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(80, 25);
            this.btnReset.TabIndex = 11;
            this.btnReset.Text = "重置默认";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            
            // 
            // GridScrollSettingsDialog
            // 
            this.AcceptButton = this.btnOk;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(370, 280);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblTaskLines);
            this.Controls.Add(this.numTaskLines);
            this.Controls.Add(this.chkTaskEnabled);
            this.Controls.Add(this.lblKnowLines);
            this.Controls.Add(this.numKnowLines);
            this.Controls.Add(this.chkKnowEnabled);
            this.Controls.Add(this.lblRefLines);
            this.Controls.Add(this.numRefLines);
            this.Controls.Add(this.chkRefEnabled);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GridScrollSettingsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Grid 滚动设置";
            ((System.ComponentModel.ISupportInitialize)(this.numRefLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKnowLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTaskLines)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        #region 控件声明
        
        private System.Windows.Forms.CheckBox chkRefEnabled;
        private System.Windows.Forms.NumericUpDown numRefLines;
        private System.Windows.Forms.Label lblRefLines;
        
        private System.Windows.Forms.CheckBox chkKnowEnabled;
        private System.Windows.Forms.NumericUpDown numKnowLines;
        private System.Windows.Forms.Label lblKnowLines;
        
        private System.Windows.Forms.CheckBox chkTaskEnabled;
        private System.Windows.Forms.NumericUpDown numTaskLines;
        private System.Windows.Forms.Label lblTaskLines;
        
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnReset;
        
        #endregion
    }
}
