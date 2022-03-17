using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    ///閉畢命式
    /// </summary>
    class Heihitumeisiki
    {

        public static string GetHeihitumeisiki(Person person)
        {
            string result = "";
            TableMng tblMng = TableMng.GetTblManage();
            TableMng.GogyouAttrRerationshipTbl relation = tblMng.gogyouAttrRelationshipTbl;

            //1)笙が透干する
            // →日干→年干が相生の関係にある
            if (!relation.IsCreateByKan(person.nikkansi.kan, person.nenkansi.kan))
            {   //条件不一致
                return null;
            }

            //2)笙を七殺するものが透干する
            //　→月干→年干で七殺
            if(! tblMng.nanasatsuTbl.IsNanasastsu(person.gekkansi.kan, person.nenkansi.kan))
            {   //条件不一致
                return null;
            }
            //3)両親が透干する
            JuniSinKanHou juniSinKanHou = new JuniSinKanHou();
            var Node = juniSinKanHou.Create(person);

            //母親
            var mother = Node.parent.kan;
            var father = Node.parent.partnerMan.kan;
            if (person.gekkansi.kan == mother)
            {
                if (person.nenkansi.kan == father)
                {
                    result = "閉畢命式";
                }
                else
                {
                    result = "閉畢命式(準)";
                }
            }
            else if(person.gekkansi.kan == father)
            {
                if (person.nenkansi.kan == mother)
                {
                    result = "閉畢命式";
                }
                else
                {
                    result = "閉畢命式(準)";
                }
            }

            //過去と未来が逆になる
            //　→これは、1)の部分ですでに逆になるケースだけにフィルタリング
            //　　されているので特に何か比較することはない。

            return result;
        }
    }
    /// <summary>
    /// 閟畢命式
    /// </summary>
    class Hihitumeisiki
    {

        public static string GetHihitumeisiki(Person person)
        {
            string result = "";
            TableMng tblMng = TableMng.GetTblManage();
            TableMng.GogyouAttrRerationshipTbl relation = tblMng.gogyouAttrRelationshipTbl;

            //1)笙が透干する
            // →日干→年干が相生の関係にある
            if (!relation.IsCreateByKan(person.nikkansi.kan, person.nenkansi.kan))
            {   //条件不一致
                return null;
            }

            //2)笙を干合する
            //　→月干→年干で干合
            if (!tblMng.kangouTbl.IsKangou(person.gekkansi.kan, person.nenkansi.kan))
            {   //条件不一致
                return null;
            }
            //3)両親が透干する
            JuniSinKanHou juniSinKanHou = new JuniSinKanHou();
            var Node = juniSinKanHou.Create(person);

            //母親
            var mother = Node.parent.kan;
            var father = Node.parent.partnerMan.kan;
            if (person.gekkansi.kan == mother)
            {
                if (person.nenkansi.kan == father)
                {
                    result = "閉畢命式";
                }
                else
                {
                    result = "閉畢命式(準)";
                }
            }
            else if (person.gekkansi.kan == father)
            {
                if (person.nenkansi.kan == mother)
                {
                    result = "閉畢命式";
                }
                else
                {
                    result = "閉畢命式(準)";
                }
            }

            //過去と未来が逆になる
            //　→これは、1)の部分ですでに逆になるケースだけにフィルタリング
            //　　されているので特に何か比較することはない。

            return result;
        }
    }
}
