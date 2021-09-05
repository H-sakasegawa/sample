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
        Person person;
        TableMng tblMng = TableMng.GetTblManage();

        public event CloseHandler OnClose;

        public FormShugoSinHou()
        {
            InitializeComponent();
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


            //第１守護神情報取得
            //調和の守護神属性, inigamiAttr
            if (string.IsNullOrEmpty(person.shugosinAttr))
            {
                lbl4.Text = lbl3.Text = "(なし)";
            }
            else { 
                //忌神
                lbl4.Text = person.imigamiAttr + "性" ;
                //調和の守護神
                lbl3.Text = person.shugosinAttr + "性";
            }

        }


        private void FormKonkihou_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnClose?.Invoke();
        }
    }
}
