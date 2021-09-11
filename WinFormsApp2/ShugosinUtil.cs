using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    class ShugosinUtil
    {

        /// <summary>
        /// 十干 の守護神判定
        /// ※守護神情報に十二支がある場合は、干に該当する項目を守護神とする
        /// 　十二支がない場合は、五行属性に該当するものを守護神とする
        /// </summary>
        /// <param name="kan">十二支 干</param>
        /// <returns></returns>
        public static bool IsShugosin(Person person ,string kan)
        {
            TableMng tblMng = TableMng.GetTblManage();
            //干の属性取得
            string kanAttr = tblMng.jyukanTbl[kan].gogyou;

            if (person.bCustomShugosin)
            {
                var shugosinAttr = person.customShugosin;   //カスタム守護神
                foreach (var shugoKan in shugosinAttr.lstCustShugosin)
                {
                    if (shugoKan.junisi != null)
                    {
                        if (kan == shugoKan.junisi) return true;
                    }
                    else
                    {
                        if (kanAttr == shugoKan.gogyouAttr) return true;
                    }
                }
            }
            else
            {
                var shugosinAttr = person.shugosinAttr; //基本の守護神
                string[] choukouShugosinKan = person.choukouShugosin;
                //守護神判定
                if (!string.IsNullOrEmpty(shugosinAttr))
                {
                    if (kanAttr == shugosinAttr) return true;
                }
                else
                {
                    if (choukouShugosinKan != null)
                    {
                        foreach (var shugoKan in choukouShugosinKan)
                        {
                            if (shugoKan == kan)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// 十干　 忌神判定
        /// ※忌神情報に十二支がある場合は、干に該当する項目を守護神とする
        /// 　十二支がない場合は、五行属性に該当するものを守護神とする
        /// </summary>
        /// <param name="kan"></param>
        /// <returns></returns>
        public static bool IsImigami(Person person, string kan)
        {
            TableMng tblMng = TableMng.GetTblManage();

            //干の属性取得
            string kanAttr = tblMng.jyukanTbl[kan].gogyou;

            if (person.bCustomShugosin)
            {
                var imigamiAttrs = person.customImigami.lstCustShugosin; //カスタム忌神
                foreach (var imigami in imigamiAttrs)
                {
                    if (imigami.junisi != null)
                    {
                        if (kan == imigami.junisi) return true;
                    }
                    else
                    {
                        if (kanAttr == imigami.gogyouAttr) return true;
                    }
                }

            }
            else
            {
                var imigami = person.imigamiAttr; //基本の忌神
                string choukouImigami = person.choukouImigamiAttr;


                //忌神判定
                if (!string.IsNullOrEmpty(imigami))
                {
                    if (kanAttr == imigami) return true;
                }
                else
                {
                    if (choukouImigami != null)
                    {
                        if (choukouImigami.IndexOf(kanAttr) >= 0)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// 基本の調和の守護神または、カスタム守護神から
        /// bCustomShugosinに従って対象の守護神を返す
        /// </summary>
        public static List<CustomShugosinAttr> GetShugosinAttrs(Person person)
        {
            List<CustomShugosinAttr> lstAttrs;

            if (person.bCustomShugosin)
            {   //カスタム守護神、カスタム忌神の有効指定あり
                lstAttrs = new List<CustomShugosinAttr>(person.customShugosin.lstCustShugosin);
            }
            else
            {
                lstAttrs = new List<CustomShugosinAttr>();
                if (person.shugosinAttr != null) lstAttrs.Add(new CustomShugosinAttr(null, person.shugosinAttr));
            }
            return lstAttrs;
        }
        /// <summary>
        /// 基本調和の忌神または、カスタム忌神から
        /// bCustomShugosinに従って対象の忌神を返す
        /// </summary>
        public static List<CustomShugosinAttr> GetImigamiAttrs(Person person)
        {
            List<CustomShugosinAttr> lstAttrs;
            if (person.bCustomShugosin)
            {   //カスタム守護神、カスタム忌神の有効指定あり
                lstAttrs = new List<CustomShugosinAttr>(person.customImigami.lstCustShugosin);
            }
            else
            {
                lstAttrs = new List<CustomShugosinAttr>();
                if (person.imigamiAttr != null) lstAttrs.Add(new CustomShugosinAttr(null, person.imigamiAttr));
            }
            return lstAttrs;
        }
    }
}
