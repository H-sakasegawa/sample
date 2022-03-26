using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    ///干支双破
    /// </summary>
    class KansiSouha
    {

        public static string GetKansiSouha(Person person)
        {
            TableMng tblMng = TableMng.GetTblManage();
            Kansi[] aryKansi = new Kansi[] { person.nikkansi, person.gekkansi, person.nenkansi };
            string[] sPos = new string[] { "日", "月", "年" };
            string[] items = { "害", "刑", "破" };

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
                        //散法 判定（害、刑、破）
                        string[] sanpou = tblMng.gouhouSanpouTbl.GetGouhouSanpou(aryKansi[i].si, aryKansi[j].si);
                        if (sanpou == null) continue;

                        foreach (var item in items)
                        {
                            if (sanpou.Where(x => x.Contains(item)).Count() > 0)
                            {
                                return string.Format("干支双破({0}-{1})", sPos[i], sPos[j]);
                            }
                        }
                    }
                }
            }
            return null;
        }
     
    }

}
