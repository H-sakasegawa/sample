using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    /// 虚気透干
    /// </summary>
    class KyokiToukan
    {
        TableMng tblMng = TableMng.GetTblManage();

        class ChkData
        {
            public ChkData(string _name, int _bitTargetType, bool _bIgnoreKyokiTarget=false, bool _bKan = true)
            {
                name = _name;
                bitTargetType = _bitTargetType;
                bIgnoreKyokiTarget = _bIgnoreKyokiTarget;
                bKan = _bKan;
            }
            public string name;
            public int bitTargetType;
            public bool bIgnoreKyokiTarget;
            public bool bKan;
        }

        public class KyokiChkResult
        {
            public KyokiChkResult()
            {
                kyokiAttr = null;
                kyokiItemBit = 0;
            }

            public string kyokiAttr;
            public int kyokiItemBit;
        }

        public bool IsKyokiTokan_Shukumei(Person person)
        {
            //int chkRangeBit = Const.bitFlgNiti | Const.bitFlgGetu | Const.bitFlgNen;
            Insen insen = new Insen(person);

            List<ChkData> aryChkData = new List<ChkData>
            {
                new ChkData(person.nikkansi.kan, Const.bitFlgNiti), //日干
                new ChkData(person.gekkansi.kan, Const.bitFlgGetu), //月干
                new ChkData(person.nenkansi.kan, Const.bitFlgNen), //年干
            };
            foreach (var item in Enum.GetValues(typeof(NijuhachiGenso.enmGensoType)))//初元、中元、本元
            {
                int idx = (int)item;
                aryChkData.Add(new ChkData(insen.nikkansiHongen[idx].name, 0, false, false));
                aryChkData.Add(new ChkData(insen.gekkansiHongen[idx].name, 0, false, false));
                aryChkData.Add(new ChkData(insen.nenkansiHongen[idx].name, 0, false, false));
            }
            if( CheckKyokiToukan(person, aryChkData, 0, null, null)!=null)
            {
                return true;
            }
            return false;

        }

        public KyokiChkResult IsKyokiTokan_Koutenun(Person person,
                                            Kansi kansiTaiun,
                                            Kansi kansiNenun,
                                            Kansi kansiGetuun,
                                            string taiunKyokiAttr,  //大運で虚気となったときの属性
                                            int taiunKyokiTarget,    //大運で虚気となった干支のビット
                                            string nenunKyokiAttr,  //年運で虚気となったときの属性
                                            int nenunKyokiTarget,    //年運で虚気となった干支のビット
                                            int kansiTypeBit        //今回検証干支（大運？年運？月運？）
            )
        {

            //すでに虚気と判断されている干支のビット情報
            int ignoreKyokiTarget = taiunKyokiTarget | nenunKyokiTarget;

            List<ChkData> aryChkData = new List<ChkData>
            {
                new ChkData(person.nikkansi.kan, Const.bitFlgNiti, (ignoreKyokiTarget &  Const.bitFlgNiti)!=0 ), //日干
                new ChkData(person.gekkansi.kan, Const.bitFlgGetu, (ignoreKyokiTarget &  Const.bitFlgGetu)!=0), //月干
                new ChkData(person.nenkansi.kan, Const.bitFlgNen, (ignoreKyokiTarget &  Const.bitFlgNen)!=0), //年干
            };


            if (kansiTypeBit == Const.bitFlgTaiun)
            {    //大運、日干、月干、年干が同一チェック範囲
                aryChkData.Add(new ChkData(kansiTaiun.kan, Const.bitFlgTaiun));
            }
            else if (kansiTypeBit == Const.bitFlgNenun)
            {   //年運、大運、日干、月干、年干が同一チェック範囲
                aryChkData.Add(new ChkData(kansiTaiun.kan, Const.bitFlgTaiun, (ignoreKyokiTarget & Const.bitFlgTaiun) != 0));
                aryChkData.Add(new ChkData(kansiNenun.kan, Const.bitFlgNenun));
            }
            else if (kansiTypeBit == Const.bitFlgGetuun)
            {   //月運、年運、大運、日干、月干、年干が同一チェック範囲
                aryChkData.Add(new ChkData(kansiTaiun.kan, Const.bitFlgTaiun, (ignoreKyokiTarget & Const.bitFlgTaiun) != 0));
                aryChkData.Add(new ChkData(kansiNenun.kan, Const.bitFlgNenun, (ignoreKyokiTarget & Const.bitFlgNenun) != 0));
                aryChkData.Add(new ChkData(kansiGetuun.kan, Const.bitFlgGetuun));
            }

            return CheckKyokiToukan(person, aryChkData, kansiTypeBit, taiunKyokiAttr, nenunKyokiAttr);
        }

        private KyokiChkResult CheckKyokiToukan(Person person,
                                    List<ChkData> lstChkDatas, 
                                    int kansiTypeBit, 
                                    string taiunKyokiAttr,
                                    string nenunKyokiAttr
            )
        {
            for (int i = 0; i < lstChkDatas.Count; i++)
            {
                //今回の干合チェック対象干支でなければSKIP
                //if (!bAllPatternCheck && lstChkDatas[i].bIgnoreTarget) continue;

                for (int j = 0; j < lstChkDatas.Count; j++)
                {
                    if (i == j) continue;

                    //今回の干合チェック対象干支でなければSKIP
                    //if (!bAllPatternCheck && lstChkDatas[j].bIgnoreTarget) continue;


                    //干以外同士はSKIP
                    if (!lstChkDatas[i].bKan && !lstChkDatas[j].bKan) continue;


                    var gogyouAttr = tblMng.kangouTbl.GetKangouAttr(lstChkDatas[i].name, lstChkDatas[j].name);
                    if (gogyouAttr != null)
                    {

                        if(lstChkDatas[i].bitTargetType == kansiTypeBit)
                        {
                            if (!string.IsNullOrEmpty(taiunKyokiAttr) && gogyouAttr == taiunKyokiAttr)
                            {
                                KyokiChkResult result = new KyokiChkResult();
                                result.kyokiAttr = gogyouAttr;
                                return result;

                            }
                            if (!string.IsNullOrEmpty(nenunKyokiAttr) && gogyouAttr == nenunKyokiAttr)
                            {
                                KyokiChkResult result = new KyokiChkResult();
                                result.kyokiAttr = gogyouAttr;
                                return result;

                            }
                        }


                        //変化した属性は干合判定のあった干以外のオリジナル属性と同じか？
                        for (int iChk = 0; iChk < lstChkDatas.Count; iChk++)
                        {
                            if (iChk == i || iChk == j) continue; //干合判定の文字なのでSKIP
                            if (string.IsNullOrEmpty(lstChkDatas[iChk].name)) continue;
                            if (!lstChkDatas[iChk].bKan) continue; //干以外（蔵元など）はSKIP
                            if (lstChkDatas[iChk].bIgnoreKyokiTarget) continue;


                            if (gogyouAttr == tblMng.jyukanTbl[lstChkDatas[iChk].name].gogyou)
                            {
                                KyokiChkResult result = new KyokiChkResult();
                                result.kyokiAttr = gogyouAttr;
                                result.kyokiItemBit = lstChkDatas[iChk].bitTargetType;
                                return result;
                            }
                        }
                    }
                }
            }
            return null;
        }


    }
}
