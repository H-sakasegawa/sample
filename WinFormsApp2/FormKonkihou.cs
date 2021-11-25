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
    /// 根気法　表示画面
    /// </summary>
    public partial class FormKonkihou : Form
    {
        public delegate void CloseHandler();
        Person person;
        TableMng tblMng = TableMng.GetTblManage();
        //陰占 描画オブジェクト
        DrawInsen drawInsen = null;
        public event Common.CloseHandler OnClose;

        public FormKonkihou()
        {
            InitializeComponent();
        }

        public void Update(Person _person )
        {
            person = _person;
            drawInsen = new DrawInsen(_person, pictureBox1, false, false);
            drawInsen.Draw();

            Konkihou konkihou = new Konkihou( _person );

            //各干支の根情報取得
            var kansiRoot =  konkihou.GetKansiRoot();

            DrawRootInfo(lblKonkiNikkansi, _person.nikkansi, kansiRoot[0]);
            DrawRootInfo(lblKonkiGekkansi, _person.gekkansi, kansiRoot[1]);
            DrawRootInfo(lblKonkiNenkansi, _person.nenkansi, kansiRoot[2]);

            DrawAllow(drawInsen.rectNikansiKan, kansiRoot[0]);
            DrawAllow(drawInsen.rectGekkansiKan, kansiRoot[1]);
            DrawAllow(drawInsen.rectNenkansiKan, kansiRoot[2]);

            lblScoreNikkansi.Text = konkihou.GetSumScore(_person.nikkansi.kan).ToString();
            lblScoreGekkansi.Text = konkihou.GetSumScore(_person.gekkansi.kan).ToString();
            lblScoreNenkansi.Text = konkihou.GetSumScore(_person.nenkansi.kan).ToString();


        }

        private void DrawRootInfo(Label lbl, Kansi kansi, Konkihou.FindItem item)
        {
            var attr1 = tblMng.jyukanTbl[person.nikkansi.kan].gogyou;
            var attr2 = tblMng.jyukanTbl[kansi.kan].gogyou;
            lbl.Text = "なし";
            if (item != null)
            {

                lbl.Text = string.Format("{0}({1}) - {2} [ {3} ] {4}点",
                                            kansi.kan,
                                            tblMng.gotokuTbl.GetGotoku(attr1, attr2),
                                            item.si,
                                            item.junidaiJusei.name,
                                            item.junidaiJusei.tensuu);
            }else
            {
                lbl.Text = string.Format("{0}({1}) - なし",
                                            kansi.kan,
                                            tblMng.gotokuTbl.GetGotoku(attr1, attr2)
                                            );
            }
        }

        private void DrawAllow(Rectangle rectStart, Konkihou.FindItem item)
        {
            if (item == null) return;

            Rectangle rectEnd = default;
            if (item.kansiBit == Const.bitFlgNiti) rectEnd = drawInsen.rectNikansiSi;
            if (item.kansiBit == Const.bitFlgGetu) rectEnd = drawInsen.rectGekkansiSi;
            if (item.kansiBit == Const.bitFlgNen) rectEnd = drawInsen.rectNenkansiSi;
            if(rectEnd!=default)
                drawInsen.DrawArrowLine(drawInsen.graph, rectStart, rectEnd);

        }

        private void FormKonkihou_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnClose?.Invoke(this);
        }
    }
}
