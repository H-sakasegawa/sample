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
        //桑原さん提供の節入り日テーブルには、節入り日しか情報がない
        //節入り日テーブルの最初の年度２月の基準干支を求めるための基準
        int calcBaseYear = 1936;
        int calcBaseMonth = 2;
        int calcBaseSetuiribi = 5;
        int calcBaseNenkansi = 13;
        int calcBaseGekkansi = 27;
        int calcBaseNikkansiSanshutuSu = 49;

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

        public void GetBaseSetuiribiData(ref int baseYear,
                                          ref int baseMonth,
                                          ref int baseDay,
                                          ref int baseNenkansi,
                                          ref int baseGekkansi,
                                          ref int baseNikkansiSanshutuSu
            )
        {
            var value = dicSetuiribiTbl.ToArray()[0];

            YearItem yearItem = value.Value;
            baseYear = yearItem.year;
            var item = yearItem.dicSetuiribi.ToArray()[0];
            baseMonth = item.Key;
            baseDay = item.Value;
            baseNenkansi = CalcNenkansi( baseYear, baseMonth);
            baseGekkansi = CalcGekkansi(baseYear, baseMonth);
            baseNikkansiSanshutuSu = CalcNikkansiSanshutuSu(baseYear, baseMonth, baseDay);

            /*
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"G:\Temp\test.csv",false) )
                        {
                            for (int y = 1937; y >= baseYear; y--)
                            {
                                int year = y;

                                int m = 2;
                                for (int i = 0; i < 12; i++)
                                {
                                    if (m == 0)
                                    {
                                        m = 12;
                                        year = y - 1;
                                    }

                                    var baseNenkansiWk = CalcNenkansi(year, m);
                                    var baseGekkansiWk = CalcGekkansi(year, m);
                                    var baseNikkansiSanshutuSuWk = CalcNikkansiSanshutuSu(year, m);

                                    sw.WriteLine(string.Format("{0}/{1}, {2},{3},{4}", year, m, baseNenkansiWk, baseGekkansiWk, baseNikkansiSanshutuSuWk));
                                    m--;
                                }
                            }
                        }
            */
        }
        private int CalcNenkansi(int targetYear, int targetMonth)
        {
            int nenkansi = calcBaseNenkansi;
            if (calcBaseYear > targetYear)
            {
                for (int i = calcBaseYear - 1; i >= targetYear; i--)
                {
                    nenkansi--;
                    if (nenkansi == 0) nenkansi = 60;
                }
                if (targetMonth == 1)
                {
                    nenkansi--;
                    if (nenkansi == 0) nenkansi = 60;
                }
            }
            else
            {
                for (int i = calcBaseYear+1; i <= targetYear; i++)
                {
                    nenkansi++;
                    if (nenkansi == 61) nenkansi = 1;
                }
                if (targetMonth == 1)
                {
                    nenkansi--;
                    if (nenkansi == 0) nenkansi = 60;
                }
            }
            return nenkansi;
        }

        private int CalcGekkansi(int targetYear, int targetMonth)
        {
            int gekkansi = calcBaseGekkansi;
            DateTime dateFrom = new System.DateTime(calcBaseYear, calcBaseMonth, 1);
            DateTime dateTo = new System.DateTime(targetYear, targetMonth, 1);

            if(dateFrom>dateTo)
            {
                int monthNum = Common.GetElapsedMonths(dateTo, dateFrom);

                for (int i = 0; i < monthNum; i++)
                {
                    gekkansi--;
                    if (gekkansi == 0) gekkansi = 60;
                }
            }
            else
            {
                int monthNum = Common.GetElapsedMonths(dateFrom, dateTo);

                for (int i = 0; i < monthNum; i++)
                {
                    gekkansi++;
                    if (gekkansi == 61) gekkansi = 1;
                }
            }
                
 
            return gekkansi;
        }

        private int CalcNikkansiSanshutuSu(int targetYear, int targetMonth, int targetDay)
        {
            int nikkansiSanshutu = calcBaseNikkansiSanshutuSu;
            DateTime dateFrom = new System.DateTime(calcBaseYear, calcBaseMonth, calcBaseSetuiribi);
            DateTime dateTo = new System.DateTime(targetYear, targetMonth, targetDay);

            int dayNum = 0;
            if (dateFrom > dateTo)
            {
                dayNum = (int)(dateFrom - dateTo).TotalDays;
                for (int i = 0; i < dayNum; i++)
                {
                    nikkansiSanshutu--;
                    if (nikkansiSanshutu == 0) nikkansiSanshutu = 60;
                }
            }
            else
            {
                dayNum = (int)(dateTo - dateFrom).TotalDays;
                for (int i = 0; i < dayNum; i++)
                {
                    nikkansiSanshutu++;
                    if (nikkansiSanshutu == 61) nikkansiSanshutu = 1;
                }
            }
            return nikkansiSanshutu;
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
            if (value >= 61)
            {
                value = value % 60;
            }
            return value;
        }
        //月干支番号取得
        public int GetGekkansiNo(int year, int month, int day)
        {
            int value = 0;
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

        //入力された生年月日に紐付く月の節入り日からの経過日数
        public int CalcDayCountFromSetuiribi(int year, int month, int day)
        {
            int orgYear = year;
            int orgMonth = month;
            int orgDay = day;

            CorrectMonthBySetuiribi(ref year, ref month, day);
            var yearItem = dicSetuiribiTbl[year];
            int seturibi = yearItem.dicSetuiribi[month];

            DateTime dateFrom = new DateTime(year, month, seturibi);
            DateTime dateTo = new DateTime(orgYear, orgMonth, orgDay);

            return (int)(dateTo - dateFrom).TotalDays;

        }

        //入力された生年月日から、紐付く月最終日までの日数
        public int CalcDayCountBirthdayToLastMonthDay(int year, int month, int day)
        {
            int orgYear = year;
            int orgMonth = month;
            int orgDay = day;

            CorrectMonthBySetuiribi(ref year, ref month, day);

            int toYear = year;
            int toMonth = month+1;
            if (toMonth > 12)
            {
                toYear++;
                toMonth = 1;
            }

            var yearItem = dicSetuiribiTbl[toYear];
            int seturibi = yearItem.dicSetuiribi[toMonth];

            //次の月の節入り日の前日
            DateTime dateFrom = new DateTime(orgYear, orgMonth, orgDay);
            DateTime dateTo = new DateTime(toYear, toMonth, seturibi-1);

            return (int)(dateTo - dateFrom).TotalDays+1;

        }

        ////入力された生年月日に紐付く節入り日から生年月日までの日数
        //public int CalcDayCountSetuiribiToBirthday(int year, int month, int day)
        //{
        //    int orgYear = year;
        //    int orgMonth = month;
        //    int orgDay = day;

        //    CorrectMonthBySetuiribi(ref year, ref month, day);
        //    var yearItem = dicSetuiribiTbl[year];
        //    int seturibi = yearItem.dicSetuiribi[month];


        //    DateTime dateFrom = new DateTime(year, month, day);
        //    DateTime dateTo = new DateTime(orgYear, orgMonth, orgDay);

        //    return (int)(dateTo - dateFrom).TotalDays;

        //}


        private int CorrectMonthBySetuiribi(ref int year, ref int month, int day)
        {
            if (dicSetuiribiTbl.ContainsKey(year))
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

            return -1;
        }

        /// <summary>
        /// 読み込んだ節入り日テーブルに指定年度の情報がふくまれているか？
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public bool IsContainsYear( int year )
        {
            return dicSetuiribiTbl.ContainsKey(year);
        }

    }
}
