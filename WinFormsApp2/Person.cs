using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    /// 性別
    /// </summary>
    public enum Gender
    {
        MAN = 0,
        WOMAN
    }

    /// <summary>
    /// 合法・三法の検索結果情報
    /// </summary>
    public class GouhouSannpouResult
    {

        public bool bEnable;        //true...有効  false...無効
        public string orgName;      //テーブルに登録されている名称
        public string displayName;  //条件によって変換された名称

        public GouhouSannpouResult(string _orgName)
        {
            bEnable = true;
            orgName = _orgName;
            displayName = orgName;
        }
    }

    //===========================================================
    // ユーザ情報　管理クラス
    //===========================================================
    /// <summary>
    /// ユーザ情報リスト管理クラス
    /// </summary>
    public class Persons
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

        // ユーザ基本データ <人名,　ユーザ情報>
        private Dictionary<string, Person> dicPersons = null;
        // グループデータ <グループ名,　グループ情報>
        private Dictionary<string, Group> dicGroup = null;
  
        public Persons( )
        {
            dicPersons = new Dictionary<string, Person>();
            dicGroup = new Dictionary<string, Group>();
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
        public List<Person> GetPersons()
        {
            return dicPersons.Values.ToList();
        }

        /// <summary>
        /// グループ一覧取得
        /// </summary>
        /// <returns></returns>
        public List<Group> GetGroups()
        {
            return dicGroup.Values.ToList();
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
                Gender gender = (sGender == "男" ? Gender.MAN : Gender.WOMAN);

                //グループ
                string group = ExcelReader.CellValue(sheet, iRow, (int)PersonListCol.COL_GROUP);

                var person = new Person(name, birthday, gender, group);
                dicPersons.Add(name, person);

                //グループディクショナリ
                if( !dicGroup.ContainsKey(group))
                {
                    dicGroup.Add(group, new Group(group));
                }
                dicGroup[group].AddMember(person);


                iRow++;

            }

            return 0;
        }
    }

    /// <summary>
    /// ユーザ情報
    /// </summary>
    public class Person
    {
        public string name { get; }
        public Birthday birthday { get; }
        public Gender gender { get; }
        public string group { get; }

        public Career career { get; private set; }


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
            ReadCareer();
        }
        public Person(string _name, Birthday _birthday, Gender _gender, string _group)
        {
            name = _name;
            birthday = _birthday;
            gender = _gender;
            group = _group;

            ReadCareer();

        }

        private int ReadCareer()
        {

            career = new Career( this );
            return career.ReaCareerFile();
        }

        public override string ToString()
        {
            return name;
        }

        public Person Clone()
        {
            // Object型で返ってくるのでキャストが必要
            return (Person)MemberwiseClone();
        }


        public int Init(TableMng _tblMng, SetuiribiTable _tblSetuiribi)
        {
            tblMng = _tblMng;
            tblSetuiribi = _tblSetuiribi;

            //誕生日に該当する節入り日から誕生日までの経過日数(節入り日はカウントされません）
            dayNumFromSetuiribi = tblSetuiribi.CalcDayCountFromSetuiribi(birthday.year, birthday.month, birthday.day);


            //日干支番号、月干支番号、年干支番号
            nikkansiNo = tblSetuiribi.GetNikkansiNo(birthday.year, birthday.month, birthday.day);
            gekkansiNo = tblSetuiribi.GetGekkansiNo(birthday.year, birthday.month, birthday.day);
            nenkansiNo = tblSetuiribi.GetNenKansiNo(birthday.year, birthday.month, birthday.day);

            //干支
            nikkansi = tblMng.kansiTbl[nikkansiNo];
            gekkansi = tblMng.kansiTbl[gekkansiNo];
            nenkansi = tblMng.kansiTbl[nenkansiNo];

            //二十八元表
            nijuhachiGensoNikkansi = tblMng.nijuhachiGensoTbl[nikkansi.si];
            nijuhachiGensoGekkansi = tblMng.nijuhachiGensoTbl[gekkansi.si];
            nijuhachiGensoNenkansi = tblMng.nijuhachiGensoTbl[nenkansi.si];


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

        /// <summary>
        /// 大運 順行、逆行判定
        /// </summary>
        /// <param name="NenkansiNo"></param>
        /// <returns></returns>
        public int Direction()
        {
            var Nenkansi = nenkansi;

            //性別
            if (gender == Gender.MAN)
            {   //男性
                if (tblMng.jyukanTbl[Nenkansi.kan].inyou == "+") return 1;
                else return -1;
            }
            else
            {   //女性
                if (tblMng.jyukanTbl[Nenkansi.kan].inyou == "+") return -1;
                else return 1;
            }
        }

        public class TaiunKansiItem
        {
            public TaiunKansiItem(int _year, int _startYear, int No)
            {
                year = _year;
                startYear = _startYear;
                kansiNo = No;
            }

            public int year;
            public int startYear;
            public int kansiNo;
        }
        /// <summary>
        /// 大運表の１０年周期の年と干支の組み合わせリスト取得
        /// </summary>
        /// <returns></returns>
        public List<TaiunKansiItem> GetTaiunKansiList()
        {
            List<TaiunKansiItem> lstResult = new List<TaiunKansiItem>();

            //初旬干支番号
            int kansiNo = gekkansiNo;

            //順行、逆行
            int dirc = Direction();

            //才運
            int dayCnt;
            if (dirc == 1) //順行
            {
                dayCnt = CalcDayCountBirthdayToLastMonthDay();
            }
            else
            {
                //CalcDayCountFromSetuiribi()は節入り日を含めないので、＋１する
                dayCnt = CalcDayCountFromSetuiribi() + 1;
            }
            int countStartNen = (int)Math.Ceiling(dayCnt / 3.0);
            if (countStartNen > 10) countStartNen = 10;

            lstResult.Add(new TaiunKansiItem(birthday.year, 0, kansiNo));

            //1旬～10旬まで
            for (int i = 0; i < 10; i++)
            {
                kansiNo += dirc;
                if (kansiNo < 1) kansiNo = 60;
                if (kansiNo > 60) kansiNo = 1;

                lstResult.Add(new TaiunKansiItem(birthday.year + countStartNen,  countStartNen, kansiNo));
                countStartNen += 10;
            }

            return lstResult;
        }

        /// <summary>
        /// 年に関する年干支を取得
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public Kansi GetNenkansi(int year)
        {
            int kansiNo = GetNenkansiNo(year);
            if (kansiNo < 0) return null;

            return GetKansi(kansiNo);
        }

        /// <summary>
        /// 年に関する年干支番号を取得
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public int GetNenkansiNo(int year)
        {
            if (year < birthday.year) return -1;


            //0才 干支番号
            int targetNenkansiNo = nenkansiNo;

            //指定された年の干支番号
            targetNenkansiNo += year - birthday.year;
            targetNenkansiNo = targetNenkansiNo % 60;
            if (targetNenkansiNo == 0) targetNenkansiNo = 60;

            return targetNenkansiNo;
        }

        /// <summary>
        /// 入力された生年月日から、紐付く月最終日までの日数
        /// </summary>
        /// <returns></returns>
        public int CalcDayCountBirthdayToLastMonthDay()
        {
            return tblSetuiribi.CalcDayCountBirthdayToLastMonthDay(birthday.year, birthday.month, birthday.day);
        }

        /// <summary>
        /// 入力された生年月日に紐付く月の節入り日からの経過日数
        /// </summary>
        /// <returns></returns>
        public int CalcDayCountFromSetuiribi()
        {
            return tblSetuiribi.CalcDayCountFromSetuiribi(birthday.year, birthday.month, birthday.day);
        }

        /// <summary>
        /// 干支番号に該当する干支を取得
        /// </summary>
        /// <param name="kansiNo"></param>
        /// <returns></returns>
        public Kansi GetKansi( int kansiNo )
        {
            return tblMng.kansiTbl.GetKansi(kansiNo);
        }

        /// <summary>
        /// 日、月、年の干支の支に引数で指定された文字が含まれるかを判定
        /// </summary>
        /// <param name="si"></param>
        /// <returns></returns>
        public bool IsExistStrInKansiSi( string[] si)
        {
            foreach (var s in si)
            {
                if (nikkansi.si == s) return true;
                if (gekkansi.si == s) return true;
                if (nenkansi.si == s) return true;
            }
            return false;

        }

        /// <summary>
        /// 十大主星 取得
        /// </summary>
        /// <param name="kan"></param>
        /// <param name="si"></param>
        /// <returns></returns>
        public JudaiShusei GetJudaiShusei(string kan, string si)
        {
            return tblMng.juudaiShusei.GetJudaiShusei(kan, si);
        }

        // 
        /// <summary>
        /// 十大主星 陰陽関係判定（干）
        /// 日→月
        /// </summary>
        /// <returns></returns>
        public bool IsInyouNitiGetsuKan()
        {
            return tblMng.jyukanTbl.IsInyou(nikkansi.kan, gekkansi.kan);
        }
        /// <summary>
        /// 十大主星 陰陽関係判定（干）
        /// 月→年
        /// </summary>
        /// <returns></returns>
        public bool IsInyouGetsuNenKan()
        {
            return tblMng.jyukanTbl.IsInyou(gekkansi.kan, nenkansi.kan);
        }
        /// <summary>
        /// 十大主星 陰陽関係判定（干）
        /// 日→年
        /// </summary>
        /// <returns></returns>
        public bool IsInyouNitiNenKan()
        {
            return tblMng.jyukanTbl.IsInyou(nikkansi.kan, nenkansi.kan);
        }
        /// <summary>
        /// 大運→（日月年）,年運→（日月年）,他日→月などでも可
        /// </summary>
        /// <param name="kansi1"></param>
        /// <param name="kansi2"></param>
        /// <returns></returns>
        public bool IsInyou(Kansi kansi1, Kansi kansi2)
        {
            return tblMng.jyukanTbl.IsInyou(kansi1.kan, kansi2.kan);
        }

        ////十大主星 陰陽関係判定（支）
        //public bool IsInyouNitiGetsuSi()
        //{
        //    return tblMng.jyukanTbl.IsInyou(nikkansi.kan, gekkansi.kan);
        //}
        //public bool IsInyouGetsuNenSi()
        //{
        //    return tblMng.jyukanTbl.IsInyou(gekkansi.kan, nenkansi.kan);
        //}
        //public bool IsInyouNitiNenSi()
        //{
        //    return tblMng.jyukanTbl.IsInyou(nikkansi.kan, nenkansi.kan);
        //}


        /// <summary>
        /// 十二大従星 取得
        /// </summary>
        /// <param name="kan"></param>
        /// <param name="si"></param>
        /// <returns></returns>
        public JunidaiJusei GetJunidaiShusei(string kan, string si)
        {
            return tblMng.junidaiJusei.GetJunidaiJusei(kan, si);
        }


 
        //合法・散法
        /// <summary>
        /// 合法・散法 文字の配列を取得します。
        /// ・天殺地冲または、納音がある場合は、"冲動"は除外されます。
        /// ・半会の場合、２つの干支番号に20の差異があれば"大半会"に置き換えます。
        /// </summary>
        /// <param name="nenunTaiunKansi">年運、大運 干支</param>
        /// <param name="kansi">宿命干支（日、月、年）</param>
        /// <param name="bExistTensatuTichu">true...天殺地冲あり</param>
        /// <param name="bExistNentin">true...納音あり</param>
        /// <returns></returns>
        public GouhouSannpouResult[] GetGouhouSanpou(Kansi nenunTaiunKansi, Kansi kansi, bool bExistTensatuTichu, bool bExistNentin)
        {
            List<GouhouSannpouResult> lstResult = new List<GouhouSannpouResult>();

            string[] items = tblMng.gouhouSanpouTbl.GetGouhouSanpouEx(nenunTaiunKansi.si, kansi.si, bExistTensatuTichu, bExistNentin);
            if (items != null)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    GouhouSannpouResult result = new GouhouSannpouResult(items[i]);
                    if (items[i] == "半会")
                    {
                        //大半会判定
                        int no1 = tblMng.kansiTbl.GetKansiNo(nenunTaiunKansi);
                        int no2 = tblMng.kansiTbl.GetKansiNo(kansi);

                        //no1に+20、または-20した地点のどちらかがno2と同じなら大半会
                        //1～60のリング状態をベースに前後方向の20差をチェックする必要がある。
                        //前方検索
                        int no = no1 + 20;
                        if (no > 60)
                        {
                            no = no % 60;
                            if (no == 0) no = 60;
                        }
                        if (no == no2)
                        {
                            result.displayName = "大半会";
                        }
                        else
                        {
                            //後方検索
                            no = no1 - 20;
                            if (no < 0)
                            {
                                no += 60;
                            }
                            if (no == no2)
                            {
                                result.displayName = "大半会";
                            }
                        }
                        //if (Math.Abs( no1 - no2)== 20)
                        //{
                        //    items[i] = "大半会";
                        //}
                        //else
                        //{
                        //    if( no1 < no2)
                        //    {
                        //        if( no2 + 20 == no1 + 60)
                        //        {
                        //            items[i] = "大半会";
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (no1 + 20 == no2 + 60)
                        //        {
                        //            items[i] = "大半会";
                        //        }
                        //    }
                        //}
                     }

                    lstResult.Add(result);
                }
            }
            return lstResult.ToArray();
        }
        /// <summary>
        /// 合法・散法 文字の配列を取得します。
        /// </summary>
        /// <param name="unKansi1">年運、大運干支</param>
        /// <param name="unKansi2">対象干支</param>
        /// <param name="taiunKansi">大運干支</param>
        /// <param name="nenunKansi">年運干支</param>
        /// <returns></returns>
        public GouhouSannpouResult[] GetGouhouSanpouEx(Kansi unKansi1, Kansi unKansi2, Kansi taiunKansi, Kansi nenunKansi)
        {
            List<GouhouSannpouResult> lstGouhouSanpouResult = new List<GouhouSannpouResult>();

            string nentin = GetNentin(unKansi1, unKansi2); //納音、準納音
            string rittin = GetNittin(unKansi1, unKansi2); //律音、準律音
            string tensatu = GetTensatuTichuString(unKansi1, unKansi2);//天殺地冲
            //string kangou = GetKangoStr(nenunTaiunKansi, kansi); //干合            

            bool bExistNentin = (nentin == "" ? false : true);
            bool bExistTensatuTichu = tensatu == "" ? false : true;

            string[] items = tblMng.gouhouSanpouTbl.GetGouhouSanpouEx(unKansi1.si, unKansi2.si, bExistTensatuTichu, bExistNentin);
            if (items != null)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    GouhouSannpouResult result = new GouhouSannpouResult(items[i]);
                    if (items[i] == "半会")
                    {
                        //大半会判定
                        int no1 = tblMng.kansiTbl.GetKansiNo(unKansi1);
                        int no2 = tblMng.kansiTbl.GetKansiNo(unKansi2);

                        //no1に+20、または-20した地点のどちらかがno2と同じなら大半会
                        //1～60のリング状態をベースに前後方向の20差をチェックする必要がある。
                        //前方検索
                        int no = no1 + 20;
                        if (no > 60)
                        {
                            no = no % 60;
                            if (no == 0) no = 60;
                        }
                        if (no == no2)
                        {
                            result.displayName = "大半会";
                        }
                        else
                        {
                            //後方検索
                            no = no1 - 20;
                            if (no < 0)
                            {
                                no += 60;
                            }
                            if (no == no2)
                            {
                                result.displayName = "大半会";
                            }
                        }
                        //if (Math.Abs( no1 - no2)== 20)
                        //{
                        //    items[i] = "大半会";
                        //}
                        //else
                        //{
                        //    if( no1 < no2)
                        //    {
                        //        if( no2 + 20 == no1 + 60)
                        //        {
                        //            items[i] = "大半会";
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (no1 + 20 == no2 + 60)
                        //        {
                        //            items[i] = "大半会";
                        //        }
                        //    }
                        //}
                    }

                    lstGouhouSanpouResult.Add(result);
                }
            }

            //例外表示のためのbEnableフラグ設定
            SetExceptionValueEnable(unKansi1, unKansi2, taiunKansi, nenunKansi, ref lstGouhouSanpouResult);

            List<GouhouSannpouResult> lstResult = new List<GouhouSannpouResult>();

            if (!string.IsNullOrEmpty(nentin)) lstResult.Add(new GouhouSannpouResult(nentin));
            if (!string.IsNullOrEmpty(rittin)) lstResult.Add(new GouhouSannpouResult(rittin));
            if (!string.IsNullOrEmpty(tensatu)) lstResult.Add(new GouhouSannpouResult(tensatu));
//            if (!string.IsNullOrEmpty(kangou)) lstResult.Add(new GouhouSannpouResult(kangou));

            lstResult.AddRange(lstGouhouSanpouResult);

            return lstResult.ToArray();
        }

        /// <summary>
        /// 例外表示処理
        /// </summary>
        /// <param name="kansi1">干支1</param>
        /// <param name="kansi2">干支2</param>
        /// <param name="taiun">大運干支</param>
        /// <param name="nenun">年運干支</param>
        /// <param name="values">表示文字列配列</param>
        /// <returns></returns>
        private void SetExceptionValueEnable(Kansi kansi1, Kansi kansi2, Kansi taiun, Kansi nenun, ref List<GouhouSannpouResult> values)
        {
            if (kansi1.si == "亥" && kansi2.si == "寅" ||
                kansi1.si == "寅" && kansi2.si == "亥")
            {
                bool bHit = false;
                if (taiun!=null && (taiun.si == "申" || taiun.si == "巳")) bHit = true;
                else if (nenun!=null && (nenun.si == "申" || nenun.si == "巳")) bHit = true;
                else if (IsExistStrInKansiSi(new string[] { "申", "巳" })) bHit = true;

                if (bHit)
                {
                    foreach (var v in values)
                    {
                        v.bEnable = (v.orgName == "破") ? true : false;
                    }
                }
                else
                {
                    foreach (var v in values)
                    {
                        v.bEnable =  (v.orgName == "支合") ? true : false;
                    }
                }
            }
            else if (kansi1.si == "申" && kansi2.si == "巳" ||
                     kansi1.si == "巳" && kansi2.si == "申")
            {
                bool bHit = false;
                if (taiun!=null && (taiun.si == "亥" || taiun.si == "寅")) bHit = true;
                else if (nenun!=null && (nenun.si == "亥" || nenun.si == "寅")) bHit = true;
                else if (IsExistStrInKansiSi(new string[] { "亥", "寅" })) bHit = true;

                if (bHit)
                {
                    foreach (var v in values)
                    {
                        v.bEnable = (v.orgName == "破") ? true : false;
                        v.bEnable = (v.orgName == "生貴刑") ? true : false;
                     }
                }
                else
                {
                    foreach (var v in values)
                    {
                        v.bEnable = (v.orgName == "支合") ? true : false;
                    }
                }
            }
        }
        //public string[] GetGouhouSanpou(string siName1, string siName2, bool bExistTensatuTichu, bool bExistNentin)
        //{
        //    return tblMng.gouhouSanpouTbl.GetGouhouSanpou(siName1, siName2, bExistTensatuTichu, bExistNentin);
        //}

        /// <summary>
        /// 合法・散法 文字をカンマ区切りで接続した１つの文字列で取得します。
        /// ・天殺地冲または、納音がある場合は、"冲動"は除外されます。
        /// ・半会の場合、２つの干支番号に20の差異があれば"大半会"に置き換えます。
        /// </summary>
        /// <param name="nenunTaiunKansi">年運、大運 干支</param>
        /// <param name="kansi">宿命干支（日、月、年）</param>
        /// <param name="bExistTensatuTichu">true...天殺地冲あり</param>
        /// <param name="bExistNentin">true...納音あり</param>
        /// <returns></returns>
        public string GetGouhouSanpouString(Kansi nenunTaiunKansi, Kansi kansi, bool bExistTensatuTichu, bool bExistNentin)
        {
            var items =  GetGouhouSanpou(nenunTaiunKansi, kansi, bExistTensatuTichu, bExistNentin);
            string s = "";
            if (items != null)
            {
                foreach (var item in items)
                {
                    if (!item.bEnable) continue;
                    if (s != "") s += ",";
                    s += item.orgName;
                }
            }
            return s;
        }
        //public string GetGouhouSanpouString(string siName1, string siName2, bool bExistTensatuTichu, bool bExistNentin)
        //{
        //   return  tblMng.gouhouSanpouTbl.GetGouhouSanpouString(siName1, siName2, bExistTensatuTichu, bExistNentin);
        //}

        /// <summary>
        /// 合法三方（日、月）取得
        /// </summary>
        /// <returns></returns>
        public string[] GetGouhouSanpouNitiGetu()
        {
            return GetGouhouSanpou(nikkansi, gekkansi, false, false).Select(x=> x.orgName).ToArray();
        }
        /// <summary>
        /// 合法三方（月、年）取得
        /// </summary>
        /// <returns></returns>
        public string[] GetGouhouSanpouiGetuNen()
        {
            return GetGouhouSanpou(gekkansi, nenkansi, false, false).Select(x => x.orgName).ToArray();
        }
        /// <summary>
        /// 合法三方（日、年）取得
        /// </summary>
        /// <returns></returns>
        public string[] GetGouhouSanpouiNitiNen()
        {
            return GetGouhouSanpou(nikkansi, nenkansi, false, false).Select(x => x.orgName).ToArray();
        }

        /// <summary>
        /// 納音、準納音 取得
        /// </summary>
        /// <param name="nenunKansi"></param>
        /// <param name="kansi"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 律音、準律音 取得
        /// </summary>
        /// <param name="nenunKansi"></param>
        /// <param name="kansi"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 干合 関係（干）
        /// 日→月
        /// </summary>
        /// <returns></returns>
        public bool IsKangouNitiGetsuKan()
        {
            return tblMng.kangouTbl.IsKangou(nikkansi.kan, gekkansi.kan);
        }
        /// <summary>
        /// 干合 関係（干）
        /// 月→年
        /// </summary>
        /// <returns></returns>
        public bool IsKangoGetsuNenKan()
        {
            return tblMng.kangouTbl.IsKangou(gekkansi.kan, nenkansi.kan);
        }
        /// <summary>
        /// 干合 関係（干）
        /// 日→年
        /// </summary>
        /// <returns></returns>
        public bool IsKangoNitiNenKan()
        {
            return tblMng.kangouTbl.IsKangou(nikkansi.kan, nenkansi.kan);
        }
        /// <summary>
        /// 大運→（日月年）,年運→（日月年）,他日→月などでも可
        /// </summary>
        /// <param name="kansi1"></param>
        /// <param name="kansi2"></param>
        /// <returns></returns>
        public bool IsKango(Kansi kansi1, Kansi kansi2)
        {
            return tblMng.kangouTbl.IsKangou(kansi1.kan, kansi2.kan);
        }
        /// <summary>
        /// 干合 文字列取得
        /// </summary>
        /// <param name="taiunKansi"></param>
        /// <param name="kansi"></param>
        /// <returns></returns>
        public string GetKangoStr(Kansi taiunKansi, Kansi kansi)
        {
            return tblMng.kangouTbl.GetKangouStr(taiunKansi.kan, kansi.kan);
        }


        //----------------------------------------------------
        //七殺
        //----------------------------------------------------
        /// <summary>
        /// 七殺 データ取得
        /// </summary>
        /// <param name="taiunKansi"></param>
        /// <param name="kansi"></param>
        /// <returns></returns>
        public Nanasatsu GetNanasatu(Kansi taiunKansi, Kansi kansi)
        {
            return GetNanasatu(taiunKansi.kan, kansi.kan);
        }
        /// <summary>
        /// 七殺 データ取得
        /// </summary>
        /// <param name="taiunKan"></param>
        /// <param name="kan"></param>
        /// <returns></returns>
        public Nanasatsu GetNanasatu(string taiunKan, string kan)
        {
            return tblMng.nanasatsuTbl.GetNanasatsu(taiunKan, kan);
        }

        /// <summary>
        /// 七殺判定　日→月
        /// </summary>
        /// <returns></returns>
        public bool IsNanasatuNitiGetuKan()
        {
            return GetNanasatu(nikkansi.kan, gekkansi.kan) != null ? true : false;
        }
        /// <summary>
        /// 七殺判定　月→年
        /// </summary>
        /// <returns></returns>
        public bool IsNanasatuGetuNenKan()
        {
            return GetNanasatu(gekkansi.kan, nenkansi.kan) != null ? true : false;
        }
        /// <summary>
        /// 七殺判定　日→年
        /// </summary>
        /// <returns></returns>
        public bool IsNanasatuNitiNenKan()
        {
            return GetNanasatu(nikkansi.kan, nenkansi.kan) != null ? true : false;
        }

        /// <summary>
        /// 七殺判定
        /// 大運→（日月年）,年運→（日月年）,他日→月などでも加
        /// </summary>
        /// <param name="kansi1"></param>
        /// <param name="kansi2"></param>
        /// <returns></returns>
        public bool IsNanasatu(Kansi kansi1, Kansi kansi2)
        {
            return IsNanasatu(kansi1.kan, kansi2.kan);
        }
        /// <summary>
        /// 七殺判定
        /// 大運→（日月年）,年運→（日月年）,他日→月などでも加
        /// </summary>
        /// <param name="kan1"></param>
        /// <param name="kan2"></param>
        /// <returns></returns>
        public bool IsNanasatu(string kan1, string kan2)
        {
            return GetNanasatu(kan1, kan2) != null ? true : false;
        }

        //----------------------------------------------------
        //天殺
        //----------------------------------------------------
        /// <summary>
        /// 天殺 文字列取得
        /// </summary>
        /// <param name="taiunKansi"></param>
        /// <param name="kansi"></param>
        /// <returns></returns>
        public string GetTensatuString(Kansi taiunKansi, Kansi kansi)
        {
            return IsNanasatu(taiunKansi, kansi) ? "天殺" : "";
        }

        /// <summary>
        /// 地冲 　文字列取得
        /// </summary>
        /// <param name="taiunKansi"></param>
        /// <param name="kansi"></param>
        /// <returns></returns>
        public string GetTichuString(Kansi taiunKansi, Kansi kansi)
        {
            var values = GetGouhouSanpou(taiunKansi, kansi, false, false);
            if (values != null)
            {
                foreach (var item in values)
                {
                    if (item.orgName.IndexOf("冲動") >= 0) return "地冲";
                }
            }
            return "";
        }
        /// <summary>
        /// 天殺地冲 　文字列取得
        /// </summary>
        /// <param name="taiunKansi"></param>
        /// <param name="kansi"></param>
        /// <returns></returns>
        public string GetTensatuTichuString(Kansi taiunKansi, Kansi kansi)
        {
            string tensatu = GetTensatuString(taiunKansi, kansi);
            string tichu = GetTichuString(taiunKansi, kansi);
            if (tensatu != "" && tichu != "") return tensatu + tichu;

            return "";
        }

        //----------------------------------------------------
        //三合会局
        //----------------------------------------------------
        /// <summary>
        /// 三合会局 　情報取得
        /// </summary>
        /// <param name="getuun"></param>
        /// <param name="nenun"></param>
        /// <param name="taiun"></param>
        /// <returns></returns>
        public List<TableMng.SangouKaikyokuResult> GetSangouKaikyoku(Kansi getuun, Kansi nenun, Kansi taiun)
        {
           return  tblMng.sangouKaikyokuTbl.GetSangouKaikyoku(getuun, nenun, taiun,
                                                nikkansi, gekkansi, nenkansi);
        }

        //----------------------------------------------------
        //方三位
        //----------------------------------------------------
        /// <summary>
        /// 方三位 情報取得
        /// </summary>
        /// <param name="getuun"></param>
        /// <param name="nenun"></param>
        /// <param name="taiun"></param>
        /// <returns></returns>
        public List<TableMng.HouSaniResult> GetHouSani(Kansi getuun, Kansi nenun, Kansi taiun)
        {
            return tblMng.housanniTbl.GetHouSani(getuun, nenun, taiun,
                                                 nikkansi, gekkansi, nenkansi);
        }

    }

    /// <summary>
    /// 誕生日情報クラス
    /// </summary>
    public class Birthday
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
    /// グループ情報
    /// </summary>
    public class Group
    {
        public Group(string name)
        {
            groupName = name;
        }
        public void AddMember(Person member)
        {
            members.Add(member);
        }
        public override string ToString()
        {
            return groupName;
        }
        public string groupName;
        public List<Person> members = new List<Person>();
    }
    /// <summary>
    /// 経歴情報
    /// </summary>
    public class Career
    {
        /// <summary>
        /// 経歴情報登録ファイルの項目順定義
        /// </summary>
        enum CarrerListCol
        {
            COL_YEAR = 0,   //年
            COL_CAREER,     //経歴
        };

        Person person;
        // 経歴 <Year,経歴文字列>
        public Dictionary<int, string> dicCareer = new Dictionary<int, string>();
        public string careerFilePath;

        public Career(Person _person)
        {
            person = _person;
        }

        public string this[ int year]
        {
            get
            {
                if( !dicCareer.ContainsKey(year)) return null;
                return dicCareer[year];
            }
            set
            {
                if (!dicCareer.ContainsKey(year))
                {
                    dicCareer.Add(year, value);
                }
                else
                {
                    dicCareer[year] = value;
                }
            }
        }
        public string GetDataFilePath()
        {
            string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            appPath = System.IO.Path.GetDirectoryName(appPath);
            string fileName = person.name.Replace(" ", "");
            fileName = fileName.Replace("　", "");
            return  appPath + @"\Career\" + fileName + ".csv";
        }
        public int ReaCareerFile()
        {

            careerFilePath = GetDataFilePath();

            if (!System.IO.File.Exists(careerFilePath)) return 0;

            try
            {
                //ファイルのエンコードタイプ取得
                //Encoding.GetEncoding()

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using (StreamReader sr = new StreamReader(careerFilePath, Encoding.GetEncoding("shift-jis")))
                {
                    // 末尾まで繰り返す
                    while (!sr.EndOfStream)
                    {
                        // CSVファイルの一行を読み込む
                        string line = sr.ReadLine();
                        // 読み込んだ一行をカンマ毎に分けて配列に格納する
                        string[] values = line.Split(',');
                        //年
                        string sYear = values[(int)CarrerListCol.COL_YEAR];
                        if (sYear == "") break;
                        int year = int.Parse(sYear);

                        //経歴
                        string sCareer = values[(int)CarrerListCol.COL_CAREER];

                        if (!dicCareer.ContainsKey(year))
                        {
                            dicCareer.Add(int.Parse(sYear), sCareer);
                        }
                        else
                        {
                            dicCareer[year] += ",";
                            dicCareer[year] += sCareer;

                        }
                    }
                }
            }catch
            {
            }

            //var version = "xls";
            //var workbook = ExcelReader.GetWorkbook(filePath, version);
            //if (workbook == null)
            //{
            //    return -1;
            //}

            //var sheet = workbook.GetSheetAt(0);

            //int iRow = 0;
            //while (true)
            //{
            //    //年
            //    string sYear = ExcelReader.CellValue(sheet, iRow, (int)CarrerListCol.COL_YEAR);
            //    if (sYear == "") break;
            //    int year = int.Parse(sYear);

            //    //経歴
            //    string sCareer = ExcelReader.CellValue(sheet, iRow, (int)CarrerListCol.COL_CAREER);

            //    if (!dicCareer.ContainsKey(year))
            //    {
            //        dicCareer.Add(int.Parse(sYear), sCareer);
            //    }
            //    else
            //    {
            //        dicCareer[year] += ",";
            //        dicCareer[year] += sCareer;

            //    }
            //    iRow++;
            //}

            return 0;
        }
        public int Save()
        {
            if(string.IsNullOrEmpty(careerFilePath))
            {
                //読み込み時ファイルがない場合
                careerFilePath = GetDataFilePath();
            }
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using (StreamWriter sw = new StreamWriter(careerFilePath, false, Encoding.GetEncoding("shift-jis")))
                {
                    foreach (var item in dicCareer.OrderBy(c => c.Key))
                    {
                        string s = string.Format("{0},{1}", item.Key, item.Value);
                        sw.WriteLine(s);
                    }
                }

            }catch
            {
            }

            //var version = "xls";
            //var workbook = ExcelReader.CreateWorkbook();
            //if (workbook == null)
            //{
            //    return -1;
            //}

            //var sheet = workbook.CreateSheet();

            //int iRow = 0;
            //foreach(var item in dicCareer.OrderBy(c => c.Key))
            //{
            //    //年
            //    var cell = ExcelReader.GetCell(sheet, iRow, (int)CarrerListCol.COL_YEAR);
            //    cell.SetCellValue( item.Key );

            //    //経歴
            //    cell = ExcelReader.GetCell(sheet, iRow, (int)CarrerListCol.COL_CAREER);
            //    cell.SetCellValue(item.Value);

            //    iRow++;
            //}
            //ExcelReader.WriteExcel(workbook, careerFilePath);

            return 0;
        }

    }

}
