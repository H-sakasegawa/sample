using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    ///全支集印
    /// </summary>
    class ZensiShuuin
    {

        public static string GetZensiShuuin(Person person)
        {
       
            TableMng tblMng = TableMng.GetTblManage();

            string nitiKan = person.nikkansi.kan;

            string[] arySi = new string[] { person.nikkansi.si, person.gekkansi.si, person.nenkansi.si };

            //日干の属性
            string nitiAttr = tblMng.jyukanTbl.GetGogyo(nitiKan);
            //日干を生じるもの(五行属性）
            string createFromAttr = tblMng.gogyouAttrRelationshipTbl.GetCreatFrom(nitiAttr);

            //全地生母
            //日支、月支、年支に日干を生じるものが全て存在する？
            bool bZentiSeibo = true;
            foreach( var si in arySi)
            {
                if(tblMng.jyunisiTbl.GetGogyo( si ) != createFromAttr)
                {
                    bZentiSeibo = false;
                    break;
                }
            }
            if (bZentiSeibo) return "全地生母";

            //全地生母(準)
            //蔵元の日、月、年のそれぞれに、日干を生じるものが存在する？
            Insen insen = new Insen(person);
            bool bZentiSeiboJun = false;
            int bFlg = 0;
            foreach (var item in Enum.GetValues(typeof(NijuhachiGenso.enmGensoType)))//初元、中元、本元
            {
                int idx = (int)item;
                string s = insen.nikkansiHongen[idx].name; //蔵元(日）
                if (!string.IsNullOrEmpty(s) && tblMng.jyukanTbl.GetGogyo(s) == createFromAttr)
                {
                    bFlg |= Const.bitFlgNiti;
                }
                s = insen.gekkansiHongen[idx].name; //蔵元(月）
                if (!string.IsNullOrEmpty(s) && tblMng.jyukanTbl.GetGogyo(s) == createFromAttr)
                {
                    bFlg |= Const.bitFlgGetu;
                }
                s = insen.nenkansiHongen[idx].name; //蔵元(年）
                if (!string.IsNullOrEmpty(s) && tblMng.jyukanTbl.GetGogyo(s) == createFromAttr)
                {
                    bFlg |= Const.bitFlgNen;
                }
                if( bFlg == (Const.bitFlgNiti | Const.bitFlgGetu | Const.bitFlgNen) )
                {
                    bZentiSeiboJun = true;
                    break;
                }
            }
            if (bZentiSeiboJun) return "全地生母(準)";
            
            //全地配偶者
            //蔵元の日、月、年のそれぞれに、日干と干合の関係のものが存在する？
            bool bZentiHaigusha = false;
            bFlg = 0;
            //日干と干合するもの
            string kangou = tblMng.kangouTbl.GetKangouOtherStr(nitiKan);

            foreach (var item in Enum.GetValues(typeof(NijuhachiGenso.enmGensoType)))//初元、中元、本元
            {
                int idx = (int)item;
                string s = insen.nikkansiHongen[idx].name; //蔵元(日）
                if (!string.IsNullOrEmpty(s) && s == kangou)
                {
                    bFlg |= Const.bitFlgNiti;
                }
                s = insen.gekkansiHongen[idx].name; //蔵元(月）
                if (!string.IsNullOrEmpty(s) && s == kangou)
                {
                    bFlg |= Const.bitFlgGetu;
                }
                s = insen.nenkansiHongen[idx].name; //蔵元(年）
                if (!string.IsNullOrEmpty(s) && s == kangou)
                {
                    bFlg |= Const.bitFlgNen;
                }
                if (bFlg == (Const.bitFlgNiti | Const.bitFlgGetu | Const.bitFlgNen))
                {
                    bZentiHaigusha = true;
                    break;
                }
            }
            if (bZentiHaigusha) return "全地配偶者";

            //全地配偶者(準)
            //蔵元の日、月、年のそれぞれに、日干と干合または干合の陰陽違いのものが存在する？
            bool bZentiHaigushaJun = false;
            bFlg = 0;
            //日干と干合するものの陰陽違い
            string kangouInyou = tblMng.jyukanTbl.GetInyouOtherString(nitiKan);

            foreach (var item in Enum.GetValues(typeof(NijuhachiGenso.enmGensoType)))//初元、中元、本元
            {
                int idx = (int)item;
                string s = insen.nikkansiHongen[idx].name; //蔵元(日）
                if (!string.IsNullOrEmpty(s) && ( s == kangou || s == kangouInyou))
                {
                    bFlg |= Const.bitFlgNiti;
                }
                s = insen.gekkansiHongen[idx].name; //蔵元(月）
                if (!string.IsNullOrEmpty(s) && (s == kangou || s == kangouInyou))
                {
                    bFlg |= Const.bitFlgGetu;
                }
                s = insen.nenkansiHongen[idx].name; //蔵元(年）
                if (!string.IsNullOrEmpty(s) && (s == kangou || s == kangouInyou))
                {
                    bFlg |= Const.bitFlgNen;
                }
                if (bFlg == (Const.bitFlgNiti | Const.bitFlgGetu | Const.bitFlgNen))
                {
                    bZentiHaigushaJun = true;
                    break;
                }
            }
            if (bZentiHaigushaJun) return "全地配偶者(準)";
            return "";
        }
    }

}
