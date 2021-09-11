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

    /// <summary>
    /// 守護神法　表示画面
    /// </summary>
    public partial class FormShugoSinHou : Form
    {
        public delegate void CloseHandler();
        public delegate void UpdateShugoSin();
        Person person;
        TableMng tblMng = TableMng.GetTblManage();

        public event CloseHandler OnClose = null;
        public event UpdateShugoSin OnUpdateShugosin=null;

        CheckBox[] chkShugosin;
        CheckBox[] chkImigami;

        bool bEditCustom = false;

        public FormShugoSinHou()
        {
            InitializeComponent();

            chkShugosin = new CheckBox[]
             {
                    checkBox1,checkBox2,checkBox3,checkBox4,checkBox5,
                    checkBox6,checkBox7,checkBox8,checkBox9,checkBox10
             };

            chkImigami = new CheckBox[]
             {
                    checkBox11,checkBox12,checkBox13,checkBox14,checkBox15,
                    checkBox16,checkBox17,checkBox18,checkBox19,checkBox20
             }; 


        }
        private void FormShugoSinHou_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < chkShugosin.Length; i++)
            {
                chkImigami[i].Text = chkShugosin[i].Text = tblMng.jyukanTbl[i].name;
            }

        }

        //外からのユーザ指定で画面更新
        public void Update(Person _person)
        {
            if (bEditCustom)
            {
                //メンバー登録ファイル更新
                Persons.GetPersons().WritePersonList();
                bEditCustom = false;
            }
            person = _person;
            Update();

        }
        /// <summary>
        /// 現在のpersonで表示更新
        /// </summary>
        private void Update( )
        {


            //調候の守護神
            lbl1.Text = "";
            foreach (var s in person.choukouShugosin)
            {
                lbl1.Text += s;
            }

            //調候の忌神
            lbl2.Text = person.choukouImigamiAttr;

            label3.Text = person.shugosinExplanation;


            string sNone = "(なし)";
            //第１守護神情報取得
            //調和の守護神属性, imigamiAttr
            if (string.IsNullOrEmpty(person.imigamiAttr))
            {
                lbl4.Text = lbl3.Text = sNone;
            }
            else {
                //忌神
                lbl4.Text = person.imigamiAttr + "性";

                //調和の守護神
                lbl3.Text = person.shugosinAttr + "性";
            }

            //カスタム守護神チェックボックス設定
            for (int i = 0; i < chkShugosin.Length; i++)
            {
                var chkBoxShugo = chkShugosin[i];
                var chkBoxImi = chkImigami[i];

                //チェックボックス変更イベントが発生しないよういったんイベントハンドラを削除
                chkBoxShugo.CheckedChanged -= chkShugosin_CheckedChanged;
                chkBoxImi.CheckedChanged -= chkImigami_CheckedChanged;
                {
                    chkBoxShugo.Checked = person.customShugosin.IsExist(chkShugosin[i].Text);
                    chkBoxImi.Checked = person.customImigami.IsExist(chkImigami[i].Text);
                }
                //イベントハンドラ再設定
                chkBoxShugo.CheckedChanged += chkShugosin_CheckedChanged;
                chkBoxImi.CheckedChanged += chkImigami_CheckedChanged;

                SetChkboxDisp(chkImigami, chkBoxShugo.Text, !chkBoxShugo.Checked); //忌神チェックボックスで同じものを有効/無効化
                SetChkboxDisp(chkShugosin, chkBoxImi.Text, !chkBoxImi.Checked); //守護神チェックボックスで同じものを有効/無効化

            }

            //守護神、忌神の手動設定チェックボックス
            chkCustom.Checked = person.bCustomShugosin;
            chkCustom_CheckedChanged(null, null);

        }


        private void FormKonkihou_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (bEditCustom)
            {
                //メンバー登録ファイル更新
                Persons.GetPersons().WritePersonList();
                bEditCustom = false;
            }

            OnClose?.Invoke();
        }

        private void chkShugosin_CheckedChanged(object sender, EventArgs e)
        {
            bEditCustom = true; //編集ありフラグセット
            CheckBox chkBox = (CheckBox)sender;
            if (chkBox.Checked)
            {
                person.customShugosin.Add(chkBox.Text);
            }
            else
            {
                person.customShugosin.Remove(chkBox.Text);
            }
            SetChkboxDisp(chkImigami, chkBox.Text, !chkBox.Checked ); //忌神チェックボックスで同じものを有効/無効化

            //メイン画面の守護神、忌神表示更新
            OnUpdateShugosin?.Invoke();
        }

        private void chkImigami_CheckedChanged(object sender, EventArgs e)
        {
            bEditCustom = true; //編集ありフラグセット
            CheckBox chkBox = (CheckBox)sender;
            if (chkBox.Checked)
            {
                person.customImigami.Add(chkBox.Text);
            }
            else
            {
                person.customImigami.Remove(chkBox.Text);
            }
            SetChkboxDisp(chkShugosin, chkBox.Text, !chkBox.Checked); //守護神チェックボックスで同じものを有効/無効化
                                                                     //メンバー登録ファイル更新
            //メイン画面の守護神、忌神表示更新
            OnUpdateShugosin?.Invoke();

        }
        /// <summary>
        /// チェックボックスの有効、無効設定
        /// </summary>
        /// <param name="chkBoxes"></param>
        /// <param name="junisi"></param>
        /// <param name="enable"></param>
        private void SetChkboxDisp(CheckBox[] chkBoxes, string junisi, bool enable)
        {
            foreach (var chkBox in chkBoxes)
            {
                if (chkBox.Text == junisi) { chkBox.Enabled = enable; break; }
            }
        }
        /// <summary>
        /// 守護神、忌神の手動設定チェックボックス
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkCustom_CheckedChanged(object sender, EventArgs e)
        {
            grpCustom.Enabled = chkCustom.Checked;
            person.bCustomShugosin = chkCustom.Checked;
            bEditCustom = true;
            //メイン画面の守護神、忌神表示更新
            OnUpdateShugosin?.Invoke();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chkShugosin.Length; i++)
            {
                chkImigami[i].Checked = chkShugosin[i].Checked =false;
            }
        }
    }
}
