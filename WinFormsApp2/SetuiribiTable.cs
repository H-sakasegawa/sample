using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    //節入り日データテーブル管理クラス
    class SetuiribiTable
    {

        /// <summary>
        /// 年毎データ
        /// </summary>
        class YearItem
        {
            public int year =0;
            public Dictionary<int, int> dicSetuiribi = new Dictionary<int, int>();

        }

        int baseYear = 0;
        int baseMonth = 0;
        int baseDay = 0;
        int baseNenkansiNo = 0;
        int baseGekkansiNo = 0;
        int baseNikkansiNo = 0;
        List<int> lstColMonth = new List<int>();
        Dictionary<int, YearItem> dicSetuiribiTbl = new Dictionary<int, YearItem>();
        
        public SetuiribiTable()
        {
        }

        public void Init(int _baseYear, int _baseMonth, int _baseDay, 
                        int _baseNenkansiNo, int _baseGekkansiNo, int _baseNikkansiNo)
        {
            baseYear = _baseYear;
            baseMonth = _baseMonth;
            baseDay = _baseDay;
            baseNenkansiNo = _baseNenkansiNo;
            baseGekkansiNo = _baseGekkansiNo;
            baseNikkansiNo = _baseNikkansiNo;
        }


        /// <summary>
        /// 節入り日テーブル（Excel）読み込み
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public int ReadTable(string filePath)
        { 
            var version = "xls";
            var workbook = ExcelReader.GetWorkbook(filePath, version);
            if(workbook == null)
            {
                return -1;
            }

            var sheet = workbook.GetSheetAt(0);

            //1行目（月）取得
            for (int idxCol = 0; idxCol < 12; idxCol++)
            {
                var value = ExcelReader.CellValue(sheet, 0, idxCol + 1);
                lstColMonth.Add(int.Parse(value));
            }
            //2行目以降、年毎の節入り日取得
            int iRow = 1;
            while(true)
            {
                YearItem item = new YearItem();
                //年
                var sYear = ExcelReader.CellValue(sheet, iRow, 0);
                if( sYear=="")
                {
                    break;
                }
                item.year = int.Parse(sYear);

                //節入り日
                for(int idxCol = 0; idxCol < 12; idxCol++)
                {
                    int day = int.Parse(ExcelReader.CellValue(sheet, iRow, idxCol+1));

                    item.dicSetuiribi.Add(lstColMonth[idxCol], day);

                }

                dicSetuiribiTbl.Add(item.year, item);
                iRow++;
            }
            return 0;
        }

        //年干支番号取得
        public int GetNenKansiNo(int year, int month, int day)
        {
            int value = 0;
            if (month == 1)
            {
                //１月は、前年のテーブル項目となる
                value = ((year - 1) - baseYear) + baseNenkansiNo;
            }
            else
            {   //2月～12月
                value = (year - baseYear) + baseNenkansiNo;
            }
            if (value >= 61) value -= 60;

            return value;
        }
        //月干支番号取得
        public int GetGekkansiNo(int year, int month, int day)
        {
            int value = 0;
            //if (month == 1)
            //{
            //    var yearItem = dicSetuiribiTbl[year];
            //    int seturibi = yearItem.dicSetuiribi[month];
            //    //１月の節入り日前の日は、前年の１２月の月干支番号となる
            //    if (day < seturibi)
            //    {
            //        year = year - 1;
            //        month = 12;
            //    }
            //}
            //else
            //{
            //    var yearItem = dicSetuiribiTbl[year];
            //    int seturibi = yearItem.dicSetuiribi[month];
            //    if (day < seturibi)
            //    {
            //        month = month -1;
            //    }
            // }
            month = CorrectMonthBySetuiribi(ref year, ref month, day);

            DateTime dateFrom = new System.DateTime(baseYear, baseMonth, 1);
            DateTime dateTo = new System.DateTime(year, month, 1);

            int monthNum = Common.GetElapsedMonths(dateFrom, dateTo);
            value = (int)monthNum % 60 + baseGekkansiNo;
            if (value >= 61) value -= 60;

            return value;
        }

        //日干支番号取得
        public int GetNikkansiNo(int year, int month, int day)
        {
            int value = 0;
            DateTime dateFrom = new System.DateTime(baseYear, baseMonth, baseDay);
            DateTime dateTo = new System.DateTime(year, month, day);

            int dayNum = (int)(dateTo - dateFrom).TotalDays;
            value = (dayNum % 60) + baseNikkansiNo;
            if (value >= 61) value -= 60;

            return value;
        }

        //生年月日に日も付く月の節入り日からの経過日数
        public int CalcPassedDayFromSetuiribi(int year, int month, int day)
        {
            int correctMonth = CorrectMonthBySetuiribi(ref year, ref month, day);
            var yearItem = dicSetuiribiTbl[year];
            int seturibi = yearItem.dicSetuiribi[correctMonth];

            DateTime dateFrom = new DateTime(year, correctMonth, seturibi);
            DateTime dateTo = new DateTime(year, month, day);

            return (int)(dateTo - dateFrom).TotalDays;

        }

        private int CorrectMonthBySetuiribi(ref int year, ref int month, int day)
        {
            if (month == 1)
            {
                var yearItem = dicSetuiribiTbl[year];
                int seturibi = yearItem.dicSetuiribi[month];
                //１月の節入り日前の日は、前年の１２月の月干支番号となる
                if (day < seturibi)
                {
                    year = year - 1;
                    month = 12;
                }
            }
            else
            {
                var yearItem = dicSetuiribiTbl[year];
                int seturibi = yearItem.dicSetuiribi[month];
                if (day < seturibi)
                {
                    month = month - 1;
                }
            }
            return month;
        }

    }
}
