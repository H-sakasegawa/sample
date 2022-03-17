using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    ///中殺
    /// </summary>
    class Chusatu
    {

        /// <summary>
        /// 日座中殺
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static string GetNichizaChusatu(Person person)
        {

            TableMng tblMng = TableMng.GetTblManage();

            string nitiKan = person.nikkansi.kan;
            string nitiSi = person.nikkansi.si;
            //string nenKan = person.nenkansi.kan;

            if ((nitiKan == "甲" && nitiSi == "戌") ||
                 (nitiKan == "乙" && nitiSi == "亥"))
            {
                return "日座中殺";
            }

            return null;
        }
        /// <summary>
        /// 日居中殺
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static string GetNikkyoChusatu(Person person)
        {

            TableMng tblMng = TableMng.GetTblManage();

            string nitiKan = person.nikkansi.kan;
            string nitiSi = person.nikkansi.si;

            if ((nitiKan == "甲" && nitiSi == "辰") ||
                 (nitiKan == "乙" && nitiSi == "巳"))
            {
                return "日居中殺";
            }

            return null;
        }
        /// <summary>
        /// 宿命二中殺
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static string GetShukumeiNiChusatu(Person person)
        {

            TableMng tblMng = TableMng.GetTblManage();

            string nitiSi = person.nikkansi.si;
            string getuSi = person.gekkansi.si;
            string nenSi = person.nenkansi.si;

            string[] NikkansiTenchusatu = person.nikkansi.tenchusatu.ToArray();
 
            bool bNenChusatu = false;
            for (int i = 0; i < NikkansiTenchusatu.Length; i++)
            {
                if (person.nenkansi.IsExist(NikkansiTenchusatu[i]))//生年中殺
                {
                    bNenChusatu = true;
                    break;
                }
            }
            if (!bNenChusatu) return null;

            for (int i = 0; i < NikkansiTenchusatu.Length; i++)
            {
                if (person.gekkansi.IsExist(NikkansiTenchusatu[i]))  //生月中殺
                {
                    return "宿命二中殺";
                }
            }
            return null;
        }
        /// <summary>
        /// 宿命全中殺
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static string GetShukumeiZenChusatu(Person person)
        {

            if (string.IsNullOrEmpty(GetShukumeiNiChusatu(person))) return null;

            string[] NenkansiTenchusatu = person.nenkansi.tenchusatu.ToArray();
            bool bShukmeiZenChusatu = false;
            for (int i = 0; i < NenkansiTenchusatu.Length; i++)
            {
                //日干支に含まれているか？
                if (person.nikkansi.IsExist(NenkansiTenchusatu[i]))// 生日中殺
                {
                    bShukmeiZenChusatu = true;
                    break;
                }
            }
            if (!bShukmeiZenChusatu)
            {
                if (!string.IsNullOrEmpty(GetNichizaChusatu(person)))
                {
                    bShukmeiZenChusatu = true;
                }
            }
            return bShukmeiZenChusatu ? "宿命全中殺" : null;
        }

        /// <summary>
        /// 互換中殺
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static string GetShukumeiGokanChusatu(Person person)
        {
            //日座中殺は対象外
            //日居中殺は対象外
            if (!string.IsNullOrEmpty(GetNichizaChusatu(person))) return null;
            if (!string.IsNullOrEmpty(GetNikkyoChusatu(person))) return null;

            string[] NikkansiTenchusatu = person.nikkansi.tenchusatu.ToArray();
            string[] NenkansiTenchusatu = person.nenkansi.tenchusatu.ToArray();

            bool bNenChusatu = false;
            for (int i = 0; i < NikkansiTenchusatu.Length; i++)
            {
                if (person.nenkansi.IsExist(NikkansiTenchusatu[i]))//生年中殺
                {
                    bNenChusatu = true;
                    break;
                }
            }
            if (!bNenChusatu) return null;
           
            bool bShukmeiGokanChusatu = false;
            for (int i = 0; i < NenkansiTenchusatu.Length; i++)
            {
                //日干支に含まれているか？
                if (person.nikkansi.IsExist(NenkansiTenchusatu[i]))// 生日中殺
                {
                    bShukmeiGokanChusatu = true;
                    break;
                }
            }

            return bShukmeiGokanChusatu ? "互換中殺" : null;
        }
    }

}
