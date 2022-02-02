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
    public partial class FormFinderCustom : Form
    {
        class FindResultItem : LvItemDataBase
        {
            public FindResultItem(FinderCustom.ResultItem item )
            {
                findItem = item;
            }
            public FinderCustom.ResultItem findItem;
        }


        public event Common.CloseHandler OnClose = null;

        private const string title = "検索";
        private const string sep = "---";
        FormMain parentForm = null;
        TableMng tblMng;
        Persons personList;

        Person curPerson = null;

        FinderCustom finder = new FinderCustom();

        ComboBox[] cmbKan;
        ComboBox[] cmbSi;
        ComboBox[] cmbJudaiShusei;
        ComboBox[] cmbJunidaijusei;

        public FormFinderCustom(FormMain _parentForm)
        {
            InitializeComponent();

            parentForm = _parentForm;

        }

        private void FormSerch_Load( object sender, EventArgs e)
        {
            tblMng = TableMng.GetTblManage();

            this.MinimumSize = this.Size;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer2.IsSplitterFixed = true;


            //コンボボックス項目設定
            cmbKan = new ComboBox[]
            {
                cmbNitiKan,  cmbGetuKan, cmbNenKan,
                cmbNitiZoganChugen,cmbNitiZoganShogen, cmbNitiZoganHongen,
                cmbGetuZoganChugen, cmbGetuZoganShogen, cmbGetuZoganHongen,
                cmbNenZoganChugen, cmbNenZoganShogen, cmbNenZoganHongen
            };
            cmbSi = new ComboBox[]
            {
                cmbNitiSi,  cmbGetuSi, cmbNenSi,cmbNenTenchusatuSi, cmbNitiTenchusatuSi,
                cmbNenTenchusatuKan, cmbNitiTenchusatuKan,
            };

            cmbJudaiShusei = new ComboBox[]
            {
                cmbJudaishuseiA, cmbJudaishuseiB,cmbJudaishuseiC,cmbJudaishuseiD, cmbJudaishuseiE
            };
            cmbJunidaijusei = new ComboBox[]
            {
                cmbJunidaijuseiA, cmbJunidaijuseiB,cmbJunidaijuseiC
            };


            var lstJukan = tblMng.jyukanTbl.ToList();
            var lstJukanGogyo = lstJukan.ConvertAll(x => x.gogyou).Distinct();
            var lstJunisi = tblMng.jyunisiTbl.ToList();
            var lstJunisiGogyo = lstJunisi.ConvertAll(x => x.gogyou).Distinct();
            var lstJudaishusei = tblMng.juudaiShusei.lstJudaiShusei;
            var lstJudaishuseiGogyo = lstJudaishusei.ConvertAll(x => x.gogyou).Distinct();
            var lstJunidaijusei = tblMng.junidaiJusei.lstJunidaiJusei;

            FindItem emptyItem = new FindItem("", FindItem.ItemType.NONE);
            foreach (var cmb in cmbKan)
            {
                cmb.Items.Add(emptyItem);
                foreach (var item in lstJukan)
                {
                    cmb.Items.Add(new FindItem(item.name));
                }
                cmb.Items.Add(new FindItem(sep, FindItem.ItemType.SEP));
                foreach (var item in lstJukanGogyo)
                {
                    cmb.Items.Add(new FindItem(item, FindItem.ItemType.ATTR));
                }

            }
            foreach (var cmb in cmbSi)
            {
                cmb.Items.Add(emptyItem);
                foreach (var item in lstJunisi)
                {
                    cmb.Items.Add(new FindItem(item.name));
                }
                cmb.Items.Add(new FindItem(sep, FindItem.ItemType.SEP));
                foreach (var item in lstJunisiGogyo)
                {
                    cmb.Items.Add(new FindItem(item, FindItem.ItemType.ATTR));
                }
            }
            foreach (var cmb in cmbJudaiShusei)
            {
                cmb.Items.Add(emptyItem);
                foreach (var item in lstJudaishusei)
                {
                    cmb.Items.Add(new FindItem(item.name));
                }
                cmb.Items.Add(new FindItem(sep, FindItem.ItemType.SEP));
                foreach (var item in lstJudaishuseiGogyo)
                {
                    cmb.Items.Add(new FindItem(item, FindItem.ItemType.ATTR));
                }
            }
            foreach (var cmb in cmbJunidaijusei)
            {
                cmb.Items.Add(emptyItem);
                foreach (var item in lstJunidaijusei)
                {
                    cmb.Items.Add(new FindItem(item.name));
                }
              
            }

            //誕生日検索のFromとToコンボボックス
            int minYear = tblMng.setuiribiTbl.GetMinYear();
            int maxYear = tblMng.setuiribiTbl.GetMaxYear();

            for(int year = minYear; year<=maxYear; year++)
            {
                cmbFromYear.Items.Add(year);
                cmbToYear.Items.Add(year);
            }
            cmbFromYear.SelectedIndex = 0;
            cmbToYear.SelectedIndex = cmbToYear.Items.Count - 1;

            chkTargetDb.Checked = true;
            chkTargetBirthday.Checked = false;
            chkTargetBirthday_CheckedChanged(null, null);

            //フォーム上のコントロールのカーソルをdefaultに設定
            SetFormControlCursor(this);
        }

        void SetFormControlCursor(Control parentCtrl)
        {
            foreach(Control ctrl in parentCtrl.Controls)
            {
                ctrl.Cursor = default;
                SetFormControlCursor(ctrl);
            }
        }


        /// <summary>
        /// 検索開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click_1(object sender, EventArgs e)
        {
            btnFind.Enabled = false;
            lblStatus.Text = "";

            try
            {
                lvFindResultDB.Columns.Clear();
                lvFindResultDB.Items.Clear();

                //条件未入力チェック
                int n = 0;
                foreach (var item in cmbKan) { n += (item.SelectedIndex>0?1:0) ; }
                foreach (var item in cmbSi) { n += (item.SelectedIndex > 0 ? 1 : 0); }
                foreach (var item in cmbJudaiShusei) { n += (item.SelectedIndex > 0 ? 1 : 0); }
                foreach (var item in cmbJunidaijusei) { n += (item.SelectedIndex > 0 ? 1 : 0); }

                if( n==0)
                {
                    MessageBox.Show("条件が指定されていません");
                    return;
                }

                FinderCustom finder = new FinderCustom();

                FindParameter param =  new FindParameter();

                if (radAnd.Checked) param.cond = FindCondition.COND_AND;
                else param.cond = FindCondition.COND_OR;

                if (chkTargetDb.Checked) param.target |= (int)FindTarget.TARGET_DB;
                if (chkTargetBirthday.Checked) param.target |= (int)FindTarget.TARGET_BIRTHDAY;

                param.minYear = (int)cmbFromYear.SelectedItem;
                param.maxYear = (int)cmbToYear.SelectedItem;
                if(param.minYear> param.maxYear)
                {
                    //入れ替え
                    int year = param.minYear;
                    param.minYear = param.maxYear;
                    param.maxYear = year;
                }
                param.minYearSetuiribi = tblMng.setuiribiTbl.GetSetuiribi(param.minYear);


                //■陰占
                //干支
                param.nitiKansiKan = (FindItem)cmbNitiKan.SelectedItem;
                param.nitiKansiSi = (FindItem)cmbNitiSi.SelectedItem;
                param.getuKansiKan = (FindItem)cmbGetuKan.SelectedItem;
                param.getuKansiSi = (FindItem)cmbGetuSi.SelectedItem;
                param.nenKansiKan = (FindItem)cmbNenKan.SelectedItem;
                param.nenKansiSi = (FindItem)cmbNenSi.SelectedItem;

                //天中殺
                param.tenchusatuNiti[0] = (FindItem)cmbNitiTenchusatuKan.SelectedItem;
                param.tenchusatuNiti[1] = (FindItem)cmbNitiTenchusatuSi.SelectedItem;
                param.tenchusatuNen[0] = (FindItem)cmbNenTenchusatuKan.SelectedItem;
                param.tenchusatuNen[1] = (FindItem)cmbNenTenchusatuSi.SelectedItem;

                //蔵元
                param.zouganNitiShogen = (FindItem)cmbNitiZoganShogen.SelectedItem;
                param.zouganNitiChugen = (FindItem)cmbNitiZoganChugen.SelectedItem;
                param.zouganNitiHongen = (FindItem)cmbNitiZoganHongen.SelectedItem;
                param.zouganGetuShogen = (FindItem)cmbGetuZoganShogen.SelectedItem;
                param.zouganGetuChugen = (FindItem)cmbGetuZoganChugen.SelectedItem;
                param.zouganGetuHongen = (FindItem)cmbGetuZoganHongen.SelectedItem;
                param.zouganNenShogen = (FindItem)cmbNenZoganShogen.SelectedItem;
                param.zouganNenChugen = (FindItem)cmbNenZoganChugen.SelectedItem;
                param.zouganNenHongen = (FindItem)cmbNenZoganHongen.SelectedItem;

                //■陽占
                param.judaiShuseiA = (FindItem)cmbJudaishuseiA.SelectedItem;
                param.judaiShuseiB = (FindItem)cmbJudaishuseiB.SelectedItem;
                param.judaiShuseiC = (FindItem)cmbJudaishuseiC.SelectedItem;
                param.judaiShuseiD = (FindItem)cmbJudaishuseiD.SelectedItem;
                param.judaiShuseiE = (FindItem)cmbJudaishuseiE.SelectedItem;

                param.junidaiJuseiA = (FindItem)cmbJunidaijuseiA.SelectedItem;
                param.junidaiJuseiB = (FindItem)cmbJunidaijuseiB.SelectedItem;
                param.junidaiJuseiC = (FindItem)cmbJunidaijuseiC.SelectedItem;

                var result = finder.Find(param);

                DispResult(result);
            }
            finally
            {
                btnFind.Enabled = true;
            }

        }

        /// <summary>
        /// 検索結果表示
        /// </summary>
        /// <param name="result"></param>
        private void DispResult(FinderCustom.FindResult result)
        {
            lvFindResultDB.Columns.Clear();
            lvFindResultBIRTHDAY.Columns.Clear();
            lvFindResultDB.Items.Clear();
            lvFindResultBIRTHDAY.Items.Clear();

            if (result == null)
            {
                lblStatus.Text = string.Format("該当項目は見つかりませんでした。");
                return;
            }

            lblStatus.Text = string.Format("{0}件 見つかりました。", 
                result.lstFindItemsDB.Count + result.lstFindItemsBIRTHDAY.Count);

            //表示カラム設定
            foreach (var fmt in result.lstFormatTargetDB)
            {
                var colHeader = lvFindResultDB.Columns.Add(fmt.title, fmt.columnWidth);
                colHeader.Tag = fmt.type;
                colHeader.TextAlign = fmt.alignment;
            }
            //表示カラム設定
            foreach (var fmt in result.lstFormatTargetBIRTHDAY)
            {
                var colHeader = lvFindResultBIRTHDAY.Columns.Add(fmt.title, fmt.columnWidth);
                colHeader.Tag = fmt.type;
                colHeader.TextAlign = fmt.alignment;
            }

            foreach (var item in result.lstFindItemsDB)
            {
                ListViewItem lvItem = null;

                lvItem = lvFindResultDB.Items.Add(item.person.name);
                var tagIAttr = new FindResultItem(item); //FindResultItem

                lvItem.Tag = tagIAttr;

            }

            lvFindResultBIRTHDAY.BeginUpdate();
            foreach (var item in result.lstFindItemsBIRTHDAY)
            {
                ListViewItem lvItem = null;

                //カラム数分ループ
                for (int i = 0; i < item.lstItem.Count; i++)
                {
                    string str = (string)item.lstItem[i];
                    if (i == 0)
                    {
                        lvItem = lvFindResultBIRTHDAY.Items.Add(str);
                    }
                    else
                    {
                        lvItem.SubItems.Add(str);
                    }

                }
               var tagIAttr = new FindResultItem(item); //FindResultItem


                lvItem.Tag = tagIAttr;

            }
            lvFindResultBIRTHDAY.EndUpdate();

            if(result.lstFormatTargetDB.Count>0)
            {
                tabResult.SelectedIndex = 0;
            }else if(result.lstFormatTargetBIRTHDAY.Count > 0)
            {
                tabResult.SelectedIndex = 1;
            }
        }



        /// <summary>
        /// 検索結果に大運、年運の情報を連動表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstFindResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvFindResultDB.SelectedItems.Count <= 0) return;

            var lvItem = lvFindResultDB.SelectedItems[0];
            FinderCustom.ResultItem findItem = ((FindResultItem)lvItem.Tag).findItem;


            Person findPerson = (Person)findItem.person;

            parentForm.SelectFindResult(findPerson);
            
        }
        private void lvFindResultBIRTHDAY_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvFindResultBIRTHDAY.SelectedItems.Count <= 0) return;

            var lvItem = lvFindResultBIRTHDAY.SelectedItems[0];
            FinderCustom.ResultItem findItem = ((FindResultItem)lvItem.Tag).findItem;


            Person findPerson = (Person)findItem.person;

            parentForm.SelectFindResult(findPerson);

        }

        /// <summary>
        /// 指定タイプデータが表示されているカラムIndex取得
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private int GetColIndex(Finder.ResultFormat.Type type)
        {
            int iCol;
            for (iCol = 0; iCol < lvFindResultDB.Columns.Count; iCol++)
            {
                ColumnHeader colHeader = lvFindResultDB.Columns[iCol];
                if ((Finder.ResultFormat.Type)colHeader.Tag == type)
                {
                    return iCol;
                }
            }
            return -1;
        }


        private void DispCtrl()
        {
            //chkTenchusatu.Enabled = (radNattin.Checked || radRittin.Checked);
            //chkIncludeGetuun.Enabled = (radNattin.Checked || radRittin.Checked);
        }

        private void FormFinder_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (OnClose != null) OnClose(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var cmb in cmbKan) cmb.SelectedIndex = 0;
            foreach (var cmb in cmbSi) cmb.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var cmb in cmbJudaiShusei) cmb.SelectedIndex = 0;
            foreach (var cmb in cmbJunidaijusei) cmb.SelectedIndex = 0;
        }
        //生年月日検索チェックボックス
        private void chkTargetBirthday_CheckedChanged(object sender, EventArgs e)
        {
            cmbFromYear.Enabled = chkTargetBirthday.Checked;
            cmbToYear.Enabled = chkTargetBirthday.Checked;
        }
    }
}
