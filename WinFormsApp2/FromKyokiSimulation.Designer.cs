
namespace WinFormsApp2
{
    partial class FromKyokiSimulation
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "1992",
            "99"}, -1);
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lvPatternNum = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(114, 2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(358, 359);
            this.flowLayoutPanel1.TabIndex = 50;
            // 
            // lvPatternNum
            // 
            this.lvPatternNum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvPatternNum.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvPatternNum.FullRowSelect = true;
            this.lvPatternNum.GridLines = true;
            this.lvPatternNum.HideSelection = false;
            this.lvPatternNum.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.lvPatternNum.Location = new System.Drawing.Point(1, 2);
            this.lvPatternNum.MultiSelect = false;
            this.lvPatternNum.Name = "lvPatternNum";
            this.lvPatternNum.Size = new System.Drawing.Size(110, 358);
            this.lvPatternNum.TabIndex = 51;
            this.lvPatternNum.UseCompatibleStateImageBehavior = false;
            this.lvPatternNum.View = System.Windows.Forms.View.Details;
            this.lvPatternNum.SelectedIndexChanged += new System.EventHandler(this.lvPatternNum_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "年";
            this.columnHeader1.Width = 40;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "変化数";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 48;
            // 
            // FromKyokiSimulation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 366);
            this.Controls.Add(this.lvPatternNum);
            this.Controls.Add(this.flowLayoutPanel1);
            this.MaximumSize = new System.Drawing.Size(490, 2048);
            this.MinimumSize = new System.Drawing.Size(490, 200);
            this.Name = "FromKyokiSimulation";
            this.Text = "虚気 変化パターン";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FromKyokiSimulation_FormClosing);
            this.Load += new System.EventHandler(this.FromKyokiSimulation_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.ListView lvPatternNum;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}