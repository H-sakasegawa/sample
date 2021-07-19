using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    class hongen
    {
        public hongen( string _name, bool _bJudaiShuseiGenso)
        {
            name = _name;
            bJudaiShuseiGenso = _bJudaiShuseiGenso;
        }
        public bool bJudaiShuseiGenso;
        public string name;
    }
    /// <summary>
    /// 陰占 表示データ管理クラス
    /// </summary>
    class Insen
    {
        Person person;

        public Kansi nikkansi;
        public Kansi gekkansi;
        public Kansi nenkansi;

        public hongen[] nikkansiHongen = null;
        public hongen[] gekkansiHongen = null;
        public hongen[] nenkansiHongen = null;

        public Insen(Person _person)
        {
            person = _person;

            int num = Enum.GetValues(typeof(NijuhachiGenso.enmGensoType)).Length;
            nikkansiHongen = new hongen[num];
            gekkansiHongen = new hongen[num];
            nenkansiHongen = new hongen[num];


            //干支
            nikkansi = person.nikkansi;
            gekkansi = person.gekkansi;
            nenkansi = person.nenkansi;



            NijuhachiGenso nijuhachiGensoNikkansi = person.nijuhachiGensoNikkansi;
            NijuhachiGenso nijuhachiGensoGekkansi = person.nijuhachiGensoGekkansi;
            NijuhachiGenso nijuhachiGensoNenkansi = person.nijuhachiGensoNenkansi;

            //十大主星判定用基準元素
            var idxNikkansiGensoType = (int)nijuhachiGensoNikkansi.GetTargetGensoType(person.dayNumFromSetuiribi);
            var idxGekkansiGensoType = (int)nijuhachiGensoGekkansi.GetTargetGensoType(person.dayNumFromSetuiribi);
            var idxNenkaisiGensoType = (int)nijuhachiGensoNenkansi.GetTargetGensoType(person.dayNumFromSetuiribi);

            foreach (var Value in Enum.GetValues(typeof(NijuhachiGenso.enmGensoType)))//初元、中元、本元
            {

                bool bBold = (idxNikkansiGensoType == (int)Value) ? true : false;
                nikkansiHongen[(int)Value] = new hongen( nijuhachiGensoNikkansi.genso[(int)Value].name, bBold);

                bBold = (idxGekkansiGensoType == (int)Value) ? true : false;
                gekkansiHongen[(int)Value] = new hongen(nijuhachiGensoGekkansi.genso[(int)Value].name, bBold);

                bBold = (idxNenkaisiGensoType == (int)Value) ? true : false;
                nenkansiHongen[(int)Value] = new hongen(nijuhachiGensoNenkansi.genso[(int)Value].name, bBold);

            }

        }

        //陰占の文字に含まれているか？
        public bool IsExist(string s)
        {
            if (nikkansi.kan == s || nikkansi.si == s) return true;
            if (gekkansi.kan == s || gekkansi.si == s) return true;
            if (nenkansi.kan == s || nenkansi.si == s) return true;


            var item = nikkansiHongen.FirstOrDefault(x => x.name == s);
            if (item != null) return true;
            item = gekkansiHongen.FirstOrDefault(x => x.name == s);
            if (item != null) return true;
            item = nenkansiHongen.FirstOrDefault(x => x.name == s);
            if (item != null) return true;

            return false;
        }
    }
}
