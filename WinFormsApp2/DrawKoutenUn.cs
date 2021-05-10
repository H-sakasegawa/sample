using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp2
{
    /// <summary>
    /// 後天運 表示用クラス
    /// </summary>
    class DrawKoutenUn : IsouhouBase
    {

        public Point nenun;
        public Point taiun;
        public Point nikkansi;
        public Point gekkansi;
        public Point nenkansi;

        public int nenunCenterX;
        public int taiunCenterX;
        public int nikkansiCenterX;
        public int gekkansiCenterX;
        public int nenkansiCenterX;

        public int drawTopKan;      //干文字表示領域TOP
        public int drawTopSi;       //支文字表示領域TOP
        public int drawBottomSi;    //支文字表示領域BOTTOM
        public int rangeHeight;     //干支文字領域高さ
        public int rangeWidth;      //干支文字領域幅



        //干支文字表示領域
        Rectangle rectNenunTitle;
        Rectangle rectNenunKan;
        Rectangle rectNenunSi;

        Rectangle rectTaiunTitle;
        Rectangle rectTaiunKan;
        Rectangle rectTaiunSi;

        Rectangle rectNikansiKan;
        Rectangle rectNikansiSi;
        Rectangle rectGekkansiKan;
        Rectangle rectGekkansiSi;
        Rectangle rectNenkansiKan;
        Rectangle rectNenkansiSi;

        const int bitFlgNenun = 0x20;
        const int bitFlgTaiun = 0x10;

        const int bitFlgNiti = 0x04;
        const int bitFlgGetu = 0x02;
        const int bitFlgNen = 0x01;

        Kansi taiunKansi = null;
        Kansi nenunKansi = null;

        public DrawKoutenUn(Person person, PictureBox pictureBox, Kansi _taiunKansi, Kansi _nenunKansi) :
            base(person, pictureBox)
        {

            taiunKansi = _taiunKansi;
            nenunKansi = _nenunKansi;

            rangeHeight = GetFontHeight() * 2;
            rangeWidth = 45;

        }
        void CalcCoord()
        {
            //年運表示開始位置
            nenun.X = 5;
            //nenun.Y = 80;
            //nenun.Y = (int)(GetDrawArea().Height / 2.0 - rangeHeight*1.5); //上表示部分を少し少なめに
            nenun.Y = (idxMtx + 1) * GetLineOffsetY() + 10;

            nenunCenterX = nenun.X + rangeWidth / 2;

            taiun.X = nenun.X + rangeWidth;
            taiun.Y = nenun.Y;
            taiunCenterX = taiun.X + rangeWidth / 2;

            nikkansi.X = taiun.X + rangeWidth + 10;
            nikkansi.Y = taiun.Y;
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
            rectNenunKan = new Rectangle(nenun.X, nenun.Y, rangeWidth, rangeHeight);
            rectNenunSi = new Rectangle(nenun.X, drawTopSi, rangeWidth, rangeHeight);
            rectTaiunKan = new Rectangle(taiun.X, taiun.Y, rangeWidth, rangeHeight);
            rectTaiunSi = new Rectangle(taiun.X, drawTopSi, rangeWidth, rangeHeight);
            rectNikansiKan = new Rectangle(nikkansi.X, nikkansi.Y, rangeWidth, rangeHeight);
            rectNikansiSi = new Rectangle(nikkansi.X, drawTopSi, rangeWidth, rangeHeight);
            rectGekkansiKan = new Rectangle(gekkansi.X, gekkansi.Y, rangeWidth, rangeHeight);
            rectGekkansiSi = new Rectangle(gekkansi.X, drawTopSi, rangeWidth, rangeHeight);
            rectNenkansiKan = new Rectangle(nenkansi.X, nenkansi.Y, rangeWidth, rangeHeight);
            rectNenkansiSi = new Rectangle(nenkansi.X, drawTopSi, rangeWidth, rangeHeight);


        }


        /// <summary>
        /// 描画処理
        /// 本関数は、IsouhouBase::Draw()から呼び出されます
        /// </summary>
        /// <param name="g"></param>
        public override void DrawItem(Graphics g)
        {
            if (person == null) return;

            //陰陽(年運→大運）
            //-------------------               
            bool bInyouTNenunTaiun = person.IsInyou(nenunKansi, taiunKansi); //（年運 - 大運) の関係
            int idxInyouTNenunTaiun = SetMatrixUp(bInyouTNenunTaiun, bitFlgNenun, (bitFlgNenun | bitFlgTaiun));//日 - 月


            int bitFlgNitiGetuNen = (bitFlgNiti | bitFlgGetu | bitFlgNen);
            //陰陽(大運→＊）
            //-------------------               
            bool bInyouTaiunNiti = person.IsInyou(taiunKansi, person.nikkansi); //（大運-日) の関係
            bool bInyouTaiunGetu = person.IsInyou(taiunKansi, person.gekkansi); //（大運-月) の関係
            bool bInyouTaiunNen = person.IsInyou(taiunKansi, person.nenkansi);//（大運-年) の関係
            int idxInyouTaiunNiti = SetMatrixUp(bInyouTaiunNiti, bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgNiti));//大運 - 日
            int idxInyouTaiunGetu = SetMatrixUp(bInyouTaiunGetu, bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgGetu));//大運 - 月
            int idxInyouTaiunNen = SetMatrixUp(bInyouTaiunNen, bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgNen));//大運 - 年


            //陰陽(年運→＊）
            //-------------------               
            bool bInyouNenunNiti = person.IsInyou(nenunKansi, person.nikkansi); //（年運-日) の関係
            bool bInyouNenunGetu = person.IsInyou(nenunKansi, person.gekkansi);//（年運-月) の関係
            bool bInyouNenunNen = person.IsInyou(nenunKansi, person.nenkansi);//（年運-年) の関係
            int idxInyouNenunNiti = SetMatrixUp(bInyouNenunNiti, bitFlgNenun | bitFlgNitiGetuNen, (bitFlgNenun | bitFlgNiti));//年運 - 日
            int idxInyouNenunGetu = SetMatrixUp(bInyouNenunGetu, bitFlgNenun | bitFlgNitiGetuNen, (bitFlgNenun | bitFlgGetu));//年運 - 月
            int idxInyouNenunNen = SetMatrixUp(bInyouNenunNen, bitFlgNenun | bitFlgNitiGetuNen, (bitFlgNenun | bitFlgNen));//年運 - 年


            //干合(年運→大運）
            //-------------------               
            bool bKangouTNenunTaiun = person.IsKango(nenunKansi, taiunKansi); //（年運 - 大運) の関係
            int idxKangouTNenunTaiun = SetMatrixUp(bKangouTNenunTaiun, bitFlgNenun | bitFlgTaiun, (bitFlgNenun | bitFlgTaiun));//年運 - 大運


            //干合(大運→＊）
            //-------------------
            bool bKangouTaiunNiti = person.IsKango(taiunKansi, person.nikkansi);//（大運-日) の関係
            bool bKangouTaiunGetu = person.IsKango(taiunKansi, person.gekkansi);//（大運-月) の関係
            bool bKangouTaiunNen = person.IsKango(taiunKansi, person.nenkansi);//（大運-年) の関係
            int idxKangouTaiunNiti = SetMatrixUp(bKangouTaiunNiti, bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgNiti));//大運 - 日
            int idxKangouTaiunGetu = SetMatrixUp(bKangouTaiunGetu, bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgGetu));//大運 - 月
            int idxKangouTaiunNen = SetMatrixUp(bKangouTaiunNen, bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgNen));//大運 - 年


            //干合(年運→＊）
            //-------------------
            bool bKangouNenunNiti = person.IsKango(nenunKansi, person.nikkansi);//（年運-日) の関係
            bool bKangouNenunGetu = person.IsKango(nenunKansi, person.gekkansi);//（年運-月) の関係
            bool bKangouNenunNen = person.IsKango(nenunKansi, person.nenkansi);//（年運-年) の関係
            int idxKangouNenunNiti = SetMatrixUp(bKangouNenunNiti, bitFlgNenun | bitFlgNitiGetuNen, (bitFlgNenun | bitFlgNiti));//年運 - 日
            int idxKangouNenunGetu = SetMatrixUp(bKangouNenunGetu, bitFlgNenun | bitFlgNitiGetuNen, (bitFlgNenun | bitFlgGetu));//年運 - 月
            int idxKangouNenunNen = SetMatrixUp(bKangouNenunNen, bitFlgNenun | bitFlgNitiGetuNen, (bitFlgNenun | bitFlgNen));//年運 - 年

            //七殺(年運→大運）
            //-------------------               
            bool bNanasatuTNenunTaiun = person.IsNanasatu(nenunKansi, taiunKansi); //（年運 - 大運) の関係
            int idxNanasatuTNenunTaiun = SetMatrixUp(bNanasatuTNenunTaiun, bitFlgNenun|bitFlgTaiun, (bitFlgNenun | bitFlgTaiun));//年運 - 大運

            //七殺(大運→＊）
            //-------------------
            bool bNanasatuTaiunNiti = person.IsNanasatu(taiunKansi, person.nikkansi);//（大運-日) の関係
            bool bNanasatuTaiunGetu = person.IsNanasatu(taiunKansi, person.gekkansi);//（大運-月) の関係
            bool bNanasatuTaiunNen = person.IsNanasatu(taiunKansi, person.nenkansi);//（大運-年) の関係
            int idxNanasatuTaiunNiti = SetMatrixUp(bNanasatuTaiunNiti, bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgNiti));//大運 - 日
            int idxNanasatuTaiunGetu = SetMatrixUp(bNanasatuTaiunGetu, bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgGetu));//大運 - 月
            int idxNanasatuTaiunNen = SetMatrixUp(bNanasatuTaiunNen, bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgNen));//大運 - 年

            //七殺(年運→＊）
            //-------------------
            bool bNanasatuNenunNiti = person.IsNanasatu(nenunKansi, person.nikkansi);//（年運-日) の関係
            bool bNanasatuNenunGetu = person.IsNanasatu(nenunKansi, person.gekkansi);//（年運-月) の関係
            bool bNanasatuNenunNen = person.IsNanasatu(nenunKansi, person.nenkansi);//（年運-年) の関係
            int idxNanasatuNenunNiti = SetMatrixUp(bNanasatuNenunNiti, bitFlgNenun | bitFlgNitiGetuNen, (bitFlgNenun | bitFlgNiti));//年運 - 日
            int idxNanasatuNenunGetu = SetMatrixUp(bNanasatuNenunGetu, bitFlgNenun | bitFlgNitiGetuNen, (bitFlgNenun | bitFlgGetu));//年運 - 月
            int idxNanasatuNenunNen = SetMatrixUp(bNanasatuNenunNen, bitFlgNenun | bitFlgNitiGetuNen, (bitFlgNenun | bitFlgNen));//年運 - 年


            //干支の上部に表示する情報の段数から干支表示基準座標を計算
            CalcCoord();

            //干支表示
            DrawKansi(nenunKansi, rectNenunKan, rectNenunSi);
            DrawKansi(taiunKansi, rectTaiunKan, rectTaiunSi);

            DrawKansi(person.nikkansi, rectNikansiKan, rectNikansiSi);
            DrawKansi(person.gekkansi, rectGekkansiKan, rectGekkansiSi);
            DrawKansi(person.nenkansi, rectNenkansiKan, rectNenkansiSi);

            rectNenunTitle = new Rectangle(nenun.X, nenun.Y - GetSmallFontHeight() / 2, rangeWidth, GetSmallFontHeight());
            rectTaiunTitle = new Rectangle(taiun.X, taiun.Y - GetSmallFontHeight() / 2, rangeWidth, GetSmallFontHeight());
            DrawString(rectNenunTitle, "<年運>");
            DrawString(rectTaiunTitle, "<大運>");

            //陰陽(年運→大運）
            //-------------------               
            if (bInyouTNenunTaiun)//大運 - 年運
            {
                DrawLine(idxInyouTNenunTaiun, nenunCenterX, taiunCenterX, drawTopKan, dircUp);
                DrawString(idxInyouTNenunTaiun, nenunCenterX, taiunCenterX, drawTopKan, dircUp, strInyou);
            }


            //陰陽(大運→＊）
            //-------------------               
            if (bInyouTaiunNiti)//大運 - 日
            {
                DrawLine(idxInyouTaiunNiti, taiunCenterX, nikkansiCenterX, drawTopKan, dircUp);
                DrawString(idxInyouTaiunNiti, taiunCenterX, nikkansiCenterX, drawTopKan, dircUp, strInyou);
            }
            if (bInyouTaiunGetu)//大運 - 月
            {
                DrawLine(idxInyouTaiunGetu, taiunCenterX, gekkansiCenterX, drawTopKan, dircUp);
                DrawString(idxInyouTaiunGetu, taiunCenterX, gekkansiCenterX, drawTopKan, dircUp, strInyou);
            }
            if (bInyouTaiunNen)//大運 - 年
            {
                DrawLine(idxInyouTaiunNen, taiunCenterX, nenkansiCenterX, drawTopKan, dircUp);
                DrawString(idxInyouTaiunNen, taiunCenterX, nenkansiCenterX, drawTopKan, dircUp, strInyou);
            }

            //陰陽(年運→＊）
            //-------------------               
            if (bInyouNenunNiti)//年運 - 日
            {
                DrawLine(idxInyouNenunNiti, nenunCenterX, nikkansiCenterX, drawTopKan, dircUp);
                DrawString(idxInyouNenunNiti, nenunCenterX, nikkansiCenterX, drawTopKan, dircUp, strInyou);
            }
            if (bInyouNenunGetu)//年運 - 月
            {
                DrawLine(idxInyouNenunGetu, nenunCenterX, gekkansiCenterX, drawTopKan, dircUp);
                DrawString(idxInyouNenunGetu, nenunCenterX, gekkansiCenterX, drawTopKan, dircUp, strInyou);
            }
            if (bInyouNenunNen)//年運 - 年
            {
                DrawLine(idxInyouNenunNen, nenunCenterX, nenkansiCenterX, drawTopKan, dircUp);
                DrawString(idxInyouNenunNen, nenunCenterX, nenkansiCenterX, drawTopKan, dircUp, strInyou);
            }


            //干合(年運→大運）
            //-------------------               
            if (bKangouTNenunTaiun)//大運 - 年運
            {
                DrawLine(idxKangouTNenunTaiun, nenunCenterX, taiunCenterX, drawTopKan, dircUp);
                DrawString(idxKangouTNenunTaiun, nenunCenterX, taiunCenterX, drawTopKan, dircUp, strKangou);
            }
            //干合(大運→＊）
            //-------------------
            if (bKangouTaiunNiti)//大運 - 日
            {
                DrawLine(idxKangouTaiunNiti, taiunCenterX, nikkansiCenterX, drawTopKan, dircUp);
                DrawString(idxKangouTaiunNiti, taiunCenterX, nikkansiCenterX, drawTopKan, dircUp, strKangou);
            }
            if (bKangouTaiunGetu)//大運 - 月
            {
                DrawLine(idxKangouTaiunGetu, taiunCenterX, gekkansiCenterX, drawTopKan, dircUp);
                DrawString(idxKangouTaiunGetu, taiunCenterX, gekkansiCenterX, drawTopKan, dircUp, strKangou);
            }
            if (bKangouTaiunNen)//大運 - 年
            {
                DrawLine(idxKangouTaiunNen, taiunCenterX, nenkansiCenterX, drawTopKan, dircUp);
                DrawString(idxKangouTaiunNen, taiunCenterX, nenkansiCenterX, drawTopKan, dircUp, strKangou);
            }

            //干合(年運→＊）
            //-------------------
            if (bKangouNenunNiti)//年運 - 日
            {
                DrawLine(idxKangouNenunNiti, nenunCenterX, nikkansiCenterX, drawTopKan, dircUp);
                DrawString(idxKangouNenunNiti, nenunCenterX, nikkansiCenterX, drawTopKan, dircUp, strKangou);
            }
            if (bKangouNenunGetu)//年運 - 月
            {
                DrawLine(idxKangouNenunGetu, nenunCenterX, gekkansiCenterX, drawTopKan, dircUp);
                DrawString(idxKangouNenunGetu, nenunCenterX, gekkansiCenterX, drawTopKan, dircUp, strKangou);
            }
            if (bKangouNenunNen)//年運 - 年
            {
                DrawLine(idxKangouNenunNen, nenunCenterX, nenkansiCenterX, drawTopKan, dircUp);
                DrawString(idxKangouNenunNen, nenunCenterX, nenkansiCenterX, drawTopKan, dircUp, strKangou);
            }

            //七殺(年運→大運）
            //-------------------               
            if (bNanasatuTNenunTaiun)//大運 - 年運
            {
                DrawLine(idxNanasatuTNenunTaiun, nenunCenterX, taiunCenterX, drawTopKan, dircUp);
                DrawString(idxNanasatuTNenunTaiun, nenunCenterX, taiunCenterX, drawTopKan, dircUp, strNanasatu);
            }
            //七殺(大運→＊）
            //-------------------
            if (bNanasatuTaiunNiti)//大運 - 日
            {
                DrawLine(idxNanasatuTaiunNiti, taiunCenterX, nikkansiCenterX, drawTopKan, dircUp);
                DrawString(idxNanasatuTaiunNiti, taiunCenterX, nikkansiCenterX, drawTopKan, dircUp, strNanasatu);
            }
            if (bNanasatuTaiunGetu)//大運 - 月
            {
                DrawLine(idxNanasatuTaiunGetu, taiunCenterX, gekkansiCenterX, drawTopKan, dircUp);
                DrawString(idxNanasatuTaiunGetu, taiunCenterX, gekkansiCenterX, drawTopKan, dircUp, strNanasatu);
            }
            if (bNanasatuTaiunNen)//大運 - 年
            {
                DrawLine(idxNanasatuTaiunNen, taiunCenterX, nenkansiCenterX, drawTopKan, dircUp);
                DrawString(idxNanasatuTaiunNen, taiunCenterX, nenkansiCenterX, drawTopKan, dircUp, strNanasatu);
            }
            //七殺(年運→＊）
            //-------------------
            if (bNanasatuNenunNiti)//年運 - 日
            {
                DrawLine(idxNanasatuNenunNiti, nenunCenterX, nikkansiCenterX, drawTopKan, dircUp);
                DrawString(idxNanasatuNenunNiti, nenunCenterX, nikkansiCenterX, drawTopKan, dircUp, strNanasatu);
            }
            if (bNanasatuNenunGetu)//年運 - 月
            {
                DrawLine(idxNanasatuNenunGetu, nenunCenterX, gekkansiCenterX, drawTopKan, dircUp);
                DrawString(idxNanasatuNenunGetu, nenunCenterX, gekkansiCenterX, drawTopKan, dircUp, strNanasatu);
            }
            if (bNanasatuNenunNen)//年運 - 年
            {
                DrawLine(idxNanasatuNenunNen, nenunCenterX, nenkansiCenterX, drawTopKan, dircUp);
                DrawString(idxNanasatuNenunNen, nenunCenterX, nenkansiCenterX, drawTopKan, dircUp, strNanasatu);

            }


            //合法・散法
            //-------------------
            //GouhouSannpouResult[] gouhouSanpouTaiunNenun = person.GetGouhouSanpou(taiunKansi, nenunKansi, false, false);
            string[] gouhouSanpouTaiunNenun = person.GetGouhouSanpou(taiunKansi, nenunKansi, false, false);

            if (gouhouSanpouTaiunNenun != null && gouhouSanpouTaiunNenun.Length > 0)//大運 - 年運
            {
                int enableFlag = CheckExceptionValue(person, taiunKansi, nenunKansi, taiunKansi, nenunKansi, gouhouSanpouTaiunNenun);

                int idx = SetMatrixDown(true, bitFlgNenun, (bitFlgNenun | bitFlgTaiun));//年運 - 大運
                DrawLine(idx, nenunCenterX, taiunCenterX, drawBottomSi, dircDown);
                DrawString(idx, nenunCenterX, taiunCenterX, drawBottomSi, dircDown, gouhouSanpouTaiunNenun);

            }

            //大運 →＊
            //GouhouSannpouResult[] gouhouSanpouTaiunNiti = person.GetGouhouSanpou(taiunKansi, person.nikkansi, false, false);
            //GouhouSannpouResult[] gouhouSanpouTaiunGetu = person.GetGouhouSanpou(taiunKansi, person.gekkansi, false, false);
            //GouhouSannpouResult[] gouhouSanpouTaiunNen = person.GetGouhouSanpou(taiunKansi, person.nenkansi, false, false);
            string[] gouhouSanpouTaiunNiti = person.GetGouhouSanpou(taiunKansi, person.nikkansi, false, false);
            string[] gouhouSanpouTaiunGetu = person.GetGouhouSanpou(taiunKansi, person.gekkansi, false, false);
            string[] gouhouSanpouTaiunNen = person.GetGouhouSanpou(taiunKansi, person.nenkansi, false, false);

            if (gouhouSanpouTaiunNiti != null && gouhouSanpouTaiunNiti.Length>0)//大運 - 日
            {
                int enableFlag = CheckExceptionValue(person, taiunKansi, person.nikkansi, taiunKansi, nenunKansi, gouhouSanpouTaiunNiti);

                int idx = SetMatrixDown(true, bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgNiti));
                DrawLine(idx, taiunCenterX, nikkansiCenterX, drawBottomSi, dircDown);
                DrawString(idx, taiunCenterX, nikkansiCenterX, drawBottomSi, dircDown, gouhouSanpouTaiunNiti, enableFlag);

            }
            if (gouhouSanpouTaiunGetu != null && gouhouSanpouTaiunGetu.Length > 0)//大運 - 月
            {
                int enableFlag = CheckExceptionValue(person, taiunKansi, person.gekkansi, taiunKansi, nenunKansi, gouhouSanpouTaiunGetu);
                int idx = SetMatrixDown(true, bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgGetu));
                DrawLine(idx, taiunCenterX, gekkansiCenterX, drawBottomSi, dircDown);
                DrawString(idx, taiunCenterX, gekkansiCenterX, drawBottomSi, dircDown, gouhouSanpouTaiunGetu, enableFlag);
            }

            if (gouhouSanpouTaiunNen != null && gouhouSanpouTaiunNen.Length > 0)//大運 - 年
            {
                int enableFlag = CheckExceptionValue(person, taiunKansi, person.nenkansi, taiunKansi, nenunKansi, gouhouSanpouTaiunNen);
                int idx = SetMatrixDown(true, bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgTaiun | bitFlgNen));
                DrawLine(idx, taiunCenterX, nenkansiCenterX, drawBottomSi, dircDown);
                DrawString(idx, taiunCenterX, nenkansiCenterX, drawBottomSi, dircDown, gouhouSanpouTaiunNen, enableFlag);
            }

            //年運 →＊
            //GouhouSannpouResult[] gouhouSanpouNenunNiti = person.GetGouhouSanpou(nenunKansi, person.nikkansi, false, false);
            //GouhouSannpouResult[] gouhouSanpouNenunGetu = person.GetGouhouSanpou(nenunKansi, person.gekkansi, false, false);
            //GouhouSannpouResult[] gouhouSanpouNenunNen = person.GetGouhouSanpou(nenunKansi, person.nenkansi, false, false);
            string[] gouhouSanpouNenunNiti = person.GetGouhouSanpou(nenunKansi, person.nikkansi, false, false);
            string[] gouhouSanpouNenunGetu = person.GetGouhouSanpou(nenunKansi, person.gekkansi, false, false);
            string[] gouhouSanpouNenunNen = person.GetGouhouSanpou(nenunKansi, person.nenkansi, false, false);

            if (gouhouSanpouNenunNiti != null && gouhouSanpouNenunNiti.Length > 0)//年運 - 日
            {
                int enableFlag = CheckExceptionValue(person, nenunKansi, person.nikkansi, taiunKansi, nenunKansi, gouhouSanpouNenunNiti);
                int idx = SetMatrixDown(true, bitFlgNenun | bitFlgNitiGetuNen, (bitFlgNenun | bitFlgNiti));
                DrawLine(idx, nenunCenterX, nikkansiCenterX, drawBottomSi, dircDown);
                DrawString(idx, nenunCenterX, nikkansiCenterX, drawBottomSi, dircDown, gouhouSanpouNenunNiti, enableFlag);
            }
            if (gouhouSanpouNenunGetu != null && gouhouSanpouNenunGetu.Length > 0)//年運 - 月
            {
                int enableFlag = CheckExceptionValue(person, nenunKansi, person.gekkansi, taiunKansi, nenunKansi, gouhouSanpouNenunGetu);
                int idx = SetMatrixDown(true, bitFlgNenun | bitFlgNitiGetuNen, (bitFlgNenun | bitFlgGetu));
                DrawLine(idx, nenunCenterX, gekkansiCenterX, drawBottomSi, dircDown);
                DrawString(idx, nenunCenterX, gekkansiCenterX, drawBottomSi, dircDown, gouhouSanpouNenunGetu, enableFlag);
            }

            if (gouhouSanpouNenunNen != null && gouhouSanpouNenunNen.Length > 0)//年運 - 年
            {
                int enableFlag = CheckExceptionValue(person, nenunKansi, person.nenkansi, taiunKansi, nenunKansi, gouhouSanpouNenunNen);
                int idx = SetMatrixDown(true, bitFlgNenun | bitFlgNitiGetuNen, (bitFlgNenun | bitFlgNen));
                DrawLine(idx, nenunCenterX, nenkansiCenterX, drawBottomSi, dircDown);
                DrawString(idx, nenunCenterX, nenkansiCenterX, drawBottomSi, dircDown, gouhouSanpouNenunNen, enableFlag);
            }

        }
        /// <summary>
        /// 例外表示処理
        /// </summary>
        /// <param name="person"></param>
        /// <param name="kansi1">干支1</param>
        /// <param name="kansi2">干支2</param>
        /// <param name="taiun">大運干支</param>
        /// <param name="nenun">年運干支</param>
        /// <param name="values">表示文字列配列</param>
        /// <returns>表示文字列の配列の要素に対する有効、無効ビットフラグ
        ///         0x01 ... values[0]は有効
        ///         0x02 ... values[1]は有効
        ///         0x05 ... values[0]と[2]は有効
        ///         0xFFFF ...valuesのすべての配列文字が有効
        /// </returns>
        //int CheckExceptionValue(Person person, Kansi kansi1, Kansi kansi2, Kansi taiun, Kansi nenun, GouhouSannpouResult[] values)
        //{
        //    int retValue = 0xFFFF;
        //    if (kansi1.si == "亥" && kansi2.si == "寅" ||
        //        kansi1.si == "寅" && kansi2.si == "亥")
        //    {
        //        retValue = 0;
        //        bool bHit = false;
        //        if (taiun.si == "申" || taiun.si == "巳") bHit = true;
        //        else if (nenun.si == "申" || nenun.si == "巳") bHit = true;
        //        else if (person.IsExistStrInKansiSi(new string[] { "申", "巳" })) bHit = true;

        //        if (bHit)
        //        {
        //            int bit = 0x1;
        //            foreach (var v in values)
        //            {
        //                if (v.orgName == "破") retValue |= bit;
        //                bit <<= 1;
        //            }

        //        }
        //        else
        //        {
        //            int bit = 0x1;
        //            foreach (var v in values)
        //            {
        //                if (v.orgName == "支合") retValue |= bit;
        //                bit <<= 1;
        //            }
        //        }
        //    }
        //    else if (kansi1.si == "申" && kansi2.si == "巳" ||
        //             kansi1.si == "巳" && kansi2.si == "申")
        //    {
        //        retValue = 0;
        //        bool bHit = false;
        //        if (taiun.si == "亥" || taiun.si == "寅") bHit = true;
        //        else if (taiun.si == "亥" || taiun.si == "寅") bHit = true;
        //        else if (person.IsExistStrInKansiSi(new string[] { "亥", "寅" })) bHit = true;

        //        if (bHit)
        //        {
        //            int bit = 0x1;
        //            foreach (var v in values)
        //            {
        //                if (v.orgName == "破") retValue |= bit;
        //                if (v.orgName == "生貴刑") retValue |= bit;
        //                bit <<= 1;
        //            }
        //        }
        //        else
        //        {
        //            int bit = 0x1;
        //            foreach (var v in values)
        //            {
        //                if (v.orgName == "支合") retValue |= bit;
        //                bit <<= 1;
        //            }
        //        }
        //    }
        //    return retValue;

        //}
        int CheckExceptionValue(Person person, Kansi kansi1, Kansi kansi2, Kansi taiun, Kansi nenun, string[] values)
        {
            int retValue = 0xFFFF;
            if (kansi1.si == "亥" && kansi2.si == "寅" ||
                kansi1.si == "寅" && kansi2.si == "亥")
            {
                retValue = 0;
                bool bHit = false;
                if (taiun.si == "申" || taiun.si == "巳") bHit = true;
                else if (nenun.si == "申" || nenun.si == "巳") bHit = true;
                else if (person.IsExistStrInKansiSi(new string[] { "申", "巳" })) bHit = true;

                if (bHit)
                {
                    int bit = 0x1;
                    foreach (var v in values)
                    {
                        if (v == "破") retValue |= bit;
                        bit <<= 1;
                    }

                }
                else
                {
                    int bit = 0x1;
                    foreach (var v in values)
                    {
                        if (v == "支合") retValue |= bit;
                        bit <<= 1;
                    }
                }
            }
            else if (kansi1.si == "申" && kansi2.si == "巳" ||
                     kansi1.si == "巳" && kansi2.si == "申")
            {
                retValue = 0;
                bool bHit = false;
                if (taiun.si == "亥" || taiun.si == "寅") bHit = true;
                else if (taiun.si == "亥" || taiun.si == "寅") bHit = true;
                else if (person.IsExistStrInKansiSi(new string[] { "亥", "寅" })) bHit = true;

                if (bHit)
                {
                    int bit = 0x1;
                    foreach (var v in values)
                    {
                        if (v == "破") retValue |= bit;
                        if (v == "生貴刑") retValue |= bit;
                        bit <<= 1;
                    }
                }
                else
                {
                    int bit = 0x1;
                    foreach (var v in values)
                    {
                        if (v == "支合") retValue |= bit;
                        bit <<= 1;
                    }
                }
            }
            return retValue;

        }
    }

}
