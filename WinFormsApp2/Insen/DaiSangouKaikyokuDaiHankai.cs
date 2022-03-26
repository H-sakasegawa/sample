using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    ///大三合会局、大半会
    /// </summary>
    class DaiSangouKaikyokuDaiHankai
    {

        public static string GetDaiSangouKaikyokuDaiHankai(Person person)
        {
            TableMng tblMng = TableMng.GetTblManage();

            Kansi[] aryKansi = new Kansi[] { person.nikkansi, person.gekkansi, person.nenkansi };
            string sub = "";

            int cnt = 0;
            //日-月
            if (tblMng.kansiTbl.GetMinNoDistance(aryKansi[0] ,aryKansi[1]) == 20)
            {
                sub = "日-月";
                cnt++;
            }
            //日-年
            if (tblMng.kansiTbl.GetMinNoDistance(aryKansi[0], aryKansi[2]) == 20)
            {
                sub = "日-年";
                cnt++;
            }
            //月-年
            if (tblMng.kansiTbl.GetMinNoDistance(aryKansi[1], aryKansi[2]) == 20)
            {
                sub = "月-年";
                cnt++;
            }


            if (cnt == 3) return "大三合会局";
            if (cnt == 1) return string.Format("大半会({0})", sub);

            return null;
        }
     
    }

}
