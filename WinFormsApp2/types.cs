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
    class Jyunisi
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
    class Jyukan
    {
        public string name;
        public string gogyou;
        public string inyou;

        public Jyukan(string _name, string _gogyou, string _inyou)
        {
            name = _name;
            gogyou = _gogyou;
            inyou = _inyou;

        }

    }

    /// <summary>
    /// 干支 データ
    /// </summary>

    class Kansi
    {
        public int no;
        public string kan;
        public string si;
        public string tenchusatu;

        public Kansi(int _no, string _kan, string _si, string _tenchusatu)
        {
            no = _no;
            kan = _kan;
            si = _si;
            tenchusatu = _tenchusatu;

        }
        public string[] GetArray()
        {
            return new string[] { kan, si };
        }
        public override string ToString()
        {
            return kan + si;
        }

    }

    /// <summary>
    /// 二十八元表 データ
    /// </summary>
    class NijuhachiGenso
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

            //節入日                                 次月の節入り日
            //  |---------------------------------------|-----------
            //  |------->  初元
            //           |------------->中元
            //                         |--------------->本元
            if (genso[0] != null)
            {
                if (genso[0].dayNum >= dayNumFromSetuiribi) return enmGensoType.GENSO_SHOGEN;
            }
            if (genso[1] != null)
            {
                if (genso[1].dayNum >= dayNumFromSetuiribi) return enmGensoType.GENSO_CHUGEN;
            }

            return enmGensoType.GENSO_HONGEN;
        }
    }
    class Genso
    {
        public Genso(string _name, int _dayNum = -1)
        {
            name = _name;
            dayNum = _dayNum;
        }
        public new string ToString() { return name; }
        public string name;
        public int dayNum;
    }

    /// <summary>
    /// 十大主星 データ
    /// </summary>
    class JudaiShusei
    {

        /// <summary>
        /// 主キー
        /// </summary>
        public string[] jukan1;
        public List<JudaiShuseiItem> lstJudaiShusei;

        public string GetJudaiShuseiName(string key1, string key2)
        {
            var value = GetJudaiShusei(key1, key2);
            if (value == null) return null;
            
            return value.name;
        }

        public JudaiShuseiItem GetJudaiShusei(string key1, string key2)
        { 
            //主キーのインデックス番号取得
            for( int idxItem=0; idxItem < jukan1.Length; idxItem++)
            {
                if(jukan1[idxItem] == key1)
                {
                    for (int i = 0; i < lstJudaiShusei.Count; i++)
                    {
                        if(lstJudaiShusei[i].jukan2[idxItem] == key2)
                        {
                            return lstJudaiShusei[i];
                        }
                    }
                }

            }
            return null;
        }


    }
    class JudaiShuseiItem
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

        public JudaiShuseiItem(string _name, string _gogyou, string _inyou, string _juntaku, string[] _jukan2)
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
    class JunidaiJusei
    {
        /// <summary>
        /// 主キー
        /// </summary>
        public string[] jukan1;
        public List<JunidaiJuseiItem> lstJunidaiJusei;


        public string GetJunidaiJuseiName(string key1, string key2)
        {
            var value = GetJunidaiJusei(key1, key2);
            if (value == null) return null;

            return value.name;
        }
        public JunidaiJuseiItem GetJunidaiJusei(string key1, string key2)
        {

            //主キーのインデックス番号取得
            for (int idxItem = 0; idxItem < jukan1.Length; idxItem++)
            {
                if (jukan1[idxItem] == key1)
                {
                    for (int i = 0; i < lstJunidaiJusei.Count; i++)
                    {
                        if (lstJunidaiJusei[i].jukan2[idxItem] == key2)
                        {
                            return lstJunidaiJusei[i];
                        }
                    }
                }

            }
            return null;
        }

    }
    class JunidaiJuseiItem
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

        public JunidaiJuseiItem(string _name, string _jidai, int _tensuu,  string _kyojaku, string[] _jukan2)
        {
            name = _name;
            jidai = _jidai;
            tensuu = _tensuu;
            kyojaku = _kyojaku;
            jukan2 = _jukan2;
        }
    }


    //天中殺
    class TenchusatuLabelPair
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




}
