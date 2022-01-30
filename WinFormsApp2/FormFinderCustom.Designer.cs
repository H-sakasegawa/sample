
namespace WinFormsApp2
{
    partial class FormFinderCustom
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
            this.lvFindResultDB = new WinFormsApp2.ListViewEx();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.cmbNenZoganHongen = new System.Windows.Forms.ComboBox();
            this.cmbNitiKan = new System.Windows.Forms.ComboBox();
            this.cmbNenTenchusatuSi = new System.Windows.Forms.ComboBox();
            this.cmbNitiTenchusatuKan = new System.Windows.Forms.ComboBox();
            this.cmbNenSi = new System.Windows.Forms.ComboBox();
            this.cmbNitiTenchusatuSi = new System.Windows.Forms.ComboBox();
            this.cmbNenTenchusatuKan = new System.Windows.Forms.ComboBox();
            this.cmbNitiZoganShogen = new System.Windows.Forms.ComboBox();
            this.cmbNenKan = new System.Windows.Forms.ComboBox();
            this.cmbNenZoganShogen = new System.Windows.Forms.ComboBox();
            this.cmbGetuSi = new System.Windows.Forms.ComboBox();
            this.cmbGetuZoganShogen = new System.Windows.Forms.ComboBox();
            this.cmbGetuKan = new System.Windows.Forms.ComboBox();
            this.cmbNitiSi = new System.Windows.Forms.ComboBox();
            this.cmbGetuZoganHongen = new System.Windows.Forms.ComboBox();
            this.cmbNitiZoganChugen = new System.Windows.Forms.ComboBox();
            this.cmbNenZoganChugen = new System.Windows.Forms.ComboBox();
            this.cmbNitiZoganHongen = new System.Windows.Forms.ComboBox();
            this.cmbGetuZoganChugen = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbJudaishuseiD = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.cmbJunidaijuseiC = new System.Windows.Forms.ComboBox();
            this.cmbJudaishuseiB = new System.Windows.Forms.ComboBox();
            this.cmbJudaishuseiE = new System.Windows.Forms.ComboBox();
            this.cmbJudaishuseiA = new System.Windows.Forms.ComboBox();
            this.cmbJunidaijuseiA = new System.Windows.Forms.ComboBox();
            this.cmbJudaishuseiC = new System.Windows.Forms.ComboBox();
            this.cmbJunidaijuseiB = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.cmbToYear = new System.Windows.Forms.ComboBox();
            this.cmbFromYear = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radOR = new System.Windows.Forms.RadioButton();
            this.radAnd = new System.Windows.Forms.RadioButton();
            this.btnFind = new System.Windows.Forms.Button();
            this.chkTargetDb = new System.Windows.Forms.CheckBox();
            this.chkTargetBirthday = new System.Windows.Forms.CheckBox();
            this.tabResult = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.lvFindResultBIRTHDAY = new WinFormsApp2.ListViewEx();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabResult.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvFindResultDB
            // 
            this.lvFindResultDB.Cursor = System.Windows.Forms.Cursors.Default;
            this.lvFindResultDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFindResultDB.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lvFindResultDB.FullRowSelect = true;
            this.lvFindResultDB.GridLines = true;
            this.lvFindResultDB.HideSelection = false;
            this.lvFindResultDB.Location = new System.Drawing.Point(3, 3);
            this.lvFindResultDB.MultiSelect = false;
            this.lvFindResultDB.Name = "lvFindResultDB";
            this.lvFindResultDB.OwnerDraw = true;
            this.lvFindResultDB.Size = new System.Drawing.Size(260, 274);
            this.lvFindResultDB.TabIndex = 1;
            this.lvFindResultDB.UseCompatibleStateImageBehavior = false;
            this.lvFindResultDB.View = System.Windows.Forms.View.Details;
            this.lvFindResultDB.SelectedIndexChanged += new System.EventHandler(this.lstFindResult_SelectedIndexChanged);
            // 
            // lblStatus
            // 
            this.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Location = new System.Drawing.Point(0, 406);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(596, 19);
            this.lblStatus.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.splitContainer1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(596, 406);
            this.panel2.TabIndex = 4;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(596, 406);
            this.splitContainer1.SplitterDistance = 318;
            this.splitContainer1.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(318, 406);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(310, 378);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "陰・陽占";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.cmbNenZoganHongen);
            this.groupBox3.Controls.Add(this.cmbNitiKan);
            this.groupBox3.Controls.Add(this.cmbNenTenchusatuSi);
            this.groupBox3.Controls.Add(this.cmbNitiTenchusatuKan);
            this.groupBox3.Controls.Add(this.cmbNenSi);
            this.groupBox3.Controls.Add(this.cmbNitiTenchusatuSi);
            this.groupBox3.Controls.Add(this.cmbNenTenchusatuKan);
            this.groupBox3.Controls.Add(this.cmbNitiZoganShogen);
            this.groupBox3.Controls.Add(this.cmbNenKan);
            this.groupBox3.Controls.Add(this.cmbNenZoganShogen);
            this.groupBox3.Controls.Add(this.cmbGetuSi);
            this.groupBox3.Controls.Add(this.cmbGetuZoganShogen);
            this.groupBox3.Controls.Add(this.cmbGetuKan);
            this.groupBox3.Controls.Add(this.cmbNitiSi);
            this.groupBox3.Controls.Add(this.cmbGetuZoganHongen);
            this.groupBox3.Controls.Add(this.cmbNitiZoganChugen);
            this.groupBox3.Controls.Add(this.cmbNenZoganChugen);
            this.groupBox3.Controls.Add(this.cmbNitiZoganHongen);
            this.groupBox3.Controls.Add(this.cmbGetuZoganChugen);
            this.groupBox3.Location = new System.Drawing.Point(6, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(298, 206);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "陰占";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(239, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(48, 21);
            this.button2.TabIndex = 19;
            this.button2.Text = "クリア";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // cmbNenZoganHongen
            // 
            this.cmbNenZoganHongen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNenZoganHongen.FormattingEnabled = true;
            this.cmbNenZoganHongen.Location = new System.Drawing.Point(176, 167);
            this.cmbNenZoganHongen.Name = "cmbNenZoganHongen";
            this.cmbNenZoganHongen.Size = new System.Drawing.Size(48, 23);
            this.cmbNenZoganHongen.TabIndex = 18;
            // 
            // cmbNitiKan
            // 
            this.cmbNitiKan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNitiKan.FormattingEnabled = true;
            this.cmbNitiKan.Location = new System.Drawing.Point(68, 42);
            this.cmbNitiKan.Name = "cmbNitiKan";
            this.cmbNitiKan.Size = new System.Drawing.Size(48, 23);
            this.cmbNitiKan.TabIndex = 2;
            // 
            // cmbNenTenchusatuSi
            // 
            this.cmbNenTenchusatuSi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNenTenchusatuSi.FormattingEnabled = true;
            this.cmbNenTenchusatuSi.Location = new System.Drawing.Point(239, 71);
            this.cmbNenTenchusatuSi.Name = "cmbNenTenchusatuSi";
            this.cmbNenTenchusatuSi.Size = new System.Drawing.Size(48, 23);
            this.cmbNenTenchusatuSi.TabIndex = 9;
            // 
            // cmbNitiTenchusatuKan
            // 
            this.cmbNitiTenchusatuKan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNitiTenchusatuKan.FormattingEnabled = true;
            this.cmbNitiTenchusatuKan.Location = new System.Drawing.Point(7, 42);
            this.cmbNitiTenchusatuKan.Name = "cmbNitiTenchusatuKan";
            this.cmbNitiTenchusatuKan.Size = new System.Drawing.Size(48, 23);
            this.cmbNitiTenchusatuKan.TabIndex = 0;
            // 
            // cmbNenSi
            // 
            this.cmbNenSi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNenSi.FormattingEnabled = true;
            this.cmbNenSi.Location = new System.Drawing.Point(176, 71);
            this.cmbNenSi.Name = "cmbNenSi";
            this.cmbNenSi.Size = new System.Drawing.Size(48, 23);
            this.cmbNenSi.TabIndex = 7;
            // 
            // cmbNitiTenchusatuSi
            // 
            this.cmbNitiTenchusatuSi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNitiTenchusatuSi.FormattingEnabled = true;
            this.cmbNitiTenchusatuSi.Location = new System.Drawing.Point(7, 71);
            this.cmbNitiTenchusatuSi.Name = "cmbNitiTenchusatuSi";
            this.cmbNitiTenchusatuSi.Size = new System.Drawing.Size(48, 23);
            this.cmbNitiTenchusatuSi.TabIndex = 1;
            // 
            // cmbNenTenchusatuKan
            // 
            this.cmbNenTenchusatuKan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNenTenchusatuKan.FormattingEnabled = true;
            this.cmbNenTenchusatuKan.Location = new System.Drawing.Point(239, 42);
            this.cmbNenTenchusatuKan.Name = "cmbNenTenchusatuKan";
            this.cmbNenTenchusatuKan.Size = new System.Drawing.Size(48, 23);
            this.cmbNenTenchusatuKan.TabIndex = 8;
            // 
            // cmbNitiZoganShogen
            // 
            this.cmbNitiZoganShogen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNitiZoganShogen.FormattingEnabled = true;
            this.cmbNitiZoganShogen.Location = new System.Drawing.Point(68, 109);
            this.cmbNitiZoganShogen.Name = "cmbNitiZoganShogen";
            this.cmbNitiZoganShogen.Size = new System.Drawing.Size(48, 23);
            this.cmbNitiZoganShogen.TabIndex = 10;
            // 
            // cmbNenKan
            // 
            this.cmbNenKan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNenKan.FormattingEnabled = true;
            this.cmbNenKan.Location = new System.Drawing.Point(176, 42);
            this.cmbNenKan.Name = "cmbNenKan";
            this.cmbNenKan.Size = new System.Drawing.Size(48, 23);
            this.cmbNenKan.TabIndex = 6;
            // 
            // cmbNenZoganShogen
            // 
            this.cmbNenZoganShogen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNenZoganShogen.FormattingEnabled = true;
            this.cmbNenZoganShogen.Location = new System.Drawing.Point(176, 109);
            this.cmbNenZoganShogen.Name = "cmbNenZoganShogen";
            this.cmbNenZoganShogen.Size = new System.Drawing.Size(48, 23);
            this.cmbNenZoganShogen.TabIndex = 16;
            // 
            // cmbGetuSi
            // 
            this.cmbGetuSi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGetuSi.FormattingEnabled = true;
            this.cmbGetuSi.Location = new System.Drawing.Point(122, 71);
            this.cmbGetuSi.Name = "cmbGetuSi";
            this.cmbGetuSi.Size = new System.Drawing.Size(48, 23);
            this.cmbGetuSi.TabIndex = 5;
            // 
            // cmbGetuZoganShogen
            // 
            this.cmbGetuZoganShogen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGetuZoganShogen.FormattingEnabled = true;
            this.cmbGetuZoganShogen.Location = new System.Drawing.Point(122, 109);
            this.cmbGetuZoganShogen.Name = "cmbGetuZoganShogen";
            this.cmbGetuZoganShogen.Size = new System.Drawing.Size(48, 23);
            this.cmbGetuZoganShogen.TabIndex = 13;
            // 
            // cmbGetuKan
            // 
            this.cmbGetuKan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGetuKan.FormattingEnabled = true;
            this.cmbGetuKan.Location = new System.Drawing.Point(122, 42);
            this.cmbGetuKan.Name = "cmbGetuKan";
            this.cmbGetuKan.Size = new System.Drawing.Size(48, 23);
            this.cmbGetuKan.TabIndex = 4;
            // 
            // cmbNitiSi
            // 
            this.cmbNitiSi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNitiSi.FormattingEnabled = true;
            this.cmbNitiSi.Location = new System.Drawing.Point(68, 71);
            this.cmbNitiSi.Name = "cmbNitiSi";
            this.cmbNitiSi.Size = new System.Drawing.Size(48, 23);
            this.cmbNitiSi.TabIndex = 3;
            // 
            // cmbGetuZoganHongen
            // 
            this.cmbGetuZoganHongen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGetuZoganHongen.FormattingEnabled = true;
            this.cmbGetuZoganHongen.Location = new System.Drawing.Point(122, 167);
            this.cmbGetuZoganHongen.Name = "cmbGetuZoganHongen";
            this.cmbGetuZoganHongen.Size = new System.Drawing.Size(48, 23);
            this.cmbGetuZoganHongen.TabIndex = 15;
            // 
            // cmbNitiZoganChugen
            // 
            this.cmbNitiZoganChugen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNitiZoganChugen.FormattingEnabled = true;
            this.cmbNitiZoganChugen.Location = new System.Drawing.Point(68, 138);
            this.cmbNitiZoganChugen.Name = "cmbNitiZoganChugen";
            this.cmbNitiZoganChugen.Size = new System.Drawing.Size(48, 23);
            this.cmbNitiZoganChugen.TabIndex = 11;
            // 
            // cmbNenZoganChugen
            // 
            this.cmbNenZoganChugen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNenZoganChugen.FormattingEnabled = true;
            this.cmbNenZoganChugen.Location = new System.Drawing.Point(176, 138);
            this.cmbNenZoganChugen.Name = "cmbNenZoganChugen";
            this.cmbNenZoganChugen.Size = new System.Drawing.Size(48, 23);
            this.cmbNenZoganChugen.TabIndex = 17;
            // 
            // cmbNitiZoganHongen
            // 
            this.cmbNitiZoganHongen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNitiZoganHongen.FormattingEnabled = true;
            this.cmbNitiZoganHongen.Location = new System.Drawing.Point(68, 167);
            this.cmbNitiZoganHongen.Name = "cmbNitiZoganHongen";
            this.cmbNitiZoganHongen.Size = new System.Drawing.Size(48, 23);
            this.cmbNitiZoganHongen.TabIndex = 12;
            // 
            // cmbGetuZoganChugen
            // 
            this.cmbGetuZoganChugen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGetuZoganChugen.FormattingEnabled = true;
            this.cmbGetuZoganChugen.Location = new System.Drawing.Point(122, 138);
            this.cmbGetuZoganChugen.Name = "cmbGetuZoganChugen";
            this.cmbGetuZoganChugen.Size = new System.Drawing.Size(48, 23);
            this.cmbGetuZoganChugen.TabIndex = 14;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbJudaishuseiD);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.cmbJunidaijuseiC);
            this.groupBox2.Controls.Add(this.cmbJudaishuseiB);
            this.groupBox2.Controls.Add(this.cmbJudaishuseiE);
            this.groupBox2.Controls.Add(this.cmbJudaishuseiA);
            this.groupBox2.Controls.Add(this.cmbJunidaijuseiA);
            this.groupBox2.Controls.Add(this.cmbJudaishuseiC);
            this.groupBox2.Controls.Add(this.cmbJunidaijuseiB);
            this.groupBox2.Location = new System.Drawing.Point(6, 214);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(298, 114);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "陽占";
            // 
            // cmbJudaishuseiD
            // 
            this.cmbJudaishuseiD.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbJudaishuseiD.FormattingEnabled = true;
            this.cmbJudaishuseiD.Location = new System.Drawing.Point(83, 16);
            this.cmbJudaishuseiD.Name = "cmbJudaishuseiD";
            this.cmbJudaishuseiD.Size = new System.Drawing.Size(68, 23);
            this.cmbJudaishuseiD.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(244, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(43, 21);
            this.button1.TabIndex = 8;
            this.button1.Text = "クリア";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cmbJunidaijuseiC
            // 
            this.cmbJunidaijuseiC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbJunidaijuseiC.FormattingEnabled = true;
            this.cmbJunidaijuseiC.Location = new System.Drawing.Point(9, 74);
            this.cmbJunidaijuseiC.Name = "cmbJunidaijuseiC";
            this.cmbJunidaijuseiC.Size = new System.Drawing.Size(68, 23);
            this.cmbJunidaijuseiC.TabIndex = 1;
            // 
            // cmbJudaishuseiB
            // 
            this.cmbJudaishuseiB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbJudaishuseiB.FormattingEnabled = true;
            this.cmbJudaishuseiB.Location = new System.Drawing.Point(83, 45);
            this.cmbJudaishuseiB.Name = "cmbJudaishuseiB";
            this.cmbJudaishuseiB.Size = new System.Drawing.Size(68, 23);
            this.cmbJudaishuseiB.TabIndex = 3;
            // 
            // cmbJudaishuseiE
            // 
            this.cmbJudaishuseiE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbJudaishuseiE.FormattingEnabled = true;
            this.cmbJudaishuseiE.Location = new System.Drawing.Point(83, 74);
            this.cmbJudaishuseiE.Name = "cmbJudaishuseiE";
            this.cmbJudaishuseiE.Size = new System.Drawing.Size(68, 23);
            this.cmbJudaishuseiE.TabIndex = 4;
            // 
            // cmbJudaishuseiA
            // 
            this.cmbJudaishuseiA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbJudaishuseiA.FormattingEnabled = true;
            this.cmbJudaishuseiA.Location = new System.Drawing.Point(9, 45);
            this.cmbJudaishuseiA.Name = "cmbJudaishuseiA";
            this.cmbJudaishuseiA.Size = new System.Drawing.Size(68, 23);
            this.cmbJudaishuseiA.TabIndex = 0;
            // 
            // cmbJunidaijuseiA
            // 
            this.cmbJunidaijuseiA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbJunidaijuseiA.FormattingEnabled = true;
            this.cmbJunidaijuseiA.Location = new System.Drawing.Point(157, 16);
            this.cmbJunidaijuseiA.Name = "cmbJunidaijuseiA";
            this.cmbJunidaijuseiA.Size = new System.Drawing.Size(68, 23);
            this.cmbJunidaijuseiA.TabIndex = 5;
            // 
            // cmbJudaishuseiC
            // 
            this.cmbJudaishuseiC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbJudaishuseiC.FormattingEnabled = true;
            this.cmbJudaishuseiC.Location = new System.Drawing.Point(157, 45);
            this.cmbJudaishuseiC.Name = "cmbJudaishuseiC";
            this.cmbJudaishuseiC.Size = new System.Drawing.Size(68, 23);
            this.cmbJudaishuseiC.TabIndex = 6;
            // 
            // cmbJunidaijuseiB
            // 
            this.cmbJunidaijuseiB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbJunidaijuseiB.FormattingEnabled = true;
            this.cmbJunidaijuseiB.Location = new System.Drawing.Point(157, 74);
            this.cmbJunidaijuseiB.Name = "cmbJunidaijuseiB";
            this.cmbJunidaijuseiB.Size = new System.Drawing.Size(68, 23);
            this.cmbJunidaijuseiB.TabIndex = 7;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(310, 378);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "詳細条件";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.cmbToYear);
            this.splitContainer2.Panel1.Controls.Add(this.cmbFromYear);
            this.splitContainer2.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer2.Panel1.Controls.Add(this.btnFind);
            this.splitContainer2.Panel1.Controls.Add(this.chkTargetDb);
            this.splitContainer2.Panel1.Controls.Add(this.chkTargetBirthday);
            this.splitContainer2.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabResult);
            this.splitContainer2.Size = new System.Drawing.Size(274, 406);
            this.splitContainer2.SplitterDistance = 94;
            this.splitContainer2.TabIndex = 2;
            // 
            // cmbToYear
            // 
            this.cmbToYear.FormattingEnabled = true;
            this.cmbToYear.Location = new System.Drawing.Point(174, 58);
            this.cmbToYear.Name = "cmbToYear";
            this.cmbToYear.Size = new System.Drawing.Size(64, 23);
            this.cmbToYear.TabIndex = 4;
            // 
            // cmbFromYear
            // 
            this.cmbFromYear.FormattingEnabled = true;
            this.cmbFromYear.Location = new System.Drawing.Point(94, 58);
            this.cmbFromYear.Name = "cmbFromYear";
            this.cmbFromYear.Size = new System.Drawing.Size(64, 23);
            this.cmbFromYear.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radOR);
            this.groupBox1.Controls.Add(this.radAnd);
            this.groupBox1.Location = new System.Drawing.Point(11, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(73, 58);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "条件";
            // 
            // radOR
            // 
            this.radOR.AutoSize = true;
            this.radOR.Location = new System.Drawing.Point(10, 35);
            this.radOR.Name = "radOR";
            this.radOR.Size = new System.Drawing.Size(41, 19);
            this.radOR.TabIndex = 1;
            this.radOR.Text = "OR";
            this.radOR.UseVisualStyleBackColor = true;
            // 
            // radAnd
            // 
            this.radAnd.AutoSize = true;
            this.radAnd.Checked = true;
            this.radAnd.Location = new System.Drawing.Point(10, 16);
            this.radAnd.Name = "radAnd";
            this.radAnd.Size = new System.Drawing.Size(50, 19);
            this.radAnd.TabIndex = 0;
            this.radAnd.TabStop = true;
            this.radAnd.Text = "AND";
            this.radAnd.UseVisualStyleBackColor = true;
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFind.Location = new System.Drawing.Point(194, 8);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(72, 41);
            this.btnFind.TabIndex = 3;
            this.btnFind.Text = "検索";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click_1);
            // 
            // chkTargetDb
            // 
            this.chkTargetDb.AutoSize = true;
            this.chkTargetDb.Location = new System.Drawing.Point(90, 12);
            this.chkTargetDb.Name = "chkTargetDb";
            this.chkTargetDb.Size = new System.Drawing.Size(100, 19);
            this.chkTargetDb.TabIndex = 1;
            this.chkTargetDb.Text = "登録データ検索";
            this.chkTargetDb.UseVisualStyleBackColor = true;
            // 
            // chkTargetBirthday
            // 
            this.chkTargetBirthday.AutoSize = true;
            this.chkTargetBirthday.Location = new System.Drawing.Point(90, 31);
            this.chkTargetBirthday.Name = "chkTargetBirthday";
            this.chkTargetBirthday.Size = new System.Drawing.Size(98, 19);
            this.chkTargetBirthday.TabIndex = 2;
            this.chkTargetBirthday.Text = "生年月日検索";
            this.chkTargetBirthday.UseVisualStyleBackColor = true;
            this.chkTargetBirthday.CheckedChanged += new System.EventHandler(this.chkTargetBirthday_CheckedChanged);
            // 
            // tabResult
            // 
            this.tabResult.Controls.Add(this.tabPage3);
            this.tabResult.Controls.Add(this.tabPage4);
            this.tabResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabResult.Location = new System.Drawing.Point(0, 0);
            this.tabResult.Name = "tabResult";
            this.tabResult.SelectedIndex = 0;
            this.tabResult.Size = new System.Drawing.Size(274, 308);
            this.tabResult.TabIndex = 2;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.lvFindResultDB);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(266, 280);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "登録データ";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.lvFindResultBIRTHDAY);
            this.tabPage4.Location = new System.Drawing.Point(4, 24);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(266, 280);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "生年月日";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // lvFindResultBIRTHDAY
            // 
            this.lvFindResultBIRTHDAY.Cursor = System.Windows.Forms.Cursors.Default;
            this.lvFindResultBIRTHDAY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFindResultBIRTHDAY.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lvFindResultBIRTHDAY.FullRowSelect = true;
            this.lvFindResultBIRTHDAY.GridLines = true;
            this.lvFindResultBIRTHDAY.HideSelection = false;
            this.lvFindResultBIRTHDAY.Location = new System.Drawing.Point(3, 3);
            this.lvFindResultBIRTHDAY.MultiSelect = false;
            this.lvFindResultBIRTHDAY.Name = "lvFindResultBIRTHDAY";
            this.lvFindResultBIRTHDAY.OwnerDraw = true;
            this.lvFindResultBIRTHDAY.Size = new System.Drawing.Size(260, 274);
            this.lvFindResultBIRTHDAY.TabIndex = 2;
            this.lvFindResultBIRTHDAY.UseCompatibleStateImageBehavior = false;
            this.lvFindResultBIRTHDAY.View = System.Windows.Forms.View.Details;
            this.lvFindResultBIRTHDAY.SelectedIndexChanged += new System.EventHandler(this.lvFindResultBIRTHDAY_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Yu Gothic UI", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(38, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(216, 50);
            this.label1.TabIndex = 0;
            this.label1.Text = "ここはまだまだ";
            // 
            // FormFinderCustom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 425);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lblStatus);
            this.MinimumSize = new System.Drawing.Size(438, 200);
            this.Name = "FormFinderCustom";
            this.Text = "検索";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormFinder_FormClosing);
            this.Load += new System.EventHandler(this.FormSerch_Load);
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabResult.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ListViewEx lvFindResultDB;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ComboBox cmbNenTenchusatuSi;
        private System.Windows.Forms.ComboBox cmbNenSi;
        private System.Windows.Forms.ComboBox cmbNenTenchusatuKan;
        private System.Windows.Forms.ComboBox cmbNenKan;
        private System.Windows.Forms.ComboBox cmbGetuSi;
        private System.Windows.Forms.ComboBox cmbGetuKan;
        private System.Windows.Forms.ComboBox cmbGetuZoganHongen;
        private System.Windows.Forms.ComboBox cmbNenZoganHongen;
        private System.Windows.Forms.ComboBox cmbNitiZoganHongen;
        private System.Windows.Forms.ComboBox cmbGetuZoganChugen;
        private System.Windows.Forms.ComboBox cmbNenZoganChugen;
        private System.Windows.Forms.ComboBox cmbNitiZoganChugen;
        private System.Windows.Forms.ComboBox cmbNitiSi;
        private System.Windows.Forms.ComboBox cmbGetuZoganShogen;
        private System.Windows.Forms.ComboBox cmbNenZoganShogen;
        private System.Windows.Forms.ComboBox cmbNitiZoganShogen;
        private System.Windows.Forms.ComboBox cmbNitiTenchusatuSi;
        private System.Windows.Forms.ComboBox cmbNitiTenchusatuKan;
        private System.Windows.Forms.ComboBox cmbNitiKan;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.CheckBox chkTargetDb;
        private System.Windows.Forms.CheckBox chkTargetBirthday;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radOR;
        private System.Windows.Forms.RadioButton radAnd;
        private System.Windows.Forms.TabControl tabResult;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private ListViewEx lvFindResultBIRTHDAY;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cmbJudaishuseiD;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cmbJunidaijuseiC;
        private System.Windows.Forms.ComboBox cmbJudaishuseiB;
        private System.Windows.Forms.ComboBox cmbJudaishuseiE;
        private System.Windows.Forms.ComboBox cmbJudaishuseiA;
        private System.Windows.Forms.ComboBox cmbJunidaijuseiA;
        private System.Windows.Forms.ComboBox cmbJudaishuseiC;
        private System.Windows.Forms.ComboBox cmbJunidaijuseiB;
        private System.Windows.Forms.ComboBox cmbToYear;
        private System.Windows.Forms.ComboBox cmbFromYear;
        private System.Windows.Forms.Label label1;
    }
}