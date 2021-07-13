
namespace WinFormsApp2
{
    partial class FormKonkihou
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblKonkiNikkansi = new System.Windows.Forms.Label();
            this.lblKonkiGekkansi = new System.Windows.Forms.Label();
            this.lblKonkiNenkansi = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblScoreNikkansi = new System.Windows.Forms.Label();
            this.lblScoreGekkansi = new System.Windows.Forms.Label();
            this.lblScoreNenkansi = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(10, 23);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(173, 105);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lblKonkiNikkansi
            // 
            this.lblKonkiNikkansi.AutoSize = true;
            this.lblKonkiNikkansi.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblKonkiNikkansi.Location = new System.Drawing.Point(247, 23);
            this.lblKonkiNikkansi.Name = "lblKonkiNikkansi";
            this.lblKonkiNikkansi.Size = new System.Drawing.Size(52, 21);
            this.lblKonkiNikkansi.TabIndex = 1;
            this.lblKonkiNikkansi.Text = "label1";
            // 
            // lblKonkiGekkansi
            // 
            this.lblKonkiGekkansi.AutoSize = true;
            this.lblKonkiGekkansi.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblKonkiGekkansi.Location = new System.Drawing.Point(247, 61);
            this.lblKonkiGekkansi.Name = "lblKonkiGekkansi";
            this.lblKonkiGekkansi.Size = new System.Drawing.Size(52, 21);
            this.lblKonkiGekkansi.TabIndex = 2;
            this.lblKonkiGekkansi.Text = "label2";
            // 
            // lblKonkiNenkansi
            // 
            this.lblKonkiNenkansi.AutoSize = true;
            this.lblKonkiNenkansi.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblKonkiNenkansi.Location = new System.Drawing.Point(247, 96);
            this.lblKonkiNenkansi.Name = "lblKonkiNenkansi";
            this.lblKonkiNenkansi.Size = new System.Drawing.Size(52, 21);
            this.lblKonkiNenkansi.TabIndex = 3;
            this.lblKonkiNenkansi.Text = "label3";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(207, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 21);
            this.label1.TabIndex = 4;
            this.label1.Text = "日：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(207, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 21);
            this.label2.TabIndex = 5;
            this.label2.Text = "月：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(207, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 21);
            this.label3.TabIndex = 5;
            this.label3.Text = "年：";
            // 
            // lblScoreNikkansi
            // 
            this.lblScoreNikkansi.AutoSize = true;
            this.lblScoreNikkansi.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblScoreNikkansi.Location = new System.Drawing.Point(35, 2);
            this.lblScoreNikkansi.Name = "lblScoreNikkansi";
            this.lblScoreNikkansi.Size = new System.Drawing.Size(21, 17);
            this.lblScoreNikkansi.TabIndex = 4;
            this.lblScoreNikkansi.Text = "点";
            // 
            // lblScoreGekkansi
            // 
            this.lblScoreGekkansi.AutoSize = true;
            this.lblScoreGekkansi.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblScoreGekkansi.Location = new System.Drawing.Point(80, 2);
            this.lblScoreGekkansi.Name = "lblScoreGekkansi";
            this.lblScoreGekkansi.Size = new System.Drawing.Size(21, 17);
            this.lblScoreGekkansi.TabIndex = 4;
            this.lblScoreGekkansi.Text = "点";
            // 
            // lblScoreNenkansi
            // 
            this.lblScoreNenkansi.AutoSize = true;
            this.lblScoreNenkansi.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblScoreNenkansi.Location = new System.Drawing.Point(124, 2);
            this.lblScoreNenkansi.Name = "lblScoreNenkansi";
            this.lblScoreNenkansi.Size = new System.Drawing.Size(21, 17);
            this.lblScoreNenkansi.TabIndex = 4;
            this.lblScoreNenkansi.Text = "点";
            // 
            // FormKonkihou
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 131);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblScoreNenkansi);
            this.Controls.Add(this.lblScoreGekkansi);
            this.Controls.Add(this.lblScoreNikkansi);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblKonkiNenkansi);
            this.Controls.Add(this.lblKonkiGekkansi);
            this.Controls.Add(this.lblKonkiNikkansi);
            this.Controls.Add(this.pictureBox1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(450, 170);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(450, 170);
            this.Name = "FormKonkihou";
            this.Text = "根気法";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormKonkihou_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblKonkiNikkansi;
        private System.Windows.Forms.Label lblKonkiGekkansi;
        private System.Windows.Forms.Label lblKonkiNenkansi;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblScoreNikkansi;
        private System.Windows.Forms.Label lblScoreGekkansi;
        private System.Windows.Forms.Label lblScoreNenkansi;
    }
}