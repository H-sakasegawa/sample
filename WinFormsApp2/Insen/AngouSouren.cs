using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    ///暗合双連
    /// </summary>
    class AngouSouren
    {

        public static string GetAngouSouren(Person person)
        {

            string result = null;

            TableMng tblMng = TableMng.GetTblManage();

            string nitiKan = person.nikkansi.kan;

            Kansi[] aryKansi = new Kansi[] { person.nikkansi, person.gekkansi, person.nenkansi };

            //干合組み合わせチェック
            for (int i = 0; i < aryKansi.Length - 1; i++)
            {
                for (int j = i + 1; j < aryKansi.Length; j++)
                {

                    if (tblMng.kangouTbl.IsKangou(aryKansi[i].kan, aryKansi[j].kan))
                    {
                        //支が丑寅
                        if ((aryKansi[i].si == "丑" && aryKansi[j].si == "寅") ||
                            (aryKansi[i].si == "寅" && aryKansi[j].si == "丑"))
                        {
                            return "丑寅暗合双連";
                        }
                        else
                        //支が午亥
                        if ((aryKansi[i].si == "午" && aryKansi[j].si == "亥") ||
                           (aryKansi[i].si == "亥" && aryKansi[j].si == "午"))
                        {
                            return "午亥暗合双連";
                        }
                    }
                }
            }
            return null;
        }
     
    }

}
