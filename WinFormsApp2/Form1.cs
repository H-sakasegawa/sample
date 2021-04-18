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


            txtYear.Text = "1960";
            txtMonth.Text = "8";
            txtDay.Text = "2";

            txtNikkansiSanshutuSu_TextChanged(null, null);
        }


        //天中殺
        class TenchusatuLabelPair
        {
            public TenchusatuLabelPair(Label[] _aryLabel, Label[] _zokanLabel)
            {
                aryLabel = _aryLabel;
                aryZoukanLabel = _zokanLabel;
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
                for (int i = 0; i < aryZoukanLabel.Length; i++)
                {
                    aryZoukanLabel[i].ForeColor = color;
                }
            }


            public Label[] aryLabel;
            public Label[] aryZoukanLabel;
        };

        private void button1_Click(object sender, EventArgs e)
        {

            dataMng = new TableMng();



            //----------------------------------------------
            //ラベルの組み合わせを登録
            //----------------------------------------------
            //日干支 ラベル
            List<Label> lstLblNikkansi = new List<Label>() { lblNikkansi1, lblNikkansi2 };
            //月干支 ラベル
            List<Label> lstLblGekkaansi = new List<Label>() { lblGekkansi1, lblGekkansi2 };
            //年干支 ラベル
            List<Label> lstLblNenkansi = new List<Label>() { lblNenkansi1, lblNenkansi2 };

            //日干支 天中殺 ラベル
            List<Label> lstLblNikkansiTenchusatu = new List<Label>() { lblNikkansiTenchusatu1, lblNikkansiTenchusatu2 };
            //年干支 天中殺 ラベル
            List<Label> lstLblNenkansiTenchusatu = new List<Label>() { lblNenkansiTenchusatu1, lblNenkansiTenchusatu2 };

            //日干支 二十八元素 ラベル
            List<Label> lstLblNikkansiNijuhachiGenso = new List<Label>() { lblNikkansiShogen, lblNikkansiChugen, lblNikkansiHongen };
            //月干支 二十八元素 ラベル
            List<Label> lstLblGekkansiNijuhachiGenso = new List<Label>() { lblGekkansiShogen, lblGekkansiChugen, lblGekkansiHongen };
            //年干支 二十八元素 ラベル
            List<Label> lstLblNenkansiNijuhachiGenso = new List<Label>() { lblNenkansiShogen, lblNenkansiChugen, lblNenkansiHongen };


            int baseYear = int.Parse(txtBaseYear.Text);
            int baseMonth = int.Parse(txtBaseMonth.Text);
            int baseDay = int.Parse(txtBaseDay.Text);

            int baseNenkansiNo = int.Parse(txtBaseNenkansiNo.Text);
            int baseGekkansiNo = int.Parse(txtBaseGekkansiNo.Text);
            int baseNikkansiNo = int.Parse(txtBaseNikkansiNo.Text);

            int Year = int.Parse(txtYear.Text);
            int Month = int.Parse(txtMonth.Text);
            int Day = int.Parse(txtDay.Text);

            //節入り日テーブル有効範囲チェック
            if( !setuiribiTbl.IsContainsYear(Year))
            {
                MessageBox.Show("節入り日テーブルに指定された年度の情報が不足しています");
                return;
            }
            setuiribiTbl.Init(baseYear, baseMonth, baseDay, baseNenkansiNo, baseGekkansiNo, baseNikkansiNo);

            int NikkansiNo = setuiribiTbl.GetNikkansiNo(Year, Month, Day);
            int GekkansiNo = setuiribiTbl.GetGekkansiNo(Year, Month, Day);
            int NenkansiNo = setuiribiTbl.GetNenKansiNo(Year, Month, Day);
            lblNIkkansiNo.Text = NikkansiNo.ToString();
            lblGekkansiNo.Text = GekkansiNo.ToString();
            lblNenkansiNo.Text = NenkansiNo.ToString();

            //============================================================
            //陰占
            //============================================================

            //------------------
            //日干支
            //------------------
            var Nikkansi = dataMng.dicKansi[NikkansiNo];

            lblNikkansi1.Text = Nikkansi.kan;
            lblNikkansi2.Text = Nikkansi.si;

            //誕生日に該当する節入り日から誕生日までの経過日数
            int dayNumFromSetuiribi = setuiribiTbl.CalcPassedDayFromSetuiribi(Year, Month, Day);

            //節理日から７日を超える日数の日干支を太字にする
            if (dayNumFromSetuiribi > 7)
            {
                Common.SetBold(lblNikkansi2, true);
            }
            else
            {
                Common.SetBold(lblNikkansi2, false);
            }


            //------------------
            //月干支
            //------------------
            var Gekkansi = dataMng.dicKansi[GekkansiNo];

            lblGekkansi1.Text = Gekkansi.kan;
            lblGekkansi2.Text = Gekkansi.si;

            //------------------
            //年干支
            //------------------
            var Nenkansi = dataMng.dicKansi[NenkansiNo];

            lblNenkansi1.Text = Nenkansi.kan;
            lblNenkansi2.Text = Nenkansi.si;


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

            //------------------
            //二十八元表
            //------------------
            NijuhachiGenso gensoNikkansi = dataMng.lstNijuhachiGenso[lblNikkansi2.Text];
            NijuhachiGenso gensoGekkansi = dataMng.lstNijuhachiGenso[lblGekkansi2.Text];
            NijuhachiGenso gensoNenkansi = dataMng.lstNijuhachiGenso[lblNenkansi2.Text];

            //十大主星判定用基準元素
            var idxNikkansiGensoType = (int)gensoNikkansi.GetTargetGensoType(dayNumFromSetuiribi);
            var idxGekkansiGensoType = (int)gensoGekkansi.GetTargetGensoType(dayNumFromSetuiribi);
            var idxNenkaisiGensoType = (int)gensoNenkansi.GetTargetGensoType(dayNumFromSetuiribi);

            foreach (var Value in Enum.GetValues(typeof(NijuhachiGenso.enmGensoType)))//初元、中元、本元
            {
                Label label = aryLblNikkansiZougan[(int)Value];
                label.Text = gensoNikkansi.genso[(int)Value].name;
                if(idxNikkansiGensoType== (int)Value) Common.SetBold(label, true);
                else Common.SetBold(label, false);

                label = aryLblGekkansiZougan[(int)Value];
                label.Text = gensoGekkansi.genso[(int)Value].name;
                if (idxGekkansiGensoType == (int)Value) Common.SetBold(label, true);
                else Common.SetBold(label, false);

                label = aryLblNenkansiZougan[(int)Value];
                label.Text = gensoNenkansi.genso[(int)Value].name;
                if (idxNenkaisiGensoType == (int)Value) Common.SetBold(label, true);
                else Common.SetBold(label, false);
            }

            //============================================================
            //陽占
            //============================================================

            //------------------
            //十大主星
            //------------------
            //干1 → 蔵13
            lblJudaiShuseiA.Text = dataMng.juudaiShusei.GetJudaiShuseiName(lblNikkansi1.Text, lstLblNikkansiNijuhachiGenso[ idxNikkansiGensoType ].Text);
            //干1 → 蔵23
            lblJudaiShuseiB.Text = dataMng.juudaiShusei.GetJudaiShuseiName(lblNikkansi1.Text, lstLblGekkansiNijuhachiGenso[ idxGekkansiGensoType ].Text);
            //干1 → 蔵33
            lblJudaiShuseiC.Text = dataMng.juudaiShusei.GetJudaiShuseiName(lblNikkansi1.Text, lstLblNenkansiNijuhachiGenso[idxNenkaisiGensoType ].Text);
            //干1 → 干3
            lblJudaiShuseiD.Text = dataMng.juudaiShusei.GetJudaiShuseiName(lblNikkansi1.Text, lblNenkansi1.Text);
            //干1 → 干2
            lblJudaiShuseiE.Text = dataMng.juudaiShusei.GetJudaiShuseiName(lblNikkansi1.Text, lblGekkansi1.Text);

            //------------------
            //十二大主星
            //------------------
            //干1 → 支3
            lblJunidaiJuseiA.Text = dataMng.junidaiJusei.GetJunidaiJuseiName(lblNikkansi1.Text, lblNenkansi2.Text);
            //干1 → 支2
            lblJunidaiJuseiB.Text = dataMng.junidaiJusei.GetJunidaiJuseiName(lblNikkansi1.Text, lblGekkansi2.Text);
            //干1 → 支1
            lblJunidaiJuseiC.Text = dataMng.junidaiJusei.GetJunidaiJuseiName(lblNikkansi1.Text, lblNikkansi2.Text);


            //日干支天中殺の文字チェック対象ラベル
            TenchusatuLabelPair[] nikkansiTenchusatuCheckLabels = new TenchusatuLabelPair[]
            {
                  new TenchusatuLabelPair(new Label[]{lblNenkansi1, lblNenkansi2 } ,new Label[]{lblNenkansiShogen, lblNenkansiChugen, lblNenkansiHongen }),
                  new TenchusatuLabelPair(new Label[]{lblGekkansi1, lblGekkansi2 } ,new Label[]{lblGekkansiShogen, lblGekkansiChugen, lblGekkansiHongen })
            };
            //年干支天中殺の文字チェック対象ラベル
            TenchusatuLabelPair[] nenkansiTenchusatuCheckLabels = new TenchusatuLabelPair[]
            {
                  new TenchusatuLabelPair(new Label[]{lblNikkansi1, lblNikkansi2 } ,new Label[]{lblNikkansiShogen, lblNikkansiChugen, lblNikkansiHongen }),
            };


            //---------------------------------------------
            //天中殺
            //---------------------------------------------
            string[] NikkansiTenchusatu = Nikkansi.tenchusatu.Split(",");
            string[] NenkansiTenchusatu = Nenkansi.tenchusatu.Split(",");
            for (int i = 0; i < 2; i++)
            {
                lstLblNikkansiTenchusatu[i].Text = NikkansiTenchusatu[i];
                lstLblNenkansiTenchusatu[i].Text = NenkansiTenchusatu[i];
            }

            //陽占ラベルの色設定に影響するラベルとの関連付け
            Dictionary<Label, Label> dicYosenCombineInsenLabel = new Dictionary<Label, Label>()
            {
                { lblJudaiShuseiA, lblNikkansiHongen },
                { lblJudaiShuseiB, lblGekkansiHongen },
                { lblJudaiShuseiC, lblNenkansiHongen },
                { lblJudaiShuseiD, lblNenkansi1 },
                { lblJudaiShuseiE, lblGekkansi1 },
                { lblJunidaiJuseiA, lblNenkansi2 },
                { lblJunidaiJuseiB, lblGekkansi2 },
                { lblJunidaiJuseiC, lblNikkansi2 },
            };
            //色を初期化
            for (int ary = 0; ary < nikkansiTenchusatuCheckLabels.Length; ary++)
            {
                nikkansiTenchusatuCheckLabels[ary].SetColor(Color.Black);
            }
            for (int ary = 0; ary < nenkansiTenchusatuCheckLabels.Length; ary++)
            {
                nikkansiTenchusatuCheckLabels[ary].SetColor(Color.Black);
            }
            foreach (var lbl in dicYosenCombineInsenLabel)
            {
                lbl.Value.ForeColor = Color.Black;
            }
            //日干支天中殺の文字が月干支と年干支に含まれていたら赤色設定
            for (int i = 0; i < NikkansiTenchusatu.Length; i++)
            {
                for (int j = 0; j < nikkansiTenchusatuCheckLabels.Length; j++)
                {
                    if (nikkansiTenchusatuCheckLabels[j].IsExist(NikkansiTenchusatu[i]))
                    {
                        nikkansiTenchusatuCheckLabels[j].SetColor(Color.Red);
                    }
                }
            }
            //年干支天中殺の文字が日干支に含まれていたら赤色設定
            for (int i = 0; i < NenkansiTenchusatu.Length; i++)
            {
                for (int j = 0; j < nenkansiTenchusatuCheckLabels.Length; j++)
                {
                    if (nenkansiTenchusatuCheckLabels[j].IsExist(NenkansiTenchusatu[i]))
                    {
                        nenkansiTenchusatuCheckLabels[j].SetColor(Color.Red);
                    }
                }
            }
            //陰占ラベルの色を陽占のラベルに反映
            foreach (var item in dicYosenCombineInsenLabel)
            {
                item.Key.ForeColor = item.Value.ForeColor;
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
