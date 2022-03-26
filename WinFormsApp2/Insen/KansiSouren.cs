using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    ///干支双連
    /// </summary>
    class KansiSouren
    {

        public static string GetKansiSouren(Person person)
        {
            TableMng tblMng = TableMng.GetTblManage();
            Kansi[] aryKansi = new Kansi[] { person.nikkansi, person.gekkansi, person.nenkansi };
            string[] sPos = new string[] { "日", "月", "年" };

            //干合組み合わせチェック
            for (int i = 0; i < aryKansi.Length - 1; i++)
            {
                for (int j = i + 1; j < aryKansi.Length; j++)
                {
                    //干合 || 陰陽
                    if (tblMng.kangouTbl.IsKangou(aryKansi[i].kan, aryKansi[j].kan) ||
                        tblMng.jyukanTbl.IsInyou(aryKansi[i].kan, aryKansi[j].kan)
                        )
                    {
                        if (tblMng.sigouTbl.IsSgou(aryKansi[i].si, aryKansi[j].si))
                        {
                            return string.Format("干支双連({0}-{1})", sPos[i], sPos[j]);
                        }
                    }
                }
            }
            return null;
        }
     
    }

}
