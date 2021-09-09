using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Drawing.Drawing2D;

namespace WinFormsApp2
{

    /// <summary>
    /// 十二親干法 表示用クラス
    /// </summary>
    class DrawJuniSinKanhoun
    {
        public delegate void CloseHandler();

        Person person;
        PictureBox pictureBox;
        StringFormat stringFormat;
        public Pen blackPen = null;

        Font fnt;
        Font fntBold;
        SolidBrush brush;
        int strHeight;
        int strWidth;
        int horLineLength;  //縦方向ラインの長さ
        int verLineLength;  //横方向のライン長さ

        public DrawJuniSinKanhoun()
        {
            var fontName = "メイリオ";
            fnt = new Font(fontName, 14, FontStyle.Regular);
            fntBold = new Font(fontName, 14, FontStyle.Bold);
            brush = new SolidBrush(Color.Black);
            stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            blackPen = new Pen(Color.Black, 1);

            strHeight = (int)(fnt.Height * Const.dKansiHeightRate);

            strWidth = 45;
            horLineLength = 25;
            verLineLength = 10;
        }
        /// <summary>
        /// 解放処理
        /// </summary>
        public void Dispose()
        {
            blackPen.Dispose();
            fnt.Dispose();
            fntBold.Dispose();
            stringFormat.Dispose();

        }

        /// <summary>
        /// 十二親干法 図描画
        /// </summary>
        /// <param name="person">人情報</param>
        /// <param name="pictureBox">描画先のピクチャーボックス</param>
        /// <param name="_bDispGogyou">true...五行反映</param>
        /// <param name="_bDispGotoku">true... 五徳反映</param>
        /// <param name="_bDispRefrectGouhou">true...五行/五徳反映時の合法反映表示</param>
        public void Draw(
                            Person _person,
                            PictureBox pictureBox,
                            Node node
            )
        {

            person = _person;

            //派生先クラスの描画I/F呼び出し
            Bitmap canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            // Graphicsオブジェクトの作成
            var g = Graphics.FromImage(canvas);
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            pictureBox.Image = canvas;

            Point ofsPnt = new Point(0, 0);
            //親方向
            CalcOffset(node, ref ofsPnt, true);

            Point drawPnt = ofsPnt;
            drawPnt.X += 10; //左側を少しだけ隙間を開ける
            drawPnt.Y += 10; //上側を少しだけ隙間を開ける

            //自身描画
            Rectangle rectKan = new Rectangle(drawPnt.X, drawPnt.Y, strWidth, strHeight);

            g.DrawRectangle(blackPen, rectKan);
            g.DrawString(node.kan, fntBold, brush, rectKan, stringFormat);

            if (node.gender == Gender.MAN)
            {
                DrawNodeToParent(g, node.parent, drawPnt, DIRC.enumLeft);
                DrawNodeToPartner(g, node.partnerWoman, drawPnt, DIRC.enumRight);
            }
            else
            {
                DrawNodeToParent(g, node.parent, drawPnt, DIRC.enumLeft);
                DrawNodeToPartner(g, node.partnerMan, drawPnt, DIRC.enumRight);
                DrawNodeToChild(g, node.child, drawPnt, DIRC.enumLeft);
            }


        }

        /// <summary>
        /// 左上座標までのオフセットを計算
        /// </summary>
        /// <param name="node"></param>
        /// <param name="pnt"></param>
        /// <param name="bFirst"></param>
        void CalcOffset(Node node, ref Point pnt,bool bFirst=false)
        {

            if (node.parent != null)
            {
                if (bFirst || node.gender == Gender.MAN)
                {
                    pnt.Y += (verLineLength + strHeight);
                    CalcOffset(node.parent, ref pnt);
                }
            }
            if (node.partnerMan != null)
            {
                if (!bFirst && node.partnerMan.gender == Gender.MAN)
                {
                    pnt.X += (horLineLength + strWidth);
                    CalcOffset(node.partnerMan, ref pnt);
                }
            }

        }

        enum DIRC
        {
            enumRight = 0,
            enumLeft
        }



        /// <summary>
        /// 親方向のノード描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="parentNode"></param>
        /// <param name="drawPnt"></param>
        /// <param name="dirc"></param>
        void DrawNodeToParent(Graphics g, Node parentNode, Point drawPnt, DIRC dirc)
        {
            if (parentNode == null) return;
            Point pnt = drawPnt;
            if (parentNode != null)
            {
                int x = drawPnt.X + strWidth / 2;
                int y = drawPnt.Y;
                g.DrawLine(blackPen, new Point(x, y), new Point(x, y - verLineLength));

                pnt.Y -= (verLineLength + strHeight);

                Rectangle rectKan = new Rectangle(pnt.X, pnt.Y, strWidth, strHeight);


               int shugosinOrImigami = GetShugosinOrImigami(parentNode.kan);
                DrawString(g, parentNode.kan, rectKan, shugosinOrImigami);

            }

            if (parentNode.parent != null)
            {
                DIRC parentDirc;
                if (parentNode.gender == Gender.WOMAN) parentDirc = DIRC.enumRight;
                else parentDirc = DIRC.enumLeft;

                DrawNodeToParent(g, parentNode.parent, pnt, parentDirc);
            }
            if (parentNode.partnerMan != null)
            {
                DrawNodeToPartner(g, parentNode.partnerMan, pnt, dirc);
            }

        }

        /// <summary>
        /// 配偶者方向ノード描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="partnerNode"></param>
        /// <param name="drawPnt"></param>
        /// <param name="dirc"></param>
        void DrawNodeToPartner(Graphics g, Node partnerNode, Point drawPnt, DIRC dirc)
        {
            if (partnerNode == null) return;
            Point pnt = drawPnt;

            if (partnerNode != null)
            {
                int x = drawPnt.X;
                int y = drawPnt.Y + strHeight / 2;

                if (dirc == DIRC.enumLeft)
                {
                    g.DrawLine(blackPen, new Point(x, y), new Point(x - horLineLength, y));
                    pnt.X -= (horLineLength + strWidth);
                }
                else
                {
                    g.DrawLine(blackPen, new Point(x + strWidth, y), new Point(x + strWidth + horLineLength, y));
                    pnt.X += (horLineLength + strWidth);
                }

                int shugosinOrImigami = GetShugosinOrImigami(partnerNode.kan);

                Rectangle rectKan = new Rectangle(pnt.X, pnt.Y, strWidth, strHeight);
                DrawString(g, partnerNode.kan, rectKan, shugosinOrImigami);


            }
            if (partnerNode.partnerMan != null)
            {
                DrawNodeToPartner(g, partnerNode.partnerMan, pnt, dirc);
            }


            if (partnerNode.parent != null)
            {
                DrawNodeToParent(g, partnerNode.parent, pnt, dirc);
            }

            if (partnerNode.child != null)
            {
                DrawNodeToChild(g, partnerNode.child, pnt, dirc);
            }
        }
        /// <summary>
        /// 子ノード方向への描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="ChildNode"></param>
        /// <param name="drawPnt"></param>
        /// <param name="dirc"></param>
        void DrawNodeToChild(Graphics g, Node ChildNode, Point drawPnt, DIRC dirc)
        {
            if (ChildNode == null) return;

            Point pnt = drawPnt;

            if (ChildNode != null)
            {
                int x = drawPnt.X + strWidth / 2;
                int y = drawPnt.Y + strHeight;
                g.DrawLine(blackPen, new Point(x, y), new Point(x, y + verLineLength));

                pnt.Y += (verLineLength + strHeight);

                int shugosinOrImigami = GetShugosinOrImigami(ChildNode.kan);

                Rectangle rectKan = new Rectangle(pnt.X, pnt.Y, strWidth, strHeight);
                DrawString(g, ChildNode.kan, rectKan, shugosinOrImigami);

            }
            if (ChildNode.partnerMan != null)
            {
                DrawNodeToPartner(g, ChildNode.partnerMan, pnt, dirc);
            }
            if (ChildNode.partnerWoman != null)
            {
                DrawNodeToPartner(g, ChildNode.partnerWoman, pnt, dirc);
            }


            if (ChildNode.parent != null)
            {
                DrawNodeToParent(g, ChildNode.parent, pnt, dirc);
            }


        }
        /// <summary>
        /// 文字列描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="str"></param>
        /// <param name="rect"></param>
        /// <param name="shugosinOrImigami">1..守護神  2..忌神</param>
        void DrawString(Graphics g, string str, Rectangle rect, int shugosinOrImigami)
        {

            if (string.IsNullOrEmpty(str))
            {
                g.DrawString("✕", fnt, brush, rect, stringFormat);
            }
            else
            {
                switch(shugosinOrImigami)
                {
                    case 1: g.FillRectangle(Const.brusShugosin, rect); break;
                    case 2: g.FillRectangle(Const.brusImigami, rect); break;
                }

                g.DrawRectangle(blackPen, rect);
                g.DrawString(str, fnt, brush, rect, stringFormat);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kan"></param>
        /// <returns>1..守護神  2..忌神</returns>
        int GetShugosinOrImigami(string kan)
        {
            if (string.IsNullOrEmpty(kan)) return 0;

            var shugosinAttrs = person.ShugosinAttrs;
            string[] choukouShugosinKan = person.choukouShugosin;
            var imigamiAttrs = person.ImigamiAttrs;

            var tblMng = TableMng.GetTblManage();
            //干の守護神判定
            //干の属性取得
            string kanAttr = tblMng.jyukanTbl[kan].gogyou;


            //守護神判定
            if (shugosinAttrs.Count > 0)
            {
                foreach (var shugosin in shugosinAttrs)
                {
                    if (kanAttr == shugosin.gogyouAttr) return 1;
                }
            }
            else
            {
                foreach (var k in choukouShugosinKan)
                {
                    if (k == kan) return 1;
                }
            }
            //忌神判定
            foreach (var imigame in imigamiAttrs)
            {
                if (kanAttr == imigame.gogyouAttr)
                {
                    return 2;
                }
            }
            return 0;
        }
    }

}