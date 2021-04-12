using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.IO;


namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        string exePath = "";

        TableMng dataMng;
        SetuiribiTable setuiribiTbl = null;
        public Form1()
        {
            InitializeComponent();

            exePath = Path.GetDirectoryName(Application.ExecutablePath);

            setuiribiTbl = new SetuiribiTable();
            setuiribiTbl.ReadTable(exePath + @"\節入り日.xls");


            txtNikkansiSanshutuSu_TextChanged(null, null);
        }


        //天中殺
        class TenchusatuLabelPair
        {
            public TenchusatuLabelPair(Label[] _aryLabel)
            {
                aryLabel = _aryLabel;
            }
            public bool IsExist(string s)
            {
                for (int i = 0; i < aryLabel.Length; i++)
                {
                    if (aryLabel[i].Text == s) return true;
                }
                return false;
            }
            public Label GetSameLabel(string s)
            {
                for (int i = 0; i < aryLabel.Length; i++)
                {
                    if (aryLabel[i].Text == s) return aryLabel[i];
                }
                return null;
            }
            public void SetColor(Color color)
            {
                for (int i = 0; i < aryLabel.Length; i++)
                {
                    aryLabel[i].ForeColor = color;
                }
            }


            public Label[] aryLabel;
        };

        private void button1_Click(object sender, EventArgs e)
        {

            dataMng = new TableMng();


            int baseYear = int.Parse(txtBaseYear.Text);
            int baseMonth = int.Parse(txtBaseMonth.Text);
            int baseDay = int.Parse(txtBaseDay.Text);

            int baseNenkansiNo = int.Parse(txtBaseNenkansiNo.Text);
            int baseGekkansiNo = int.Parse(txtBaseGekkansiNo.Text);
            int baseNikkansiNo = int.Parse(txtBaseNikkansiNo.Text);

            int Year = int.Parse(txtYear.Text);
            int Month = int.Parse(txtMonth.Text);
            int Day = int.Parse(txtDay.Text);

            setuiribiTbl.Init(baseYear, baseMonth, baseDay, baseNenkansiNo, baseGekkansiNo, baseNikkansiNo);

            int NikkansiNo = setuiribiTbl.GetNikkansiNo(Year, Month, Day);
            int GekkansiNo = setuiribiTbl.GetGekkansiNo(Year, Month, Day);
            int NenkansiNo = setuiribiTbl.GetNenKansiNo(Year, Month, Day);
            lblNIkkansiNo.Text = NikkansiNo.ToString();
            lblGekkansiNo.Text = GekkansiNo.ToString();
            lblNenkansiNo.Text = NenkansiNo.ToString();

            //日干支
            var kansi = dataMng.dicKansi[NikkansiNo];
            lblTenchusatu.Text = kansi.tenchusatu;

            lblNikkansi1.Text = kansi.kan;
            lblNikkansi2.Text = kansi.si;

            if(setuiribiTbl.CalcPassedDayFromSetuiribi(Year, Month, Day)>7)
            {

                lblNikkansi2.Font = new Font(lblNikkansi2.Font, FontStyle.Bold);
            }
            else
            {
                lblNikkansi2.Font = new Font(lblNikkansi2.Font, FontStyle.Regular);
            }


            //月干支
            kansi = dataMng.dicKansi[GekkansiNo];

            lblGekkansi1.Text = kansi.kan;
            lblGekkansi2.Text = kansi.si;

            //年干支
            kansi = dataMng.dicKansi[NenkansiNo];

            lblNenkansi1.Text = kansi.kan;
            lblNenkansi2.Text = kansi.si;


            Label[] aryLblNikkansiZougan = new Label[]
            {
                lblNikkansiShogen, lblNikkansiChugen, lblNikkansiHongen
            };
            Label[] aryLblGekkansiZougan = new Label[]
            {
                lblGekkansiShogen, lblGekkansiChugen, lblGekkansiHongen
            };
            Label[] aryLblNenkansiZougan = new Label[]
            {
                lblNenkansiShogen, lblNenkansiChugen, lblNenkansiHongen
            };

            //二十八元表
            NijuhachiGenso gensoNIkkansi = dataMng.lstNijuhachiGenso[lblNikkansi2.Text];
            NijuhachiGenso gensoGekkansi = dataMng.lstNijuhachiGenso[lblGekkansi2.Text];
            NijuhachiGenso gensoNenkansi = dataMng.lstNijuhachiGenso[lblNenkansi2.Text];
            for (int i = 0; i < 3; i++)
            {
                aryLblNikkansiZougan[i].Text = gensoNIkkansi.genso[i];
                aryLblGekkansiZougan[i].Text = gensoGekkansi.genso[i];
                aryLblNenkansiZougan[i].Text = gensoNenkansi.genso[i];
            }

            //-----------------------------
            //陽占
            //-----------------------------

            //十大主星
            //干1 → 蔵13
            lblJudaiShuseiA.Text = dataMng.juudaiShusei.GetJudaiShuseiName(lblNikkansi1.Text, lblNikkansiHongen.Text);
            //干1 → 蔵23
            lblJudaiShuseiB.Text = dataMng.juudaiShusei.GetJudaiShuseiName(lblNikkansi1.Text, lblGekkansiHongen.Text);
            //干1 → 蔵33
            lblJudaiShuseiC.Text = dataMng.juudaiShusei.GetJudaiShuseiName(lblNikkansi1.Text, lblNenkansiHongen.Text);
            //干1 → 干3
            lblJudaiShuseiD.Text = dataMng.juudaiShusei.GetJudaiShuseiName(lblNikkansi1.Text, lblNenkansi1.Text);
            //干1 → 干2
            lblJudaiShuseiE.Text = dataMng.juudaiShusei.GetJudaiShuseiName(lblNikkansi1.Text, lblGekkansi1.Text);

            //十二大主星
            //干1 → 支1
            lblJunidaiJuseiA.Text = dataMng.junidaiJusei.GetJunidaiJuseiName(lblNikkansi1.Text, lblNikkansi2.Text);
            //干1 → 支2
            lblJunidaiJuseiB.Text = dataMng.junidaiJusei.GetJunidaiJuseiName(lblNikkansi1.Text, lblGekkansi2.Text);
            //干1 → 支3
            lblJunidaiJuseiC.Text = dataMng.junidaiJusei.GetJunidaiJuseiName(lblNikkansi1.Text, lblNenkansi2.Text);



            TenchusatuLabelPair[] aryPair = new TenchusatuLabelPair[]
            {
                  new TenchusatuLabelPair(new Label[]{lblNenkansi1, lblNenkansi2 }),
                  new TenchusatuLabelPair(new Label[]{lblGekkansi1, lblGekkansi2 })
            };

            //干支との陽占ラベル関連付け
            Dictionary<Label, Label> dicKansiCombineYosenLabel = new Dictionary<Label, Label>()
            {
                { lblNenkansi1,lblJudaiShuseiD },
                { lblGekkansi1,lblJudaiShuseiE },
                { lblNenkansi2,lblJunidaiJuseiA },
                { lblGekkansi2,lblJunidaiJuseiB },
                { lblNikkansi2,lblJunidaiJuseiC },
            };
            //色を初期化
            for (int ary = 0; ary < aryPair.Length; ary++)
            {
                aryPair[ary].SetColor(Color.Black);
            }
            foreach(var lbl in dicKansiCombineYosenLabel)
            {
                lbl.Value.ForeColor = Color.Black;
            }

            string[] tenchusatu = lblTenchusatu.Text.Split(",");
            for(int i=0; i<tenchusatu.Length; i++)
            {

                for(int ary=0; ary< aryPair.Length; ary++)
                {
                   if( aryPair[ary].IsExist(tenchusatu[i]))
                   {
                        aryPair[ary].SetColor(Color.Red);

                        Label lbl = aryPair[ary].GetSameLabel(tenchusatu[i]);
                        dicKansiCombineYosenLabel[lbl].ForeColor = Color.Red;

                   }
                }

            }
        }

        private void txtNikkansiSanshutuSu_TextChanged(object sender, EventArgs e)
        {
            if (txtNikkansiSanshutuSu.Text == "") return;
            if (txtBaseDay.Text == "") return;

            int baseDay = int.Parse(txtBaseDay.Text);
            int no = int.Parse(txtNikkansiSanshutuSu.Text)+ baseDay;

            txtBaseNikkansiNo.Text = no.ToString();
        }
    }
}
