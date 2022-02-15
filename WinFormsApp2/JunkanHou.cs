using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    /// 循環法
    /// </summary>
    class JunkanHou
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="person"></param>
        /// <param name="siseiAttr">始星</param>
        /// <param name="kiseiAttr">帰星</param>
        /// <returns>true...五行循環</returns>
        public static bool GetJunkanHouAttr(Person person,
            ref string siseiAttr,
            ref string kiseiAttr)
        {
            TableMng tblMng = TableMng.GetTblManage();
            TableMng.GogyouAttrRerationshipTbl relation = tblMng.gogyouAttrRelationshipTbl;

            //水、木、火、土、金
            string[] gogyous = relation.GetCreatedRelation("水");
            bool[] bMark = new bool[gogyous.Length];
            for (int i = 0; i < bMark.Length; i++) bMark[i] = false;

            for (int i = 0; i < gogyous.Length; i++)
            {
                bool bFind = false;
                //gogyous[i]が対象者の十大主星の五行にふくまれているか？
                for (int j = 0; j < person.judaiShuseiAry.Length; j++)
                {
                    if (gogyous[i] == person.judaiShuseiAry[j].gogyou)
                    {
                        bMark[i] = true;
                        break;
                    }
                }
            }

            int iGogyo = 0;
            int iNext = 0;
            //該当しない五行の位置を特定
            for (iGogyo=0; iGogyo< bMark.Length; iGogyo++)
            {
                if (!bMark[iGogyo]) break;
            }
            if(iGogyo == bMark.Length)
            {   //五行循環
                siseiAttr = "木";
                kiseiAttr = "水";
                return true;
            }


            //帰星 検索
            for ( ; iGogyo < bMark.Length; iGogyo++)
            {
                iNext = iGogyo + 1;
                if (iNext >= bMark.Length) iNext = 0;

                //最初に2つ続く位置を検索
                if( bMark[iGogyo] && bMark[iNext])
                {
                    kiseiAttr = gogyous[iGogyo];
                    break;
                }
            }
            int kiseiIndex = iGogyo;
            if (iGogyo < bMark.Length)
            {
                iGogyo = iNext;
                //始星 検索
                while (true)
                {
                    iGogyo++;
                    if (iGogyo >= bMark.Length) iGogyo = 0;
                    if( iGogyo == kiseiIndex)
                    {   //五行循環の場合は無限ループしてしまう
                        break;
                    }
                    //五行が存在するまで帰星を変えながら進める
                    if (bMark[iGogyo])
                    {
                        siseiAttr = gogyous[iGogyo];
                    }else
                    {
                        break;
                    }
                 }
            }
            if (!string.IsNullOrEmpty(siseiAttr))
            {
                //五行循環
                return (relation.GetCreatFrom(siseiAttr) == kiseiAttr);
            }
            else
            {
                kiseiAttr = "";
            }
            return false;
        }
        public static bool GetJunkanHou(Person person,
                            ref string sisei,
                            ref string kisei)
        {
            string siseiAttr = "";
            string kiseiAttr = "";

            bool bGogyJunkan = GetJunkanHouAttr(person,ref siseiAttr, ref kiseiAttr);
            //始星、/帰星に該当する十大主星
            for (int i = 0; i < person.judaiShuseiAry.Length; i++)
            {
                if (siseiAttr == person.judaiShuseiAry[i].gogyou)
                {
                    if (!string.IsNullOrEmpty(sisei)) sisei += ", ";
                    sisei += person.judaiShuseiAry[i].name;
                }
                if (kiseiAttr == person.judaiShuseiAry[i].gogyou)
                {
                    if (!string.IsNullOrEmpty(kisei)) kisei += ", ";
                    kisei += person.judaiShuseiAry[i].name;
                }
            }


            return bGogyJunkan;
        }

        public static int GetCreateDistanceFromSiseiToN(string sisei, string kisei, JudaiShusei judaiShusei)
        {
            if( sisei == judaiShusei.gogyou)
            {
                return 1;
            }
            TableMng tblMng = TableMng.GetTblManage();
            TableMng.GogyouAttrRerationshipTbl relation = tblMng.gogyouAttrRelationshipTbl;
            int d = -1;
            string a = sisei;
            while (true)
            {
                a =  relation.GetRelation(a).createToName;
                d++;
                if (a == judaiShusei.gogyou)
                {
                    return d+2;
                }
                if (a == kisei) break;
            }
            return -1; 
        }

    }
}
