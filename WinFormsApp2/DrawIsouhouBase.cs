﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp2
{
    abstract class IsouhouBase : IDisposable
    {
        public Person person;
        Font fnt = null;
        Font fntSmall = null;
        Font fntSmallMark = null;
        Font fntSmallDisable = null;
        Pen blackPen = null;
        Pen redPen = null;
        StringFormat stringFormat = null;
        StringFormat smallStringFormat = null;
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

        public IsouhouBase(Person _person, PictureBox _pictureBox)
        {
            person = _person;
            pictureBox = _pictureBox;


            blackPen = new Pen(Color.Black, 1); ;
            redPen = new Pen(Color.Red, 1); ;

            fnt = new Font("MS Gothic", 14, FontStyle.Regular);
            fntSmall = new Font("MS Gothic", 8, FontStyle.Regular);
            fntSmallMark = new Font("MS Gothic", 5, FontStyle.Regular);
            fntSmallDisable = new Font("MS Gothic", 8, FontStyle.Regular | FontStyle.Strikeout);


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
            redPen.Dispose();
            fnt.Dispose();
        }

        /// <summary>
        /// 干支描画
        /// </summary>
        /// <param name="kansi"></param>
        /// <param name="rectKan"></param>
        /// <param name="rectSi"></param>
        protected void DrawKansi(Kansi kansi, Rectangle rectKan, Rectangle rectSi, Color[] color=null, Color[] colorOrg=null)
        {
            if (color != null)
            {
                if (color.Length >= 2)
                {
                    SolidBrush brsKan = new SolidBrush(color[0]);
                    SolidBrush brsSi = new SolidBrush(color[1]);
                    g.FillRectangle(brsKan, rectKan);
                    g.FillRectangle(brsSi, rectSi);
                    brsKan.Dispose();
                    brsSi.Dispose();
                }

                if( colorOrg!=null)
                {
                    Size sz = new Size(8,5);
                    if (color[0] != colorOrg[0])
                    {
                        Rectangle rect = new Rectangle(rectKan.X , rectKan.Y + rectKan.Height - sz.Height, sz.Width, sz.Height);
                        g.DrawString("◆", fntSmallMark, Brushes.Black, rect, smallStringFormat);

                    }
                    if (color[1] != colorOrg[1])
                    {
                        Rectangle rect = new Rectangle(rectSi.X, rectSi.Y + rectSi.Height - sz.Height, sz.Width, sz.Height);
                        g.DrawString("◆", fntSmallMark, Brushes.Black, rect, smallStringFormat);

                    }
                }
            }
            g.DrawRectangle(blackPen, rectKan);
            g.DrawRectangle(blackPen, rectSi);
            g.DrawString(kansi.kan, fnt, Brushes.Black, rectKan, stringFormat);
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
        protected void DrawLine(int mtxIndex, int fromX, int toX, int baseY, int dirc, int xOfset=0)
        {
            Point start = new Point(fromX, baseY);
            Point end = new Point(toX, baseY);
            Point startOfs = new Point(start.X, start.Y + ((mtxIndex + 1) * offsetY) * dirc);
            Point endOfs = new Point(end.X, end.Y + ((mtxIndex + 1) * offsetY) * dirc);

            if(xOfset!=0)
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
        /// 五行 属性カラー取得
        /// </summary>
        /// <param name="kansi"></param>
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
        protected Color[] GetGotokuColor(string nikkansiKan, Kansi kansi, bool bBaseKan=false)
        {
            var tblMng = TableMng.GetTblManage();
            Color[] color = new Color[2];

            string gotokuName;

            string baseAttrName = tblMng.jyukanTbl[nikkansiKan].gogyou;
            string attrName;
            if (!bBaseKan)
            {
                //十干支テーブルから 干に該当する五行名
                attrName = tblMng.jyukanTbl[kansi.kan].gogyou;
                gotokuName = tblMng.gotokuTbl.GetGotoku(baseAttrName, attrName);
                color[0] = tblMng.gotokuAttrColorTbl[gotokuName];
            }

            attrName = tblMng.jyunisiTbl[kansi.si].gogyou;
            gotokuName = tblMng.gotokuTbl.GetGotoku(baseAttrName, attrName);
            color[1] = tblMng.gotokuAttrColorTbl[gotokuName];

            return color;
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

        //合法反映 カラー設定
        public void RefrectGouhou(Color[] colorNikkansi, Color[] colorGekkansi, Color[] colorNenkansi)
        {
            //日干支、月干支、年干支の 干、支の数（６）
            int kansiItemNum = 6;
            RefrectGouhou(colorNikkansi, colorGekkansi, colorNenkansi, kansiItemNum);
        }
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
            var gogyou = tblMng.sigouTbl.GetGogyouAttr(person.nikkansi.si, person.gekkansi.si, bManyAttrDo);
            if(gogyou != null)
            {
                colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou]; 
                colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //日支 - 年支
            gogyou = tblMng.sigouTbl.GetGogyouAttr(person.nikkansi.si, person.nenkansi.si, bManyAttrDo);
            if (gogyou != null)
            {
                colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //月支 - 年支
            gogyou = tblMng.sigouTbl.GetGogyouAttr(person.gekkansi.si, person.nenkansi.si, bManyAttrDo);
            if (gogyou != null)
            {
                colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
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
            }
            //日支 - 年支
            gogyou = tblMng.hankaiTbl.GetGogyou(person.nikkansi.si, person.nenkansi.si);
            if (gogyou != null)
            {
                colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //月支 - 年支
            gogyou = tblMng.hankaiTbl.GetGogyou(person.gekkansi.si, person.nenkansi.si);
            if (gogyou != null)
            {
                colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
        }

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

            //月運、年運、大運 カラー設定
            //================================================
            //支合
            //================================================
            //----------------------------------
            // 月運 →＊
            //----------------------------------
            //月運支 - 年運支
            var gogyou = tblMng.sigouTbl.GetGogyouAttr(kansiGetuun.si, kansiNenun.si, bManyAttrDo);
            if (gogyou != null)
            {
                colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //月運支 - 大運支
            gogyou = tblMng.sigouTbl.GetGogyouAttr(kansiGetuun.si, kansiTaiun.si, bManyAttrDo);
            if (gogyou != null)
            {
                colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //月運支 - 日支
            gogyou = tblMng.sigouTbl.GetGogyouAttr(kansiGetuun.si, person.nikkansi.si, bManyAttrDo);
            if (gogyou != null)
            {
                colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //月運支 - 月支
            gogyou = tblMng.sigouTbl.GetGogyouAttr(kansiGetuun.si, person.gekkansi.si, bManyAttrDo);
            if (gogyou != null)
            {
                colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //月運支 - 年支
            gogyou = tblMng.sigouTbl.GetGogyouAttr(kansiGetuun.si, person.nenkansi.si, bManyAttrDo);
            if (gogyou != null)
            {
                colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }

            //----------------------------------
            // 年運 →＊
            //----------------------------------
            //年運支 - 大運支
            gogyou = tblMng.sigouTbl.GetGogyouAttr(kansiNenun.si, kansiTaiun.si, bManyAttrDo);
            if (gogyou != null)
            {
                colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //年運支 - 日支
            gogyou = tblMng.sigouTbl.GetGogyouAttr(kansiNenun.si, person.nikkansi.si, bManyAttrDo);
            if (gogyou != null)
            {
                colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //年運支 - 月支
            gogyou = tblMng.sigouTbl.GetGogyouAttr(kansiNenun.si, person.gekkansi.si, bManyAttrDo);
            if (gogyou != null)
            {
                colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //年運支 - 年支
            gogyou = tblMng.sigouTbl.GetGogyouAttr(kansiNenun.si, person.nenkansi.si, bManyAttrDo);
            if (gogyou != null)
            {
                colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //----------------------------------
            // 大運 →＊
            //----------------------------------
            //大運支 - 日支
            gogyou = tblMng.sigouTbl.GetGogyouAttr(kansiTaiun.si, person.nikkansi.si, bManyAttrDo);
            if (gogyou != null)
            {
                colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //大運支 - 月支
            gogyou = tblMng.sigouTbl.GetGogyouAttr(kansiTaiun.si, person.gekkansi.si, bManyAttrDo);
            if (gogyou != null)
            {
                colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //大運支 - 年支
            gogyou = tblMng.sigouTbl.GetGogyouAttr(kansiTaiun.si, person.nenkansi.si, bManyAttrDo);
            if (gogyou != null)
            {
                colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }



            //================================================
            //半会
            //================================================
            //----------------------------------
            // 月運 →＊
            //----------------------------------
            //月運支 - 年運支
            gogyou = tblMng.hankaiTbl.GetGogyou(kansiGetuun.si, kansiNenun.si);
            if (gogyou != null)
            {
                colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //月運支 - 大運支
            gogyou = tblMng.hankaiTbl.GetGogyou(kansiGetuun.si, kansiTaiun.si);
            if (gogyou != null)
            {
                colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //月運支 - 日支
            gogyou = tblMng.hankaiTbl.GetGogyou(kansiGetuun.si, person.nikkansi.si);
            if (gogyou != null)
            {
                colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //月運支 - 月支
            gogyou = tblMng.hankaiTbl.GetGogyou(kansiGetuun.si, person.gekkansi.si);
            if (gogyou != null)
            {
                colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //月運支 - 年支
            gogyou = tblMng.hankaiTbl.GetGogyou(kansiGetuun.si, person.nenkansi.si);
            if (gogyou != null)
            {
                colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //----------------------------------
            // 年運 →＊
            //----------------------------------
            //年運(支) - 大運（支）
            gogyou = tblMng.hankaiTbl.GetGogyou(kansiNenun.si, kansiTaiun.si);
            if (gogyou != null)
            {
                colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //年運（支） - 日（支）
            gogyou = tblMng.hankaiTbl.GetGogyou(kansiNenun.si, person.nikkansi.si);
            if (gogyou != null)
            {
                colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //年運（支） - 月（支）
            gogyou = tblMng.hankaiTbl.GetGogyou(kansiNenun.si, person.gekkansi.si);
            if (gogyou != null)
            {
                colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //年運（支） - 年（支）
            gogyou = tblMng.hankaiTbl.GetGogyou(kansiNenun.si, person.nenkansi.si);
            if (gogyou != null)
            {
                colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //----------------------------------
            // 大運 →＊
            //----------------------------------
            //大運（支） - 日（支）
            gogyou = tblMng.hankaiTbl.GetGogyou(kansiTaiun.si, person.nikkansi.si);
            if (gogyou != null)
            {
                colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //大運（支） - 月（支）
            gogyou = tblMng.hankaiTbl.GetGogyou(kansiTaiun.si, person.gekkansi.si);
            if (gogyou != null)
            {
                colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //大運（支） - 年（支）
            gogyou = tblMng.hankaiTbl.GetGogyou(kansiTaiun.si, person.nenkansi.si);
            if (gogyou != null)
            {
                colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
        }


        public void RefrectSangouKaikyokuHousanni(
                                  List<TableMng.SangouKaikyokuResult> lstSangouKaikyoku,
                                  List<TableMng.HouSaniResult> lstHousani,
                                  Color[] colorNikkansi, Color[] colorGekkansi, Color[] colorNenkansi,
                                  Color[] colorGetuun, Color[] colorNenun, Color[] colorTaiun
                                  )
        {
            var tblMng = TableMng.GetTblManage();
            int idx = 1;

            foreach (var item in lstSangouKaikyoku)
            {

                if ((item.hitItemBit & Const.bitFlgGetuun) != 0) colorGetuun[idx] = tblMng.gogyouAttrColorTbl[item.sangouKaikyoku.gogyou];
                if ((item.hitItemBit & Const.bitFlgNenun) != 0) colorNenun[idx] = tblMng.gogyouAttrColorTbl[item.sangouKaikyoku.gogyou];
                if ((item.hitItemBit & Const.bitFlgTaiun) != 0) colorTaiun[idx] = tblMng.gogyouAttrColorTbl[item.sangouKaikyoku.gogyou];
                if ((item.hitItemBit & Const.bitFlgNiti) != 0) colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[item.sangouKaikyoku.gogyou];
                if ((item.hitItemBit & Const.bitFlgGetu) != 0) colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[item.sangouKaikyoku.gogyou];
                if ((item.hitItemBit & Const.bitFlgNen) != 0) colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[item.sangouKaikyoku.gogyou];

            }
            foreach (var item in lstHousani)
            {

                if ((item.hitItemBit & Const.bitFlgGetuun) != 0) colorGetuun[idx] = tblMng.gogyouAttrColorTbl[item.houSani.gogyou];
                if ((item.hitItemBit & Const.bitFlgNenun) != 0) colorNenun[idx] = tblMng.gogyouAttrColorTbl[item.houSani.gogyou];
                if ((item.hitItemBit & Const.bitFlgTaiun) != 0) colorTaiun[idx] = tblMng.gogyouAttrColorTbl[item.houSani.gogyou];
                if ((item.hitItemBit & Const.bitFlgNiti) != 0) colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[item.houSani.gogyou];
                if ((item.hitItemBit & Const.bitFlgGetu) != 0) colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[item.houSani.gogyou];
                if ((item.hitItemBit & Const.bitFlgNen) != 0) colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[item.houSani.gogyou];

            }

        }

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
            }
            //日（干） - 年（干）
            gogyou = tblMng.kangouTbl.GetKangouAttr(person.nikkansi.kan, person.nenkansi.kan);
            if (gogyou != null)
            {
                colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //月（干） - 年（干）
            gogyou = tblMng.kangouTbl.GetKangouAttr(person.gekkansi.kan, person.nenkansi.kan);
            if (gogyou != null)
            {
                colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
        }
        public void RefrectKangou(
                Color[] colorNikkansi, Color[] colorGekkansi, Color[] colorNenkansi,
                Color[] colorGetuun, Color[] colorNenun, Color[] colorTaiun,
                                  Kansi kansiGetuun, Kansi kansiNenun, Kansi kansiTaiun
            )
        {
            var tblMng = TableMng.GetTblManage();

            int idx = 0;
            //宿命カラー設定
            RefrectKangou(colorNikkansi, colorGekkansi, colorNenkansi);

            //月運、年運、大運 カラー設定
            //================================================
            //干合
            //================================================
            //----------------------------------
            // 月運 →＊
            //----------------------------------
            //月運（干） - 年運（干）
            var gogyou = tblMng.kangouTbl.GetKangouAttr(kansiGetuun.kan, kansiNenun.kan);
            if (gogyou != null)
            {
                colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //月運（干） - 大運（干）
            gogyou = tblMng.kangouTbl.GetKangouAttr(kansiGetuun.kan, kansiTaiun.kan);
            if (gogyou != null)
            {
                colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //月運（干） - 日（干）
            gogyou = tblMng.kangouTbl.GetKangouAttr(kansiGetuun.kan, person.nikkansi.kan);
            if (gogyou != null)
            {
                colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //月運（干） - 月（干）
            gogyou = tblMng.kangouTbl.GetKangouAttr(kansiGetuun.kan, person.gekkansi.kan);
            if (gogyou != null)
            {
                colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //月運（干） - 年（干）
            gogyou = tblMng.kangouTbl.GetKangouAttr(kansiGetuun.kan, person.nenkansi.kan);
            if (gogyou != null)
            {
                colorGetuun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
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
            }
            //年運（干） - 日（干）
            gogyou = tblMng.kangouTbl.GetKangouAttr(kansiNenun.kan, person.nikkansi.kan);
            if (gogyou != null)
            {
                colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNikkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //年運（干） - 月（干）
            gogyou = tblMng.kangouTbl.GetKangouAttr(kansiNenun.kan, person.gekkansi.kan);
            if (gogyou != null)
            {
                colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //年運（干） - 年（干）
            gogyou = tblMng.kangouTbl.GetKangouAttr(kansiNenun.kan, person.nenkansi.kan);
            if (gogyou != null)
            {
                colorNenun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
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
            }
            //大運（干） - 月（干）
            gogyou = tblMng.kangouTbl.GetKangouAttr(kansiTaiun.kan, person.gekkansi.kan);
            if (gogyou != null)
            {
                colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorGekkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }
            //大運（干） - 年（干）
            gogyou = tblMng.kangouTbl.GetKangouAttr(kansiTaiun.kan, person.nenkansi.kan);
            if (gogyou != null)
            {
                colorTaiun[idx] = tblMng.gogyouAttrColorTbl[gogyou];
                colorNenkansi[idx] = tblMng.gogyouAttrColorTbl[gogyou];
            }

        }


    }

}
