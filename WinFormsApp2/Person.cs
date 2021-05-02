using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    /// 性別
    /// </summary>
    enum Gender
    {
        NAN = 0,
        WOMAN
    }

    /// <summary>
    /// ユーザ情報
    /// </summary>
    class Person
    {
        public string name { get; }
        public Birthday birthday { get; }
        public Gender gender { get; }
        public string group { get; }

        public int dayNumFromSetuiribi { get; set; }

        public int nikkansiNo { get; set; }
        public int gekkansiNo { get; set; }
        public int nenkansiNo { get; set; }
        //干支
        public Kansi nikkansi { get; set; }
        public Kansi gekkansi { get; set; }
        public Kansi nenkansi { get; set; }
        //二十八元素
        public NijuhachiGenso nijuhachiGensoNikkansi { get; set; }
        public NijuhachiGenso nijuhachiGensoGekkansi { get; set; }
        public NijuhachiGenso nijuhachiGensoNenkansi { get; set; }
        //十大主星
        public JudaiShusei judaiShuseiA { get; set; }
        public JudaiShusei judaiShuseiB { get; set; }
        public JudaiShusei judaiShuseiC { get; set; }
        public JudaiShusei judaiShuseiD { get; set; }
        public JudaiShusei judaiShuseiE { get; set; }
        //十二大主星
        public JunidaiJusei junidaiJuseiA { get; set; }
        public JunidaiJusei junidaiJuseiB { get; set; }
        public JunidaiJusei junidaiJuseiC { get; set; }

        private TableMng tblMng;
        private SetuiribiTable tblSetuiribi;

        public Person(string _name, int year, int month, int day, Gender _gender)
        {
            name = _name;


            birthday = new Birthday(year, month, day);
            gender = _gender;
            group = "";


        }
        public Person(string _name, Birthday _birthday, Gender _gender, string _group)
        {
            name = _name;
            birthday = _birthday;
            gender = _gender;
            group = _group;


        }

        public override string ToString()
        {
            return name;
        }

        public int Init(TableMng _tblMng, SetuiribiTable _tblSetuiribi)
        {
            tblMng = _tblMng;
            tblSetuiribi = _tblSetuiribi;

            //誕生日に該当する節入り日から誕生日までの経過日数
            dayNumFromSetuiribi = tblSetuiribi.CalcDayCountFromSetuiribi(birthday.year, birthday.month, birthday.day);


            //日干支番号、月干支番号、年干支番号
            nikkansiNo = tblSetuiribi.GetNikkansiNo(birthday.year, birthday.month, birthday.day);
            gekkansiNo = tblSetuiribi.GetGekkansiNo(birthday.year, birthday.month, birthday.day);
            nenkansiNo = tblSetuiribi.GetNenKansiNo(birthday.year, birthday.month, birthday.day);

            //干支
            nikkansi = tblMng.kansiTbl.GetKansi(nikkansiNo);
            gekkansi = tblMng.kansiTbl.GetKansi(gekkansiNo);
            nenkansi = tblMng.kansiTbl.GetKansi(nenkansiNo);

            //二十八元表
            nijuhachiGensoNikkansi = tblMng.dicNijuhachiGenso[nikkansi.si];
            nijuhachiGensoGekkansi = tblMng.dicNijuhachiGenso[gekkansi.si];
            nijuhachiGensoNenkansi = tblMng.dicNijuhachiGenso[nenkansi.si];


            NijuhachiGenso gensoNikkansi = nijuhachiGensoNikkansi;
            NijuhachiGenso gensoGekkansi = nijuhachiGensoGekkansi;
            NijuhachiGenso gensoNenkansi = nijuhachiGensoNenkansi;

            var idxNikkansiGensoType = (int)gensoNikkansi.GetTargetGensoType(dayNumFromSetuiribi);
            var idxGekkansiGensoType = (int)gensoGekkansi.GetTargetGensoType(dayNumFromSetuiribi);
            var idxNenkaisiGensoType = (int)gensoNenkansi.GetTargetGensoType(dayNumFromSetuiribi);


            //------------------
            //十大主星
            //------------------
            //干1 → 蔵x1
            judaiShuseiA = tblMng.juudaiShusei.GetJudaiShusei(nikkansi.kan, gensoNikkansi.genso[idxNikkansiGensoType].name);
            //干1 → 蔵x2
            judaiShuseiB = tblMng.juudaiShusei.GetJudaiShusei(nikkansi.kan, gensoGekkansi.genso[idxGekkansiGensoType].name);
            //干1 → 蔵x3
            judaiShuseiC = tblMng.juudaiShusei.GetJudaiShusei(nikkansi.kan, gensoNenkansi.genso[idxNenkaisiGensoType].name);
            //干1 → 干3
            judaiShuseiD = tblMng.juudaiShusei.GetJudaiShusei(nikkansi.kan, nenkansi.kan);
            //干1 → 干2
            judaiShuseiE = tblMng.juudaiShusei.GetJudaiShusei(nikkansi.kan, gekkansi.kan);


            //------------------
            //十二大主星
            //------------------
            //干1 → 支3
            junidaiJuseiA = tblMng.junidaiJusei.GetJunidaiJusei(nikkansi.kan, nenkansi.si);
            //干1 → 支2
            junidaiJuseiB = tblMng.junidaiJusei.GetJunidaiJusei(nikkansi.kan, gekkansi.si);
            //干1 → 支1
            junidaiJuseiC = tblMng.junidaiJusei.GetJunidaiJusei(nikkansi.kan, nikkansi.si);



            return 0;
        }

        //入力された生年月日から、紐付く月最終日までの日数
        public int CalcDayCountBirthdayToLastMonthDay()
        {
            return tblSetuiribi.CalcDayCountBirthdayToLastMonthDay(birthday.year, birthday.month, birthday.day);
        }

        //入力された生年月日に紐付く月の節入り日からの経過日数
        public int CalcDayCountFromSetuiribi()
        {
            return tblSetuiribi.CalcDayCountFromSetuiribi(birthday.year, birthday.month, birthday.day);
        }

        //干支番号に該当する干支を取得
        public Kansi GetKansi( int kansiNo )
        {
            return tblMng.kansiTbl.GetKansi(kansiNo);
        }

        //十大主星 取得
        public JudaiShusei GetJudaiShusei(string kan, string si)
        {
            return tblMng.juudaiShusei.GetJudaiShusei(kan, si);
        }
        //十二大従星 取得
        public JunidaiJusei GetJunidaiShusei(string kan, string si)
        {
            return tblMng.junidaiJusei.GetJunidaiJusei(kan, si);
        }

        //合法・散法
        public string[] GetGouhouSanpou(Kansi nenunKansi, Kansi kansi)
        {
            return GetGouhouSanpou(nenunKansi.si, kansi.si);
        }
        public string[] GetGouhouSanpou(string nenunKansiSi, string kansiSi)
        {
            return tblMng.gouhouSanpouTbl.GetGouhouSanpou(nenunKansiSi, kansiSi);
        }
        public string GetGouhouSanpouString(Kansi nenunKansi, Kansi kansi)
        {
            return GetGouhouSanpouString(nenunKansi.si, kansi.si);
        }
        public string GetGouhouSanpouString(string nenunKansiSi, string kansiSi)
        {
            return tblMng.gouhouSanpouTbl.GetGouhouSanpouString(nenunKansiSi, kansiSi);
        }
        //納音、準納音
        public string GetNentin(Kansi nenunKansi, Kansi kansi)
        {
            int nenunKansiNo = tblMng.kansiTbl.GetKansiNo(nenunKansi);
            int kansiNo = tblMng.kansiTbl.GetKansiNo(kansi);

            int dif = 0;
            if (nenunKansiNo <= kansiNo) dif = kansiNo - nenunKansiNo;
            else dif = nenunKansiNo - kansiNo;

            if (dif == 30) return "納音";
            if (dif == 29 || dif == 31)
            {
                //nenunKansiの干とkansiの干が同じ五行の陰陽の関係か？
                var nenunKansiJyukan = tblMng.jyukanTbl[nenunKansi.kan];
                var kansiJyukan = tblMng.jyukanTbl[kansi.kan];

                if (nenunKansiJyukan.gogyou == kansiJyukan.gogyou) return "準納音";

            }

            return "";
        }
        //律音、準律音
        public string GetNittin(Kansi nenunKansi, Kansi kansi)
        {
            int nenunKansiNo = tblMng.kansiTbl.GetKansiNo(nenunKansi);
            int kansiNo = tblMng.kansiTbl.GetKansiNo(kansi);

            if (nenunKansiNo == kansiNo) return "律音";
            else
            {
                int dif = Math.Abs(kansiNo - nenunKansiNo);
                if ( kansiNo==1 && nenunKansiNo == 60 ||
                    kansiNo == 60 && nenunKansiNo == 1 ||
                    dif == 1)
                {
                    //nenunKansiの干とkansiの干が同じ五行の陰陽の関係か？
                    var nenunKansiJyukan = tblMng.jyukanTbl[nenunKansi.kan];
                    var kansiJyukan = tblMng.jyukanTbl[kansi.kan];

                    if (nenunKansiJyukan.gogyou == kansiJyukan.gogyou) return "準律音";
                }

            }

            return "";
        }
        //七殺
        public Nanasatsu GetNanasatu(Kansi taiunKansi, Kansi kansi)
        {
            return GetNanasatu(taiunKansi.kan, kansi.kan);
        }
        public Nanasatsu GetNanasatu(string taiunKan, string kan)
        {
            return tblMng.nanasatsuTbl.GetNanasatsu(taiunKan, kan);
        }
        public bool IsNanasatu(Kansi taiunKansi, Kansi kansi)
        {
            return IsNanasatu(taiunKansi.kan, kansi.kan);
        }
        public bool IsNanasatu(string taiunKan, string kan)
        {
            return GetNanasatu(taiunKan, kan)!=null?true:false;
        }
        //天殺
        public string GetTensatuString(Kansi taiunKansi, Kansi kansi)
        {
            return IsNanasatu(taiunKansi, kansi) ? "天殺" : "";
        }
        //地冲
        public string GetTichuString(Kansi taiunKansi, Kansi kansi)
        {
            var values = GetGouhouSanpou(taiunKansi, kansi);
            if (values != null)
            {
                foreach (var item in values)
                {
                    if (item.IndexOf("冲動") >= 0) return "地冲";
                }
            }
            return "";
        }


    }
    /// <summary>
    /// 誕生日情報クラス
    /// </summary>
    class Birthday
    {
        public Birthday(string _birthday)
        {
            birthday = _birthday;
            string[] items = birthday.Split("/");
            year = int.Parse(items[0]);
            month = int.Parse(items[1]);
            day = int.Parse(items[2]);

        }
        public Birthday(int _year, int _month, int _day)
        {
            birthday = string.Format("{0}/{1}/{2}", _year, _month, _day);
            year = _year;
            month = _month;
            day = _day;

        }

        public string birthday;
        public int year;
        public int month;
        public int day;
    }

    /// <summary>
    /// ユーザ情報リスト管理クラス
    /// </summary>
    class Persons
    {
        /// <summary>
        /// ユーザ情報登録ファイルの項目順定義
        /// </summary>
        enum PersonListCol
        {
           COL_NAME=0,  //氏名
           COL_BIRTHDAY,//誕生日
           COL_GENDER,  //性別
           COL_GROUP    //グループ
        };

        private Dictionary<string, Person> dicPersons = null;

        public Persons( )
        {
            dicPersons = new Dictionary<string, Person>();
        }


        public Person this[string name]
        {
            get
            {
                if (!dicPersons.ContainsKey(name)) return null;
                return dicPersons[name];
            }
        }
        public Person this[int index]
        {
            get
            {
                if (dicPersons.Count<index) return null;
                return dicPersons.Values.ToList()[index];
            }
        }

        public int Count
        {
            get { return dicPersons.Count; }
        }

        /// <summary>
        /// エクセルファイルからのユーザ情報読み込み
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public int ReadPersonList(string filePath)
        {
            var version = "xls";
            var workbook = ExcelReader.GetWorkbook(filePath, version);
            if (workbook == null)
            {
                return -1;
            }

            var sheet = workbook.GetSheetAt(0);

            int iRow =1;
            while (true)
            {
                //氏名
                string name = ExcelReader.CellValue(sheet, iRow, (int)PersonListCol.COL_NAME);
                if (name == "") break;

                //生年月日
                string sBirthday = ExcelReader.CellValue(sheet, iRow, (int)PersonListCol.COL_BIRTHDAY);
                //時間は不要なので除外
                sBirthday = sBirthday.Substring(0, sBirthday.IndexOf(" "));
                Birthday birthday = new Birthday(sBirthday);

                //性別
                string sGender = ExcelReader.CellValue(sheet, iRow, (int)PersonListCol.COL_GENDER);
                Gender gender = (sGender == "男" ? Gender.NAN : Gender.WOMAN);

                //グループ
                string group = ExcelReader.CellValue(sheet, iRow, (int)PersonListCol.COL_GROUP);

                dicPersons.Add(name, new Person(name, birthday, gender, group));

                iRow++;

            }

            return 0;
        }
    }

 }
