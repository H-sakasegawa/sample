using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    ///干支法
    /// </summary>
    class KansiHou
    {

        /// <summary>
        /// 天合地破(干に干合または、比和陰陽があり、支に散法がある)
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static string GetTengouChiha(Person person)
        {
            //干合または、比和陰陽
            if (!IsKangouOrHiwa(person)) return null;

            //散法
            bool bResult = IsGouhouSanpou(person, "刑", "害", "破", "冲動");
            return bResult? "天合地破":null;
        }
        /// <summary>
        /// 天殺地合(干に七殺、支に合法がある)
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static string GetTensatuChigou(Person person)
        {
            //七殺
            if (!IsNanasatu(person)) return null;

            //合法
            bool bResult = IsGouhouSanpou(person, "半会", "支合");
            return bResult ? "天殺地合" : null;
        }
        /// <summary>
        /// 天剋地合(干に相剋、支に合法がある)
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static string GetTenkokuChigou(Person person)
        {
            //相剋
            if (!IsSokoku(person)) return null;

            //合法
            bool bResult = IsGouhouSanpou(person, "半会", "支合");
            return bResult ? "天殺地合" : null;
        }
        /// <summary>
        /// 天殺地冲(干に七殺、支に冲動がある)
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static string GetTensatuChichu(Person person)
        {
            //七殺
            if (!IsNanasatu(person)) return null;

            //散法
            bool bResult = IsGouhouSanpou(person, "冲動");
            return bResult ? "天殺地冲" : null;
        }

        /// <summary>
        /// 天殺地破(干に七殺、支に冲動がある)
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static string GetTensatuChiha(Person person)
        {
            //七殺
            if (!IsNanasatu(person)) return null;

            //散法
            bool bResult = IsGouhouSanpou(person, "自刑");
            return bResult ? "天殺地破" : null;
        }

        /// <summary>
        /// 天剋地破(干に相剋、支に刑がある)
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static string GetTenkokuChiha(Person person)
        {
            //粗剋
            if (!IsSokoku(person)) return null;

            //散法
            bool bResult = IsGouhouSanpou(person, "刑");
            return bResult ? "天剋地破" : null;
        }
        /// <summary>
        /// 天合地合(干に干合または、比和陰陽があり、支に合法がある)
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static string GetTengouChigou(Person person)
        {
            //干合または、比和陰陽
            if (!IsKangouOrHiwa(person)) return null;

            //合法
            bool bResult = IsGouhouSanpou(person, "半会", "支合");
            return bResult ? "天合地合" : null;
        }

        //------------------------------------------
        //干の干合または、比和陰陽有無チェック
        //------------------------------------------
        static bool IsKangouOrHiwa(Person person )
        {
            TableMng tblMng = TableMng.GetTblManage();
            string[] sKan = new string[] { person.nikkansi.kan, person.gekkansi.kan, person.nenkansi.kan };
 
            //干合または、比和陰陽
            for (int i = 0; i < sKan.Length; i++)
            {
                for (int j = 0; j < sKan.Length; j++)
                {
                    if (i == j) continue;
                    if (tblMng.kangouTbl.IsKangou(sKan[i], sKan[j]))
                    {
                        return true;
                    }
                    if (tblMng.jyukanTbl.IsInyou(sKan[i], sKan[j]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //------------------------------------------
        //干の七殺有無チェック
        //------------------------------------------
        static bool IsNanasatu(Person person )
        {
            TableMng tblMng = TableMng.GetTblManage();
            string[] sKan = new string[] { person.nikkansi.kan, person.gekkansi.kan, person.nenkansi.kan };
 
            for (int i = 0; i < sKan.Length; i++)
            {
                for (int j = 0; j < sKan.Length; j++)
                {
                    if (i == j) continue;
                    if (tblMng.nanasatsuTbl.IsNanasastsu(sKan[i], sKan[j]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //----------------------------------------
        //干の相剋有無チェック
        //----------------------------------------
        static bool IsSokoku(Person person)
        {
            TableMng tblMng = TableMng.GetTblManage();
            string[] sKan = new string[] { person.nikkansi.kan, person.gekkansi.kan, person.nenkansi.kan };
            //相剋
            for (int i = 0; i < sKan.Length; i++)
            {
                for (int j = 0; j < sKan.Length; j++)
                {
                    if (i == j) continue;
                    if (tblMng.gogyouAttrRelationshipTbl.IsDestoryByKan(sKan[i], sKan[j]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //----------------------------------------
        //支の合法散法　有無チェック
        //----------------------------------------
        static bool IsGouhouSanpou(Person person, params string[] items)
        {
            TableMng tblMng = TableMng.GetTblManage();
            string[] sSi = new string[] { person.nikkansi.si, person.gekkansi.si, person.nenkansi.si };
            for (int i = 0; i < sSi.Length; i++)
            {
                for (int j = 0; j < sSi.Length; j++)
                {
                    if (i == j) continue;
                    string[] sanpou = tblMng.gouhouSanpouTbl.GetGouhouSanpou(sSi[i], sSi[j]);
                    if (sanpou == null) continue;

                    foreach (var item in items)
                    {
                        if (sanpou.Where(x => x.Contains(item)).Count() > 0) return true;
                    }
                }
            }
            return false;
        }
    }

}
