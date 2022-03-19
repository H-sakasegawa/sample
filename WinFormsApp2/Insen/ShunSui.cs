using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    ///春水
    /// </summary>
    class ShunSui
    {

        public static string GetShunSuii(Person person)
        {
       
            TableMng tblMng = TableMng.GetTblManage();

            string nitiKan = person.nikkansi.kan;
            string getuSi = person.gekkansi.si;
            //string nenKan = person.nenkansi.kan;

            if (nitiKan == "壬" || nitiKan == "癸")
            {
                if(getuSi == "寅" || getuSi == "卯" || getuSi == "辰")
                {
                    return "春水";
                }

            }
            return "";
        }
    }

}
