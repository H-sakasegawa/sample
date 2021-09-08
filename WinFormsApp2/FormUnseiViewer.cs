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

        public delegate void CloseHandler();
        public delegate void DelegateOnChangeCurYear(int year);

        Persons persons = null;
        Person basePerson = null;
        int grpItemCnt =0;
        uint MAX_YEAR_RANGE = 120;
        Color basePersonColor = Color.PaleTurquoise;

        bool bRowSelectEvent = false;


        public event DelegateOnChangeCurYear OnChangeCurYear =null;
        public event CloseHandler OnClose;

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
            persons.GetPersonList().ForEach(x => dicPersons.Add(x.name, x ) );

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

            DispListItem();



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

        public void SelectYear( int year )
        {
            if (bRowSelectEvent) return;
            if (year < minYear || year > maxYear) return;
            int index = year - minYear;

            var bk = OnChangeCurYear;
            OnChangeCurYear = null;

            grdViewNenUn.Rows[index].Selected = true;

            OnChangeCurYear = bk;
        }


        private void ColumnSetting()
        {


        }

        private int AddColmnOnPerson(Person person,Color bkColor)
        {
            int idx;
            int cnt = 0;
            DataGridViewColumn column = new DataGridViewColumn();

            //大運情報
            idx = grdViewNenUn.Columns.Add(person.name, "干支(大)");
            grdViewNenUn.Columns[idx].Width = 60;
            grdViewNenUn.Columns[idx].DefaultCellStyle.BackColor = bkColor;
            cnt++;

            //年運情報

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
            DispListItem();

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
            DispListItem();

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


        public void DispListItem()
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

            int colIndex = 0;
            //年運表示
            int i = 0;
            DispListItem(basePerson, colIndex);
            colIndex += grpItemCnt;

            foreach (var item in lstDispItems.Items)
            {

                Person person = (Person)item;

                //年運表示
                DispListItem(person, colIndex);

                colIndex += grpItemCnt;
            }

        }

        public void DispListItem(Person person, int colIndex)
        {


            int baseYear = person.birthday.year;
            int nenkansiNo = person.GetNenkansiNo(baseYear);

            //大運表示用の干支リストを取得

            var lstTaiunKansi = person.GetTaiunKansiList();
            Kansi taiunKansi = null;
            var shugosinAttr = person.ShugosinAttr;
            var imigamiAttr = person.ImigamiAttr;
            string[] choukouShugosinKan = null;
            if (imigamiAttr.Count==0)
            {
                choukouShugosinKan = person.choukouShugosin;
                imigamiAttr.Add( person.choukouImigamiAttr );
            }

            int startYear = person.birthday.year;
            int endYear = (int)(person.birthday.year + MAX_YEAR_RANGE - 1);

            //大運表示用の干支リストを取得

            int prevTaiunKansiNo = 0;
            for (int year = startYear; year <= endYear; year++)
            {
                //順行のみなので、60超えたら1にするだけ
                if (nenkansiNo > 60) nenkansiNo = 1;

                for (int iTaiun = 0; iTaiun < lstTaiunKansi.Count; iTaiun++)
                {
                    var taiun = lstTaiunKansi[iTaiun];

                    if (taiun.year <= year)
                    {
                        taiunKansi = person.GetKansi(taiun.kansiNo);
                    }
                    else
                    {
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

                //string title = string.Format("{0}歳({1})", (baseYear + year) - person.birthday.year, baseYear + year);

                AddListItem(person,year, colIndex, idxRow, "", nenkansiNo, taiunKansi,
                                                   shugosinAttr, imigamiAttr, choukouShugosinKan, ref prevTaiunKansiNo);
                nenkansiNo += 1;
            }
        }


        private void AddListItem(Person person, int year, int startCol,
                                int idxRow, string title, int targetkansiNo, Kansi taiunKansi,
                                List<string> shugosinAttr, List<string> imigamiAttr, string[] choukouShugosinKan,
                                ref int prevTaiunKansiNo)
        {

            //大運干支表示
            var taiunItem = Common.GetTaiunItem(person, "", taiunKansi.no, year,
                                        shugosinAttr, imigamiAttr, choukouShugosinKan);
            //年運情報取得
            var nenunItem = Common.GetNenunGetuunItems(person, title, targetkansiNo, taiunKansi,
                                                        shugosinAttr, imigamiAttr, choukouShugosinKan);

            var row = grdViewNenUn.Rows[idxRow];

            int cnt = startCol;
            if( taiunKansi.no != prevTaiunKansiNo)
            {
                row.Cells[cnt].Value = taiunItem.sItems[(int)Const.ColTaiun.COL_KANSI];
                prevTaiunKansiNo = taiunKansi.no;
            }
            row.Cells[cnt].Style.BackColor = (taiunItem.bTenchusatu? Color.Red: row.Cells[cnt].Style.BackColor);
            cnt++;

           // row.Cells[cnt].Style.BackColor = (nenunItem.bTenchusatu ? Color.Red : row.Cells[cnt].Style.BackColor);
            for (int i = (int)Const.ColNenunListView.COL_KANSI; i <= (int)Const.ColNenunListView.COL_GOUHOUSANPOU_NEN; i++)
            {
                row.Cells[cnt].Value = nenunItem.sItems[i];
                row.Cells[cnt].Style.ForeColor = nenunItem.colorTenchusatu;
                cnt++;
            }

            row.Tag = new RowItems(year);
        }

        class RowItems
        {
            public RowItems(int _year)
            {
                year = _year;
            }
            public int year;
        }

        private void txtMaxNenNum_Leave(object sender, EventArgs e)
        {

            if(!uint.TryParse(txtMaxNenNum.Text, out MAX_YEAR_RANGE))
            {
                MessageBox.Show("異常な値が設定されました");
                txtMaxNenNum.Text = MAX_YEAR_RANGE.ToString();
                return;
            }
            DispListItem();
        }

        private void chkDispBaseYearRange_CheckedChanged(object sender, EventArgs e)
        {
            DispListItem();
        }

        private void grdViewNenUn_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (grdViewNenUn.SelectedRows.Count > 0)
            {
                var row = grdViewNenUn.SelectedRows[0];
                var rowItem = (RowItems)row.Tag;

                bRowSelectEvent = true;
                OnChangeCurYear?.Invoke(rowItem.year);
                bRowSelectEvent = false;
            }
        }
        private void FormUnseiViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnClose?.Invoke();
        }
    }


}
