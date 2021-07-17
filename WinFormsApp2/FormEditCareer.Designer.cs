
namespace WinFormsApp2
{
    partial class FormEditCareer
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblYear = new System.Windows.Forms.Label();
            this.txtCareer = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(179, 111);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 26);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(267, 111);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(82, 26);
            this.button2.TabIndex = 1;
            this.button2.Text = "キャンセル";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "年：";
            // 
            // lblYear
            // 
            this.lblYear.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblYear.Location = new System.Drawing.Point(49, 6);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(82, 18);
            this.lblYear.TabIndex = 3;
            this.lblYear.Text = "label2";
            // 
            // txtCareer
            // 
            this.txtCareer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCareer.Location = new System.Drawing.Point(49, 33);
            this.txtCareer.Multiline = true;
            this.txtCareer.Name = "txtCareer";
            this.txtCareer.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCareer.Size = new System.Drawing.Size(300, 71);
            this.txtCareer.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "経歴：";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitContainer1.Location = new System.Drawing.Point(179, 2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Size = new System.Drawing.Size(59, 25);
            this.splitContainer1.SplitterDistance = 25;
            this.splitContainer1.TabIndex = 6;
            // 
            // EditCareer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 149);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtCareer);
            this.Controls.Add(this.lblYear);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.MinimizeBox = false;
            this.Name = "EditCareer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "経歴編集";
            this.Load += new System.EventHandler(this.EditCareer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.TextBox txtCareer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}