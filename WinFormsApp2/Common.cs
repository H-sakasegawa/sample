using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    class Common
    {

        public static int GetElapsedMonths(DateTime baseDay, DateTime day)
        {
            if (day < baseDay)
                // 日付が基準日より前の場合は例外とする
                throw new ArgumentException();

            // 経過月数を求める(満月数を考慮しない単純計算)
            var elapsedMonths = (day.Year - baseDay.Year) * 12 + (day.Month - baseDay.Month);

            if (baseDay.Day <= day.Day)
                // baseDayの日部分がdayの日部分以上の場合は、その月を満了しているとみなす
                // (例:1月30日→3月30日以降の場合は満(3-1)ヶ月)
                return elapsedMonths;
            else if (day.Day == DateTime.DaysInMonth(day.Year, day.Month) && day.Day <= baseDay.Day)
                // baseDayの日部分がdayの表す月の末日以降の場合は、その月を満了しているとみなす
                // (例:1月30日→2月28日(平年2月末日)/2月29日(閏年2月末日)以降の場合は満(2-1)ヶ月)
                return elapsedMonths;
            else
                // それ以外の場合は、その月を満了していないとみなす
                // (例:1月30日→3月29日以前の場合は(3-1)ヶ月未満、よって満(3-1-1)ヶ月)
                return elapsedMonths - 1;
        }
    }
}
