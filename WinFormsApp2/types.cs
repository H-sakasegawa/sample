using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WinFormsApp2
{
    /// <summary>
    /// 十二支 データ
    /// </summary>
    public class Jyunisi
    {
        public string name;
        public string gogyou;
        public string inyou;

        public Jyunisi( string _name, string _gogyou, string _inyou)
        {
            name = _name;
            gogyou = _gogyou;
            inyou = _inyou;

        }

    }
    /// <summary>
    /// 十干 データ
    /// </summary>
    public class Jyukan
    {
        public string name;
        public string gogyou;
        public string inyou;

        /// <summary>
        /// この干を生じるもの
        /// </summary>
        public string sFromName;
        /// <summary>
        /// この干から生じられるの
        /// </summary>
        public string sToName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_gogyou"></param>
        /// <param name="_inyou"></param>
        /// <param name="_sFromName">生じるもの</param>
        public Jyukan(string _name, string _gogyou, string _inyou, string _sFromName, string _sToName)
        {
            name = _name;
            gogyou = _gogyou;
            inyou = _inyou;
            sFromName = _sFromName;
            sToName = _sToName;

        }

    }

    /// <summary>
    /// 干支 データ
    /// </summary>
    public class Kansi
    {
        public int no;
        public string kan;
        public string si;
        public Tenchusatu tenchusatu;

        public Kansi(int _no, string _kan, string _si, string _tenchusatu)
        {
            no = _no;
            kan = _kan;
            si = _si;
            tenchusatu = new Tenchusatu(_tenchusatu);

        }
        public string[] ToArray()
        {
            return new string[] { kan, si };
        }
        public override string ToString()
        {
            return kan + si;
        }
        public Kansi Clone()
        {
            // Object型で返ってくるのでキャストが必要
            return (Kansi)MemberwiseClone();
        }
        public bool IsSame( Kansi kansi)
        {
            if (kan == kansi.kan && si == kansi.si) return true;

            return false;
        }

        public bool IsExist( string str)
        {
            if( kan==str||si==str)
            {
                return true;
            }
            return false;
        }
    }
    /// <summary>
    /// 天中殺データ
    /// </summary>
    public class Tenchusatu
    {
        public Tenchusatu(string _tenchusatu)
        {
            tencusatu = _tenchusatu;
        }
        public string tencusatu;
        public string this[int index]
        {
            get
            {
                return ToArray()[index];
            }
        }
        public string[] ToArray()
        {
            return tencusatu.Split(",");
        }
    }

    /// <summary>
    /// 二十八元表 データ
    /// </summary>
    public class NijuhachiGenso
    {
        public enum enmGensoType
        {
            GENSO_SHOGEN = 0,   //初元
            GENSO_CHUGEN,       //中元
            GENSO_HONGEN,       //本元
        }
        public Genso[] genso = new Genso[3];

        public NijuhachiGenso(Genso _shogen, Genso _chugen, Genso _hongen)
        {
            if (_shogen == null) _shogen = new Genso("");
            if (_chugen == null) _chugen = new Genso("");
            if (_hongen == null) _hongen = new Genso("");
            genso[0] = _shogen;
            genso[1] = _chugen;
            genso[2] = _hongen;
        }

        public enmGensoType GetTargetGensoType(int dayNumFromSetuiribi )
        {
            //dayNumFromSetuiribiは、節入り日からの経過日数（節入り日は含まない）
            //ここで使用する日数は、節入り日も含めるるため、＋１する
            dayNumFromSetuiribi++;

            int dayNum = 0;
            //節入日                                 次月の節入り日
            //  |---------------------------------------|-----------
            //  |------->  初元
            //           |------------->中元
            //                         |--------------->本元
            if (genso[0].dayNum != -1)
            {
                dayNum += genso[0].dayNum;
                if (dayNum >= dayNumFromSetuiribi) return enmGensoType.GENSO_SHOGEN;
            }
            if (genso[1].dayNum != -1)
            {
                dayNum += genso[1].dayNum;
                if (dayNum >= dayNumFromSetuiribi) return enmGensoType.GENSO_CHUGEN;
            }
            return enmGensoType.GENSO_HONGEN;
        }
    }
    /// <summary>
    /// 二十八元表の元素情報
    /// </summary>
    public class Genso
    {
        public Genso(string _name, int _dayNum = -1)
        {
            name = _name;
            dayNum = _dayNum;
        }
        public new string ToString() { return name; }
        /// <summary>
        /// 元素名
        /// </summary>
        public string name;
        /// <summary>
        /// 初元、中元 判定日
        /// </summary>
        public int dayNum;
    }

    /// <summary>
    /// 十大主星
    /// </summary>
    public class JudaiShusei
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name;
        /// <summary>
        /// 五行
        /// </summary>
        public string gogyou;
        /// <summary>
        /// 陰陽
        /// </summary>
        public string inyou;
        /// <summary>
        /// 純濁
        /// </summary>
        public string juntaku;
        /// <summary>
        /// サブキー
        /// </summary>
        public string[] jukan2;

        public JudaiShusei(string _name, string _gogyou, string _inyou, string _juntaku, string[] _jukan2)
        {
            name = _name;
            gogyou = _gogyou;
            inyou = _inyou;
            juntaku = _juntaku;
            jukan2 = _jukan2;
        }
    }

    /// <summary>
    /// 十二大従星
    /// </summary>
    public class JunidaiJusei
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name;
        /// <summary>
        /// 時代
        /// </summary>
        public string jidai;
        /// <summary>
        /// 点数
        /// </summary>
        public int tensuu;
        /// <summary>
        /// 強弱
        /// </summary>
        public string kyojaku;
        /// <summary>
        /// サブキー
        /// </summary>
        public string[] jukan2;

        public JunidaiJusei(string _name, string _jidai, int _tensuu,  string _kyojaku, string[] _jukan2)
        {
            name = _name;
            jidai = _jidai;
            tensuu = _tensuu;
            kyojaku = _kyojaku;
            jukan2 = _jukan2;
        }
    }


    /// <summary>
    /// 日干支天中殺の文字チェック対象ラベル組み合わせ
    /// </summary>
    public class TenchusatuLabelPair
    {
        public TenchusatuLabelPair(Label[] _aryLabel, Label[] _zokanLabel)
        {
            aryLabel = _aryLabel;
            aryZoukanLabel = _zokanLabel;
        }
        public bool IsExist(string s)
        {
            for (int i = 0; i < aryLabel.Length; i++)
            {
                if (aryLabel[i].Text == s) return true;
            }
            return false;
        }
        public Label GetSameLabel(string s)
        {
            for (int i = 0; i < aryLabel.Length; i++)
            {
                if (aryLabel[i].Text == s) return aryLabel[i];
            }
            return null;
        }
        public void SetColor(Color color)
        {
            for (int i = 0; i < aryLabel.Length; i++)
            {
                aryLabel[i].ForeColor = color;
            }
            for (int i = 0; i < aryZoukanLabel.Length; i++)
            {
                aryZoukanLabel[i].ForeColor = color;
            }
        }


        public Label[] aryLabel;
        public Label[] aryZoukanLabel;
    };

    public class TColor
    {
        public TColor(Color _color =default) 
        {
            if (color != default) { color = _color; } 
        }
        public Color color = Color.Black;
    }

    /// <summary>
    /// 日干支天中殺の文字チェック対象ラベル組み合わせ
    /// </summary>
    public class TenchusatuColorPair
    {
         public TenchusatuColorPair(TColor[] _aryKansiColor, TColor[] _zokanColor)
        {
            aryKansiColor = _aryKansiColor;
            aryZoukanColor = _zokanColor;
        }
        //public bool IsExist(string s)
        //{
        //    for (int i = 0; i < aryLabel.Length; i++)
        //    {
        //        if (aryLabel[i].Text == s) return true;
        //    }
        //    return false;
        //}
        //public Label GetSameLabel(string s)
        //{
        //    for (int i = 0; i < aryLabel.Length; i++)
        //    {
        //        if (aryLabel[i].Text == s) return aryLabel[i];
        //    }
        //    return null;
        //}
        public void SetColor(Color color)
        {
            for (int i = 0; i < aryKansiColor.Length; i++)
            {
                aryKansiColor[i].color = color;
            }
            for (int i = 0; i < aryZoukanColor.Length; i++)
            {
                aryZoukanColor[i].color = color;
            }
        }


        public TColor[] aryKansiColor;
        public TColor[] aryZoukanColor;
    };



    /// <summary>
    /// 干合
    /// </summary>
    public class Kangou
    {
        public Kangou(string _kangou1, string _kangou2, string _gogyou, string _kyoki1, string _kyoki2,  string _yomi)
        {
            sKangou1 = _kangou1;
            sKangou2 = _kangou2;
            gogyou = _gogyou;
            kyoki = new string[] { _kyoki1, _kyoki2 };
            yojm = _yomi;
        }
        /// <summary>
        /// 干合(陽）
        /// </summary>
        public string sKangou1 { get; set; }
        /// <summary>
        /// 干合(陰）
        /// </summary>
        public string sKangou2 { get; set; }
        /// <summary>
        /// 化気
        /// </summary>
        public string gogyou { get; set; }
        /// <summary>
        /// 虚気
        /// </summary>
        public string[] kyoki { get; set; }
        /// 読み
        /// </summary>
        public string yojm { get; set; }
    }


    /// <summary>
    /// 七殺
    /// </summary>
    public class Nanasatsu
    {
        public Nanasatsu( string _name1, string _name2)
        {
            name1 = _name1;
            name2 = _name2;
        }
        /// <summary>
        /// 七殺 するもの
        /// </summary>
        public string name1; 
        /// <summary>
        /// 七殺 されるもの
        /// </summary>
        public string name2;
    }


    /// <summary>
    /// 三合会局
    /// </summary>
    public class SangouKaikyoku
    {
        public SangouKaikyoku( string[] _names, string _gogyou)
        {
            names = _names;
            gogyou = _gogyou;
        }
        public bool IsExist(string name)
        {
            if (Array.IndexOf(names,name) >=0)
                return true;
            else
                return false;
        }
        public string[] names;
        public string gogyou;
    }
    /// <summary>
    /// 方三位
    /// </summary>
    public class HouSani
    {
        public HouSani(string[] _names, string _gogyou)
        {
            names = _names;
            gogyou = _gogyou;
        }
        public bool IsExist(string name)
        {
            if (Array.IndexOf(names, name) >= 0)
                return true;
            else
                return false;
        }
        public string[] names;
        public string gogyou;
    }
    /// <summary>
    /// 支合
    /// </summary>
    public class Sigou
    {
        public Sigou(string[] _names, string _gogyou, string _goryouSub)
        {
            names = _names;
            gogyou = _gogyou;
            goryouSub = _goryouSub;
        }
        public bool IsMatch(string name1, string name2)
        {
            if (names[0] == name1 & names[1] == name2) return true;
            if (names[1] == name1 & names[0] == name2) return true;
            return false;
        }
        public string[] names;
        public string gogyou;
        public string goryouSub;
    }
    /// <summary>
    /// 五徳
    /// </summary>
    public class Gotoku
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string attrName;
        /// <summary>
        /// サブキー
        /// </summary>
        public string[] gotoku;


    }
    /// <summary>
    /// 半会
    /// </summary>
    public class Hankai
    {
        public Hankai(string[] _names, string _gogyou)
        {
            names = _names;
            gogyou = _gogyou;
        }
        public bool IsMatch(string name1, string name2)
        {
            if (names[0] == name1 & names[1] == name2) return true;
            if (names[1] == name1 & names[0] == name2) return true;
            return false;
        }
        public string[] names;
        public string gogyou;

    }

    /// <summary>
    /// 五行属性テーブル項目
    /// </summary>
    public class AttrTblItem
    {
        public void Init(string _attrKan, string _attrSi)
        {
            attrKanOrg = attrKan = _attrKan;
            attrSiOrg = attrSi = _attrSi;
        }

        public string attrKanOrg;
        public string attrSiOrg;
        public string attrKan;
        public string attrSi;
    }

    //-------------------------------------------
    // 守護神テーブル項目
    //-------------------------------------------
    /// <summary>
    /// 守護神テーブル条件
    /// </summary>
    public enum EnmSugosinCond
    {
        None = 0,
        Do_Ari,        //土あり
        Do_Nasi,        //土なし
        Do_2num,        //土2つ
        Do_2numOver,    //土2つ以上
        Do_Weak,        //土弱
        Do_Strong,      //土強
        Do_Toukan,      //土性が透干 （追加）
        Hi_2num,        //火2つ
        Hi_3numOver,    //火3つ以上
        Mizu_1num,        //水1つ
        Mizu_2num,        //水2つ
        Mizu_Ari,       //水あり
        Mizu_Nasi,       //水なし
        Mizu_Weak,       //水弱
        Mizu_Strong,       //水強
        Moku_Strong,       //木強
        Moku_2numOver,     //木性が２つ以上 （追加）
        Tei_Strong,     //丁強
        Tei_Weak,     //丁弱（追加）
        Hei_Weak,     //丙弱
        Sin_Weak,     //辛弱
        Ki_Arii,      //宿命に癸水がある場合
        Ki_Weak,      //癸弱
        Ki_Toukan,      //癸水が透干 （追加）

        Do1Hi2,        //土1つ火２つ
        Hei_Nasi_and_Hi_Ari, //宿命に丙がなくかつ丁がある場合

        Natu_Mae,        //夏至前
        Natu_Ato,        //夏至後
        Aki_Mae,         //秋至前
        Aki_Ato,         //秋至後
        Fuyu_Mae,        //冬至前
        Fuyu_Ato,        //冬至後
    
    }

    /// <summary>
    /// 守護神情報
    /// </summary>
    public class ShugoSin
    {
        public ShugoSin( string[] _kan, EnmSugosinCond _cond, string _imi, string _explanation="")
        {
            kan = _kan;
            cond = _cond;
            imi = _imi;
            explanation = _explanation;
        }
        public override string ToString()
        {
            string s = "";
            foreach(var item in kan)
            {
                s += item;
            }
            return s;
        }
        /// <summary>
        /// 干文字組み合わせ
        /// </summary>
        public string[] kan;
        /// <summary>
        /// 条件
        /// </summary>
        public EnmSugosinCond cond;
        /// <summary>
        /// 忌
        /// </summary>
        public string imi;
        /// <summary>
        /// 説明文
        /// </summary>
        public string explanation;

    }

    public class GogyouAttrRelationship
    {
        public GogyouAttrRelationship(string _name, string _createFromName, string _createToName, string _destoryFromName, string _destoryToName)
        {
            attrName = _name;
            createFromName = _createFromName;
            createToName = _createToName;
            destoryFromName = _destoryFromName;
            destoryToName = _destoryToName;
        }

        public string attrName;
        public string createFromName; //attrNameを生むもの
        public string createToName;    //attrNameが生むもの

        public string destoryFromName; //attrNameを剋するもの
        public string destoryToName; //attrが剋するもの
    }

 


}

