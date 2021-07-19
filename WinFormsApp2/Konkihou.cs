using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    /// 根気法 データクラス
    /// </summary>
    class Konkihou : Insen
    {
        TableMng tblMng = null;
        FindItem[] kansiRoot = new FindItem[3];

        public Konkihou(Person person) : base(person)
        {
            tblMng = TableMng.GetTblManage();

            //日干支の干の根を検索
            kansiRoot[0] = FindRoot(nikkansi.kan);

            //月干支の干の根を検索
            kansiRoot[1] = FindRoot(gekkansi.kan);

            //年干支の干の根を検索
            kansiRoot[2] = FindRoot(nenkansi.kan);
        }

        public FindItem[] GetKansiRoot() { return kansiRoot; }

        public int GetSumScore(string kan)
        {
            //kanと日干支（支）、月干支（支）、年干支（支）の組み合わせから
            //十二大従星の点数の合計値を取得する。

            var item1 = tblMng.junidaiJusei.GetJunidaiJusei(kan, nikkansi.si);
            var item2 = tblMng.junidaiJusei.GetJunidaiJusei(kan, gekkansi.si);
            var item3 = tblMng.junidaiJusei.GetJunidaiJusei(kan, nenkansi.si);

            return item1.tensuu + item2.tensuu + item3.tensuu;
        }

        //根 探索
        private FindItem FindRoot(string kan)
        {
            string[] findStrs = new string[2];
            findStrs[0] = kan;
            //指定された干と陰陽の関係にある干文字を取得
            findStrs[1] = tblMng.jyukanTbl.GetInyouOtherString(kan);

            //本元、中元、初元の項目数（=3)
            int num = Enum.GetValues(typeof(NijuhachiGenso.enmGensoType)).Length;

            List<FindItem> lstItem = new List<FindItem>();

            //蔵元の中からfindStrに該当するものを探す
            foreach (var str in findStrs)
            {
                for (int i = 0; i < num; i++)
                {
                    if (nikkansiHongen[i].name == str)
                    {
                        lstItem.Add(new FindItem((NijuhachiGenso.enmGensoType)i, nikkansi.si, Const.bitFlgNiti));
                    }
                    if (gekkansiHongen[i].name == str)
                    {
                        lstItem.Add(new FindItem((NijuhachiGenso.enmGensoType)i, gekkansi.si, Const.bitFlgGetu));
                    }
                    if (nenkansiHongen[i].name == str)
                    {
                        lstItem.Add(new FindItem((NijuhachiGenso.enmGensoType)i, nenkansi.si, Const.bitFlgNen));
                    }
                }
            }

            FindItem result = null;

            //本元項目数
            var items = lstItem.FindAll(x => x.type == NijuhachiGenso.enmGensoType.GENSO_HONGEN);
            if (items.Count == 1)
            {
                //本元の項目が１件あったので、この干支が根となる
                var junidaiJusei = tblMng.junidaiJusei.GetJunidaiJusei(kan, items[0].si);
                items[0].junidaiJusei = junidaiJusei;
                result = items[0];
            }
            else
            {
                //本元に２つ以上または他の蔵元に複数存在する

                //--------------------------
                //点数が高いものを探す
                //--------------------------
                //十二大従星の表からkanとitems[*]の組み合わせに該当する十二大従星と点数を取得
                for (int i = 0; i < lstItem.Count; i++)
                {

                    var junidaiJusei = tblMng.junidaiJusei.GetJunidaiJusei(kan, lstItem[i].si);
                    lstItem[i].junidaiJusei = junidaiJusei;
                }

                for (int i = 0; i < lstItem.Count; i++)
                {
                    if (result == null)
                    {
                        result = lstItem[i];
                        continue;
                    }
                    if (result.junidaiJusei.tensuu < lstItem[i].junidaiJusei.tensuu)
                    {
                        //点数の大きい方をresultに設定
                        result = items[i];
                    }
                }
            }



            return result;
        }
 
        
    }

    class FindItem
    {
        public FindItem(NijuhachiGenso.enmGensoType _type, string _si, int _kansiBit)
        {
            type = _type;
            si = _si;
            kansiBit = _kansiBit;

        }
        public NijuhachiGenso.enmGensoType type;
        public string  si;
        public int kansiBit;

        public JunidaiJusei junidaiJusei;

    }

}
