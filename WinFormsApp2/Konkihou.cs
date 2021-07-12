using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    class Konkihou : Insen
    {
        TableMng tblMng = null;

        public Konkihou(Person person ): base(person)
        {
            tblMng = TableMng.GetTblManage();
        }

        //根 探索
        private void FindRoot( string kan)
        {
            //指定された干と陰陽の関係にある干文字を取得
            string findStr = tblMng.jyukanTbl.GetInyouOtherString(kan);

        }
    }
}
