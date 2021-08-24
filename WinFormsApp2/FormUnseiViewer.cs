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
        Persons personList = null;
        int[] grpItemCnt = new int[2];
        TableMng tblMng = TableMng.GetTblManage();

        public FormUnseiViewer(Persons persons)
        {
            InitializeComponent();

            personList = persons;
        }

        private void FormUnseiViewer_Load(object sender, EventArgs e)
        {
 
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

            grpItemCnt[0] = AddColmnOnPerson(personList[0], true);
            grpItemCnt[1] = AddColmnOnPerson(personList[1]);

            DispNenun();

        }

        private void ColumnSetting()
        {


        }

        private int AddColmnOnPerson(Person person, bool bYearColume=false)
        {
            int idx;
            int cnt = 0;
            DataGridViewColumn column = new DataGridViewColumn();

             idx = grdViewNenUn.Columns.Add(person.name, "干支");
            grdViewNenUn.Columns[idx].Width = 60;
            cnt++;

            idx = grdViewNenUn.Columns.Add(person.name, "十大主星");
            grdViewNenUn.Columns[idx].Width = 40;
            cnt++;

            idx = grdViewNenUn.Columns.Add(person.name, "十二大従星");
            grdViewNenUn.Columns[idx].Width = 40;
            cnt++;

            idx = grdViewNenUn.Columns.Add(person.name, "日");
            grdViewNenUn.Columns[idx].Width = 100;
            cnt++;

            idx = grdViewNenUn.Columns.Add(person.name, "月");
            grdViewNenUn.Columns[idx].Width = 100;
            cnt++;

            idx = grdViewNenUn.Columns.Add(person.name, "年");
            grdViewNenUn.Columns[idx].Width = 100;
            cnt++;

            return cnt;
        }

        public void DispNenun()
        {
            grdViewNenUn.Rows.Clear();
            grdViewNenUn.Rows.Add(120);

            Person person = personList[0];
            int baseYear = person.birthday.year;
            int nenkansiNo = person.GetNenkansiNo(baseYear);

            var lstTaiunKansi = person.GetTaiunKansiList();
            Kansi taiunKansi=null;
            string shugosinAttr = person.shugosinAttr;
            string imigamiAttr = person.imigamiAttr;
            string[] choukouShugosinKan = null;
            if (string.IsNullOrEmpty(imigamiAttr))
            {
                choukouShugosinKan = person.choukouShugosin;
                imigamiAttr = person.choukouImigamiAttr;
            }

            for (int year=0; year < 120; year++)
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

                string title = string.Format("{0}歳({1})", (baseYear + year) - person.birthday.year, baseYear + year);
                AddNenunItem(personList[0], year, title, nenkansiNo, taiunKansi,
                                                   shugosinAttr, imigamiAttr, choukouShugosinKan);
                nenkansiNo += 1;
            }
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
                for (int i = 0; i < grpItemCnt[x]; i++)
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
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(colItem.Name,
                                    this.grdViewNenUn.ColumnHeadersDefaultCellStyle.Font,
                                    new SolidBrush(this.grdViewNenUn.ColumnHeadersDefaultCellStyle.ForeColor),
                                    rect,
                                    format);
                col += grpItemCnt[x];
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
            
            if(e.ColumnIndex<0 && e.RowIndex>=0)
            {
                //セルを描画する
                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);

                //行番号を描画する範囲を決定する
                //e.AdvancedBorderStyleやe.CellStyle.Paddingは無視しています
                Rectangle indexRect = e.CellBounds;
                indexRect.Inflate(-2, -2);
                //行番号を描画する
                TextRenderer.DrawText(e.Graphics,
                    (e.RowIndex + 1).ToString(),
                    e.CellStyle.Font,
                    indexRect,
                    e.CellStyle.ForeColor,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                //描画が完了したことを知らせる
                e.Handled = true;
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



        private void AddNenunItem(Person person, int year, string title, int targetkansiNo, Kansi taiunKansi,
                                  string shugosinAttr, string imigamiAttr, string[] choukouShugosinKan)
        {

            Kansi taregetKansi = person.GetKansi(targetkansiNo);
            int idxNanasatuItem = 0;


            string judai = person.GetJudaiShusei(person.nikkansi.kan, taregetKansi.kan).name;
            string junidai = person.GetJunidaiShusei(person.nikkansi.kan, taregetKansi.si).name;

            var row = grdViewNenUn.Rows[year];
            int cnt = 0;
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
            row.Cells[cnt++].Value = GetListViewItemString(gouhouSanpoui, kangou, nanasatu);

            //合法三法(月)
            gouhouSanpoui = person.GetGouhouSanpouEx(taregetKansi, person.gekkansi, taiunKansi, taregetKansi);
            kangou = person.GetKangoStr(taregetKansi, person.gekkansi); //干合  
            nanasatu = (person.IsNanasatu(taregetKansi, person.gekkansi, ref idxNanasatuItem) == true && idxNanasatuItem == 1) ? Const.sNanasatu : "";   //七殺
            row.Cells[cnt++].Value =  GetListViewItemString(gouhouSanpoui, kangou, nanasatu);

            //合法三法(年)
            gouhouSanpoui = person.GetGouhouSanpouEx(taregetKansi, person.nenkansi, taiunKansi, taregetKansi);
            kangou = person.GetKangoStr(taregetKansi, person.nenkansi); //干合  
            nanasatu = (person.IsNanasatu(taregetKansi, person.nenkansi, ref idxNanasatuItem) == true && idxNanasatuItem == 1) ? Const.sNanasatu : "";   //七殺
            row.Cells[cnt++].Value =  GetListViewItemString(gouhouSanpoui, kangou, nanasatu);


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

        //    lvItem.ForeColor = color;

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
    }
}
