using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing;
using System.Diagnostics;

namespace WinFormsApp2
{
    public enum FindCondition
    {
        COND_OR = 0,
        COND_AND
    }
    public enum FindTarget
    {
        TARGET_DB = 0x0001,
        TARGET_BIRTHDAY = 0x0002
    }

    public class FindParameter
    {
        public FindCondition cond;
        public int target;
        public int minYear;
        public int minYearSetuiribi;
        public int maxYear;

        //干支
        public Kansi nikkansi = new Kansi();
        public Kansi gekkansi = new Kansi();
        public Kansi nenkansi = new Kansi();
        //天中殺
        public string[] tenchusatuNiti = new string[2];
        public string[] tenchusatuNen = new string[2];


        //蔵元
        public string zouganNitiShogen;
        public string zouganNitiChugen;
        public string zouganNitiHongen;
        public string zouganGetuShogen;
        public string zouganGetuChugen;
        public string zouganGetuHongen;
        public string zouganNenShogen;
        public string zouganNenChugen;
        public string zouganNenHongen;
        ////二十八元素
        //public NijuhachiGenso nijuhachiGensoNikkansi { get; set; }
        //public NijuhachiGenso nijuhachiGensoGekkansi { get; set; }
        //public NijuhachiGenso nijuhachiGensoNenkansi { get; set; }

        //十大主星
        public string judaiShuseiA;
        public string judaiShuseiB;
        public string judaiShuseiC;
        public string judaiShuseiD;
        public string judaiShuseiE;

        public string judaiShuseiAttrA;
        public string judaiShuseiAttrB;
        public string judaiShuseiAttrC;
        public string judaiShuseiAttrD;
        public string judaiShuseiAttrE;
        //十二大主星
        public string junidaiJuseiA;
        public string junidaiJuseiB;
        public string junidaiJuseiC;
        public string junidaiJuseiAttrA;
        public string junidaiJuseiAttrB;
        public string junidaiJuseiAttrC;

    }



    /// <summary>
    /// 拡張検索機能クラス
    /// </summary>
    public class FinderCustom
    {
        TableMng tblMng = TableMng.GetTblManage();

        /// <summary>
        /// 検索結果
        /// </summary>
        public class FindResult
        {
            public List<FindItem> lstFindItemsDB = new List<FindItem>();
            public List<ResultFormatTargetDB> lstFormatTargetDB = new List<ResultFormatTargetDB>();

            public List<FindItem> lstFindItemsBIRTHDAY = new List<FindItem>();
            public List<ResultFormatTargetBIRTHDAY> lstFormatTargetBIRTHDAY = new List<ResultFormatTargetBIRTHDAY>();

            public FindResult()
            {
            }

            public void AddDB(FindItem item)
            {
                lstFindItemsDB.Add(item);
            }
            public void AddDB(FindResult result)
            {
                foreach (var item in result.lstFindItemsDB)
                {
                    lstFindItemsDB.Add(item);
                }
            }
            public void AddBIRTHDAY(FindItem item)
            {
                lstFindItemsBIRTHDAY.Add(item);
            }
            public void AddBIRTHDAY(FindResult result)
            {
                foreach (var item in result.lstFindItemsDB)
                {
                    lstFindItemsBIRTHDAY.Add(item);
                }
            }

            //public bool IsFinded()
            //{
            //    return lstFindItems.Count > 0 ? true : false;
            //}

        }
        /// <summary>
        /// 検索結果項目
        /// </summary>
        public class FindItem
        {
            public FindItem(Person _person)
            {
                person = _person;
            }
            public Person person = null;
            public List<object> lstItem = new List<object>();
        }
        /// <summary>
        /// 検索結果表示フォーマット
        /// </summary>
        public class ResultFormatTargetDB
        {
            public enum Type
            {
                NONE = 0,
                PERSON_NAME,
            }
            public ResultFormatTargetDB(string _title, int width,
                                HorizontalAlignment _alignment = HorizontalAlignment.Left,
                                Type _type = Type.NONE)
            {
                title = _title;
                columnWidth = width;
                type = _type;
                alignment = _alignment;

            }
            public Type type;
            public string title;
            public int columnWidth;
            public HorizontalAlignment alignment;
        }

        public class ResultFormatTargetBIRTHDAY
        {
            public enum Type
            {
                NONE = 0,
                YEAR,
                MONTH,
                DAY,
  

            }
            public ResultFormatTargetBIRTHDAY(string _title, int width,
                                HorizontalAlignment _alignment = HorizontalAlignment.Left,
                                Type _type = Type.NONE)
            {
                title = _title;
                columnWidth = width;
                type = _type;
                alignment = _alignment;

            }
            public Type type;
            public string title;
            public int columnWidth;
            public HorizontalAlignment alignment;
        }

        public FindResult Find(FindParameter param)
        {

            FindResult result = new FindResult();

            result = FindInyou(param);
            return result;
        }


        //ローカルデータから陰陽検索
        private FindResult FindInyou(FindParameter param)
        {
            FindResult result = new FindResult();

 
            if ((param.target & (int)FindTarget.TARGET_DB)!=0)
            {

                //検索結果表示用カラムフォーマット定義
                result.lstFormatTargetDB = new List<ResultFormatTargetDB>()
                {
                   // new ResultFormat("氏名",100, ResultFormat.Type.PERSON_NAME),
                    new ResultFormatTargetDB("氏名",150, HorizontalAlignment.Right, ResultFormatTargetDB.Type.PERSON_NAME),
                };


                Persons persons = Persons.GetPersons();

                foreach (var person in persons.GetPersonList())
                {
                    if (IsSameCondition(param, person))
                    {
                        FindItem item = new FindItem(person);
                        result.AddDB(item);

                    }
                }
            }
            if ((param.target & (int)FindTarget.TARGET_BIRTHDAY) != 0)
            {
                //検索結果表示用カラムフォーマット定義
                result.lstFormatTargetBIRTHDAY = new List<ResultFormatTargetBIRTHDAY>()
                {
                   // new ResultFormat("氏名",100, ResultFormat.Type.PERSON_NAME),
                    new ResultFormatTargetBIRTHDAY("年",60, HorizontalAlignment.Right, ResultFormatTargetBIRTHDAY.Type.YEAR),
                    new ResultFormatTargetBIRTHDAY("月",60, HorizontalAlignment.Right, ResultFormatTargetBIRTHDAY.Type.MONTH),
                    new ResultFormatTargetBIRTHDAY("日",60, HorizontalAlignment.Right, ResultFormatTargetBIRTHDAY.Type.DAY),
                    new ResultFormatTargetBIRTHDAY("性別",80, HorizontalAlignment.Right, ResultFormatTargetBIRTHDAY.Type.DAY),
                };


                DateTime fromDT = new DateTime(param.minYear, 2, param.minYearSetuiribi);
                DateTime ToDT = new DateTime(param.maxYear, 1, 31);
                for (DateTime dt = fromDT; dt<=ToDT;  dt = dt.AddDays(1))
                {
                    
                    //節入り日を設定したいが、検索速度をあげるため、
                    //節入り日より前の日にならない程度の日をpersonの誕生日に設定する。
                    //取あえず20日あたりでよいか...
                    Person man = new Person("名無し", Gender.MAN, PersonClassIdentity.Birthday);
                    Person woman = new Person("名無し", Gender.WOMAN, PersonClassIdentity.Birthday);
                    man.InitEx(tblMng, dt.Year, dt.Month, dt.Day);
                    woman.InitEx(tblMng, dt.Year, dt.Month, dt.Day);

                    List<Gender> lstFindedGender = new List<Gender>();
                    FindItem findItem = null;
                    if (IsSameCondition(param, man))
                    {
                        findItem = new FindItem(man);
                        findItem.lstItem.Add(man.birthday.year.ToString()); //年
                        findItem.lstItem.Add(man.birthday.month.ToString());//月
                        findItem.lstItem.Add(man.birthday.day.ToString()); //日
                        lstFindedGender.Add(Gender.MAN);

                    }
                    if (IsSameCondition(param, woman))
                    {
                        if (findItem == null)
                        {
                            findItem = new FindItem(woman);
                            findItem.lstItem.Add(woman.birthday.year.ToString()); //年
                            findItem.lstItem.Add(woman.birthday.month.ToString());//月
                            findItem.lstItem.Add(woman.birthday.day.ToString()); //日
                        }
                        lstFindedGender.Add(Gender.WOMAN);

                    }
                    if (findItem != null)
                    {
                        string s="";
                        foreach(var gender in lstFindedGender)
                        {
                            if (!string.IsNullOrEmpty(s)) s += ",";
                            if (gender == Gender.MAN) s += "男性";
                            else if (gender == Gender.WOMAN) s += "女性";
                        }
                        findItem.lstItem.Add(s); //性別
                        result.AddBIRTHDAY(findItem);
                    }

                }
            }

                return result;
        }

        public bool IsSameCondition(FindParameter param, Person person)
        {
            bool bResult = false;
            bool bMatch = IsSameConditionInsen( param,  person);
            if (IsReturn(param, bMatch, ref bResult)) return bResult;

            bMatch = IsSameConditionYousen(param, person);
            if (IsReturn(param, bMatch, ref bResult)) return bResult;

            if (param.cond == FindCondition.COND_OR) return false;
            else return true;

        }
        /// <summary>
        /// 陰占
        /// </summary>
        /// <param name="param"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        public bool IsSameConditionInsen(FindParameter param, Person person)
        { 
            bool bMatch = false;
            bool bResult = false;

            if (!string.IsNullOrEmpty(param.nikkansi.kan))
            {
                bMatch = (param.nikkansi.kan == person.nikkansi.kan);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            if (!string.IsNullOrEmpty(param.nikkansi.si))
            {
                bMatch = (param.nikkansi.si == person.nikkansi.si);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            if (!string.IsNullOrEmpty(param.gekkansi.kan))
            {
                bMatch = (param.gekkansi.kan == person.gekkansi.kan);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            if (!string.IsNullOrEmpty(param.gekkansi.si))
            {
                bMatch = (param.gekkansi.si == person.gekkansi.si);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            if (!string.IsNullOrEmpty(param.nenkansi.kan))
            {
                bMatch = (param.nenkansi.kan == person.nenkansi.kan);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            if (!string.IsNullOrEmpty(param.nenkansi.si))
            {
                bMatch = (param.nenkansi.si == person.nenkansi.si);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            //天中殺
            if (!string.IsNullOrEmpty(param.tenchusatuNiti[0]))
            {
                bMatch = (param.tenchusatuNiti[0] == person.nikkansi.tenchusatu[0]);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            if (!string.IsNullOrEmpty(param.tenchusatuNiti[1]))
            {
                bMatch = (param.tenchusatuNiti[1] == person.nikkansi.tenchusatu[1]);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            if (!string.IsNullOrEmpty(param.tenchusatuNen[0]))
            {
                bMatch = (param.tenchusatuNen[0] == person.nenkansi.tenchusatu[0]);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            if (!string.IsNullOrEmpty(param.tenchusatuNen[1]))
            {
                bMatch = (param.tenchusatuNen[1] == person.nenkansi.tenchusatu[1]);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            Insen insen = new Insen(person);
            //蔵元
            if (!string.IsNullOrEmpty(param.zouganNitiShogen))
            {
                bMatch = (param.zouganNitiShogen == insen.nikkansiHongen[(int)NijuhachiGenso.enmGensoType.GENSO_SHOGEN].name);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            if (!string.IsNullOrEmpty(param.zouganNitiChugen))
            {
                bMatch = (param.zouganNitiChugen == insen.nikkansiHongen[(int)NijuhachiGenso.enmGensoType.GENSO_CHUGEN].name);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            if (!string.IsNullOrEmpty(param.zouganNitiHongen))
            {
                bMatch = (param.zouganNitiHongen == insen.nikkansiHongen[(int)NijuhachiGenso.enmGensoType.GENSO_HONGEN].name);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            if (!string.IsNullOrEmpty(param.zouganGetuShogen))
            {
                bMatch = (param.zouganGetuShogen == insen.gekkansiHongen[(int)NijuhachiGenso.enmGensoType.GENSO_SHOGEN].name);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            if (!string.IsNullOrEmpty(param.zouganGetuChugen))
            {
                bMatch = (param.zouganGetuChugen == insen.gekkansiHongen[(int)NijuhachiGenso.enmGensoType.GENSO_CHUGEN].name);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            if (!string.IsNullOrEmpty(param.zouganGetuHongen))
            {
                bMatch = (param.zouganGetuHongen == insen.gekkansiHongen[(int)NijuhachiGenso.enmGensoType.GENSO_HONGEN].name);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            if (!string.IsNullOrEmpty(param.zouganNenShogen))
            {
                bMatch = (param.zouganNenShogen == insen.nenkansiHongen[(int)NijuhachiGenso.enmGensoType.GENSO_SHOGEN].name);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            if (!string.IsNullOrEmpty(param.zouganNenChugen))
            {
                bMatch = (param.zouganNenChugen == insen.nenkansiHongen[(int)NijuhachiGenso.enmGensoType.GENSO_CHUGEN].name);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            if (!string.IsNullOrEmpty(param.zouganNenHongen))
            {
                bMatch = (param.zouganNenHongen == insen.nenkansiHongen[(int)NijuhachiGenso.enmGensoType.GENSO_HONGEN].name);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }

            if (param.cond == FindCondition.COND_OR) return false;
            else return true;
        }

        /// <summary>
        /// 陽占
        /// </summary>
        /// <param name="param"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        public bool IsSameConditionYousen(FindParameter param, Person person)
        {
            bool bMatch = false;
            bool bResult = false;
            if (!string.IsNullOrEmpty(param.judaiShuseiA))
            {
                bMatch = (param.judaiShuseiA == person.judaiShuseiA.name);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            if (!string.IsNullOrEmpty(param.judaiShuseiB))
            {
                bMatch = (param.judaiShuseiB == person.judaiShuseiB.name);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            if (!string.IsNullOrEmpty(param.judaiShuseiC))
            {
                bMatch = (param.judaiShuseiC == person.judaiShuseiC.name);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            if (!string.IsNullOrEmpty(param.judaiShuseiD))
            {
                bMatch = (param.judaiShuseiD == person.judaiShuseiD.name);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            if (!string.IsNullOrEmpty(param.judaiShuseiE))
            {
                bMatch = (param.judaiShuseiE == person.judaiShuseiE.name);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            if (!string.IsNullOrEmpty(param.junidaiJuseiA))
            {
                bMatch = (param.junidaiJuseiA == person.junidaiJuseiA.name);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            if (!string.IsNullOrEmpty(param.junidaiJuseiB))
            {
                bMatch = (param.junidaiJuseiB == person.junidaiJuseiB.name);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }
            if (!string.IsNullOrEmpty(param.junidaiJuseiC))
            {
                bMatch = (param.junidaiJuseiC == person.junidaiJuseiC.name);
                if (IsReturn(param, bMatch, ref bResult)) return bResult;
            }


            if (param.cond == FindCondition.COND_OR) return false;
            else return true;
        }
        bool IsReturn(FindParameter param, bool isMatch, ref bool retValue)
        {
            if (isMatch)
            {
                if (param.cond == FindCondition.COND_OR)
                {
                    retValue = true;
                    return true;
                }
            }
            else
            {
                if (param.cond == FindCondition.COND_AND)
                {
                    retValue = false;
                    return true;
                }
            }
            return false;
        }
    }
}