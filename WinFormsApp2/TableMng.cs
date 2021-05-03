using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    class TableMng
    {

        /// <summary>
        /// 十干 管理テーブル
        /// </summary>
        public class JyukanTbl
        {
            public Dictionary<string, Jyukan> dicJyukan = null;

            public Jyukan this[string name]
            {
                get
                {
                    if (!dicJyukan.ContainsKey(name)) return null;
                    return dicJyukan[name];
                }
            }
            public int Count { get { return dicJyukan.Count; } }
            public List<Jyukan> ToList()
            {
                return dicJyukan.Values.ToList();
            }
            /// <summary>
            /// 指定した２の十干名称の組み合わせが陰陽の関係かチェック
            /// </summary>
            /// <param name="kan1">干名称1</param>
            /// <param name="kan2">干名称2</param>
            /// <returns></returns>
            public bool IsInyou(string kan1, string kan2)
            {
                var jyukan1 = this[kan1];
                var jyukan2 = this[kan2];
                if (jyukan1 == null || jyukan2 == null) return false;

                //同じ五行か？
                if (jyukan1.gogyou != jyukan2.gogyou) return false;
                //陰陽の関係か？
                if (jyukan1.inyou != jyukan2.inyou) return true;

                return false;

            }
        }
        public JyukanTbl jyukanTbl = new JyukanTbl();


        /// <summary>
        /// 十二支 テーブル管理
        /// </summary>
        public class JyunisiTbl
        {
            public Dictionary<string, Jyunisi> dicJyunisi = null;

            public Jyunisi this[string name]
            {
                get
                {
                    if (!dicJyunisi.ContainsKey(name)) return null;
                    return dicJyunisi[name];
                }
            }
            public int Count { get { return dicJyunisi.Count; } }
            public List<Jyunisi> ToList()
            {
                return dicJyunisi.Values.ToList();
            }
        }
        public JyunisiTbl jyunisiTbl = new JyunisiTbl();

        /// <summary>
        /// 干支 テーブル管理
        /// </summary>
        public class KansiTbl
        {
            public Dictionary<int, Kansi> dicKansi = null;

            public Kansi this[int kansiNo]
            {
                get
                {
                    return GetKansi(kansiNo);
                }
            }
            public int Count { get { return dicKansi.Count; } }

            /// <summary>
            /// 干支文字列から干支番号取得
            /// </summary>
            /// <param name="kansi">干支</param>
            /// <returns></returns>
            public int GetKansiNo(Kansi kansi)
            {
                return GetKansiNo(kansi.kan, kansi.si);
            }
            /// <summary>
            /// 干支文字列から干支番号取得
            /// </summary>
            /// <param name="kansi">干支の名称文字列配列</param>
            /// <returns></returns>
            public int GetKansiNo(string[] kansi)
            {
                return GetKansiNo(kansi[0], kansi[1]);
            }
            /// <summary>
            /// 干支文字列から干支番号取得
            /// </summary>
            /// <param name="kan">干名称</param>
            /// <param name="si">支名称</param>
            /// <returns></returns>
            public int GetKansiNo(string kan, string si)
            {
                return dicKansi.First(x => x.Value.kan == kan && x.Value.si == si).Key;
            }
            /// <summary>
            /// 干支番号から干支取得
            /// </summary>
            /// <param name="kansiNo"></param>
            /// <returns></returns>
            public Kansi GetKansi(int kansiNo)
            {
                if (!dicKansi.ContainsKey(kansiNo)) return null;
                var kansi = dicKansi[kansiNo];
                return kansi;
            }
            /// <summary>
            /// 干支番号から干支文字取得
            /// </summary>
            /// <param name="kansiNo">干支No</param>
            /// <returns></returns>
            public string[] GetKansiStr(int kansiNo)
            {
                Kansi kansi = GetKansi(kansiNo);
                if (kansi == null) return null;

                return kansi.ToArray();
            }

        }
        public KansiTbl kansiTbl = new KansiTbl();


        /// <summary>
        /// 二十八元表
        /// </summary>
        public class NijuhaciGensoTbl
        {
            public Dictionary<string, NijuhachiGenso> dicNijuhachiGenso = null;

            public NijuhachiGenso this[string siName]
            {
                get
                {
                    if (!dicNijuhachiGenso.ContainsKey(siName)) return null;
                    return dicNijuhachiGenso[siName];
                }
            }

        }
        public NijuhaciGensoTbl nijuhachiGensoTbl = null;

        /// <summary>
        /// 十大主星  管理テーブル
        /// </summary>
        public class JudaiShuseiTbl
        {
            /// <summary>
            /// 主キー
            /// </summary>
            public string[] jukan1;
            public List<JudaiShusei> lstJudaiShusei;

            public string GetJudaiShuseiName(string key1, string key2)
            {
                var value = GetJudaiShusei(key1, key2);
                if (value == null) return null;

                return value.name;
            }

            public JudaiShusei GetJudaiShusei(string key1, string key2)
            {
                //主キーのインデックス番号取得
                for (int idxItem = 0; idxItem < jukan1.Length; idxItem++)
                {
                    if (jukan1[idxItem] == key1)
                    {
                        for (int i = 0; i < lstJudaiShusei.Count; i++)
                        {
                            if (lstJudaiShusei[i].jukan2[idxItem] == key2)
                            {
                                return lstJudaiShusei[i];
                            }
                        }
                    }

                }
                return null;
            }


        }
        public JudaiShuseiTbl juudaiShusei = null;
       
        /// <summary>
        /// 十二大従星 管理テーブル 管理
        /// </summary>
        public class JunidaiJuseiTbl
        {
            /// <summary>
            /// 主キー
            /// </summary>
            public string[] jukan1;
            public List<JunidaiJusei> lstJunidaiJusei;


            public string GetJunidaiJuseiName(string key1, string key2)
            {
                var value = GetJunidaiJusei(key1, key2);
                if (value == null) return null;

                return value.name;
            }
            public JunidaiJusei GetJunidaiJusei(string key1, string key2)
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
        public JunidaiJuseiTbl junidaiJusei = null;

 
        /// <summary>
        /// 干合テーブル管理
        /// </summary>
        public class KangouTbl
        {
            public List<Kangou> lstKangou = null;

            public Kangou GetKangou(string kan1, string kan2)
            {
                foreach (var val in lstKangou)
                {
                    if (val.kan == kan1 && val.gou == kan2 ||
                        val.kan == kan2 && val.gou == kan1)
                    {
                        return val;
                    }
                }
                return null;
            }
            public bool IsKangou(string kan1, string kan2)
            {
                return GetKangou(kan1, kan2) != null ? true : false;

            }
        }
        public KangouTbl kangouTbl = new KangouTbl();

        /// <summary>
        /// 七殺テーブル 管理
        /// </summary>
        public class NanasatsuTbl
        {
            public List<Nanasatsu> lstNanasatsu = null;

            /// <summary>
            /// 七殺 取得
            /// </summary>
            /// <param name="kan1">干名称1</param>
            /// <param name="kan2">干名称2</param>
            /// <returns></returns>
            public Nanasatsu GetNanasatsu(string kan1, string kan2)
            {
                foreach (var val in lstNanasatsu)
                {
                    if (val.name1 == kan1 && val.name2 == kan2)
                    {
                        return val;
                    }
                }
                return null;
            }
            /// <summary>
            /// 七殺に該当するかを判定
            /// </summary>
            /// <param name="kan1">干名称1</param>
            /// <param name="kan2">干名称2</param>
            /// <returns>true...七殺  false...該当なし</returns>
            public bool IsNanasastsu(string kan1, string kan2)
            {
                return GetNanasatsu(kan1, kan2) != null ? true : false;

            }

        }
        public NanasatsuTbl nanasatsuTbl = new NanasatsuTbl();

        /// <summary>
        /// 合法・散法 テーブル　管理
        /// </summary>
        public class GouhouSanpouTbl
        {
            /// <summary>
            /// 主キー
            /// </summary>
            public string[] jyunisi;
            public Dictionary<string, string[]> dicGouhouSanpou;
            /// <summary>
            /// 干支の支名称の組み合わせから合法・散法テーブルに登録されている文字列を取得
            /// </summary>
            /// <param name="siName1">支名称1</param>
            /// <param name="siName2">支名称2</param>
            /// <returns></returns>
            public string[] GetGouhouSanpou(string siName1, string siName2)
            {
                int idx = 0;
                for (idx = 0; idx < jyunisi.Length; idx++)
                {
                    if (siName1 == jyunisi[idx])
                    {
                        break;
                    }
                }
                if (idx >= jyunisi.Length) return null;

                if (!dicGouhouSanpou.ContainsKey(siName2)) return null;

                string value = dicGouhouSanpou[siName2][idx];

                if(value=="") return null;
                return value.Split(",");


            }
            /// <summary>
            /// 干支の支名称の組み合わせから合法・散法テーブルに登録されている文字列をカンマ区切りで取得
            /// </summary>
            /// <param name="siName1">支名称1</param>
            /// <param name="siName2">支名称2</param>
            /// <returns></returns>
            public string GetGouhouSanpouString(string siName1, string siName2)
            {
                var values = GetGouhouSanpou(siName1, siName2);
                string s = "";
                if (values != null)
                {
                    foreach (var item in values)
                    {
                        if (s != "") s += ",";
                        s += item;
                    }
                }
                return s;
            }

        }
        public GouhouSanpouTbl gouhouSanpouTbl = null;


        /// <summary>
        /// 三合会局 テーブル 管理
        /// </summary>
        public class SangouKaikyokuTbl
        {
            public List<SangouKaikyoku> lstSangouKaikyoku = null;
        }
        public SangouKaikyokuTbl sangouKaikyokuTbl = new SangouKaikyokuTbl();

        /// <summary>
        /// 方三位 テーブル　管理
        /// </summary>
        public class HouSanniTbl
        {
            public List<HouSanni> lstHousanni = null;
        }
        public HouSanniTbl housanniTbl = new HouSanniTbl();

        /// <summary>
        /// 支合 テーブル
        /// </summary>
        public class SigouTbl
        {
            public List<Sigou> lstSigou = null;
        }
        public SigouTbl sigouTbl = new SigouTbl();


        public TableMng()
        {
            //--------------------------------
            //十干
            //--------------------------------
            jyukanTbl.dicJyukan = new Dictionary<string, Jyukan>
            {
                {"甲", new Jyukan("甲","木","+") },
                {"乙", new Jyukan("乙","木","-") },
                {"丙", new Jyukan("丙","火","+") },
                {"丁", new Jyukan("丁","火","-") },
                {"戊", new Jyukan("戊","土","+") },
                {"己", new Jyukan("己","土","-") },
                {"庚", new Jyukan("庚","金","+") },
                {"辛", new Jyukan("辛","金","-") },
                {"壬", new Jyukan("壬","水","+") },
                {"癸", new Jyukan("癸","水","-") },
            };
            //--------------------------------
            //十二支
            //--------------------------------
            jyunisiTbl.dicJyunisi = new Dictionary<string, Jyunisi>
            {
                {"子", new Jyunisi("子","水","+") },
                {"丑", new Jyunisi("丑","土","-") },
                {"寅", new Jyunisi("寅","木","+") },
                {"卯", new Jyunisi("卯","木","-") },
                {"辰", new Jyunisi("辰","土","+") },
                {"巳", new Jyunisi("巳","火","-") },
                {"午", new Jyunisi("午","火","+") },
                {"未", new Jyunisi("未","土","-") },
                {"申", new Jyunisi("申","金","+") },
                {"酉", new Jyunisi("酉","金","-") },
                {"戌", new Jyunisi("戌","土","+") },
                {"亥", new Jyunisi("亥","水","-") },
            };

            //--------------------------------
            //干支            
            //--------------------------------
            string[] tenchusatu = { "戌,亥", "申,酉", "午,未", "辰,巳", "寅,卯", "子,丑" };//天中殺
            kansiTbl.dicKansi = new Dictionary<int, Kansi>();
            var lstJyukan = jyukanTbl.ToList();
            var lstJyunisi = jyunisiTbl.ToList();
            for (int i = 0; i < 60; i++)
            {
                string kan = lstJyukan[i % jyukanTbl.Count].name;
                string si = lstJyunisi[i % jyunisiTbl.Count].name;
                string techu = tenchusatu[i / 10];

                int No = i + 1;
                kansiTbl.dicKansi.Add(No, new Kansi(No, kan, si, techu));

            }

            //--------------------------------
            //二十八元表
            //--------------------------------
            nijuhachiGensoTbl = new NijuhaciGensoTbl();
            nijuhachiGensoTbl.dicNijuhachiGenso = new Dictionary<string, NijuhachiGenso>
            {
                { "子",new NijuhachiGenso(null               ,null               ,new Genso("癸") ) },
                { "丑",new NijuhachiGenso(new Genso("癸", 9) ,new Genso("辛", 3) ,new Genso("己") ) },
                { "寅",new NijuhachiGenso(new Genso("戊", 7) ,new Genso("丙", 7) ,new Genso("甲") ) },
                { "卯",new NijuhachiGenso(null               ,null               ,new Genso("乙") ) },
                { "辰",new NijuhachiGenso(new Genso("乙", 9) ,new Genso("癸", 3) ,new Genso("戊") ) },
                { "巳",new NijuhachiGenso(new Genso("戊", 5) ,new Genso("庚", 9) ,new Genso("丙") ) },
                { "午",new NijuhachiGenso(null               ,new Genso("己",19) ,new Genso("丁") ) },
                { "未",new NijuhachiGenso(new Genso("丁",9)  ,new Genso("乙", 3) ,new Genso("己") ) },
                { "申",new NijuhachiGenso(new Genso("戊",10) ,new Genso("壬", 3) ,new Genso("庚") ) },
                { "酉",new NijuhachiGenso(null               ,null               ,new Genso("辛") ) },
                { "戌",new NijuhachiGenso(new Genso("辛", 9) ,new Genso("丁", 3) ,new Genso("戊") ) },
                { "亥",new NijuhachiGenso(new Genso("甲",12) ,null               ,new Genso("壬") ) },
            };

            //--------------------------------
            //十大主星
            //--------------------------------
            //主キーを登録
            juudaiShusei = new JudaiShuseiTbl();
            juudaiShusei.jukan1 = new string[] { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };
            juudaiShusei.lstJudaiShusei = new List<JudaiShusei>
            {   //サブキーを登録
                new JudaiShusei("貫索星","木","陽","濁", new string[]{"甲","乙","丙","丁","戊","己","庚","辛","壬","癸" }),
                new JudaiShusei("石門星","木","陰","濁", new string[]{"乙","甲","丁","丙","己","戊","辛","庚","癸","壬" }),
                new JudaiShusei("鳳閣星","火","陽","純", new string[]{"丙","丁","戊","己","庚","辛","壬","癸","甲","乙" }),
                new JudaiShusei("調舒星","火","陰","濁", new string[]{"丁","丙","己","戊","辛","庚","癸","壬","乙","甲" }),
                new JudaiShusei("禄存星","土","陽","純", new string[]{"戊","己","庚","辛","壬","癸","甲","乙","丙","丁" }),
                new JudaiShusei("司禄星","土","陰","純", new string[]{"己","戊","辛","庚","癸","壬","乙","甲","丁","丙" }),
                new JudaiShusei("車騎星","金","陽","濁", new string[]{"庚","辛","壬","癸","甲","乙","丙","丁","戊","己" }),
                new JudaiShusei("牽牛星","金","陰","純", new string[]{"辛","庚","癸","壬","乙","甲","丁","丙","己","戊" }),
                new JudaiShusei("龍高星","水","陽","濁", new string[]{"壬","癸","甲","乙","丙","丁","戊","己","庚","辛" }),
                new JudaiShusei("玉堂星","水","陰","純", new string[]{"癸","壬","乙","甲","丁","丙","己","戊","辛","庚" }),
            };

            //--------------------------------
            //十二大従星
            //--------------------------------
            junidaiJusei = new JunidaiJuseiTbl();
            junidaiJusei.jukan1 = new string[] { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };
            junidaiJusei.lstJunidaiJusei = new List<JunidaiJusei>
            {    //サブキーを登録
                new JunidaiJusei("天報星","胎児",3 ,"弱"     ,new string[]{"酉","申","子","亥","子","亥","卯","寅","午","巳"}),
                new JunidaiJusei("天印星","赤子",6 ,"中(弱)" ,new string[]{"戌","未","丑","戌","丑","戌","辰","丑","未","辰"}),
                new JunidaiJusei("天貴星","児童",9 ,"中"     ,new string[]{"亥","午","寅","酉","寅","酉","巳","子","申","卯"}),
                new JunidaiJusei("天恍星","少年",7 ,"中"     ,new string[]{"子","巳","卯","申","卯","申","午","亥","酉","寅"}),
                new JunidaiJusei("天南星","青年",10,"強"     ,new string[]{"丑","辰","辰","未","辰","未","未","戌","戌","丑"}),
                new JunidaiJusei("天禄星","壮年",11,"強"     ,new string[]{"寅","卯","巳","午","巳","午","申","酉","亥","子"}),
                new JunidaiJusei("天将星","家長",12,"強"     ,new string[]{"卯","寅","午","巳","午","巳","酉","申","子","亥"}),
                new JunidaiJusei("天堂星","老人",8 ,"中"     ,new string[]{"辰","丑","未","辰","未","辰","戌","未","丑","戌"}),
                new JunidaiJusei("天胡星","病人",4 ,"弱"     ,new string[]{"巳","子","申","卯","申","卯","亥","午","寅","酉"}),
                new JunidaiJusei("天極星","死人",2 ,"弱"     ,new string[]{"午","亥","酉","寅","酉","寅","子","巳","卯","申"}),
                new JunidaiJusei("天庫星","入墓",5 ,"中(弱)" ,new string[]{"未","戌","戌","丑","戌","丑","丑","辰","辰","未"}),
                new JunidaiJusei("天馳星","彼世",1 ,"弱"     ,new string[]{"申","酉","亥","子","亥","子","寅","卯","巳","午"}),
            };

            //--------------------------------
            //干合
            //--------------------------------
            kangouTbl.lstKangou = new List<Kangou>
            {
                new Kangou("甲","己","土性","戊","己","甲己火土"),
                new Kangou("乙","庚","金性","辛","庚","乙庚化金"),
                new Kangou("丙","辛","水性","壬","癸","丙辛化水"),
                new Kangou("壬","丁","木性","甲","乙","壬丁化木"),
                new Kangou("戊","癸","火性","丙","丁","戊癸化火"),

            };
            //--------------------------------
            //七殺
            //--------------------------------
            nanasatsuTbl.lstNanasatsu = new List<Nanasatsu>
            {
                new Nanasatsu("甲","戊"),
                new Nanasatsu("乙","己"),
                new Nanasatsu("丙","庚"),
                new Nanasatsu("丁","辛"),
                new Nanasatsu("戊","壬"),
                new Nanasatsu("己","癸"),
                new Nanasatsu("庚","甲"),
                new Nanasatsu("辛","乙"),
                new Nanasatsu("壬","丙"),
                new Nanasatsu("癸","丁"),

            };


            //--------------------------------
            //合法・散法 テーブル
            //--------------------------------
            gouhouSanpouTbl = new GouhouSanpouTbl();
            gouhouSanpouTbl.jyunisi = new string[] { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };
            gouhouSanpouTbl.dicGouhouSanpou = new Dictionary<string, string[]>
            {
                { "子", new string[]{""      ,"支合"       ,""           ,"旺気刑","半会",""              ,"冲動","害"         ,"半会"          ,"破"  , ""         ,""         }},
                { "丑", new string[]{"支合"  ,""           ,""           ,""      ,"破"  ,"半会"          ,"害"  ,"冲動,庫気刑",""              ,"半会", "庫気刑"   ,""         }},
                { "寅", new string[]{""      ,""           ,""           ,""      ,""    ,"生貴刑,害"     ,"半会", ""          ,"冲動,生貴刑"   ,""    , "半会"     ,"支合,破"  }},
                { "卯", new string[]{"旺気刑",""           ,""           ,""      ,"害"  ,""              ,"破"  ,"半会"       ,""              ,"冲動", "支合"     ,"半会"     }},
                { "辰", new string[]{"半会"  ,"破"         ,""           ,"害"    ,"自刑",""              ,""    ,""           ,"半会,破,生貴刑","支合", " 冲動"    ,""         }},
                { "巳", new string[]{""      ,"半会"       ,"生貴刑,害"  ,""      ,""    ,""              ,""    ,""           ,""              ,"半会", ""         ,"冲動"     }},
                { "午", new string[]{"冲動"  ,"害"         ,"半会"       ,"破"    ,""    ,""              ,"自刑","支合"       ,""              ,""    , "半会"     ,""         }},
                { "未", new string[]{"害"    ,"冲動,庫気刑",""           ,"半会"  ,""    ,""              ,"支合",""           ,""              ,""    , "庫気刑,破"," 半会"    }},
                { "申", new string[]{"半会"  ,""           ,"冲動,生貴刑",""      ,"半会","支合,破,生貴刑",""    ,""           ,""              ,""    , ""         ,"害"       }},
                { "酉", new string[]{"破"    ,"半会"       ,""           ,"冲動"  ,"支合","半会"          ,""    ,""           ,""              ,"自刑","害"        ,""         }},
                { "戌", new string[]{""      ,"庫気刑"     ,"半会"       ,"支合"  ,"冲動",""              ,"半会","庫気刑,破"  ,""              ,"害"  , ""         ,""         }},
                { "亥", new string[]{""      ,""           ,"支合,破"    ,"半会"  ,""    ,"冲動"          ,""    ,"半会"       ,"害"            ,""    , ""         ,"自刑"     }},

            };
            //--------------------------------
            //三合会局 テーブル
            //--------------------------------
            sangouKaikyokuTbl.lstSangouKaikyoku = new List<SangouKaikyoku>()
            {
                new SangouKaikyoku(new string[]{"申","子","辰"},"水性"),
                new SangouKaikyoku(new string[]{"亥","卯","未"},"木性"),
                new SangouKaikyoku(new string[]{"寅","午","戌"},"火性"),
                new SangouKaikyoku(new string[]{"巳","酉","丑"},"金性"),
            };
            //--------------------------------
            //方三位 テーブル
            //--------------------------------
            housanniTbl.lstHousanni = new List<HouSanni>()
            {
                new HouSanni(new string[]{"亥","子","丑"},"水性"),
                new HouSanni(new string[]{"寅","卯","辰"},"木性"),
                new HouSanni(new string[]{"巳","午","未"},"火性"),
                new HouSanni(new string[]{"申","酉","戌"},"金性"),

            };
            //--------------------------------
            //支合 テーブル
            //--------------------------------
            sigouTbl.lstSigou = new List<Sigou>()
            {
                new Sigou(new string[]{"子","丑"},"水性"),
                new Sigou(new string[]{"亥","寅"},"木性"),
                new Sigou(new string[]{"戌","卯"},"木性 or 土性"),//この五行はいつか対応が必要
                new Sigou(new string[]{"酉","辰"},"金性 or 土性"),//この五行はいつか対応が必要
                new Sigou(new string[]{"申","巳"},"金性"),
                new Sigou(new string[]{"未","午"},"火性"),

            };

        }
    }




    class DataAccessor
    {
        TableMng tblMng;

        public DataAccessor( TableMng mng)
        {
            tblMng = mng;
        }


    }

}
