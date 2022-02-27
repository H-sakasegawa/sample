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

        int curPageNo = 0;

        Image curImage = null;

        const string explanationFileDefName = "ExplanationFileDef.ini";

        public FormExplanation()
        {
            InitializeComponent();
        }

        private void FormExplanation_Load(object sender, EventArgs e)
        {
            this.TopMost = true;

            lstKeys.Dock = DockStyle.Fill;
            picExplanation.Dock = DockStyle.Fill;

            //拡大縮小で縦横比率を維持
            picExplanation.SizeMode = PictureBoxSizeMode.Zoom;

            picExplanation.MouseWheel
                += new System.Windows.Forms.MouseEventHandler(this.picExplanation_MouseWheel);
        }

        private void FormExplanation_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (OnClose != null) OnClose(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">説明資料種別</param>
        /// <param name="dispTargetKey">説明項目キー名</param>
        public void Show( string type, string dispTargetKey)
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

                //項目一覧表示

                var keys = reader.GetExplanationKeys();
                foreach(var item in keys)
                {
                    lstKeys.Items.Add(item);
                }

            }


            //キー文字から"(～)"などを除外
            char[] splitKeys = new char[] { '(', ':', '：', '['};
            int index = dispTargetKey.IndexOfAny(splitKeys);
            if( index>=0)
            {
                dispTargetKey = dispTargetKey.Substring(0, index).Trim();
            }

            SetCurrentExplanation(dispTargetKey);
            //bool bEnable = true;
            //curData = reader.GetExplanation(dispTargetKey);
            //if (curData != null)
            //{
            //    label1.Text = string.Format("/{0}", curData.pictureInfos.Count);
            //    lblPage.Text = "1";
            //    ShowPage(1);
            //    ResizeWindow(1);
            //}
            //else
            //{
            //    label1.Text = "/0";
            //    lblPage.Text = "0";
            //    bEnable = false;
            //    FormExplanation_Resize(null, null);
            //}
            //button1.Enabled = bEnable;
            //button2.Enabled = bEnable;
            //button3.Enabled = bEnable;
            //button4.Enabled = bEnable;
            base.Show();

        }

        private void SetCurrentExplanation( string dispTargetKey, bool bResize=true )
        {
            bool bEnable = true;
            curData = reader.GetExplanation(dispTargetKey);
            if (curData != null)
            {
                lblPage.Text = string.Format("{0}/{1}", 1, curData.pictureInfos.Count);
                ShowPage(1);
                if(bResize )ResizeWindow(1);
                lstKeys.SelectedItem = dispTargetKey;

            }
            else
            {

                ShowPage(-1);
                bEnable = false;
                FormExplanation_Resize(null, null);
                lstKeys.SelectedItem = null;
            }
            button1.Enabled = bEnable;
            button2.Enabled = bEnable;
            button3.Enabled = bEnable;
            button4.Enabled = bEnable;
        }

        //指定されたページの画像サイズにピクチャー（ウィンドウは＋α）サイズに合わせる
        private void ResizeWindow(int pageNo)
        {
            ImageConverter imgconv = new ImageConverter();
            Image img = (Image)imgconv.ConvertFrom(curData.pictureInfos[pageNo - 1].pictureData.Data);

            Size szForm = this.Size;
            Size szSplitWin = splitContainer1.Size;
 
            int ofsW = szForm.Width - szSplitWin.Width;
            int ofsH = szForm.Height - szSplitWin.Height;


            this.Width = img.Width + ofsW+ splitContainer1.Panel1.Width + splitContainer1.SplitterWidth;
            this.Height = img.Height + ofsH;

        }

        private string GetDataFileName( string type)
        {
            string filePath = Path.Combine( FormMain.GetExePath() , explanationFileDefName);
            IniFile iniFile = new IniFile(filePath);

            return iniFile.GetString("Setting", type);
        }

         private void ShowPage(int pageNo)
        {

            curPageNo = pageNo;

            if (curPageNo >= 0)
            {
                if (curData == null) return;
                ImageConverter imgconv = new ImageConverter();
                curImage = (Image)imgconv.ConvertFrom(curData.pictureInfos[pageNo - 1].pictureData.Data);
                picExplanation.Image = curImage;

                lblPage.Text = string.Format("{0}/{1}", curPageNo, curData.pictureInfos.Count);
            }
            else
            {
                picExplanation.Image = null;
                lblPage.Text = string.Format("{0}/{1}", 0,0);
            }
        }

        private void PageDown()
        {
            if (curData==null || curPageNo <= 1) return;

            ShowPage(curPageNo - 1);
        }

        private void PageUp()
        {
            if (curData == null || curPageNo >= curData.pictureInfos.Count) return;
            ShowPage(curPageNo + 1);
        }

        // "<"ボタン
        private void button2_Click(object sender, EventArgs e)
        {
            PageDown();
        }
        // ">"ボタン
        private void button1_Click(object sender, EventArgs e)
        {
            PageUp();
        }
        // "|<"ボタン
        private void button3_Click(object sender, EventArgs e)
        {
            ShowPage(1);

        }

        // ">|"ボタン
        private void button4_Click(object sender, EventArgs e)
        {
            ShowPage(curData.pictureInfos.Count);
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
            //ページ切り替えコントロールパネル位置を左右中央に設定
            panel1.Left = (this.Width - panel1.Width) / 2;
        }

        // マウスホイールイベント  
        private void picExplanation_MouseWheel(object sender, MouseEventArgs e)
        {
            if(e.Delta<0)
            {
                PageUp();
            }
            else if( e.Delta>0)
            {
                PageDown();
            }
        }

        private void lstKeys_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (string)lstKeys.SelectedItem;
            if (string.IsNullOrEmpty(key)) return;

            SetCurrentExplanation(key, false);

        }
    }
}
