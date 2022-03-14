
namespace WinFormsApp2
{
    partial class FormOption
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkGotoku = new System.Windows.Forms.CheckBox();
            this.chkGogyou = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkSangouKaikyoku = new System.Windows.Forms.CheckBox();
            this.chkDispNenun = new System.Windows.Forms.CheckBox();
            this.chkDispTaiun = new System.Windows.Forms.CheckBox();
            this.chkDispGetuun = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.chkDispToday = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkDispToday);
            this.groupBox1.Controls.Add(this.chkGotoku);
            this.groupBox1.Controls.Add(this.chkGogyou);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 152);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "初期表示設定";
            // 
            // chkGotoku
            // 
            this.chkGotoku.AutoSize = true;
            this.chkGotoku.Location = new System.Drawing.Point(75, 47);
            this.chkGotoku.Name = "chkGotoku";
            this.chkGotoku.Size = new System.Drawing.Size(50, 19);
            this.chkGotoku.TabIndex = 5;
            this.chkGotoku.Text = "五徳";
            this.chkGotoku.UseVisualStyleBackColor = true;
            this.chkGotoku.CheckedChanged += new System.EventHandler(this.chkGotoku_CheckedChanged);
            // 
            // chkGogyou
            // 
            this.chkGogyou.AutoSize = true;
            this.chkGogyou.Location = new System.Drawing.Point(19, 47);
            this.chkGogyou.Name = "chkGogyou";
            this.chkGogyou.Size = new System.Drawing.Size(50, 19);
            this.chkGogyou.TabIndex = 4;
            this.chkGogyou.Text = "五行";
            this.chkGogyou.UseVisualStyleBackColor = true;
            this.chkGogyou.CheckedChanged += new System.EventHandler(this.chkGogyou_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkSangouKaikyoku);
            this.groupBox2.Controls.Add(this.chkDispNenun);
            this.groupBox2.Controls.Add(this.chkDispTaiun);
            this.groupBox2.Controls.Add(this.chkDispGetuun);
            this.groupBox2.Location = new System.Drawing.Point(19, 72);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 68);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "後天運";
            // 
            // chkSangouKaikyoku
            // 
            this.chkSangouKaikyoku.AutoSize = true;
            this.chkSangouKaikyoku.Location = new System.Drawing.Point(20, 38);
            this.chkSangouKaikyoku.Name = "chkSangouKaikyoku";
            this.chkSangouKaikyoku.Size = new System.Drawing.Size(116, 19);
            this.chkSangouKaikyoku.TabIndex = 3;
            this.chkSangouKaikyoku.Text = "三合会局・方三位";
            this.chkSangouKaikyoku.UseVisualStyleBackColor = true;
            // 
            // chkDispNenun
            // 
            this.chkDispNenun.AutoSize = true;
            this.chkDispNenun.Location = new System.Drawing.Point(76, 13);
            this.chkDispNenun.Name = "chkDispNenun";
            this.chkDispNenun.Size = new System.Drawing.Size(50, 19);
            this.chkDispNenun.TabIndex = 1;
            this.chkDispNenun.Text = "年運";
            this.chkDispNenun.UseVisualStyleBackColor = true;
            // 
            // chkDispTaiun
            // 
            this.chkDispTaiun.AutoSize = true;
            this.chkDispTaiun.Location = new System.Drawing.Point(132, 13);
            this.chkDispTaiun.Name = "chkDispTaiun";
            this.chkDispTaiun.Size = new System.Drawing.Size(50, 19);
            this.chkDispTaiun.TabIndex = 2;
            this.chkDispTaiun.Text = "大運";
            this.chkDispTaiun.UseVisualStyleBackColor = true;
            // 
            // chkDispGetuun
            // 
            this.chkDispGetuun.AutoSize = true;
            this.chkDispGetuun.Location = new System.Drawing.Point(20, 13);
            this.chkDispGetuun.Name = "chkDispGetuun";
            this.chkDispGetuun.Size = new System.Drawing.Size(50, 19);
            this.chkDispGetuun.TabIndex = 0;
            this.chkDispGetuun.Text = "月運";
            this.chkDispGetuun.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(116, 161);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(65, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(187, 161);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(65, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "キャンセル";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // chkDispToday
            // 
            this.chkDispToday.AutoSize = true;
            this.chkDispToday.Location = new System.Drawing.Point(19, 22);
            this.chkDispToday.Name = "chkDispToday";
            this.chkDispToday.Size = new System.Drawing.Size(84, 19);
            this.chkDispToday.TabIndex = 6;
            this.chkDispToday.Text = "今日へ移動";
            this.chkDispToday.UseVisualStyleBackColor = true;
            // 
            // FormOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 194);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormOption";
            this.Text = "FormOption";
            this.Load += new System.EventHandler(this.FormOption_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkDispTaiun;
        private System.Windows.Forms.CheckBox chkDispNenun;
        private System.Windows.Forms.CheckBox chkDispGetuun;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkSangouKaikyoku;
        private System.Windows.Forms.CheckBox chkGotoku;
        private System.Windows.Forms.CheckBox chkGogyou;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox chkDispToday;
    }
}