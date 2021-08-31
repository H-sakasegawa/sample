
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
            this.grdViewNenUn = new DataGridViewEx();
            this.cmbGroup = new System.Windows.Forms.ComboBox();
            this.chkListPerson = new System.Windows.Forms.CheckedListBox();
            this.lstDispItems = new System.Windows.Forms.ListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewNenUn)).BeginInit();
            this.SuspendLayout();
            // 
            // grdViewNenUn
            // 
            this.grdViewNenUn.AllowUserToAddRows = false;
            this.grdViewNenUn.AllowUserToDeleteRows = false;
            this.grdViewNenUn.AllowUserToResizeRows = false;
            this.grdViewNenUn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdViewNenUn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.InactiveCaption;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdViewNenUn.DefaultCellStyle = dataGridViewCellStyle1;
            this.grdViewNenUn.Location = new System.Drawing.Point(115, 12);
            this.grdViewNenUn.MultiSelect = false;
            this.grdViewNenUn.Name = "grdViewNenUn";
            this.grdViewNenUn.RowTemplate.Height = 25;
            this.grdViewNenUn.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdViewNenUn.Size = new System.Drawing.Size(994, 478);
            this.grdViewNenUn.TabIndex = 0;
            this.grdViewNenUn.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grdViewNenUn_CellFormatting);
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
            this.chkListPerson.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkListPerson_ItemCheck);
            // 
            // lstDispItems
            // 
            this.lstDispItems.FormattingEnabled = true;
            this.lstDispItems.ItemHeight = 15;
            this.lstDispItems.Location = new System.Drawing.Point(2, 306);
            this.lstDispItems.Name = "lstDispItems";
            this.lstDispItems.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstDispItems.Size = new System.Drawing.Size(106, 169);
            this.lstDispItems.TabIndex = 3;
            // 
            // btnAdd
            // 
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
            this.btnRemove.Location = new System.Drawing.Point(56, 277);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(54, 29);
            this.btnRemove.TabIndex = 5;
            this.btnRemove.Text = "↑削除";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // FormUnseiViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1121, 502);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lstDispItems);
            this.Controls.Add(this.chkListPerson);
            this.Controls.Add(this.cmbGroup);
            this.Controls.Add(this.grdViewNenUn);
            this.Name = "FormUnseiViewer";
            this.Text = "FormUnseiViewer";
            this.Load += new System.EventHandler(this.FormUnseiViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdViewNenUn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdViewNenUn;
        private System.Windows.Forms.ComboBox cmbGroup;
        private System.Windows.Forms.CheckedListBox chkListPerson;
        private System.Windows.Forms.ListBox lstDispItems;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
    }
}