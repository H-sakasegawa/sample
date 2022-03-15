using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    ///争財、争母、争官
    /// </summary>
    class JozaiJoboJokan
    {

        public static string GetJouzai(Person person, string optKan=null)
        {
            if (Judge(person, optKan, new string[] { "禄存星", "司禄星" }))
            {
                return "争財";
            }
            return "";
        }
        public static string GetJoubo(Person person, string optKan = null)
        {
            if (Judge(person, optKan, new string[] { "龍高星", "玉堂星" }))
            {
                return "争母";
            }
            return "";
        }
        public static string GetJoukan(Person person, string optKan = null)
        {
            if (Judge(person, optKan, new string[] { "車騎星", "牽牛星" }))
            {
                return "争官";
            }
            return "";
        }


        private static bool Judge(Person person ,string optKan, string [] targetJusei)
        {
            TableMng tblMng = TableMng.GetTblManage();

            string nitiKan = person.nikkansi.kan;
            string getuKan = person.gekkansi.kan;
            string nenKan = person.nenkansi.kan;


            //日干→月干
            string nitiGetu = tblMng.juudaiShusei.GetJudaiShuseiName(nitiKan, getuKan);
            if (targetJusei.Any(x => x == nitiGetu))
            {
                //年干→月干
                string nenGetu;                
                if(!string.IsNullOrEmpty(optKan))
                {
                    //大運、年運、月運の干 →月干
                    nenGetu = tblMng.juudaiShusei.GetJudaiShuseiName(optKan, getuKan);
                    if (targetJusei.Any(x => x == nenGetu))
                    {
                        return true;
                    }
                }
                else
                {
                    nenGetu = tblMng.juudaiShusei.GetJudaiShuseiName(nenKan, getuKan);
                    if (targetJusei.Any(x => x == nenGetu))
                    {
                        return true;
                    }

                }


            }
            else
            {
                //日干→年干
                string nitiNen = tblMng.juudaiShusei.GetJudaiShuseiName(nitiKan, nenKan);
                if (targetJusei.Any(x => x == nitiNen))
                {
                    string getuNen;
                    //月干→年干
                    if (!string.IsNullOrEmpty(optKan))
                    {
                        //大運、年運、月運の干 →年干
                        getuNen = tblMng.juudaiShusei.GetJudaiShuseiName(optKan, getuKan);
                        if (targetJusei.Any(x => x == getuNen))
                        {
                            return true;
                        }
                    }else
                    {
                        getuNen = tblMng.juudaiShusei.GetJudaiShuseiName(getuKan, nenKan);
                        if (targetJusei.Any(x => x == getuNen))
                        {
                            return true;
                        }

                    }
                }
            }
            return false;
        }
    }

}
