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

    public partial class FormKonkihou : Form
    {
        public delegate void CloseHandler();

        //陰占 描画オブジェクト
        DrawInsen drawInsen = null;
        public event CloseHandler OnClose;

        public FormKonkihou()
        {
            InitializeComponent();
        }

        public void Update(Person person )
        {
            drawInsen = new DrawInsen(person, pictureBox1, false, false);
            drawInsen.Draw();

            Konkihou konkihou = new Konkihou( person );

            //各干支の根情報取得
            var kansiRoot =  konkihou.GetKansiRoot();

            DrawRootInfo(lblKonkiNikkansi, konkihou.nikkansi.kan, kansiRoot[0]);
            DrawRootInfo(lblKonkiGekkansi, konkihou.gekkansi.kan, kansiRoot[1]);
            DrawRootInfo(lblKonkiNenkansi, konkihou.nenkansi.kan, kansiRoot[2]);

            DrawAllow(drawInsen.rectNikansiKan, kansiRoot[0]);
            DrawAllow(drawInsen.rectGekkansiKan, kansiRoot[1]);
            DrawAllow(drawInsen.rectNenkansiKan, kansiRoot[2]);


        }

        private void DrawRootInfo(Label lbl, string kan, FindItem item)
        {
            lbl.Text = "なし";
            if (item != null)
            {
                lbl.Text = string.Format("{0} - {1} [ {2} ] {3}点", kan, item.si, item.junidaiJusei.name, item.junidaiJusei.tensuu);


            }
        }

        private void DrawAllow(Rectangle rectStart, FindItem item)
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
            OnClose?.Invoke();
        }
    }
}
