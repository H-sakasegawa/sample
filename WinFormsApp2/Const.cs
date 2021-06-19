using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    class Const
    {
        //-----------------------------------
        // 月運、年運、大運、日干支、月干支、年干支　識別フラグ
        //-----------------------------------
        public const int bitFlgGetuun = 0x40;
        public const int bitFlgNenun = 0x20;
        public const int bitFlgTaiun = 0x10;

        public const int bitFlgNiti = 0x04;
        public const int bitFlgGetu = 0x02;
        public const int bitFlgNen = 0x01;


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
