using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    class Hongen
    {
        public Hongen( string _name, bool _bJudaiShuseiGenso)
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

        public Hongen[] nikkansiHongen = null; 
        public Hongen[] gekkansiHongen = null;
        public Hongen[] nenkansiHongen = null;

        public Hongen[] getuunHongen = null;
        public Hongen[] nenunHongen = null;
        public Hongen[] taiuniHongen = null;


        public Insen(Person _person)
        {
            person = _person;

            int num = Enum.GetValues(typeof(NijuhachiGenso.enmGensoType)).Length;
            nikkansiHongen = new Hongen[num];
            gekkansiHongen = new Hongen[num];
            nenkansiHongen = new Hongen[num];

            getuunHongen = new Hongen[num];
            nenunHongen = new Hongen[num];
            taiuniHongen = new Hongen[num];

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
                nikkansiHongen[(int)Value] = new Hongen( nijuhachiGensoNikkansi.genso[(int)Value].name, bBold);

                bBold = (idxGekkansiGensoType == (int)Value) ? true : false;
                gekkansiHongen[(int)Value] = new Hongen(nijuhachiGensoGekkansi.genso[(int)Value].name, bBold);

                bBold = (idxNenkaisiGensoType == (int)Value) ? true : false;
                nenkansiHongen[(int)Value] = new Hongen(nijuhachiGensoNenkansi.genso[(int)Value].name, bBold);



            }

        }

        public string[] GetNikkansiHongen()
        {
            string[] names = new string[nikkansiHongen.Length];
            for (int i = 0; i < nikkansiHongen.Length; i++)
            {
                names[i] = nikkansiHongen[i].name;
            }
            return names;
        }
        public string[] GetGekkansiHongen()
        {
            string[] names = new string[gekkansiHongen.Length];
            for (int i = 0; i < gekkansiHongen.Length; i++)
            {
                names[i] = gekkansiHongen[i].name;
            }
            return names;
        }
        public string[] GetNenkansiHongen()
        {
            string[] names = new string[nenkansiHongen.Length];
            for (int i = 0; i < nenkansiHongen.Length; i++)
            {
                names[i] = nenkansiHongen[i].name;
            }
            return names;
        }

        public Hongen[] GetHongen(Kansi kansi)
        {
            if (kansi == null) return null;

           var  tblMng = TableMng.GetTblManage();
           
            var tblSetuiribi = tblMng.setuiribiTbl;

           var nijuhachiGenso =  tblMng.nijuhachiGensoTbl[kansi.si];

            Hongen[] hongen = new Hongen[3];
            foreach (var Value in Enum.GetValues(typeof(NijuhachiGenso.enmGensoType)))//初元、中元、本元
            {
                hongen[(int)Value] = new Hongen(nijuhachiGenso.genso[(int)Value].name, false);
            }

            return hongen;
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

    /// <summary>
    /// 陰占詳細データ
    /// </summary>
    class InsenDetail
    {
        public InsenDetail(string s, Const.InsenDetailType _type)
        {
            sText = s;
            type = _type;
        }
        public Const.InsenDetailType type;
        public string sText;

        public override string ToString() { return sText; }
    }

}
