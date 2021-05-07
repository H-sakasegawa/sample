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
        Persons personList = null;

        //----------------------------------------------
        //ラベルの組み合わせを登録
        //----------------------------------------------
        //日干支 ラベル
        List<Label> lstLblNikkansi;
        //月干支 ラベル
        List<Label> lstLblGekkansi;
        //年干支 ラベル
        List<Label> lstLblNenkansi;

        //日蔵元 ラベル
        List<Label> lstLblNikkansiZougan;
        //月蔵元 ラベル
        List<Label> lstLblGekkansiZougan;
        //年蔵元 ラベル
        List<Label> lstLblNenkansiZougan;


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

        //宿命 描画オブジェクト
        ShukumeiDraw drawItem = null;
        //後天運 描画オブジェクト
        KoutenUnDraw drawItem2 = null;

        //表示対象データ
        Person curPerson = null;


        public Form1()
        {
            InitializeComponent();

            lvTaiun.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.lvTaiun_MouseWheel);
            lvNenun.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.lvNenun_MouseWheel);


            exePath = Path.GetDirectoryName(Application.ExecutablePath);

            personList = new Persons();
            setuiribiTbl = new SetuiribiTable();
            try
            {
                setuiribiTbl.ReadTable(exePath + @"\節入り日.xls");
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("節入り日テーブルが読み込めません。\n\n{0}", e.Message));
                return;
            }

            try
            {
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
            //日干支 ラベル
            lstLblNikkansi = new List<Label>() { lblNikkansi1, lblNikkansi2 };
            //月干支 ラベル
            lstLblGekkansi = new List<Label>() { lblGekkansi1, lblGekkansi2 };
            //年干支 ラベル
            lstLblNenkansi = new List<Label>() { lblNenkansi1, lblNenkansi2 };

            //日干支 天中殺 ラベル
            lstLblNikkansiTenchusatu = new List<Label>() { lblNikkansiTenchusatu1, lblNikkansiTenchusatu2 };
            //年干支 天中殺 ラベル
            lstLblNenkansiTenchusatu = new List<Label>() { lblNenkansiTenchusatu1, lblNenkansiTenchusatu2 };

            //日干支 二十八元素 ラベル
            lstLblNikkansiNijuhachiGenso = new List<Label>() { lblNikkansiShogen, lblNikkansiChugen, lblNikkansiHongen };
            //月干支 二十八元素 ラベル
            lstLblGekkansiNijuhachiGenso = new List<Label>() { lblGekkansiShogen, lblGekkansiChugen, lblGekkansiHongen };
            //年干支 二十八元素 ラベル
            lstLblNenkansiNijuhachiGenso = new List<Label>() { lblNenkansiShogen, lblNenkansiChugen, lblNenkansiHongen };


            lstLblNikkansiZougan = new List<Label> { lblNikkansiShogen, lblNikkansiChugen, lblNikkansiHongen };
            lstLblGekkansiZougan = new List<Label> { lblGekkansiShogen, lblGekkansiChugen, lblGekkansiHongen };
            lstLblNenkansiZougan = new List<Label> { lblNenkansiShogen, lblNenkansiChugen, lblNenkansiHongen };


            txtNikkansiSanshutuSu_TextChanged(null, null);

            for (int i = 0; i < personList.Count; i++)
            {
                cmbPerson.Items.Add(personList[i]);
            }
            if (cmbPerson.Items.Count > 0)
            {
                cmbPerson.SelectedIndex = 0;
            }

            int baseYear = 0;
            int baseMonth = 0;
            int baseDay = 0;
            int baseNenkansi = 0;
            int baseGekkansi = 0;
            int baseNikkansiSanshutusuu = 0;
            //節入り日テーブルの先頭データを基準に基準情報を取得
            setuiribiTbl.GetBaseSetuiribiData(ref baseYear, ref baseMonth, ref baseDay,
                                              ref baseNenkansi, ref baseGekkansi, ref baseNikkansiSanshutusuu);
            txtBaseYear.Text = baseYear.ToString();
            txtBaseMonth.Text = baseMonth.ToString();
            txtBaseDay.Text = baseDay.ToString();
            txtBaseNenkansiNo.Text = baseNenkansi.ToString();
            txtBaseGekkansiNo.Text = baseGekkansi.ToString();
            txtNikkansiSanshutuSu.Text = baseNikkansiSanshutusuu.ToString();

        }


        private void button1_Click(object sender, EventArgs e)
        {
            Person person = new Person(cmbPerson.Text,
                                        int.Parse(txtYear.Text),
                                        int.Parse(txtMonth.Text),
                                        int.Parse(txtDay.Text),
                                        radMan.Checked ? Gender.NAN : Gender.WOMAN
                                        );
            MainProc(person);
        }

        private void MainProc(Person person)
        {
            curPerson = person;

            dataMng = new TableMng();

            int baseYear = int.Parse(txtBaseYear.Text);
            int baseMonth = int.Parse(txtBaseMonth.Text);
            int baseDay = int.Parse(txtBaseDay.Text);

            int baseNenkansiNo = int.Parse(txtBaseNenkansiNo.Text);
            int baseGekkansiNo = int.Parse(txtBaseGekkansiNo.Text);
            int baseNikkansiNo = int.Parse(txtBaseNikkansiNo.Text);

            //節入り日テーブル有効範囲チェック
            if ( !setuiribiTbl.IsContainsYear(person.birthday.year))
            {
                MessageBox.Show("節入り日テーブルに指定された年度の情報が不足しています");
                return;
            }
            setuiribiTbl.Init(baseYear, baseMonth, baseDay, baseNenkansiNo, baseGekkansiNo, baseNikkansiNo);

            //ユーザ情報初期設定
            person.Init(dataMng, setuiribiTbl);

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

            lblNikkansi1.Text = Nikkansi.kan;
            lblNikkansi2.Text = Nikkansi.si;

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

            lblGekkansi1.Text = Gekkansi.kan;
            lblGekkansi2.Text = Gekkansi.si;

            //------------------
            //年干支
            //------------------
            var Nenkansi = person.nenkansi;

            lblNenkansi1.Text = Nenkansi.kan;
            lblNenkansi2.Text = Nenkansi.si;



            //------------------
            //二十八
            //------------------
            NijuhachiGenso gensoNikkansi = person.nijuhachiGensoNikkansi;
            NijuhachiGenso gensoGekkansi = person.nijuhachiGensoGekkansi;
            NijuhachiGenso gensoNenkansi = person.nijuhachiGensoNenkansi;

            //十大主星判定用基準元素
            var idxNikkansiGensoType = (int)gensoNikkansi.GetTargetGensoType(person.dayNumFromSetuiribi);
            var idxGekkansiGensoType = (int)gensoGekkansi.GetTargetGensoType(person.dayNumFromSetuiribi);
            var idxNenkaisiGensoType = (int)gensoNenkansi.GetTargetGensoType(person.dayNumFromSetuiribi);

            foreach (var Value in Enum.GetValues(typeof(NijuhachiGenso.enmGensoType)))//初元、中元、本元
            {
                Label label = lstLblNikkansiZougan[(int)Value];
                label.Text = gensoNikkansi.genso[(int)Value].name;
                if(idxNikkansiGensoType== (int)Value) Common.SetBold(label, true);
                else                                  Common.SetBold(label, false);

                label = lstLblGekkansiZougan[(int)Value];
                label.Text = gensoGekkansi.genso[(int)Value].name;
                if (idxGekkansiGensoType == (int)Value) Common.SetBold(label, true);
                else                                    Common.SetBold(label, false);

                label = lstLblNenkansiZougan[(int)Value];
                label.Text = gensoNenkansi.genso[(int)Value].name;
                if (idxNenkaisiGensoType == (int)Value) Common.SetBold(label, true);
                else                                    Common.SetBold(label, false);
            }

            //============================================================
            //陽占
            //============================================================
            DispYousen(person, idxNikkansiGensoType, idxGekkansiGensoType, idxNenkaisiGensoType);

            //============================================================
            //天中殺
            //============================================================
            DispTenchusatu(person); 

            //============================================================
            //後天運：大運
            //============================================================
            DispTaiun(person);


            //============================================================
            //位相法
            //============================================================
            DispShukumei(person);

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
        /// 天中殺
        /// </summary>
        private void DispTenchusatu(Person person)
        {
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

            Kansi Nikkansi = person.nikkansi;
            Kansi Nenkansi = person.nenkansi;

            string[] NikkansiTenchusatu = Nikkansi.tenchusatu.ToArray();
            string[] NenkansiTenchusatu = Nenkansi.tenchusatu.ToArray();
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

        //====================================================
        // 大運 表示処理
        //====================================================
        /// <summary>
        /// 大運
        /// </summary>
        /// <param name="nenkansiNo"></param>
        private void DispTaiun(Person person )
        {
            lvTaiun.Items.Clear();

            int nenkansiNo = person.nenkansiNo;

            //初旬干支番号
            int kansiNo = person.gekkansiNo;

            //順行、逆行
            int dirc = Direction(person);

            int Year = int.Parse(txtYear.Text);
            int Month = int.Parse(txtMonth.Text);
            int Day = int.Parse(txtDay.Text);
            //才運
            int dayCnt = 0;
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

            lvTaiun.Items[0].Selected = true;

        }
        /// <summary>
        /// 大運 順行、逆行判定
        /// </summary>
        /// <param name="NenkansiNo"></param>
        /// <returns></returns>
        private int Direction(Person person)
        {
            var Nenkansi = person.nenkansi;// dataMng.kansiMng.dicKansi[ person.nenkansiNo ];

            //性別
            if (radMan.Checked)
            {   //男性
                if (dataMng.jyukanTbl[Nenkansi.kan].inyou == "+") return 1;
                else return -1;
            }
            else
            {   //女性
                if (dataMng.jyukanTbl[Nenkansi.kan].inyou == "+") return -1;
                else return 1;
            }
        }
        /// <summary>
        /// 大運 行データ追加
        /// </summary>
        /// <param name="title"></param>
        /// <param name="kansiNo"></param>
        private void AddTaiunItem(Person person , string title, int kansiNo, int startNen)
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

            //日
            string nentin = person.GetNentin(taiunKansi, person.nikkansi); //納音、準納音
            string rittin = person.GetNittin(taiunKansi, person.nikkansi); //律音、準律音
            string tensatu = person.GetTensatuTichuString(taiunKansi, person.nikkansi);//天殺地冲
            bool bExistNentin = (nentin == "" ? false : true);
            bool bExistTensatuTichu = (tensatu == "" ? false : true);

            string gouhou = person.GetGouhouSanpouString(taiunKansi, person.nikkansi, bExistTensatuTichu, bExistNentin); //合法・散法
            if(gouhou.IndexOf("半会")>=0)
            {

            }
            string kangou = person.GetKangoStr(taiunKansi, person.nikkansi); //干合            
            string value = nentin + " " + rittin + " "+tensatu+ " " + kangou + " " + gouhou;
            lvItem.SubItems.Add(value.Trim());

            //月
            nentin = person.GetNentin(taiunKansi, person.gekkansi); //納音、準納音
            rittin = person.GetNittin(taiunKansi, person.gekkansi); //律音、準律音
            tensatu = person.GetTensatuTichuString(taiunKansi, person.gekkansi);//天殺地冲
            bExistNentin = (nentin == "" ? false : true);
            bExistTensatuTichu = (tensatu == "" ? false : true);

            gouhou = person.GetGouhouSanpouString(taiunKansi, person.gekkansi, bExistTensatuTichu, bExistNentin); //合法・散法
            kangou = person.GetKangoStr(taiunKansi, person.gekkansi); //干合            
            value = nentin + " " + rittin + " " + tensatu + " " + kangou + " " + gouhou;
            lvItem.SubItems.Add(value.Trim());

            //年
            nentin = person.GetNentin(taiunKansi, person.nenkansi); //納音、準納音
            rittin = person.GetNittin(taiunKansi, person.nenkansi); //律音、準律音
            tensatu = person.GetTensatuTichuString(taiunKansi, person.nenkansi);//天殺地冲
            bExistNentin = (nentin == "" ? false : true);
            bExistTensatuTichu = (tensatu == "" ? false : true);

            gouhou = person.GetGouhouSanpouString(taiunKansi, person.nenkansi, bExistTensatuTichu, bExistNentin);//合法・散法
            kangou = person.GetKangoStr(taiunKansi, person.nenkansi); //干合            
            value = nentin + " " + rittin + " " + tensatu + " " + kangou + " " + gouhou;
            lvItem.SubItems.Add(value.Trim());
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

            TaiunLvItemData itemData = new TaiunLvItemData();
            itemData.startNen = startNen;   //開始年
            itemData.kansi = taiunKansi;    //干支

            //行のサブ情報を保持させておく
            lvItem.Tag = itemData;


        }


        //====================================================
        // 年運 表示処理
        //====================================================
        /// <summary>
        /// 年運
        /// </summary>
        /// <param name="baseYear">大運で選択された行の開始年</param>
        private void DispNenun(Person person ,int startNen)
        {
            lvNenun.Items.Clear();

            int year = person.birthday.year + startNen;
            int Month = person.birthday.month;
            int Day = person.birthday.day;

            //0才 干支番号
            int nenkansiNo = person.nenkansiNo;

            //選択された大運年度の開始干支番号
            nenkansiNo += year - person.birthday.year;
            nenkansiNo = nenkansiNo % 60;
            if (nenkansiNo == 0) nenkansiNo = 60;

            //11年分を表示
            for (int i = 0; i < 10+1; i++)
            {
                //順行のみなので、60超えたら1にするだけ
                if (nenkansiNo > 60) nenkansiNo = 1;

                AddNenunItem(person, string.Format("{0}歳({1})", (year +i) - person.birthday.year,  year +i),
                             nenkansiNo);
                nenkansiNo += 1;
            }
            lvNenun.Items[0].Selected = true;
            lvNenun.Items[0].Focused = true;

        }

        /// <summary>
        /// 年運 行データ追加
        /// </summary>
        /// <param name="title"></param>
        /// <param name="kansiNo"></param>
        private void AddNenunItem(Person person , string title, int nenunkansiNo)
        {

            Kansi nenunKansi = person.GetKansi( nenunkansiNo );

            var lvItem = lvNenun.Items.Add(title);
            lvItem.SubItems.Add(string.Format("{0}{1}", nenunKansi.kan, nenunKansi.si)); //干支

            string judai = person.GetJudaiShusei(person.nikkansi.kan, nenunKansi.kan).name;
            string junidai = person.GetJunidaiShusei(person.nikkansi.kan, nenunKansi.si).name;


            //"星"を削除
            judai = judai.Replace("星", "");
            junidai = junidai.Replace("星", "");

            lvItem.SubItems.Add(judai); //十大主星
            lvItem.SubItems.Add(junidai); //十二大従星

            //日
            string nentin = person.GetNentin(nenunKansi, person.nikkansi); //納音、準納音
            string rittin = person.GetNittin(nenunKansi, person.nikkansi); //律音、準律音
            string tensatu = person.GetTensatuTichuString(nenunKansi, person.nikkansi);//天殺地冲
            bool bExistNentin = (nentin == "" ? false : true);
            bool bExistTensatuTichu = tensatu == "" ? false : true;

            string gouhou = person.GetGouhouSanpouString(nenunKansi, person.nikkansi, bExistTensatuTichu, bExistNentin);//合法・散法
            string kangou = person.GetKangoStr(nenunKansi, person.nikkansi); //干合            
            string value = nentin + " " + rittin + " " + tensatu + " " + kangou+" "+gouhou; ;
            lvItem.SubItems.Add(value.Trim());

            //月
            nentin = person.GetNentin(nenunKansi, person.gekkansi); //納音、準納音
            rittin = person.GetNittin(nenunKansi, person.gekkansi); //律音、準律音
            tensatu = person.GetTensatuTichuString(nenunKansi, person.gekkansi);//天殺地冲
            bExistNentin = (nentin == "" ? false : true);
            bExistTensatuTichu = (tensatu==""?false:true);

            gouhou = person.GetGouhouSanpouString(nenunKansi, person.gekkansi, bExistTensatuTichu, bExistNentin);//合法・散法
            kangou = person.GetKangoStr(nenunKansi, person.gekkansi); //干合            
            value = nentin + " " + rittin + " " + tensatu + " " + kangou + " " + gouhou; ;
            lvItem.SubItems.Add(value);

            //年
            nentin = person.GetNentin(nenunKansi, person.nenkansi); //納音、準納音
            rittin = person.GetNittin(nenunKansi, person.nenkansi); //律音、準律音
            tensatu = person.GetTensatuTichuString(nenunKansi, person.nenkansi);//天殺地冲
            bExistNentin = (nentin == "" ? false : true);
            bExistTensatuTichu = (tensatu == "" ? false : true);

            gouhou = person.GetGouhouSanpouString(nenunKansi, person.nenkansi, bExistTensatuTichu, bExistNentin);//合法・散法
            kangou = person.GetKangoStr(nenunKansi, person.nenkansi); //干合            
            value = nentin + " " + rittin + " " + tensatu + " " + kangou + " " + gouhou; ;
            lvItem.SubItems.Add(value); 


            //天中殺
            Color color = Color.Black;
            for (int i = 0; i < lstLblNenkansiTenchusatu.Count; i++)
            {
                if (nenunKansi.kan == person.nikkansi.tenchusatu[i] ||
                   nenunKansi.si == person.nikkansi.tenchusatu[i])
                {
                    color = Color.Red;
                    break;
                }
            }

            lvItem.ForeColor = color;

            NenunLvItemData itemData = new NenunLvItemData();
            itemData.kansi = nenunKansi;    //干支
            //行のサブ情報を保持させておく
            lvItem.Tag = itemData;

        }


        /// <summary>
        /// 大運リストビューアイテムデータクラス
        /// </summary>
        class TaiunLvItemData
        {
            public int startNen; //開始年
            public Kansi kansi; //干支
        }
        /// <summary>
        /// 年運リストビューアイテムデータクラス
        /// </summary>
        class NenunLvItemData
        {
            public Kansi kansi; //干支
        }


        //============================================================
        //位相法
        //============================================================
        abstract class IsouhouBase : IDisposable
        {
            public Person person;
            Font fnt = null;
            Font fntSmall = null;
            Font fntSmallDisable = null;
            Pen blackPen = null;
            StringFormat stringFormat = null;
            StringFormat smallStringFormat = null;
            PictureBox pictureBox = null;
            Graphics g;
            int offsetY = 20;

            public string[] strInyou = new string[] { "陰陽" };
            public string[] strKangou = new string[] { "干合" };
            public string[] strNanasatu = new string[] { "七殺" };

            // 4:[ ][ ][ ] 
            // 3:[ ][ ][ ]  (設定例）
            // 2:[ ][ ][ ]  日年[1][0][1]  (bitFlgNiti | bitFlgNen) 
            // 1:[ ][ ][ ]  月年[0][1][1]  (bitFlgGetu | bitFlgNen) 
            // 0:[ ][ ][ ]  日月[1][1][0]  (bitFlgNiti | bitFlgGetu)
            protected List<int> matrix = new List<int>();
            protected List<int> matrixBottom = new List<int>();
            protected int idxMtx = 0;
            protected int idxMtxButtom = 0;

            public IsouhouBase(Person _person, PictureBox _pictureBox)
            {
                person = _person;
                pictureBox = _pictureBox;


                blackPen = new Pen(Color.Black, 1); ;

                fnt = new Font("MS Gothic", 14, FontStyle.Regular);
                fntSmall = new Font("MS Gothic", 8, FontStyle.Regular);
                fntSmallDisable = new Font("MS Gothic", 8, FontStyle.Regular| FontStyle.Strikeout);


                //干支文字センタリング表示用フォーマット
                stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                smallStringFormat = new StringFormat();
                smallStringFormat.Alignment = StringAlignment.Center;
                smallStringFormat.LineAlignment = StringAlignment.Center;

                matrix.Add(0);
                matrixBottom.Add(0);
            }

            public Size GetDrawArea()
            {
                return new Size(pictureBox.Width, pictureBox.Height);
            }

            public int GetFontHeight()
            {
                return fnt.Height;
            }
            public int GetSmallFontHeight()
            {
                return fntSmall.Height;
            }
            public int GetLineOffsetY()
            {
                return offsetY;
            }

            public abstract void DrawItem(Graphics g);

            public void Draw()
            {
                //派生先クラスの描画I/F呼び出し
                Bitmap canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
                // Graphicsオブジェクトの作成
                g = Graphics.FromImage(canvas);

                DrawItem(g);

                pictureBox.Image = canvas;

            }

            public void Dispose()
            {
                blackPen.Dispose();
                fnt.Dispose();
            }


            protected void DrawKansi(Kansi kansi, Rectangle rectKan, Rectangle rectSi)
            {
                g.DrawString(kansi.kan, fnt, Brushes.Black, rectKan, stringFormat);
                g.DrawString(kansi.si, fnt, Brushes.Black, rectSi, stringFormat);
                g.DrawRectangle(blackPen, rectKan);
                g.DrawRectangle(blackPen, rectSi);
            }

            protected void DrawLine( int mtxIndex, int fromX, int toX, int baseY, int dirc)
            {
                Point start = new Point(fromX, baseY);
                Point end = new Point(toX, baseY);
                Point startOfs = new Point(start.X, start.Y + ((mtxIndex + 1) * offsetY) * dirc);
                Point endOfs = new Point(end.X, end.Y + ((mtxIndex + 1) * offsetY) * dirc);

                g.DrawLine(blackPen, start, startOfs);
                g.DrawLine(blackPen, startOfs, endOfs);
                g.DrawLine(blackPen, endOfs, end);

            }

            protected void DrawString(Rectangle rect, string s)
            {
                SizeF w = g.MeasureString(s, fntSmall);
                Rectangle fillRect = rect;
                if (fillRect.Width > w.Width)
                {
                    fillRect.X = (int)(fillRect.X + (rect.Width - w.Width) / 2);
                    fillRect.Width = (int)w.Width;
                }
                g.FillRectangle(Brushes.WhiteSmoke, fillRect);
                g.DrawString(s, fntSmall, Brushes.Black, rect, smallStringFormat);
            }
            protected void DrawString( int mtxIndex, int from, int to, int baseY, int dirc, string[] strs, int enableBit=0xFFFF)
            {
                float maxWidth = 0f;
                float sumHeight = 0f;
                //文字列の最大幅,高さ取得
                foreach (var s in strs)
                {
                    SizeF w = g.MeasureString(s, fntSmall);
                    if (maxWidth < w.Width) maxWidth = w.Width;

                    sumHeight += w.Height;
                }

                int x = from + (Math.Abs(from - to) - (int)Math.Ceiling(maxWidth)) / 2;
                int y = (int)(baseY + ((mtxIndex + 1) * offsetY) * dirc - Math.Ceiling(sumHeight) / 2) + 2;

                int bit= 0x01;
                foreach (var s in strs)
                {

                    Rectangle rect = new Rectangle(x, y, (int)Math.Ceiling(maxWidth), fntSmall.Height);
                    g.FillRectangle(Brushes.WhiteSmoke, rect);

                    if ((enableBit & bit)!=0)
                    {
                        g.DrawString(s, fntSmall, Brushes.Black, rect, smallStringFormat);
                    }
                    else
                    {
                        g.DrawString(s, fntSmallDisable, Brushes.Gray, rect, smallStringFormat);
                    }
                    bit <<= 1;

                    y += fntSmall.Height;
                }

            }

            public int SetMatrixUp(bool bFlg, int mtxCheckBits, int mtxSetBits)
            {
                return SetMatrix(bFlg, mtxCheckBits, mtxSetBits, ref matrix, ref idxMtx);
            }
            public int SetMatrixDown(bool bFlg, int mtxCheckBits, int mtxSetBits)
            {
                return SetMatrix(bFlg, mtxCheckBits, mtxSetBits, ref matrixBottom, ref idxMtxButtom);
            }
            public int SetMatrix(bool bFlg, int mtxCheckBits, int mtxSetBits, ref List<int> matrix, ref int idxMtx)
            {
                if (bFlg)
                {
                    for (int i = 0; i < matrix.Count; i++)
                    {
                        if ((matrix[i] & mtxCheckBits) == 0)
                        {
                            matrix[i] |= mtxSetBits;
                            return i;
                        }
                    }
                    matrix.Add(0);
                    idxMtx++;
                    matrix[idxMtx] |= mtxSetBits;
                }
                return idxMtx;
            }
        }
        /// <summary>
        /// 宿命 表示用クラス
        /// </summary>
        class ShukumeiDraw : IsouhouBase
        {

            public Point nikkansi;
            public Point gekkansi;
            public Point nenkansi;

            public int nikkansiCenterX;
            public int gekkansiCenterX;
            public int nenkansiCenterX;

            public int drawTopKan;      //干文字表示領域TOP
            public int drawTopSi;       //支文字表示領域TOP
            public int drawBottomSi;    //支文字表示領域BOTTOM
            public int rangeHeight;     //干支文字領域高さ
            public int rangeWidth;      //干支文字領域幅



            //干支文字表示領域
            Rectangle rectNikansiKan;
            Rectangle rectNikansiSi;
            Rectangle rectGekkansiKan;
            Rectangle rectGekkansiSi;
            Rectangle rectNenkansiKan;
            Rectangle rectNenkansiSi;


            const int bitFlgNiti = 0x04;
            const int bitFlgGetu = 0x02;
            const int bitFlgNen = 0x01;





            public ShukumeiDraw(Person person, PictureBox pictureBox) :
                base(person, pictureBox)
            {

                rangeHeight = GetFontHeight() * 2;
                rangeWidth = 45;

            }
            private void CalcCoord()
            { 
                nikkansi.X = 5;
                //nikkansi.Y = GetDrawArea().Height / 2 - rangeHeight;
                nikkansi.Y = (idxMtx+1) * GetLineOffsetY()+10;
                nikkansiCenterX = nikkansi.X + rangeWidth / 2;

                gekkansi.X = nikkansi.X + rangeWidth;
                gekkansi.Y = nikkansi.Y;
                gekkansiCenterX = gekkansi.X + rangeWidth / 2;

                nenkansi.X = gekkansi.X + rangeWidth;
                nenkansi.Y = gekkansi.Y;
                nenkansiCenterX = nenkansi.X + rangeWidth / 2;

                drawTopKan = nikkansi.Y;
                drawTopSi = drawTopKan + rangeHeight;
                drawBottomSi = drawTopSi + rangeHeight;


                //干支表示領域
                rectNikansiKan = new Rectangle(nikkansi.X, nikkansi.Y, rangeWidth, rangeHeight);
                rectNikansiSi = new Rectangle(nikkansi.X, drawTopSi, rangeWidth, rangeHeight);
                rectGekkansiKan = new Rectangle(gekkansi.X, gekkansi.Y, rangeWidth, rangeHeight);
                rectGekkansiSi = new Rectangle(gekkansi.X, drawTopSi, rangeWidth, rangeHeight);
                rectNenkansiKan = new Rectangle(nenkansi.X, nenkansi.Y, rangeWidth, rangeHeight);
                rectNenkansiSi = new Rectangle(nenkansi.X, drawTopSi, rangeWidth, rangeHeight);


            }



            public override void DrawItem(Graphics g)
            {
                if (person == null) return;

                int dircUp = -1;
                int dircDown = +1;

                //干支表示領域の上部表示エリア必要サイズ計算

                //陰陽
                //-------------------
                bool bInyouNitiGetsuKan = person.IsInyouNitiGetsuKan(); //干（日-月) の関係
                bool bInyouGetsuNenKan = person.IsInyouGetsuNenKan();//干（月-年) の関係
                bool bInyouNiItiNenKan = person.IsInyouNitiNenKan();//干（日-年) の関係
                int idxInyouNitiGetsuKan = SetMatrixUp(bInyouNitiGetsuKan, bitFlgNiti, (bitFlgNiti | bitFlgGetu));//日 - 月
                int idxInyouGetsuNenKan = SetMatrixUp(bInyouGetsuNenKan, bitFlgNen, (bitFlgGetu | bitFlgNen));//月 - 年
                int idxInyouNiItiNenKan = SetMatrixUp(bInyouNiItiNenKan, (bitFlgNiti | bitFlgNen), (bitFlgNiti | bitFlgNen));//日 - 年

                //干合
                //-------------------

                bool bKangouNitiGetsuKan = person.IsKangouNitiGetsuKan();//干合（日-月) の関係
                bool bKangouGetsuNenKan = person.IsKangoGetsuNenKan(); //干合（月-年) の関係
                bool bKangouNiItiNenKan = person.IsKangoNitiNenKan(); //干合（日-年) の関係
                int idxKangouNitiGetsuKan = SetMatrixUp(bKangouNitiGetsuKan, bitFlgNiti, (bitFlgNiti | bitFlgGetu));//日 - 月
                int idxKangouGetsuNenKan = SetMatrixUp(bKangouGetsuNenKan, bitFlgNen, (bitFlgGetu | bitFlgNen));//月 - 年
                int idxKangouNiItiNenKan = SetMatrixUp(bKangouNiItiNenKan, (bitFlgNiti | bitFlgNen), (bitFlgNiti | bitFlgNen));//日 - 年

                //七殺
                //-------------------
                bool bNanasatuNitGetu = person.IsNanasatuNitiGetuKan();
                bool bNanasatuGetuNen = person.IsNanasatuGetuNenKan();
                bool bNanasatuNitNen = person.IsNanasatuNitiNenKan();
                int idxNanasatuNitGetu = SetMatrixUp(bNanasatuNitGetu, bitFlgNiti, (bitFlgNiti | bitFlgGetu));//日 - 月
                int idxNanasatuGetuNen = SetMatrixUp(bNanasatuGetuNen, bitFlgNen, (bitFlgGetu | bitFlgNen));//月 - 年
                int idxNanasatuNitNen = SetMatrixUp(bNanasatuNitNen, (bitFlgNiti | bitFlgNen), (bitFlgNiti | bitFlgNen));//日 - 年


                //干支の上部に表示する情報の段数から干支表示基準座標を計算
                CalcCoord();

                //干支表示
                DrawKansi(person.nikkansi, rectNikansiKan, rectNikansiSi);
                DrawKansi(person.gekkansi, rectGekkansiKan, rectGekkansiSi);
                DrawKansi(person.nenkansi, rectNenkansiKan, rectNenkansiSi);

                //ライン描画
                //陰陽
                //-------------------

                if (bInyouNitiGetsuKan)//日 - 月
                {
                    DrawLine(idxInyouNitiGetsuKan, nikkansiCenterX, gekkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxInyouNitiGetsuKan, nikkansiCenterX, gekkansiCenterX, drawTopKan, dircUp, strInyou);
                }
                if (bInyouGetsuNenKan)//月 - 年
                {
                    DrawLine(idxInyouGetsuNenKan, gekkansiCenterX, nenkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxInyouGetsuNenKan, gekkansiCenterX, nenkansiCenterX, drawTopKan, dircUp, strInyou);
                }
                if (bInyouNiItiNenKan)//日 - 年
                {
                    DrawLine(idxInyouNiItiNenKan, nikkansiCenterX, nenkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxInyouNiItiNenKan, nikkansiCenterX, nenkansiCenterX, drawTopKan, dircUp, strInyou);
                }
                //干合
                //-------------------
                if (bKangouNitiGetsuKan)//日 - 月
                {
                    DrawLine(idxKangouNitiGetsuKan, nikkansiCenterX, gekkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxKangouNitiGetsuKan, nikkansiCenterX, gekkansiCenterX, drawTopKan, dircUp, strKangou);
                }
                if (bKangouGetsuNenKan)//月 - 年
                {
                    DrawLine(idxKangouGetsuNenKan, gekkansiCenterX, nenkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxKangouGetsuNenKan, gekkansiCenterX, nenkansiCenterX, drawTopKan, dircUp, strKangou);
                }
                if (bKangouNiItiNenKan)//日 - 年
                {
                    DrawLine(idxKangouNiItiNenKan, nikkansiCenterX, nenkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxKangouNiItiNenKan, nikkansiCenterX, nenkansiCenterX, drawTopKan, dircUp, strKangou);
                }
                //七殺
                //-------------------
                if (bNanasatuNitGetu)//日 - 月
                {
                    DrawLine(idxNanasatuNitGetu, nikkansiCenterX, gekkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxNanasatuNitGetu, nikkansiCenterX, gekkansiCenterX, drawTopKan, dircUp, strNanasatu);
                }
                if (bNanasatuGetuNen)//月 - 年
                {
                    DrawLine(idxNanasatuGetuNen, gekkansiCenterX, nenkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxNanasatuGetuNen, gekkansiCenterX, nenkansiCenterX, drawTopKan, dircUp, strNanasatu);
                }
                if (bNanasatuNitNen)//日 - 年
                {
                    DrawLine(idxNanasatuNitNen, nikkansiCenterX, nenkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxNanasatuNitNen, nikkansiCenterX, nenkansiCenterX, drawTopKan, dircUp, strNanasatu);
                }

                //合法・散法
                //-------------------
                string[] gouhouSanpouNitiGetu = person.GetGouhouSanpouNitiGetu();
                string[] gouhouSanpouGetuNen = person.GetGouhouSanpouiGetuNen();
                string[] gouhouSanpouNitiNen = person.GetGouhouSanpouiNitiNen();

                int idxGouhouSanpouNitiGetu = SetMatrixDown((gouhouSanpouNitiGetu != null), bitFlgNiti, (bitFlgNiti | bitFlgGetu));//日 - 月
                int idxGouhouSanpouGetuNen = SetMatrixDown((gouhouSanpouGetuNen != null), bitFlgNen, (bitFlgGetu | bitFlgNen));//月 - 年
                int idxGouhouSanpouNitiNen = SetMatrixDown((gouhouSanpouNitiNen != null), (bitFlgNiti | bitFlgNen), (bitFlgNiti | bitFlgNen));//日 - 年

                if (gouhouSanpouNitiGetu != null)
                {
                    DrawLine(idxGouhouSanpouNitiGetu, nikkansiCenterX, gekkansiCenterX, drawBottomSi, dircDown);
                    DrawString(idxGouhouSanpouNitiGetu, nikkansiCenterX, gekkansiCenterX, drawBottomSi, dircDown, gouhouSanpouNitiGetu);
                }
                if (gouhouSanpouGetuNen != null)
                {
                    DrawLine(idxGouhouSanpouGetuNen, gekkansiCenterX, nenkansiCenterX, drawBottomSi, dircDown);
                    DrawString(idxGouhouSanpouGetuNen, gekkansiCenterX, nenkansiCenterX, drawBottomSi, dircDown, gouhouSanpouGetuNen);
                }

                if (gouhouSanpouNitiNen != null)
                {
                    DrawLine(idxGouhouSanpouNitiNen, nikkansiCenterX, nenkansiCenterX, drawBottomSi, dircDown);
                    DrawString(idxGouhouSanpouNitiNen, nikkansiCenterX, nenkansiCenterX, drawBottomSi, dircDown, gouhouSanpouNitiNen);
                } 
            }
        }
        /// <summary>
        /// 後天運 表示用クラス
        /// </summary>
        class KoutenUnDraw : IsouhouBase
        {

            public Point nenun;
            public Point taiun;
            public Point nikkansi;
            public Point gekkansi;
            public Point nenkansi;

            public int nenunCenterX;
            public int taiunCenterX;
            public int nikkansiCenterX;
            public int gekkansiCenterX;
            public int nenkansiCenterX;

            public int drawTopKan;      //干文字表示領域TOP
            public int drawTopSi;       //支文字表示領域TOP
            public int drawBottomSi;    //支文字表示領域BOTTOM
            public int rangeHeight;     //干支文字領域高さ
            public int rangeWidth;      //干支文字領域幅



            //干支文字表示領域
            Rectangle rectNenunTitle;
            Rectangle rectNenunKan;
            Rectangle rectNenunSi;

            Rectangle rectTaiunTitle;
            Rectangle rectTaiunKan;
            Rectangle rectTaiunSi;

            Rectangle rectNikansiKan;
            Rectangle rectNikansiSi;
            Rectangle rectGekkansiKan;
            Rectangle rectGekkansiSi;
            Rectangle rectNenkansiKan;
            Rectangle rectNenkansiSi;

            const int bitFlgNenun = 0x20;
            const int bitFlgTaiun = 0x10;

            const int bitFlgNiti = 0x04;
            const int bitFlgGetu = 0x02;
            const int bitFlgNen = 0x01;

            Kansi taiunKansi=null;
            Kansi nenunKansi = null;

            public KoutenUnDraw(Person person, PictureBox pictureBox, Kansi _taiunKansi, Kansi _nenunKansi) :
                base(person, pictureBox)
            {

                taiunKansi = _taiunKansi;
                nenunKansi = _nenunKansi;

                rangeHeight = GetFontHeight() * 2;
                rangeWidth = 45;

            }
            void CalcCoord()
            { 
                //年運表示開始位置
                nenun.X = 5;
                //nenun.Y = 80;
                //nenun.Y = (int)(GetDrawArea().Height / 2.0 - rangeHeight*1.5); //上表示部分を少し少なめに
                nenun.Y = (idxMtx + 1) * GetLineOffsetY() + 10;

                nenunCenterX = nenun.X + rangeWidth / 2;

                taiun.X = nenun.X + rangeWidth;
                taiun.Y = nenun.Y;
                taiunCenterX = taiun.X + rangeWidth / 2;

                nikkansi.X = taiun.X + rangeWidth +10;
                nikkansi.Y = taiun.Y;
                nikkansiCenterX = nikkansi.X + rangeWidth / 2;

                gekkansi.X = nikkansi.X + rangeWidth;
                gekkansi.Y = nikkansi.Y;
                gekkansiCenterX = gekkansi.X + rangeWidth / 2;

                nenkansi.X = gekkansi.X + rangeWidth;
                nenkansi.Y = gekkansi.Y;
                nenkansiCenterX = nenkansi.X + rangeWidth / 2;

                drawTopKan = nikkansi.Y;
                drawTopSi = drawTopKan + rangeHeight;
                drawBottomSi = drawTopSi + rangeHeight;


                //干支表示領域
                rectNenunKan = new Rectangle(nenun.X, nenun.Y, rangeWidth, rangeHeight);
                rectNenunSi = new Rectangle(nenun.X, drawTopSi, rangeWidth, rangeHeight);
                rectTaiunKan = new Rectangle(taiun.X, taiun.Y, rangeWidth, rangeHeight);
                rectTaiunSi = new Rectangle(taiun.X, drawTopSi, rangeWidth, rangeHeight);
                rectNikansiKan = new Rectangle(nikkansi.X, nikkansi.Y, rangeWidth, rangeHeight);
                rectNikansiSi = new Rectangle(nikkansi.X, drawTopSi, rangeWidth, rangeHeight);
                rectGekkansiKan = new Rectangle(gekkansi.X, gekkansi.Y, rangeWidth, rangeHeight);
                rectGekkansiSi = new Rectangle(gekkansi.X, drawTopSi, rangeWidth, rangeHeight);
                rectNenkansiKan = new Rectangle(nenkansi.X, nenkansi.Y, rangeWidth, rangeHeight);
                rectNenkansiSi = new Rectangle(nenkansi.X, drawTopSi, rangeWidth, rangeHeight);


            }


            public override void DrawItem(Graphics g)
            {
                if (person == null) return;

                int dircUp = -1;
                int dircDown = +1;



                //陰陽(年運→大運）
                //-------------------               
                bool bInyouTNenunTaiun = person.IsInyou(nenunKansi, taiunKansi); //（年運 - 大運) の関係
                int idxInyouTNenunTaiun = SetMatrixUp(bInyouTNenunTaiun, bitFlgNenun, (bitFlgNenun | bitFlgTaiun));//日 - 月


                int bitFlgNitiGetuNen = (bitFlgNiti | bitFlgGetu | bitFlgNen);
                //陰陽(大運→＊）
                //-------------------               
                bool bInyouTaiunNiti  = person.IsInyou(taiunKansi, person.nikkansi); //（大運-日) の関係
                bool bInyouTaiunGetu  = person.IsInyou(taiunKansi, person.gekkansi); //（大運-月) の関係
                bool bInyouTaiunNen   = person.IsInyou(taiunKansi, person.nenkansi);//（大運-年) の関係
                int idxInyouTaiunNiti = SetMatrixUp(bInyouTaiunNiti, bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgNiti));//大運 - 日
                int idxInyouTaiunGetu = SetMatrixUp(bInyouTaiunGetu, bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgGetu));//大運 - 月
                int idxInyouTaiunNen  = SetMatrixUp(bInyouTaiunNen,  bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgNen));//大運 - 年


                //陰陽(年運→＊）
                //-------------------               
                bool bInyouNenunNiti = person.IsInyou(nenunKansi, person.nikkansi); //（年運-日) の関係
                bool bInyouNenunGetu  = person.IsInyou(nenunKansi, person.gekkansi);//（年運-月) の関係
                bool bInyouNenunNen   = person.IsInyou(nenunKansi, person.nenkansi);//（年運-年) の関係
                int idxInyouNenunNiti = SetMatrixUp(bInyouNenunNiti, bitFlgNenun | bitFlgNitiGetuNen, (bitFlgNenun | bitFlgNiti));//年運 - 日
                int idxInyouNenunGetu = SetMatrixUp(bInyouNenunGetu, bitFlgNenun | bitFlgNitiGetuNen, (bitFlgNenun | bitFlgGetu));//年運 - 月
                int idxInyouNenunNen  = SetMatrixUp(bInyouNenunNen,  bitFlgNenun | bitFlgNitiGetuNen, (bitFlgNenun | bitFlgNen));//年運 - 年


                //干合(年運→大運）
                //-------------------               
                bool bKangouTNenunTaiun = person.IsKango(nenunKansi, taiunKansi); //（年運 - 大運) の関係
                int idxKangouTNenunTaiun = SetMatrixUp(bKangouTNenunTaiun, bitFlgNenun, (bitFlgNenun | bitFlgTaiun));//年運 - 大運


                //干合(大運→＊）
                //-------------------
                bool bKangouTaiunNiti  = person.IsKango(taiunKansi, person.nikkansi);//（大運-日) の関係
                bool bKangouTaiunGetu  = person.IsKango(taiunKansi, person.gekkansi);//（大運-月) の関係
                bool bKangouTaiunNen   = person.IsKango(taiunKansi, person.nenkansi);//（大運-年) の関係
                int idxKangouTaiunNiti = SetMatrixUp(bKangouTaiunNiti, bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgNiti));//大運 - 日
                int idxKangouTaiunGetu = SetMatrixUp(bKangouTaiunGetu, bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgGetu));//大運 - 月
                int idxKangouTaiunNen  = SetMatrixUp(bKangouTaiunNen,  bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgNen));//大運 - 年


                //干合(年運→＊）
                //-------------------
                bool bKangouNenunNiti  = person.IsKango(nenunKansi, person.nikkansi);//（年運-日) の関係
                bool bKangouNenunGetu  = person.IsKango(nenunKansi, person.gekkansi);//（年運-月) の関係
                bool bKangouNenunNen   = person.IsKango(nenunKansi, person.nenkansi);//（年運-年) の関係
                int idxKangouNenunNiti = SetMatrixUp(bKangouNenunNiti, bitFlgNenun | bitFlgNitiGetuNen, (bitFlgNenun | bitFlgNiti));//年運 - 日
                int idxKangouNenunGetu = SetMatrixUp(bKangouNenunGetu, bitFlgNenun | bitFlgNitiGetuNen, (bitFlgNenun | bitFlgGetu));//年運 - 月
                int idxKangouNenunNen  = SetMatrixUp(bKangouNenunNen,  bitFlgNenun | bitFlgNitiGetuNen, (bitFlgNenun | bitFlgNen));//年運 - 年

                //七殺(年運→大運）
                //-------------------               
                bool bNanasatuTNenunTaiun = person.IsNanasatu(nenunKansi, taiunKansi); //（年運 - 大運) の関係
                int idxNanasatuTNenunTaiun = SetMatrixUp(bNanasatuTNenunTaiun, bitFlgNenun, (bitFlgNenun | bitFlgTaiun));//年運 - 大運

                //七殺(大運→＊）
                //-------------------
                bool bNanasatuTaiunNiti  = person.IsNanasatu(taiunKansi, person.nikkansi);//（大運-日) の関係
                bool bNanasatuTaiunGetu  = person.IsNanasatu(taiunKansi, person.gekkansi);//（大運-月) の関係
                bool bNanasatuTaiunNen   = person.IsNanasatu(taiunKansi, person.nenkansi);//（大運-年) の関係
                int idxNanasatuTaiunNiti = SetMatrixUp(bNanasatuTaiunNiti, bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgNiti));//大運 - 日
                int idxNanasatuTaiunGetu = SetMatrixUp(bNanasatuTaiunGetu, bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgGetu));//大運 - 月
                int idxNanasatuTaiunNen  = SetMatrixUp(bNanasatuTaiunNen,  bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgNen));//大運 - 年

                //七殺(年運→＊）
                //-------------------
                bool bNanasatuNenunNiti  = person.IsNanasatu(nenunKansi, person.nikkansi);//（年運-日) の関係
                bool bNanasatuNenunGetu  = person.IsNanasatu(nenunKansi, person.gekkansi);//（年運-月) の関係
                bool bNanasatuNenunNen   = person.IsNanasatu(nenunKansi, person.nenkansi);//（年運-年) の関係
                int idxNanasatuNenunNiti = SetMatrixUp(bNanasatuNenunNiti, bitFlgNenun | bitFlgNitiGetuNen, (bitFlgNenun | bitFlgNiti));//年運 - 日
                int idxNanasatuNenunGetu = SetMatrixUp(bNanasatuNenunGetu, bitFlgNenun | bitFlgNitiGetuNen, (bitFlgNenun | bitFlgGetu));//年運 - 月
                int idxNanasatuNenunNen  = SetMatrixUp(bNanasatuNenunNen,  bitFlgNenun | bitFlgNitiGetuNen, (bitFlgNenun | bitFlgNen));//年運 - 年


                //干支の上部に表示する情報の段数から干支表示基準座標を計算
                CalcCoord();

                //干支表示
                DrawKansi(nenunKansi, rectNenunKan, rectNenunSi);
                DrawKansi(taiunKansi, rectTaiunKan, rectTaiunSi);

                DrawKansi(person.nikkansi, rectNikansiKan, rectNikansiSi);
                DrawKansi(person.gekkansi, rectGekkansiKan, rectGekkansiSi);
                DrawKansi(person.nenkansi, rectNenkansiKan, rectNenkansiSi);

                rectNenunTitle = new Rectangle(nenun.X, nenun.Y - GetSmallFontHeight()/2, rangeWidth, GetSmallFontHeight());
                rectTaiunTitle = new Rectangle(taiun.X, taiun.Y - GetSmallFontHeight()/2, rangeWidth, GetSmallFontHeight());
                DrawString(rectNenunTitle, "<年運>");
                DrawString(rectTaiunTitle, "<大運>");

                //陰陽(年運→大運）
                //-------------------               
                if (bInyouTNenunTaiun)//大運 - 年運
                {
                    DrawLine(idxInyouTNenunTaiun, nenunCenterX, taiunCenterX, drawTopKan, dircUp);
                    DrawString(idxInyouTNenunTaiun, nenunCenterX, taiunCenterX, drawTopKan, dircUp, strInyou);
                }


                //陰陽(大運→＊）
                //-------------------               
                if (bInyouTaiunNiti)//大運 - 日
                {
                    DrawLine(idxInyouTaiunNiti, taiunCenterX, nikkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxInyouTaiunNiti, taiunCenterX, nikkansiCenterX, drawTopKan, dircUp, strInyou);
                }
                if (bInyouTaiunGetu)//大運 - 月
                {
                    DrawLine(idxInyouTaiunGetu, taiunCenterX, gekkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxInyouTaiunGetu, taiunCenterX, gekkansiCenterX, drawTopKan, dircUp, strInyou);
                }
                if (bInyouTaiunNen)//大運 - 年
                {
                    DrawLine(idxInyouTaiunNen, taiunCenterX, nenkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxInyouTaiunNen, taiunCenterX, nenkansiCenterX, drawTopKan, dircUp, strInyou);
                }

                //陰陽(年運→＊）
                //-------------------               
                if (bInyouNenunNiti)//年運 - 日
                {
                    DrawLine(idxInyouNenunNiti, nenunCenterX, nikkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxInyouNenunNiti, nenunCenterX, nikkansiCenterX, drawTopKan, dircUp, strInyou);
                }
                if (bInyouNenunGetu)//年運 - 月
                {
                    DrawLine(idxInyouNenunGetu, nenunCenterX, gekkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxInyouNenunGetu, nenunCenterX, gekkansiCenterX, drawTopKan, dircUp, strInyou);
                }
                if (bInyouNenunNen)//年運 - 年
                {
                    DrawLine(idxInyouNenunNen, nenunCenterX, nenkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxInyouNenunNen, nenunCenterX, nenkansiCenterX, drawTopKan, dircUp, strInyou);
                }


                //干合(年運→大運）
                //-------------------               
                if (bKangouTNenunTaiun)//大運 - 年運
                {
                    DrawLine(idxKangouTNenunTaiun, nenunCenterX, taiunCenterX, drawTopKan, dircUp);
                    DrawString(idxKangouTNenunTaiun, nenunCenterX, taiunCenterX, drawTopKan, dircUp, strKangou);
                }
                //干合(大運→＊）
                //-------------------
                if (bKangouTaiunNiti)//大運 - 日
                {
                    DrawLine(idxKangouTaiunNiti, taiunCenterX, nikkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxKangouTaiunNiti, taiunCenterX, nikkansiCenterX, drawTopKan, dircUp, strKangou);
                }
                if (bKangouTaiunGetu)//大運 - 月
                {
                    DrawLine(idxKangouTaiunGetu, taiunCenterX, gekkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxKangouTaiunGetu, taiunCenterX, gekkansiCenterX, drawTopKan, dircUp, strKangou);
                }
                if (bKangouTaiunNen)//大運 - 年
                {
                    DrawLine(idxKangouTaiunNen, taiunCenterX, nenkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxKangouTaiunNen, taiunCenterX, nenkansiCenterX, drawTopKan, dircUp, strKangou);
                }

                //干合(年運→＊）
                //-------------------
                if (bKangouNenunNiti)//年運 - 日
                {
                    DrawLine(idxKangouNenunNiti, nenunCenterX, nikkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxKangouNenunNiti, nenunCenterX, nikkansiCenterX, drawTopKan, dircUp, strKangou);
                }
                if (bKangouNenunGetu)//年運 - 月
                {
                    DrawLine(idxKangouNenunGetu, nenunCenterX, gekkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxKangouNenunGetu, nenunCenterX, gekkansiCenterX, drawTopKan, dircUp, strKangou);
                }
                if (bKangouNenunNen)//年運 - 年
                {
                    DrawLine(idxKangouNenunNen, nenunCenterX, nenkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxKangouNenunNen, nenunCenterX, nenkansiCenterX, drawTopKan, dircUp, strKangou);
                }

                //七殺(年運→大運）
                //-------------------               
                if (bNanasatuTNenunTaiun)//大運 - 年運
                { 
                    DrawLine(idxNanasatuTNenunTaiun, nenunCenterX, taiunCenterX, drawTopKan, dircUp);
                    DrawString(idxNanasatuTNenunTaiun, nenunCenterX, taiunCenterX, drawTopKan, dircUp, strNanasatu);
                }
                //七殺(大運→＊）
                //-------------------
                if (bNanasatuTaiunNiti)//大運 - 日
                {
                    DrawLine(idxNanasatuTaiunNiti, taiunCenterX, nikkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxNanasatuTaiunNiti, taiunCenterX, nikkansiCenterX, drawTopKan, dircUp, strNanasatu);
                }
                if (bNanasatuTaiunGetu)//大運 - 月
                {
                    DrawLine(idxNanasatuTaiunGetu, taiunCenterX, gekkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxNanasatuTaiunGetu, taiunCenterX, gekkansiCenterX, drawTopKan, dircUp, strNanasatu);
                }
                if (bNanasatuTaiunNen)//大運 - 年
                {
                    DrawLine(idxNanasatuTaiunNen, taiunCenterX, nenkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxNanasatuTaiunNen, taiunCenterX, nenkansiCenterX, drawTopKan, dircUp, strNanasatu);
                }
                //七殺(年運→＊）
                //-------------------
                if (bNanasatuNenunNiti)//年運 - 日
                {
                    DrawLine(idxNanasatuNenunNiti, nenunCenterX, nikkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxNanasatuNenunNiti, nenunCenterX, nikkansiCenterX, drawTopKan, dircUp, strNanasatu);
                }
                if (bNanasatuNenunGetu)//年運 - 月
                {
                    DrawLine(idxNanasatuNenunGetu, nenunCenterX, gekkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxNanasatuNenunGetu, nenunCenterX, gekkansiCenterX, drawTopKan, dircUp, strNanasatu);
                }
                if (bNanasatuNenunNen)//年運 - 年
                {
                    DrawLine(idxNanasatuNenunNen, nenunCenterX, nenkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxNanasatuNenunNen, nenunCenterX, nenkansiCenterX, drawTopKan, dircUp, strNanasatu);

                }


                //合法・散法
                //-------------------
                string[] gouhouSanpouTaiunNenun = person.GetGouhouSanpou(taiunKansi, nenunKansi, false, false);

                if (gouhouSanpouTaiunNenun != null)//大運 - 年運
                {
                    int enableFlag = CheckExceptionValue(person, taiunKansi, nenunKansi, taiunKansi, nenunKansi, gouhouSanpouTaiunNenun);

                    int idx = SetMatrixDown(true, bitFlgNenun, (bitFlgNenun | bitFlgTaiun));//年運 - 大運
                    DrawLine(idx, nenunCenterX, taiunCenterX, drawBottomSi, dircDown);
                    DrawString(idx, nenunCenterX, taiunCenterX, drawBottomSi, dircDown, gouhouSanpouTaiunNenun);

                }

                string[] gouhouSanpouTaiunNiti = person.GetGouhouSanpou(taiunKansi, person.nikkansi, false, false);
                string[] gouhouSanpouTaiunGetu = person.GetGouhouSanpou(taiunKansi, person.gekkansi, false, false);
                string[] gouhouSanpouTaiunNen = person.GetGouhouSanpou(taiunKansi, person.nenkansi, false, false);

                if (gouhouSanpouTaiunNiti != null)//大運 - 日
                {
                    int enableFlag = CheckExceptionValue( person, taiunKansi, person.nikkansi, taiunKansi, nenunKansi, gouhouSanpouTaiunNiti);

                    int idx = SetMatrixDown(true, bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgNiti));
                    DrawLine(idx, taiunCenterX, nikkansiCenterX, drawBottomSi, dircDown);
                    DrawString(idx, taiunCenterX, nikkansiCenterX, drawBottomSi, dircDown, gouhouSanpouTaiunNiti, enableFlag);

                }
                if (gouhouSanpouTaiunGetu != null)//大運 - 月
                {
                    int enableFlag = CheckExceptionValue(person, taiunKansi, person.gekkansi, taiunKansi, nenunKansi, gouhouSanpouTaiunGetu);
                    int idx = SetMatrixDown(true, bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgGetu));
                    DrawLine(idx, taiunCenterX, gekkansiCenterX, drawBottomSi, dircDown);
                    DrawString(idx, taiunCenterX, gekkansiCenterX, drawBottomSi, dircDown, gouhouSanpouTaiunGetu, enableFlag);
                }

                if (gouhouSanpouTaiunNen != null)//大運 - 年
                {
                    int enableFlag = CheckExceptionValue(person, taiunKansi, person.nenkansi, taiunKansi, nenunKansi, gouhouSanpouTaiunNen);
                    int idx = SetMatrixDown(true, bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgNen));
                    DrawLine(idx, taiunCenterX, nenkansiCenterX, drawBottomSi, dircDown);
                    DrawString(idx, taiunCenterX, nenkansiCenterX, drawBottomSi, dircDown, gouhouSanpouTaiunNen, enableFlag);
                }

                string[] gouhouSanpouNenunNiti = person.GetGouhouSanpou(nenunKansi, person.nikkansi, false, false);
                string[] gouhouSanpouNenunGetu = person.GetGouhouSanpou(nenunKansi, person.gekkansi, false, false);
                string[] gouhouSanpouNenunNen = person.GetGouhouSanpou(nenunKansi, person.nenkansi, false, false);

                if (gouhouSanpouNenunNiti != null)//年運 - 日
                {
                    int enableFlag = CheckExceptionValue(person, nenunKansi, person.nikkansi, taiunKansi, nenunKansi, gouhouSanpouNenunNiti);
                    int idx = SetMatrixDown(true, bitFlgNitiGetuNen, (bitFlgNenun | bitFlgNiti));
                    DrawLine(idx, nenunCenterX, nikkansiCenterX, drawBottomSi, dircDown);
                    DrawString(idx, nenunCenterX, nikkansiCenterX, drawBottomSi, dircDown, gouhouSanpouNenunNiti, enableFlag);
                }
                if (gouhouSanpouNenunGetu != null)//年運 - 月
                {
                    int enableFlag = CheckExceptionValue(person, nenunKansi, person.gekkansi, taiunKansi, nenunKansi, gouhouSanpouNenunGetu);
                    int idx = SetMatrixDown(true, bitFlgNitiGetuNen, (bitFlgNenun | bitFlgGetu));
                    DrawLine(idx, nenunCenterX, gekkansiCenterX, drawBottomSi, dircDown);
                    DrawString(idx, nenunCenterX, gekkansiCenterX, drawBottomSi, dircDown, gouhouSanpouNenunGetu, enableFlag);
                }

                if (gouhouSanpouNenunNen != null)//年運 - 年
                {
                    int enableFlag = CheckExceptionValue(person, nenunKansi, person.nenkansi, taiunKansi, nenunKansi, gouhouSanpouNenunNen);
                    int idx = SetMatrixDown(true, bitFlgNitiGetuNen, (bitFlgNenun | bitFlgNen));
                    DrawLine(idx, nenunCenterX, nenkansiCenterX, drawBottomSi, dircDown);
                    DrawString(idx, nenunCenterX, nenkansiCenterX, drawBottomSi, dircDown, gouhouSanpouNenunNen, enableFlag);
                }

            }
            int CheckExceptionValue(Person person, Kansi kansi1, Kansi kansi2, Kansi taiun, Kansi nenun, string[] values)
            {
                int retValue = 0xFFFF;
                if (kansi1.si == "亥" && kansi2.si == "寅" ||
                    kansi1.si == "寅" && kansi2.si == "亥")
                {
                    retValue = 0;
                    bool bHit = false;
                    if (taiun.si == "申" || taiun.si == "巳") bHit = true;
                    else if (nenun.si == "申" || nenun.si == "巳") bHit = true;
                    else if (person.IsExistStrInKansiSi(new string[] { "申", "巳" })) bHit = true;

                    if (bHit)
                    {
                        int bit = 0x1;
                        foreach (var v in values)
                        {
                            if (v == "破") retValue |= bit;
                            bit <<= 1;
                        }

                    }
                    else
                    {
                        int bit = 0x1;
                        foreach (var v in values)
                        {
                            if (v == "支合") retValue |= bit;
                            bit <<= 1;
                        }
                    }
                }
                else if (kansi1.si == "申" && kansi2.si == "巳" ||
                         kansi1.si == "巳" && kansi2.si == "申")
                {
                    retValue = 0;
                    bool bHit = false;
                    if (taiun.si == "亥" || taiun.si == "寅") bHit = true;
                    else if (taiun.si == "亥" || taiun.si == "寅") bHit = true;
                    else if (person.IsExistStrInKansiSi(new string[] { "亥", "寅" })) bHit = true;

                    if (bHit)
                    {
                        int bit = 0x1;
                        foreach (var v in values)
                        {
                            if (v == "破") retValue |= bit;
                            if (v == "生貴刑") retValue |= bit;
                            bit <<= 1;
                        }
                    }
                    else
                    {
                        int bit = 0x1;
                        foreach (var v in values)
                        {
                            if (v == "支合") retValue |= bit;
                            bit <<= 1;
                        }
                    }
                }
                return retValue;

            }
        }

       
        /// <summary>
        /// 宿命図表示
        /// </summary>
        /// <param name="person"></param>
        private void DispShukumei(Person person)
        {

            drawItem = new ShukumeiDraw(person, pictureBox1);
            drawItem.Draw();

        }
        /// <summary>
        /// 後天運図表示
        /// </summary>
        /// <param name="person"></param>
        private void DispKoutenUn(Person person)
        {

            //大運の選択行の干支取得
            var selectedItem = lvTaiun.SelectedItems;
            if (selectedItem.Count == 0) return;

            TaiunLvItemData itemData = (TaiunLvItemData)selectedItem[0].Tag;

            //年運の選択行の干支取得
            selectedItem = lvNenun.SelectedItems;
            if (selectedItem.Count == 0) return;

            NenunLvItemData itemData2 = (NenunLvItemData)selectedItem[0].Tag;


            if (drawItem2 != null) drawItem2.Dispose();
            drawItem2 = new KoutenUnDraw(person, pictureBox2, itemData.kansi, itemData2.kansi);
            drawItem2.Draw();


        }


        //====================================================
        // イベントハンドラ
        //====================================================
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
            DispKoutenUn(curPerson);
        }
        /// <summary>
        /// 年運リストビュー選択イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvNenun_SelectedIndexChanged(object sender, EventArgs e)
        {
            //後天運 図の表示更新
            DispKoutenUn(curPerson);
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

            if (person.gender == Gender.NAN) radMan.Checked = true;
            else radWoman.Checked = true;

            MainProc(person);


        }
        /// <summary>
        /// 後天運リストビューでの上下キー押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvNenun_KeyDown(object sender, KeyEventArgs e)
        {
            var idxNenun = lvNenun.SelectedItems[0].Index;
            switch (e.KeyData)
            {
                case Keys.Down:
                    if (idxNenun >= lvNenun.Items.Count - 1)
                    {
                        //次の旬に移動
                        var idxTaiun = lvTaiun.SelectedItems[0].Index;
                        if (idxTaiun >= lvTaiun.Items.Count - 1) return;

                        lvTaiun.Items[idxTaiun + 1].Selected = true;
                        lvNenun.Items[0].Selected = true;
                        lvNenun.Items[0].Focused = true;
                        e.Handled = true;
                    }
                    break;
                case Keys.Up:
                    if (idxNenun <= 0)
                    {
                        var idxTaiun = lvTaiun.SelectedItems[0].Index;
                        if (idxTaiun <= 0) return;
                        lvTaiun.Items[idxTaiun - 1].Selected = true;
                        lvNenun.Items[lvNenun.Items.Count - 1].Selected = true;
                        lvNenun.Items[lvNenun.Items.Count - 1].Focused = true;
                        e.Handled = true;
                    }
                    break;
            }
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
                if (idxTaiun >= lvTaiun.Items.Count-1) return;
                lvTaiun.Items[idxTaiun + 1].Selected = true;
            }
        }
        /// <summary>
        /// 後天運リストビューのホイール操作イベント
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
                }
                else
                {
                    var idxTaiun = lvTaiun.SelectedItems[0].Index;
                    if (idxTaiun <= 0) return;
                    lvTaiun.Items[idxTaiun - 1].Selected = true;
                    lvNenun.Items[lvNenun.Items.Count - 1].Selected = true;
                    lvNenun.Items[lvNenun.Items.Count - 1].Focused = true;
                }


            }
            else
            {
                //↓
                if(idxNenun < lvNenun.Items.Count - 1)
                {
                    lvNenun.Items[idxNenun + 1].Selected = true;
                }
                else
                {
                    //次の旬に移動
                    var idxTaiun = lvTaiun.SelectedItems[0].Index;
                    if (idxTaiun >= lvTaiun.Items.Count - 1) return;

                    lvTaiun.Items[idxTaiun + 1].Selected = true;
                    lvNenun.Items[0].Selected = true;
                    lvNenun.Items[0].Focused = true;
                }

            }
        }

        /// <summary>
        /// 後天運ピクチャーボックスサイズ変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_SizeChanged(object sender, EventArgs e)
        {
            DispKoutenUn(curPerson);

        }
    }
}
