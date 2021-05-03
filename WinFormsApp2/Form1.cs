﻿using System;
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


        public Form1()
        {
            InitializeComponent();

            exePath = Path.GetDirectoryName(Application.ExecutablePath);

            setuiribiTbl = new SetuiribiTable();
            setuiribiTbl.ReadTable(exePath + @"\節入り日.xls");

            personList = new Persons();
            personList.ReadPersonList(exePath + @"\名簿.xls");

 

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

            //節理日から７日を超える日数の日干支を太字にする
            if (person.dayNumFromSetuiribi > 7)
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
                else Common.SetBold(label, false);

                label = lstLblGekkansiZougan[(int)Value];
                label.Text = gensoGekkansi.genso[(int)Value].name;
                if (idxGekkansiGensoType == (int)Value) Common.SetBold(label, true);
                else Common.SetBold(label, false);

                label = lstLblNenkansiZougan[(int)Value];
                label.Text = gensoNenkansi.genso[(int)Value].name;
                if (idxNenkaisiGensoType == (int)Value) Common.SetBold(label, true);
                else Common.SetBold(label, false);
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
            string gouhou = person.GetGouhouSanpouString(taiunKansi, person.nikkansi); //合法・散法            
            string tensatu = person.GetTensatuString(taiunKansi, person.nikkansi);//天殺
            string tichu = person.GetTichuString(taiunKansi, person.nikkansi);//地冲
            string value = tensatu + tichu+ " " +gouhou;
            lvItem.SubItems.Add(value.Trim());

            //月
            gouhou = person.GetGouhouSanpouString(taiunKansi.si, person.gekkansi.si); //合法・散法
            tensatu = person.GetTensatuString(taiunKansi, person.gekkansi);//天殺
            tichu = person.GetTichuString(taiunKansi, person.gekkansi);//地冲
            value = tensatu + tichu + " " + gouhou; ;
            lvItem.SubItems.Add(value.Trim());

            //年
            gouhou = person.GetGouhouSanpouString(taiunKansi.si, person.nenkansi.si);//合法・散法
            tensatu = person.GetTensatuString(taiunKansi, person.nenkansi);//天殺
            tichu = person.GetTichuString(taiunKansi, person.nenkansi);//地冲
            value = tensatu + tichu + " " + gouhou; ;
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
            string gouhou = person.GetGouhouSanpouString(nenunKansi.si, person.nikkansi.si);//合法・散法
            string value = nentin + " " + rittin +" "+ gouhou;
            lvItem.SubItems.Add(value.Trim());

            //月
            nentin = person.GetNentin(nenunKansi, person.gekkansi); //納音、準納音
            rittin = person.GetNittin(nenunKansi, person.gekkansi); //律音、準律音
            gouhou = person.GetGouhouSanpouString(nenunKansi.si, person.gekkansi.si);//合法・散法
            value = nentin + " " + rittin + " " + gouhou;
            lvItem.SubItems.Add(value);

            //年
            nentin = person.GetNentin(nenunKansi, person.nenkansi); //納音、準納音
            rittin = person.GetNittin(nenunKansi, person.nenkansi); //律音、準律音
            gouhou = person.GetGouhouSanpouString(nenunKansi.si, person.nenkansi.si);//合法・散法
            value = nentin + " " + rittin + " " + gouhou;
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

        private void txtNikkansiSanshutuSu_TextChanged(object sender, EventArgs e)
        {
            if (txtNikkansiSanshutuSu.Text == "") return;
            if (txtBaseDay.Text == "") return;

            int baseDay = int.Parse(txtBaseDay.Text);
            int no = int.Parse(txtNikkansiSanshutuSu.Text)+ baseDay;

            txtBaseNikkansiNo.Text = no.ToString();


        }
        /// <summary>
        /// 大運リストビュー選択イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvTaiun_SelectedIndexChanged(object sender, EventArgs e)
        {
            Person person = (Person)cmbPerson.SelectedItem;
            int Year = int.Parse(txtYear.Text);

            var selectedItem = lvTaiun.SelectedItems;
            if (selectedItem.Count == 0) return;

            TaiunLvItemData itemData = (TaiunLvItemData)selectedItem[0].Tag;

            //年運リスト表示更新
            DispNenun(person, itemData.startNen);

            //後天運 図の表示更新
            DispKoutenUn(person);
        }
        /// <summary>
        /// 年運リストビュー選択イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvNenun_SelectedIndexChanged(object sender, EventArgs e)
        {
            Person person = (Person)cmbPerson.SelectedItem;
            //後天運 図の表示更新
            DispKoutenUn(person);
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
        class IsouhouBase : IDisposable
        {
            public Person person;
            Font fnt = null;
            Font fntSmall = null;
            Pen blackPen = null;
            StringFormat stringFormat = null;
            StringFormat smallStringFormat = null;
            Graphics g;
            int offsetY = 20;

            public IsouhouBase(Person _person, Graphics _g)
            {
                person = _person;
                g = _g;

                blackPen = new Pen(Color.Black, 1); ;

                fnt = new Font("MS Gothic", 14, FontStyle.Regular);
                fntSmall = new Font("MS Gothic", 8, FontStyle.Regular);

                //干支文字センタリング表示用フォーマット
                stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                smallStringFormat = new StringFormat();
                smallStringFormat.Alignment = StringAlignment.Center;
                smallStringFormat.LineAlignment = StringAlignment.Center;

            }

            public int GetFontHeight()
            {
                return fnt.Height;
            }
            public int GetSmallFontHeight()
            {
                return fntSmall.Height;
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
                g.DrawString(s, fntSmall, Brushes.Black, rect, smallStringFormat);
            }
            protected void DrawString( int mtxIndex, int from, int to, int baseY, int dirc, string[] strs)
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


                foreach (var s in strs)
                {

                    Rectangle rect = new Rectangle(x, y, (int)Math.Ceiling(maxWidth), fntSmall.Height);
                    g.FillRectangle(Brushes.WhiteSmoke, rect);

                    g.DrawString(s, fntSmall, Brushes.Black, rect, smallStringFormat);
                    y += fntSmall.Height;
                }

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

            // 4:[ ][ ][ ] 
            // 3:[ ][ ][ ]  (設定例）
            // 2:[ ][ ][ ]  日年[1][0][1]  (bitFlgNiti | bitFlgNen) 
            // 1:[ ][ ][ ]  月年[0][1][1]  (bitFlgGetu | bitFlgNen) 
            // 0:[ ][ ][ ]  日月[1][1][0]  (bitFlgNiti | bitFlgGetu)
            int[] matrix = null;
            int[] matrixBottom = null;

            public ShukumeiDraw(Person person, Graphics _g) :
                base(person, _g)
            {
                matrix = new int[5];
                matrixBottom = new int[5];



                rangeHeight = GetFontHeight() * 2;
                rangeWidth = 45;


                nikkansi.X = 5;
                nikkansi.Y = 50;
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


            public void Draw()
            {
                if (person == null) return;

                //干支表示
                DrawKansi(person.nikkansi, rectNikansiKan, rectNikansiSi);
                DrawKansi(person.gekkansi, rectGekkansiKan, rectGekkansiSi);
                DrawKansi(person.nenkansi, rectNenkansiKan, rectNenkansiSi);

                //陰陽
                //-------------------
                //干（日-月) の関係
                bool bInyouNitiGetsuKan = person.IsInyouNitiGetsuKan();
                //干（月-年) の関係
                bool bInyouGetsuNenKan = person.IsInyouGetsuNenKan();
                //干（日-年) の関係
                bool bInyouNiItiNenKan = person.IsInyouNitiNenKan();

                int idxMtx = 0;
                int idxMtxButtom = 0;
                int dircUp = -1;
                int dircDown = +1;
                if (bInyouNitiGetsuKan)
                {
                    DrawLine(idxMtx, nikkansiCenterX, gekkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxMtx, nikkansiCenterX, gekkansiCenterX, drawTopKan, dircUp, new string[] { "陰陽" });
                    matrix[idxMtx] |= bitFlgNiti | bitFlgGetu;

                }
                else if (bInyouGetsuNenKan)
                {
                    DrawLine(idxMtx, gekkansiCenterX, nenkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxMtx, gekkansiCenterX, nenkansiCenterX, drawTopKan, dircUp, new string[] { "陰陽"});
                    matrix[idxMtx] |= bitFlgGetu | bitFlgNen;

                }
                else if (bInyouNiItiNenKan)
                {
                    DrawLine(idxMtx, nikkansiCenterX, nenkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxMtx, nikkansiCenterX, nenkansiCenterX, drawTopKan, dircUp, new string[] { "陰陽" });
                    matrix[idxMtx] |= bitFlgNiti | bitFlgNen;
                }
                //干合
                //-------------------
                //干合（日-月) の関係
                bool bKangouNitiGetsuKan = person.IsKangouNitiGetsuKan();
                //干合（月-年) の関係
                bool bKangouGetsuNenKan = person.IsKangoGetsuNenKan();
                //干合（日-年) の関係
                bool bKangouNiItiNenKan = person.IsKangoNitiNenKan();

                if (bKangouNitiGetsuKan)
                {
                    if ((matrix[idxMtx] & bitFlgNiti) != 0) idxMtx++;
                    DrawLine(idxMtx, nikkansiCenterX, gekkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxMtx, nikkansiCenterX, gekkansiCenterX, drawTopKan, dircUp, new string[] { "干合" });
                    matrix[idxMtx] |= bitFlgNiti | bitFlgGetu;

                }
                else if (bKangouGetsuNenKan)
                {
                    if ((matrix[idxMtx] & bitFlgNen) != 0) idxMtx++;
                    DrawLine(idxMtx, gekkansiCenterX, nenkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxMtx, gekkansiCenterX, nenkansiCenterX, drawTopKan, dircUp, new string[] { "干合" });
                    matrix[idxMtx] |= bitFlgGetu | bitFlgNen;

                }
                else if (bKangouNiItiNenKan)
                {
                    if ((matrix[idxMtx] & (bitFlgNiti | bitFlgNen)) != 0) idxMtx++;
                    DrawLine(idxMtx, nikkansiCenterX, nenkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxMtx, nikkansiCenterX, nenkansiCenterX, drawTopKan, dircUp, new string[] { "干合" });
                    matrix[idxMtx] |= bitFlgNiti | bitFlgNen;
                }
                //七殺
                //-------------------
                bool bNanasatuNitGetu = person.IsNanasatuNitiGetuKan();
                bool bNanasatuGetuNen = person.IsNanasatuGetuNenKan();
                bool bNanasatuNitNen = person.IsNanasatuNitiNenKan();
                if (bNanasatuNitGetu)
                {
                    if ((matrix[idxMtx] & bitFlgNiti) != 0) idxMtx++;
                    DrawLine(idxMtx, nikkansiCenterX, gekkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxMtx, nikkansiCenterX, gekkansiCenterX, drawTopKan, dircUp, new string[] { "七殺" });
                    matrix[idxMtx] |= bitFlgNiti | bitFlgGetu;

                }
                if (bNanasatuGetuNen)
                {
                    if ((matrix[idxMtx] & bitFlgNen) != 0) idxMtx++;
                    DrawLine(idxMtx, gekkansiCenterX, nenkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxMtx, gekkansiCenterX, nenkansiCenterX, drawTopKan, dircUp, new string[] { "七殺" });
                    matrix[idxMtx] |= bitFlgNiti | bitFlgGetu;

                }
                if (bNanasatuNitNen)
                {
                    if ((matrix[idxMtx]) != 0) idxMtx++;
                    DrawLine(idxMtx, nikkansiCenterX, nenkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxMtx, nikkansiCenterX, nenkansiCenterX, drawTopKan, dircUp, new string[] { "七殺" });
                    matrix[idxMtx] |= bitFlgNiti | bitFlgNen;

                }

                //合法・散法
                //-------------------
                string[] gouhouSanpouNitiGetu = person.GetGouhouSanpouNitiGetu();
                string[] gouhouSanpouGetuNen = person.GetGouhouSanpouiGetuNen();
                string[] gouhouSanpouNitiNen = person.GetGouhouSanpouiNitiNen();

                if (gouhouSanpouNitiGetu != null)
                {
                    if ((matrixBottom[idxMtxButtom] & bitFlgNiti) != 0) idxMtxButtom++;
                    DrawLine(idxMtxButtom, nikkansiCenterX, gekkansiCenterX, drawBottomSi, dircDown);
                    DrawString(idxMtxButtom, nikkansiCenterX, gekkansiCenterX, drawBottomSi, dircDown, gouhouSanpouNitiGetu);
                    matrixBottom[idxMtxButtom] |= bitFlgNiti | bitFlgGetu;

                }
                if (gouhouSanpouGetuNen != null)
                {
                    if ((matrixBottom[idxMtxButtom] & bitFlgNen) != 0) idxMtx++;
                    DrawLine(idxMtxButtom, gekkansiCenterX, nenkansiCenterX, drawBottomSi, dircDown);
                    DrawString(idxMtxButtom, gekkansiCenterX, nenkansiCenterX, drawBottomSi, dircDown, gouhouSanpouGetuNen);
                    matrixBottom[idxMtxButtom] |= bitFlgGetu | bitFlgNen;

                }

                if (gouhouSanpouNitiNen != null)
                {
                    if ((matrixBottom[idxMtxButtom] & (bitFlgNiti | bitFlgNen)) != 0) idxMtxButtom++;
                    DrawLine(idxMtxButtom, nikkansiCenterX, nenkansiCenterX, drawBottomSi, dircDown);
                    DrawString(idxMtxButtom, nikkansiCenterX, nenkansiCenterX, drawBottomSi, dircDown, gouhouSanpouNitiNen);
                    matrixBottom[idxMtxButtom] |= bitFlgNiti | bitFlgNen;

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


            const int bitFlgNiti = 0x04;
            const int bitFlgGetu = 0x02;
            const int bitFlgNen = 0x01;


            public KoutenUnDraw(Person person, Graphics _g) :
                base(person, _g)
            {

                rangeHeight = GetFontHeight() * 2;
                rangeWidth = 45;


                //年運表示開始位置
                nenun.X = 5;
                nenun.Y = 20;
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
                rectNenunTitle = new Rectangle(nenun.X, nenun.Y - GetSmallFontHeight(), rangeWidth, GetSmallFontHeight());
                rectTaiunTitle = new Rectangle(taiun.X, taiun.Y - GetSmallFontHeight(), rangeWidth, GetSmallFontHeight());
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


            public void Draw(Kansi taiunKansi, Kansi nenunKansi)
            {
                if (person == null) return;

                int idxMtxButtom = 0;
                int dircDown = +1;

                //干支表示
                DrawString(rectNenunTitle, "年運");
                DrawString(rectTaiunTitle, "大運");
                DrawKansi(nenunKansi, rectNenunKan, rectNenunSi);
                DrawKansi(taiunKansi, rectTaiunKan, rectTaiunSi);

                DrawKansi(person.nikkansi, rectNikansiKan, rectNikansiSi);
                DrawKansi(person.gekkansi, rectGekkansiKan, rectGekkansiSi);
                DrawKansi(person.nenkansi, rectNenkansiKan, rectNenkansiSi);


                //合法・散法
                //-------------------
                string[] gouhouSanpouTaiunNiti = person.GetGouhouSanpou(taiunKansi, person.nikkansi);
                string[] gouhouSanpouTaiunGetu = person.GetGouhouSanpou(taiunKansi, person.gekkansi);
                string[] gouhouSanpouTaiunNen = person.GetGouhouSanpou(taiunKansi, person.nenkansi);

                if (gouhouSanpouTaiunNiti != null)
                {
                    DrawLine(idxMtxButtom, taiunCenterX, nikkansiCenterX, drawBottomSi, dircDown);
                    DrawString(idxMtxButtom, taiunCenterX, nikkansiCenterX, drawBottomSi, dircDown, gouhouSanpouTaiunNiti);
                    idxMtxButtom++;

                }
                if (gouhouSanpouTaiunGetu != null)
                {
                    DrawLine(idxMtxButtom, taiunCenterX, gekkansiCenterX, drawBottomSi, dircDown);
                    DrawString(idxMtxButtom, taiunCenterX, gekkansiCenterX, drawBottomSi, dircDown, gouhouSanpouTaiunGetu);
                    idxMtxButtom++;
                }

                if (gouhouSanpouTaiunNen != null)
                {
                    DrawLine(idxMtxButtom, taiunCenterX, nenkansiCenterX, drawBottomSi, dircDown);
                    DrawString(idxMtxButtom, taiunCenterX, nenkansiCenterX, drawBottomSi, dircDown, gouhouSanpouTaiunNen);
                    idxMtxButtom++;
                }

                string[] gouhouSanpouTaiunNenuni = person.GetGouhouSanpou(taiunKansi, nenunKansi);
                string[] gouhouSanpouNenunNiti = person.GetGouhouSanpou(nenunKansi, person.nikkansi);
                string[] gouhouSanpouNenunGetu = person.GetGouhouSanpou(nenunKansi, person.gekkansi);
                string[] gouhouSanpouNenunNen = person.GetGouhouSanpou(nenunKansi, person.nenkansi);

                if (gouhouSanpouTaiunNenuni != null)
                {
                    DrawLine(idxMtxButtom, nenunCenterX, taiunCenterX, drawBottomSi, dircDown);
                    DrawString(idxMtxButtom, nenunCenterX, taiunCenterX, drawBottomSi, dircDown, gouhouSanpouTaiunNenuni);
                    idxMtxButtom++;
                }
                if (gouhouSanpouNenunNiti != null)
                {
                    DrawLine(idxMtxButtom, nenunCenterX, nikkansiCenterX, drawBottomSi, dircDown);
                    DrawString(idxMtxButtom, nenunCenterX, nikkansiCenterX, drawBottomSi, dircDown, gouhouSanpouNenunNiti);
                    idxMtxButtom++;
                }
                if (gouhouSanpouNenunGetu != null)
                {
                    DrawLine(idxMtxButtom, nenunCenterX, gekkansiCenterX, drawBottomSi, dircDown);
                    DrawString(idxMtxButtom, nenunCenterX, gekkansiCenterX, drawBottomSi, dircDown, gouhouSanpouNenunGetu);
                    idxMtxButtom++;
                }

                if (gouhouSanpouNenunNen != null)
                {
                    DrawLine(idxMtxButtom, nenunCenterX, nenkansiCenterX, drawBottomSi, dircDown);
                    DrawString(idxMtxButtom, nenunCenterX, nenkansiCenterX, drawBottomSi, dircDown, gouhouSanpouNenunNen);
                    idxMtxButtom++;
                }

            }

        }

        ShukumeiDraw drawItem = null;
        KoutenUnDraw drawItem2 = null;
        private void DispShukumei(Person person)
        {

            Bitmap canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            // Graphicsオブジェクトの作成
            Graphics g = Graphics.FromImage(canvas);
            if (drawItem != null) drawItem.Dispose();
            drawItem = new ShukumeiDraw(person, g);
            drawItem.Draw();
            pictureBox1.Image = canvas;

        }

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


            Bitmap canvas2 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            // Graphicsオブジェクトの作成
            Graphics g2 = Graphics.FromImage(canvas2);
            if (drawItem2 != null) drawItem2.Dispose();
            drawItem2 = new KoutenUnDraw(person, g2);
            drawItem2.Draw(itemData.kansi, itemData2.kansi);
            pictureBox2.Image = canvas2;


        }


    }
}
