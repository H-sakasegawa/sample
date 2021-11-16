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
        Insen insen;
        JuniSinKanHou juniSinKanHou;

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

       
        Rectangle[] rectNikansiZogan = new Rectangle[3];    //日干支 初元、中元、本元
        Rectangle[] rectGekkansiZogan = new Rectangle[3];   //月干支 初元、中元、本元
        Rectangle[] rectNenkansiZogan = new Rectangle[3];   //年干支 初元、中元、本元
        Rectangle[] rectGetuunZogan = new Rectangle[3];     //月運   初元、中元、本元
        Rectangle[] rectNenunZogan = new Rectangle[3];      //年運   初元、中元、本元
        Rectangle[] rectTaiunZogan = new Rectangle[3];      //大運   初元、中元、本元

        Kansi taiunKansi = null;
        Kansi nenunKansi = null;
        Kansi getuunKansi = null;

        bool bDispTaiun = false; //true..大運表示
        bool bDispNenun = false; //true..年運表示
        bool bDispGetuun = false; //true..月運表示
        bool bDispSangouKaikyoku = false;
        bool bDispGogyou = false;
        bool bDispGotoku = false;
        bool bDispZougan = false; //蔵元表示　有無
        bool bDispJuniSinkanHou = false;

        /// <summary>
        /// 後天運 描画クラス コンストラクタ
        /// </summary>
        /// <param name="person">人情報</param>
        /// <param name="pictureBox">描画先のピクチャーボックス</param>
        /// <param name="_getuunKansi">月運干支</param>
        /// <param name="_nenunKansi">年運干支</param>
        /// <param name="_taiunKansi">大運干支</param>
        /// <param name="_bDispTaiun">true..大運⇒＊のライン描画</param>
        /// <param name="_bDispNenun">true...年運⇒＊のライン描画</param>
        /// <param name="_bDispGetuun">true...月運表示と月運⇒＊のライン描画</param>
        /// <param name="_bDispSangouKaikyoku">true...三合会局・方三位表示</param>
        /// <param name="_bDispGogyou">true...五行反映</param>
        /// <param name="_bDispGotoku">true... 五徳反映</param>
        /// <param name="_bDispZougan">true... 蔵元表示</param>
        /// <param name="_bDispJuniSinkanHou">true... 十二親干法</param>
        public DrawKoutenUn(Person person, 
                            PictureBox pictureBox, 
                            Kansi _taiunKansi,
                            Kansi _nenunKansi,
                            Kansi _getuunKansi,
                            bool _bDispTaiun,
                            bool _bDispNenun,
                            bool _bDispGetuun,
                            bool _bDispSangouKaikyoku,
                            bool _bDispGogyou,
                            bool _bDispGotoku,
                            bool _bDispZougan,
                            bool _bDispJuniSinkanHou,
                            int _fntSize = -1
                            ) 
            :base(person, pictureBox, _fntSize)
        {

            insen = new Insen(person);
            juniSinKanHou = new JuniSinKanHou();

            taiunKansi = _taiunKansi;
            nenunKansi = _nenunKansi;
            getuunKansi = _getuunKansi;

            bDispTaiun = _bDispTaiun;
            bDispNenun = _bDispNenun;
            bDispGetuun = _bDispGetuun;
            bDispSangouKaikyoku = _bDispSangouKaikyoku;
            bDispGogyou = _bDispGogyou;
            bDispGotoku = _bDispGotoku;
            bDispZougan = _bDispZougan;
            bDispJuniSinkanHou = _bDispJuniSinkanHou;

        }

        /// <summary>
        /// 虚気変化パターン表示用 後天運 描画クラス コンストラクタ
        /// </summary>
        /// <param name="person">人情報</param>
        /// <param name="pictureBox">描画先のピクチャーボックス</param>
        /// <param name="year">表示対象年</param>
        /// <param name="_getuunKansi">月運干支</param>
        /// <param name="_nenunKansi">年運干支</param>
        /// <param name="_taiunKansi">大運干支</param>
        /// <param name="_bDispGetuun">true...月運表示と月運⇒＊のライン描画</param>
        /// <param name="_bDispSangouKaikyoku">true...三合会局・方三位表示</param>
        /// <param name="_bDispGogyou">true...五行反映</param>
        /// <param name="_bDispGotoku">true... 五徳反映</param>
        /// <param name="_bDispJuniSinkanHou">true... 十二親干法</param>
        public DrawKoutenUn(Person person, 
                            PictureBox pictureBox,
                            Kansi _taiunKansi, 
                            Kansi _nenunKansi, 
                            Kansi _getuunKansi,
                            bool _bDispGetuun,
                            bool _bDispSangouKaikyoku,
                            bool _bDispGogyou,
                            bool _bDispGotoku,
                            bool _bDispJuniSinkanHou
                            )
            : base(person, pictureBox)
        {

            taiunKansi = _taiunKansi;
            nenunKansi = _nenunKansi;
            getuunKansi = _getuunKansi;


            rangeHeight = (int)(GetFontHeight() * Const.dKansiHeightRate);
            rangeWidth = 45;
            bDispTaiun = false;
            bDispNenun = false;
            bDispGetuun = _bDispGetuun;
            bDispSangouKaikyoku = _bDispSangouKaikyoku;
            bDispGogyou = _bDispGogyou;
            bDispGotoku = _bDispGotoku;
            bDispJuniSinkanHou = _bDispJuniSinkanHou;
 
        }

        /// <summary>
        /// 表示座標計算
        /// </summary>
        void CalcCoord()
        {
            CalcCoord(idxMtx + 1);
        }
        public void CalcCoord(int topLineCnt)
        {
            int ofsX = GetLineOffsetX();
            int ofsY = (topLineCnt) * GetLineOffsetY() + 10;
            if (bDispGetuun)
            {
                //月運表示開始位置
                getuun.X = ofsX;
                getuun.Y = ofsY;
                getuunCenterX = nenun.X + rangeWidth / 2;
                ofsX += rangeWidth;
            }

            //年運 表示開始位置
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
            drawBottomStartY = drawTopSi + rangeHeight;


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

            if (bDispZougan)
            {
                int zouganStartY = drawTopSi + rangeHeight +2;
                int zouganY = zouganStartY;
                 ofsY = 20;
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


                //月運 初元、中元、本元
                zouganY = zouganStartY;
                rectGetuunZogan[(int)NijuhachiGenso.enmGensoType.GENSO_SHOGEN] = new Rectangle(getuun.X, zouganY, rangeWidth, rangeHeight); zouganY += ofsY;
                rectGetuunZogan[(int)NijuhachiGenso.enmGensoType.GENSO_CHUGEN] = new Rectangle(getuun.X, zouganY, rangeWidth, rangeHeight); zouganY += ofsY;
                rectGetuunZogan[(int)NijuhachiGenso.enmGensoType.GENSO_HONGEN] = new Rectangle(getuun.X, zouganY, rangeWidth, rangeHeight);
                //年運 初元、中元、本元
                zouganY = zouganStartY;
                rectNenunZogan[(int)NijuhachiGenso.enmGensoType.GENSO_SHOGEN] = new Rectangle(nenun.X, zouganY, rangeWidth, rangeHeight); zouganY += ofsY;
                rectNenunZogan[(int)NijuhachiGenso.enmGensoType.GENSO_CHUGEN] = new Rectangle(nenun.X, zouganY, rangeWidth, rangeHeight); zouganY += ofsY;
                rectNenunZogan[(int)NijuhachiGenso.enmGensoType.GENSO_HONGEN] = new Rectangle(nenun.X, zouganY, rangeWidth, rangeHeight);
                //大運 初元、中元、本元
                zouganY = zouganStartY;
                rectTaiunZogan[(int)NijuhachiGenso.enmGensoType.GENSO_SHOGEN] = new Rectangle(taiun.X, zouganY, rangeWidth, rangeHeight); zouganY += ofsY;
                rectTaiunZogan[(int)NijuhachiGenso.enmGensoType.GENSO_CHUGEN] = new Rectangle(taiun.X, zouganY, rangeWidth, rangeHeight); zouganY += ofsY;
                rectTaiunZogan[(int)NijuhachiGenso.enmGensoType.GENSO_HONGEN] = new Rectangle(taiun.X, zouganY, rangeWidth, rangeHeight);
               
                //干支表より下の表示のライン描画開始座標を変更する                    
                drawBottomStartY = rectTaiunZogan[(int)NijuhachiGenso.enmGensoType.GENSO_HONGEN].Y + rangeHeight-10;
            }
        }

        /// <summary>
        /// 干支描画
        /// </summary>
        /// <param name="g"></param>
        protected override void DrawKansi( Graphics g, JuniSinkankanHouAttr attrJuniSinkanHou = null)
        {

            //三合会局
            var lstSangouKaikyoku = person.GetSangouKaikyoku(getuunKansi, nenunKansi, taiunKansi);
            //方三位
            var lstHouSani = person.GetHouSani(getuunKansi, nenunKansi, taiunKansi);

            Color[] colorGetuunKansi = null;
            Color[] colorNenunKansi = null;
            Color[] colorTaiunKansi = null;
            Color[] colorNikkansi = null;
            Color[] colorGekkansi = null;
            Color[] colorNenkansi = null;

            Color[] colorGetuunKansiOrg = new Color[2];
            Color[] colorNenunKansiOrg = new Color[2];
            Color[] colorTaiunKansiOrg = new Color[2];
            Color[] colorNikkansiOrg = new Color[2];
            Color[] colorGekkansiOrg = new Color[2];
            Color[] colorNenkansiOrg = new Color[2];

            CreateGogyouAttrMatrix(person, getuunKansi, nenunKansi, taiunKansi);

            if (bDispGogyou)
            {   //五行色表示
                colorNikkansi = GetGogyouColor(Const.enumKansiItemID.NIKKANSI);
                colorGekkansi = GetGogyouColor(Const.enumKansiItemID.GEKKANSI);
                colorNenkansi = GetGogyouColor(Const.enumKansiItemID.NENKANSI);

                colorGetuunKansi = GetGogyouColor(Const.enumKansiItemID.GETUUN); //月運
                colorNenunKansi = GetGogyouColor(Const.enumKansiItemID.NENUN);   //年運
                colorTaiunKansi = GetGogyouColor(Const.enumKansiItemID.TAIUN);   //大運

            }
            else if (bDispGotoku)
            {   //五徳色表示
                string baseKan = person.nikkansi.kan;
                colorNikkansi = GetGotokuColor(baseKan, person.nikkansi, true);
                colorGekkansi = GetGotokuColor(baseKan, person.gekkansi);
                colorNenkansi = GetGotokuColor(baseKan, person.nenkansi);

                colorGetuunKansi = GetGotokuColor(baseKan, getuunKansi);
                colorNenunKansi = GetGotokuColor(baseKan, nenunKansi);
                colorTaiunKansi = GetGotokuColor(baseKan, taiunKansi);
            }

            if (colorGetuunKansi != null) colorGetuunKansi.CopyTo(colorGetuunKansiOrg, 0);
            if (colorNenunKansi != null) colorNenunKansi.CopyTo(colorNenunKansiOrg, 0);
            if (colorTaiunKansi != null) colorTaiunKansi.CopyTo(colorTaiunKansiOrg, 0);

            if (colorNikkansi != null) colorNikkansi.CopyTo(colorNikkansiOrg, 0);
            if (colorGekkansi != null) colorGekkansi.CopyTo(colorGekkansiOrg, 0);
            if (colorNenkansi != null) colorNenkansi.CopyTo(colorNenkansiOrg, 0);

            if (bDispGogyou || bDispGotoku)
            {
                //合法反映
                //----------------------------
                if (person.bRefrectSigou || person.bRefrectHankai) //支合、半会 反映指定あり
                {
                    //支の変換
                    RefrectGouhou(
                                    colorNikkansi, colorGekkansi, colorNenkansi,
                                    colorGetuunKansi, colorNenunKansi, colorTaiunKansi,
                                    getuunKansi, nenunKansi, taiunKansi,
                                    bDispGetuun
                                    );
                }
                if (person.bRefrectKangou)
                {
                    //干合反映　干の変換
                    RefrectKangou(
                                    colorNikkansi, colorGekkansi, colorNenkansi,
                                    colorGetuunKansi, colorNenunKansi, colorTaiunKansi,
                                    getuunKansi, nenunKansi, taiunKansi,
                                    bDispGetuun
                                    );
                }

                //三合会局・方三位　反映
                if (person.bRefrectHousani|| person.bRefrectSangouKaikyoku)//方三位、三合会局 反映指定あり
                {
                    RefrectSangouKaikyokuHousanni(
                                lstSangouKaikyoku, lstHouSani,
                                colorNikkansi, colorGekkansi, colorNenkansi,
                                colorGetuunKansi, colorNenunKansi, colorTaiunKansi
                                );
                }
            }
            //五徳表示の時に、合法反映、三合会局・方三位　反映があった場合は、属性が変わっているので
            //変わった属性をもとに再度表示カラーを求める
            if (bDispGotoku && (person.bRefrectHankai || person.bRefrectSigou || person.bRefrectHousani || person.bRefrectSangouKaikyoku))
            {
                var attrBaseItem = GetAttrTblItem(Const.enumKansiItemID.NIKKANSI);

                var attrItem = GetAttrTblItem(Const.enumKansiItemID.NIKKANSI);
                colorNikkansi = GetGotokuColor(colorNikkansi, attrBaseItem.attrKan, null, attrItem.attrSi);

                attrItem = GetAttrTblItem(Const.enumKansiItemID.GEKKANSI);
                colorGekkansi = GetGotokuColor(colorGekkansi, attrBaseItem.attrKan, attrItem.attrKan, attrItem.attrSi);

                attrItem = GetAttrTblItem(Const.enumKansiItemID.NENKANSI);
                colorNenkansi = GetGotokuColor(colorNenkansi, attrBaseItem.attrKan, attrItem.attrKan, attrItem.attrSi);

                attrItem = GetAttrTblItem(Const.enumKansiItemID.GETUUN);
                colorGetuunKansi = GetGotokuColor(colorGetuunKansi, attrBaseItem.attrKan, attrItem.attrKan, attrItem.attrSi);

                attrItem = GetAttrTblItem(Const.enumKansiItemID.NENUN);
                colorNenunKansi = GetGotokuColor(colorNenunKansi, attrBaseItem.attrKan, attrItem.attrKan, attrItem.attrSi);

                attrItem = GetAttrTblItem(Const.enumKansiItemID.TAIUN);
                colorTaiunKansi = GetGotokuColor(colorTaiunKansi, attrBaseItem.attrKan, attrItem.attrKan, attrItem.attrSi);


            }


            Kansi getuunKansiWk = null;
            Kansi nenunKansiWk = null;
            Kansi taiunKansiWk = null;
            Kansi nikkansiWk = null;
            Kansi gekkansiWk = null;
            Kansi nenkansiWk = null;


            if(attrJuniSinkanHou!=null)
            {
                //十二親干法の表示文字に切り替える
                if (getuunKansi != null) getuunKansiWk = attrJuniSinkanHou.GeJuniSinkanHouString(getuunKansi);
                nenunKansiWk = attrJuniSinkanHou.GeJuniSinkanHouString(nenunKansi);
                taiunKansiWk = attrJuniSinkanHou.GeJuniSinkanHouString(taiunKansi);
                nikkansiWk = attrJuniSinkanHou.GeJuniSinkanHouString(person.nikkansi);
                gekkansiWk = attrJuniSinkanHou.GeJuniSinkanHouString(person.gekkansi);
                nenkansiWk = attrJuniSinkanHou.GeJuniSinkanHouString(person.nenkansi);
            }
            else
            {
                //干支表示 
                if (getuunKansi != null) getuunKansiWk = getuunKansi.Clone();
                nenunKansiWk = nenunKansi.Clone();
                taiunKansiWk = taiunKansi.Clone();
                nikkansiWk = person.nikkansi.Clone();
                gekkansiWk = person.gekkansi.Clone();
                nenkansiWk = person.nenkansi.Clone();
            }

            int titleHeight = (int)(GetSmallFontHeight() * 0.7);
            rectGetuunTitle = new Rectangle(getuun.X, getuun.Y - titleHeight / 2, rangeWidth, titleHeight);
            rectNenunTitle = new Rectangle(nenun.X, nenun.Y - titleHeight / 2, rangeWidth, titleHeight);
            rectTaiunTitle = new Rectangle(taiun.X, taiun.Y - titleHeight / 2, rangeWidth, titleHeight);

            if (bDispGetuun)
            {
                DrawKansi(getuunKansiWk, rectGetuunKan, rectGetuunSi, colorGetuunKansi, Const.enumKansiItemID.GETUUN);　//月運干支
                DrawString(rectGetuunTitle, "月運");
            }
            DrawKansi(nenunKansiWk, rectNenunKan, rectNenunSi, colorNenunKansi, Const.enumKansiItemID.NENUN);//年運干支
            DrawKansi(taiunKansiWk, rectTaiunKan, rectTaiunSi, colorTaiunKansi, Const.enumKansiItemID.TAIUN);//大運干支
            DrawString(rectNenunTitle, "年運");
            DrawString(rectTaiunTitle, "大運");

            DrawKansi(nikkansiWk, rectNikansiKan, rectNikansiSi, colorNikkansi, Const.enumKansiItemID.NIKKANSI);//日干支
            DrawKansi(gekkansiWk, rectGekkansiKan, rectGekkansiSi, colorGekkansi, Const.enumKansiItemID.GEKKANSI);//月干支
            DrawKansi(nenkansiWk, rectNenkansiKan, rectNenkansiSi, colorNenkansi, Const.enumKansiItemID.NENKANSI);//年干支

        }
        /// <summary>
        /// 蔵元描画
        /// </summary>
        /// <param name="g"></param>
        private void DrawZougan(Graphics g, JuniSinkankanHouAttr attrJuniSinkanHou=null)
        {
            if (bDispZougan)
            {
                var getuunHongen = insen.GetHongen(getuunKansi);
                var nenunHongen = insen.GetHongen(nenunKansi);
                var taiunHongen = insen.GetHongen(taiunKansi);

                string[] nikkansiWk = new string[3];
                string[] gekkansiWk = new string[3];
                string[] nenkansiWk = new string[3];
                string[] getuunWk = new string[3];
                string[] nenunWK = new string[3];
                string[] taiunWk = new string[3];

                if (attrJuniSinkanHou!=null)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        nikkansiWk[i] = attrJuniSinkanHou.GeJuniSinkanHouString( insen.nikkansiHongen[i].name);
                        gekkansiWk[i] = attrJuniSinkanHou.GeJuniSinkanHouString( insen.gekkansiHongen[i].name);
                        nenkansiWk[i] = attrJuniSinkanHou.GeJuniSinkanHouString( insen.nenkansiHongen[i].name);

                        if(getuunHongen!=null)getuunWk[i] = attrJuniSinkanHou.GeJuniSinkanHouString(getuunHongen[i].name);
                        nenunWK[i] = attrJuniSinkanHou.GeJuniSinkanHouString(nenunHongen[i].name);
                        taiunWk[i] = attrJuniSinkanHou.GeJuniSinkanHouString(taiunHongen[i].name);
                    }
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        nikkansiWk[i] = insen.nikkansiHongen[i].name;
                        gekkansiWk[i] = insen.gekkansiHongen[i].name;
                        nenkansiWk[i] = insen.nenkansiHongen[i].name;

                        if (getuunHongen != null) getuunWk[i] = getuunHongen[i].name;
                        nenunWK[i] = nenunHongen[i].name;
                        taiunWk[i] = taiunHongen[i].name;
                    }
                }


                foreach (var item in Enum.GetValues(typeof(NijuhachiGenso.enmGensoType)))//初元、中元、本元
                {
                    int idx = (int)item;
                    //bool bBold = insen.nikkansiHongen[idx].bJudaiShuseiGenso;
                    DrawZouganItem(g, nikkansiWk[idx], rectNikansiZogan[idx], Color.Black, false);
                    //bBold = insen.gekkansiHongen[idx].bJudaiShuseiGenso;
                    DrawZouganItem(g, gekkansiWk[idx], rectGekkansiZogan[idx], Color.Black, false, false);

                    //bBold = insen.nenkansiHongen[idx].bJudaiShuseiGenso;
                    DrawZouganItem(g, nenkansiWk[idx], rectNenkansiZogan[idx], Color.Black, false);

                    //月運
                    if (bDispGetuun)
                    {
                        if (getuunHongen != null) DrawZouganItem(g, getuunWk[idx], rectGetuunZogan[idx], Color.Black, false);
                    }
                    DrawZouganItem(g, nenunWK[idx], rectNenunZogan[idx], Color.Black, false);
                    DrawZouganItem(g, taiunWk[idx], rectTaiunZogan[idx], Color.Black, false);
                }
            }


        }
        /// <summary>
        /// 蔵元描画 サブ関数
        /// </summary>
        /// <param name="g"></param>
        /// <param name="genso"></param>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        /// <param name="bBold"></param>
        /// <param name="bShugosin"></param>
        private void DrawZouganItem(Graphics g, string genso, Rectangle rect, Color color, bool bBold, bool bShugosin = true)
        {
            var fntZougan = fnt;
            Font goodFont = Common.FindFont(g, genso, rect.Size, fnt);

            //if (bBold)
            //{
            //    fntZougan = fntBold;
            //}
            ////干の守護神判定
            //if (!string.IsNullOrEmpty(genso) && bShugosin)
            //{
            //    if (ShugosinUtil.IsShugosin(person, genso)) g.FillRectangle(Const.brusShugosin, rect);

            //    if (ShugosinUtil.IsImigami(person, genso)) g.FillRectangle(Const.brusImigami, rect);
            //}
            var brush = new SolidBrush(color);
            g.DrawString(genso, goodFont, brush, rect, stringFormat);

        }


        private JuniSinkankanHouAttr GetJuniSinkanHou()
        {
            var node = juniSinKanHou.Create(person);
            JuniSinkankanHouAttr attr = new JuniSinkankanHouAttr();
            //自分
            attr.mine = node.kan;
            //母親
            attr.mother = node.parent.kan;
            //父親
            attr.father = node.parent.partnerMan.kan;

            if (person.gender == Gender.WOMAN)
            {
                //夫
                attr.husband = node.partnerMan.kan;
                //子
                attr.child = node.child.kan;
            }
            else
            {
                //妻
                attr.wife = node.partnerWoman.kan;
                //子
                attr.child = node.partnerWoman.child.kan;
            }

 
            return attr;
        }


        /// <summary>
        /// 描画処理
        /// 本関数は、IsouhouBase::Draw()から呼び出されます
        /// </summary>
        /// <param name="g"></param>
        protected override void DrawItem(Graphics g)
        {
            if (person == null) return;

            int bitFlgNitiGetuNen = (Const.bitFlgNiti | Const.bitFlgGetu | Const.bitFlgNen);
            int bitFlgAll = (Const.bitFlgGetuun | Const.bitFlgNenun | Const.bitFlgTaiun| bitFlgNitiGetuNen);
            int chkBitFlg = 0;

            //陰陽(年運→大運、月運→年運、月運→大運）
            //-------------------               
            bool bInyouNenunTaiun = person.IsInyou(nenunKansi, taiunKansi); //（年運 - 大運) の関係
            bool bInyouGetuunNenun = person.IsInyou(getuunKansi, nenunKansi); //（月運 - 年運) の関係
            bool bInyouGetuunTaiun = person.IsInyou(getuunKansi, taiunKansi); //（月運 - 大運) の関係
            int idxInyouNenunTaiun = SetMatrixUp(bInyouNenunTaiun, Const.bitFlgNenun, (Const.bitFlgNenun | Const.bitFlgTaiun));//年運 - 大運
            int idxInyouGetuunNenun = -1;
            int idxInyouGetuunTaiun = -1;
            if (bDispGetuun)
            {
                idxInyouGetuunNenun = SetMatrixUp(bInyouGetuunNenun, Const.bitFlgGetuun, (Const.bitFlgGetuun | Const.bitFlgNenun));//月運 - 年運
                idxInyouGetuunTaiun = SetMatrixUp(bInyouGetuunTaiun, Const.bitFlgGetuun | Const.bitFlgNenun, (Const.bitFlgGetuun | Const.bitFlgTaiun));//月運 - 大運
            }

            //陰陽(大運→＊）
            //-------------------
            chkBitFlg = Const.bitFlgTaiun | bitFlgNitiGetuNen;
            bool bInyouTaiunNiti = person.IsInyou(taiunKansi, person.nikkansi); //（大運-日) の関係
            bool bInyouTaiunGetu = person.IsInyou(taiunKansi, person.gekkansi); //（大運-月) の関係
            bool bInyouTaiunNen = person.IsInyou(taiunKansi, person.nenkansi);//（大運-年) の関係
            int idxInyouTaiunNiti = SetMatrixUp(bInyouTaiunNiti, chkBitFlg, (Const.bitFlgTaiun | Const.bitFlgNiti));//大運 - 日
            int idxInyouTaiunGetu = SetMatrixUp(bInyouTaiunGetu, chkBitFlg, (Const.bitFlgTaiun | Const.bitFlgGetu));//大運 - 月
            int idxInyouTaiunNen = SetMatrixUp(bInyouTaiunNen, chkBitFlg, (Const.bitFlgTaiun | Const.bitFlgNen));//大運 - 年


            //陰陽(年運→＊）
            //-------------------               
            chkBitFlg = Const.bitFlgNenun | Const.bitFlgTaiun | bitFlgNitiGetuNen;
            bool bInyouNenunNiti = person.IsInyou(nenunKansi, person.nikkansi); //（年運-日) の関係
            bool bInyouNenunGetu = person.IsInyou(nenunKansi, person.gekkansi);//（年運-月) の関係
            bool bInyouNenunNen = person.IsInyou(nenunKansi, person.nenkansi);//（年運-年) の関係
            int idxInyouNenunNiti = SetMatrixUp(bInyouNenunNiti, chkBitFlg, (Const.bitFlgNenun | Const.bitFlgNiti));//年運 - 日
            int idxInyouNenunGetu = SetMatrixUp(bInyouNenunGetu, chkBitFlg, (Const.bitFlgNenun | Const.bitFlgGetu));//年運 - 月
            int idxInyouNenunNen = SetMatrixUp(bInyouNenunNen, chkBitFlg, (Const.bitFlgNenun | Const.bitFlgNen));//年運 - 年


            //陰陽(月運→＊）
            //-------------------               
            chkBitFlg = Const.bitFlgGetuun | Const.bitFlgNenun | Const.bitFlgTaiun | bitFlgNitiGetuNen;
            bool bInyouGetuunNiti = person.IsInyou(getuunKansi, person.nikkansi); //（月運-日) の関係
            bool bInyouGetuunGetu = person.IsInyou(getuunKansi, person.gekkansi);//（月運-月) の関係
            bool bInyouGetuunNen = person.IsInyou(getuunKansi, person.nenkansi);//（月運-年) の関係
            int idxInyouGetuunNiti = -1;
            int idxInyouGetuunGetu = -1;
            int idxInyouGetuunNen  = -1;
            if (bDispGetuun)
            {
                idxInyouGetuunNiti = SetMatrixUp(bInyouGetuunNiti, chkBitFlg, (Const.bitFlgGetuun | Const.bitFlgNiti));//月運 - 日
                idxInyouGetuunGetu = SetMatrixUp(bInyouGetuunGetu, chkBitFlg, (Const.bitFlgGetuun | Const.bitFlgGetu));//月運 - 月
                idxInyouGetuunNen = SetMatrixUp(bInyouGetuunNen, chkBitFlg, (Const.bitFlgGetuun | Const.bitFlgNen));//月運 - 年
            }

            //干合(年運→大運、月運→年運、月運→大運運）
            //-------------------               
            bool bKangouNenunTaiun = person.IsKango(nenunKansi, taiunKansi); //（年運 - 大運) の関係
            bool bKangouGetuunNenun = person.IsKango(getuunKansi, nenunKansi); //（月運 - 年運) の関係
            bool bKangouGetuunTaiun = person.IsKango(getuunKansi, taiunKansi); //（月運 - 大運) の関係
            int idxKangouNenunTaiun = SetMatrixUp(bKangouNenunTaiun, Const.bitFlgNenun | Const.bitFlgTaiun | bitFlgNitiGetuNen, (Const.bitFlgNenun | Const.bitFlgTaiun));//年運 - 大運
            int idxKangouGetuunNenun = -1;
            int idxKangouGetuunTaiun = -1;
            if (bDispGetuun)
            {
                idxKangouGetuunNenun = SetMatrixUp(bKangouGetuunNenun, Const.bitFlgNenun | Const.bitFlgTaiun | bitFlgNitiGetuNen, (Const.bitFlgGetuun | Const.bitFlgNenun));//月運 - 年運
                idxKangouGetuunTaiun = SetMatrixUp(bKangouGetuunTaiun, Const.bitFlgGetuun | Const.bitFlgNenun | Const.bitFlgTaiun | bitFlgNitiGetuNen, (Const.bitFlgGetuun | Const.bitFlgTaiun));//月運 - 大運
            }

            //干合(大運→＊）
            //-------------------
            chkBitFlg = Const.bitFlgTaiun | bitFlgNitiGetuNen;
            bool bKangouTaiunNiti = person.IsKango(taiunKansi, person.nikkansi);//（大運-日) の関係
            bool bKangouTaiunGetu = person.IsKango(taiunKansi, person.gekkansi);//（大運-月) の関係
            bool bKangouTaiunNen = person.IsKango(taiunKansi, person.nenkansi);//（大運-年) の関係
            int idxKangouTaiunNiti = SetMatrixUp(bKangouTaiunNiti, chkBitFlg, (Const.bitFlgTaiun | Const.bitFlgNiti));//大運 - 日
            int idxKangouTaiunGetu = SetMatrixUp(bKangouTaiunGetu, chkBitFlg, (Const.bitFlgTaiun | Const.bitFlgGetu));//大運 - 月
            int idxKangouTaiunNen = SetMatrixUp(bKangouTaiunNen, chkBitFlg, (Const.bitFlgTaiun | Const.bitFlgNen));//大運 - 年


            //干合(年運→＊）
            //-------------------
            chkBitFlg = Const.bitFlgNenun | Const.bitFlgTaiun | bitFlgNitiGetuNen;
            bool bKangouNenunNiti = person.IsKango(nenunKansi, person.nikkansi);//（年運-日) の関係
            bool bKangouNenunGetu = person.IsKango(nenunKansi, person.gekkansi);//（年運-月) の関係
            bool bKangouNenunNen = person.IsKango(nenunKansi, person.nenkansi);//（年運-年) の関係
            int idxKangouNenunNiti = SetMatrixUp(bKangouNenunNiti, chkBitFlg, (Const.bitFlgNenun | Const.bitFlgNiti));//年運 - 日
            int idxKangouNenunGetu = SetMatrixUp(bKangouNenunGetu, chkBitFlg, (Const.bitFlgNenun | Const.bitFlgGetu));//年運 - 月
            int idxKangouNenunNen = SetMatrixUp(bKangouNenunNen, chkBitFlg, (Const.bitFlgNenun | Const.bitFlgNen));//年運 - 年

            //干合(月運→＊）
            //-------------------
            chkBitFlg = Const.bitFlgGetuun | bitFlgNitiGetuNen;
            bool bKangouGetuunNiti = person.IsKango(getuunKansi, person.nikkansi);//（月運-日) の関係
            bool bKangouGetuunGetu = person.IsKango(getuunKansi, person.gekkansi);//（月運-月) の関係
            bool bKangouGetuunNen = person.IsKango(getuunKansi, person.nenkansi);//（月運-年) の関係
            int idxKangouGetuunNiti = -1;
            int idxKangouGetuunGetu = -1;
            int idxKangouGetuunNen = -1;
            if (bDispGetuun)
            {
                idxKangouGetuunNiti = SetMatrixUp(bKangouGetuunNiti, chkBitFlg, (Const.bitFlgGetuun | Const.bitFlgNiti));//月運 - 日
                idxKangouGetuunGetu = SetMatrixUp(bKangouGetuunGetu, chkBitFlg, (Const.bitFlgGetuun | Const.bitFlgGetu));//月運 - 月
                idxKangouGetuunNen = SetMatrixUp(bKangouGetuunNen, chkBitFlg, (Const.bitFlgGetuun | Const.bitFlgNen));//月運 - 年
            }

        
            //七殺(年運→大運、月運→年運、月運→大運）
            //-------------------               
            chkBitFlg = Const.bitFlgNenun | Const.bitFlgTaiun | bitFlgNitiGetuNen;
            int idxNanasatuPointNenunTaiun = 0;
            int idxNanasatuPointGetuunNenun = 0;
            int idxNanasatuPointGetuunTaiun = 0;
            bool bNanasatuNenunTaiun = person.IsNanasatu(nenunKansi, taiunKansi, ref idxNanasatuPointNenunTaiun); //（年運 - 大運) の関係
            bool bNanasatuGetuunNenun = person.IsNanasatu(getuunKansi, nenunKansi, ref idxNanasatuPointGetuunNenun); //（月運 - 年運) の関係
            bool bNanasatuGetuunTaiun = person.IsNanasatu(getuunKansi, taiunKansi, ref idxNanasatuPointGetuunTaiun); //（月運 - 大運) の関係

            int idxNanasatuNenunTaiun = SetMatrixUp(bNanasatuNenunTaiun, chkBitFlg, (Const.bitFlgNenun | Const.bitFlgTaiun));//年運 - 大運
            int idxNanasatuGetuunNenun = -1;
            int idxNanasatuGetuunTaiun = -1;
            if (bDispGetuun)
            {
                idxNanasatuGetuunNenun = SetMatrixUp(bNanasatuGetuunNenun, chkBitFlg, (Const.bitFlgGetuun | Const.bitFlgTaiun));//月運 - 年運
                idxNanasatuGetuunTaiun = SetMatrixUp(bNanasatuGetuunTaiun, chkBitFlg, (Const.bitFlgGetuun | Const.bitFlgTaiun));//月運 - 大運
            }
            //七殺(大運→＊）
            //-------------------
            chkBitFlg = Const.bitFlgTaiun | bitFlgNitiGetuNen;
            int idxNanasatuPointTaiunNiti = 0;
            int idxNanasatuPointTaiunGetu = 0;
            int idxNanasatuPointTaiunNen = 0;
            bool bNanasatuTaiunNiti = person.IsNanasatu(taiunKansi, person.nikkansi, ref idxNanasatuPointTaiunNiti);//（大運-日) の関係
            bool bNanasatuTaiunGetu = person.IsNanasatu(taiunKansi, person.gekkansi, ref idxNanasatuPointTaiunGetu);//（大運-月) の関係
            bool bNanasatuTaiunNen = person.IsNanasatu(taiunKansi, person.nenkansi, ref idxNanasatuPointTaiunNen);//（大運-年) の関係

            int idxNanasatuTaiunNiti = SetMatrixUp(bNanasatuTaiunNiti, chkBitFlg, (Const.bitFlgTaiun | Const.bitFlgNiti));//大運 - 日
            int idxNanasatuTaiunGetu = SetMatrixUp(bNanasatuTaiunGetu, chkBitFlg, (Const.bitFlgTaiun | Const.bitFlgGetu));//大運 - 月
            int idxNanasatuTaiunNen = SetMatrixUp(bNanasatuTaiunNen, chkBitFlg, (Const.bitFlgTaiun | Const.bitFlgNen));//大運 - 年

            //七殺(年運→＊）
            //-------------------
            chkBitFlg = Const.bitFlgNenun | Const.bitFlgTaiun | bitFlgNitiGetuNen;
            int idxNanasatuPointNenunNiti = 0;
            int idxNanasatuPointNenunGetu = 0;
            int idxNanasatuPointNenunNen = 0;
            bool bNanasatuNenunNiti = person.IsNanasatu(nenunKansi, person.nikkansi, ref idxNanasatuPointNenunNiti);//（年運-日) の関係
            bool bNanasatuNenunGetu = person.IsNanasatu(nenunKansi, person.gekkansi, ref idxNanasatuPointNenunGetu);//（年運-月) の関係
            bool bNanasatuNenunNen = person.IsNanasatu(nenunKansi, person.nenkansi, ref idxNanasatuPointNenunNen);//（年運-年) の関係

            int idxNanasatuNenunNiti = SetMatrixUp(bNanasatuNenunNiti, chkBitFlg, (Const.bitFlgNenun | Const.bitFlgNiti));//年運 - 日
            int idxNanasatuNenunGetu = SetMatrixUp(bNanasatuNenunGetu, chkBitFlg, (Const.bitFlgNenun | Const.bitFlgGetu));//年運 - 月
            int idxNanasatuNenunNen = SetMatrixUp(bNanasatuNenunNen, chkBitFlg, (Const.bitFlgNenun | Const.bitFlgNen));//年運 - 年

            //七殺(月運→＊）
            //-------------------
            chkBitFlg = Const.bitFlgGetuun | Const.bitFlgNenun | Const.bitFlgTaiun | bitFlgNitiGetuNen;
            int idxNanasatuPointGetuunNiti = 0;
            int idxNanasatuPointGetuunGetu = 0;
            int idxNanasatuPointGetuunNen = 0;
            bool bNanasatuGetuunNiti = person.IsNanasatu(getuunKansi, person.nikkansi, ref idxNanasatuPointGetuunNiti);//（月運-日) の関係
            bool bNanasatuGetuunGetu = person.IsNanasatu(getuunKansi, person.gekkansi, ref idxNanasatuPointGetuunGetu);//（月運-月) の関係
            bool bNanasatuGetuunNen = person.IsNanasatu(getuunKansi, person.nenkansi, ref idxNanasatuPointGetuunNen);//（月運-年) の関係

            int idxNanasatuGetuunNiti = -1;
            int idxNanasatuGetuunGetu = -1;
            int idxNanasatuGetuunNen = -1;
            if (bDispGetuun)
            {
                idxNanasatuGetuunNiti = SetMatrixUp(bNanasatuGetuunNiti, chkBitFlg, (Const.bitFlgGetuun | Const.bitFlgNiti));//月運 - 日
                idxNanasatuGetuunGetu = SetMatrixUp(bNanasatuGetuunGetu, chkBitFlg, (Const.bitFlgGetuun | Const.bitFlgGetu));//月運 - 月
                idxNanasatuGetuunNen = SetMatrixUp(bNanasatuGetuunNen, chkBitFlg, (Const.bitFlgGetuun | Const.bitFlgNen));//月運 - 年
            }

            //三合会局
            var lstSangouKaikyoku = person.GetSangouKaikyoku(getuunKansi, nenunKansi, taiunKansi);

            //方三位
            var lstHouSani = person.GetHouSani(getuunKansi, nenunKansi, taiunKansi);

            //干支の上部に表示する情報の段数から干支表示基準座標を計算
            CalcCoord();

            /*
                        Color[] colorGetuunKansi = null;
                        Color[] colorNenunKansi = null;
                        Color[] colorTaiunKansi = null;
                        Color[] colorNikkansi = null;
                        Color[] colorGekkansi = null;
                        Color[] colorNenkansi = null;

                        Color[] colorGetuunKansiOrg = new Color[2];
                        Color[] colorNenunKansiOrg = new Color[2];
                        Color[] colorTaiunKansiOrg = new Color[2];
                        Color[] colorNikkansiOrg = new Color[2];
                        Color[] colorGekkansiOrg = new Color[2];
                        Color[] colorNenkansiOrg = new Color[2];

                        CreateGogyouAttrMatrix(person, getuunKansi, nenunKansi, taiunKansi);

                        if (bDispGogyou)
                        {   //五行色表示
                            colorNikkansi = GetGogyouColor(Const.enumKansiItemID.NIKKANSI);
                            colorGekkansi = GetGogyouColor(Const.enumKansiItemID.GEKKANSI);
                            colorNenkansi = GetGogyouColor(Const.enumKansiItemID.NENKANSI);

                            colorGetuunKansi = GetGogyouColor(Const.enumKansiItemID.GETUUN); //月運
                            colorNenunKansi = GetGogyouColor(Const.enumKansiItemID.NENUN);   //年運
                            colorTaiunKansi = GetGogyouColor(Const.enumKansiItemID.TAIUN);   //大運

                        }
                        else if (bDispGotoku)
                        {   //五徳色表示
                            string baseKan = person.nikkansi.kan;
                            colorNikkansi = GetGotokuColor(baseKan, person.nikkansi, true);
                            colorGekkansi = GetGotokuColor(baseKan, person.gekkansi);
                            colorNenkansi = GetGotokuColor(baseKan, person.nenkansi);

                            colorGetuunKansi = GetGotokuColor(baseKan, getuunKansi);
                            colorNenunKansi = GetGotokuColor(baseKan, nenunKansi);
                            colorTaiunKansi = GetGotokuColor(baseKan, taiunKansi);
                        }

                        if( colorGetuunKansi != null) colorGetuunKansi.CopyTo(colorGetuunKansiOrg, 0);
                        if (colorNenunKansi != null) colorNenunKansi.CopyTo(colorNenunKansiOrg, 0);
                        if (colorTaiunKansi != null) colorTaiunKansi.CopyTo(colorTaiunKansiOrg, 0);

                        if (colorNikkansi != null) colorNikkansi.CopyTo(colorNikkansiOrg, 0);
                        if (colorGekkansi != null) colorGekkansi.CopyTo(colorGekkansiOrg, 0);
                        if (colorNenkansi != null) colorNenkansi.CopyTo(colorNenkansiOrg, 0);

                        //合法反映
                        if (bDispRefrectGouhou)
                        {
                            //支の変換
                            RefrectGouhou(
                                            colorNikkansi, colorGekkansi, colorNenkansi,
                                            colorGetuunKansi, colorNenunKansi, colorTaiunKansi,
                                            getuunKansi, nenunKansi, taiunKansi,
                                            bDispGetuun
                                            );
                            //干の変換
                            RefrectKangou(
                                            colorNikkansi, colorGekkansi, colorNenkansi,
                                            colorGetuunKansi, colorNenunKansi, colorTaiunKansi,
                                            getuunKansi, nenunKansi, taiunKansi,
                                             bDispGetuun
                                           );

                        }
                        //三合会局・方三位　反映
                        if (bDispRefrectSangouKaiyoku)
                        {
                            RefrectSangouKaikyokuHousanni(
                                            lstSangouKaikyoku, lstHouSani,
                                            colorNikkansi, colorGekkansi, colorNenkansi,
                                            colorGetuunKansi, colorNenunKansi, colorTaiunKansi
                                            );
                        }
                        //五徳表示の時に、合法反映、三合会局・方三位　反映があった場合は、属性が変わっているので
                        //変わった属性をもとに再度表示カラーを求める
                        if (bDispGotoku && (bDispRefrectGouhou || bDispRefrectSangouKaiyoku) )
                        {
                            var attrBaseItem = GetAttrTblItem(Const.enumKansiItemID.NIKKANSI);

                            var attrItem = GetAttrTblItem(Const.enumKansiItemID.NIKKANSI);
                            colorNikkansi = GetGotokuColor(colorNikkansi, attrBaseItem.attrKan, null, attrItem.attrSi);

                            attrItem = GetAttrTblItem(Const.enumKansiItemID.GEKKANSI);
                            colorGekkansi = GetGotokuColor(colorGekkansi, attrBaseItem.attrKan, attrItem.attrKan, attrItem.attrSi);

                            attrItem = GetAttrTblItem(Const.enumKansiItemID.NENKANSI);
                            colorNenkansi = GetGotokuColor(colorNenkansi, attrBaseItem.attrKan, attrItem.attrKan, attrItem.attrSi);

                            attrItem = GetAttrTblItem(Const.enumKansiItemID.GETUUN);
                            colorGetuunKansi = GetGotokuColor(colorGetuunKansi, attrBaseItem.attrKan, attrItem.attrKan, attrItem.attrSi);

                            attrItem = GetAttrTblItem(Const.enumKansiItemID.NENUN);
                            colorNenunKansi = GetGotokuColor(colorNenunKansi, attrBaseItem.attrKan, attrItem.attrKan, attrItem.attrSi);

                            attrItem = GetAttrTblItem(Const.enumKansiItemID.TAIUN);
                            colorTaiunKansi = GetGotokuColor(colorTaiunKansi, attrBaseItem.attrKan, attrItem.attrKan, attrItem.attrSi);


                        }


                        //干支表示
                        rectGetuunTitle = new Rectangle(getuun.X, getuun.Y - GetSmallFontHeight() / 2, rangeWidth, GetSmallFontHeight());
                        rectNenunTitle = new Rectangle(nenun.X, nenun.Y - GetSmallFontHeight() / 2, rangeWidth, GetSmallFontHeight());
                        rectTaiunTitle = new Rectangle(taiun.X, taiun.Y - GetSmallFontHeight() / 2, rangeWidth, GetSmallFontHeight());

                        if (bDispGetuun)
                        {
                            DrawKansi(getuunKansi, rectGetuunKan, rectGetuunSi, colorGetuunKansi, Const.enumKansiItemID.GETUUN);　//月運干支
                            DrawString(rectGetuunTitle, "<月運>");
                        }
                        DrawKansi(nenunKansi, rectNenunKan, rectNenunSi, colorNenunKansi, Const.enumKansiItemID.NENUN);//年運干支
                        DrawKansi(taiunKansi, rectTaiunKan, rectTaiunSi, colorTaiunKansi, Const.enumKansiItemID.TAIUN);//大運干支
                        DrawString(rectNenunTitle, "<年運>");
                        DrawString(rectTaiunTitle, "<大運>");

                        DrawKansi(person.nikkansi, rectNikansiKan, rectNikansiSi, colorNikkansi, Const.enumKansiItemID.NIKKANSI);//日干支
                        DrawKansi(person.gekkansi, rectGekkansiKan, rectGekkansiSi, colorGekkansi, Const.enumKansiItemID.GEKKANSI);//月干支
                        DrawKansi(person.nenkansi, rectNenkansiKan, rectNenkansiSi, colorNenkansi, Const.enumKansiItemID.NENKANSI);//年干支
            */
            if (bDispJuniSinkanHou)
            {
                var attr = GetJuniSinkanHou();
                //干支描画
                DrawKansi(g, attr);
                //蔵元描画
                DrawZougan(g, attr);

            }
            else
            {
                //干支描画
                DrawKansi(g);
                //蔵元描画
                DrawZougan(g);
            }

            //陰陽(年運→大運）
            //-------------------               
            if (bDispTaiun && bDispNenun)
            {
                if (bInyouNenunTaiun)//年運→大運
                {
                    DrawLine(idxInyouNenunTaiun, nenunCenterX, taiunCenterX, drawTopKan, dircUp);
                    DrawString(idxInyouNenunTaiun, nenunCenterX, taiunCenterX, drawTopKan, dircUp, strInyou);
                }
            }
            if (bDispGetuun)
            {
                //陰陽(月運→年運）
                //-------------------               
                if (bInyouGetuunNenun && bDispNenun)//月運 - 年運
                {
                    DrawLine(idxInyouGetuunNenun, getuunCenterX, nenunCenterX, drawTopKan, dircUp);
                    DrawString(idxInyouGetuunNenun, getuunCenterX, nenunCenterX, drawTopKan, dircUp, strInyou);
                }
                //陰陽(月運→大運）
                //-------------------               
                if (bInyouGetuunTaiun && bDispTaiun)//月運 - 大運
                {
                    DrawLine(idxInyouGetuunTaiun, getuunCenterX, taiunCenterX, drawTopKan, dircUp);
                    DrawString(idxInyouGetuunTaiun, getuunCenterX, taiunCenterX, drawTopKan, dircUp, strInyou);
                }

            }

            //陰陽(大運→＊）
            //-------------------               
            if (bDispTaiun)
            {
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
            }

            //陰陽(年運→＊）
            //-------------------               
            if (bDispNenun)
            {
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
            }
            //陰陽(月運→＊）
            //-------------------               
            if (bDispGetuun)
            {
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
            if (bDispTaiun && bDispNenun)
            {
                if (bKangouNenunTaiun)//年運 - 大運
                {
                    DrawLine(idxKangouNenunTaiun, nenunCenterX, taiunCenterX, drawTopKan, dircUp);
                    DrawString(idxKangouNenunTaiun, nenunCenterX, taiunCenterX, drawTopKan, dircUp, strKangou);
                }
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
            if (bDispTaiun)
            {
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
            }
            //干合(年運→＊）
            if (bDispNenun)
            {
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
            }
            //干合(月運→＊）
            //-------------------
            if (bDispGetuun)
            {
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
            if (bDispTaiun && bDispNenun)
            {
                if (bNanasatuNenunTaiun)//年運 - 大運
                {
                    NanasatuDraw nanasatuDraw = new NanasatuDraw(nenunCenterX, taiunCenterX, idxNanasatuPointNenunTaiun);
                    DrawLineNanasatu(idxNanasatuNenunTaiun, nanasatuDraw,  drawTopKan, dircUp);
                    DrawString(idxNanasatuNenunTaiun, nenunCenterX, taiunCenterX, drawTopKan, dircUp, strNanasatu);
                }
            }
            if (bDispGetuun)
            {
                if (bNanasatuGetuunNenun)//月運 - 年運
                {
                    NanasatuDraw nanasatuDraw = new NanasatuDraw(getuunCenterX, nenunCenterX, idxNanasatuPointGetuunNenun);
                    DrawLineNanasatu(idxNanasatuGetuunNenun, nanasatuDraw, drawTopKan, dircUp);
                    DrawString(idxNanasatuGetuunNenun, getuunCenterX, nenunCenterX, drawTopKan, dircUp, strNanasatu);
                }
                if (bNanasatuGetuunTaiun)//月運 - 大運
                {
                    NanasatuDraw nanasatuDraw = new NanasatuDraw(getuunCenterX, taiunCenterX, idxNanasatuPointGetuunTaiun);
                    DrawLineNanasatu(idxNanasatuGetuunTaiun, nanasatuDraw, drawTopKan, dircUp);
                    DrawString(idxNanasatuGetuunTaiun, getuunCenterX, taiunCenterX, drawTopKan, dircUp, strNanasatu);
                }
            }
            //七殺(大運→＊）
            //-------------------
            if (bDispTaiun)
            {
                if (bNanasatuTaiunNiti)//大運 - 日
                {
                    NanasatuDraw nanasatuDraw = new NanasatuDraw(taiunCenterX, nikkansiCenterX, idxNanasatuPointTaiunNiti);
                    DrawLineNanasatu(idxNanasatuTaiunNiti, nanasatuDraw, drawTopKan, dircUp);
                    DrawString(idxNanasatuTaiunNiti, taiunCenterX, nikkansiCenterX, drawTopKan, dircUp, strNanasatu);
                }
                if (bNanasatuTaiunGetu)//大運 - 月
                {
                    NanasatuDraw nanasatuDraw = new NanasatuDraw(taiunCenterX, gekkansiCenterX, idxNanasatuPointTaiunGetu);
                    DrawLineNanasatu(idxNanasatuTaiunGetu, nanasatuDraw, drawTopKan, dircUp);
                    DrawString(idxNanasatuTaiunGetu, taiunCenterX, gekkansiCenterX, drawTopKan, dircUp, strNanasatu);
                }
                if (bNanasatuTaiunNen)//大運 - 年
                {
                    NanasatuDraw nanasatuDraw = new NanasatuDraw(taiunCenterX, nenkansiCenterX, idxNanasatuPointTaiunNen);
                    DrawLineNanasatu(idxNanasatuTaiunNen, nanasatuDraw, drawTopKan, dircUp);
                    DrawString(idxNanasatuTaiunNen, taiunCenterX, nenkansiCenterX, drawTopKan, dircUp, strNanasatu);
                }
            }
            //七殺(年運→＊）
            //-------------------
            if (bDispNenun)
            {
                if (bNanasatuNenunNiti)//年運 - 日
                {
                    NanasatuDraw nanasatuDraw = new NanasatuDraw(nenunCenterX, nikkansiCenterX, idxNanasatuPointNenunNiti);
                    DrawLineNanasatu(idxNanasatuNenunNiti, nanasatuDraw, drawTopKan, dircUp);
                    DrawString(idxNanasatuNenunNiti, nenunCenterX, nikkansiCenterX, drawTopKan, dircUp, strNanasatu);
                }
                if (bNanasatuNenunGetu)//年運 - 月
                {
                    NanasatuDraw nanasatuDraw = new NanasatuDraw(nenunCenterX, gekkansiCenterX, idxNanasatuPointNenunGetu);
                    DrawLineNanasatu(idxNanasatuNenunGetu, nanasatuDraw, drawTopKan, dircUp);
                    DrawString(idxNanasatuNenunGetu, nenunCenterX, gekkansiCenterX, drawTopKan, dircUp, strNanasatu);
                }
                if (bNanasatuNenunNen)//年運 - 年
                {
                    NanasatuDraw nanasatuDraw = new NanasatuDraw(nenunCenterX, nenkansiCenterX, idxNanasatuPointNenunNen);
                    DrawLineNanasatu(idxNanasatuNenunNen, nanasatuDraw, drawTopKan, dircUp);
                    DrawString(idxNanasatuNenunNen, nenunCenterX, nenkansiCenterX, drawTopKan, dircUp, strNanasatu);

                }
            }
            //七殺(月運→＊）
            //-------------------
            if (bDispGetuun)
            {
                if (bNanasatuGetuunNiti)//月運 - 日
                {
                    NanasatuDraw nanasatuDraw = new NanasatuDraw(getuunCenterX, nikkansiCenterX, idxNanasatuPointGetuunNiti);
                    DrawLineNanasatu(idxNanasatuGetuunNiti, nanasatuDraw, drawTopKan, dircUp);
                    DrawString(idxNanasatuGetuunNiti, getuunCenterX, nikkansiCenterX, drawTopKan, dircUp, strNanasatu);
                }
                if (bNanasatuGetuunGetu)//月運 - 月
                {
                    NanasatuDraw nanasatuDraw = new NanasatuDraw(getuunCenterX, gekkansiCenterX, idxNanasatuPointGetuunGetu);
                    DrawLineNanasatu(idxNanasatuGetuunGetu, nanasatuDraw, drawTopKan, dircUp);
                    DrawString(idxNanasatuGetuunGetu, getuunCenterX, gekkansiCenterX, drawTopKan, dircUp, strNanasatu);
                }
                if (bNanasatuGetuunNen)//月運 - 年
                {
                    NanasatuDraw nanasatuDraw = new NanasatuDraw(getuunCenterX, nenkansiCenterX, idxNanasatuPointGetuunNen);
                    DrawLineNanasatu(idxNanasatuGetuunNen, nanasatuDraw, drawTopKan, dircUp);
                    DrawString(idxNanasatuGetuunNen, getuunCenterX, nenkansiCenterX, drawTopKan, dircUp, strNanasatu);

                }
            }


            //合法・散法,干合
            //-------------------
            GouhouSannpouResult[] gouhouSanpouNenunTaiun = person.GetGouhouSanpouEx(taiunKansi, nenunKansi, taiunKansi, nenunKansi);
            GouhouSannpouResult[] gouhouSanpouGetuunNenun = person.GetGouhouSanpouEx(nenunKansi,getuunKansi, nenunKansi, getuunKansi);
            GouhouSannpouResult[] gouhouSanpouGetuunTaiun = person.GetGouhouSanpouEx(taiunKansi, getuunKansi, taiunKansi, getuunKansi);
            int idx = 0;
            int dircDownOfsX = -6;
            if (bDispTaiun && bDispNenun)
            {
                if (gouhouSanpouNenunTaiun != null && gouhouSanpouNenunTaiun.Length > 0)//年運 - 大運
                {
                    idx = SetMatrixDown(true, Const.bitFlgNenun | Const.bitFlgTaiun, (Const.bitFlgNenun | Const.bitFlgTaiun));
                    DrawLine(idx, nenunCenterX, taiunCenterX, drawBottomStartY, dircDown, dircDownOfsX);
                    DrawString(idx, nenunCenterX, taiunCenterX, drawBottomStartY, dircDown, gouhouSanpouNenunTaiun);

                }
            }
            if (bDispGetuun)
            {
                if ( bDispNenun)
                {
                    if (gouhouSanpouGetuunNenun != null && gouhouSanpouGetuunNenun.Length > 0)//月運 - 年運
                    {
                        idx = SetMatrixDown(true, Const.bitFlgGetuun | Const.bitFlgNenun, (Const.bitFlgGetuun | Const.bitFlgNenun));
                        DrawLine(idx, getuunCenterX, nenunCenterX, drawBottomStartY, dircDown, dircDownOfsX);
                        DrawString(idx, getuunCenterX, nenunCenterX, drawBottomStartY, dircDown, gouhouSanpouGetuunNenun);

                    }
                }
                if (bDispTaiun)
                {
                    if (gouhouSanpouGetuunTaiun != null && gouhouSanpouGetuunTaiun.Length > 0)//月運 - 大運
                    {
                        idx = SetMatrixDown(true, Const.bitFlgGetuun | Const.bitFlgNenun | Const.bitFlgTaiun, (Const.bitFlgGetuun | Const.bitFlgTaiun));
                        DrawLine(idx, getuunCenterX, taiunCenterX, drawBottomStartY, dircDown, dircDownOfsX);
                        DrawString(idx, getuunCenterX, taiunCenterX, drawBottomStartY, dircDown, gouhouSanpouGetuunTaiun);

                    }
                }
            }
            //大運 →＊
            if (bDispTaiun)
            {
                GouhouSannpouResult[] gouhouSanpouTaiunNiti = person.GetGouhouSanpouEx(taiunKansi, person.nikkansi, taiunKansi, nenunKansi);
                GouhouSannpouResult[] gouhouSanpouTaiunGetu = person.GetGouhouSanpouEx(taiunKansi, person.gekkansi, taiunKansi, nenunKansi);
                GouhouSannpouResult[] gouhouSanpouTaiunNen = person.GetGouhouSanpouEx(taiunKansi, person.nenkansi, taiunKansi, nenunKansi);
                chkBitFlg = Const.bitFlgTaiun | bitFlgNitiGetuNen;

                if (gouhouSanpouTaiunNiti != null && gouhouSanpouTaiunNiti.Length > 0)//大運 - 日
                {
                    idx = SetMatrixDown(true, chkBitFlg, (Const.bitFlgTaiun | Const.bitFlgNiti));
                    DrawLine(idx, taiunCenterX, nikkansiCenterX, drawBottomStartY, dircDown, dircDownOfsX);
                    DrawString(idx, taiunCenterX, nikkansiCenterX, drawBottomStartY, dircDown, gouhouSanpouTaiunNiti);

                }
                if (gouhouSanpouTaiunGetu != null && gouhouSanpouTaiunGetu.Length > 0)//大運 - 月
                {
                    idx = SetMatrixDown(true, chkBitFlg, (Const.bitFlgTaiun | Const.bitFlgGetu));
                    DrawLine(idx, taiunCenterX, gekkansiCenterX, drawBottomStartY, dircDown, dircDownOfsX);
                    DrawString(idx, taiunCenterX, gekkansiCenterX, drawBottomStartY, dircDown, gouhouSanpouTaiunGetu);
                }

                if (gouhouSanpouTaiunNen != null && gouhouSanpouTaiunNen.Length > 0)//大運 - 年
                {
                    idx = SetMatrixDown(true, chkBitFlg, (Const.bitFlgTaiun | Const.bitFlgNen));
                    DrawLine(idx, taiunCenterX, nenkansiCenterX, drawBottomStartY, dircDown, dircDownOfsX);
                    DrawString(idx, taiunCenterX, nenkansiCenterX, drawBottomStartY, dircDown, gouhouSanpouTaiunNen);
                }
            }
            //年運 →＊
            if (bDispNenun)
            {
                GouhouSannpouResult[] gouhouSanpouNenunNiti = person.GetGouhouSanpouEx(nenunKansi, person.nikkansi, taiunKansi, nenunKansi);
                GouhouSannpouResult[] gouhouSanpouNenunGetu = person.GetGouhouSanpouEx(nenunKansi, person.gekkansi, taiunKansi, nenunKansi);
                GouhouSannpouResult[] gouhouSanpouNenunNen = person.GetGouhouSanpouEx(nenunKansi, person.nenkansi, taiunKansi, nenunKansi);
                chkBitFlg = Const.bitFlgTaiun | bitFlgNitiGetuNen;

                if (gouhouSanpouNenunNiti != null && gouhouSanpouNenunNiti.Length > 0)//年運 - 日
                {
                    idx = SetMatrixDown(true, chkBitFlg, (Const.bitFlgNenun | Const.bitFlgNiti));
                    DrawLine(idx, nenunCenterX, nikkansiCenterX, drawBottomStartY, dircDown, dircDownOfsX);
                    DrawString(idx, nenunCenterX, nikkansiCenterX, drawBottomStartY, dircDown, gouhouSanpouNenunNiti);
                }
                if (gouhouSanpouNenunGetu != null && gouhouSanpouNenunGetu.Length > 0)//年運 - 月
                {
                    idx = SetMatrixDown(true, chkBitFlg, (Const.bitFlgNenun | Const.bitFlgGetu));
                    DrawLine(idx, nenunCenterX, gekkansiCenterX, drawBottomStartY, dircDown, dircDownOfsX);
                    DrawString(idx, nenunCenterX, gekkansiCenterX, drawBottomStartY, dircDown, gouhouSanpouNenunGetu);
                }

                if (gouhouSanpouNenunNen != null && gouhouSanpouNenunNen.Length > 0)//年運 - 年
                {
                    idx = SetMatrixDown(true, chkBitFlg, (Const.bitFlgNenun | Const.bitFlgNen));
                    DrawLine(idx, nenunCenterX, nenkansiCenterX, drawBottomStartY, dircDown, dircDownOfsX);
                    DrawString(idx, nenunCenterX, nenkansiCenterX, drawBottomStartY, dircDown, gouhouSanpouNenunNen);
                }
            }

            //月運 →＊
            if (bDispGetuun)
            {
                GouhouSannpouResult[] gouhouSanpouGetuunNiti = person.GetGouhouSanpouEx(getuunKansi, person.nikkansi, taiunKansi, nenunKansi);
                GouhouSannpouResult[] gouhouSanpouuGetuunGetu = person.GetGouhouSanpouEx(getuunKansi, person.gekkansi, taiunKansi, nenunKansi);
                GouhouSannpouResult[] gouhouSanpouuGetuunNen = person.GetGouhouSanpouEx(getuunKansi, person.nenkansi, taiunKansi, nenunKansi);
                chkBitFlg = Const.bitFlgGetuun | Const.bitFlgNenun | Const.bitFlgTaiun | bitFlgNitiGetuNen;

                if (gouhouSanpouGetuunNiti != null && gouhouSanpouGetuunNiti.Length > 0)//月運 - 日
                {
                    idx = SetMatrixDown(true, chkBitFlg, (Const.bitFlgGetuun | Const.bitFlgNiti));
                    DrawLine(idx, getuunCenterX, nikkansiCenterX, drawBottomStartY, dircDown, dircDownOfsX);
                    DrawString(idx, getuunCenterX, nikkansiCenterX, drawBottomStartY, dircDown, gouhouSanpouGetuunNiti);
                }
                if (gouhouSanpouuGetuunGetu != null && gouhouSanpouuGetuunGetu.Length > 0)//月運 - 月
                {
                    idx = SetMatrixDown(true, chkBitFlg, (Const.bitFlgGetuun | Const.bitFlgGetu));
                    DrawLine(idx, getuunCenterX, gekkansiCenterX, drawBottomStartY, dircDown, dircDownOfsX);
                    DrawString(idx, getuunCenterX, gekkansiCenterX, drawBottomStartY, dircDown, gouhouSanpouuGetuunGetu);
                }

                if (gouhouSanpouuGetuunNen != null && gouhouSanpouuGetuunNen.Length > 0)//月運 - 年
                {
                    idx = SetMatrixDown(true, chkBitFlg, (Const.bitFlgGetuun | Const.bitFlgNen));
                    DrawLine(idx, getuunCenterX, nenkansiCenterX, drawBottomStartY, dircDown, dircDownOfsX);
                    DrawString(idx, getuunCenterX, nenkansiCenterX, drawBottomStartY, dircDown, gouhouSanpouuGetuunNen);
                }
            }
            //---------------------------------------
            // 三合会局
            //---------------------------------------

            if (bDispSangouKaikyoku)
            {
                foreach (var item in lstSangouKaikyoku)
                {
                    int[] posX = new int[3];
                    int index = 0;
                    //月運がHitしているが月運表示OFFの場合は表示しない
                    if ((item.hitItemBit & Const.bitFlgGetuun)!=0 && !bDispGetuun)
                    {
                        continue;
                    }
                    if ((item.hitItemBit & Const.bitFlgGetuun) != 0) posX[index++] = getuunCenterX;
                    if ((item.hitItemBit & Const.bitFlgNenun) != 0) posX[index++] = nenunCenterX;
                    if ((item.hitItemBit & Const.bitFlgTaiun) != 0) posX[index++] = taiunCenterX;
                    if ((item.hitItemBit & Const.bitFlgNiti) != 0) posX[index++] = nikkansiCenterX;
                    if ((item.hitItemBit & Const.bitFlgGetu) != 0) posX[index++] = gekkansiCenterX;
                    if ((item.hitItemBit & Const.bitFlgNen) != 0) posX[index++] = nenkansiCenterX;

                    idx = SetMatrixDown(true, bitFlgAll, bitFlgAll);
                    dircDownOfsX += 4;
                    DrawLine3Point(idx, posX, drawBottomStartY, dircDown, dircDownOfsX, Color.Red);
                    DrawString(idx, posX[0], posX[2], drawBottomStartY, dircDown, "三合会局", Brushes.Red);
                }
                //---------------------------------------
                // 方三位
                //---------------------------------------
                foreach (var item in lstHouSani)
                {
                    int[] posX = new int[3];
                    int index = 0;
                    //月運がHitしているが月運表示OFFの場合は表示しない
                    if ((item.hitItemBit & Const.bitFlgGetuun) != 0 && !bDispGetuun)
                    {
                        continue;
                    }
                    if ((item.hitItemBit & Const.bitFlgGetuun) != 0) posX[index++] = getuunCenterX;
                    if ((item.hitItemBit & Const.bitFlgNenun) != 0) posX[index++] = nenunCenterX;
                    if ((item.hitItemBit & Const.bitFlgTaiun) != 0) posX[index++] = taiunCenterX;
                    if ((item.hitItemBit & Const.bitFlgNiti) != 0) posX[index++] = nikkansiCenterX;
                    if ((item.hitItemBit & Const.bitFlgGetu) != 0) posX[index++] = gekkansiCenterX;
                    if ((item.hitItemBit & Const.bitFlgNen) != 0) posX[index++] = nenkansiCenterX;

                    idx = SetMatrixDown(true, bitFlgAll, bitFlgAll);
                    dircDownOfsX += 4;
                    DrawLine3Point(idx, posX, drawBottomStartY, dircDown, dircDownOfsX, Color.Blue);
                    DrawString(idx, posX[0], posX[2], drawBottomStartY, dircDown, "方三位", Brushes.Blue);
                }
            }


        }
    }

}
