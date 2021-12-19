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
            public ChkData(string _name, bool _bIgnoreTarget=false, bool _bKan = true)
            {
                name = _name;
                bIgnoreTarget = _bIgnoreTarget;
                bKan = _bKan;
            }
            public string name;
            public bool bIgnoreTarget;
            public bool bKan;
        }

        public bool IsKyokiTokan_Shukumei(Person person)
        {
            string gogyouAttr = null;
            //int chkRangeBit = Const.bitFlgNiti | Const.bitFlgGetu | Const.bitFlgNen;
            Insen insen = new Insen(person);

            List<ChkData> aryChkData = new List<ChkData>
            {
                new ChkData(person.nikkansi.kan), //日干
                new ChkData(person.gekkansi.kan), //月干
                new ChkData(person.nenkansi.kan), //年干
            };
            foreach (var item in Enum.GetValues(typeof(NijuhachiGenso.enmGensoType)))//初元、中元、本元
            {
                int idx = (int)item;
                aryChkData.Add(new ChkData(insen.nikkansiHongen[idx].name, false, false));
                aryChkData.Add(new ChkData(insen.gekkansiHongen[idx].name, false, false));
                aryChkData.Add(new ChkData(insen.nenkansiHongen[idx].name, false, false));
            }

            return CheckKyokiToukan(person, aryChkData);

            ////--------------------------------------------
            //// 日干、月干、年干の組み合わせでチェック
            ////--------------------------------------------
            //if (IsKyokiTokanInKansi(person, chkRangeBit)) return true;

            ////--------------------------------------------
            //// ＊干と蔵元 でチェック
            ////--------------------------------------------
            ////日干 － 蔵元
            //gogyouAttr = GetKangouAttrByZougan(person, person.nikkansi.kan);
            //if (gogyouAttr != null)
            //{
            //    //変化した属性は月干、年干の属性と同じか？
            //    if (IsExistSameAttrInShukumei(person, gogyouAttr, Const.bitFlgGetu | Const.bitFlgNen)) return true;
            //}
            ////月干 － 蔵元
            //gogyouAttr = GetKangouAttrByZougan(person, person.gekkansi.kan);
            //if (gogyouAttr != null)
            //{
            //    //変化した属性は日干、年干の属性と同じか？
            //    if (IsExistSameAttrInShukumei(person, gogyouAttr, Const.bitFlgNiti | Const.bitFlgNen)) return true;
            //}
            ////年干 － 蔵元
            //gogyouAttr = GetKangouAttrByZougan(person, person.nenkansi.kan);
            //if (gogyouAttr != null)
            //{
            //    //変化した属性は日干、月干の属性と同じか？
            //    if (IsExistSameAttrInShukumei(person, gogyouAttr, Const.bitFlgNiti | Const.bitFlgGetu)) return true;
            //}
            //return false;
        }

        public bool IsKyokiTokan_Koutenun(Person person,
                                            Kansi kansiTaiun,
                                            Kansi kansiNenun,
                                            Kansi kansiGetuun,
                                            bool bTaiunKyoki,
                                            bool bNenunKyoki,
                                            int kansiTypeBit
            )
        {

            List<ChkData> aryChkData = new List<ChkData>
            {
                new ChkData(person.nikkansi.kan), //日干
                new ChkData(person.gekkansi.kan), //月干
                new ChkData(person.nenkansi.kan), //年干
            };


            if (kansiTypeBit == Const.bitFlgTaiun)
            {    //大運、日干、月干、年干が同一チェック範囲
                aryChkData.Add(new ChkData(kansiTaiun.kan));
            }
            else if (kansiTypeBit == Const.bitFlgNenun)
            {   //年運、大運、日干、月干、年干が同一チェック範囲
                aryChkData.Add(new ChkData(kansiTaiun.kan, bTaiunKyoki));
                aryChkData.Add(new ChkData(kansiNenun.kan));
            }
            else if (kansiTypeBit == Const.bitFlgGetuun)
            {   //月運、年運、大運、日干、月干、年干が同一チェック範囲
                aryChkData.Add(new ChkData(kansiTaiun.kan, bTaiunKyoki));
                aryChkData.Add(new ChkData(kansiNenun.kan, bNenunKyoki));
                aryChkData.Add(new ChkData(kansiGetuun.kan));
            }

            return CheckKyokiToukan(person, aryChkData, false);
        }

        private bool CheckKyokiToukan(Person person, List<ChkData> lstChkDatas, bool bAllPatternCheck=true)
        {
            for (int i = 0; i < lstChkDatas.Count; i++)
            {
                //今回の干合チェック対象干支でなければSKIP
                if (!bAllPatternCheck && lstChkDatas[i].bIgnoreTarget) continue;

                for (int j = 0; j < lstChkDatas.Count; j++)
                {
                    if (i == j) continue;

                    //今回の干合チェック対象干支でなければSKIP
                    if (!bAllPatternCheck && lstChkDatas[j].bIgnoreTarget) continue;


                    //干以外同士はSKIP
                    if (!lstChkDatas[i].bKan && !lstChkDatas[j].bKan) continue;

                    var gogyouAttr = tblMng.kangouTbl.GetKangouAttr(lstChkDatas[i].name, lstChkDatas[j].name);
                    if (gogyouAttr != null)
                    {
                        //変化した属性は干合判定のあった干以外のオリジナル属性と同じか？
                        for (int iChk = 0; iChk < lstChkDatas.Count; iChk++)
                        {
                            if (iChk == i || iChk == j) continue; //干合判定の文字なのでSKIP
                            if (string.IsNullOrEmpty(lstChkDatas[iChk].name)) continue;
                            if (!lstChkDatas[iChk].bKan) continue; //干以外（蔵元など）はSKIP
                            //if (!bAllPatternCheck && lstChkDatas[iChk].bIgnoreTarget) continue;


                            if (gogyouAttr == tblMng.jyukanTbl[lstChkDatas[iChk].name].gogyou)
                                return true;
                        }
                    }
                }
            }
            return false;
        }



        //public bool IsKyokiTokanInKansi(Person person, int checkRangeBit)
        //{
        //    string gogyouAttr = null;
        //    //--------------------------------------------
        //    // 日干、月干、年干の組み合わせでチェック
        //    //--------------------------------------------
        //    //日干 - 月干
        //    gogyouAttr = tblMng.kangouTbl.GetKangouAttr(person.nikkansi.kan, person.gekkansi.kan);
        //    if (gogyouAttr != null)
        //    {
        //        //変化した属性は年干の属性と同じか？
        //        if (IsExistSameAttrInShukumei(person, gogyouAttr, checkRangeBit & ~(Const.bitFlgNiti | Const.bitFlgGetu) )) return true;
        //    }
        //    //日干 - 年干
        //    gogyouAttr = tblMng.kangouTbl.GetKangouAttr(person.nikkansi.kan, person.nenkansi.kan);
        //    if (gogyouAttr != null)
        //    {
        //        //変化した属性は月干の属性と同じか？
        //        if (IsExistSameAttrInShukumei(person, gogyouAttr, checkRangeBit & ~(Const.bitFlgNiti | Const.bitFlgNen))) return true;
        //    }
        //    //月干 - 年干
        //    gogyouAttr = tblMng.kangouTbl.GetKangouAttr(person.gekkansi.kan, person.nenkansi.kan);
        //    if (gogyouAttr != null)
        //    {
        //        //変化した属性は日干の属性と同じか？
        //        if (IsExistSameAttrInShukumei(person, gogyouAttr, checkRangeBit & ~(Const.bitFlgGetu | Const.bitFlgNen)) return true;
        //    }
        //    return false;
        //}

        ////指定された干と蔵元の文字で干合をチェック
        //string GetKangouAttrByZougan( Person person , string kan)
        //{
        //    string gogyouAttr = null;
        //    Insen insen = new Insen(person);
        //    foreach (var item in Enum.GetValues(typeof(NijuhachiGenso.enmGensoType)))//初元、中元、本元
        //    {
        //        int idx = (int)item;
        //        //日蔵元
        //        gogyouAttr = tblMng.kangouTbl.GetKangouAttr(kan, insen.nikkansiHongen[idx].name);
        //        if (gogyouAttr != null) return gogyouAttr;

        //        //月蔵元
        //        gogyouAttr = tblMng.kangouTbl.GetKangouAttr(kan, insen.gekkansiHongen[idx].name);
        //        if (gogyouAttr != null) return gogyouAttr;

        //        //年蔵元
        //        gogyouAttr = tblMng.kangouTbl.GetKangouAttr(kan, insen.nenkansiHongen[idx].name);
        //        if (gogyouAttr != null) return gogyouAttr;
        //    }

        //    return null;

        //}


        ////指定された属性が指定干以外の干の属性に存在するか？
        //private bool IsExistSameAttrInShukumei(Person person ,string goryouAttr, int findTargetBit )
        //{
        //    if ((findTargetBit & Const.bitFlgNiti) != 0) //日干
        //    {
        //        if (goryouAttr == tblMng.jyukanTbl[person.nikkansi.kan].gogyou) return true;
        //    }
        //    if ((findTargetBit & Const.bitFlgGetu) != 0) //月干
        //    {
        //        if (goryouAttr == tblMng.jyukanTbl[person.gekkansi.kan].gogyou) return true;
        //    }
        //    if ((findTargetBit & Const.bitFlgNen) != 0) //年干
        //    {
        //        if (goryouAttr == tblMng.jyukanTbl[person.nenkansi.kan].gogyou) return true;
        //    }
        //    return false;
        //}

        ////指定された属性が指定干以外の干の属性に存在するか？
        //private bool IsExistSameAttrInKoutenun(string goryouAttr, int findTargetBit)
        //{
        //    if ((findTargetBit & Const.bitFlgGetuun) != 0) //月運
        //    {

        //    }
        //    else if ((findTargetBit & Const.bitFlgNenun) != 0) //年運
        //    {

        //    }
        //    else if ((findTargetBit & Const.bitFlgTaiun) != 0) //大運
        //    {

        //    }
        //    else if ((findTargetBit & Const.bitFlgNiti) != 0) //日干
        //    {

        //    }
        //    else if ((findTargetBit & Const.bitFlgGetu) != 0) //月干
        //    {

        //    }
        //    else if ((findTargetBit & Const.bitFlgNen) != 0) //年干
        //    {

        //    }
        //}
    }
}
