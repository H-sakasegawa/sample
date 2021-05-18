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
        //干支表示座標
        public Point getuun;
        public Point nenun;
        public Point taiun;
        public Point nikkansi;
        public Point gekkansi;
        public Point nenkansi;
        //干支エリアの中心X座標
        public int getuunCenterX;
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
        Rectangle rectGetuunTitle;
        Rectangle rectGetuunKan;
        Rectangle rectGetuunSi;

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

        const int bitFlgGetuun = 0x40;
        const int bitFlgNenun = 0x20;
        const int bitFlgTaiun = 0x10;

        const int bitFlgNiti = 0x04;
        const int bitFlgGetu = 0x02;
        const int bitFlgNen = 0x01;

        Kansi taiunKansi = null;
        Kansi nenunKansi = null;
        Kansi getuunKansi = null;

        bool bDispGetuun = false; //true..月運表示

        public DrawKoutenUn(Person person, PictureBox pictureBox, Kansi _taiunKansi, Kansi _nenunKansi, Kansi _getuunKansi, bool _bDispGetuun) :
            base(person, pictureBox)
        {

            taiunKansi = _taiunKansi;
            nenunKansi = _nenunKansi;
            getuunKansi = _getuunKansi;

            rangeHeight = GetFontHeight() * 2;
            rangeWidth = 45;
            bDispGetuun = _bDispGetuun;

        }
        void CalcCoord()
        {
            int ofsX = 5;
            int ofsY = (idxMtx + 1) * GetLineOffsetY() + 10;
            if (bDispGetuun)
            {
                //月運表示開始位置
                getuun.X = ofsX;
                getuun.Y = ofsY;
                getuunCenterX = nenun.X + rangeWidth / 2;
                ofsX += rangeWidth;
            }

            //年運表示開始位置
            nenun.X = ofsX;
            nenun.Y = ofsY;
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
            rectGetuunKan = new Rectangle(getuun.X, getuun.Y, rangeWidth, rangeHeight);
            rectGetuunSi = new Rectangle(getuun.X, drawTopSi, rangeWidth, rangeHeight);
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

            int bitFlgNitiGetuNen = (bitFlgNiti | bitFlgGetu | bitFlgNen);
            int chkBitFlg = 0;

            //陰陽(年運→大運、月運→年運、月運→大運）
            //-------------------               
            bool bInyouNenunTaiun = person.IsInyou(nenunKansi, taiunKansi); //（年運 - 大運) の関係
            bool bInyouGetuunNenun = person.IsInyou(getuunKansi, nenunKansi); //（月運 - 年運) の関係
            bool bInyouGetuunTaiun = person.IsInyou(getuunKansi, taiunKansi); //（月運 - 大運) の関係
            int idxInyouNenunTaiun = SetMatrixUp(bInyouNenunTaiun, bitFlgNenun, (bitFlgNenun | bitFlgTaiun));//年運 - 大運
            int idxInyouGetuunNenun = -1;
            int idxInyouGetuunTaiun = -1;
            if (bDispGetuun)
            {
                idxInyouGetuunNenun = SetMatrixUp(bInyouGetuunNenun, bitFlgGetuun, (bitFlgGetuun | bitFlgNenun));//月運 - 年運
                idxInyouGetuunTaiun = SetMatrixUp(bInyouGetuunTaiun, bitFlgGetuun | bitFlgNenun, (bitFlgGetuun | bitFlgTaiun));//月運 - 大運
            }

            //陰陽(大運→＊）
            //-------------------
            chkBitFlg = bitFlgTaiun | bitFlgNitiGetuNen;
            bool bInyouTaiunNiti = person.IsInyou(taiunKansi, person.nikkansi); //（大運-日) の関係
            bool bInyouTaiunGetu = person.IsInyou(taiunKansi, person.gekkansi); //（大運-月) の関係
            bool bInyouTaiunNen = person.IsInyou(taiunKansi, person.nenkansi);//（大運-年) の関係
            int idxInyouTaiunNiti = SetMatrixUp(bInyouTaiunNiti, chkBitFlg, (bitFlgTaiun | bitFlgNiti));//大運 - 日
            int idxInyouTaiunGetu = SetMatrixUp(bInyouTaiunGetu, chkBitFlg, (bitFlgTaiun | bitFlgGetu));//大運 - 月
            int idxInyouTaiunNen = SetMatrixUp(bInyouTaiunNen, chkBitFlg, (bitFlgTaiun | bitFlgNen));//大運 - 年


            //陰陽(年運→＊）
            //-------------------               
            chkBitFlg = bitFlgNenun | bitFlgTaiun | bitFlgNitiGetuNen;
            bool bInyouNenunNiti = person.IsInyou(nenunKansi, person.nikkansi); //（年運-日) の関係
            bool bInyouNenunGetu = person.IsInyou(nenunKansi, person.gekkansi);//（年運-月) の関係
            bool bInyouNenunNen = person.IsInyou(nenunKansi, person.nenkansi);//（年運-年) の関係
            int idxInyouNenunNiti = SetMatrixUp(bInyouNenunNiti, chkBitFlg, (bitFlgNenun | bitFlgNiti));//年運 - 日
            int idxInyouNenunGetu = SetMatrixUp(bInyouNenunGetu, chkBitFlg, (bitFlgNenun | bitFlgGetu));//年運 - 月
            int idxInyouNenunNen = SetMatrixUp(bInyouNenunNen, chkBitFlg, (bitFlgNenun | bitFlgNen));//年運 - 年


            //陰陽(月運→＊）
            //-------------------               
            chkBitFlg = bitFlgGetuun | bitFlgNenun | bitFlgTaiun | bitFlgNitiGetuNen;
            bool bInyouGetuunNiti = person.IsInyou(getuunKansi, person.nikkansi); //（月運-日) の関係
            bool bInyouGetuunGetu = person.IsInyou(getuunKansi, person.gekkansi);//（月運-月) の関係
            bool bInyouGetuunNen = person.IsInyou(getuunKansi, person.nenkansi);//（月運-年) の関係
            int idxInyouGetuunNiti = -1;
            int idxInyouGetuunGetu = -1;
            int idxInyouGetuunNen  = -1;
            if (bDispGetuun)
            {
                idxInyouGetuunNiti = SetMatrixUp(bInyouGetuunNiti, chkBitFlg, (bitFlgGetuun | bitFlgNiti));//月運 - 日
                idxInyouGetuunGetu = SetMatrixUp(bInyouGetuunGetu, chkBitFlg, (bitFlgGetuun | bitFlgGetu));//月運 - 月
                idxInyouGetuunNen = SetMatrixUp(bInyouGetuunNen, chkBitFlg, (bitFlgGetuun | bitFlgNen));//月運 - 年
            }

            //干合(年運→大運、月運→年運、月運→大運運）
            //-------------------               
            bool bKangouNenunTaiun = person.IsKango(nenunKansi, taiunKansi); //（年運 - 大運) の関係
            bool bKangouGetuunNenun = person.IsKango(getuunKansi, nenunKansi); //（月運 - 年運) の関係
            bool bKangouGetuunTaiun = person.IsKango(getuunKansi, taiunKansi); //（月運 - 大運) の関係
            int idxKangouNenunTaiun = SetMatrixUp(bKangouNenunTaiun, bitFlgNenun | bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgNenun | bitFlgTaiun));//年運 - 大運
            int idxKangouGetuunNenun = -1;
            int idxKangouGetuunTaiun = -1;
            if (bDispGetuun)
            {
                idxKangouGetuunNenun = SetMatrixUp(bKangouGetuunNenun, bitFlgNenun | bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgGetuun | bitFlgNenun));//月運 - 年運
                idxKangouGetuunTaiun = SetMatrixUp(bKangouGetuunTaiun, bitFlgGetuun | bitFlgNenun | bitFlgTaiun | bitFlgNitiGetuNen, (bitFlgGetuun | bitFlgTaiun));//月運 - 大運
            }

            //干合(大運→＊）
            //-------------------
            chkBitFlg = bitFlgTaiun | bitFlgNitiGetuNen;
            bool bKangouTaiunNiti = person.IsKango(taiunKansi, person.nikkansi);//（大運-日) の関係
            bool bKangouTaiunGetu = person.IsKango(taiunKansi, person.gekkansi);//（大運-月) の関係
            bool bKangouTaiunNen = person.IsKango(taiunKansi, person.nenkansi);//（大運-年) の関係
            int idxKangouTaiunNiti = SetMatrixUp(bKangouTaiunNiti, chkBitFlg, (bitFlgTaiun | bitFlgNiti));//大運 - 日
            int idxKangouTaiunGetu = SetMatrixUp(bKangouTaiunGetu, chkBitFlg, (bitFlgTaiun | bitFlgGetu));//大運 - 月
            int idxKangouTaiunNen = SetMatrixUp(bKangouTaiunNen, chkBitFlg, (bitFlgTaiun | bitFlgNen));//大運 - 年


            //干合(年運→＊）
            //-------------------
            chkBitFlg = bitFlgNenun | bitFlgTaiun | bitFlgNitiGetuNen;
            bool bKangouNenunNiti = person.IsKango(nenunKansi, person.nikkansi);//（年運-日) の関係
            bool bKangouNenunGetu = person.IsKango(nenunKansi, person.gekkansi);//（年運-月) の関係
            bool bKangouNenunNen = person.IsKango(nenunKansi, person.nenkansi);//（年運-年) の関係
            int idxKangouNenunNiti = SetMatrixUp(bKangouNenunNiti, chkBitFlg, (bitFlgNenun | bitFlgNiti));//年運 - 日
            int idxKangouNenunGetu = SetMatrixUp(bKangouNenunGetu, chkBitFlg, (bitFlgNenun | bitFlgGetu));//年運 - 月
            int idxKangouNenunNen = SetMatrixUp(bKangouNenunNen, chkBitFlg, (bitFlgNenun | bitFlgNen));//年運 - 年

            //干合(月運→＊）
            //-------------------
            chkBitFlg = bitFlgGetuun | bitFlgNitiGetuNen;
            bool bKangouGetuunNiti = person.IsKango(getuunKansi, person.nikkansi);//（月運-日) の関係
            bool bKangouGetuunGetu = person.IsKango(getuunKansi, person.gekkansi);//（月運-月) の関係
            bool bKangouGetuunNen = person.IsKango(getuunKansi, person.nenkansi);//（月運-年) の関係
            int idxKangouGetuunNiti = -1;
            int idxKangouGetuunGetu = -1;
            int idxKangouGetuunNen = -1;
            if (bDispGetuun)
            {
                idxKangouGetuunNiti = SetMatrixUp(bKangouGetuunNiti, chkBitFlg, (bitFlgGetuun | bitFlgNiti));//月運 - 日
                idxKangouGetuunGetu = SetMatrixUp(bKangouGetuunGetu, chkBitFlg, (bitFlgGetuun | bitFlgGetu));//月運 - 月
                idxKangouGetuunNen = SetMatrixUp(bKangouGetuunNen, chkBitFlg, (bitFlgGetuun | bitFlgNen));//月運 - 年
            }
            //七殺(年運→大運、月運→年運、月運→大運）
            //-------------------               
            chkBitFlg = bitFlgNenun | bitFlgTaiun | bitFlgNitiGetuNen;
            bool bNanasatuNenunTaiun = person.IsNanasatu(nenunKansi, taiunKansi); //（年運 - 大運) の関係
            bool bNanasatuGetuunNenun = person.IsNanasatu(getuunKansi, nenunKansi); //（月運 - 年運) の関係
            bool bNanasatuGetuunTaiun = person.IsNanasatu(getuunKansi, taiunKansi); //（月運 - 大運) の関係
            int idxNanasatuNenunTaiun = SetMatrixUp(bNanasatuNenunTaiun, chkBitFlg, (bitFlgNenun | bitFlgTaiun));//年運 - 大運
            int idxNanasatuGetuunNenun = -1;
            int idxNanasatuGetuunTaiun = -1;
            if (bDispGetuun)
            {
                idxNanasatuGetuunNenun = SetMatrixUp(bNanasatuGetuunNenun, chkBitFlg, (bitFlgGetuun | bitFlgTaiun));//月運 - 年運
                idxNanasatuGetuunTaiun = SetMatrixUp(bNanasatuGetuunTaiun, chkBitFlg, (bitFlgGetuun | bitFlgTaiun));//月運 - 大運
            }
            //七殺(大運→＊）
            //-------------------
            chkBitFlg = bitFlgTaiun | bitFlgNitiGetuNen;
            bool bNanasatuTaiunNiti = person.IsNanasatu(taiunKansi, person.nikkansi);//（大運-日) の関係
            bool bNanasatuTaiunGetu = person.IsNanasatu(taiunKansi, person.gekkansi);//（大運-月) の関係
            bool bNanasatuTaiunNen = person.IsNanasatu(taiunKansi, person.nenkansi);//（大運-年) の関係
            int idxNanasatuTaiunNiti = SetMatrixUp(bNanasatuTaiunNiti, chkBitFlg, (bitFlgTaiun | bitFlgNiti));//大運 - 日
            int idxNanasatuTaiunGetu = SetMatrixUp(bNanasatuTaiunGetu, chkBitFlg, (bitFlgTaiun | bitFlgGetu));//大運 - 月
            int idxNanasatuTaiunNen = SetMatrixUp(bNanasatuTaiunNen, chkBitFlg, (bitFlgTaiun | bitFlgNen));//大運 - 年

            //七殺(年運→＊）
            //-------------------
            chkBitFlg = bitFlgNenun | bitFlgTaiun | bitFlgNitiGetuNen;
            bool bNanasatuNenunNiti = person.IsNanasatu(nenunKansi, person.nikkansi);//（年運-日) の関係
            bool bNanasatuNenunGetu = person.IsNanasatu(nenunKansi, person.gekkansi);//（年運-月) の関係
            bool bNanasatuNenunNen = person.IsNanasatu(nenunKansi, person.nenkansi);//（年運-年) の関係
            int idxNanasatuNenunNiti = SetMatrixUp(bNanasatuNenunNiti, chkBitFlg, (bitFlgNenun | bitFlgNiti));//年運 - 日
            int idxNanasatuNenunGetu = SetMatrixUp(bNanasatuNenunGetu, chkBitFlg, (bitFlgNenun | bitFlgGetu));//年運 - 月
            int idxNanasatuNenunNen = SetMatrixUp(bNanasatuNenunNen, chkBitFlg, (bitFlgNenun | bitFlgNen));//年運 - 年

            //七殺(月運→＊）
            //-------------------
            chkBitFlg = bitFlgGetuun | bitFlgNenun | bitFlgTaiun | bitFlgNitiGetuNen;
            bool bNanasatuGetuunNiti = person.IsNanasatu(getuunKansi, person.nikkansi);//（月運-日) の関係
            bool bNanasatuGetuunGetu = person.IsNanasatu(getuunKansi, person.gekkansi);//（月運-月) の関係
            bool bNanasatuGetuunNen = person.IsNanasatu(getuunKansi, person.nenkansi);//（月運-年) の関係
            int idxNanasatuGetuunNiti = -1;
            int idxNanasatuGetuunGetu = -1;
            int idxNanasatuGetuunNen = -1;
            if (bDispGetuun)
            {
                idxNanasatuGetuunNiti = SetMatrixUp(bNanasatuGetuunNiti, chkBitFlg, (bitFlgGetuun | bitFlgNiti));//月運 - 日
                idxNanasatuGetuunGetu = SetMatrixUp(bNanasatuGetuunGetu, chkBitFlg, (bitFlgGetuun | bitFlgGetu));//月運 - 月
                idxNanasatuGetuunNen = SetMatrixUp(bNanasatuGetuunNen, chkBitFlg, (bitFlgGetuun | bitFlgNen));//月運 - 年
            }
            //干支の上部に表示する情報の段数から干支表示基準座標を計算
            CalcCoord();

            //干支表示
            rectGetuunTitle = new Rectangle(getuun.X, getuun.Y - GetSmallFontHeight() / 2, rangeWidth, GetSmallFontHeight());
            rectNenunTitle = new Rectangle(nenun.X, nenun.Y - GetSmallFontHeight() / 2, rangeWidth, GetSmallFontHeight());
            rectTaiunTitle = new Rectangle(taiun.X, taiun.Y - GetSmallFontHeight() / 2, rangeWidth, GetSmallFontHeight());

            if (bDispGetuun)
            {
                DrawKansi(getuunKansi, rectGetuunKan, rectGetuunSi);　//月運干支
                DrawString(rectGetuunTitle, "<月運>");
            }
            DrawKansi(nenunKansi, rectNenunKan, rectNenunSi);//年運干支
            DrawKansi(taiunKansi, rectTaiunKan, rectTaiunSi);//大運干支
            DrawString(rectNenunTitle, "<年運>");
            DrawString(rectTaiunTitle, "<大運>");

            DrawKansi(person.nikkansi, rectNikansiKan, rectNikansiSi);//日干支
            DrawKansi(person.gekkansi, rectGekkansiKan, rectGekkansiSi);//月干支
            DrawKansi(person.nenkansi, rectNenkansiKan, rectNenkansiSi);//年干支


            //陰陽(年運→大運）
            //-------------------               
            if (bInyouNenunTaiun)//年運→大運
            {
                DrawLine(idxInyouNenunTaiun, nenunCenterX, taiunCenterX, drawTopKan, dircUp);
                DrawString(idxInyouNenunTaiun, nenunCenterX, taiunCenterX, drawTopKan, dircUp, strInyou);
            }
            if (bDispGetuun)
            {
                //陰陽(月運→年運）
                //-------------------               
                if (bInyouGetuunNenun)//月運 - 年運
                {
                    DrawLine(idxInyouGetuunNenun, getuunCenterX, nenunCenterX, drawTopKan, dircUp);
                    DrawString(idxInyouGetuunNenun, getuunCenterX, nenunCenterX, drawTopKan, dircUp, strInyou);
                }
                //陰陽(月運→大運）
                //-------------------               
                if (bInyouGetuunTaiun)//月運 - 年運
                {
                    DrawLine(idxInyouGetuunTaiun, getuunCenterX, taiunCenterX, drawTopKan, dircUp);
                    DrawString(idxInyouGetuunTaiun, getuunCenterX, taiunCenterX, drawTopKan, dircUp, strInyou);
                }

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

            if (bDispGetuun)
            {
                //陰陽(月運→＊）
                //-------------------               
                if (bInyouGetuunNiti)//月運 - 日
                {
                    DrawLine(idxInyouGetuunNiti, getuunCenterX, nikkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxInyouGetuunNiti, getuunCenterX, nikkansiCenterX, drawTopKan, dircUp, strInyou);
                }
                if (bInyouGetuunGetu)//月運 - 月
                {
                    DrawLine(idxInyouGetuunGetu, getuunCenterX, gekkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxInyouGetuunGetu, getuunCenterX, gekkansiCenterX, drawTopKan, dircUp, strInyou);
                }
                if (bInyouGetuunNen)//月運 - 年
                {
                    DrawLine(idxInyouGetuunNen, getuunCenterX, nenkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxInyouGetuunNen, getuunCenterX, nenkansiCenterX, drawTopKan, dircUp, strInyou);
                }
            }
            //干合(年運→大運）
            //-------------------               
            if (bKangouNenunTaiun)//年運 - 大運
            {
                DrawLine(idxKangouNenunTaiun, nenunCenterX, taiunCenterX, drawTopKan, dircUp);
                DrawString(idxKangouNenunTaiun, nenunCenterX, taiunCenterX, drawTopKan, dircUp, strKangou);
            }
            if (bDispGetuun)
            {
                if (bKangouGetuunNenun)//月運 - 年運
                {
                    DrawLine(idxKangouGetuunNenun, getuunCenterX, nenunCenterX, drawTopKan, dircUp);
                    DrawString(idxKangouGetuunNenun, getuunCenterX, nenunCenterX, drawTopKan, dircUp, strKangou);
                }
                if (bKangouGetuunTaiun)//月運 - 大運
                {
                    DrawLine(idxKangouGetuunTaiun, getuunCenterX, taiunCenterX, drawTopKan, dircUp);
                    DrawString(idxKangouGetuunTaiun, getuunCenterX, taiunCenterX, drawTopKan, dircUp, strKangou);
                }
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

            if (bDispGetuun)
            {
                //干合(月運→＊）
                //-------------------
                if (bKangouGetuunNiti)//年運 - 日
                {
                    DrawLine(idxKangouGetuunNiti, getuunCenterX, nikkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxKangouGetuunNiti, getuunCenterX, nikkansiCenterX, drawTopKan, dircUp, strKangou);
                }
                if (bKangouGetuunGetu)//年運 - 月
                {
                    DrawLine(idxKangouGetuunGetu, getuunCenterX, gekkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxKangouGetuunGetu, getuunCenterX, gekkansiCenterX, drawTopKan, dircUp, strKangou);
                }
                if (bKangouGetuunNen)//年運 - 年
                {
                    DrawLine(idxKangouGetuunNen, getuunCenterX, nenkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxKangouGetuunNen, getuunCenterX, nenkansiCenterX, drawTopKan, dircUp, strKangou);
                }
            }

            //七殺(年運→大運）
            //-------------------               
            if (bNanasatuNenunTaiun)//年運 - 大運
            {
                DrawLine(idxNanasatuNenunTaiun, nenunCenterX, taiunCenterX, drawTopKan, dircUp);
                DrawString(idxNanasatuNenunTaiun, nenunCenterX, taiunCenterX, drawTopKan, dircUp, strNanasatu);
            }
            if (bDispGetuun)
            {
                if (bNanasatuGetuunNenun)//月運 - 年運
                {
                    DrawLine(idxNanasatuGetuunNenun, getuunCenterX, nenunCenterX, drawTopKan, dircUp);
                    DrawString(idxNanasatuGetuunNenun, getuunCenterX, nenunCenterX, drawTopKan, dircUp, strNanasatu);
                }
                if (bNanasatuGetuunTaiun)//月運 - 大運
                {
                    DrawLine(idxNanasatuGetuunTaiun, getuunCenterX, taiunCenterX, drawTopKan, dircUp);
                    DrawString(idxNanasatuGetuunTaiun, getuunCenterX, taiunCenterX, drawTopKan, dircUp, strNanasatu);
                }
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
            if (bDispGetuun)
            {
                //七殺(月運→＊）
                //-------------------
                if (bNanasatuGetuunNiti)//月運 - 日
                {
                    DrawLine(idxNanasatuGetuunNiti, getuunCenterX, nikkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxNanasatuGetuunNiti, getuunCenterX, nikkansiCenterX, drawTopKan, dircUp, strNanasatu);
                }
                if (bNanasatuGetuunGetu)//月運 - 月
                {
                    DrawLine(idxNanasatuGetuunGetu, getuunCenterX, gekkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxNanasatuGetuunGetu, getuunCenterX, gekkansiCenterX, drawTopKan, dircUp, strNanasatu);
                }
                if (bNanasatuGetuunNen)//月運 - 年
                {
                    DrawLine(idxNanasatuGetuunNen, getuunCenterX, nenkansiCenterX, drawTopKan, dircUp);
                    DrawString(idxNanasatuGetuunNen, getuunCenterX, nenkansiCenterX, drawTopKan, dircUp, strNanasatu);

                }
            }


            //合法・散法,干合
            //-------------------
            GouhouSannpouResult[] gouhouSanpouNenunTaiun = person.GetGouhouSanpouEx(taiunKansi, nenunKansi, taiunKansi, nenunKansi);
            GouhouSannpouResult[] gouhouSanpouGetuunNenun = person.GetGouhouSanpouEx(nenunKansi,getuunKansi, nenunKansi, getuunKansi);
            GouhouSannpouResult[] gouhouSanpouGetuunTaiun = person.GetGouhouSanpouEx(taiunKansi, getuunKansi, taiunKansi, getuunKansi);

            if (gouhouSanpouNenunTaiun != null && gouhouSanpouNenunTaiun.Length > 0)//年運 - 大運
            {
                int idx = SetMatrixDown(true, bitFlgNenun| bitFlgTaiun, (bitFlgNenun | bitFlgTaiun));
                DrawLine(idx, nenunCenterX, taiunCenterX, drawBottomSi, dircDown);
                DrawString(idx, nenunCenterX, taiunCenterX, drawBottomSi, dircDown, gouhouSanpouNenunTaiun);

            }

            if (bDispGetuun)
            {
                if (gouhouSanpouGetuunNenun != null && gouhouSanpouGetuunNenun.Length > 0)//月運 - 年運
                {
                    int idx = SetMatrixDown(true, bitFlgGetuun | bitFlgNenun, (bitFlgGetuun | bitFlgNenun));
                    DrawLine(idx, getuunCenterX,nenunCenterX, drawBottomSi, dircDown);
                    DrawString(idx, getuunCenterX, nenunCenterX, drawBottomSi, dircDown, gouhouSanpouGetuunNenun);

                }

                if (gouhouSanpouGetuunTaiun != null && gouhouSanpouGetuunTaiun.Length > 0)//月運 - 大運
                {
                    int idx = SetMatrixDown(true, bitFlgGetuun| bitFlgNenun | bitFlgTaiun, (bitFlgGetuun | bitFlgTaiun));
                    DrawLine(idx, getuunCenterX, taiunCenterX, drawBottomSi, dircDown);
                    DrawString(idx, getuunCenterX, taiunCenterX, drawBottomSi, dircDown, gouhouSanpouGetuunTaiun);

                }
            }
            //大運 →＊
            GouhouSannpouResult[] gouhouSanpouTaiunNiti = person.GetGouhouSanpouEx(taiunKansi, person.nikkansi, taiunKansi, nenunKansi);
            GouhouSannpouResult[] gouhouSanpouTaiunGetu = person.GetGouhouSanpouEx(taiunKansi, person.gekkansi, taiunKansi, nenunKansi);
            GouhouSannpouResult[] gouhouSanpouTaiunNen = person.GetGouhouSanpouEx(taiunKansi, person.nenkansi, taiunKansi, nenunKansi);
            chkBitFlg = bitFlgTaiun | bitFlgNitiGetuNen;

            if (gouhouSanpouTaiunNiti != null && gouhouSanpouTaiunNiti.Length>0)//大運 - 日
            {
                int idx = SetMatrixDown(true, chkBitFlg, (bitFlgTaiun | bitFlgNiti));
                DrawLine(idx, taiunCenterX, nikkansiCenterX, drawBottomSi, dircDown);
                DrawString(idx, taiunCenterX, nikkansiCenterX, drawBottomSi, dircDown, gouhouSanpouTaiunNiti);

            }
            if (gouhouSanpouTaiunGetu != null && gouhouSanpouTaiunGetu.Length > 0)//大運 - 月
            {
                int idx = SetMatrixDown(true, chkBitFlg, (bitFlgTaiun | bitFlgGetu));
                DrawLine(idx, taiunCenterX, gekkansiCenterX, drawBottomSi, dircDown);
                DrawString(idx, taiunCenterX, gekkansiCenterX, drawBottomSi, dircDown, gouhouSanpouTaiunGetu);
            }

            if (gouhouSanpouTaiunNen != null && gouhouSanpouTaiunNen.Length > 0)//大運 - 年
            {
                int idx = SetMatrixDown(true, chkBitFlg, (bitFlgTaiun | bitFlgNen));
                DrawLine(idx, taiunCenterX, nenkansiCenterX, drawBottomSi, dircDown);
                DrawString(idx, taiunCenterX, nenkansiCenterX, drawBottomSi, dircDown, gouhouSanpouTaiunNen);
            }

            //年運 →＊
            GouhouSannpouResult[] gouhouSanpouNenunNiti = person.GetGouhouSanpouEx(nenunKansi, person.nikkansi, taiunKansi, nenunKansi);
            GouhouSannpouResult[] gouhouSanpouNenunGetu = person.GetGouhouSanpouEx(nenunKansi, person.gekkansi, taiunKansi, nenunKansi);
            GouhouSannpouResult[] gouhouSanpouNenunNen = person.GetGouhouSanpouEx(nenunKansi, person.nenkansi, taiunKansi, nenunKansi);
            chkBitFlg = bitFlgTaiun | bitFlgNitiGetuNen;

            if (gouhouSanpouNenunNiti != null && gouhouSanpouNenunNiti.Length > 0)//年運 - 日
            {
                int idx = SetMatrixDown(true, chkBitFlg, (bitFlgNenun | bitFlgNiti));
                DrawLine(idx, nenunCenterX, nikkansiCenterX, drawBottomSi, dircDown);
                DrawString(idx, nenunCenterX, nikkansiCenterX, drawBottomSi, dircDown, gouhouSanpouNenunNiti);
            }
            if (gouhouSanpouNenunGetu != null && gouhouSanpouNenunGetu.Length > 0)//年運 - 月
            {
                int idx = SetMatrixDown(true, chkBitFlg, (bitFlgNenun | bitFlgGetu));
                DrawLine(idx, nenunCenterX, gekkansiCenterX, drawBottomSi, dircDown);
                DrawString(idx, nenunCenterX, gekkansiCenterX, drawBottomSi, dircDown, gouhouSanpouNenunGetu);
            }

            if (gouhouSanpouNenunNen != null && gouhouSanpouNenunNen.Length > 0)//年運 - 年
            {
                int idx = SetMatrixDown(true, chkBitFlg, (bitFlgNenun | bitFlgNen));
                DrawLine(idx, nenunCenterX, nenkansiCenterX, drawBottomSi, dircDown);
                DrawString(idx, nenunCenterX, nenkansiCenterX, drawBottomSi, dircDown, gouhouSanpouNenunNen);
            }


            if (bDispGetuun)
            {
                //月運 →＊
                GouhouSannpouResult[] gouhouSanpouGetuunNiti = person.GetGouhouSanpouEx(getuunKansi, person.nikkansi, taiunKansi, nenunKansi);
                GouhouSannpouResult[] gouhouSanpouuGetuunGetu = person.GetGouhouSanpouEx(getuunKansi, person.gekkansi, taiunKansi, nenunKansi);
                GouhouSannpouResult[] gouhouSanpouuGetuunNen = person.GetGouhouSanpouEx(getuunKansi, person.nenkansi, taiunKansi, nenunKansi);
                chkBitFlg = bitFlgGetuun | bitFlgNenun | bitFlgTaiun | bitFlgNitiGetuNen;

                if (gouhouSanpouGetuunNiti != null && gouhouSanpouGetuunNiti.Length > 0)//月運 - 日
                {
                    int idx = SetMatrixDown(true, chkBitFlg, (bitFlgGetuun | bitFlgNiti));
                    DrawLine(idx, getuunCenterX, nikkansiCenterX, drawBottomSi, dircDown);
                    DrawString(idx, getuunCenterX, nikkansiCenterX, drawBottomSi, dircDown, gouhouSanpouGetuunNiti);
                }
                if (gouhouSanpouuGetuunGetu != null && gouhouSanpouuGetuunGetu.Length > 0)//月運 - 月
                {
                    int idx = SetMatrixDown(true, chkBitFlg, (bitFlgGetuun | bitFlgGetu));
                    DrawLine(idx, getuunCenterX, gekkansiCenterX, drawBottomSi, dircDown);
                    DrawString(idx, getuunCenterX, gekkansiCenterX, drawBottomSi, dircDown, gouhouSanpouuGetuunGetu);
                }

                if (gouhouSanpouuGetuunNen != null && gouhouSanpouuGetuunNen.Length > 0)//月運 - 年
                {
                    int idx = SetMatrixDown(true, chkBitFlg, (bitFlgGetuun | bitFlgNen));
                    DrawLine(idx, getuunCenterX, nenkansiCenterX, drawBottomSi, dircDown);
                    DrawString(idx, getuunCenterX, nenkansiCenterX, drawBottomSi, dircDown, gouhouSanpouuGetuunNen);
                }
            }
        }
    }

}
