using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WinFormsApp2
{
    public partial class FormMain : Form
    {

        TableMng tblMng = TableMng.GetTblManage();
        Persons personList = null;
        string exePath = "";
        int tabId = -1;

        public FormMain()
        {
            InitializeComponent();

            tabControl1.TabPages.Clear();
        }


        public void addform(TabPage tp, Form f)
        {
            tp.SuspendLayout();

            f.TopLevel = false;
            //no border if needed
            f.FormBorderStyle = FormBorderStyle.None;
            f.AutoScaleMode = AutoScaleMode.Dpi;
            if (!tp.Controls.Contains(f))
            {
                tp.Controls.Add(f);
                f.Dock = DockStyle.Fill;
                f.Show();
            }
            tp.ResumeLayout();
           // Refresh();
        }

 
        private void FormMain_Load(object sender, EventArgs e)
        {
            exePath = Path.GetDirectoryName(Application.ExecutablePath);

            personList = new Persons();
            //setuiribiTbl = new SetuiribiTable();
            try
            {
                //節入り日テーブル読み込み
                tblMng.setuiribiTbl.ReadTable(exePath + @"\節入り日.xls");
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("節入り日テーブルが読み込めません。\n\n{0}", ex.Message));
                return;
            }

            try
            {
                //名簿読み込み
                personList.ReadPersonList(exePath + @"\名簿.xls");
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("名簿.xlsが読み込めません。\n{0}", ex.Message));
                return;
            }


            tabControl1.TabPages.Add("基本");

            Form1 frm = new Form1(0, personList);
            addform(tabControl1.TabPages[0], frm);

        }

        private void mnuAddTab_Click(object sender, EventArgs e)
        {
            FormAddTab frmAddTab = new FormAddTab(personList);
            if (frmAddTab.ShowDialog() == DialogResult.OK)
            {
                ++tabId;
                Form1 frm = new Form1(tabId, personList, frmAddTab.selectPerson);
                frm.onCloseTab += OnTabClose;

                tabControl1.TabPages.Add(frmAddTab.selectPerson.name);
                tabControl1.TabPages[tabControl1.TabPages.Count - 1].Tag = tabId;

                addform(tabControl1.TabPages[tabControl1.TabPages.Count - 1], frm);
                tabControl1.SelectedTab = tabControl1.TabPages[tabControl1.TabPages.Count - 1];

            }
        }

        private void OnTabClose( int tagId)
        {
            foreach( TabPage tp in tabControl1.TabPages)
            {
                if (tp.Tag == null) continue;
                if( (int)tp.Tag == tagId)
                {
                    tabControl1.TabPages.Remove(tp);
                    break;
                }
            }

        }

    }
}
