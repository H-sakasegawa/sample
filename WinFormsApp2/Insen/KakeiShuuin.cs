using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    ///家系集印
    /// </summary>
    class KakeiShuuin
    {

        public static string GetKakeiShuuin(Person person)
        {
       
            TableMng tblMng = TableMng.GetTblManage();

            string nitiKan = person.nikkansi.kan;
            string nitiSi = person.nikkansi.si;
            string getuKan = person.gekkansi.kan;
            string getuSi = person.gekkansi.si;
            string nenKan = person.nenkansi.kan;
            string nenSi = person.nenkansi.si;

            string attNitiKan = tblMng.jyukanTbl.GetGogyo(nitiKan);
            string attNitiSi = tblMng.jyukanTbl.GetGogyo(nitiSi);
            string attGetuKan = tblMng.jyukanTbl.GetGogyo(getuKan);
            string attGetuSi = tblMng.jyunisiTbl.GetGogyo(getuSi);
            string attNenKan = tblMng.jyunisiTbl.GetGogyo(nenKan);
            string attNenSi = tblMng.jyunisiTbl.GetGogyo(nenSi);

            //専気 判定
            if (attGetuKan != attGetuSi) return null;//専気ではない

            string sResult = null;

            //家系集印? (月干→日干）
            if( tblMng.gogyouAttrRelationshipTbl.IsCreate(attGetuKan, attNitiKan))
            {
                sResult = "家系集印";

                if (attNitiSi == attGetuSi) sResult = "包曲" + sResult;
            }
            else
            {
                if (tblMng.gogyouAttrRelationshipTbl.IsCreate(attGetuSi, attNitiSi))
                {
                    sResult = "家系集印(配偶者)";
                    if (attNitiKan == attGetuKan) sResult = "包曲" + sResult;

                }
                else if (tblMng.gogyouAttrRelationshipTbl.IsCreate(attGetuKan, attNenKan))
                {
                    sResult = "家系集印(父)";
                    if (attNenSi == attGetuSi) sResult = "包曲" + sResult;
                }
            }

            return sResult;
        }
    }

}
