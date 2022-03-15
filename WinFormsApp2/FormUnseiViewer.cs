using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace WinFormsApp2
{

    public partial class FormUnseiViewer : Form
    {
        //年運表行データTag情報
        class RowItems
        {
            public RowItems(int _year)
            {
                year = _year;
            }
            public int year;
        }

        class KoutenUn
        {
            public KoutenUn(Person _person, SplitContainer _sc, PictureBox _picture)
            {
                person = _person;
                sc = _sc;
                picture = _picture;
            }
            public Person person;
            public SplitContainer sc;
            public PictureBox picture;
        }

 
        public delegate void CloseHandler();
        public delegate void DelegateOnChangeCurYear(int year);

        Persons persons = null;
        Person basePerson = null;
        int grpItemCnt =0;
        uint MAX_YEAR_RANGE = 120;
        Color basePersonColor = Color.PaleTurquoise;

        int koutenunRectWidth = 35;

        bool bRowSelectEvent = false;

        bool bInitializeLoad = true;
        public event DelegateOnChangeCurYear OnChangeCurYear =null;
        public event Common.CloseHandler OnClose;

        TableMng tblMng = TableMng.GetTblManage();

        List<Person> dicPersons = new List<Person>();
        List<KoutenUn> lstKoutenUn = new List<KoutenUn>();

        Form1 parentForm;

        double Magnification = 1; //倍率

        public FormUnseiViewer(Form _parenForm, Persons _persons, Person _basePerson)
        {
            InitializeComponent();

            parentForm = (Form1)_parenForm;
            this.persons = _persons;
            basePerson = _basePerson;
        }

        private void FormUnseiViewer_Load(object sender, EventArgs e)
        {
            bInitializeLoad = true;
            //メンバーの表示状態を管理するディクショナリを作成
            persons.GetPersonList().ForEach(x => dicPersons.Add( x ) );

            //グループコンボボックス
            Common.SetGroupCombobox(persons, cmbGroup);
            cmbGroup_SelectedIndexChanged(null, null);

            grdViewNenUn.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            grdViewNenUn.ColumnHeadersHeight = this.grdViewNenUn.ColumnHeadersHeight * 2;
            grdViewNenUn.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            grdViewNenUn.Dock = DockStyle.Fill;
            grdViewNenUn.Cursor = Cursors.Arrow;

            grdViewNenUn.CellPainting += new DataGridViewCellPaintingEventHandler(grdViewNenUn_CellPainting);
            grdViewNenUn.Paint += new PaintEventHandler(grdViewNenUn_Paint);
            grdViewNenUn.ColumnWidthChanged += grdViewNenUn_ColumnWidthChanged;
            grdViewNenUn.Resize += grdViewNenUn_Resize;
            grdViewNenUn.Scroll += grdViewNenUn_Scroll;
            grdViewNenUn.RowEnter += grdViewNenUn_RowEnter;

            grdViewNenUn.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            grdViewNenUn.ReadOnly = true;
            grdViewNenUn.RowHeadersWidth = 70;

            txtMaxNenNum.Text = MAX_YEAR_RANGE.ToString();
            chkDispBaseYearRange.Checked = true;

            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.MouseWheel += OnFlowLayoutPanel_Wheel;

            DispListItem();


            trackBar1.Maximum = 15; //1.5
            trackBar1.Minimum = 5;  //0.5
            trackBar1.TickFrequency = trackBar1.Minimum + (trackBar1.Maximum - trackBar1.Minimum) / 2;

            int trackValue = 1;
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string sValue = config.AppSettings.Settings["UnseiViewerMagnification"].Value;
            if (!string.IsNullOrEmpty(sValue))
            {
                trackValue = int.Parse(sValue);
            }
            try
            {
                trackBar1.Value = trackValue;
            }
            catch
            {
                //異常値の場合は、デフォルト
            }
            bInitializeLoad = false;

        }

        private void OnFlowLayoutPanel_Wheel(object sender, MouseEventArgs e)
        {
            //if (Common.IsKeyLocked(System.Windows.Forms.Keys.ControlKey))
            //{
            //    int flg = 1;
            //    if (e.Delta < 0) flg = -1;
            //    //処理
            //    koutenunRectWidth += (2 * flg);
            //    DrawKoutenun();
            //    e.


            //}
           
        }

        private void cmbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Person> personsEx = null;
            if (cmbGroup.SelectedIndex == 0)
            {
                //全て
                personsEx = dicPersons;
            }
            else
            {
                personsEx = new List<Person>();
                var item = (Group)cmbGroup.SelectedItem;
                foreach (var person in item.members)
                {
                    personsEx.Add(person);
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


        private int AddColmnOnPerson(Person person,Color bkColor)
        {
            int cnt = 0;
            DataGridViewColumn column = new DataGridViewColumn();

            //大運情報
            AddColumn(person, "干支(大)", 60, bkColor);
            cnt++;

            //年運情報
            AddColumn(person, "干支", 60, bkColor);
            cnt++;

            AddColumn(person, "十大主星", 40, bkColor);
            cnt++;

            AddColumn(person, "十二大従星", 40, bkColor);
            cnt++;

            AddColumn(person, "日", 100, bkColor);
            cnt++;

            AddColumn(person, "月", 100, bkColor);
            cnt++;

            AddColumn(person, "年", 100, bkColor);
            cnt++;


            return cnt;
        }
        void AddColumn(Person person, string title, int width, Color bkColor)
        {
            int idx = grdViewNenUn.Columns.Add(person.name, title);
            grdViewNenUn.Columns[idx].Width = width;
            grdViewNenUn.Columns[idx].DefaultCellStyle.BackColor = bkColor;
            grdViewNenUn.Columns[idx].SortMode = DataGridViewColumnSortMode.NotSortable;

        }



        private void grdViewNenUn_Paint(object sender, PaintEventArgs e)
        {
            int colCount = grdViewNenUn.Columns.Count;
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
                person.Init();

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

            RemoveKoutenunAll();
            flowLayoutPanel1.Controls.Clear();

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
            //基準となる人の年運表示
            DispListItem(basePerson, colIndex);

            colIndex += grpItemCnt;

            foreach (var item in lstDispItems.Items)
            {

                Person person = (Person)item;

                //表示追加した人の年運表示
                DispListItem(person, colIndex);
                colIndex += grpItemCnt;
            }

            //-------------------------------------
            // 後天運表示更新
            //-------------------------------------
            DispKoutenUn();
        }

        public void DispListItem(Person person, int colIndex)
        {

            int baseYear = person.birthday.year;
            int nenkansiNo = person.GetNenkansiNo(baseYear, true);

            //大運表示用の干支リストを取得

            var lstTaiunKansi = person.GetTaiunKansiList();
            Kansi taiunKansi = null;
            //var shugosinAttrs = person.ShugosinAttrs;
            //var imigamiAttrs = person.ImigamiAttrs;
            //string[] choukouShugosinKan = null;
            //if (imigamiAttrs.Count==0)
            //{
            //    choukouShugosinKan = person.choukouShugosin;
            //    imigamiAttrs.Add( new  CustomShugosinAttr( person.choukouImigamiAttr ));
            //}

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
                    if (year < minYear || year >= maxYear)
                    {
                        nenkansiNo += 1;
                        continue;
                    }
                }
                
                idxRow = year - minYear;

                //string title = string.Format("{0}歳({1})", (baseYear + year) - person.birthday.year, baseYear + year);

                AddListItem(person,year, colIndex, idxRow, "", nenkansiNo, taiunKansi, ref prevTaiunKansiNo);
                nenkansiNo += 1;
            }
        }


        private void AddListItem(Person person, int year, int startCol,
                                int idxRow, string title, int targetkansiNo, Kansi taiunKansi,
                                ref int prevTaiunKansiNo)
        {

            //大運干支表示
            var taiunItem = Common.GetTaiunItem(person, "", taiunKansi.no, year);
            //年運情報取得
            Kansi nenunKansi = person.GetKansi(targetkansiNo);
            var nenunItem = Common.GetNenunItems(person, title, nenunKansi, taiunKansi);

            var row = grdViewNenUn.Rows[idxRow];

            int cnt = startCol;
            if( taiunKansi.no != prevTaiunKansiNo)
            {
                row.Cells[cnt].Value = taiunItem.sItems[(int)Const.ColUnseiLv.COL_KANSI];
                prevTaiunKansiNo = taiunKansi.no;
            }
            row.Cells[cnt].Style.BackColor = (taiunItem.bTenchusatu? Color.Red: row.Cells[cnt].Style.BackColor);
            cnt++;

           // row.Cells[cnt].Style.BackColor = (nenunItem.bTenchusatu ? Color.Red : row.Cells[cnt].Style.BackColor);
            for (int i = (int)Const.ColUnseiLv.COL_KANSI; i <= (int)Const.ColUnseiLv.COL_GOUHOUSANPOU_NEN; i++)
            {
                row.Cells[cnt].Value = nenunItem.sItems[i];
                row.Cells[cnt].Style.ForeColor = nenunItem.colorTenchusatu;
                cnt++;
            }

            row.Tag = new RowItems(year);
        }




        public void DispKoutenUn()
        {
            flowLayoutPanel1.SuspendLayout();

            RemoveKoutenunAll();
            flowLayoutPanel1.Controls.Clear();

            //基準となる人の後天運表示
            AddKoutenun(basePerson);

            foreach (var item in lstDispItems.Items)
            {
                Person person = (Person)item;
                //表示追加した人の後天運表示
                AddKoutenun(person);
            }
            UpdateKoutenUn();

            flowLayoutPanel1.ResumeLayout();
        }
        public void UpdateKoutenUn()
        {
            foreach(var item in lstKoutenUn)
            {
                DrawKoutenun(item);
            }
        }


        private void AddKoutenun(Person person)
        {
            Label lbl = new Label();
            lbl.Dock = DockStyle.Fill;
            lbl.Text = person.name;
            lbl.TextAlign = ContentAlignment.MiddleCenter;


            PictureBox pictureBox = new PictureBox();
           // pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.Dock = DockStyle.Fill;
           // pictureBox.SizeChanged += KoutenunPicture_OnSizeChanged;

            SplitContainer sc = new SplitContainer();
            sc.IsSplitterFixed = true;
            sc.Orientation = Orientation.Horizontal;
            sc.SplitterWidth = 1;
            sc.FixedPanel = FixedPanel.Panel1;  //分割パネル上側を固定パネルにする
            sc.Panel1MinSize = lbl.Height;
            sc.SplitterDistance = lbl.Height; //分割パネル上側の高さ設定
            sc.Panel1.Controls.Add(lbl);        //分割パネル上側に回数ラベルを追加
            sc.Panel2.Controls.Add(pictureBox); //分割パネル下側に後天運表示ピクチャーボックスを追加
            sc.Width = 200;  //この幅は、DrawKoutenun()で再設定されます。
            sc.Height = 200; //この高さは、DrawKoutenun()で再設定されます。

            pictureBox.SizeChanged += KoutenunPicture_OnSizeChanged;

            //for debug
            sc.BorderStyle = BorderStyle.FixedSingle;

            flowLayoutPanel1.Controls.Add(sc);
            lstKoutenUn.Add(new KoutenUn(person, sc, pictureBox));


        }
        private void RemoveKoutenun(Person person)
        {
            var item = lstKoutenUn.FirstOrDefault(x => x.person.name == person.name);
            if (item == null) return;

            SetPictureEvent(false);
            {
                flowLayoutPanel1.Controls.Remove(item.sc);
            }
            SetPictureEvent(true);
            lstKoutenUn.Remove(item);
        }
        private void RemoveKoutenunAll()
        {
            SetPictureEvent(false);
            {
                flowLayoutPanel1.Controls.Clear();
            }
            SetPictureEvent(true);

            lstKoutenUn.Clear();
        }

        /// <summary>
        /// 後天運描画ピクチャーボックスのサイズ変更イベントの有効・無効設定
        /// </summary>
        /// <param name="bActive"></param>
        private void SetPictureEvent(bool bActive)
        {

            foreach (SplitContainer sc in flowLayoutPanel1.Controls)
            {
                PictureBox picture = (PictureBox)sc.Panel2.Controls[0];
                if (!bActive) picture.SizeChanged -= KoutenunPicture_OnSizeChanged;
                else picture.SizeChanged += KoutenunPicture_OnSizeChanged;
            }
        }

        private void DrawKoutenun(KoutenUn koutenun)
        {
            //初回描画はsc.Heightと描画エリアサイズが不一致のため、sc.Heightが設定されると、
            //PictureBoxサイズが連動して変わるので、KoutenunPicture_OnSizeChangedから再度呼び出されます。
            //そのタイミング、正しい表示になります。
            var row = grdViewNenUn.SelectedRows[0];
            var rowItem = (RowItems)row.Tag;


            if (rowItem.year < koutenun.person.birthday.year)
            {
                koutenun.picture.Image = null;
                //koutenun.picture.BackColor = Color.LightGray;
                return;
            }
           // koutenun.picture.BackColor = SystemColors.Control;

            var taiunKansi = koutenun.person.GetTaiunKansi(rowItem.year);
            var nenunKansi = koutenun.person.GetNenkansi(rowItem.year, true);

            bool bSangouKaikyoku = parentForm.IsChkSangouKaikyoku();
            bool bGogyou = parentForm.IsChkGogyou();
            bool bGotoku = parentForm.IsChkGotoku();
            bool bZougan = parentForm.IsChkZougan();

            DrawKoutenUn drawItem2 = new DrawKoutenUn(koutenun.person, koutenun.picture,
                                        taiunKansi, nenunKansi, null,
                                        true, //大運との情報表示
                                        true, //年運との情報表示
                                        false,//月運との情報なし
                                        bSangouKaikyoku, //三合会局
                                        bGogyou,         //五行 
                                        bGotoku,         //五徳
                                        bZougan,         //蔵元
                                        false,           //十二親干法
                                        10               //フォントサイズ
                                        );
            drawItem2.rangeWidth = koutenunRectWidth;
            Size drawSize = drawItem2.CalcDrawAreaSize();

            SetPictureEvent(false);
            {
                //koutenun.sc.Width = Common.CalcDoubleToIntSize((drawSize.Width + koutenun.sc.SplitterWidth) * Magnification);
                //koutenun.sc.Height = Common.CalcDoubleToIntSize((drawSize.Height + koutenun.sc.SplitterDistance + koutenun.sc.SplitterWidth) * Magnification);
                koutenun.sc.Width = Common.CalcDoubleToIntSize(drawSize.Width * Magnification)+5;
                koutenun.sc.Height = Common.CalcDoubleToIntSize(drawSize.Height * Magnification)+30;
            }
            SetPictureEvent(true);
            drawItem2.Draw(
                     new Size(drawSize.Width, drawSize.Height), 
                     Magnification
                     );

        }


        private void KoutenunPicture_OnSizeChanged(object sender, EventArgs e)
        {
            KoutenUn item = lstKoutenUn.FirstOrDefault(x => x.picture == sender);
            if (item == null) return;
            DrawKoutenun(item);
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
            if (bInitializeLoad) return;
            DispListItem();
        }

        private void grdViewNenUn_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (grdViewNenUn.SelectedRows.Count > 0)
            {
                //後天運再描画
                UpdateKoutenUn();

                var row = grdViewNenUn.SelectedRows[0];
                var rowItem = (RowItems)row.Tag;

                bRowSelectEvent = true;
                OnChangeCurYear?.Invoke(rowItem.year);
                bRowSelectEvent = false;
            }
        }
        private void FormUnseiViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnClose?.Invoke(this);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            double value = (double)trackBar1.Value;
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["UnseiViewerMagnification"].Value = value.ToString();
            config.Save();

            //10倍したトラックバーの値を目的の倍率値に変換
            Magnification = value / 10.0;
            DispKoutenUn();

        }
        private void button2_Click(object sender, EventArgs e)
        {
            trackBar1.Value = 10;
        }
    }


}
