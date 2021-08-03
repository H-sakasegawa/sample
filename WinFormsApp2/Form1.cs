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
using System.Configuration;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        string exePath = "";

        TableMng tblMng = TableMng.GetTblManage();
        Persons personList = null;
        const int GetuunDispStartGetu = 2;

        //----------------------------------------------
        //ラベルの組み合わせを登録
        //----------------------------------------------
        ////日干支 ラベル
        //List<Label> lstLblNikkansi;
        ////月干支 ラベル
        //List<Label> lstLblGekkansi;
        ////年干支 ラベル
        //List<Label> lstLblNenkansi;

        ////日蔵元 ラベル
        //List<Label> lstLblNikkansiZougan;
        ////月蔵元 ラベル
        //List<Label> lstLblGekkansiZougan;
        ////年蔵元 ラベル
        //List<Label> lstLblNenkansiZougan;


        //日干支 天中殺 ラベル
        List<Label> lstLblNikkansiTenchusatu;
        //年干支 天中殺 ラベル
        List<Label> lstLblNenkansiTenchusatu;

        //日干支 二十八元素 ラベル
        List<Label> lstLblNikkansiNijuhachiGenso;
        //月干支 二十八元素 ラベル
        List<Label> lstLblGekkansiNijuhachiGenso;
        //年干支 二十八元素 ラベル
        List<Label> lstLblNenkansiNijuhachiGenso;

        //陰占 描画オブジェクト
        DrawInsen drawInsen = null;

        //宿命 描画オブジェクト
        DrawShukumei drawItem = null;
        //後天運 描画オブジェクト
        DrawKoutenUn drawItem2 = null;

        //表示対象データ
        Person curPerson = null;

        List<Label> lstLblGogyou;
        List<Label> lstLblGotoku;

        TaiunLvItemData curTaiun = null;
        GetuunNenunLvItemData curNenun = null;
        GetuunNenunLvItemData curGetuun = null;

        FromKyokiSimulation frmKykiSim = null;
        FormKonkihou frmKonkihou = null;
        FormJuniSinKanHou formJuniSinKanHou = null;

        FormShugoSinHou FormShugoSinHou = null;

        /// <summary>
        /// 年運表カラム Index
        /// </summary>
        enum ColNenunListView
        {
            COL_TITLE = 0,
            COL_KANSI,
            COL_JUDAISHUSEI,
            COL_JUNIDAIJUUSEI,
            COL_GOUHOUSANPOU_NITI,
            COL_GOUHOUSANPOU_GETU,
            COL_GOUHOUSANPOU_NEN,
            COL_CAREER
        }


        public Form1()
        {
            InitializeComponent();


            lvTaiun.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.lvTaiun_MouseWheel);
            lvNenun.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.lvNenun_MouseWheel);
            lvGetuun.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.lvGetuun_MouseWheel);


            exePath = Path.GetDirectoryName(Application.ExecutablePath);

            personList = new Persons();
            //setuiribiTbl = new SetuiribiTable();
            try
            {
                //節入り日テーブル読み込み
                tblMng.setuiribiTbl.ReadTable(exePath + @"\節入り日.xls");
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("節入り日テーブルが読み込めません。\n\n{0}", e.Message));
                return;
            }

            try
            {
                //名簿読み込み
                personList.ReadPersonList(exePath + @"\名簿.xls");
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("名簿.xlsが読み込めません。\n{0}", e.Message));
                return;
            }




            //----------------------------------------------
            //ラベルの組み合わせを登録
            //----------------------------------------------
            ////日干支 ラベル
            //lstLblNikkansi = new List<Label>() { lblNikkansi1, lblNikkansi2 };
            ////月干支 ラベル
            //lstLblGekkansi = new List<Label>() { lblGekkansi1, lblGekkansi2 };
            ////年干支 ラベル
            //lstLblNenkansi = new List<Label>() { lblNenkansi1, lblNenkansi2 };

            ////日干支 天中殺 ラベル
            //lstLblNikkansiTenchusatu = new List<Label>() { lblNikkansiTenchusatu1, lblNikkansiTenchusatu2 };
            ////年干支 天中殺 ラベル
            //lstLblNenkansiTenchusatu = new List<Label>() { lblNenkansiTenchusatu1, lblNenkansiTenchusatu2 };

            //未使用
            ////日干支 二十八元素 ラベル
            //lstLblNikkansiNijuhachiGenso = new List<Label>() { lblNikkansiShogen, lblNikkansiChugen, lblNikkansiHongen };
            ////月干支 二十八元素 ラベル
            //lstLblGekkansiNijuhachiGenso = new List<Label>() { lblGekkansiShogen, lblGekkansiChugen, lblGekkansiHongen };
            ////年干支 二十八元素 ラベル
            //lstLblNenkansiNijuhachiGenso = new List<Label>() { lblNenkansiShogen, lblNenkansiChugen, lblNenkansiHongen };


            //lstLblNikkansiZougan = new List<Label> { lblNikkansiShogen, lblNikkansiChugen, lblNikkansiHongen };
            //lstLblGekkansiZougan = new List<Label> { lblGekkansiShogen, lblGekkansiChugen, lblGekkansiHongen };
            //lstLblNenkansiZougan = new List<Label> { lblNenkansiShogen, lblNenkansiChugen, lblNenkansiHongen };

            lstLblGogyou = new List<Label> { lblGgyou1, lblGgyou2, lblGgyou3, lblGgyou4, lblGgyou5 };
            lstLblGotoku = new List<Label> { lblGotoku1, lblGotoku2, lblGotoku3, lblGotoku4, lblGotoku5 };

            txtNikkansiSanshutuSu_TextChanged(null, null);



            int baseYear = 0;
            int baseMonth = 0;
            int baseDay = 0;
            int baseNenkansi = 0;
            int baseGekkansi = 0;
            int baseNikkansiSanshutusuu = 0;
            //節入り日テーブルの先頭データを基準に基準情報を取得
            tblMng.setuiribiTbl.GetBaseSetuiribiData(ref baseYear, ref baseMonth, ref baseDay,
                                              ref baseNenkansi, ref baseGekkansi, ref baseNikkansiSanshutusuu);
            txtBaseYear.Text = baseYear.ToString();
            txtBaseMonth.Text = baseMonth.ToString();
            txtBaseDay.Text = baseDay.ToString();
            txtBaseNenkansiNo.Text = baseNenkansi.ToString();
            txtBaseGekkansiNo.Text = baseGekkansi.ToString();
            txtNikkansiSanshutuSu.Text = baseNikkansiSanshutusuu.ToString();

            for (int i = 0; i < lstLblGogyou.Count; i++)
            {
                lstLblGogyou[i].BackColor = tblMng.gogyouAttrColorTbl[lstLblGogyou[i].Text];
            }
            for (int i = 0; i < lstLblGotoku.Count; i++)
            {
                lstLblGotoku[i].BackColor = tblMng.gotokuAttrColorTbl[lstLblGotoku[i].Text];
            }


            grpGogyouGotoku.Enabled = chkGogyou.Checked || chkGotoku.Checked;
            chkRefrectSangouKaikyokuHousani.Enabled = false;

           //グループコンボボックス設定
           var groups = personList.GetGroups();
            cmbGroup.Items.Add("全て");
            foreach (var group in groups)
            {
                cmbGroup.Items.Add(group);
            }
            if (cmbGroup.Items.Count > 0)
            {
                cmbGroup.SelectedIndex = 0;
            }

            //Properties.Settings.Default.Reload();

            //if (cmbGroup.Items.Contains(Properties.Settings.Default.Group))
            //{
            //    cmbGroup.Text = Properties.Settings.Default.Group;
            //}
            ReloadSetting();



        }

        private void ReloadSetting()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            SetInitComboBox( config, "Group", cmbGroup);
            SetInitComboBox( config, "Name", cmbPerson);

            SetInitCheckBox(config, "Getuun", chkDispGetuun);
            SetInitCheckBox(config, "Nenun", chkDispNenun);
            SetInitCheckBox(config, "Taiun", chkDispTaiun);
            SetInitCheckBox(config, "SangouKaikyoku", chkSangouKaikyoku);
            SetInitCheckBox(config, "Gogyou", chkGogyou);
            SetInitCheckBox(config, "Gotoku", chkGotoku);
            SetInitCheckBox(config, "RefrectGouhou", chkRefrectGouhou);
            SetInitCheckBox(config, "RefrectSangouKaikyokuHousani", chkRefrectSangouKaikyokuHousani);



        }
        public void SetInitComboBox(Configuration config, string keyName, ComboBox cmb)
        {
            string sValue = config.AppSettings.Settings[keyName].Value;
            if (sValue != "")
            {
                for (int i = 0; i < cmb.Items.Count; i++)
                {
                    if ( cmb.Items[i].ToString() == sValue)
                    {
                        cmb.Text = sValue;
                        return;
                    }
                }
            }
        }
        public void SetInitCheckBox(Configuration config, string keyName, CheckBox chk)
        {
            string sValue = config.AppSettings.Settings[keyName].Value;
            if (sValue != "")
            {
                chk.Checked = bool.Parse(sValue);
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["Group"].Value = cmbGroup.Text;
            config.AppSettings.Settings["Name"].Value = cmbPerson.Text;
            config.AppSettings.Settings["Getuun"].Value = chkDispGetuun.Checked.ToString();
            config.AppSettings.Settings["Nenun"].Value = chkDispNenun.Checked.ToString();
            config.AppSettings.Settings["Taiun"].Value = chkDispTaiun.Checked.ToString();
            config.AppSettings.Settings["SangouKaikyoku"].Value = chkSangouKaikyoku.Checked.ToString();
            config.AppSettings.Settings["Gogyou"].Value = chkGogyou.Checked.ToString();
            config.AppSettings.Settings["Gotoku"].Value = chkGotoku.Checked.ToString();
            config.AppSettings.Settings["RefrectGouhou"].Value = chkRefrectGouhou.Checked.ToString();
            config.AppSettings.Settings["RefrectSangouKaikyokuHousani"].Value = chkRefrectSangouKaikyokuHousani.Checked.ToString();
            config.Save();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Person person = new Person(cmbPerson.Text,
                                        int.Parse(txtYear.Text),
                                        int.Parse(txtMonth.Text),
                                        int.Parse(txtDay.Text),
                                        radMan.Checked ? Gender.MAN : Gender.WOMAN
                                        );
            MainProc(person);
        }

        private void MainProc(Person person)
        {
            curPerson = person;

            int baseYear = int.Parse(txtBaseYear.Text);
            int baseMonth = int.Parse(txtBaseMonth.Text);
            int baseDay = int.Parse(txtBaseDay.Text);

            int baseNenkansiNo = int.Parse(txtBaseNenkansiNo.Text);
            int baseGekkansiNo = int.Parse(txtBaseGekkansiNo.Text);
            int baseNikkansiNo = int.Parse(txtBaseNikkansiNo.Text);

            //節入り日テーブル有効範囲チェック
            if ( !tblMng.setuiribiTbl.IsContainsYear(person.birthday.year))
            {
                MessageBox.Show("節入り日テーブルに指定された年度の情報が不足しています");
                return;
            }
            tblMng.setuiribiTbl.Init(baseYear, baseMonth, baseDay, baseNenkansiNo, baseGekkansiNo, baseNikkansiNo);

            //ユーザ情報初期設定
            person.Init(tblMng, tblMng.setuiribiTbl);

            //経歴リスト表示
            DispCarrerList(person);

            lblNikkansiNo.Text = person.nikkansiNo.ToString();
            lblGekkansiNo.Text = person.gekkansiNo.ToString();
            lblNenkansiNo.Text = person.nenkansiNo.ToString();

            //============================================================
            //陰占
            //============================================================

            //------------------
            //日干支
            //------------------
            var Nikkansi = person.nikkansi; 

            //lblNikkansi1.Text = Nikkansi.kan;
            //lblNikkansi2.Text = Nikkansi.si;

            ////誕生日に該当する節入り日から誕生日までの経過日数
            //int dayNumFromSetuiribi = setuiribiTbl.CalcDayCountFromSetuiribi(Year, Month, Day);

            ////節入日から７日を超える日数の日干支を太字にする
            //if (person.dayNumFromSetuiribi > 7)
            //{
            //    Common.SetBold(lblNikkansi2, true);
            //}
            //else
            //{
            //    Common.SetBold(lblNikkansi2, false);
            //}


            //------------------
            //月干支
            //------------------
            var Gekkansi = person.gekkansi;

            //lblGekkansi1.Text = Gekkansi.kan;
            //lblGekkansi2.Text = Gekkansi.si;

            //------------------
            //年干支
            //------------------
            var Nenkansi = person.nenkansi;

            //lblNenkansi1.Text = Nenkansi.kan;
            //lblNenkansi2.Text = Nenkansi.si;



            //------------------
            //二十八
            //------------------
            NijuhachiGenso nijuhachiGensoNikkansi = person.nijuhachiGensoNikkansi;
            NijuhachiGenso nijuhachiGensoGekkansi = person.nijuhachiGensoGekkansi;
            NijuhachiGenso nijuhachiGensoNenkansi = person.nijuhachiGensoNenkansi;

            //十大主星判定用基準元素
            var idxNikkansiGensoType = (int)nijuhachiGensoNikkansi.GetTargetGensoType(person.dayNumFromSetuiribi);
            var idxGekkansiGensoType = (int)nijuhachiGensoGekkansi.GetTargetGensoType(person.dayNumFromSetuiribi);
            var idxNenkaisiGensoType = (int)nijuhachiGensoNenkansi.GetTargetGensoType(person.dayNumFromSetuiribi);

            //foreach (var Value in Enum.GetValues(typeof(NijuhachiGenso.enmGensoType)))//初元、中元、本元
            //{
            //    Label label = lstLblNikkansiZougan[(int)Value];
            //    label.Text = nijuhachiGensoNikkansi.genso[(int)Value].name;
            //    if(idxNikkansiGensoType== (int)Value) Common.SetBold(label, true);
            //    else                                  Common.SetBold(label, false);

            //    label = lstLblGekkansiZougan[(int)Value];
            //    label.Text = nijuhachiGensoGekkansi.genso[(int)Value].name;
            //    if (idxGekkansiGensoType == (int)Value) Common.SetBold(label, true);
            //    else                                    Common.SetBold(label, false);

            //    label = lstLblNenkansiZougan[(int)Value];
            //    label.Text = nijuhachiGensoNenkansi.genso[(int)Value].name;
            //    if (idxNenkaisiGensoType == (int)Value) Common.SetBold(label, true);
            //    else                                    Common.SetBold(label, false);
            //}

            DispInsen(person, pictureBox3);

            //============================================================
            //陽占
            //============================================================
            DispYousen(person, idxNikkansiGensoType, idxGekkansiGensoType, idxNenkaisiGensoType);

            //============================================================
            //天中殺
            //============================================================
            //DispTenchusatu(person);
            SetYousenTenchusatuColor(drawInsen);

            //============================================================
            //後天運：大運
            //============================================================
            DispTaiun(person);

            //============================================================
            //位相法
            //============================================================
            DispShukumei(person, pictureBox1);

        }

        private void DispCarrerList(Person person)
        {
            lvCareer.Items.Clear();
            foreach (var career in person.career.dicCareer.OrderBy(c => c.Key))
            {
                var item =  lvCareer.Items.Add(career.Key.ToString());
                item.SubItems.Add(career.Value);
                
            }
        }


        /// <summary>
        /// 陽占 表示
        /// </summary>
        private void DispYousen(Person person , int idxNikkansiGensoType, int idxGekkansiGensoType, int idxNenkaisiGensoType)
        {

            //------------------
            //十大主星
            //------------------
            //干1 → 蔵x1
            lblJudaiShuseiA.Text = person.judaiShuseiA.name;
            //干1 → 蔵x2
            lblJudaiShuseiB.Text = person.judaiShuseiB.name;
            //干1 → 蔵x3
            lblJudaiShuseiC.Text = person.judaiShuseiC.name;
            //干1 → 干3
            lblJudaiShuseiD.Text = person.judaiShuseiD.name;
            //干1 → 干2
            lblJudaiShuseiE.Text = person.judaiShuseiE.name;

            //------------------
            //十二大主星
            //------------------
            //干1 → 支3
            lblJunidaiJuseiA.Text = person.junidaiJuseiA.name;
            //干1 → 支2
            lblJunidaiJuseiB.Text = person.junidaiJuseiB.name;
            //干1 → 支1
            lblJunidaiJuseiC.Text = person.junidaiJuseiC.name;

        }

        /// <summary>
        /// 天中殺 カラー設定（陽占）
        /// </summary>
        //private void DispTenchusatu(Person person)
        //{
        //    //日干支天中殺の文字チェック対象ラベル
        //    TenchusatuLabelPair[] nikkansiTenchusatuCheckLabels = new TenchusatuLabelPair[]
        //    {
        //          new TenchusatuLabelPair(new Label[]{lblNenkansi1, lblNenkansi2 } ,new Label[]{lblNenkansiShogen, lblNenkansiChugen, lblNenkansiHongen }),
        //          new TenchusatuLabelPair(new Label[]{lblGekkansi1, lblGekkansi2 } ,new Label[]{lblGekkansiShogen, lblGekkansiChugen, lblGekkansiHongen })
        //    };
        //    //年干支天中殺の文字チェック対象ラベル
        //    TenchusatuLabelPair[] nenkansiTenchusatuCheckLabels = new TenchusatuLabelPair[]
        //    {
        //          new TenchusatuLabelPair(new Label[]{lblNikkansi1, lblNikkansi2 } ,new Label[]{lblNikkansiShogen, lblNikkansiChugen, lblNikkansiHongen }),
        //    };

        //    Kansi Nikkansi = person.nikkansi;
        //    Kansi Nenkansi = person.nenkansi;

        //    string[] NikkansiTenchusatu = Nikkansi.tenchusatu.ToArray();
        //    string[] NenkansiTenchusatu = Nenkansi.tenchusatu.ToArray();
        //    for (int i = 0; i < 2; i++)
        //    {
        //        lstLblNikkansiTenchusatu[i].Text = NikkansiTenchusatu[i];
        //        lstLblNenkansiTenchusatu[i].Text = NenkansiTenchusatu[i];
        //    }

        //    //陽占ラベルの色設定に影響するラベルとの関連付け
        //    Dictionary<Label, Label> dicYosenCombineInsenLabel = new Dictionary<Label, Label>()
        //    {
        //        { lblJudaiShuseiA, lblNikkansiHongen },
        //        { lblJudaiShuseiB, lblGekkansiHongen },
        //        { lblJudaiShuseiC, lblNenkansiHongen },
        //        { lblJudaiShuseiD, lblNenkansi1 },
        //        { lblJudaiShuseiE, lblGekkansi1 },
        //        { lblJunidaiJuseiA, lblNenkansi2 },
        //        { lblJunidaiJuseiB, lblGekkansi2 },
        //        { lblJunidaiJuseiC, lblNikkansi2 },
        //    };
        //    //色を初期化
        //    for (int ary = 0; ary < nikkansiTenchusatuCheckLabels.Length; ary++)
        //    {
        //        nikkansiTenchusatuCheckLabels[ary].SetColor(Color.Black);
        //    }
        //    for (int ary = 0; ary < nenkansiTenchusatuCheckLabels.Length; ary++)
        //    {
        //        nikkansiTenchusatuCheckLabels[ary].SetColor(Color.Black);
        //    }
        //    foreach (var lbl in dicYosenCombineInsenLabel)
        //    {
        //        lbl.Value.ForeColor = Color.Black;
        //    }
        //    //日干支天中殺の文字が月干支と年干支に含まれていたら赤色設定
        //    for (int i = 0; i < NikkansiTenchusatu.Length; i++)
        //    {
        //        for (int j = 0; j < nikkansiTenchusatuCheckLabels.Length; j++)
        //        {
        //            if (nikkansiTenchusatuCheckLabels[j].IsExist(NikkansiTenchusatu[i]))
        //            {
        //                nikkansiTenchusatuCheckLabels[j].SetColor(Color.Red);
        //            }
        //        }
        //    }
        //    //年干支天中殺の文字が日干支に含まれていたら赤色設定
        //    for (int i = 0; i < NenkansiTenchusatu.Length; i++)
        //    {
        //        for (int j = 0; j < nenkansiTenchusatuCheckLabels.Length; j++)
        //        {
        //            if (nenkansiTenchusatuCheckLabels[j].IsExist(NenkansiTenchusatu[i]))
        //            {
        //                nenkansiTenchusatuCheckLabels[j].SetColor(Color.Red);
        //            }
        //        }
        //    }
        //    //陰占ラベルの色を陽占のラベルに反映
        //    foreach (var item in dicYosenCombineInsenLabel)
        //    {
        //        item.Key.ForeColor = item.Value.ForeColor;
        //    }
        //}


        private void SetYousenTenchusatuColor(DrawInsen insen)
        {
            //陽占ラベルの色設定に影響するラベルとの関連付け
            Dictionary<Label, TColor> dicYosenCombineInsenLabel = new Dictionary<Label, TColor>()
            {
                { lblJudaiShuseiA, insen.colorNikkansiHongen },
                { lblJudaiShuseiB, insen.colorGekkansiHongen },
                { lblJudaiShuseiC, insen.colorNenkansiHongen },
                { lblJudaiShuseiD, insen.colorNenkansiKan },
                { lblJudaiShuseiE, insen.colorGekkansiKan },
                { lblJunidaiJuseiA, insen.colorNenkansiSi},
                { lblJunidaiJuseiB, insen.colorGekkansiSi },
                { lblJunidaiJuseiC, insen.colorNikkansiSi },
            };
 
            //陰占ラベルの色を陽占のラベルに反映
            foreach (var item in dicYosenCombineInsenLabel)
            {
                item.Key.ForeColor = item.Value.color;
            }
        }



        //====================================================
        // 大運 表示処理
        //====================================================
        /// <summary>
        /// 大運リストビューアイテムデータクラス
        /// </summary>
        class TaiunLvItemData: LvItemDataBase
        {
            public int startNen; //開始年
            public int startYear; //開始年
            public Kansi kansi; //干支
            public bool bShugosin; //true...守護神
            public bool bImigami;   //true...忌神
        }
        /// <summary>
        /// 大運
        /// </summary>
        /// <param name="nenkansiNo"></param>
        private void DispTaiun(Person person )
        {
            lvTaiun.Items.Clear();

#if false
            //初旬干支番号
            int kansiNo = person.gekkansiNo;

            //順行、逆行
            int dirc = person.Direction();

            //int Year = int.Parse(txtYear.Text);
            //int Month = int.Parse(txtMonth.Text);
            //int Day = int.Parse(txtDay.Text);

            //才運
            int dayCnt;
            if ( dirc==1) //順行
            {
                dayCnt = person.CalcDayCountBirthdayToLastMonthDay();
            }
            else
            {
                //CalcDayCountFromSetuiribi()は節入り日を含めないので、＋１する
                dayCnt = person.CalcDayCountFromSetuiribi()+1;
            }
            int countStartNen = (int)Math.Ceiling(dayCnt / 3.0);
            if (countStartNen > 10) countStartNen = 10;

            //初旬
            AddTaiunItem(person, "初旬 0～", kansiNo, 0);


            //1旬～10旬まで
            for (int i=0; i<10; i++)
            {
                kansiNo += dirc;
                if (kansiNo < 1) kansiNo = 60;
                if (kansiNo > 60) kansiNo = 1;

                AddTaiunItem(person , string.Format("{0}旬 {1}～", i + 1, countStartNen),
                             kansiNo, countStartNen);

                countStartNen += 10;

            }
#else
            string[] choukouShugosinKan = null;
            string shugosinAttr = person.shugosinAttr;
            string imigamiAttr = person.imigamiAttr;
            if (string.IsNullOrEmpty(imigamiAttr))
            {
                choukouShugosinKan = person.choukouShugosin;
                imigamiAttr = person.choukouImigamiAttr;
            }

            var lstTaiunKansi = person.GetTaiunKansiList();
            for(int i=0; i< lstTaiunKansi.Count; i++)
            {
                var kansiItem = lstTaiunKansi[i];
                if ( i==0)
                {
                    //初旬
                    AddTaiunItem(person, "初旬 0～", kansiItem.kansiNo, 0, shugosinAttr, imigamiAttr, choukouShugosinKan);
                }
                else
                {
                    AddTaiunItem(person, string.Format("{0}旬 {1}～", i + 1, kansiItem.startYear),
                                 kansiItem.kansiNo, kansiItem.startYear,
                                 shugosinAttr, imigamiAttr, choukouShugosinKan);
                }
            }

#endif
            lvTaiun.Items[0].Selected = true;

        }
        ///// <summary>
        ///// 大運 順行、逆行判定
        ///// </summary>
        ///// <param name="NenkansiNo"></param>
        ///// <returns></returns>
        //private int Direction(Person person)
        //{
        //    var Nenkansi = person.nenkansi;// dataMng.kansiMng.dicKansi[ person.nenkansiNo ];

        //    //性別
        //    if (radMan.Checked)
        //    {   //男性
        //        if (dataMng.jyukanTbl[Nenkansi.kan].inyou == "+") return 1;
        //        else return -1;
        //    }
        //    else
        //    {   //女性
        //        if (dataMng.jyukanTbl[Nenkansi.kan].inyou == "+") return -1;
        //        else return 1;
        //    }
        //}
        /// <summary>
        /// 大運 行データ追加
        /// </summary>
        /// <param name="title"></param>
        /// <param name="kansiNo"></param>
        private void AddTaiunItem(Person person , string title, int kansiNo, int startNen,
                                  string shugosinAttr, string imigamiAttr,  string[] shugosinKan
            )
        {
            Kansi taiunKansi = person.GetKansi(kansiNo);

            var lvItem = lvTaiun.Items.Add(title);
            lvItem.SubItems.Add(string.Format("{0}{1}", taiunKansi.kan, taiunKansi.si)); //干支

            string judai = person.GetJudaiShusei(person.nikkansi.kan, taiunKansi.kan).name;
            string junidai = person.GetJunidaiShusei(person.nikkansi.kan, taiunKansi.si).name;

            //"星"を削除
            judai = judai.Replace("星", "");
            junidai = junidai.Replace("星", "");

            lvItem.SubItems.Add(judai); //十大主星
            lvItem.SubItems.Add(junidai); //十二大従星
     
            int idxNanasatuItem = 0;

            //日
            GouhouSannpouResult[] gouhouSanpoui = person.GetGouhouSanpouEx(taiunKansi, person.nikkansi, null, null);
            string nanasatu = (person.IsNanasatu(taiunKansi, person.nikkansi, ref idxNanasatuItem)==true && idxNanasatuItem==1) ? Const.sNanasatu : "";   //七殺
            string kangou = person.GetKangoStr(taiunKansi, person.nikkansi); //干合            
            lvItem.SubItems.Add(GetListViewItemString(gouhouSanpoui, kangou, nanasatu) );

            //月
            gouhouSanpoui = person.GetGouhouSanpouEx(taiunKansi, person.gekkansi, null, null);
            nanasatu = (person.IsNanasatu(taiunKansi, person.gekkansi, ref idxNanasatuItem) == true && idxNanasatuItem == 1) ? Const.sNanasatu : "";   //七殺
            kangou = person.GetKangoStr(taiunKansi, person.gekkansi); //干合
            lvItem.SubItems.Add(GetListViewItemString(gouhouSanpoui, kangou, nanasatu));

            //年
            gouhouSanpoui = person.GetGouhouSanpouEx(taiunKansi, person.nenkansi, null, null);
            nanasatu = (person.IsNanasatu(taiunKansi, person.nenkansi, ref idxNanasatuItem) == true && idxNanasatuItem == 1) ? Const.sNanasatu : "";   //七殺
            kangou = person.GetKangoStr(taiunKansi, person.nenkansi); //干合
            lvItem.SubItems.Add(GetListViewItemString(gouhouSanpoui, kangou, nanasatu));

            //天中殺
            Color color = Color.Black;
            for(int i=0; i< person.nikkansi.tenchusatu.ToArray().Length; i++)
            {
                if(taiunKansi.kan == person.nikkansi.tenchusatu[i] ||
                   taiunKansi.si == person.nikkansi.tenchusatu[i])
                {
                    color = Color.Red;
                    break;
                }
            }

            lvItem.ForeColor = color;

            //干、支の属性取得
            string kanAttr = tblMng.jyukanTbl[taiunKansi.kan].gogyou;
            string siAttr = tblMng.jyunisiTbl[taiunKansi.si].gogyou;


            //守護神判定
            bool bShugosin = false;
            if (!string.IsNullOrEmpty(shugosinAttr))
            {
                if (kanAttr == shugosinAttr || siAttr == shugosinAttr)
                {
                    bShugosin = true;
                }
            }
            else
            {
                foreach(var kan in shugosinKan)
                {
                    if( kan == taiunKansi.kan)
                    {
                        bShugosin = true;
                    }
                }
            }
            //忌神判定
            bool bImigami = false;
            if (kanAttr == imigamiAttr || siAttr == imigamiAttr)
            {
                bImigami = true;
            }

            TaiunLvItemData itemData = new TaiunLvItemData();
            itemData.startNen = startNen;   //開始年
            itemData.startYear = startNen + person.birthday.year;
            itemData.kansi = taiunKansi;    //干支
            itemData.bShugosin = bShugosin;  //守護神
            itemData.bImigami = bImigami;  //忌神

 
            if (bShugosin)
            {
                itemData.lstItemColors.Add(new LvItemColor(1, Const.colorShugosin));
            }
            else if (bImigami)
            {
                itemData.lstItemColors.Add(new LvItemColor(1, Const.colorShugosin));
            }

            //行のサブ情報を保持させておく
            lvItem.Tag = itemData;

        }


        //====================================================
        // 年運 表示処理
        //====================================================
        /// <summary>
        /// 年運リストビューアイテムデータクラス
        /// </summary>
        class GetuunNenunLvItemData: LvItemDataBase
        {
            /// <summary>
            /// 年運では、年
            /// 月運では、月
            /// </summary>
            public int keyValue; 
            public Kansi kansi; //干支
            public bool bShugosin; //true...守護神
            public bool bImigami;   //true...忌神
        }
        /// <summary>
        /// 年運
        /// </summary>
        /// <param name="baseYear">大運で選択された行の開始年</param>
        private void DispNenun(Person person ,int startNen)
        {
            lvNenun.Items.Clear();


            TaiunLvItemData taiunItemData = (TaiunLvItemData)lvTaiun.SelectedItems[0].Tag;
            Kansi taiunKansi = taiunItemData.kansi;

            int baseYear = person.birthday.year + startNen;
            int Month = person.birthday.month;
            int Day = person.birthday.day;
#if false
            //0才 干支番号
            int nenkansiNo = person.nenkansiNo;

            //選択された大運年度の開始干支番号
            nenkansiNo += baseYear - person.birthday.year;
            nenkansiNo = nenkansiNo % 60;
            if (nenkansiNo == 0) nenkansiNo = 60;
#else

            int nenkansiNo = person.GetNenkansiNo(baseYear);
#endif

            string[] choukouShugosinKan = null;
            string shugosinAttr = person.shugosinAttr;
            string imigamiAttr = person.imigamiAttr;
            if (string.IsNullOrEmpty(imigamiAttr))
            {
                choukouShugosinKan = person.choukouShugosin;
                imigamiAttr = person.choukouImigamiAttr;
            }

            //11年分を表示
            for (int i = 0; i < 10+1; i++)
            {
                //順行のみなので、60超えたら1にするだけ
                if (nenkansiNo > 60) nenkansiNo = 1;

                AddNenunItem( person,
                                    baseYear + i,
                                    string.Format("{0}歳({1})", (baseYear +i) - person.birthday.year,  baseYear +i),
                                    nenkansiNo,
                                    taiunKansi,
                                    shugosinAttr,
                                    imigamiAttr,
                                    choukouShugosinKan,
                                    lvNenun
                                    );
                nenkansiNo += 1;
            }
            lvNenun.Items[0].Selected = true;
            lvNenun.Items[0].Focused = true;

        }


        //====================================================
        // 月運 表示処理
        //====================================================
        /// <summary>
        /// 月運
        /// </summary>
        /// <param name="baseYear">大運で選択された行の開始年</param>
        private void DispGetuun(Person person)
        {
            lvGetuun.Items.Clear();

            TaiunLvItemData taiunItemData = (TaiunLvItemData)lvTaiun.SelectedItems[0].Tag;

            GetuunNenunLvItemData nenunItemData = (GetuunNenunLvItemData)lvNenun.SelectedItems[0].Tag;
            Kansi nenunKansi = nenunItemData.kansi;
            int year = nenunItemData.keyValue;

            //１月の月干支 ← 対象年運の選択行の干支
            //int gekkansiNo = taiunItemData.kansi.no;

            string[] choukouShugosinKan =null;
            string shugosinAttr = person.shugosinAttr;
            string imigamiAttr = person.imigamiAttr;
            if( string.IsNullOrEmpty(imigamiAttr))
            {
                choukouShugosinKan = person.choukouShugosin;
                imigamiAttr = person.choukouImigamiAttr;
            }

            //2月～12月,1月分を表示
            for (int i = 0; i < 12; i++)
            {
                int mMonth = GetuunDispStartGetu + i;
                if (mMonth > 12)
                {
                    mMonth = (mMonth - 12);
                    year = nenunItemData.keyValue+1;
                }

                //月干支番号取得(節入り日無視で単純月で取得）
                int gekkansiNo = tblMng.setuiribiTbl.GetGekkansiNo(year, mMonth);


                //順行のみなので、60超えたら1にするだけ
                //if (gekkansiNo > 60) gekkansiNo = 1;

                AddNenunGetuunItem(person, 
                                    mMonth, 
                                    string.Format("{0}月", mMonth),
                                    gekkansiNo,
                                    taiunItemData.kansi,
                                    shugosinAttr,
                                    imigamiAttr,
                                    choukouShugosinKan,
                                    lvGetuun
                                    );
                gekkansiNo += 1;
            }
            lvGetuun.Items[0].Selected = true;
            lvGetuun.Items[0].Focused = true;

        }


        /// <summary>
        /// 年運・月運 行データ追加
        /// </summary>
        /// <param name="person"></param>
        /// <param name="rowKeyValue">年 or 月</param>
        /// <param name="title">行タイトル文字列</param>
        /// <param name="targetkansiNo">年運干支No</param>
        /// <param name="kansi">大運干支No</param>
        private void AddNenunItem(Person person, int rowKeyValue, string title, int targetkansiNo, Kansi taiunKansi,
                                  string shugosinAttr, string imigamiAttr, string[] choukouShugosinKan, 
                                  ListView lv)
        {

            AddNenunGetuunItem(person, rowKeyValue, title, targetkansiNo, taiunKansi, shugosinAttr, imigamiAttr, choukouShugosinKan, lv);
            var lvItem = lv.Items[lv.Items.Count - 1];

            lvItem.SubItems[(int)ColNenunListView.COL_CAREER].Text = person.career[rowKeyValue]; //経歴


        }

        private void AddNenunGetuunItem(Person person, int rowKeyValue, string title, int targetkansiNo, Kansi taiunKansi,
                                  string shugosinAttr, string imigamiAttr, string[] choukouShugosinKan, 
                                  ListView lv)
        {

            Kansi taregetKansi = person.GetKansi(targetkansiNo);
            int idxNanasatuItem = 0;


            string judai = person.GetJudaiShusei(person.nikkansi.kan, taregetKansi.kan).name;
            string junidai = person.GetJunidaiShusei(person.nikkansi.kan, taregetKansi.si).name;

            var lvItem = lv.Items.Add(title);
            for (int i=0; i< Enum.GetNames(typeof(ColNenunListView)).Length-1; i++)
            {
                lvItem.SubItems.Add("");
            }
            lvItem.SubItems[(int)ColNenunListView.COL_KANSI].Text = string.Format("{0}{1}", taregetKansi.kan, taregetKansi.si); //干支

            //"星"を削除
            judai = judai.Replace("星", "");
            junidai = junidai.Replace("星", "");

            lvItem.SubItems[(int)ColNenunListView.COL_JUDAISHUSEI].Text = judai; //十大主星
            lvItem.SubItems[(int)ColNenunListView.COL_JUNIDAIJUUSEI].Text = junidai; //十二大従星

            //合法三法(日)
            GouhouSannpouResult[] gouhouSanpoui = person.GetGouhouSanpouEx(taregetKansi, person.nikkansi, taiunKansi, taregetKansi);
            string kangou = person.GetKangoStr(taregetKansi, person.nikkansi); //干合            
            string nanasatu = (person.IsNanasatu(taregetKansi, person.nikkansi, ref idxNanasatuItem) == true && idxNanasatuItem == 1)? Const.sNanasatu : "";   //七殺
            lvItem.SubItems[(int)ColNenunListView.COL_GOUHOUSANPOU_NITI].Text = GetListViewItemString(gouhouSanpoui, kangou, nanasatu);

            //合法三法(月)
            gouhouSanpoui = person.GetGouhouSanpouEx(taregetKansi, person.gekkansi, taiunKansi, taregetKansi);
            kangou = person.GetKangoStr(taregetKansi, person.gekkansi); //干合  
            nanasatu = (person.IsNanasatu(taregetKansi, person.gekkansi, ref idxNanasatuItem) == true && idxNanasatuItem == 1) ? Const.sNanasatu : "";   //七殺
            lvItem.SubItems[(int)ColNenunListView.COL_GOUHOUSANPOU_GETU].Text = GetListViewItemString(gouhouSanpoui, kangou, nanasatu);

            //合法三法(年)
            gouhouSanpoui = person.GetGouhouSanpouEx(taregetKansi, person.nenkansi, taiunKansi, taregetKansi);
            kangou = person.GetKangoStr(taregetKansi, person.nenkansi); //干合  
            nanasatu = (person.IsNanasatu(taregetKansi, person.nenkansi, ref idxNanasatuItem) == true && idxNanasatuItem == 1) ? Const.sNanasatu : "";   //七殺
            lvItem.SubItems[(int)ColNenunListView.COL_GOUHOUSANPOU_NEN].Text = GetListViewItemString(gouhouSanpoui, kangou, nanasatu);


            //天中殺
            Color color = Color.Black;
            for (int i = 0; i < 2; i++)
            {
                if (taregetKansi.IsExist(person.nikkansi.tenchusatu[i]) )
                {
                    color = Color.Red;
                    break;
                }
            }

            lvItem.ForeColor = color;

            //干、支の属性取得
            string kanAttr = tblMng.jyukanTbl[taregetKansi.kan].gogyou;
            string siAttr = tblMng.jyunisiTbl[taregetKansi.si].gogyou;


            //守護神判定
            bool bShugosin = false;
            if (!string.IsNullOrEmpty(shugosinAttr))
            {
                if (kanAttr == shugosinAttr || siAttr == shugosinAttr)
                {
                    bShugosin = true;
                }
            }
            else
            {
                foreach (var kan in choukouShugosinKan)
                {
                    if (kan == taregetKansi.kan)
                    {
                        bShugosin = true;
                    }
                }
            }
            //忌神判定
            bool bImigami = false;
            if (kanAttr == imigamiAttr || siAttr == imigamiAttr)
            {
                bImigami = true;
            }



            GetuunNenunLvItemData itemData = new GetuunNenunLvItemData();
            itemData.keyValue = rowKeyValue;           //年 or 月
            itemData.kansi = taregetKansi;    //干支
            itemData.bShugosin = bShugosin;  //守護神
            itemData.bImigami = bImigami;  //忌神

            if (bShugosin)
            {
                itemData.lstItemColors.Add(new LvItemColor(1, Const.colorShugosin));
            }
            else if (bImigami)
            {
                itemData.lstItemColors.Add(new LvItemColor(1, Const.colorImigami));
            }
            //行のサブ情報を保持させておく
            lvItem.Tag = itemData;

        }
        string GetListViewItemString(GouhouSannpouResult[] lstGouhouSanpouResult, params string[] ary)
        {
            string result = "";
            foreach (var item in lstGouhouSanpouResult)
            {
                if (!string.IsNullOrEmpty(result)) result += " ";
                if (item.bEnable) result += item.displayName;
                else result += string.Format("[{0}]", item.displayName);
            }
            foreach (var item in ary)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    if (!string.IsNullOrEmpty(result)) result += " ";
                    result += item;
                }
            }
            return result;
        }


        /// <summary>
        /// 宿命図表示
        /// </summary>
        /// <param name="person"></param>
        private void DispInsen(Person person, PictureBox pictureBox)
        {

            drawInsen = new DrawInsen(person, pictureBox, true, true);
            drawInsen.Draw();

        }


        //============================================================
        //位相法
        //============================================================

        /// <summary>
        /// 宿命図表示
        /// </summary>
        /// <param name="person"></param>
        private void DispShukumei(Person person, PictureBox pictureBox)
        {

            drawItem = new DrawShukumei(person, pictureBox, chkGogyou.Checked, chkGotoku.Checked, chkRefrectGouhou.Checked);
            drawItem.Draw();

        }
        /// <summary>
        /// 後天運図表示
        /// </summary>
        /// <param name="person"></param>
        private void DispKoutenUn(Person person, PictureBox pictureBox)
        {

            //大運の選択行の干支取得
            var selectedItem = lvTaiun.SelectedItems;
            if (selectedItem.Count == 0) return;

            curTaiun = (TaiunLvItemData)selectedItem[0].Tag;

            //年運の選択行の干支取得
            selectedItem = lvNenun.SelectedItems;
            if (selectedItem.Count == 0) return;

            curNenun = (GetuunNenunLvItemData)selectedItem[0].Tag;

            //月運の選択行の干支取得
            selectedItem = lvGetuun.SelectedItems;
            if (selectedItem.Count == 0) return;

            curGetuun = (GetuunNenunLvItemData)selectedItem[0].Tag;

            if (drawItem2 != null) drawItem2.Dispose();
            drawItem2 = new DrawKoutenUn(person, pictureBox, 
                                        curTaiun.kansi, curNenun.kansi, curGetuun.kansi,
                                        chkDispTaiun.Checked,
                                        chkDispNenun.Checked,
                                        chkDispGetuun.Checked,
                                        chkSangouKaikyoku.Checked,
                                        chkGogyou.Checked, 
                                        chkGotoku.Checked,
                                        chkRefrectGouhou.Checked,
                                        chkRefrectSangouKaikyokuHousani.Checked
                                        );
            drawItem2.Draw();


            //虚気変化数表示
            KyokiSimulation sim = new KyokiSimulation();

            sim.Simulation(person, curGetuun.kansi, curNenun.kansi, curTaiun.kansi, chkDispGetuun.Checked);
            lblKyokiNum.Text = string.Format("虚気変化パターン数:{0}", sim.lstKansPattern.Count-1);

            if (frmKykiSim != null && frmKykiSim.Visible==true)
            {
                frmKykiSim.UpdateKyokiPatternOnly(curPerson,
                                        curNenun.keyValue,
                                        curGetuun.kansi, curNenun.kansi, curTaiun.kansi,
                                        chkDispGetuun.Checked,
                                        chkSangouKaikyoku.Checked,
                                        chkGogyou.Checked,
                                        chkGotoku.Checked,
                                        chkRefrectGouhou.Checked,
                                        chkRefrectSangouKaikyokuHousani.Checked
                                    );;
            }

        }

        void DispDateView(DateTime today)
        {
            int year = today.Year;

            //大運リストビューで年に該当する行を選択
            for (int i = 0; i < lvTaiun.Items.Count; i++)
            {

                TaiunLvItemData itemData = (TaiunLvItemData)lvTaiun.Items[i].Tag;
                if (itemData.startYear > today.Year)
                {
                    int index = i - 1;
                    if (index < 0) index = 0;

                    itemData = (TaiunLvItemData)lvTaiun.Items[index].Tag;
                    if (itemData.startYear == year)
                    {
                        //１月の場合、前年を表示する必要がある
                        if (today.Month < GetuunDispStartGetu)
                        {
                            index--;
                            if (index < 0) index = 0; //このチェックで引っかかることはない
                        }
                    }

                    lvTaiun.Items[index].Selected = true; ;
                    break;
                }
            }
            //年運リストビューで年に該当する行を選択
            if( today.Month< GetuunDispStartGetu)
            {
                //月運で選択される月は、次の年度の月となるので、
                //年運の選択を１年前に設定する必要がある。
                year--;
            }
            for (int i = 0; i < lvNenun.Items.Count; i++)
            {

                GetuunNenunLvItemData itemData = (GetuunNenunLvItemData)lvNenun.Items[i].Tag;
                if (itemData.keyValue == year)
                {
                    lvNenun.Items[i].Selected = true;
                    break;
                }
            }
            //月運リストビューで月に該当する行を選択
            for (int i = 0; i < lvGetuun.Items.Count; i++)
            {

                GetuunNenunLvItemData itemData = (GetuunNenunLvItemData)lvGetuun.Items[i].Tag;
                if (itemData.keyValue == today.Month)
                {
                    lvGetuun.Items[i].Selected = true;
                    break;
                }
            }
        }

        /// <summary>
        /// .大運、年運のリスト表示を指定年月に設定
        /// </summary>
        /// <param name="year"></param>
        public void UpdateNeunTaiunDisp(int year)
        {
            //月運リストビューは年度の最初の月を選択
            DispDateView(new DateTime(year, GetuunDispStartGetu, 1));

        }


        //====================================================
        // イベントハンドラ
        //====================================================
        /// <summary>
        /// グループコンボボックス
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Person> persons = null;
            if (cmbGroup.SelectedIndex == 0)
            {
                //全て
                persons = personList.GetPersons();
            }
            else
            {
                var item = (Group)cmbGroup.SelectedItem;
                persons = item.members;

            }
            cmbPerson.Items.Clear();
            for (int i = 0; i < persons.Count; i++)
            {
                cmbPerson.Items.Add(persons[i]);
            }
            if (cmbPerson.Items.Count > 0)
            {
                cmbPerson.SelectedIndex = 0;
            }

        }
        /// <summary>
        /// 人コンボボックス選択イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPerson_SelectedIndexChanged(object sender, EventArgs e)
        {
            Person person = (Person)cmbPerson.SelectedItem;
            Birthday birthday = person.birthday;
            txtYear.Text = birthday.year.ToString();
            txtMonth.Text = birthday.month.ToString();
            txtDay.Text = birthday.day.ToString();

            if (person.gender == Gender.MAN) radMan.Checked = true;
            else radWoman.Checked = true;

            MainProc(person);

            if (frmKykiSim != null)
            {
                frmKykiSim.UpdateAll(curPerson, birthday.year,
                                        curGetuun.kansi, curNenun.kansi, curTaiun.kansi,
                                        chkDispGetuun.Checked,
                                        chkSangouKaikyoku.Checked,
                                        chkGogyou.Checked,
                                        chkGotoku.Checked,
                                        chkRefrectGouhou.Checked,
                                        chkRefrectSangouKaikyokuHousani.Checked
                                    );
            }
            //根気法画面再描画
            if(frmKonkihou!=null)
            {
                frmKonkihou.Update(curPerson);
            }
            if(formJuniSinKanHou!=null)
            {
                formJuniSinKanHou.Update(curPerson);
            }
            if(FormShugoSinHou!=null)
            {
                FormShugoSinHou.Update(curPerson);
            }


        }
        /// <summary>
        /// 日干支算出番号エディットボックス変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNikkansiSanshutuSu_TextChanged(object sender, EventArgs e)
        {
            if (txtNikkansiSanshutuSu.Text == "") return;
            if (txtBaseDay.Text == "") return;

            int baseDay = int.Parse(txtBaseDay.Text);
            int no = int.Parse(txtNikkansiSanshutuSu.Text) + baseDay;

            txtBaseNikkansiNo.Text = no.ToString();

        }

        /// <summary>
        /// 今日へ移動ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            DispDateView(DateTime.Now);
            //DispDateView(new DateTime(2003,1,1));
        }
        /// <summary>
        /// 経歴リストビュー選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvCareer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvCareer.SelectedItems.Count == 0) return;
            var item = lvCareer.SelectedItems[0];
            int year = int.Parse(item.SubItems[0].Text);
            //月運リストビューは年度の最初の月を選択
            DispDateView(new DateTime(year, GetuunDispStartGetu, 1));
        }


        //------------------------------------------------------------
        // 大運リストビュー イベント
        //------------------------------------------------------------
        /// <summary>
        /// 大運リストビュー選択イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvTaiun_SelectedIndexChanged(object sender, EventArgs e)
        {
            int Year = int.Parse(txtYear.Text);

            var selectedItem = lvTaiun.SelectedItems;
            if (selectedItem.Count == 0) return;

            TaiunLvItemData itemData = (TaiunLvItemData)selectedItem[0].Tag;

            //年運リスト表示更新
            DispNenun(curPerson, itemData.startNen);

            //後天運 図の表示更新
            DispKoutenUn(curPerson, pictureBox2);
        }
        /// <summary>
        /// 大運リストビューのホイール操作イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvTaiun_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var idxTaiun = lvTaiun.SelectedItems[0].Index;
            if (e.Delta > 0)
            {   //↑
                if (idxTaiun <= 0) return;
                lvTaiun.Items[idxTaiun - 1].Selected = true;
            }
            else
            {
                //↓
                if (idxTaiun >= lvTaiun.Items.Count - 1) return;
                lvTaiun.Items[idxTaiun + 1].Selected = true;
            }
        }


        //------------------------------------------------------------
        // 年運リストビュー イベント
        //------------------------------------------------------------
        /// <summary>
        /// 年運リストビュー選択イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvNenun_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = lvNenun.SelectedItems;
            if (selectedItem.Count == 0) return;
            //月運表示更新
            DispGetuun(curPerson);

            //後天運 図の表示更新
            DispKoutenUn(curPerson, pictureBox2);
        }
        /// <summary>
        /// 年運リストビューのホイール操作イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvNenun_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var idxNenun = lvNenun.SelectedItems[0].Index;
            if (e.Delta > 0)
            {
                //↑
                if (idxNenun > 0)
                {
                    lvNenun.Items[idxNenun - 1].Selected = true;
                    lvNenun.Items[idxNenun - 1].Focused = true;
                }
                else
                {
                    var idxTaiun = lvTaiun.SelectedItems[0].Index;
                    if (idxTaiun <= 0) return;
                    lvTaiun.Items[idxTaiun - 1].Selected = true;
                    lvTaiun.Items[idxTaiun - 1].Focused = true;
                    lvNenun.Items[lvNenun.Items.Count - 1].Selected = true;
                    lvNenun.Items[lvNenun.Items.Count - 1].Focused = true;
                }


            }
            else
            {
                //↓
                if (idxNenun < lvNenun.Items.Count - 1)
                {
                    lvNenun.Items[idxNenun + 1].Selected = true;
                    lvNenun.Items[idxNenun + 1].Focused = true;
                }
                else
                {
                    //次の旬に移動
                    var idxTaiun = lvTaiun.SelectedItems[0].Index;
                    if (idxTaiun >= lvTaiun.Items.Count - 1) return;

                    lvTaiun.Items[idxTaiun + 1].Selected = true;
                    lvTaiun.Items[idxTaiun + 1].Focused = true;
                    lvNenun.Items[0].Selected = true;
                    lvNenun.Items[0].Focused = true;
                }

            }
        }
        /// <summary>
        /// 年運リストビューでの上下キー押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvNenun_KeyDown(object sender, KeyEventArgs e)
        {
            var idxNenun = lvNenun.SelectedItems[0].Index;
            switch (e.KeyData)
            {
                case Keys.Up:
                    if (idxNenun <= 0)
                    {
                        var idxTaiun = lvTaiun.SelectedItems[0].Index;
                        if (idxTaiun <= 0) return;
                        lvTaiun.Items[idxTaiun - 1].Selected = true;
                        lvTaiun.Items[idxTaiun - 1].Focused = true;
                        lvNenun.Items[lvNenun.Items.Count - 1].Selected = true;
                        lvNenun.Items[lvNenun.Items.Count - 1].Focused = true;
                        lvTaiun.Update();
                        e.Handled = true;
                    }
                    break;
                case Keys.Down:
                    if (idxNenun >= lvNenun.Items.Count - 1)
                    {
                        //次の旬に移動
                        var idxTaiun = lvTaiun.SelectedItems[0].Index;
                        if (idxTaiun >= lvTaiun.Items.Count - 1) return;

                        lvTaiun.Items[idxTaiun + 1].Selected = true;
                        lvTaiun.Items[idxTaiun + 1].Focused = true;
                        lvNenun.Items[0].Selected = true;
                        lvNenun.Items[0].Focused = true;
                        lvTaiun.Update();
                        e.Handled = true;
                    }
                    break;
            }
        }
        private void lvGetuun_KeyDown(object sender, KeyEventArgs e)
        {
            var idxGetuun = lvGetuun.SelectedItems[0].Index;
            switch (e.KeyData)
            {
                case Keys.Up:
                    if (idxGetuun <= 0)
                    {
                        var idxNenun = lvNenun.SelectedItems[0].Index;
                        //if (idxNenun <= 0) return;
                        //先に年運の選択行を移動させてしまうと、lvNenun_KeyDownで先頭行でUPキーが
                        //押下されたとご認識してしまうので、lvNenun_KeyDownコール後に選択行を設定
                        KeyEventArgs eventArgs = new KeyEventArgs(e.KeyData);
                        lvNenun_KeyDown(lvNenun, eventArgs);

                        var idTaiun = lvTaiun.SelectedItems[0].Index;
                        idxNenun = lvNenun.SelectedItems[0].Index;
                        if (idTaiun == 0 && idxNenun == 0) return;

                        if (idxNenun == lvNenun.SelectedItems[0].Index)
                        {
                            lvNenun.Items[idxNenun - 1].Selected = true;
                            lvNenun.Items[idxNenun - 1].Focused = true;
                        }

                        lvGetuun.Items[lvGetuun.Items.Count - 1].Selected = true;
                        lvGetuun.Items[lvGetuun.Items.Count - 1].Focused = true;
                        e.Handled = true;
                    }
                    break;
                case Keys.Down:
                    if (idxGetuun >= lvGetuun.Items.Count - 1)
                    {
                        //次の年に移動
                        // if (idxNenun >= lvNenun.Items.Count - 1) return;
                        //先に年運の選択行を移動させてしまうと、lvNenun_KeyDownで最終行でDOWNキーが
                        //押下されたとご認識してしまうので、lvNenun_KeyDownコール後に選択行を設定
                        KeyEventArgs eventArgs = new KeyEventArgs(e.KeyData);
                        lvNenun_KeyDown(lvNenun, eventArgs);

                        var idxNenun = lvNenun.SelectedItems[0].Index;
                        var idTaiun = lvTaiun.SelectedItems[0].Index;
                        if (idTaiun == lvTaiun.Items.Count - 1 && idxNenun == lvNenun.Items.Count - 1) return;

                        lvNenun.Items[idxNenun + 1].Selected = true;
                        lvNenun.Items[idxNenun + 1].Focused = true;

                        lvGetuun.Items[0].Selected = true;
                        lvGetuun.Items[0].Focused = true;
                        e.Handled = true;
                    }
                    break;
            }
        }
        /// <summary>
        /// 年運リストビュー　ダブルクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvNenun_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = lvNenun.SelectedItems[0];

            GetuunNenunLvItemData itemData = (GetuunNenunLvItemData)item.Tag;
            //編集画面表示
            FormEditCareer frm = new FormEditCareer(itemData.keyValue, curPerson);
            if( frm.ShowDialog()==DialogResult.OK)
            {
                //リストビューの経歴表示更新
                item.SubItems[(int)ColNenunListView.COL_CAREER].Text = curPerson.career[itemData.keyValue];
                DispCarrerList(curPerson);
            }


        }




        //------------------------------------------------------------
        // 月運リストビュー イベント
        //------------------------------------------------------------
        /// <summary>
        /// 月運リストビュー選択イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvGetuUn_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = lvGetuun.SelectedItems;
            if (selectedItem.Count == 0) return;
            //後天運 図の表示更新
            DispKoutenUn(curPerson, pictureBox2);
        }

        /// <summary>
        /// 月運リストビューのホイール操作イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvGetuun_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var idxGetuun = lvGetuun.SelectedItems[0].Index;
            if (e.Delta > 0)
            {
                //↑
                if (idxGetuun > 0)
                {
                    lvGetuun.Items[idxGetuun - 1].Selected = true;
                    lvGetuun.Items[idxGetuun - 1].Focused = true;
                }
                else
                {
                    System.Windows.Forms.MouseEventArgs lvNenunEvent = new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta);
                    lvNenun_MouseWheel(lvNenun, lvNenunEvent);

                    var idTaiun = lvTaiun.SelectedItems[0].Index;
                    var idxNenun = lvNenun.SelectedItems[0].Index;
                    if (idTaiun == 0 && idxNenun == 0) return;
                    lvGetuun.Items[lvGetuun.Items.Count - 1].Selected = true;
                    lvGetuun.Items[lvGetuun.Items.Count - 1].Focused = true;
                }


            }
            else
            {
                //↓
                if (idxGetuun < lvGetuun.Items.Count - 1)
                {
                    lvGetuun.Items[idxGetuun + 1].Selected = true;
                    lvGetuun.Items[idxGetuun + 1].Focused = true;
                }
                else
                {
                    System.Windows.Forms.MouseEventArgs lvNenunEvent = new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta);
                    lvNenun_MouseWheel(lvNenun, lvNenunEvent);

                    var idTaiun = lvTaiun.SelectedItems[0].Index;
                    var idxNenun = lvNenun.SelectedItems[0].Index;
                    if (idTaiun == lvTaiun.Items.Count-1 && idxNenun == lvNenun.Items.Count - 1) return;

                    lvGetuun.Items[0].Selected = true;
                    lvGetuun.Items[0].Focused = true;
                }

            }
        }

 
        //------------------------------------------------------------
        //  後天運ピクチャーボックス イベント
        //------------------------------------------------------------
        /// <summary>
        /// 後天運ピクチャーボックスサイズ変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) return;
            DispKoutenUn(curPerson, pictureBox2);

        }
        //大運表示チェックボックス
        private void chkDispTaiun_CheckedChanged(object sender, EventArgs e)
        {
            DispKoutenUn(curPerson, pictureBox2);
        }
        //年運表示チェックボックス
        private void chkDispNenun_CheckedChanged(object sender, EventArgs e)
        {
            DispKoutenUn(curPerson, pictureBox2);
        }
        //月運表示チェックボックス
        private void chkDispGetuun_CheckedChanged(object sender, EventArgs e)
        {
            DispKoutenUn(curPerson, pictureBox2);
            if (frmKykiSim != null) frmKykiSim.UpdateKyokiPatternYearList(curPerson);
        }
        //三合会局・方三位チェックボックス
        private void chkSangouKaikyoku_CheckedChanged(object sender, EventArgs e)
        {
            DispKoutenUn(curPerson, pictureBox2);
        }
        // 五徳表示チェックボックス
        private void chkGotoku_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGotoku.Checked)
            {
                chkGogyou.Checked = false;
            }
            grpGogyouGotoku.Enabled = chkGogyou.Checked || chkGotoku.Checked;

            DispShukumei(curPerson, pictureBox1);
            DispKoutenUn(curPerson, pictureBox2);
        }
        // 五行表示チェックボックス
        private void chkGogyou_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGogyou.Checked)
            {
                chkGotoku.Checked = false;
            }
            grpGogyouGotoku.Enabled = chkGogyou.Checked || chkGotoku.Checked;
            DispShukumei(curPerson, pictureBox1);
            DispKoutenUn(curPerson, pictureBox2);
        }
        //合法反映
        private void chkRefrectGouhou_CheckedChanged(object sender, EventArgs e)
        {
            DispShukumei(curPerson, pictureBox1);
            DispKoutenUn(curPerson, pictureBox2);

            chkRefrectSangouKaikyokuHousani.Enabled = chkRefrectGouhou.Checked;
        }

        //三合会局・方三位 反映
        private void chkSangouKaikyokuHousanni_CheckedChanged(object sender, EventArgs e)
        {
            DispShukumei(curPerson, pictureBox1);
            DispKoutenUn(curPerson, pictureBox2);
        }

        /// <summary>
        /// 虚気 変化パターン画面表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            //年運の選択行の干支取得
           var selectedItem = lvNenun.SelectedItems;
            if (selectedItem.Count == 0) return;

            curNenun = (GetuunNenunLvItemData)selectedItem[0].Tag;

            if (frmKykiSim != null)
            {
                OnFormKyokiSimulationClose();
            }


            frmKykiSim = new FromKyokiSimulation( this);
            frmKykiSim.OnClose += OnFormKyokiSimulationClose;
            frmKykiSim.Show();

            frmKykiSim.UpdateAll(curPerson, curNenun.keyValue,
                                    curGetuun.kansi, curNenun.kansi, curTaiun.kansi,
                                    chkDispGetuun.Checked,
                                    chkSangouKaikyoku.Checked,
                                    chkGogyou.Checked,
                                    chkGotoku.Checked,
                                    chkRefrectGouhou.Checked,
                                    chkRefrectSangouKaikyokuHousani.Checked
                                    );
            //}
            //else
            //{
            //    frmKykiSim.UpdateKyokiPattern(curPerson, curNenun.keyValue,
            //                            curGetuun.kansi, curNenun.kansi, curTaiun.kansi,
            //                            chkDispGetuun.Checked,
            //                            chkSangouKaikyoku.Checked,
            //                            chkGogyou.Checked,
            //                            chkGotoku.Checked,
            //                            chkRefrectGouhou.Checked,
            //                            chkRefrectSangouKaikyokuHousani.Checked
            //                            );
            //}
        }

        void OnFormKyokiSimulationClose()
        {
            frmKykiSim.Dispose();
            frmKykiSim = null;
        }

        //根気法画面表示
        private void button4_Click(object sender, EventArgs e)
        {
            frmKonkihou = new FormKonkihou();
            frmKonkihou.OnClose += OnFormKonkihouClose;


            frmKonkihou.Show();
            frmKonkihou.Update(curPerson);
        }
        void OnFormKonkihouClose()
        {
            frmKonkihou.Dispose();
            frmKonkihou = null;
        }

        /// <summary>
        /// 十二親干法　画面表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            formJuniSinKanHou = new FormJuniSinKanHou();
            formJuniSinKanHou.OnClose += OnFormJuniSinKanHouClose;

            formJuniSinKanHou.Show();
            formJuniSinKanHou.Update(curPerson);
        }
        void OnFormJuniSinKanHouClose()
        {
            formJuniSinKanHou.Dispose();
            formJuniSinKanHou = null;
        }

        /// <summary>
        /// 守護神法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            FormShugoSinHou = new FormShugoSinHou();
            FormShugoSinHou.OnClose += OnFormShugoSinHouClose;
            FormShugoSinHou.Show();
            FormShugoSinHou.Update(curPerson);

        }
        void OnFormShugoSinHouClose()
        {
            FormShugoSinHou.Dispose();
            FormShugoSinHou = null;
        }

        //=================================================
        //Owner Draw 　⇒  ListViewExに統合しました
        //=================================================
        //----------------------------------------
        // 大運 ListView OwnerDraw処理
        //----------------------------------------
        //private void lvTaiun_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        //{
        //    e.DrawDefault = true;
        //}
        //private void lvTaiun_DrawItem(object sender, DrawListViewItemEventArgs e)
        //{
        //    if ((e.State & ListViewItemStates.Selected) == ListViewItemStates.Selected)
        //    {
        //        e.DrawFocusRectangle();
        //    }
        //    // View.DetailsならばDrawSubItemイベントで描画するため、ここでは描画しない
        //    if (lvTaiun.View != View.Details)
        //    {
        //        e.DrawText();
        //    }
        //}
        //private void lvTaiun_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        //{
        //    Brush brush = new SolidBrush(e.Item.ForeColor);
        //    if (e.Item.Selected)
        //    {
        //        // Hightlightで範囲を塗りつぶす
        //        e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds);
        //    }
        //    // 上で設定した,brushとdrawFormatを利用して文字を描画する
        //    e.Graphics.DrawString(e.SubItem.Text, e.Item.Font, brush, e.Bounds);
        //    brush.Dispose();
        //}
        //----------------------------------------
        // 年運 ListView OwnerDraw処理
        //----------------------------------------
        //private void lvNenun_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        //{
        //    e.DrawDefault = true;
        //}

        //private void lvNenun_DrawItem(object sender, DrawListViewItemEventArgs e)
        //{
        //    if ((e.State & ListViewItemStates.Selected) == ListViewItemStates.Selected)
        //    {
        //        e.DrawFocusRectangle();
        //    }
        //    // View.DetailsならばDrawSubItemイベントで描画するため、ここでは描画しない
        //    if (lvNenun.View != View.Details)
        //    {
        //        e.DrawText();
        //    }
        //}

        //private void lvNenun_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        //{
        //    Brush brush = new SolidBrush(e.Item.ForeColor);
        //    if (e.Item.Selected)
        //    {
        //        // Hightlightで範囲を塗りつぶす
        //        e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds);
        //    }
        //    // 上で設定した,brushとdrawFormatを利用して文字を描画する
        //    e.Graphics.DrawString(e.SubItem.Text, e.Item.Font, brush, e.Bounds);
        //    brush.Dispose();
        //}
        //----------------------------------------
        // 月運 ListView OwnerDraw処理
        //----------------------------------------
        //private void lvGetuUn_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        //{
        //    e.DrawDefault = true;
        //}

        //private void lvGetuUn_DrawItem(object sender, DrawListViewItemEventArgs e)
        //{
        //    if ((e.State & ListViewItemStates.Selected) == ListViewItemStates.Selected)
        //    {
        //        e.DrawFocusRectangle();
        //    }
        //    // View.DetailsならばDrawSubItemイベントで描画するため、ここでは描画しない
        //    if (lvGetuU.View != View.Details)
        //    {
        //        e.DrawText();
        //    }
        //}

        //private void lvGetuUn_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        //{
        //    Brush brush = new SolidBrush(e.Item.ForeColor);
        //    if (e.Item.Selected)
        //    {
        //        // Hightlightで範囲を塗りつぶす
        //        e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds);
        //    }
        //    // 上で設定した,brushとdrawFormatを利用して文字を描画する
        //    e.Graphics.DrawString(e.SubItem.Text, e.Item.Font, brush, e.Bounds);
        //    brush.Dispose();
        //}

    }
}
