using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace WinFormsApp2
{
    public partial class FormExplanation : Form
    {
        public event Common.CloseHandler OnClose = null;
        ExplanationReader reader = new ExplanationReader();
        ExplanationReader.ExplanationData curData = null;
        string curType = "";

        Image curImage = null;

        const string explanationFileDefName = "ExplanationFileDef.ini";

        public FormExplanation()
        {
            InitializeComponent();
        }

        private void FormExplanation_Load(object sender, EventArgs e)
        {
            this.TopMost = true;

            //拡大縮小で縦横比率を維持
            picExplanation.SizeMode = PictureBoxSizeMode.Zoom;
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

                string fileName = GetDataFileName(type);
                if(string.IsNullOrEmpty(fileName))
                {
                    return;
                }
                string excelFilePath = Path.Combine(FormMain.GetExePath(), fileName );

                reader.ReadExcel(excelFilePath);

                curType = type;
            }

            //キー文字から"(～)"などを除外
            char[] splitKeys = new char[] { '(', ':', '：', '['};
            int index = key.IndexOfAny(splitKeys);
            if( index>=0)
            {
                key = key.Substring(0, index).Trim();
            }

            bool bEnable = true;
            curData = reader.GetExplanation(key);
            if (curData != null)
            {
                label1.Text = string.Format("/{0}", curData.pictureInfos.Count);
                lblPage.Text = "1";
                ShowPage(1);
                ResizeWindow(1);
            }
            else
            {
                label1.Text = "/0";
                lblPage.Text = "0";
                bEnable = false;
                FormExplanation_Resize(null, null);
            }
            button1.Enabled = bEnable;
            button2.Enabled = bEnable;
            button3.Enabled = bEnable;
            button4.Enabled = bEnable;
            base.Show();

        }

        //指定されたページの画像サイズにピクチャー（ウィンドウは＋α）サイズに合わせる
        private void ResizeWindow(int pageNo)
        {
            ImageConverter imgconv = new ImageConverter();
            Image img = (Image)imgconv.ConvertFrom(curData.pictureInfos[pageNo - 1].pictureData.Data);

            Size szForm = this.Size;
            Size szPicture = picExplanation.Size;
            int ofsW = szForm.Width - szPicture.Width;
            int ofsH = szForm.Height - szPicture.Height;


            this.Width = img.Width + ofsW;
            this.Height = img.Height + ofsH;

        }

        private string GetDataFileName( string type)
        {
            string filePath = Path.Combine( FormMain.GetExePath() , explanationFileDefName);
            IniFile iniFile = new IniFile(filePath);

            return iniFile.GetString("Setting", type);
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
            curImage = (Image)imgconv.ConvertFrom(curData.pictureInfos[pageNo - 1].pictureData.Data);
            picExplanation.Image = curImage;

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

        protected override bool ProcessDialogKey(Keys keyData)
        {
            //左キーが押されているか調べる
            if ((keyData & Keys.KeyCode) == Keys.Left)
            {
                button2_Click(null, null);
                //左キーの本来の処理（左側のコントロールにフォーカスを移す）を
                //させたくないときは、trueを返す
                return true;
            }
            //→キーが押されているか調べる
            else if ((keyData & Keys.KeyCode) == Keys.Right)
            {
                button1_Click(null, null);
                return true;
            }

            return base.ProcessDialogKey(keyData);
        }

        private void FormExplanation_Resize(object sender, EventArgs e)
        {
            panel1.Left = (this.Width - panel1.Width) / 2;
        }
    }
}
