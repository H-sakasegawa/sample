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
        /// 十干
        /// </summary>
        public List<Jyunisi> lstJyunisi = null;
        /// <summary>
        /// 十二支
        /// </summary>
        public List<Jyukan> lstJyukan = null;

        /// <summary>
        /// 干支
        /// </summary>
        public Dictionary<int, Kansi> dicKansi = null;

        /// <summary>
        /// 二十八元表
        /// </summary>
        public Dictionary<string, NijuhachiGenso> lstNijuhachiGenso = null;
        /// <summary>
        /// 十大主星
        /// </summary>
        public JudaiShusei juudaiShusei = null;
        /// <summary>
        /// 十二大従星
        /// </summary>
        public JunidaiJusei junidaiJusei = null;

        public TableMng()
        {
            //--------------------------------
            //十干
            //--------------------------------
            lstJyukan = new List<Jyukan>
            {
                new Jyukan("甲","木","陽(+)"),
                new Jyukan("乙","木","陰(-)"),
                new Jyukan("丙","火","陽(+)"),
                new Jyukan("丁","火","陰(-)"),
                new Jyukan("戊","土","陽(+)"),
                new Jyukan("己","土","陰(-)"),
                new Jyukan("庚","金","陽(+)"),
                new Jyukan("辛","金","陰(-)"),
                new Jyukan("壬","水","陽(+)"),
                new Jyukan("癸","水","陰(-)"),
            };
            //--------------------------------
            //十二支
            //--------------------------------
            lstJyunisi = new List<Jyunisi>
            {
                new Jyunisi("子","水","陽(+)"),
                new Jyunisi("丑","土","陰(-)"),
                new Jyunisi("寅","木","陽(+)"),
                new Jyunisi("卯","木","陰(-)"),
                new Jyunisi("辰","土","陽(+)"),
                new Jyunisi("巳","火","陰(-)"),
                new Jyunisi("午","火","陽(+)"),
                new Jyunisi("未","土","陰(-)"),
                new Jyunisi("申","金","陽(+)"),
                new Jyunisi("酉","金","陰(-)"),
                new Jyunisi("戌","土","陽(+)"),
                new Jyunisi("亥","水","陰(-)"),
            };

            //--------------------------------
            //干支            
            //--------------------------------
            string[] tenchusatu = { "戌,亥", "申,酉", "午,未", "辰,巳", "寅,卯", "子,丑" };//天中殺
            dicKansi = new Dictionary<int, Kansi>();
            for (int i=0; i<60; i++)
            {
                string kan = lstJyukan[i % lstJyukan.Count].name;
                string si= lstJyunisi[i % lstJyunisi.Count].name;
                string techu = tenchusatu[i / 10];

                int No = i + 1;
                dicKansi.Add(No, new Kansi(No, kan, si, techu));
 
            }

            //--------------------------------
            //二十八元表
            //--------------------------------
            lstNijuhachiGenso = new Dictionary<string, NijuhachiGenso>
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
            juudaiShusei = new JudaiShusei();
            juudaiShusei.jukan1 = new string[] { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };
            juudaiShusei.lstJudaiShusei = new List<JudaiShuseiItem>
            {   //サブキーを登録
                new JudaiShuseiItem("貫索星","木","陽","濁", new string[]{"甲","乙","丙","丁","戊","己","庚","辛","壬","癸" }),
                new JudaiShuseiItem("石門星","木","陰","濁", new string[]{"乙","甲","丁","丙","己","戊","辛","庚","癸","壬" }),
                new JudaiShuseiItem("鳳閣星","火","陽","純", new string[]{"丙","丁","戊","己","庚","辛","壬","癸","甲","乙" }),
                new JudaiShuseiItem("調舒星","火","陰","濁", new string[]{"丁","戊","己","庚","辛","壬","癸","甲","乙","丙" }),
                new JudaiShuseiItem("禄存星","土","陽","純", new string[]{"戊","己","庚","辛","壬","癸","甲","乙","丙","丁" }),
                new JudaiShuseiItem("司禄星","土","陰","純", new string[]{"己","戊","辛","庚","癸","壬","乙","甲","丁","丙" }),
                new JudaiShuseiItem("車騎星","金","陽","濁", new string[]{"庚","辛","壬","癸","甲","乙","丙","丁","戊","己" }),
                new JudaiShuseiItem("牽牛星","金","陰","純", new string[]{"辛","庚","癸","壬","乙","甲","丁","丙","己","戊" }),
                new JudaiShuseiItem("龍高星","水","陽","濁", new string[]{"壬","癸","甲","乙","丙","丁","戊","己","庚","辛" }),
                new JudaiShuseiItem("玉堂星","水","陰","純", new string[]{"癸","壬","乙","甲","丁","丙","己","戊","辛","庚" }),

            };

            //--------------------------------
            //十二大従星
            //--------------------------------
            junidaiJusei = new JunidaiJusei();
            junidaiJusei.jukan1 = new string[] { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };
            junidaiJusei.lstJunidaiJusei = new List<JunidaiJuseiItem>
            {    //サブキーを登録
                new JunidaiJuseiItem("天報星","胎児",3 ,"弱"     ,new string[]{"酉","申","子","亥","子","亥","卯","寅","午","巳"}),
                new JunidaiJuseiItem("天印星","赤子",6 ,"中(弱)" ,new string[]{"戌","未","丑","戌","丑","戌","辰","丑","未","辰"}),
                new JunidaiJuseiItem("天貴星","児童",9 ,"中"     ,new string[]{"亥","午","寅","酉","寅","酉","巳","子","申","卯"}),
                new JunidaiJuseiItem("天恍星","少年",7 ,"中"     ,new string[]{"子","巳","卯","申","卯","申","午","亥","酉","寅"}),
                new JunidaiJuseiItem("天南星","青年",10,"強"     ,new string[]{"丑","辰","辰","未","辰","未","未","戌","戌","丑"}),
                new JunidaiJuseiItem("天禄星","壮年",11,"強"     ,new string[]{"寅","卯","巳","午","巳","午","申","酉","亥","子"}),
                new JunidaiJuseiItem("天将星","家長",12,"強"     ,new string[]{"卯","寅","午","巳","午","巳","酉","申","子","亥"}),
                new JunidaiJuseiItem("天堂星","老人",8 ,"中"     ,new string[]{"辰","丑","未","辰","未","辰","戌","未","丑","戌"}),
                new JunidaiJuseiItem("天胡星","病人",4 ,"弱"     ,new string[]{"巳","子","申","卯","申","卯","亥","午","寅","酉"}),
                new JunidaiJuseiItem("天極星","死人",2 ,"弱"     ,new string[]{"午","亥","酉","寅","酉","寅","子","巳","卯","申"}),
                new JunidaiJuseiItem("天庫星","入墓",5 ,"中(弱)" ,new string[]{"未","戌","戌","丑","戌","丑","丑","辰","辰","未"}),
                new JunidaiJuseiItem("天馳星","彼世",1 ,"弱"     ,new string[]{"申","酉","亥","子","亥","子","寅","卯","巳","午"}),
            };
         }

    }
}
