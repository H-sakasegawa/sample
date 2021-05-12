using System;
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
        Font fntSmallDisable = null;
        Pen blackPen = null;
        StringFormat stringFormat = null;
        StringFormat smallStringFormat = null;
        PictureBox pictureBox = null;
        Graphics g;
        int offsetY = 20;

        protected int dircUp = -1;  //上方向ライン描画
        protected int dircDown = +1;//下方向ライン描画

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
            fnt.Dispose();
        }

        /// <summary>
        /// 干支描画
        /// </summary>
        /// <param name="kansi"></param>
        /// <param name="rectKan"></param>
        /// <param name="rectSi"></param>
        protected void DrawKansi(Kansi kansi, Rectangle rectKan, Rectangle rectSi)
        {
            g.DrawString(kansi.kan, fnt, Brushes.Black, rectKan, stringFormat);
            g.DrawString(kansi.si, fnt, Brushes.Black, rectSi, stringFormat);
            g.DrawRectangle(blackPen, rectKan);
            g.DrawRectangle(blackPen, rectSi);
        }
        /// <summary>
        /// 位相法描画
        /// </summary>
        /// <param name="mtxIndex">描画位置マトリクスIndex</param>
        /// <param name="fromX">ライン描画開始X</param>
        /// <param name="toX">ライン描画終了X</param>
        /// <param name="baseY">基準Y座標</param>
        /// <param name="dirc">描画方向</param>
        protected void DrawLine(int mtxIndex, int fromX, int toX, int baseY, int dirc)
        {
            Point start = new Point(fromX, baseY);
            Point end = new Point(toX, baseY);
            Point startOfs = new Point(start.X, start.Y + ((mtxIndex + 1) * offsetY) * dirc);
            Point endOfs = new Point(end.X, end.Y + ((mtxIndex + 1) * offsetY) * dirc);

            g.DrawLine(blackPen, start, startOfs);
            g.DrawLine(blackPen, startOfs, endOfs);
            g.DrawLine(blackPen, endOfs, end);

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

        protected void DrawString(int mtxIndex, int from, int to, int baseY, int dirc, string[] aryStr)
        {
            float maxWidth = 0f;
            float sumHeight = 0f;
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

                g.DrawString(s, fntSmall, Brushes.Black, rect, smallStringFormat);
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
    }

}
