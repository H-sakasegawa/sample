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

        public delegate void CloseTab( int tabId);
        public event CloseTab onCloseTab = null;

        Form mainForm = null;
        int tabId = -1;
        string exePath = "";

        TableMng tblMng = TableMng.GetTblManage();
        Persons personList = null;
        const int GetuunDispStartGetu = 2;
        bool bControlEventEnable = true;


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
        FormUnseiViewer frmUnseiViewer = null;




        public Form1( Form mainFrm ,int _tabId, Persons _persons, Person targetPerson=null)
        {
            InitializeComponent();

            try
            {
                bControlEventEnable = false;

                mainForm = mainFrm;
                tabId = _tabId;

                lvTaiun.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.lvTaiun_MouseWheel);
                lvNenun.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.lvNenun_MouseWheel);
                lvGetuun.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.lvGetuun_MouseWheel);


                exePath = Path.GetDirectoryName(Application.ExecutablePath);

                personList = _persons;
                //setuiribiTbl = new SetuiribiTable();
                //try
                //{
                //    //節入り日テーブル読み込み
                //    tblMng.setuiribiTbl.ReadTable(exePath + @"\節入り日.xls");
                //}
                //catch (Exception e)
                //{
                //    MessageBox.Show(string.Format("節入り日テーブルが読み込めません。\n\n{0}", e.Message));
                //    return;
                //}

                //try
                //{
                //    //名簿読み込み
                //    personList.ReadPersonList(exePath + @"\名簿.xls");
                //}
                //catch (Exception e)
                //{
                //    MessageBox.Show(string.Format("名簿.xlsが読み込めません。\n{0}", e.Message));
                //    return;
                //}




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
                //chkRefrectHousani.Enabled = false;

                //グループコンボボックス設定
                UpdateGroupCombobox();

                //Properties.Settings.Default.Reload();

                //if (cmbGroup.Items.Contains(Properties.Settings.Default.Group))
                //{
                //    cmbGroup.Text = Properties.Settings.Default.Group;
                //}
                if (targetPerson == null)
                {
                    ReloadSetting();
                    btnTabClose.Visible = false;
                }
                else
                {
                    cmbGroup.Text = targetPerson.group;
                    cmbPerson.Text = targetPerson.name;

                    cmbGroup.Enabled = false;
                    cmbPerson.Enabled = false;
                    curPerson = targetPerson;
                    button7.Visible = false;
                    button8.Visible = false;
                    button9.Visible = false;


                }
            }
            finally
            {
                bControlEventEnable = true;
            }


        }

        private void ReloadSetting()
        {
            try
            {
                bControlEventEnable = false;

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            SetInitComboBox( config, "Group", cmbGroup);
            SetInitComboBox( config, "Name", cmbPerson);

            SetInitCheckBox(config, "Getuun", chkDispGetuun);
            SetInitCheckBox(config, "Nenun", chkDispNenun);
            SetInitCheckBox(config, "Taiun", chkDispTaiun);
            SetInitCheckBox(config, "SangouKaikyoku", chkSangouKaikyoku);
            SetInitCheckBox(config, "Gogyou", chkGogyou);
            SetInitCheckBox(config, "Gotoku", chkGotoku);

            }
            finally
            {
                bControlEventEnable = true;
            }


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
            if (tabId==0)
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
                config.Save();
            }
        }


        /// <summary>
        /// メンバー情報の追加ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click_1(object sender, EventArgs e)
        {
            FormPersonInfo frm = new FormPersonInfo(personList, FormPersonInfo.Mode.MODE_NEW);
            if( frm.ShowDialog() == DialogResult.OK)
            {
                SelectGroupAndPersonCombobox(curPerson);
            }
        }
        /// <summary>
        /// メンバー情報の更新ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            FormPersonInfo frm = new FormPersonInfo(personList, curPerson, FormPersonInfo.Mode.MODE_UPDATE);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                SelectGroupAndPersonCombobox(curPerson);
            }
        }
        /// <summary>
        /// メンバー削除ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            if( MessageBox.Show(string.Format("'{0}' \nを削除します。よろしいですか？", curPerson.name),
                             "削除確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                personList.Remove(curPerson);
                personList.WritePersonList();
                UpdateGroupCombobox();
            }

        }

        void SelectGroupAndPersonCombobox(Person person)
        {
            string groupName = person.group;
            if (cmbGroup.SelectedIndex == 0)
            {
                groupName = "全て";
            }
            //グループコンボボックス更新し、現在の人物のグループを選択
            UpdateGroupCombobox(groupName);
            //追加した人物を選択選択
            for (int i = 0; i < cmbPerson.Items.Count; i++)
            {
                if (((Person)cmbPerson.Items[i]).name == person.name)
                {
                    cmbPerson.SelectedIndex = i;
                    break;
                }
            }
        }

        //グループコンボボックス更新
        void UpdateGroupCombobox(string selectGroup=null)
        {

            Common.SetGroupCombobox(personList, cmbGroup, selectGroup);

         }

        public Person GetCurrentPerson()
        {
            return (Person)cmbPerson.SelectedItem;
        }

        //三合会局 表示有無チェックボックス情報取得
        public bool IsChkSangouKaikyoku()
        {
            return chkSangouKaikyoku.Checked;
        }
        //五行 表示有無チェックボックス情報取得
        public bool IsChkGogyou()
        {
            return chkGogyou.Checked;
        }
        //五徳 表示有無チェックボックス情報取得
        public bool IsChkGotoku()
        {
            return chkGotoku.Checked;
        }
        //蔵元表示有無チェックボックス情報取得
        public bool IsChkZougan()
        {
            return chkZougan.Checked;
        }

        




        private void MainProc(Person person)
        {
            try
            {
                bControlEventEnable = false;
                curPerson = person;

                int baseYear = int.Parse(txtBaseYear.Text);
                int baseMonth = int.Parse(txtBaseMonth.Text);
                int baseDay = int.Parse(txtBaseDay.Text);

                int baseNenkansiNo = int.Parse(txtBaseNenkansiNo.Text);
                int baseGekkansiNo = int.Parse(txtBaseGekkansiNo.Text);
                int baseNikkansiNo = int.Parse(txtBaseNikkansiNo.Text);

                //節入り日テーブル有効範囲チェック
                if (!tblMng.setuiribiTbl.IsContainsYear(person.birthday.year))
                {
                    MessageBox.Show("節入り日テーブルに指定された年度の情報が不足しています");
                    return;
                }
                tblMng.setuiribiTbl.Init(baseYear, baseMonth, baseDay, baseNenkansiNo, baseGekkansiNo, baseNikkansiNo);

                //ユーザ情報初期設定
                person.Init(tblMng);

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
                //============================================================
                //支合、半会、干合、方三位、三合会局反映チェックボックス
                //============================================================
                chkRefrectHankai.Checked = person.bRefrectHankai;
                chkRefrectSigou.Checked = person.bRefrectSigou;
                chkRefrectKangou.Checked = person.bRefrectKangou;
                chkRefrectHousani.Checked = person.bRefrectHousani;
                chkRefrectSangouKaikyoku.Checked = person.bRefrectSangouKaikyoku;

                //============================================================
                //陰占
                //============================================================
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

            }finally
            {
                bControlEventEnable = true;

            }

        }

        //経歴一覧表
        private void DispCarrerList(Person person)
        {
            lvCareer.Items.Clear();
            foreach (var career in person.career.dicCareer.OrderBy(c => c.Key))
            {
                var item = lvCareer.Items.Add(career.Key.ToString());
                item.SubItems.Add(career.Value.Replace("\r\n", ","));

            }

            txtCarrerMemo.Text = person.career.Memo;
        }
        //経歴メモ フォーカスロスト
        private void txtCarrerMemo_Leave(object sender, EventArgs e)
        {
            curPerson.career.Memo = txtCarrerMemo.Text;
            curPerson.career.Save();
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
            //string[] choukouShugosinKan = null;
            //var shugosinAttrs = ShugosinUtil.GetShugosinAttrs(person);   //調和の守護神（基本 or カスタム）
            //var imigamiAttrs = ShugosinUtil.GetImigamiAttrs(person);     //調和の忌神（基本 or カスタム）
            //if (imigamiAttrs.Count==0)
            //{
            //    imigamiAttrs.Add( new CustomShugosinAttr( person.choukouImigamiAttr) );
            //    choukouShugosinKan = person.choukouShugosin;
            //}

            //大運表示用の干支リストを取得
            var lstTaiunKansi = person.GetTaiunKansiList();
            for(int i=0; i< lstTaiunKansi.Count; i++)
            {
                var kansiItem = lstTaiunKansi[i];
                if ( i==0)
                {
                    //初旬
                    AddTaiunItem(person, "初旬 0～", kansiItem.kansiNo, 0);
                }
                else
                {
                    AddTaiunItem(person, string.Format("{0}旬 {1}～", i, kansiItem.startYear),
                                 kansiItem.kansiNo, kansiItem.startYear);
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
        private void AddTaiunItem(Person person , string title, int kansiNo, int startNen)
        {


            var item  = Common.GetTaiunItem(person, title, kansiNo, startNen);



            var lvItem = lvTaiun.Items.Add(title);
            //サブアイテム追加
            for (int i = (int)Const.ColNenunListView.COL_KANSI; i < item.sItems.Length; i++)
            {
                lvItem.SubItems.Add(item.sItems[i]);
            }

            //天中殺
            lvItem.ForeColor = item.colorTenchusatu;


            TaiunLvItemData itemData = new TaiunLvItemData();
            itemData.startNen = startNen;   //開始年
            itemData.startYear = startNen + person.birthday.year;
            itemData.kansi = item.targetKansi;    //干支
            itemData.bShugosin = item.bShugosin;  //守護神
            itemData.bImigami = item.bImigami;  //忌神

 
            if (item.bShugosin)
            {
                itemData.lstItemColors.Add(new LvItemColor(1, Const.colorShugosin));
            }
            else if (item.bImigami)
            {
                itemData.lstItemColors.Add(new LvItemColor(1, Const.colorImigami));
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

            int nenkansiNo = person.GetNenkansiNo(baseYear, true);
#endif

           // string[] choukouShugosinKan = null;
            //var shugosinAttrs = person.ShugosinAttrs;
            //var imigamiAttrs = person.ImigamiAttrs;
            //if (imigamiAttrs.Count==0)
            //{
            //    imigamiAttrs.Add( new  CustomShugosinAttr(  person.choukouImigamiAttr ) );
            //    choukouShugosinKan = person.choukouShugosin;
            //}

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

            //string[] choukouShugosinKan =null;
            //var shugosinAttrs = person.ShugosinAttrs;
            //var imigamiAttrs = person.ImigamiAttrs;
            //if(imigamiAttrs.Count==0)
            //{
            //    choukouShugosinKan = person.choukouShugosin;
            //    imigamiAttrs.Add( new CustomShugosinAttr( person.choukouImigamiAttr) );
            //}

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
        private void AddNenunItem(Person person, int rowKeyValue, string title, int targetkansiNo, Kansi taiunKansi,ListView lv)
        {

            AddNenunGetuunItem(person, rowKeyValue, title, targetkansiNo, taiunKansi, lv);
            var lvItem = lv.Items[lv.Items.Count - 1];

            lvItem.SubItems[(int)Const.ColNenunListView.COL_CAREER].Text = person.career.GetLineString(rowKeyValue); //経歴


        }

        private void AddNenunGetuunItem(Person person, int rowKeyValue, string title, int targetkansiNo, Kansi taiunKansi, ListView lv)
        {

            var item = Common.GetNenunGetuunItems(person,  title, targetkansiNo, taiunKansi);

    
            var lvItem = lv.Items.Add(title);
            //サブアイテム追加
            for (int i = (int)Const.ColNenunListView.COL_KANSI; i < item.sItems.Length; i++)
            {
                lvItem.SubItems.Add(item.sItems[i]);
            }

            //天中殺
            lvItem.ForeColor = item.colorTenchusatu;


            GetuunNenunLvItemData itemData = new GetuunNenunLvItemData();
            itemData.keyValue = rowKeyValue;           //年 or 月
            itemData.kansi = item.targetKansi;    //干支
            itemData.bShugosin = item.bShugosin;  //守護神
            itemData.bImigami = item.bImigami;  //忌神

            if (item.bShugosin)
            {
                itemData.lstItemColors.Add(new LvItemColor(1, Const.colorShugosin));
            }
            else if (item.bImigami)
            {
                itemData.lstItemColors.Add(new LvItemColor(1, Const.colorImigami));
            }
            //行のサブ情報を保持させておく
            lvItem.Tag = itemData;

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

            drawItem = new DrawShukumei(person, pictureBox, chkGogyou.Checked, chkGotoku.Checked, chkRefrectSigou.Checked);
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
                                        chkSangouKaikyoku.Checked,  //三合会局
                                        chkGogyou.Checked,          //五行 
                                        chkGotoku.Checked,          //五徳
                                        chkZougan.Checked,          //蔵元
                                        chkJuniSinkanHou.Checked    //十二親干法
                                        );
            drawItem2.Draw();

            //虚気変化数表示
            KyokiSimulation sim = new KyokiSimulation();

            sim.Simulation(person, curGetuun.kansi, curNenun.kansi, curTaiun.kansi, chkDispGetuun.Checked);
            //lblKyokiNum.Text = string.Format("虚気変化パターン数:{0}", sim.lstKansPattern.Count-1);
            button3.Text = string.Format("虚気変化 [ パターン数：{0} ]", sim.lstKansPattern.Count - 1);
            if (frmKykiSim != null && frmKykiSim.Visible==true)
            {
                frmKykiSim.UpdateKyokiPatternOnly(curPerson,
                                        curNenun.keyValue,
                                        curGetuun.kansi, curNenun.kansi, curTaiun.kansi,
                                        chkDispGetuun.Checked,
                                        chkSangouKaikyoku.Checked,
                                        chkGogyou.Checked,
                                        chkGotoku.Checked
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
                persons = personList.GetPersonList();
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
                                        chkRefrectSigou.Checked,
                                        chkRefrectHousani.Checked
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

            if(frmUnseiViewer!=null)
            {
                frmUnseiViewer.SelectYear(((GetuunNenunLvItemData)selectedItem[0].Tag).keyValue);
            }
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
                item.SubItems[(int)Const.ColNenunListView.COL_CAREER].Text = curPerson.career.GetLineString(itemData.keyValue);
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
            
            if (mainForm.WindowState == FormWindowState.Minimized) return;
            UpdateShukumeiAndKoutenunDraw();

        }
        //大運表示チェックボックス
        private void chkDispTaiun_CheckedChanged(object sender, EventArgs e)
        {
            UpdateShukumeiAndKoutenunDraw();
        }
        //年運表示チェックボックス
        private void chkDispNenun_CheckedChanged(object sender, EventArgs e)
        {
            UpdateShukumeiAndKoutenunDraw();
        }
        //月運表示チェックボックス
        private void chkDispGetuun_CheckedChanged(object sender, EventArgs e)
        {
            UpdateShukumeiAndKoutenunDraw();
            if (frmKykiSim != null) frmKykiSim.UpdateKyokiPatternYearList(curPerson);
        }
        //三合会局・方三位チェックボックス
        private void chkSangouKaikyoku_CheckedChanged(object sender, EventArgs e)
        {
            UpdateShukumeiAndKoutenunDraw();
        }
        //蔵元表示チェックボックス
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateShukumeiAndKoutenunDraw();
        }
        //十二親干法
        private void chkJuniSinkanHou_CheckedChanged(object sender, EventArgs e)
        {
            UpdateShukumeiAndKoutenunDraw();
        }

        // 五徳表示チェックボックス
        private void chkGotoku_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGotoku.Checked)
            {
                chkGogyou.Checked = false;
            }
            grpGogyouGotoku.Enabled = chkGogyou.Checked || chkGotoku.Checked;

            UpdateShukumeiAndKoutenunDraw();
        }
        // 五行表示チェックボックス
        private void chkGogyou_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGogyou.Checked)
            {
                chkGotoku.Checked = false;
            }
            grpGogyouGotoku.Enabled = chkGogyou.Checked || chkGotoku.Checked;
            UpdateShukumeiAndKoutenunDraw();
        }
        //支合反映
        private void chkRefrectSigou_CheckedChanged(object sender, EventArgs e)
        {
            if (!bControlEventEnable) return;

            curPerson.bRefrectSigou = chkRefrectSigou.Checked;
            personList.WritePersonList();
            UpdateShukumeiAndKoutenunDraw();

            //            chkRefrectSangouKaikyokuHousani.Enabled = chkRefrectSigou.Checked;
        }
        //半会反映
        private void chkRefrectHankai_CheckedChanged(object sender, EventArgs e)
        {
            if (!bControlEventEnable) return;
            curPerson.bRefrectHankai = chkRefrectHankai.Checked;
            personList.WritePersonList();
            UpdateShukumeiAndKoutenunDraw();
        }
        //干合反映
        private void chkRefrectKangou_CheckedChanged(object sender, EventArgs e)
        {
            if (!bControlEventEnable) return;
            curPerson.bRefrectKangou = chkRefrectKangou.Checked;
            personList.WritePersonList();
            UpdateShukumeiAndKoutenunDraw();
        }

        //方三位 反映
        private void chkRefrectHousani_CheckedChanged(object sender, EventArgs e)
        {
            if (!bControlEventEnable) return;
            curPerson.bRefrectHousani = chkRefrectHousani.Checked;
            personList.WritePersonList();
            UpdateShukumeiAndKoutenunDraw();
        }
        //三合会局 反映
        private void chkRefrectSangouKaikyoku_CheckedChanged(object sender, EventArgs e)
        {
            if (!bControlEventEnable) return;
            curPerson.bRefrectSangouKaikyoku = chkRefrectSangouKaikyoku.Checked;
            personList.WritePersonList();
            UpdateShukumeiAndKoutenunDraw();
        }
        /// <summary>
        /// 宿命図、後天運図の表示更新
        /// 年運比較表が表示されていたら、年運比較表の後天運表示も更新
        /// </summary>
        private void UpdateShukumeiAndKoutenunDraw()
        {
            DispShukumei(curPerson, pictureBox1);
            DispKoutenUn(curPerson, pictureBox2);
            if (frmUnseiViewer != null)
            {
                frmUnseiViewer.DrawKoutenun();
            }

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
                                    chkRefrectSigou.Checked,
                                    chkRefrectHousani.Checked
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
            FormShugoSinHou.OnUpdateShugosin += OnFormUpdateShugosin;

            FormShugoSinHou.Show();
            FormShugoSinHou.Update(curPerson);

        }
        void OnFormShugoSinHouClose()
        {
            FormShugoSinHou.Dispose();
            FormShugoSinHou = null;
        }

        void OnFormUpdateShugosin()
        {
            //守護神の編集で大運、年運、月運表示行が先頭行にもどされないよう
            //現在選択されてる大運、年運、月運の選択行を記録
            var idxTaiun = lvTaiun.SelectedIndices[0];
            var idxNenun = lvNenun.SelectedIndices[0];
            var idxGetuun = lvGetuun.SelectedIndices[0];

            MainProc(curPerson);

            //表示更新後、前回選択されていた行を再選択
            lvTaiun.Items[idxTaiun].Selected = true;
            lvNenun.Items[idxNenun].Selected = true;
            lvGetuun.Items[idxGetuun].Selected = true;

        }

        /// <summary>
        /// 年運比較表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            if (frmUnseiViewer == null)
            {
                frmUnseiViewer = new FormUnseiViewer(this, personList, GetCurrentPerson());
                frmUnseiViewer.OnChangeCurYear += OnFromUnseiViewerChangeYear;
                frmUnseiViewer.OnClose += OnFromUnseiViewerClose;
                frmUnseiViewer.Show();
            }

        }

        private void OnFromUnseiViewerChangeYear(int year)
        {
            DateTime dt = new DateTime(year, 2, 1);
            var bk = frmUnseiViewer;
            frmUnseiViewer = null; //年運選択時のfrmUnseiViewerへの通知をさせないための施策
            DispDateView(dt);
            frmUnseiViewer = bk;

        }
        void OnFromUnseiViewerClose()
        {
            frmUnseiViewer.OnChangeCurYear -= OnFromUnseiViewerChangeYear;
            frmUnseiViewer.OnClose -= OnFromUnseiViewerClose;
            frmUnseiViewer.Dispose();
            frmUnseiViewer = null;
        }

        /// <summary>
        /// タブ終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTabClose_Click(object sender, EventArgs e)
        {
            if(onCloseTab!=null) onCloseTab(tabId);
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
