using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    /// 純濁法
    /// </summary>
    class JundakuHou
    {

        public static string GetJundakuHou(Person person )
        {
            //純星数
            int JunCnt = 0;
            //濁星数
            int DakuCnt = 0;

            string result = "";

            foreach(var item in person.judaiShuseiAry)
            {
                if (item.juntaku == "純") JunCnt++;
                else DakuCnt++;
            }
            if(JunCnt> DakuCnt)
            {
                result = "純の宿命";
            }
            else
            {
                result = "濁の宿命";
            }
            return string.Format("{0}({1}純 {2}濁)", result, JunCnt, DakuCnt);
        }

    }
}
