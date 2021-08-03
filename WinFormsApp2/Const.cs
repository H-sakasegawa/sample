using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WinFormsApp2
{
    class Const
    {
        //-----------------------------------
        // 月運、年運、大運、日干支、月干支、年干支　識別フラグ
        //-----------------------------------
        /// <summary>
        /// 月運 識別フラグ
        /// </summary>
        public const int bitFlgGetuun = 0x40;
        /// <summary>
        /// 年運 識別フラグ
        /// </summary>
        public const int bitFlgNenun = 0x20;
        /// <summary>
        /// 大運 識別フラグ
        /// </summary>
        public const int bitFlgTaiun = 0x10;

        /// <summary>
        /// 日干支 識別フラグ
        /// </summary>
        public const int bitFlgNiti = 0x04;
        /// <summary>
        /// 月干支 識別フラグ
        /// </summary>
        public const int bitFlgGetu = 0x02;
        /// <summary>
        /// 年干支 識別フラグ
        /// </summary>
        public const int bitFlgNen = 0x01;


        public const double dKansiHeightRate = 1.5;

        //守護神表示カラー
        public static Brush brusShugosin = Brushes.Yellow;
        //忌神表示カラー
        public static Brush brusImigami = Brushes.LightGray;

        public static Color colorShugosin = Color.Yellow;
        public static Color colorImigami = Color.LightGray;

        /// <summary>
        /// 干支項目の識別ID
        /// </summary>
        public enum enumKansiItemID
        {
            None = -1,
            GETUUN = 0,
            NENUN,
            TAIUN,
            NIKKANSI,
            GEKKANSI,
            NENKANSI,
        }


        public enum enumOyakoID
        {
            None = -1,
            Myself, //自身
            Father, //父
            Mother, //母
            Child,  //子
            Spouse  //配偶者
        }


        public const string sKangou = "干合";
        public const string sInyou = "陰陽";
        public const string sNanasatu = "七殺";

        public const string sOkidou = "冲動";

        public const string sGogyouMoku = "木";
        public const string sGogyouKa = "火";
        public const string sGogyouDo = "土";
        public const string sGogyouKin = "金";
        public const string sGogyouSui = "水";
    }
}
