using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class FormExplanation : Form
    {
        public event Common.CloseHandler OnClose = null;
        ExplanationReader reader = new ExplanationReader();
        ExplanationReader.ExplanationData curData = null;
        string curType = "";

        public FormExplanation()
        {
            InitializeComponent();
        }

        private void FormExplanation_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
        }

        private void FormExplanation_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (OnClose != null) OnClose(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">説明資料種別</param>
        /// <param name="key">説明項目キー名</param>
        public void Show( string type, string key)
        {
            if (curType != type)
            {
                reader.Clear();

                string exePath = FormMain.GetExePath();
                string excelFilePath = exePath + @"\Contents.xlsx";

                reader.ReadExcel(excelFilePath);

                curType = type;
            }

            bool bEnable = true;
            curData = reader.GetExplanation(key);
            if (curData != null)
            {
                label1.Text = string.Format("/{0}", curData.pictureInfos.Count);
                lblPage.Text = "1";
                ShowPage(1);
            }
            else
            {
                label1.Text = "/0";
                lblPage.Text = "0";
                bEnable = false;
            }
            button1.Enabled = bEnable;
            button2.Enabled = bEnable;
            button3.Enabled = bEnable;
            button4.Enabled = bEnable;
            base.Show();

        }
        private void ShowPage()
        {
            int page = int.Parse(lblPage.Text);
            ShowPage(page);
        }
         private void ShowPage(int pageNo)
        {
            if (curData == null) return;

            ImageConverter imgconv = new ImageConverter();
            Image img = (Image)imgconv.ConvertFrom(curData.pictureInfos[pageNo - 1].pictureData.Data);
            picExplanation.Image = img;

        }

        // "<"ボタン
        private void button2_Click(object sender, EventArgs e)
        {
            int page = int.Parse(lblPage.Text);
            if (page <= 1) return;
            lblPage.Text = (page - 1).ToString();

            ShowPage();
        }
        // ">"ボタン
        private void button1_Click(object sender, EventArgs e)
        {
            int page = int.Parse(lblPage.Text);
            if (page >= curData.pictureInfos.Count) return;
            lblPage.Text = (page + 1).ToString();

            ShowPage();

        }
        // "|<"ボタン
        private void button3_Click(object sender, EventArgs e)
        {
            lblPage.Text = "1";
            ShowPage();

        }

        // ">|"ボタン
        private void button4_Click(object sender, EventArgs e)
        {
            lblPage.Text = curData.pictureInfos.Count.ToString();
            ShowPage();
        }
    }
}
