using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace WinFormsApp2
{

    /// <summary>
    /// 陰占 表示用クラス
    /// 
    /// 子  壬癸庚  辰
    /// 丑  戌未子  巳 
    /// 
    ///     辛丁
    ///     丁乙
    ///     戊己癸
    /// 
    /// </summary>
    class DrawInsen : IsouhouBase
    {

        Insen insen;
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
        public Rectangle rectNikansiKan;
        public Rectangle rectNikansiSi;
        public Rectangle rectGekkansiKan;
        public Rectangle rectGekkansiSi;
        public Rectangle rectNenkansiKan;
        public Rectangle rectNenkansiSi;

        //日干支 初元、中元、本元
        Rectangle[] rectNikansiZogan = new Rectangle[3];

        //月干支 初元、中元、本元
        Rectangle[] rectGekkansiZogan = new Rectangle[3];

        //年干支 初元、中元、本元
        Rectangle[] rectNenkansiZogan = new Rectangle[3];

        //天中殺
        Rectangle[] rectNikkansiTenchusatu = new Rectangle[2];
        Rectangle[] rectNenkansiTenchusatu = new Rectangle[2];


        const int bitFlgNiti = 0x04;
        const int bitFlgGetu = 0x02;
        const int bitFlgNen = 0x01;


        public TColor colorNikkansiKan = new TColor();
        public TColor colorGekkansiKan = new TColor();
        public TColor colorNenkansiKan = new TColor();
        public TColor colorNikkansiSi = new TColor();
        public TColor colorGekkansiSi = new TColor();
        public TColor colorNenkansiSi = new TColor();

        public TColor colorNikkansiShogen = new TColor();
        public TColor colorNikkansiChugen = new TColor();
        public TColor colorNikkansiHongen = new TColor();

        public TColor colorGekkansiShogen = new TColor();
        public TColor colorGekkansiChugen = new TColor();
        public TColor colorGekkansiHongen = new TColor();

        public TColor colorNenkansiShogen = new TColor();
        public TColor colorNenkansiChugen = new TColor();
        public TColor colorNenkansiHongen = new TColor();

        TenchusatuColorPair[] nikkansiTenchusatuChecColor = null;
        TenchusatuColorPair[] nenkansiTenchusatuCheckColor = null;


        bool bDispZougan = true;
        bool bDispTenchusatu = true;

        /// <summary>
        /// 陰占 図描画
        /// </summary>
        /// <param name="person">人情報</param>
        /// <param name="pictureBox">描画先のピクチャーボックス</param>
        /// <param name="_bDispGogyou">true...五行反映</param>
        /// <param name="_bDispGotoku">true... 五徳反映</param>
        /// <param name="_bDispRefrectGouhou">true...五行/五徳反映時の合法反映表示</param>
        public DrawInsen(
                            Person person, 
                            PictureBox pictureBox,
                            bool _bDispZougan,
                            bool _bDispTenchusatu
            ) : base(person, pictureBox)
        {
            insen = new Insen(person);

            rangeHeight = (int)(GetFontHeight()* Const.dKansiHeightRate);
            rangeWidth = 45;

            //干支　枠無し
            bDrawRentangleKansi = false;

            bDispZougan = _bDispZougan;
            bDispTenchusatu = _bDispTenchusatu;

            //日干支天中殺の文字チェック対象ラベル
            nikkansiTenchusatuChecColor = new TenchusatuColorPair[]
            {
                  new TenchusatuColorPair( new TColor[]{ colorNenkansiKan, colorNenkansiSi }, new TColor[]{ colorNenkansiShogen, colorNenkansiChugen, colorNenkansiHongen }),
                  new TenchusatuColorPair( new TColor[]{ colorGekkansiKan, colorGekkansiSi }, new TColor[]{ colorGekkansiShogen, colorGekkansiChugen, colorGekkansiHongen })
            };
            //年干支天中殺の文字チェック対象ラベル
            nenkansiTenchusatuCheckColor = new TenchusatuColorPair[]
            {
                  new TenchusatuColorPair( new TColor[]{ colorNikkansiKan, colorNikkansiSi }, new TColor[]{ colorNikkansiShogen, colorNikkansiChugen, colorNikkansiHongen }),
            };


        }

        /// <summary>
        /// 表示座標計算
        /// </summary>
        private void CalcCoord()
        {
            if (bDispTenchusatu)
            {
                nikkansi.X = 60;
            }
            else
            {
                nikkansi.X = 10;
            }
            //nikkansi.Y = GetDrawArea().Height / 2 - rangeHeight;
            nikkansi.Y = 10;
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

            if (bDispZougan)
            {
                int zouganStartY = drawTopSi + rangeHeight + 10;
                int zouganY = zouganStartY;
                int ofsY = 30;
                //日干支 初元、中元、本元
                rectNikansiZogan[(int)NijuhachiGenso.enmGensoType.GENSO_SHOGEN] = new Rectangle(nikkansi.X, zouganY, rangeWidth, rangeHeight); zouganY += ofsY;
                rectNikansiZogan[(int)NijuhachiGenso.enmGensoType.GENSO_CHUGEN] = new Rectangle(nikkansi.X, zouganY, rangeWidth, rangeHeight); zouganY += ofsY;
                rectNikansiZogan[(int)NijuhachiGenso.enmGensoType.GENSO_HONGEN] = new Rectangle(nikkansi.X, zouganY, rangeWidth, rangeHeight);

                //月干支 初元、中元、本元
                zouganY = zouganStartY;
                rectGekkansiZogan[(int)NijuhachiGenso.enmGensoType.GENSO_SHOGEN] = new Rectangle(gekkansi.X, zouganY, rangeWidth, rangeHeight); zouganY += ofsY;
                rectGekkansiZogan[(int)NijuhachiGenso.enmGensoType.GENSO_CHUGEN] = new Rectangle(gekkansi.X, zouganY, rangeWidth, rangeHeight); zouganY += ofsY;
                rectGekkansiZogan[(int)NijuhachiGenso.enmGensoType.GENSO_HONGEN] = new Rectangle(gekkansi.X, zouganY, rangeWidth, rangeHeight);

                //年干支 初元、中元、本元
                zouganY = zouganStartY;
                rectNenkansiZogan[(int)NijuhachiGenso.enmGensoType.GENSO_SHOGEN] = new Rectangle(nenkansi.X, zouganY, rangeWidth, rangeHeight); zouganY += ofsY;
                rectNenkansiZogan[(int)NijuhachiGenso.enmGensoType.GENSO_CHUGEN] = new Rectangle(nenkansi.X, zouganY, rangeWidth, rangeHeight); zouganY += ofsY;
                rectNenkansiZogan[(int)NijuhachiGenso.enmGensoType.GENSO_HONGEN] = new Rectangle(nenkansi.X, zouganY, rangeWidth, rangeHeight);

                zouganY = nikkansi.Y + 5;
                rectNikkansiTenchusatu[0] = new Rectangle(nikkansi.X - rangeWidth - 10, zouganY, rangeWidth, rangeHeight); zouganY += ofsY;
                rectNikkansiTenchusatu[1] = new Rectangle(nikkansi.X - rangeWidth - 10, zouganY, rangeWidth, rangeHeight);

                zouganY = nikkansi.Y + 5;
                rectNenkansiTenchusatu[0] = new Rectangle(nenkansi.X + rangeWidth + 10, zouganY, rangeWidth, rangeHeight); zouganY += ofsY;
                rectNenkansiTenchusatu[1] = new Rectangle(nenkansi.X + rangeWidth + 10, zouganY, rangeWidth, rangeHeight);
            }
        }



        /// <summary>
        /// 描画処理
        /// 本関数は、IsouhouBase::Draw()から呼び出されます
        /// </summary>
        /// <param name="g"></param>
        protected override void DrawItem(Graphics g)
        {
            if (person == null) return;



            //干支の上部に表示する情報の段数から干支表示基準座標を計算
            CalcCoord();

            if (bDispTenchusatu)
            {
                //------------------
                //天中殺 カラー設定
                //------------------
                SetTenchusatuColor(person);
            }



            //干支表示
            DrawInsenKansi(insen.nikkansi, rectNikansiKan, rectNikansiSi, colorNikkansiKan.color);
            DrawInsenKansi(insen.gekkansi, rectGekkansiKan, rectGekkansiSi, colorGekkansiKan.color);
            DrawInsenKansi(insen.nenkansi, rectNenkansiKan, rectNenkansiSi,colorNenkansiKan.color);

            if (bDispZougan)
            {

                foreach (var item in Enum.GetValues(typeof(NijuhachiGenso.enmGensoType)))//初元、中元、本元
                {
                    int idx = (int)item;
                    bool bBold = insen.nikkansiHongen[idx].bJudaiShuseiGenso;
                    DrawZouganItem(g, insen.nikkansiHongen[idx].name, rectNikansiZogan[idx], colorNikkansiHongen.color, bBold);

                    bBold = insen.gekkansiHongen[idx].bJudaiShuseiGenso;
                    DrawZouganItem(g, insen.gekkansiHongen[idx].name, rectGekkansiZogan[idx], colorGekkansiHongen.color, bBold, false);

                    bBold = insen.nenkansiHongen[idx].bJudaiShuseiGenso;
                    DrawZouganItem(g, insen.nenkansiHongen[idx].name, rectNenkansiZogan[idx], colorNenkansiHongen.color, bBold);
                }
            }

            Kansi Nikkansi = person.nikkansi;
            Kansi Nenkansi = person.nenkansi;

            if (bDispTenchusatu)
            {
                //------------------
                //天中殺
                //------------------
                string[] NikkansiTenchusatu = Nikkansi.tenchusatu.ToArray();
                string[] NenkansiTenchusatu = Nenkansi.tenchusatu.ToArray();
                for (int i = 0; i < 2; i++)
                {


                    DrawTenchusatuItem(g, NikkansiTenchusatu[i], rectNikkansiTenchusatu[i], Color.Black, false);
                    DrawTenchusatuItem(g, NenkansiTenchusatu[i], rectNenkansiTenchusatu[i], Color.Black, false);
                }

            }


        }

        private void DrawZouganItem(Graphics g, string genso, Rectangle rect, Color color, bool bBold, bool bShugosin=true)
        {
            var fntZougan = fnt;
            if (bBold)
            {
                fntZougan = fntBold;
            }
            //干の守護神判定
            if (!string.IsNullOrEmpty(genso) && bShugosin)
            {
                if (IsShugosin(genso)) g.FillRectangle(Const.brusShugosin, rect);
                
                if (IsImigami(genso)) g.FillRectangle(Const.brusImigami, rect);
            }

            var brush = new SolidBrush(color);
            g.DrawString(genso, fntZougan, brush, rect, stringFormat);

        }
        private void DrawTenchusatuItem(Graphics g, string genso, Rectangle rect, Color color, bool bBold)
        {
            var fntZougan = fnt;
            if (bBold)
            {
                fntZougan = fntBold;
            }
            var brush = new SolidBrush(color);
            g.DrawString(genso, fntZougan, brush, rect, stringFormat);

        }


        public void DrawArrowLine(Graphics g, Rectangle rectStart, Rectangle rectEnd)
        {
            Point start = new Point();
            Point end = new Point();

            start.X = rectStart.X + rectStart.Width / 2;
            start.Y = rectStart.Y + rectStart.Height-10;

            end.X = rectEnd.X + rectEnd.Width / 2;
            end.Y = rectEnd.Y+5;

            DrawArrowLine(g, start, end);
        }



        private void DrawArrowLine(Graphics g, Point start, Point end)
        {
            Pen pen = new Pen(Color.Blue, 1);

            pen.CustomEndCap = new AdjustableArrowCap(4, 4);

            //pen.StartCap = LineCap.Round;
            //pen.EndCap = LineCap.ArrowAnchor;

            g.DrawLine(pen, start, end);

        }

        /// <summary>
        /// 天中殺 カラー設定
        /// </summary>
        private void SetTenchusatuColor(Person person)
        {


            Kansi Nikkansi = person.nikkansi;
            Kansi Nenkansi = person.nenkansi;

            //色を初期化
            for (int ary = 0; ary < nikkansiTenchusatuChecColor.Length; ary++)
            {
                nikkansiTenchusatuChecColor[ary].SetColor(Color.Black);
            }
            for (int ary = 0; ary < nenkansiTenchusatuCheckColor.Length; ary++)
            {
                nenkansiTenchusatuCheckColor[ary].SetColor(Color.Black);
            }

            string[] NikkansiTenchusatu = Nikkansi.tenchusatu.ToArray();
            string[] NenkansiTenchusatu = Nenkansi.tenchusatu.ToArray();


            //日干支天中殺の文字が月干支と年干支に含まれていたら赤色設定
            for (int i = 0; i < NikkansiTenchusatu.Length; i++)
            {
                //年干支に含まれているか？
                if (person.nenkansi.IsExist( NikkansiTenchusatu[i] ))
                {
                    nikkansiTenchusatuChecColor[0].SetColor(Color.Red);
                }
                //月干支に含まれているか？
                if (person.gekkansi.IsExist(NikkansiTenchusatu[i] ))
                {
                    nikkansiTenchusatuChecColor[1].SetColor(Color.Red);
                }
            }
            //年干支天中殺の文字が日干支に含まれていたら赤色設定
            for (int i = 0; i < NenkansiTenchusatu.Length; i++)
            {
                //日干支に含まれているか？
                if (person.nikkansi.IsExist(NenkansiTenchusatu[i]))
                {
                    nenkansiTenchusatuCheckColor[0].SetColor(Color.Red);
                }
            }
            ////陰占ラベルの色を陽占のラベルに反映
            //foreach (var item in dicYosenCombineInsenLabel)
            //{
            //    item.Key.ForeColor = item.Value.ForeColor;
            //}
        }
    }


}
