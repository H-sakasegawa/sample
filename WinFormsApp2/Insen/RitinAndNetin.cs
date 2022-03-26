using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    ///律音と納音
    /// </summary>
    class RitinAndNetin
    {

        public static string GetRitinAndNetin(Person person)
        {

            string result = null;

            TableMng tblMng = TableMng.GetTblManage();

            string nitiKan = person.nikkansi.kan;

            Kansi[] aryKansi = new Kansi[] { person.nikkansi, person.gekkansi, person.nenkansi };
            string[] sPos = new string[] { "日", "月", "年" };

            //律音
            for (int i = 0; i < aryKansi.Length - 1; i++)
            {
                for (int j = i + 1; j < aryKansi.Length; j++)
                {
                    result = person.GetRittin(aryKansi[i], aryKansi[j]);
                    if (!string.IsNullOrEmpty(result))
                    {
                        result += string.Format("({0}-{1})", sPos[i], sPos[j]);
                        return result;
                    }
                }
            }
            //納音
            for (int i = 0; i < aryKansi.Length - 1; i++)
            {
                for (int j = i + 1; j < aryKansi.Length; j++)
                {
                    result = person.GetNentin(aryKansi[i], aryKansi[j]);
                    if (!string.IsNullOrEmpty(result))
                    {
                        result += string.Format("({0}-{1})", sPos[i], sPos[j]);
                        return result;
                    }
                }
            }

            return null;
        }
     
    }

}
