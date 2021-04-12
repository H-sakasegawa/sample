﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string name;
        public string[] genso =new string[3];

        public NijuhachiGenso(string _junisi, string _shogen, string _chuugen, string _hongen)
        {
            name = _junisi;
            genso[0] = _shogen;
            genso[1] = _chuugen;
            genso[2] = _hongen;
        }
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


}
