using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace WinFormsApp2
{
    /// <summary>
    /// 位相法 基準
    /// </summary>
    abstract class IsouhouBase : IDisposable
    {
        public Person person;
        public Font fnt = null;
        public Font fntBold = null;
        public Font fntSmall = null;
        public Font fntSmallMark = null;
        public Font fntSmallDisable = null;
        public Pen blackPen = null;
        public Pen redPen = null;
        public Pen redPenBold = null;
        public StringFormat stringFormat = null;
        public StringFormat smallStringFormat = null;

        TableMng tblMng = TableMng.GetTblManage();


        public bool bDrawRentangleKansi = true;//干支の枠表示有無
        /// </summary>
        PictureBox pictureBox = null;
        Graphics g;
        int offsetY = 20;

        protected int dircUp = -1;  //上方向ライン描画
        protected int dircDown = +1;//下方向ライン描画

        public string[] strInyou = new string[] {Const.sInyou };// "陰陽"
        public string[] strKangou = new string[] { Const.sKangou };  //"干合"
        public string[] strNanasatu = new string[] { Const.sNanasatu };//"七殺"

        // 4:[ ][ ][ ] 
        // 3:[ ][ ][ ]  (設定例）
        // 2:[ ][ ][ ]  日年[1][0][1]  (bitFlgNiti | bitFlgNen) 
        // 1:[ ][ ][ ]  月年[0][1][1]  (bitFlgGetu | bitFlgNen) 
        // 0:[ ][ ][ ]  日月[1][1][0]  (bitFlgNiti | bitFlgGetu)
        protected List<int> matrix = new List<int>();
        protected List<int> matrixBottom = new List<int>();
        protected int idxMtx = 0;
        protected int idxMtxButtom = 0;

        ///// <summary>
        ///// 干支　五行情報管理テーブル
        ///// </summary>
        //AttrTblItem[] attrTbl = new AttrTblItem[6];  //月運、年運、大運, 日干支、月干支、年干支

        TableMng.KansiAttrTblMng kansiAttrTbl = null;

        public Graphics graph
        {
            get { return g; }
        }
 
        /// <summary>
        /// 位相法基準クラス　コンストラクタ
        /// </summary>
        /// <param name="_person"></param>
        /// <param name="_pictureBox"></param>
        public IsouhouBase(Person _person, PictureBox _pictureBox)
        {
            person = _person;
            pictureBox = _pictureBox;


            blackPen = new Pen(Color.Black, 1);
            redPen = new Pen(Color.Red, 1); 
            redPenBold = new Pen(Color.Red, 2);

            var fontName = "メイリオ";
            fnt =  new Font(fontName, 14, FontStyle.Regular);
            fntBold = new Font(fontName, 14, FontStyle.Regular | FontStyle.Bold);
            fntSmall = new Font(fontName, 7, FontStyle.Regular);
            fntSmallMark = new Font(fontName, 5, FontStyle.Regular);
            fntSmallDisable = new Font(fontName, 8, FontStyle.Regular | FontStyle.Strikeout);


            //干支文字センタリング表示用フォーマット
            stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            smallStringFormat = new StringFormat();
            smallStringFormat.Alignment = StringAlignment.Center;
            smallStringFormat.LineAlignment = StringAlignment.Center;

            matrix.Add(0);
            matrixBottom.Add(0);

            //干支の五行属性管理テーブル
            kansiAttrTbl = new TableMng.KansiAttrTblMng();
           // for(int i=0; i<attrTbl.Length; i++) attrTbl[i] = new AttrTblItem();
        }
        /// <summary>
        /// 描画領域サイズ取得
        /// </summary>
        /// <returns></returns>
        public Size GetDrawArea()
        {
            return new Size(pictureBox.Width, pictureBox.Height);
        }
        /// <summary>
        /// フォント高さ取得
        /// </summary>
        /// <returns></returns>
        public int GetFontHeight()
        {
            return fnt.Height;
        }
        /// <summary>
        /// 小フォント高さ取得
        /// </summary>
        /// <returns></returns>
        public int GetSmallFontHeight()
        {
            return fntSmall.Height;
        }
        /// <summary>
        /// ライン描画位置 Y方向オフセット値取得
        /// </summary>
        /// <returns></returns>
        public int GetLineOffsetY()
        {
            return offsetY;
        }

        protected abstract void DrawItem(Graphics g);
        protected virtual void DrawKansi(Graphics g) { }

        /// <summary>
        /// 描画処理メイン
        /// </summary>
        public void Draw()
        {
            //派生先クラスの描画I/F呼び出し
            Bitmap canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            // Graphicsオブジェクトの作成
            g = Graphics.FromImage(canvas);

            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            DrawItem(g);

            pictureBox.Image = canvas;

        }
        int redColorItemBit = 0;

        public void DrawKyokiPattern( int _redColorItemBit)
        {
            redColorItemBit = _redColorItemBit;

            //派生先クラスの描画I/F呼び出し
            Bitmap canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            // Graphicsオブジェクトの作成
            g = Graphics.FromImage(canvas);

            //干支文字と枠のみ描画
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            DrawKansi(g);

            pictureBox.Image = canvas;

        }
        /// <summary>
        /// 解放処理
        /// </summary>
        public void Dispose()
        {
            blackPen.Dispose();
            redPen.Dispose();
            redPenBold.Dispose();
            fnt.Dispose();
            fntBold.Dispose();
            fntSmall.Dispose();
            fntSmallMark.Dispose();
            fntSmallDisable.Dispose();
            
        }

        /// <summary>
        /// 陰占陽　干支描画
        /// </summary>
        /// <param name="kansi"></param>
        /// <param name="rectKan"></param>
        /// <param name="rectSi"></param>
        /// <param name="foreColor"></param>
        protected void DrawInsenKansi(Kansi kansi, Rectangle rectKan, Rectangle rectSi, Color foreColor = default)
        {

            if (bDrawRentangleKansi)
            {
                g.DrawRectangle(blackPen, rectKan);
                g.DrawRectangle(blackPen, rectSi);
            }
            var brush = Brushes.Black;


           // string shugosinAttr = person.ShugosinAttr;
            string[] choukouShugosinKan = person.choukouShugosin;
           // string imigamiAttr = person.ImigamiAttr;

            //干の守護神判定
            //干、支の属性取得
            string kanAttr = tblMng.jyukanTbl[kansi.kan].gogyou;
            //string siAttr = tblMng.jyunisiTbl[kansi.si].gogyou;


            //守護神判定
            //if (!string.IsNullOrEmpty(shugosinAttr))
            //{
            //    if (kanAttr == shugosinAttr) g.FillRectangle(Const.brusShugosin, rectKan);
            //    //if (siAttr == shugosinAttr) g.FillRectangle(Const.brusShugosin, rectSi);
            //}
            //else
            //{
            //    if (choukouShugosinKan != null)
            //    {
            //        foreach (var kan in choukouShugosinKan)
            //        {
            //            if (kan == kansi.kan)
            //            {
            //                g.FillRectangle(Const.brusShugosin, rectKan);
            //                break;
            //            }
            //        }
            //    }
            //}
            if(IsShugosin(kansi.kan)) g.FillRectangle(Const.brusShugosin, rectKan);

            //忌神判定
            //if (kanAttr == imigamiAttr)
            //{
            //    g.FillRectangle(Const.brusImigami, rectKan);
            //}
            if (IsImigami(kansi.kan)) g.FillRectangle(Const.brusImigami, rectKan); ;

            if ( foreColor!=default)
            {
                brush = new SolidBrush(foreColor);
            }

            g.DrawString(kansi.kan, fnt, brush, rectKan, stringFormat);
            g.DrawString(kansi.si, fnt, brush, rectSi, stringFormat);
        }

        /// <summary>
        /// 十干 の守護神判定
        /// ※守護神情報に十二支がある場合は、干に該当する項目を守護神とする
        /// 　十二支がない場合は、五行属性に該当するものを守護神とする
        /// </summary>
        /// <param name="kan">十二支 干</param>
        /// <returns></returns>
        public bool IsShugosin(string kan)
        {
            var shugosinAttr = person.ShugosinAttrs;
            string[] choukouShugosinKan = person.choukouShugosin;

            //干の守護神判定
            //干、支の属性取得
            string kanAttr = tblMng.jyukanTbl[kan].gogyou;


            //守護神判定
            if (shugosinAttr.Count>0)
            {
                foreach (var shugoKan in shugosinAttr)
                {
                    if (shugoKan.junisi != null)
                    {
                        if (kan == shugoKan.junisi) return true;
                    }
                    else
                    {
                        if (kanAttr == shugoKan.gogyouAttr) return true;
                    }
                }
            }
            else
            {
                if (choukouShugosinKan != null)
                {
                    foreach (var shugoKan in choukouShugosinKan)
                    {
                        if (shugoKan == kan)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 十干　 忌神判定
        /// ※忌神情報に十二支がある場合は、干に該当する項目を守護神とする
        /// 　十二支がない場合は、五行属性に該当するものを守護神とする
        /// </summary>
        /// <param name="kan"></param>
        /// <returns></returns>
        public bool IsImigami(string kan)
        {
            var imigamiAttrs = person.ImigamiAttrs;
            string choukouImigami = person.choukouImigamiAttr;

            //干の守護神判定
            //干、支の属性取得
            string kanAttr = tblMng.jyukanTbl[kan].gogyou;

            //忌神判定
            if (imigamiAttrs.Count>0)
            {
                foreach (var imigami in imigamiAttrs)
                {
                    if (imigami.junisi != null)
                    {
                        if (kan == imigami.junisi) return true;
                    }
                    else
                    {
                        if (kanAttr == imigami.gogyouAttr) return true;
                    }
                }
            }
            else
            {
                if (choukouImigami != null)
                {
                    if (choukouImigami.IndexOf(kanAttr) >= 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// 干支描画
        /// </summary>
        /// <param name="kansi"></param>
        /// <param name="rectKan"></param>
        /// <param name="rectSi"></param>
        protected void DrawKansi(Kansi kansi, Rectangle rectKan, Rectangle rectSi, Color[] bkColor, Const.enumKansiItemID attrNo)
        {
            if (bkColor != null)
            {
                if (bkColor.Length >= 2)
                {
                    SolidBrush brsKan = new SolidBrush(bkColor[0]);
                    SolidBrush brsSi = new SolidBrush(bkColor[1]);
                    g.FillRectangle(brsKan, rectKan);
                    g.FillRectangle(brsSi, rectSi);
                    brsKan.Dispose();
                    brsSi.Dispose();
                }

                Rectangle rect;
                Size sz = new Size(8,5);
                if (kansiAttrTbl[(int)attrNo].attrKan != kansiAttrTbl[(int)attrNo].attrKanOrg)
                {
                    //左下
                    rect = new Rectangle(rectKan.X , rectKan.Y + rectKan.Height - sz.Height, sz.Width, sz.Height);
                    g.DrawString("◆", fntSmallMark, Brushes.Black, rect, smallStringFormat);


                }
                if (kansiAttrTbl[(int)attrNo].attrSi != kansiAttrTbl[(int)attrNo].attrSiOrg)
                {
                    rect = new Rectangle(rectSi.X, rectSi.Y + rectSi.Height - sz.Height, sz.Width, sz.Height);
                    g.DrawString("◆", fntSmallMark, Brushes.Black, rect, smallStringFormat);

                }

                Size sz2 = new Size(10, 10);
                //右下
                rect = new Rectangle(rectKan.X + rectKan.Width - sz2.Width, rectKan.Y + rectKan.Height - sz2.Height, sz2.Width, sz2.Height);
                g.DrawString(kansiAttrTbl[(int)attrNo].attrKan, fntSmall, Brushes.Black, rect, smallStringFormat);

                rect = new Rectangle(rectSi.X + rectKan.Width - sz2.Width, rectSi.Y + rectSi.Height - sz2.Height, sz2.Width, sz2.Height);
                g.DrawString(kansiAttrTbl[(int)attrNo].attrSi, fntSmall, Brushes.Black, rect, smallStringFormat);

            }
            if (bDrawRentangleKansi)
            {
                g.DrawRectangle(blackPen, rectKan);
                g.DrawRectangle(blackPen, rectSi);
            }
            var brushKan = Brushes.Black;
            var fntKan = fnt;
            if (redColorItemBit != 0)
            {
                int bit = Common.ConvEnumKansiItemIDToItemBit(attrNo);
                if ((redColorItemBit & bit) != 0)
                {
                    //赤太字表示指定
                    brushKan = Brushes.Red;
                    fntKan = fntBold;
                }
            }

            g.DrawString(kansi.kan, fntKan, brushKan, rectKan, stringFormat);
            g.DrawString(kansi.si, fnt, Brushes.Black, rectSi, stringFormat);
        }


        /// <summary>
        /// 位相法描画
        /// </summary>
        /// <param name="mtxIndex">描画位置マトリクスIndex</param>
        /// <param name="fromX">ライン描画開始X</param>
        /// <param name="toX">ライン描画終了X</param>
        /// <param name="baseY">基準Y座標</param>
        /// <param name="dirc">描画方向</param>
        protected void DrawLine(int mtxIndex, int fromX, int toX, int baseY, int dirc, int xOfset = 0)
        {
            Point start = new Point(fromX, baseY);
            Point end = new Point(toX, baseY);
            Point startOfs = new Point(start.X, start.Y + ((mtxIndex + 1) * offsetY) * dirc);
            Point endOfs = new Point(end.X, end.Y + ((mtxIndex + 1) * offsetY) * dirc);

            if (xOfset != 0)
            {
                start.Offset(xOfset, 0);
                end.Offset(xOfset, 0);
                startOfs.Offset(xOfset, 0);
                endOfs.Offset(xOfset, 0);
            }
            g.DrawLine(blackPen, start, startOfs);
            g.DrawLine(blackPen, startOfs, endOfs);
            g.DrawLine(blackPen, endOfs, end);

        }
        public class NanasatuDraw
        {
            public NanasatuDraw(int _centerX1, int _centerX2,int _idxNanasatu)
            {
                centerX[0] = _centerX1;
                centerX[1] = _centerX2;
                idxNanasatu = _idxNanasatu;
            }
            public int[] centerX = new int[2];
            public int idxNanasatu;
        }
        protected void DrawLineNanasatu(int mtxIndex, NanasatuDraw nanasatuDraw, int baseY, int dirc, int xOfset = 0)
        {
            int fromX = nanasatuDraw.centerX[0];
            int toX = nanasatuDraw.centerX[1];
            Point start = new Point(fromX, baseY);
            Point end = new Point(toX, baseY);
            Point startOfs = new Point(start.X, start.Y + ((mtxIndex + 1) * offsetY) * dirc);
            Point endOfs = new Point(end.X, end.Y + ((mtxIndex + 1) * offsetY) * dirc);

            if (xOfset != 0)
            {
                start.Offset(xOfset, 0);
                end.Offset(xOfset, 0);
                startOfs.Offset(xOfset, 0);
                endOfs.Offset(xOfset, 0);
            }
            g.DrawLine(blackPen, start, startOfs);
            g.DrawLine(blackPen, startOfs, endOfs);
            g.DrawLine(blackPen, endOfs, end);

            //七殺されるものの位置にXマーク
            int len = 7;
            int markX = nanasatuDraw.centerX[nanasatuDraw.idxNanasatu];
            g.DrawLine(redPenBold, new Point(markX - len, baseY - len), new Point(markX + len, baseY + len));
            g.DrawLine(redPenBold, new Point(markX + len, baseY - len), new Point(markX - len, baseY + len));


        }

        protected void DrawLine3Point(int mtxIndex, int[] posX, int baseY, int dirc, int xOfset, Color color = default(Color))
        {
            Point start = new Point(posX[0], baseY);
            Point center = new Point(posX[1], baseY);
            Point end = new Point(posX[2], baseY);
            Point startOfs = new Point(start.X, start.Y + ((mtxIndex + 1) * offsetY) * dirc);
            Point centerOfs = new Point(center.X, center.Y + ((mtxIndex + 1) * offsetY) * dirc);
            Point endOfs = new Point(end.X, end.Y + ((mtxIndex + 1) * offsetY) * dirc);

            if (xOfset != 0)
            {
                start.Offset(xOfset, 0);
                center.Offset(xOfset, 0);
                end.Offset(xOfset, 0);
                startOfs.Offset(xOfset, 0);
                centerOfs.Offset(xOfset, 0);
                endOfs.Offset(xOfset, 0);
            }
            if (color == default(Color)) color = Color.Red;

            Pen pen = new Pen(color);
            g.DrawLine(pen, start, startOfs);
            g.DrawLine(pen, startOfs, endOfs);
            g.DrawLine(pen, endOfs, end);
            g.DrawLine(pen, center, centerOfs);
            pen.Dispose();

            SolidBrush brush = new SolidBrush(color);
            g.FillRectangle(brush, start.X - 2, baseY - 2, 5, 5);
            g.FillRectangle(brush, center.X - 2, baseY - 2, 5, 5);
            g.FillRectangle(brush, end.X - 2, baseY - 2, 5, 5);
            brush.Dispose();
        }
        /// <summary>
        /// 文字列描画
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="s"></param>
        protected void DrawString(Rectangle rect, GouhouSannpouResult s)
        {
            DrawString( rect, s.orgName);
        }
        /// <summary>
        /// 文字列描画
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="s"></param>
        /// <param name="brush"></param>
        protected void DrawString(Rectangle rect, string s, Brush brush = null)
        {
            SizeF w = g.MeasureString(s, fntSmall);
            Rectangle fillRect = rect;
            if (fillRect.Width > w.Width)
            {
                fillRect.X = (int)(fillRect.X + (rect.Width - w.Width) / 2);
                fillRect.Width = (int)w.Width;
            }
            if (brush == null) brush = Brushes.Black;
            g.FillRectangle(Brushes.WhiteSmoke, fillRect);
            g.DrawString(s, fntSmall, brush, rect, smallStringFormat);
        }
        /// <summary>
        /// 文字列描画
        /// </summary>
        /// <param name="mtxIndex"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="baseY"></param>
        /// <param name="dirc"></param>
        /// <param name="str"></param>
        /// <param name="brush"></param>
        protected void DrawString(int mtxIndex, int from, int to, int baseY, int dirc, string str, Brush brush = null)
        {
            string[] aryStr = new string[]{ str };
            DrawString( mtxIndex,  from,  to,  baseY,  dirc,  aryStr, brush);

        }

        /// <summary>
        /// 文字列描画
        /// </summary>
        /// <param name="mtxIndex">描画位置マトリクスIndex</param>
        /// <param name="fromX">ライン描画開始X</param>
        /// <param name="toX">ライン描画終了X</param>
        /// <param name="baseY">基準Y座標</param>
        /// <param name="dirc">描画方向</param>
        /// <param name="strs">描画文字列配列</param>
        /// <param name="enableBit">文字列配列有効無効指定Bit情報</param>
        protected void DrawString(int mtxIndex, int from, int to, int baseY, int dirc, GouhouSannpouResult[] gsr)
        {
            float maxWidth = 0f;
            float sumHeight = 0f;
            //文字列の最大幅,高さ取得
            foreach (var s in gsr)
            {
                SizeF w = g.MeasureString(s.displayName, fntSmall);
                if (maxWidth < w.Width) maxWidth = w.Width;

                sumHeight += w.Height;
            }

            int x = from + (Math.Abs(from - to) - (int)Math.Ceiling(maxWidth)) / 2;
            int y = (int)(baseY + ((mtxIndex + 1) * offsetY) * dirc - Math.Ceiling(sumHeight) / 2) + 2;

            foreach (var s in gsr)
            {

                Rectangle rect = new Rectangle(x, y, (int)Math.Ceiling(maxWidth), fntSmall.Height);
                g.FillRectangle(Brushes.WhiteSmoke, rect);

                if (s.bEnable)
                {
                    g.DrawString(s.displayName, fntSmall, Brushes.Black, rect, smallStringFormat);
                }
                else
                {
                    g.DrawString(s.displayName, fntSmallDisable, Brushes.Gray, rect, smallStringFormat);
                }

                y += fntSmall.Height;
            }

        }
        /// <summary>
        /// 文字列描画
        /// </summary>
        /// <param name="mtxIndex"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="baseY"></param>
        /// <param name="dirc"></param>
        /// <param name="aryStr"></param>
        /// <param name="brush"></param>
        protected void DrawString(int mtxIndex, int from, int to, int baseY, int dirc, string[] aryStr, Brush brush = null)
        {
            float maxWidth = 0f;
            float sumHeight = 0f;
            if (brush == null) brush = Brushes.Black;
            //文字列の最大幅,高さ取得
            foreach (var s in aryStr)
            {
                SizeF w = g.MeasureString(s, fntSmall);
                if (maxWidth < w.Width) maxWidth = w.Width;

                sumHeight += w.Height;
            }

            int x = from + (Math.Abs(from - to) - (int)Math.Ceiling(maxWidth)) / 2;
            int y = (int)(baseY + ((mtxIndex + 1) * offsetY) * dirc - Math.Ceiling(sumHeight) / 2) + 2;

            foreach (var s in aryStr)
            {

                Rectangle rect = new Rectangle(x, y, (int)Math.Ceiling(maxWidth), fntSmall.Height);
                g.FillRectangle(Brushes.WhiteSmoke, rect);

                g.DrawString(s, fntSmall, brush, rect, smallStringFormat);
                y += fntSmall.Height;
            }

        }

        /// <summary>
        /// 上方描画制御用マトリクス設定
        /// </summary>
        /// <param name="bFlg"></param>
        /// <param name="mtxCheckBits"></param>
        /// <param name="mtxSetBits"></param>
        /// <returns></returns>
        public int SetMatrixUp(bool bFlg, int mtxCheckBits, int mtxSetBits)
        {
            return SetMatrix(bFlg, mtxCheckBits, mtxSetBits, ref matrix, ref idxMtx);
        }
        /// <summary>
        /// 下方描画制御用マトリクス設定
        /// </summary>
        /// <param name="bFlg"></param>
        /// <param name="mtxCheckBits"></param>
        /// <param name="mtxSetBits"></param>
        /// <returns></returns>
        public int SetMatrixDown(bool bFlg, int mtxCheckBits, int mtxSetBits)
        {
            return SetMatrix(bFlg, mtxCheckBits, mtxSetBits, ref matrixBottom, ref idxMtxButtom);
        }
        /// <summary>
        /// 描画制御用マトリクス設定
        /// </summary>
        /// <param name="bFlg"></param>
        /// <param name="mtxCheckBits"></param>
        /// <param name="mtxSetBits"></param>
        /// <param name="matrix"></param>
        /// <param name="idxMtx"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 指定された月運、年運、大運, 日干支、月干支、年干支
        /// に該当する属性マトリクス情報を返す
        /// </summary>
        /// <param name="kansiItemId">干支項目ID</param>
        /// <returns></returns>
        public AttrTblItem GetAttrTblItem(Const.enumKansiItemID kansiItemId)
        {
            return kansiAttrTbl[(int)kansiItemId];
        }

        /// <summary>
        /// 変化無し状態による属性マトリクス情報を作成
        /// </summary>
        /// <param name="person"></param>
        /// <param name="getuun"></param>
        /// <param name="nenun"></param>
        /// <param name="taiun"></param>
        public void CreateGogyouAttrMatrix(Person person, Kansi getuun=null, Kansi nenun=null, Kansi taiun=null)
        {

            kansiAttrTbl.CreateGogyouAttrMatrix(person, getuun, nenun, taiun);
            //var tblMng = TableMng.GetTblManage();
            ////月運
            //if(getuun!=null)attrTbl[(int)enumKansiItemID.GETUUN].Init(tblMng.jyukanTbl[getuun.kan].gogyou, tblMng.jyunisiTbl[getuun.si].gogyou);
            ////年運
            //if(nenun!=null)attrTbl[(int)enumKansiItemID.NENUN].Init( tblMng.jyukanTbl[nenun.kan].gogyou, tblMng.jyunisiTbl[nenun.si].gogyou);
            ////大運
            //if (taiun != null)attrTbl[(int)enumKansiItemID.TAIUN].Init(tblMng.jyukanTbl[taiun.kan].gogyou, tblMng.jyunisiTbl[taiun.si].gogyou);

            ////日干支
            //attrTbl[(int)enumKansiItemID.NIKKANSI].Init(tblMng.jyukanTbl[person.nikkansi.kan].gogyou, tblMng.jyunisiTbl[person.nikkansi.si].gogyou);
            ////月干支
            //attrTbl[(int)enumKansiItemID.GEKKANSI].Init(tblMng.jyukanTbl[person.gekkansi.kan].gogyou, tblMng.jyunisiTbl[person.gekkansi.si].gogyou);
            ////年干支
            //attrTbl[(int)enumKansiItemID.NENKANSI].Init(tblMng.jyukanTbl[person.nenkansi.kan].gogyou, tblMng.jyunisiTbl[person.nenkansi.si].gogyou);
        }

        /// <summary>
        /// 五行 属性カラー取得
        /// </summary>
        /// <param name="kansi">干支</param>
        /// <returns></returns>
        protected Color[] GetGogyouColor(Kansi kansi)
        {
            var tblMng = TableMng.GetTblManage();

            Color[] color = new Color[2];
            //十干支テーブルから 干に該当する五行名
            string attrName = tblMng.jyukanTbl[kansi.kan].gogyou;
            color[0] = tblMng.gogyouAttrColorTbl[attrName]; //干の色

            //十二支テーブルから 干に該当する五行名
            attrName = tblMng.jyunisiTbl[kansi.si].gogyou;
            color[1] = tblMng.gogyouAttrColorTbl[attrName]; //支の色

            return color;
        }
        /// <summary>
        /// 五行カラー取得
        /// </summary>
        /// <param name="kansiItemId">干支項目　識別ID</param>
        /// <returns></returns>
        protected Color[] GetGogyouColor(Const.enumKansiItemID kansiItemId)
        {
            var tblMng = TableMng.GetTblManage();

            var attrTblItem = kansiAttrTbl[(int)kansiItemId];

            Color[] color = new Color[2];
            //十干支テーブルから 干に該当する五行名
            color[0] = tblMng.gogyouAttrColorTbl[attrTblItem.attrKan ]; //干の色

            //十二支テーブルから 干に該当する五行名
            color[1] = tblMng.gogyouAttrColorTbl[attrTblItem.attrSi]; //支の色

            return color;
        }
        /// <summary>
        /// 五徳カラー取得（干支情報から）
        /// </summary>
        /// <param name="nikkansiKan">日干支</param>
        /// <param name="kansi">干支</param>
        /// <param name="bBaseKan">true...日干支の干（基準）</param>
        /// <returns></returns>
        protected Color[] GetGotokuColor(string nikkansiKan, Kansi kansi, bool bBaseKan = false)
        {
            var tblMng = TableMng.GetTblManage();
            Color[] color = new Color[2];

            string baseAttrName = tblMng.jyukanTbl[nikkansiKan].gogyou;
            string attrName;
            if (!bBaseKan)
            {
                //十干支テーブルから 干に該当する五行名
                attrName = tblMng.jyukanTbl[kansi.kan].gogyou;
                color[0] = GetGotokuColor(baseAttrName, attrName);
            }

            attrName = tblMng.jyunisiTbl[kansi.si].gogyou;
            color[1] = GetGotokuColor(baseAttrName, attrName);

            return color;
        }
        /// <summary>
        /// 五徳カラー取得（属性情報から）
        /// </summary>
        /// <param name="baseAttrName">基準五徳属性</param>
        /// <param name="attrName">取得対象五徳属性</param>
        /// <param name="bBaseKan">true...日干支の干（基準）</param>
        /// <returns></returns>
        protected Color GetGotokuColor(string baseAttrName, string attrName)
        {
            var tblMng = TableMng.GetTblManage();

            string gotokuName;

            //十干支テーブルから 干に該当する五行名
            gotokuName = tblMng.gotokuTbl.GetGotoku(baseAttrName, attrName);
            return tblMng.gotokuAttrColorTbl[gotokuName];
        }
        /// <summary>
        /// 干と支の各五徳カラー取得（属性情報から）
        /// </summary>
        /// <param name="orgColor">現在確定しているカラー情報配列</param>
        /// <param name="baseAttrName">基準属性名</param>
        /// <param name="attrNameKan">干の属性名</param>
        /// <param name="attrNameSi">支の属性名</param>
        /// <returns></returns>
        protected Color[] GetGotokuColor(Color[] orgColor ,string baseAttrName, string attrNameKan, string attrNameSi)
        {
            if ( !string.IsNullOrEmpty(attrNameKan) )orgColor[0] = GetGotokuColor(baseAttrName, attrNameKan);
            if ( !string.IsNullOrEmpty(attrNameSi)) orgColor[1] = GetGotokuColor(baseAttrName, attrNameSi);
            return orgColor;
        }

        /// <summary>
        /// "土"の数をカウント(宿命のみ）
        /// </summary>
        /// <returns></returns>
        private int GetAttrDoCount()
        {
            var tblMng = TableMng.GetTblManage();

            //合法反映前の属性について"土"の数をカウント
            int cnt = 0;
            //日干支
            if (tblMng.jyukanTbl[person.nikkansi.kan].gogyou == Const.sGogyouDo) cnt++;
            if (tblMng.jyunisiTbl[person.nikkansi.si].gogyou == Const.sGogyouDo) cnt++;
            //月干支
            if (tblMng.jyukanTbl[person.gekkansi.kan].gogyou == Const.sGogyouDo) cnt++;
            if (tblMng.jyunisiTbl[person.gekkansi.si].gogyou == Const.sGogyouDo) cnt++;
            //年干支
            if (tblMng.jyukanTbl[person.nenkansi.kan].gogyou == Const.sGogyouDo) cnt++;
            if (tblMng.jyunisiTbl[person.nenkansi.si].gogyou == Const.sGogyouDo) cnt++;

            return cnt;
        }
        /// <summary>
        /// "土"の数をカウント(宿命と月運、年運、大運）
        /// </summary>
        /// <param name="kansiGetuun"></param>
        /// <param name="kansiNenun"></param>
        /// <param name="kansiTaiun"></param>
        /// <returns></returns>
        private int GetAttrDoCount(Kansi kansiGetuun, Kansi kansiNenun, Kansi kansiTaiun)
        {
            var tblMng = TableMng.GetTblManage();

            //合法反映前の属性について"土"の数をカウント
            int cnt = GetAttrDoCount();
            //日干支
            if (tblMng.jyukanTbl[kansiGetuun.kan].gogyou == Const.sGogyouDo) cnt++;
            if (tblMng.jyunisiTbl[kansiGetuun.si].gogyou == Const.sGogyouDo) cnt++;
            //月干支
            if (tblMng.jyukanTbl[kansiNenun.kan].gogyou == Const.sGogyouDo) cnt++;
            if (tblMng.jyunisiTbl[kansiNenun.si].gogyou == Const.sGogyouDo) cnt++;
            //年干支
            if (tblMng.jyukanTbl[kansiTaiun.kan].gogyou == Const.sGogyouDo) cnt++;
            if (tblMng.jyunisiTbl[kansiTaiun.si].gogyou == Const.sGogyouDo) cnt++;

            return cnt;
        }

        /// <summary>
        /// 合法反映 変換カラー反映
        /// </summary>
        /// <param name="colorNikkansi"></param>
        /// <param name="colorGekkansi"></param>
        /// <param name="colorNenkansi"></param>
        public void RefrectGouhou(Color[] colorNikkansi, Color[] colorGekkansi, Color[] colorNenkansi)
        {
            //日干支、月干支、年干支の 干、支の数（６）
            int kansiItemNum = 6;
            RefrectGouhou(colorNikkansi, colorGekkansi, colorNenkansi, kansiItemNum);
        }
        /// <summary>
        /// 合法反映 変換カラー反映
        /// </summary>
        /// <param name="colorNikkansi"></param>
        /// <param name="colorGekkansi"></param>
        /// <param name="colorNenkansi"></param>
        /// <param name="kansiItemNum"></param>
        private void RefrectGouhou(Color[] colorNikkansi, Color[] colorGekkansi, Color[] colorNenkansi, int kansiItemNum)
        {
            var tblMng = TableMng.GetTblManage();

            int idx = 1;

            //合法反映前の属性について"土"の数をカウント
            int cnt = GetAttrDoCount();
            bool bManyAttrDo = (kansiItemNum / 2 < cnt) ? true : false;

            //支合と半会はダブらない

            //================================================
            //支合
            //================================================
            //日支 - 月支
            var gogyou = tblMng.sigouTbl.GetSigouAttr(person.nikkansi.si, person.gekkansi.si, bManyAttrDo);
            if(gogyou != null)
            {
                colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou]; 
                colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.NIKKANSI].attrSi = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.GEKKANSI].attrSi = gogyou;
            }
            //日支 - 年支
            gogyou = tblMng.sigouTbl.GetSigouAttr(person.nikkansi.si, person.nenkansi.si, bManyAttrDo);
            if (gogyou != null)
            {
                colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.NIKKANSI].attrSi = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.NENKANSI].attrSi = gogyou;
            }
            //月支 - 年支
            gogyou = tblMng.sigouTbl.GetSigouAttr(person.gekkansi.si, person.nenkansi.si, bManyAttrDo);
            if (gogyou != null)
            {
                colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.GEKKANSI].attrSi = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.NENKANSI].attrSi = gogyou;
            }
            //================================================
            //半会
            //================================================
            //日支 - 月支
            gogyou = tblMng.hankaiTbl.GetGogyou(person.nikkansi.si, person.gekkansi.si);
            if (gogyou != null)
            {
                colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.NIKKANSI].attrSi = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.GEKKANSI].attrSi = gogyou;
            }
            //日支 - 年支
            gogyou = tblMng.hankaiTbl.GetGogyou(person.nikkansi.si, person.nenkansi.si);
            if (gogyou != null)
            {
                colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.NIKKANSI].attrSi = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.NENKANSI].attrSi = gogyou;
            }
            //月支 - 年支
            gogyou = tblMng.hankaiTbl.GetGogyou(person.gekkansi.si, person.nenkansi.si);
            if (gogyou != null)
            {
                colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.GEKKANSI].attrSi = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.NENKANSI].attrSi = gogyou;
            }
        }

        /// <summary>
        /// 後天運表示用の合法反映 変換カラー反映
        /// </summary>
        /// <param name="colorNikkansi"></param>
        /// <param name="colorGekkansi"></param>
        /// <param name="colorNenkansi"></param>
        /// <param name="colorGetuun"></param>
        /// <param name="colorNenun"></param>
        /// <param name="colorTaiun"></param>
        /// <param name="kansiGetuun"></param>
        /// <param name="kansiNenun"></param>
        /// <param name="kansiTaiun"></param>
        /// <param name="bDispGetuun"></param>
        public void RefrectGouhou(Color[] colorNikkansi, Color[] colorGekkansi, Color[] colorNenkansi,
                                  Color[] colorGetuun, Color[] colorNenun, Color[] colorTaiun,
                                  Kansi kansiGetuun, Kansi kansiNenun, Kansi kansiTaiun,
                                  bool bDispGetuun
                                  )
        {
            var tblMng = TableMng.GetTblManage();

            int idx = 1;

            //日干支、月干支、年干支の 干、支の数（6）
            int kansiItemNum = 6;
            //月運干支、年運干支、大運干支の 干、支の数（6または、4）
            kansiItemNum += (bDispGetuun ? 6 : 4);

            int cnt = GetAttrDoCount(kansiGetuun, kansiNenun, kansiTaiun);
            bool bManyAttrDo = (kansiItemNum / 2 < cnt) ? true : false;

            //宿命カラー設定
            RefrectGouhou(colorNikkansi, colorGekkansi, colorNenkansi, kansiItemNum);

            string gogyou = "";
            //月運、年運、大運 カラー設定
            //================================================
            //支合
            //================================================
            //----------------------------------
            // 月運 →＊
            //----------------------------------
            if (bDispGetuun)
            {

                //月運支 - 年運（支）
                gogyou = tblMng.sigouTbl.GetSigouAttr(kansiGetuun.si, kansiNenun.si, bManyAttrDo);
                if (gogyou != null)
                {
                    if (colorGetuun != null)colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    if (colorNenun != null) colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.GETUUN].attrSi = gogyou;
                    kansiAttrTbl[(int)Const.enumKansiItemID.NENUN].attrSi = gogyou;
                }
                //月運支 - 大運（支）
                gogyou = tblMng.sigouTbl.GetSigouAttr(kansiGetuun.si, kansiTaiun.si, bManyAttrDo);
                if (gogyou != null)
                {
                    if (colorGetuun != null) colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    if (colorTaiun != null) colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.GETUUN].attrSi = gogyou;
                    kansiAttrTbl[(int)Const.enumKansiItemID.TAIUN].attrSi = gogyou;
                }
                //月運支 - 日（支）
                gogyou = tblMng.sigouTbl.GetSigouAttr(kansiGetuun.si, person.nikkansi.si, bManyAttrDo);
                if (gogyou != null)
                {
                    if (colorGetuun != null) colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    if (colorNikkansi != null) colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.GETUUN].attrSi = gogyou;
                    kansiAttrTbl[(int)Const.enumKansiItemID.NIKKANSI].attrSi = gogyou;
                }
                //月運支 - 月（支）
                gogyou = tblMng.sigouTbl.GetSigouAttr(kansiGetuun.si, person.gekkansi.si, bManyAttrDo);
                if (gogyou != null)
                {
                    if (colorGetuun != null) colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    if (colorGekkansi != null) colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.GETUUN].attrSi = gogyou;
                    kansiAttrTbl[(int)Const.enumKansiItemID.GEKKANSI].attrSi = gogyou;
                }
                //月運支 - 年（支）
                gogyou = tblMng.sigouTbl.GetSigouAttr(kansiGetuun.si, person.nenkansi.si, bManyAttrDo);
                if (gogyou != null)
                {
                    if (colorGetuun != null) colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    if (colorNenkansi != null) colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.GETUUN].attrSi = gogyou;
                    kansiAttrTbl[(int)Const.enumKansiItemID.NENKANSI].attrSi = gogyou;
                }
            }
            //----------------------------------
            // 年運 →＊
            //----------------------------------
            //年運支 - 大運（支）
            gogyou = tblMng.sigouTbl.GetSigouAttr(kansiNenun.si, kansiTaiun.si, bManyAttrDo);
            if (gogyou != null)
            {
                if (colorNenun != null) colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                if (colorTaiun != null) colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.NENUN].attrSi = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.TAIUN].attrSi = gogyou;
            }
            //年運支 - 日（支）
            gogyou = tblMng.sigouTbl.GetSigouAttr(kansiNenun.si, person.nikkansi.si, bManyAttrDo);
            if (gogyou != null)
            {
                if (colorNenun != null) colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                if (colorNikkansi != null) colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.NENUN].attrSi = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.NIKKANSI].attrSi = gogyou;
            }
            //年運支 - 月（支）
            gogyou = tblMng.sigouTbl.GetSigouAttr(kansiNenun.si, person.gekkansi.si, bManyAttrDo);
            if (gogyou != null)
            {
                if (colorNenun != null) colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                if (colorGekkansi != null) colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.NENUN].attrSi = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.GEKKANSI].attrSi = gogyou;
            }
            //年運支 - 年（支）
            gogyou = tblMng.sigouTbl.GetSigouAttr(kansiNenun.si, person.nenkansi.si, bManyAttrDo);
            if (gogyou != null)
            {
                if (colorNenun != null) colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                if (colorNenkansi != null) colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.NENUN].attrSi = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.NENKANSI].attrSi = gogyou;
            }
            //----------------------------------
            // 大運 →＊
            //----------------------------------
            //大運支 - 日（支）
            gogyou = tblMng.sigouTbl.GetSigouAttr(kansiTaiun.si, person.nikkansi.si, bManyAttrDo);
            if (gogyou != null)
            {
                if (colorTaiun != null) colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                if (colorNikkansi != null) colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.TAIUN].attrSi = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.NIKKANSI].attrSi = gogyou;
            }
            //大運支 - 月（支）
            gogyou = tblMng.sigouTbl.GetSigouAttr(kansiTaiun.si, person.gekkansi.si, bManyAttrDo);
            if (gogyou != null)
            {
                if (colorTaiun != null) colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                if (colorGekkansi != null) colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.TAIUN].attrSi = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.GEKKANSI].attrSi = gogyou;
            }
            //大運支 - 年（支）
            gogyou = tblMng.sigouTbl.GetSigouAttr(kansiTaiun.si, person.nenkansi.si, bManyAttrDo);
            if (gogyou != null)
            {
                if (colorTaiun != null) colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                if (colorNenkansi != null) colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.TAIUN].attrSi = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.NENKANSI].attrSi = gogyou;
            }



            //================================================
            //半会
            //================================================
            //----------------------------------
            // 月運 →＊
            //----------------------------------
            //月運支 - 日支
            if (bDispGetuun)
            {
                //月運支 - 年運支
                gogyou = tblMng.hankaiTbl.GetGogyou(kansiGetuun.si, kansiNenun.si);
                if (gogyou != null)
                {
                    if (colorGetuun != null) colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    if (colorNenun != null) colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.GETUUN].attrSi = gogyou;
                    kansiAttrTbl[(int)Const.enumKansiItemID.NENUN].attrSi = gogyou;
                }
                //月運支 - 大運（支）
                gogyou = tblMng.hankaiTbl.GetGogyou(kansiGetuun.si, kansiTaiun.si);
                if (gogyou != null)
                {
                    if (colorGetuun != null) colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    if (colorTaiun != null) colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.GETUUN].attrSi = gogyou;
                    kansiAttrTbl[(int)Const.enumKansiItemID.TAIUN].attrSi = gogyou;
                }
                //月運支 - 日（支）
                gogyou = tblMng.hankaiTbl.GetGogyou(kansiGetuun.si, person.nikkansi.si);
                if (gogyou != null)
                {
                    if (colorGetuun != null) colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    if (colorNikkansi != null) colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.GETUUN].attrSi = gogyou;
                    kansiAttrTbl[(int)Const.enumKansiItemID.NIKKANSI].attrSi = gogyou;
                }
                //月運支 - 月（支）
                gogyou = tblMng.hankaiTbl.GetGogyou(kansiGetuun.si, person.gekkansi.si);
                if (gogyou != null)
                {
                    if (colorGetuun != null) colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    if (colorGekkansi != null) colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.GETUUN].attrSi = gogyou;
                    kansiAttrTbl[(int)Const.enumKansiItemID.GEKKANSI].attrSi = gogyou;
                }
                //月運支 - 年（支）
                gogyou = tblMng.hankaiTbl.GetGogyou(kansiGetuun.si, person.nenkansi.si);
                if (gogyou != null)
                {
                    if (colorGetuun != null) colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    if (colorNenkansi != null) colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.GETUUN].attrSi = gogyou;
                    kansiAttrTbl[(int)Const.enumKansiItemID.NENKANSI].attrSi = gogyou;
                }
            }
            //----------------------------------
            // 年運 →＊
            //----------------------------------
            //年運(支) - 大運（支）
            gogyou = tblMng.hankaiTbl.GetGogyou(kansiNenun.si, kansiTaiun.si);
            if (gogyou != null)
            {
                if (colorNenun != null) colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                if (colorTaiun != null) colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.NENUN].attrSi = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.TAIUN].attrSi = gogyou;
            }
            //年運（支） - 日（支）
            gogyou = tblMng.hankaiTbl.GetGogyou(kansiNenun.si, person.nikkansi.si);
            if (gogyou != null)
            {
                if (colorNenun != null) colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                if (colorNikkansi != null) colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.NENUN].attrSi = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.NIKKANSI].attrSi = gogyou;
            }
            //年運（支） - 月（支）
            gogyou = tblMng.hankaiTbl.GetGogyou(kansiNenun.si, person.gekkansi.si);
            if (gogyou != null)
            {
                if (colorNenun != null) colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                if (colorGekkansi != null) colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.NENUN].attrSi = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.GEKKANSI].attrSi = gogyou;
            }
            //年運（支） - 年（支）
            gogyou = tblMng.hankaiTbl.GetGogyou(kansiNenun.si, person.nenkansi.si);
            if (gogyou != null)
            {
                if (colorNenun != null) colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                if (colorNenkansi != null) colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.NENUN].attrSi = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.NENKANSI].attrSi = gogyou;
            }
            //----------------------------------
            // 大運 →＊
            //----------------------------------
            //大運（支） - 日（支）
            gogyou = tblMng.hankaiTbl.GetGogyou(kansiTaiun.si, person.nikkansi.si);
            if (gogyou != null)
            {
                if (colorTaiun != null) colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                if (colorNikkansi != null) colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.TAIUN].attrSi = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.NIKKANSI].attrSi = gogyou;
            }
            //大運（支） - 月（支）
            gogyou = tblMng.hankaiTbl.GetGogyou(kansiTaiun.si, person.gekkansi.si);
            if (gogyou != null)
            {
                if (colorTaiun != null) colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                if (colorGekkansi != null) colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.TAIUN].attrSi = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.GEKKANSI].attrSi = gogyou;
            }
            //大運（支） - 年（支）
            gogyou = tblMng.hankaiTbl.GetGogyou(kansiTaiun.si, person.nenkansi.si);
            if (gogyou != null)
            {
                if (colorTaiun != null) colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                if (colorNenkansi != null) colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.TAIUN].attrSi = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.NENKANSI].attrSi = gogyou;
            }
        }

        /// <summary>
        /// 三合会局・方三位 カラー変換設定
        /// </summary>
        /// <param name="lstSangouKaikyoku"></param>
        /// <param name="lstHousani"></param>
        /// <param name="colorNikkansi"></param>
        /// <param name="colorGekkansi"></param>
        /// <param name="colorNenkansi"></param>
        /// <param name="colorGetuun"></param>
        /// <param name="colorNenun"></param>
        /// <param name="colorTaiun"></param>
        public void RefrectSangouKaikyokuHousanni(
                                  List<TableMng.SangouKaikyokuResult> lstSangouKaikyoku,
                                  List<TableMng.HouSaniResult> lstHousani,
                                  Color[] colorNikkansi, Color[] colorGekkansi, Color[] colorNenkansi,
                                  Color[] colorGetuun, Color[] colorNenun, Color[] colorTaiun
                                  )
        {
            var tblMng = TableMng.GetTblManage();
            int idx = 1; //干支の支の色を設定する

            foreach (var item in lstSangouKaikyoku)
            {

                if ((item.hitItemBit & Const.bitFlgGetuun) != 0)
                {
                    colorGetuun[idx] = tblMng.gogyouAttrColorTbl[item.sangouKaikyoku.gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.GETUUN].attrSi = item.sangouKaikyoku.gogyou;
                }
                if ((item.hitItemBit & Const.bitFlgNenun) != 0)
                {
                    colorNenun[idx] = tblMng.gogyouAttrColorTbl[item.sangouKaikyoku.gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.NENUN].attrSi = item.sangouKaikyoku.gogyou;
                }
                if ((item.hitItemBit & Const.bitFlgTaiun) != 0)
                {
                    colorTaiun[idx] = tblMng.gogyouAttrColorTbl[item.sangouKaikyoku.gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.TAIUN].attrSi = item.sangouKaikyoku.gogyou;
                }
                if ((item.hitItemBit & Const.bitFlgNiti) != 0)
                {
                    colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[item.sangouKaikyoku.gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.NIKKANSI].attrSi = item.sangouKaikyoku.gogyou;
                }
                if ((item.hitItemBit & Const.bitFlgGetu) != 0)
                {
                    colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[item.sangouKaikyoku.gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.GEKKANSI].attrSi = item.sangouKaikyoku.gogyou;
                }
                if ((item.hitItemBit & Const.bitFlgNen) != 0)
                {
                    colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[item.sangouKaikyoku.gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.NENKANSI].attrSi = item.sangouKaikyoku.gogyou;
                }

            }
            foreach (var item in lstHousani)
            {

                if ((item.hitItemBit & Const.bitFlgGetuun) != 0)
                {
                    colorGetuun[idx] = tblMng.gogyouAttrColorTbl[item.houSani.gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.GETUUN].attrSi = item.houSani.gogyou;
                }

                if ((item.hitItemBit & Const.bitFlgNenun) != 0)
                {
                    colorNenun[idx] = tblMng.gogyouAttrColorTbl[item.houSani.gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.NENUN].attrSi = item.houSani.gogyou;
                }
                if ((item.hitItemBit & Const.bitFlgTaiun) != 0)
                {
                    colorTaiun[idx] = tblMng.gogyouAttrColorTbl[item.houSani.gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.TAIUN].attrSi = item.houSani.gogyou;
                }
                if ((item.hitItemBit & Const.bitFlgNiti) != 0)
                {
                    colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[item.houSani.gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.NIKKANSI].attrSi = item.houSani.gogyou;
                }
                if ((item.hitItemBit & Const.bitFlgGetu) != 0)
                {
                    colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[item.houSani.gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.GEKKANSI].attrSi = item.houSani.gogyou;
                }
                if ((item.hitItemBit & Const.bitFlgNen) != 0)
                {
                    colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[item.houSani.gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.NENKANSI].attrSi = item.houSani.gogyou;
                }

            }


        }

        /// <summary>
        /// 干合 カラー変化設定
        /// </summary>
        /// <param name="colorNikkansi"></param>
        /// <param name="colorGekkansi"></param>
        /// <param name="colorNenkansi"></param>
        public void RefrectKangou(Color[] colorNikkansi, Color[] colorGekkansi, Color[] colorNenkansi)
        {
            var tblMng = TableMng.GetTblManage();

            int idx = 0;
            //================================================
            //干合
            //================================================
            //日（干） - 月（干）
            var gogyou = tblMng.kangouTbl.GetKangouAttr(person.nikkansi.kan, person.gekkansi.kan);
            if (gogyou != null)
            {
                colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.NIKKANSI].attrKan = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.GEKKANSI].attrKan = gogyou;
            }
            //日（干） - 年（干）
            gogyou = tblMng.kangouTbl.GetKangouAttr(person.nikkansi.kan, person.nenkansi.kan);
            if (gogyou != null)
            {
                colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.NIKKANSI].attrKan = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.NENKANSI].attrKan = gogyou;
            }
            //月（干） - 年（干）
            gogyou = tblMng.kangouTbl.GetKangouAttr(person.gekkansi.kan, person.nenkansi.kan);
            if (gogyou != null)
            {
                colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.GEKKANSI].attrKan = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.NENKANSI].attrKan = gogyou;
            }
        }
        /// <summary>
        /// 後天運表示用の干合 カラー変化設定
        /// </summary>
        /// <param name="colorNikkansi"></param>
        /// <param name="colorGekkansi"></param>
        /// <param name="colorNenkansi"></param>
        /// <param name="colorGetuun"></param>
        /// <param name="colorNenun"></param>
        /// <param name="colorTaiun"></param>
        /// <param name="kansiGetuun"></param>
        /// <param name="kansiNenun"></param>
        /// <param name="kansiTaiun"></param>
        /// <param name="bDispGetuun"></param>
        public void RefrectKangou(
                                    Color[] colorNikkansi, Color[] colorGekkansi, Color[] colorNenkansi,
                                    Color[] colorGetuun, Color[] colorNenun, Color[] colorTaiun,
                                    Kansi kansiGetuun, Kansi kansiNenun, Kansi kansiTaiun,
                                    bool bDispGetuun
           )
        {
            var tblMng = TableMng.GetTblManage();

            int idx = 0;
            string gogyou = "";
            //宿命カラー設定
            RefrectKangou(colorNikkansi, colorGekkansi, colorNenkansi);

            //月運、年運、大運 カラー設定
            //================================================
            //干合
            //================================================
            //----------------------------------
            // 月運 →＊
            //----------------------------------
            if (bDispGetuun)
            {
                //月運（干） - 年運（干）
                gogyou = tblMng.kangouTbl.GetKangouAttr(kansiGetuun.kan, kansiNenun.kan);
                if (gogyou != null)
                {
                    colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.GETUUN].attrKan = gogyou;
                    kansiAttrTbl[(int)Const.enumKansiItemID.NENUN].attrKan = gogyou;
                }
                //月運（干） - 大運（干）
                gogyou = tblMng.kangouTbl.GetKangouAttr(kansiGetuun.kan, kansiTaiun.kan);
                if (gogyou != null)
                {
                    colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.GETUUN].attrKan = gogyou;
                    kansiAttrTbl[(int)Const.enumKansiItemID.TAIUN].attrKan = gogyou;
                }
                //月運（干） - 日（干）
                gogyou = tblMng.kangouTbl.GetKangouAttr(kansiGetuun.kan, person.nikkansi.kan);
                if (gogyou != null)
                {
                    colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.GETUUN].attrKan = gogyou;
                    kansiAttrTbl[(int)Const.enumKansiItemID.NIKKANSI].attrKan = gogyou;
                }
                //月運（干） - 月（干）
                gogyou = tblMng.kangouTbl.GetKangouAttr(kansiGetuun.kan, person.gekkansi.kan);
                if (gogyou != null)
                {
                    colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.GETUUN].attrKan = gogyou;
                    kansiAttrTbl[(int)Const.enumKansiItemID.GEKKANSI].attrKan = gogyou;
                }
                //月運（干） - 年（干）
                gogyou = tblMng.kangouTbl.GetKangouAttr(kansiGetuun.kan, person.nenkansi.kan);
                if (gogyou != null)
                {
                    colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                    kansiAttrTbl[(int)Const.enumKansiItemID.GETUUN].attrKan = gogyou;
                    kansiAttrTbl[(int)Const.enumKansiItemID.NENKANSI].attrKan = gogyou;
                }
            }
            //----------------------------------
            // 年運 →＊
            //----------------------------------
            //年運（干） - 大運（干）
            gogyou = tblMng.kangouTbl.GetKangouAttr(kansiNenun.kan, kansiTaiun.kan);
            if (gogyou != null)
            {
                colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.NENUN].attrKan = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.TAIUN].attrKan = gogyou;
            }
            //年運（干） - 日（干）
            gogyou = tblMng.kangouTbl.GetKangouAttr(kansiNenun.kan, person.nikkansi.kan);
            if (gogyou != null)
            {
                colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.NENUN].attrKan = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.NIKKANSI].attrKan = gogyou;
            }
            //年運（干） - 月（干）
            gogyou = tblMng.kangouTbl.GetKangouAttr(kansiNenun.kan, person.gekkansi.kan);
            if (gogyou != null)
            {
                colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.NENUN].attrKan = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.GEKKANSI].attrKan = gogyou;
            }
            //年運（干） - 年（干）
            gogyou = tblMng.kangouTbl.GetKangouAttr(kansiNenun.kan, person.nenkansi.kan);
            if (gogyou != null)
            {
                colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.NENUN].attrKan = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.NENKANSI].attrKan = gogyou;
            }
            //----------------------------------
            // 大運 →＊
            //----------------------------------
            //大運（干） - 日（干）
            gogyou = tblMng.kangouTbl.GetKangouAttr(kansiTaiun.kan, person.nikkansi.kan);
            if (gogyou != null)
            {
                colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.TAIUN].attrKan = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.NIKKANSI].attrKan = gogyou;
            }
            //大運（干） - 月（干）
            gogyou = tblMng.kangouTbl.GetKangouAttr(kansiTaiun.kan, person.gekkansi.kan);
            if (gogyou != null)
            {
                colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.TAIUN].attrKan = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.GEKKANSI].attrKan = gogyou;
            }
            //大運（干） - 年（干）
            gogyou = tblMng.kangouTbl.GetKangouAttr(kansiTaiun.kan, person.nenkansi.kan);
            if (gogyou != null)
            {
                colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                kansiAttrTbl[(int)Const.enumKansiItemID.TAIUN].attrKan = gogyou;
                kansiAttrTbl[(int)Const.enumKansiItemID.NENKANSI].attrKan = gogyou;
            }

        }


    }

}
