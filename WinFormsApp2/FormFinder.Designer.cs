
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.radRittin = new System.Windows.Forms.RadioButton();
            this.chkIncludeGetuun = new System.Windows.Forms.CheckBox();
            this.radNattin = new System.Windows.Forms.RadioButton();
            this.chkTenchusatu = new System.Windows.Forms.CheckBox();
            this.radKyakkaHoukai = new System.Windows.Forms.RadioButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.radSameAkkkansi = new System.Windows.Forms.RadioButton();
            this.radSameNikkansi = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbTargetGroup = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbGroup = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbPerson = new System.Windows.Forms.ComboBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
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
            this.lstFindResult.Location = new System.Drawing.Point(0, 129);
            this.lstFindResult.MultiSelect = false;
            this.lstFindResult.Name = "lstFindResult";
            this.lstFindResult.OwnerDraw = true;
            this.lstFindResult.Size = new System.Drawing.Size(511, 303);
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
            this.contextMenuStrip1.Size = new System.Drawing.Size(299, 70);
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
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cmbGroup);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cmbPerson);
            this.panel1.Controls.Add(this.btnFind);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(511, 123);
            this.panel1.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(199, -1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(307, 119);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.radRittin);
            this.tabPage1.Controls.Add(this.chkIncludeGetuun);
            this.tabPage1.Controls.Add(this.radNattin);
            this.tabPage1.Controls.Add(this.chkTenchusatu);
            this.tabPage1.Controls.Add(this.radKyakkaHoukai);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(299, 91);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "自身検索";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // radRittin
            // 
            this.radRittin.AutoSize = true;
            this.radRittin.Location = new System.Drawing.Point(9, 25);
            this.radRittin.Name = "radRittin";
            this.radRittin.Size = new System.Drawing.Size(93, 19);
            this.radRittin.TabIndex = 1;
            this.radRittin.TabStop = true;
            this.radRittin.Text = "律音、準律音";
            this.radRittin.UseVisualStyleBackColor = true;
            this.radRittin.CheckedChanged += new System.EventHandler(this.radRittin_CheckedChanged);
            // 
            // chkIncludeGetuun
            // 
            this.chkIncludeGetuun.AutoSize = true;
            this.chkIncludeGetuun.Location = new System.Drawing.Point(108, 6);
            this.chkIncludeGetuun.Name = "chkIncludeGetuun";
            this.chkIncludeGetuun.Size = new System.Drawing.Size(90, 19);
            this.chkIncludeGetuun.TabIndex = 8;
            this.chkIncludeGetuun.Text = "月運を含める";
            this.chkIncludeGetuun.UseVisualStyleBackColor = true;
            // 
            // radNattin
            // 
            this.radNattin.AutoSize = true;
            this.radNattin.Location = new System.Drawing.Point(9, 6);
            this.radNattin.Name = "radNattin";
            this.radNattin.Size = new System.Drawing.Size(93, 19);
            this.radNattin.TabIndex = 0;
            this.radNattin.TabStop = true;
            this.radNattin.Text = "納音、準納音";
            this.radNattin.UseVisualStyleBackColor = true;
            this.radNattin.CheckedChanged += new System.EventHandler(this.radNattin_CheckedChanged);
            // 
            // chkTenchusatu
            // 
            this.chkTenchusatu.AutoSize = true;
            this.chkTenchusatu.Location = new System.Drawing.Point(108, 26);
            this.chkTenchusatu.Name = "chkTenchusatu";
            this.chkTenchusatu.Size = new System.Drawing.Size(107, 19);
            this.chkTenchusatu.TabIndex = 7;
            this.chkTenchusatu.Text = "天中殺のみ検索";
            this.chkTenchusatu.UseVisualStyleBackColor = true;
            // 
            // radKyakkaHoukai
            // 
            this.radKyakkaHoukai.AutoSize = true;
            this.radKyakkaHoukai.Location = new System.Drawing.Point(9, 43);
            this.radKyakkaHoukai.Name = "radKyakkaHoukai";
            this.radKyakkaHoukai.Size = new System.Drawing.Size(73, 19);
            this.radKyakkaHoukai.TabIndex = 1;
            this.radKyakkaHoukai.TabStop = true;
            this.radKyakkaHoukai.Text = "脚下崩壊";
            this.radKyakkaHoukai.UseVisualStyleBackColor = true;
            this.radKyakkaHoukai.CheckedChanged += new System.EventHandler(this.radKyakkaHoukai_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.radSameAkkkansi);
            this.tabPage2.Controls.Add(this.radSameNikkansi);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.cmbTargetGroup);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(299, 91);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "メンバー検索";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // radSameAkkkansi
            // 
            this.radSameAkkkansi.AutoSize = true;
            this.radSameAkkkansi.Location = new System.Drawing.Point(13, 60);
            this.radSameAkkkansi.Name = "radSameAkkkansi";
            this.radSameAkkkansi.Size = new System.Drawing.Size(263, 19);
            this.radSameAkkkansi.TabIndex = 9;
            this.radSameAkkkansi.Text = "自身の(日、月、年)干支を日干支に持つ人を検索";
            this.radSameAkkkansi.UseVisualStyleBackColor = true;
            // 
            // radSameNikkansi
            // 
            this.radSameNikkansi.AutoSize = true;
            this.radSameNikkansi.Checked = true;
            this.radSameNikkansi.Location = new System.Drawing.Point(13, 35);
            this.radSameNikkansi.Name = "radSameNikkansi";
            this.radSameNikkansi.Size = new System.Drawing.Size(224, 19);
            this.radSameNikkansi.TabIndex = 8;
            this.radSameNikkansi.TabStop = true;
            this.radSameNikkansi.Text = "自身の日干支と同じ干支を持つ人を検索";
            this.radSameNikkansi.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "対象グループ";
            // 
            // cmbTargetGroup
            // 
            this.cmbTargetGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTargetGroup.FormattingEnabled = true;
            this.cmbTargetGroup.Location = new System.Drawing.Point(79, 6);
            this.cmbTargetGroup.Name = "cmbTargetGroup";
            this.cmbTargetGroup.Size = new System.Drawing.Size(126, 23);
            this.cmbTargetGroup.TabIndex = 6;
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
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFind.Location = new System.Drawing.Point(56, 88);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(126, 26);
            this.btnFind.TabIndex = 2;
            this.btnFind.Text = "検索";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Location = new System.Drawing.Point(0, 431);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(511, 19);
            this.lblStatus.TabIndex = 3;
            // 
            // FormFinder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 450);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lstFindResult);
            this.MinimumSize = new System.Drawing.Size(438, 200);
            this.Name = "FormFinder";
            this.Text = "検索";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormFinder_FormClosing);
            this.Load += new System.EventHandler(this.FormSerch_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
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
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbTargetGroup;
        private System.Windows.Forms.RadioButton radSameNikkansi;
        private System.Windows.Forms.RadioButton radSameAkkkansi;
    }
}