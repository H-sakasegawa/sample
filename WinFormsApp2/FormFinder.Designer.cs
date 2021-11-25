
namespace WinFormsApp2
{
    partial class FormFinder
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
            this.components = new System.ComponentModel.Container();
            this.lstFindResult = new WinFormsApp2.ListViewEx();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuJumpForKan = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuJumpForSi = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkIncludeGetuun = new System.Windows.Forms.CheckBox();
            this.chkTenchusatu = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbGroup = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbPerson = new System.Windows.Forms.ComboBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.radKyakkaHoukai = new System.Windows.Forms.RadioButton();
            this.radRittin = new System.Windows.Forms.RadioButton();
            this.radNattin = new System.Windows.Forms.RadioButton();
            this.lblStatus = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstFindResult
            // 
            this.lstFindResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFindResult.ContextMenuStrip = this.contextMenuStrip1;
            this.lstFindResult.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lstFindResult.FullRowSelect = true;
            this.lstFindResult.GridLines = true;
            this.lstFindResult.HideSelection = false;
            this.lstFindResult.Location = new System.Drawing.Point(0, 103);
            this.lstFindResult.MultiSelect = false;
            this.lstFindResult.Name = "lstFindResult";
            this.lstFindResult.OwnerDraw = true;
            this.lstFindResult.Size = new System.Drawing.Size(503, 329);
            this.lstFindResult.TabIndex = 1;
            this.lstFindResult.UseCompatibleStateImageBehavior = false;
            this.lstFindResult.View = System.Windows.Forms.View.Details;
            this.lstFindResult.SelectedIndexChanged += new System.EventHandler(this.lstFindResult_SelectedIndexChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuJumpForKan,
            this.mnuJumpForSi});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(299, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // mnuJumpForKan
            // 
            this.mnuJumpForKan.Name = "mnuJumpForKan";
            this.mnuJumpForKan.Size = new System.Drawing.Size(298, 22);
            this.mnuJumpForKan.Text = "現在の大運、年運の干の出現位置ににジャンプ";
            this.mnuJumpForKan.Click += new System.EventHandler(this.mnuJumpForKan_Click);
            // 
            // mnuJumpForSi
            // 
            this.mnuJumpForSi.Name = "mnuJumpForSi";
            this.mnuJumpForSi.Size = new System.Drawing.Size(298, 22);
            this.mnuJumpForSi.Text = "現在の大運、年運の支の出現位置ににジャンプ";
            this.mnuJumpForSi.Click += new System.EventHandler(this.mnuJumpForSi_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.chkIncludeGetuun);
            this.panel1.Controls.Add(this.chkTenchusatu);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cmbGroup);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cmbPerson);
            this.panel1.Controls.Add(this.btnFind);
            this.panel1.Controls.Add(this.radKyakkaHoukai);
            this.panel1.Controls.Add(this.radRittin);
            this.panel1.Controls.Add(this.radNattin);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(503, 100);
            this.panel1.TabIndex = 2;
            // 
            // chkIncludeGetuun
            // 
            this.chkIncludeGetuun.AutoSize = true;
            this.chkIncludeGetuun.Location = new System.Drawing.Point(309, 8);
            this.chkIncludeGetuun.Name = "chkIncludeGetuun";
            this.chkIncludeGetuun.Size = new System.Drawing.Size(90, 19);
            this.chkIncludeGetuun.TabIndex = 8;
            this.chkIncludeGetuun.Text = "月運を含める";
            this.chkIncludeGetuun.UseVisualStyleBackColor = true;
            // 
            // chkTenchusatu
            // 
            this.chkTenchusatu.AutoSize = true;
            this.chkTenchusatu.Location = new System.Drawing.Point(309, 28);
            this.chkTenchusatu.Name = "chkTenchusatu";
            this.chkTenchusatu.Size = new System.Drawing.Size(62, 19);
            this.chkTenchusatu.TabIndex = 7;
            this.chkTenchusatu.Text = "天中殺";
            this.chkTenchusatu.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "グループ";
            // 
            // cmbGroup
            // 
            this.cmbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroup.FormattingEnabled = true;
            this.cmbGroup.Location = new System.Drawing.Point(56, 9);
            this.cmbGroup.Name = "cmbGroup";
            this.cmbGroup.Size = new System.Drawing.Size(126, 23);
            this.cmbGroup.TabIndex = 5;
            this.cmbGroup.SelectedIndexChanged += new System.EventHandler(this.cmbGroup_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "氏名";
            // 
            // cmbPerson
            // 
            this.cmbPerson.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPerson.FormattingEnabled = true;
            this.cmbPerson.Location = new System.Drawing.Point(56, 38);
            this.cmbPerson.Name = "cmbPerson";
            this.cmbPerson.Size = new System.Drawing.Size(126, 23);
            this.cmbPerson.TabIndex = 3;
            this.cmbPerson.SelectedIndexChanged += new System.EventHandler(this.cmbPeoson_SelectedIndexChanged);
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFind.Location = new System.Drawing.Point(404, 71);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(94, 26);
            this.btnFind.TabIndex = 2;
            this.btnFind.Text = "検索";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // radKyakkaHoukai
            // 
            this.radKyakkaHoukai.AutoSize = true;
            this.radKyakkaHoukai.Location = new System.Drawing.Point(210, 45);
            this.radKyakkaHoukai.Name = "radKyakkaHoukai";
            this.radKyakkaHoukai.Size = new System.Drawing.Size(73, 19);
            this.radKyakkaHoukai.TabIndex = 1;
            this.radKyakkaHoukai.TabStop = true;
            this.radKyakkaHoukai.Text = "脚下崩壊";
            this.radKyakkaHoukai.UseVisualStyleBackColor = true;
            this.radKyakkaHoukai.CheckedChanged += new System.EventHandler(this.radKyakkaHoukai_CheckedChanged);
            // 
            // radRittin
            // 
            this.radRittin.AutoSize = true;
            this.radRittin.Location = new System.Drawing.Point(210, 27);
            this.radRittin.Name = "radRittin";
            this.radRittin.Size = new System.Drawing.Size(93, 19);
            this.radRittin.TabIndex = 1;
            this.radRittin.TabStop = true;
            this.radRittin.Text = "律音、準律音";
            this.radRittin.UseVisualStyleBackColor = true;
            this.radRittin.CheckedChanged += new System.EventHandler(this.radRittin_CheckedChanged);
            // 
            // radNattin
            // 
            this.radNattin.AutoSize = true;
            this.radNattin.Location = new System.Drawing.Point(210, 8);
            this.radNattin.Name = "radNattin";
            this.radNattin.Size = new System.Drawing.Size(93, 19);
            this.radNattin.TabIndex = 0;
            this.radNattin.TabStop = true;
            this.radNattin.Text = "納音、準納音";
            this.radNattin.UseVisualStyleBackColor = true;
            this.radNattin.CheckedChanged += new System.EventHandler(this.radNattin_CheckedChanged);
            // 
            // lblStatus
            // 
            this.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Location = new System.Drawing.Point(0, 431);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(503, 19);
            this.lblStatus.TabIndex = 3;
            // 
            // FormFinder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 450);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lstFindResult);
            this.Name = "FormFinder";
            this.Text = "検索";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormFinder_FormClosing);
            this.Load += new System.EventHandler(this.FormSerch_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ListViewEx lstFindResult;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.RadioButton radKyakkaHoukai;
        private System.Windows.Forms.RadioButton radRittin;
        private System.Windows.Forms.RadioButton radNattin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbPerson;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbGroup;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuJumpForKan;
        private System.Windows.Forms.ToolStripMenuItem mnuJumpForSi;
        private System.Windows.Forms.CheckBox chkTenchusatu;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.CheckBox chkIncludeGetuun;
    }
}