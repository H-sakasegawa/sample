
namespace WinFormsApp2
{
    partial class FormUnseiViewer
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cmbGroup = new System.Windows.Forms.ComboBox();
            this.chkListPerson = new System.Windows.Forms.CheckedListBox();
            this.lstDispItems = new System.Windows.Forms.ListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.txtMaxNenNum = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkDispBaseYearRange = new System.Windows.Forms.CheckBox();
            this.grdViewNenUn = new WinFormsApp2.DataGridViewEx();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewNenUn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbGroup
            // 
            this.cmbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroup.FormattingEnabled = true;
            this.cmbGroup.Location = new System.Drawing.Point(2, 11);
            this.cmbGroup.Name = "cmbGroup";
            this.cmbGroup.Size = new System.Drawing.Size(107, 23);
            this.cmbGroup.TabIndex = 1;
            this.cmbGroup.SelectedIndexChanged += new System.EventHandler(this.cmbGroup_SelectedIndexChanged);
            // 
            // chkListPerson
            // 
            this.chkListPerson.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.chkListPerson.CheckOnClick = true;
            this.chkListPerson.FormattingEnabled = true;
            this.chkListPerson.IntegralHeight = false;
            this.chkListPerson.Location = new System.Drawing.Point(2, 43);
            this.chkListPerson.Name = "chkListPerson";
            this.chkListPerson.Size = new System.Drawing.Size(107, 231);
            this.chkListPerson.TabIndex = 2;
            // 
            // lstDispItems
            // 
            this.lstDispItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lstDispItems.FormattingEnabled = true;
            this.lstDispItems.ItemHeight = 15;
            this.lstDispItems.Location = new System.Drawing.Point(2, 306);
            this.lstDispItems.Name = "lstDispItems";
            this.lstDispItems.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstDispItems.Size = new System.Drawing.Size(106, 184);
            this.lstDispItems.TabIndex = 3;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point(2, 277);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(54, 29);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "↓追加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemove.Location = new System.Drawing.Point(56, 277);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(54, 29);
            this.btnRemove.TabIndex = 5;
            this.btnRemove.Text = "↑削除";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // txtMaxNenNum
            // 
            this.txtMaxNenNum.Location = new System.Drawing.Point(174, 2);
            this.txtMaxNenNum.Name = "txtMaxNenNum";
            this.txtMaxNenNum.Size = new System.Drawing.Size(58, 23);
            this.txtMaxNenNum.TabIndex = 6;
            this.txtMaxNenNum.Leave += new System.EventHandler(this.txtMaxNenNum_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(131, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "年数：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(233, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "年";
            // 
            // chkDispBaseYearRange
            // 
            this.chkDispBaseYearRange.AutoSize = true;
            this.chkDispBaseYearRange.Location = new System.Drawing.Point(268, 3);
            this.chkDispBaseYearRange.Name = "chkDispBaseYearRange";
            this.chkDispBaseYearRange.Size = new System.Drawing.Size(165, 19);
            this.chkDispBaseYearRange.TabIndex = 9;
            this.chkDispBaseYearRange.Text = "基準メンバーの年範囲で表示";
            this.chkDispBaseYearRange.UseVisualStyleBackColor = true;
            this.chkDispBaseYearRange.CheckedChanged += new System.EventHandler(this.chkDispBaseYearRange_CheckedChanged);
            // 
            // grdViewNenUn
            // 
            this.grdViewNenUn.AllowUserToAddRows = false;
            this.grdViewNenUn.AllowUserToDeleteRows = false;
            this.grdViewNenUn.AllowUserToResizeRows = false;
            this.grdViewNenUn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.InactiveCaption;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdViewNenUn.DefaultCellStyle = dataGridViewCellStyle1;
            this.grdViewNenUn.Location = new System.Drawing.Point(15, 35);
            this.grdViewNenUn.MultiSelect = false;
            this.grdViewNenUn.Name = "grdViewNenUn";
            this.grdViewNenUn.RowTemplate.Height = 25;
            this.grdViewNenUn.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdViewNenUn.Size = new System.Drawing.Size(539, 140);
            this.grdViewNenUn.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitContainer1.Location = new System.Drawing.Point(116, 43);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grdViewNenUn);
            this.splitContainer1.Size = new System.Drawing.Size(993, 447);
            this.splitContainer1.SplitterDistance = 223;
            this.splitContainer1.TabIndex = 12;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(96, 57);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // trackBar1
            // 
            this.trackBar1.AutoSize = false;
            this.trackBar1.LargeChange = 1;
            this.trackBar1.Location = new System.Drawing.Point(469, 1);
            this.trackBar1.Maximum = 15;
            this.trackBar1.Minimum = 5;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(154, 33);
            this.trackBar1.TabIndex = 13;
            this.trackBar1.TickFrequency = 2;
            this.trackBar1.Value = 10;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(619, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(40, 23);
            this.button2.TabIndex = 15;
            this.button2.Text = "標準";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FormUnseiViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1121, 502);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.chkDispBaseYearRange);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMaxNenNum);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lstDispItems);
            this.Controls.Add(this.chkListPerson);
            this.Controls.Add(this.cmbGroup);
            this.Name = "FormUnseiViewer";
            this.Text = "年運比較表";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormUnseiViewer_FormClosed);
            this.Load += new System.EventHandler(this.FormUnseiViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdViewNenUn)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cmbGroup;
        private System.Windows.Forms.CheckedListBox chkListPerson;
        private System.Windows.Forms.ListBox lstDispItems;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.TextBox txtMaxNenNum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkDispBaseYearRange;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DataGridViewEx grdViewNenUn;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button button2;
    }
}