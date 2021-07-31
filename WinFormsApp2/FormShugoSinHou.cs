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

            var shugosin = tblMng.shugosinTbl.GetSugoSin(person);
            //調候の守護神
            lbl1.Text = shugosin.ToString();

            //調候の忌神
            lbl2.Text = shugosin.imi;

            label3.Text = shugosin.explanation;


            //忌神

            string imigami = person.GetImiGami();
            if( !string.IsNullOrEmpty( imigami))
            { 
                lbl4.Text = imigami+"性" ;


                //調和の守護神
                //五行属性で忌神を剋するもの
                var item = tblMng.gogyouAttrRelationshipTbl.GetRelation(imigami);
                lbl3.Text = item.destoryFromName + "性";
            }
            else
            {
                lbl4.Text = lbl3.Text = "(なし)";
            }

        }


        private void FormKonkihou_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnClose?.Invoke();
        }
    }
}
