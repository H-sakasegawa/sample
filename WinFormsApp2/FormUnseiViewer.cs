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

    public partial class FormUnseiViewer : Form
    {
        Persons persons = null;
        Person basePerson = null;
        int grpItemCnt =0;
        uint MAX_YEAR_RANGE = 120;
        Color basePersonColor = Color.PaleTurquoise;

        TableMng tblMng = TableMng.GetTblManage();

        Dictionary<string, Person> dicPersons = new Dictionary<string, Person>();

 
        public FormUnseiViewer(Persons _persons, Person _basePerson)
        {
            InitializeComponent();

            this.persons = _persons;
            basePerson = _basePerson;
        }

        private void FormUnseiViewer_Load(object sender, EventArgs e)
        {
            //メンバーの表示状態を管理するディクショナリを作成
            persons.GetPersons().ForEach(x => dicPersons.Add(x.name, x ) );

            //グループコンボボックス
            Common.SetGroupCombobox(persons, cmbGroup);
            cmbGroup_SelectedIndexChanged(null, null);

            grdViewNenUn.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            grdViewNenUn.ColumnHeadersHeight = this.grdViewNenUn.ColumnHeadersHeight * 2;
            grdViewNenUn.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            grdViewNenUn.CellPainting += new DataGridViewCellPaintingEventHandler(grdViewNenUn_CellPainting);
            grdViewNenUn.Paint += new PaintEventHandler(grdViewNenUn_Paint);
            grdViewNenUn.ColumnWidthChanged += grdViewNenUn_ColumnWidthChanged;
            grdViewNenUn.Resize += grdViewNenUn_Resize;
            grdViewNenUn.Scroll += grdViewNenUn_Scroll;

            grdViewNenUn.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            grdViewNenUn.ReadOnly = true;
            grdViewNenUn.RowHeadersWidth = 70;

            txtMaxNenNum.Text = MAX_YEAR_RANGE.ToString();
            chkDispBaseYearRange.Checked = true;

            DispNenun();



        }

        private void cmbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Person> personsEx = null;
            if (cmbGroup.SelectedIndex == 0)
            {
                //全て
                personsEx = dicPersons.Values.ToList();
            }
            else
            {
                personsEx = new List<Person>();
                var item = (Group)cmbGroup.SelectedItem;
                foreach (var person in item.members)
                {
                    personsEx.Add(dicPersons[person.name]);
                }

            }

            chkListPerson.Items.Clear();
            foreach ( var person in personsEx)
            {
                //基準人物は除外
                if (person == basePerson) continue;
                chkListPerson.Items.Add(person);
            }
         
        }



        private void ColumnSetting()
        {


        }

        private int AddColmnOnPerson(Person person,Color bkColor)
        {
            int idx;
            int cnt = 0;
            DataGridViewColumn column = new DataGridViewColumn();

             idx = grdViewNenUn.Columns.Add(person.name, "干支");
            grdViewNenUn.Columns[idx].Width = 60;
            grdViewNenUn.Columns[idx].DefaultCellStyle.BackColor = bkColor;
            cnt++;

            idx = grdViewNenUn.Columns.Add(person.name, "十大主星");
            grdViewNenUn.Columns[idx].Width = 40;
            grdViewNenUn.Columns[idx].DefaultCellStyle.BackColor = bkColor;
            cnt++;

            idx = grdViewNenUn.Columns.Add(person.name, "十二大従星");
            grdViewNenUn.Columns[idx].Width = 40;
            grdViewNenUn.Columns[idx].DefaultCellStyle.BackColor = bkColor;
            cnt++;

            idx = grdViewNenUn.Columns.Add(person.name, "日");
            grdViewNenUn.Columns[idx].Width = 100;
            grdViewNenUn.Columns[idx].DefaultCellStyle.BackColor = bkColor;
            cnt++;

            idx = grdViewNenUn.Columns.Add(person.name, "月");
            grdViewNenUn.Columns[idx].Width = 100;
            grdViewNenUn.Columns[idx].DefaultCellStyle.BackColor = bkColor;
            cnt++;

            idx = grdViewNenUn.Columns.Add(person.name, "年");
            grdViewNenUn.Columns[idx].Width = 100;
            grdViewNenUn.Columns[idx].DefaultCellStyle.BackColor = bkColor;
            cnt++;


            return cnt;
        }


 
        private void grdViewNenUn_Paint(object sender, PaintEventArgs e)
        {
            int colCount = grdViewNenUn.Columns.Count;
            int x = 0;
            int h = grdViewNenUn.ColumnHeadersHeight / 2;
            for (int col = 0; col < colCount; )
            {
                //グルーピングの最初のカラム名を取得
                var colItem = grdViewNenUn.Columns[col];
                string grpName = colItem.Name;

                int w = 0;
                int l = -1;
                int t = -1;
                for (int i = 0; i < grpItemCnt; i++)
                {
                    Rectangle r1 = this.grdViewNenUn.GetCellDisplayRectangle(col+i, -1, false); //get the column header cell
                    //同じグループの最終カラムの矩形領域を取得0
                    w += r1.Width;
                    if (l <=0)
                    {
                        l = r1.X;
                    }
                    if (t <=0)
                    {
                        t = r1.Y;
                    }
                }
                Rectangle rect = new Rectangle();
                rect.X = l+ 1;
                rect.Y =t +1;
                rect.Width = w - 2;
                rect.Height = h - 2;
                e.Graphics.FillRectangle(new SolidBrush(this.grdViewNenUn.ColumnHeadersDefaultCellStyle.BackColor), rect);
                e.Graphics.DrawRectangle(new Pen(Color.Black), rect);




                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(colItem.Name,
                                    this.grdViewNenUn.ColumnHeadersDefaultCellStyle.Font,
                                    new SolidBrush(this.grdViewNenUn.ColumnHeadersDefaultCellStyle.ForeColor),
                                    rect,
                                    format);
                col += grpItemCnt;
                x = 1;
            }
        }

        private void grdViewNenUn_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                e.PaintBackground(e.CellBounds, false);
                Rectangle r2 = e.CellBounds;
                r2.Y += e.CellBounds.Height / 2;
                r2.Height = e.CellBounds.Height / 2;
                e.PaintContent(r2);
                e.Handled = true;
            }

            //左列番号列
            if (e.ColumnIndex < 0)
            {
                if (e.RowIndex >= 0)
                {
                    //セルを描画する
                    e.Paint(e.ClipBounds, DataGridViewPaintParts.All);

                    //行番号を描画する範囲を決定する
                    //e.AdvancedBorderStyleやe.CellStyle.Paddingは無視しています
                    Rectangle indexRect = e.CellBounds;
                    indexRect.Inflate(-2, -2);
                    //行番号を描画する
                    TextRenderer.DrawText(e.Graphics,
                        (minYear + e.RowIndex).ToString(),
                        e.CellStyle.Font,
                        indexRect,
                        e.CellStyle.ForeColor,
                        TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                    //描画が完了したことを知らせる
                    e.Handled = true;
                }
            }
          
        }

        private void grdViewNenUn_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex>= 0)
            {
                if (e.RowIndex >= 0)
                {
                    e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
                }
            }
        }
        
        private void InvalidateHeader()
        {
            Rectangle rtHeader = this.grdViewNenUn.DisplayRectangle;
            rtHeader.Height = this.grdViewNenUn.ColumnHeadersHeight / 2;
            this.grdViewNenUn.Invalidate(rtHeader);
        }

        private void grdViewNenUn_Resize(object sender, EventArgs e)
        {
            this.InvalidateHeader();
        }

        private void grdViewNenUn_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            this.InvalidateHeader();
        }

        private void grdViewNenUn_Scroll(object sender, ScrollEventArgs e)
        {
            this.InvalidateHeader();
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

        private void chkListPerson_ItemCheck(object sender, ItemCheckEventArgs e)
        {
        }

        int minYear = 9999;
        int maxYear = 0;
        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            var items = chkListPerson.CheckedItems;

            foreach(var item in items)
            {
                Person person = (Person)item;
                person.Init(TableMng.GetTblManage());

                //未登録のみ追加
                if ( lstDispItems.Items.IndexOf(item)<0)
                {
                    lstDispItems.Items.Add(item);
                }
            }
            //年運表示
            DispNenun();

        }
        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            while (lstDispItems.SelectedIndex > -1)
            {
                lstDispItems.Items.RemoveAt(lstDispItems.SelectedIndex);
            }

            //年運表示
            DispNenun();

        }

        public void CreateColumn()
        {
            Color[] bkColors = new Color[] { Color.LightYellow, Color.Khaki };

            grdViewNenUn.Columns.Clear();

            grpItemCnt = AddColmnOnPerson(basePerson, basePersonColor);
            int i = 0;
            foreach (var item in lstDispItems.Items)
            {
                Person person = (Person)item;

                AddColmnOnPerson(person, bkColors[ i % 2]);
                i++;
            }
            grdViewNenUn.Columns[grpItemCnt - 1].Frozen = true;

        }


        public void DispNenun()
        {
            CreateColumn();

            //表示人物の中の最大、最小年
            minYear = 9999;
            maxYear = 0;
            //基準人物情報
            if (minYear > basePerson.birthday.year) minYear = basePerson.birthday.year;
            if (maxYear < basePerson.birthday.year + (int)MAX_YEAR_RANGE) maxYear = basePerson.birthday.year+ (int)MAX_YEAR_RANGE;
            if (!chkDispBaseYearRange.Checked)
            {
                //選択人物
                foreach (var item in lstDispItems.Items)
                {
                    Person person = (Person)item;
                    if (minYear > person.birthday.year) minYear = person.birthday.year;
                    if (maxYear < person.birthday.year + (int)MAX_YEAR_RANGE) maxYear = person.birthday.year + (int)MAX_YEAR_RANGE;
                }
            }

            grdViewNenUn.Rows.Clear();
            grdViewNenUn.Rows.Add(maxYear - minYear);

            //年運表示
            int colIndex = 0;
            int i = 0;
            DispNenun(basePerson, colIndex);
            colIndex += grpItemCnt;

            foreach (var item in lstDispItems.Items)
            {

                Person person = (Person)item;
                DispNenun(person, colIndex);

                colIndex += grpItemCnt;
            }

        }
        public void DispNenun(Person person, int colindex)
        {


            int baseYear = person.birthday.year;
            int nenkansiNo = person.GetNenkansiNo(baseYear);

            var lstTaiunKansi = person.GetTaiunKansiList();
            Kansi taiunKansi = null;
            string shugosinAttr = person.shugosinAttr;
            string imigamiAttr = person.imigamiAttr;
            string[] choukouShugosinKan = null;
            if (string.IsNullOrEmpty(imigamiAttr))
            {
                choukouShugosinKan = person.choukouShugosin;
                imigamiAttr = person.choukouImigamiAttr;
            }

            int startYear = person.birthday.year;
            int endYear = (int)(person.birthday.year + MAX_YEAR_RANGE - 1);

            for (int year = startYear; year <= endYear; year++)
            {
                //順行のみなので、60超えたら1にするだけ
                if (nenkansiNo > 60) nenkansiNo = 1;

                for (int iTaiun = 0; iTaiun < lstTaiunKansi.Count; iTaiun++)
                {
                    var taiun = lstTaiunKansi[iTaiun];

                    if (taiun.startYear > year)
                    {
                        taiunKansi = person.GetKansi(taiun.kansiNo);
                        break;
                    }
                }

                //表示Rowインデックス
                int idxRow;
                if (chkDispBaseYearRange.Checked)
                {
                    if (year < minYear || year >= maxYear) continue;
                }
                
                idxRow = year - minYear;
                

                string title = string.Format("{0}歳({1})", (baseYear + year) - person.birthday.year, baseYear + year);
                AddNenunItem(person, colindex, idxRow, title, nenkansiNo, taiunKansi,
                                                   shugosinAttr, imigamiAttr, choukouShugosinKan);
                nenkansiNo += 1;
            }
        }


        private void AddNenunItem(Person person, int startCol,
                                int idxRow, string title, int targetkansiNo, Kansi taiunKansi,
                                  string shugosinAttr, string imigamiAttr, string[] choukouShugosinKan)
        {

            Kansi taregetKansi = person.GetKansi(targetkansiNo);
            int idxNanasatuItem = 0;


            string judai = person.GetJudaiShusei(person.nikkansi.kan, taregetKansi.kan).name;
            string junidai = person.GetJunidaiShusei(person.nikkansi.kan, taregetKansi.si).name;

            var row = grdViewNenUn.Rows[idxRow];
            int cnt = startCol;

            //天中殺
            Color color = Color.Black;
            for (int i = 0; i < 2; i++)
            {
                if (taregetKansi.IsExist(person.nikkansi.tenchusatu[i]))
                {
                    color = Color.Red;
                    break;
                }
            }

            row.Cells[cnt++].Value = string.Format("{0}{1}", taregetKansi.kan, taregetKansi.si); //干支

            //"星"を削除
            judai = judai.Replace("星", "");
            junidai = junidai.Replace("星", "");


            row.Cells[cnt++].Value = judai; //十大主星
            row.Cells[cnt++].Value = junidai; //十二大従星

            //合法三法(日)
            GouhouSannpouResult[] gouhouSanpoui = person.GetGouhouSanpouEx(taregetKansi, person.nikkansi, taiunKansi, taregetKansi);
            string kangou = person.GetKangoStr(taregetKansi, person.nikkansi); //干合            
            string nanasatu = (person.IsNanasatu(taregetKansi, person.nikkansi, ref idxNanasatuItem) == true && idxNanasatuItem == 1) ? Const.sNanasatu : "";   //七殺
            row.Cells[cnt].Value = GetListViewItemString(gouhouSanpoui, kangou, nanasatu);
            row.Cells[cnt].Style.ForeColor = color;
            cnt++;

            //合法三法(月)
            gouhouSanpoui = person.GetGouhouSanpouEx(taregetKansi, person.gekkansi, taiunKansi, taregetKansi);
            kangou = person.GetKangoStr(taregetKansi, person.gekkansi); //干合  
            nanasatu = (person.IsNanasatu(taregetKansi, person.gekkansi, ref idxNanasatuItem) == true && idxNanasatuItem == 1) ? Const.sNanasatu : "";   //七殺
            row.Cells[cnt].Value = GetListViewItemString(gouhouSanpoui, kangou, nanasatu);
            row.Cells[cnt].Style.ForeColor = color;
            cnt++;

            //合法三法(年)
            gouhouSanpoui = person.GetGouhouSanpouEx(taregetKansi, person.nenkansi, taiunKansi, taregetKansi);
            kangou = person.GetKangoStr(taregetKansi, person.nenkansi); //干合  
            nanasatu = (person.IsNanasatu(taregetKansi, person.nenkansi, ref idxNanasatuItem) == true && idxNanasatuItem == 1) ? Const.sNanasatu : "";   //七殺
            row.Cells[cnt].Value = GetListViewItemString(gouhouSanpoui, kangou, nanasatu);
            row.Cells[cnt].Style.ForeColor = color;
            cnt++;


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



            //GetuunNenunLvItemData itemData = new GetuunNenunLvItemData();
            //itemData.keyValue = year;           //年 or 月
            //itemData.kansi = taregetKansi;    //干支
            //itemData.bShugosin = bShugosin;  //守護神
            //itemData.bImigami = bImigami;  //忌神

            //if (bShugosin)
            //{
            //    itemData.lstItemColors.Add(new LvItemColor(1, Const.colorShugosin));
            //}
            //else if (bImigami)
            //{
            //    itemData.lstItemColors.Add(new LvItemColor(1, Const.colorImigami));
            //}
            ////行のサブ情報を保持させておく
            //lvItem.Tag = itemData;

        }

        private void txtMaxNenNum_Leave(object sender, EventArgs e)
        {

            if(!uint.TryParse(txtMaxNenNum.Text, out MAX_YEAR_RANGE))
            {
                MessageBox.Show("異常な値が設定されました");
                txtMaxNenNum.Text = MAX_YEAR_RANGE.ToString();
                return;
            }
            DispNenun();
        }

        private void chkDispBaseYearRange_CheckedChanged(object sender, EventArgs e)
        {
            DispNenun();
        }
    }


}
