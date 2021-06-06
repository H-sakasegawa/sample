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
    }
}
