using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    /// 身強、身中、身弱
    /// </summary>
    class MikyouMichuMijaku
    {
        public static string GetMikyouMichuMijaku(Person person )
        {

            int kyou = 0;
            int chuu = 0;
            int jaku = 0;
            bool bFlg = false;
            if (person.junidaiJuseiA.kyojaku == "強") kyou++;
            if (person.junidaiJuseiA.kyojaku.Contains("中"))
            {
                chuu++;
                if (person.junidaiJuseiA.name == "天印星" || 
                    person.junidaiJuseiA.name == "天庫星")bFlg = true;
            }
            if (person.junidaiJuseiA.kyojaku == "強") jaku++;

            if (person.junidaiJuseiB.kyojaku == "強") kyou++;
            if (person.junidaiJuseiB.kyojaku.Contains("中"))
            {
                chuu++;
                if (person.junidaiJuseiB.name == "天印星" || 
                    person.junidaiJuseiB.name == "天庫星")bFlg = true;
            }
            if (person.junidaiJuseiB.kyojaku == "強") jaku++;

            if (person.junidaiJuseiC.kyojaku == "強") kyou++;
            if (person.junidaiJuseiC.kyojaku.Contains("中"))
            {
                chuu++;
                if (person.junidaiJuseiC.name == "天印星" ||
                   person.junidaiJuseiC.name == "天庫星") bFlg = true;
            }
            if (person.junidaiJuseiC.kyojaku == "強") jaku++;

            if (kyou >= 2) return "最身強";
            if (kyou >= 1) return "身強";
            if (jaku == 3) return "最身弱";
            if (chuu == 1 && jaku == 2) return "身弱";
            if (chuu == 3) return "身中";
            // 以下chuu==2
            if( bFlg) return "身弱";
            else return "身中";
        }
    }
}
