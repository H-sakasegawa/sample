using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    /// 影星法（えいせいほう）
    /// </summary>
    class EiseiHou
    {
        public static string GetEiseiHou( Person person )
        {
            TableMng tblMng = TableMng.GetTblManage();
            //年干と干合の関係にある十干を求める。

            string kangou = tblMng.kangouTbl.GetKangouOtherStr(person.nenkansi.kan);

            //干合する十干と日干から求まった十大主星が影星
            var judaiShusei = tblMng.juudaiShusei.GetJudaiShusei(person.nikkansi.kan, kangou);

            return judaiShusei.name;
        }
    }
}
