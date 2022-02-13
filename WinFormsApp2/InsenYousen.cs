using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp2
{

    //==================================================
    // 陰占、陽占特徴表示リストデータ
    //==================================================
    /// <summary>
    /// 陰占陽線 詳細データ基本
    /// </summary>
    class DetailBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_sKey">項目キー名称</param>
        /// <param name="_sType">データタイプ</param>
        public DetailBase(string _dispText, string _sKey, string _sType)
        {
            sKey = _sKey;
            sType = _sType;
            sText = _dispText;
        }
        public override string ToString() { return sText; }
        string sKey;
        string sType;
        public string sText;
    }
    /// <summary>
    /// 陰占詳細データ
    /// </summary>
    class InsenDetail : DetailBase
    {
        public InsenDetail(string sDispText, string sExpessionKey, string sExpressionType, Const.InsenDetailType _type)
            : base(sDispText, sExpessionKey, sExpressionType)
        {
            type = _type;
        }
        public Const.InsenDetailType type;
    }

    /// <summary>
    /// 陽占詳細データ
    /// </summary>
    class YousenDetail : DetailBase
    {
        public YousenDetail(string sDispText, string sExpessionKey, string sExpressionType, Const.YousenDetailType _type)
            : base(sDispText, sExpessionKey, sExpressionType)
        {
            type = _type;
        }
        public Const.YousenDetailType type;
    }

    //==================================================
    // 陰占 データ処理
    //==================================================
    /// <summary>
    /// 本元
    /// </summary>
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
                nikkansiHongen[(int)Value] = new Hongen(nijuhachiGensoNikkansi.genso[(int)Value].name, bBold);

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

            var tblMng = TableMng.GetTblManage();

            var tblSetuiribi = tblMng.setuiribiTbl;

            var nijuhachiGenso = tblMng.nijuhachiGensoTbl[kansi.si];

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


        /// <summary>
        /// 陰占 特徴データ表示
        /// </summary>
        /// <param name="person"></param>
        public void DispInsenDetailInfo(Person person, ListBox listBox)
        {
            string sExpressionType = "陰占特徴";
            string str = "";

            listBox.Items.Clear();
            //三角暗合
            if (person.IsSankakuAngou())
            {
                str = "三角暗合";
                listBox.Items.Add(new InsenDetail(str, str, sExpressionType, Const.InsenDetailType.INSEN_DETAIL_SANKAKUANGOU));
            }

            KyokiToukan kyokiToukan = new KyokiToukan();
            if (kyokiToukan.IsKyokiTokan_Shukumei(person))
            {
                str = "虚気透干";
                listBox.Items.Add(new InsenDetail(str, str, sExpressionType, Const.InsenDetailType.INSEN_DETAIL_KYOKITOUKAN0));
            }
        }
    }

    //==================================================
    //陽占 データ処理
    //==================================================
    class Yousen
    {
        Person person;
        public Yousen(Person _person)
        {
            person = _person;
        }
        /// <summary>
        /// 陽占 特徴表示
        /// </summary>
        /// <param name="person"></param>
        public void DispYousennDetailInfo(ListBox listBox)
        {
            string sExpressionType = "陽占特徴";
            string str = "";
            listBox.Items.Clear();

            //局法（凶運）表示
            Kyokuhou kyokuHou = new Kyokuhou();
            var result = kyokuHou.GetKyouUn(person);
            foreach (var item in result)
            {
                str = item.name;
                str += string.Format("({0})", LevelToStr(item.chkResult.level));
                if (item.chkResult.matchCount > 1) str += string.Format("[{0}]", item.chkResult.matchCount);


                listBox.Items.Add(new YousenDetail(str, str, sExpressionType, Const.YousenDetailType.INSEN_DETAIL_KYOKUHOU_KYOUN));
            }

            //局法（幸運）表示
            result = kyokuHou.GetKouUn(person);
            foreach (var item in result)
            {
                str = item.name;
                if (item.chkResult.matchCount > 1) str += string.Format("[{0}]", item.chkResult.matchCount);
                listBox.Items.Add(new YousenDetail(str, str, sExpressionType, Const.YousenDetailType.INSEN_DETAIL_KYOKUHOU_KOUUN));
            }
            //別格表示
            result = kyokuHou.GetBekkakku(person);
            foreach (var item in result)
            {
                str = item.name;
                listBox.Items.Add(new YousenDetail(str, str, sExpressionType, Const.YousenDetailType.INSEN_DETAIL_BEKKAKU));
            }
            //特殊五局表示
            result = kyokuHou.GetTokushuGokyoku(person);
            foreach (var item in result)
            {
                str = item.name;
                listBox.Items.Add(new YousenDetail(str, str, sExpressionType, Const.YousenDetailType.INSEN_DETAIL_TOKUSHU_GOKYOKU));
            }
            //純濁法
            str = JundakuHou.GetJundakuHou(person);
            listBox.Items.Add(new YousenDetail(str, str, sExpressionType, Const.YousenDetailType.INSEN_DETAIL_JUNDAKU));

            //循環法
            string sisei="", kisei = "";
            bool isGogyoJunkan = JunkanHou.GetJunkanHou(person, ref sisei, ref kisei);
            if (!string.IsNullOrEmpty(sisei))
            {
                str = string.Format("始星 : {0}", sisei);
                listBox.Items.Add(new YousenDetail(str, str, sExpressionType, Const.YousenDetailType.INSEN_DETAIL_JUNKAN));
                str = string.Format("帰星 : {0}", kisei);
                listBox.Items.Add(new YousenDetail(str, str, sExpressionType, Const.YousenDetailType.INSEN_DETAIL_JUNKAN));
            }

            //身強、身中、身弱
            str = MikyouMichuMijaku.GetMikyouMichuMijaku(person);
            listBox.Items.Add(new YousenDetail(str, str, sExpressionType, Const.YousenDetailType.INSEN_DETAIL_MIKYO_MICHU_MIJSKU));
        }

        string LevelToStr(Kyokuhou.Level level)
        {
            switch (level)
            {
                case Kyokuhou.Level.Weak: return "弱";
                case Kyokuhou.Level.Medium: return "中";
                case Kyokuhou.Level.Strong: return "強";
            }
            return "";
        }
    }
}
