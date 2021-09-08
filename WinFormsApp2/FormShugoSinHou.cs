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

        public void Update(Person _person )
        {
            person = _person;

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
            //調和の守護神属性, inigamiAttr
            if (person.ShugosinAttr.Count==0)
            {
                lbl4.Text = lbl3.Text = sNone;
            }
            else {
                //忌神
                lbl4.Text = lbl3.Text = "";
                foreach (var imigami in person.ImigamiAttr)
                {
                    if (lbl4.Text.Contains(imigami)) continue;
                    if (lbl4.Text != "") lbl4.Text += ",";
                    lbl4.Text += imigami + "性";
                }
                //調和の守護神
                foreach (var shugosin in person.ShugosinAttr)
                {
                    if (lbl3.Text.Contains(shugosin)) continue;
                    if (lbl3.Text != "") lbl3.Text += ",";
                    lbl3.Text += shugosin+ "性";
                }

                if (lbl4.Text == "") { lbl4.Text = sNone; }
                if (lbl3.Text == "") {lbl3.Text = sNone; } 
            }

            //カスタム守護神チェックボックス設定
            for (int i = 0; i < chkShugosin.Length; i++)
            {
                chkShugosin[i].Checked = person.customShugosin.IsExist(chkShugosin[i].Text);
                chkImigami[i].Checked = person.customImigami.IsExist(chkImigami[i].Text);
            }


        }


        private void FormKonkihou_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnClose?.Invoke();
        }

        private void chkShugosin_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkBox = (CheckBox)sender;
            if (chkBox.Checked)
            {
                person.customShugosin.Add(chkBox.Text);
            }
            else
            {
                person.customShugosin.Remove(chkBox.Text);
            }
            Update(person);
            Persons.GetPersons().WritePersonList();

            OnUpdateShugosin?.Invoke();
        }
        private void chkImigami_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkBox = (CheckBox)sender;
            if (chkBox.Checked)
            {
                person.AddCustomShugosin(chkBox.Text);
            }
            else
            {
                person.RemoveCustomShugosin(chkBox.Text);
            }
            Update(person);
            Persons.GetPersons().WritePersonList();
            OnUpdateShugosin?.Invoke();

        }

    }
}
