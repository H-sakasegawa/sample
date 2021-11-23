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

    class Isouhou : IsouhouBase
    {
        public Isouhou(Person person) : base(person)
        {
        }
        protected override void DrawItem(Graphics g) { }

    }

    /// <summary>
    /// 検索機能クラス
    /// </summary>
    public class Finder
    {
        /// <summary>
        /// 検索結果
        /// </summary>
        public class FindResult
        {
            public List<FindItem> lstFindItems = new List<FindItem>();
            public List<ResultFormat> lstFormat = new List<ResultFormat>();

            public FindResult()
            {
            }

            public void Add(FindItem item)
            {
                lstFindItems.Add(item);
            }
            public void Add(FindResult result)
            {
                foreach (var item in result.lstFindItems)
                {
                    lstFindItems.Add(item);
                }
            }

            public bool IsFinded()
            {
                return lstFindItems.Count > 0 ? true : false;
            }

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
            public List<string> lstItem = new List<string>();
        }
        /// <summary>
        /// 検索結果表示フォーマット
        /// </summary>
        public class ResultFormat
        {
            public enum Type
            {
                NONE = 0,
                PERSON_NAME,
                YEAR,
                MONTH,
                UN


            }
            public ResultFormat(string _title, int width,
                                HorizontalAlignment _alignment= HorizontalAlignment.Left, 
                                Type _type = Type.NONE )
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




        TableMng tblMng = TableMng.GetTblManage();
        string[] artNitiGetuNenStr = new string[] { "日干支", "月干支", "年干支" };

        #region 納音、準納音 or 律音、準律音 検索
        /// <summary>
        /// 納音、準納音 or 律音、準律音 検索
        /// </summary>
        /// <param name="person">検索対象人情報</param>
        /// <param name="mode">0..納音、準納音 検索,  1..律音、準律音　検索</param>
        /// <param name="bTenchusatu">true...天中殺のものを抽出</param>
        /// <returns></returns>
        public FindResult FindNattinOrRittin(Person person, int mode, bool bTenchusatu)
        {

            FindResult result = new FindResult();

            //検索結果表示用カラムフォーマット定義
            result.lstFormat = new List<ResultFormat>()
            {
               // new ResultFormat("氏名",100, ResultFormat.Type.PERSON_NAME),
                new ResultFormat("年",50, HorizontalAlignment.Right, ResultFormat.Type.YEAR),
                new ResultFormat("月",30, HorizontalAlignment.Right,ResultFormat.Type.MONTH),
                new ResultFormat("運",50, HorizontalAlignment.Left, ResultFormat.Type.UN),
                new ResultFormat("干支",50),
                new ResultFormat("検索値",200),
            };

            string[] findStr;
            if (mode == 0) findStr = new string[] { Const.sNattin, Const.sJunNattin };
            else findStr = new string[] { Const.sRittin, Const.sJunRittin };


            //日干支
            Kansi[] aryKansi = new Kansi[] { person.nikkansi, person.gekkansi, person.nenkansi };
            //大運干支
            var lstTaiunKansi = person.GetTaiunKansiList();

            foreach (var item in lstTaiunKansi)
            {
                Kansi taiunKansi = person.GetKansi(item.kansiNo);

                for (int i = 0; i < aryKansi.Length; i++)
                {
                    string str = "";
                    if (mode == 0) str = person.GetNentin(aryKansi[i], taiunKansi);
                    else str = person.GetRittin(aryKansi[i], taiunKansi);

                    if (bTenchusatu)
                    {
                        foreach (var tenchusatu in person.nikkansi.tenchusatu.ToArray())
                        {
                            //支に天中殺文字があるか？
                            //IsExist()では、干と支で同じものがあるかをチェックしている。
                            //tenchusatuには支の文字しかこないので、この関数でチェックしてもOK
                            if (taiunKansi.IsExist(tenchusatu))
                            {
                                break;
                            }
                        }
                    }
                    if (str == findStr[0] || str == findStr[1])
                    { 
                        //天中殺指定があった場合で天中殺でなければSIKIP
                        if (bTenchusatu && !IsTenchusatu(person, taiunKansi)) continue;

                        //発見！
                        FindItem findItem = new FindItem(person);
                       // findItem.lstItem.Add(person.name);
                        findItem.lstItem.Add(item.year.ToString()); //年
                        findItem.lstItem.Add("");//月
                        findItem.lstItem.Add(Const.sTaiun); //"大運"
                        findItem.lstItem.Add(artNitiGetuNenStr[i]);
                        findItem.lstItem.Add(str);

                        result.Add(findItem);
                    }
                }
                //年運・月運検索
                result.Add( FindNattinOrRittinNenuni(person, item.year, findStr, aryKansi, mode, bTenchusatu));

            }
            return result;
        }

        /// <summary>
        /// 干支の支が天中殺かを判断
        /// </summary>
        /// <param name="person"></param>
        /// <param name="kansi"></param>
        /// <returns></returns>
        private bool IsTenchusatu(Person person, Kansi kansi )
        {
            foreach (var tenchusatu in person.nikkansi.tenchusatu.ToArray())
            {
                //支に天中殺文字があるか？
                //IsExist()では、干と支で同じものがあるかをチェックしている。
                //tenchusatuには支の文字しかこないので、この関数でチェックしてもOK
                if (kansi.IsExist(tenchusatu))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 納音、準納音 or 律音、準律音 (年運検索）
        /// </summary>
        /// <param name="person">検索対象人情報</param>
        /// <param name="year">大運対象年</param>
        /// <param name="findStr">検索文字列</param>
        /// <param name="aryKansi">日干支、月干支、年干支</param>
        /// <param name="mode">0..納音、準納音 検索,  1..律音、準律音　検索</param>
        /// <param name="bTenchusatu">true...天中殺のものを抽出</param>
        /// <returns></returns>
        private FindResult FindNattinOrRittinNenuni(Person person, int year, string[] findStr, Kansi[] aryKansi, int mode, bool bTenchusatu)
        {
            FindResult result = new FindResult();
            //年運検索
            int nenunKansi = person.GetNenkansiNo(year, true);

            for (int nenCnt = 0; nenCnt < 10; nenCnt++)
            {
                //順行のみなので、60超えたら1にするだけ
                if (nenunKansi > 60) nenunKansi = 1;
                Kansi nenkansi = person.GetKansi(nenunKansi);
                int targetYear = year + nenCnt;

                for (int i = 0; i < aryKansi.Length; i++)
                {
                    string str = "";
                    if (mode == 0) str = person.GetNentin(aryKansi[i], nenkansi);
                    else str = person.GetRittin(aryKansi[i], nenkansi);

                    if (str == findStr[0] || str == findStr[1])
                    {
                        //天中殺指定があった場合で天中殺でなければSIKIP
                        if (bTenchusatu && !IsTenchusatu(person, nenkansi)) continue;

                        //発見！
                        FindItem findItem = new FindItem(person);
                        //findItem.lstItem.Add(person.name);
                        findItem.lstItem.Add(targetYear.ToString());//年
                        findItem.lstItem.Add("");//月
                        findItem.lstItem.Add(Const.sNenun); // "年運"
                        findItem.lstItem.Add(artNitiGetuNenStr[i]);
                        findItem.lstItem.Add(str);

                        result.Add(findItem);
                    }
                }
                //月運検索
                result.Add(FindNattinOrRittinGetuun(person, targetYear, findStr, aryKansi, mode, bTenchusatu));

                nenunKansi += 1;
            }

            return result;
        }
        /// <summary>
        /// 納音、準納音 or 律音、準律音 （月運検索）
        /// </summary>
        /// <param name="person">検索対象人情報</param>
        /// <param name="year">対象年</param>
        /// <param name="findStr">検索文字列</param>
        /// <param name="aryKansi">日干支、月干支、年干支</param>
        /// <param name="mode">0..納音、準納音 検索,  1..律音、準律音　検索</param>
        /// <param name="bTenchusatu">true...天中殺のものを抽出</param>
        /// <returns></returns>
        private FindResult FindNattinOrRittinGetuun(Person person, int year, string[] findStr, Kansi[] aryKansi, int mode, bool bTenchusatu)
        {
            FindResult result = new FindResult();
            //月運検索

            //2月～12月,1月分を表示
            for (int iGetu = 0; iGetu < 12; iGetu++)
            {
                int mMonth = Const.GetuunDispStartGetu + iGetu;
                if (mMonth > 12)
                {
                    mMonth = (mMonth - 12);
                    year = year + 1;
                }

                //月干支番号取得(節入り日無視で単純月で取得）
                int getuunKansiNo = tblMng.setuiribiTbl.GetGekkansiNo(year, mMonth);

                Kansi getuunKansi = person.GetKansi(getuunKansiNo);

                for (int i = 0; i < aryKansi.Length; i++)
                {
                    string str = "";
                    if (mode == 0) str = person.GetNentin(aryKansi[i], getuunKansi);
                    else str = person.GetRittin(aryKansi[i], getuunKansi);

                    if (str == findStr[0] || str == findStr[1])
                    {
                        //天中殺指定があった場合で天中殺でなければSIKIP
                        if (bTenchusatu && !IsTenchusatu(person, getuunKansi)) continue;

                        //発見！
                        FindItem findItem = new FindItem(person);
                        //findItem.lstItem.Add(person.name);
                        findItem.lstItem.Add(year.ToString());//年
                        findItem.lstItem.Add(mMonth.ToString());//月
                        findItem.lstItem.Add(Const.sGetuun); // "月運"
                        findItem.lstItem.Add(artNitiGetuNenStr[i]);
                        findItem.lstItem.Add(str);

                        result.Add(findItem);
                    }
                }
                getuunKansiNo += 1;
            }

            return result;
        }

        #endregion

        #region 現在の大運、年運の干 or 支の出現位置ににジャンプ
        /// <summary>
        /// 現在の大運、年運の干 or 支の出現位置ににジャンプ
        /// </summary>
        /// <param name="person">検索対象人情報</param>
        /// <param name="startYear">検索基準年</param>
        /// <param name="str">干or支文字</param>
        /// <param name="kansiMode">0..干で検索  1..支で検索</param>
        /// <returns></returns>
        public int FindNextKansi(Person person, int startYear, string str, int kansiMode)
        {
            int year = startYear + 1;
            //大運干支
            var lstTaiunKansi = person.GetTaiunKansiList();

            foreach (var item in lstTaiunKansi)
            {
                if (item.year < year) continue;

                Kansi taiunKansi = person.GetKansi(item.kansiNo);
                if (kansiMode == 0)
                {
                    if ( taiunKansi.kan == str)
                    {
                        return item.year;
                    }
                }
                else
                {
                    if (taiunKansi.si == str)
                    {
                        return item.year;
                    }
                }

                //年運
                int nenunYear = FindNextKanNenun(person, year, str, kansiMode); //支を検索

                if (nenunYear > 0)
                {
                    return nenunYear;
                }
            }
            return -1;
        }

        private int  FindNextKanNenun(Person person, int year, string str, int kansiMode)
        {
            FindResult result = new FindResult();
            //年運検索
            int nenkansiNo = person.GetNenkansiNo(year, true);

            for (int nenCnt = 0; nenCnt < 10; nenCnt++)
            {
                //順行のみなので、60超えたら1にするだけ
                if (nenkansiNo > 60) nenkansiNo = 1;
                Kansi nenkansi = person.GetKansi(nenkansiNo);
                int targetYear = year + nenCnt;

                if (kansiMode == 0)
                {
                    if (nenkansi.kan == str)
                    {
                        return targetYear;
                    }
                }
                else
                {
                    if (nenkansi.si == str)
                    {
                        return targetYear;
                    }
                }
                nenkansiNo += 1;
            }
            return -1;
        }

        #endregion



        public FindResult FindKyakkaHoukai(Person person)
        {

            FindResult result = new FindResult();

            bool bDispGogyou = true;
            bool bDispGotoku = true;

            //検索結果表示用カラムフォーマット定義
            result.lstFormat = new List<ResultFormat>()
            {
               // new ResultFormat("氏名",100, ResultFormat.Type.PERSON_NAME),
                new ResultFormat("年",50, HorizontalAlignment.Right, ResultFormat.Type.YEAR),
            };


             //大運干支
            var lstTaiunKansi = person.GetTaiunKansiList();

            foreach (var item in lstTaiunKansi)
            {
                Kansi taiunKansi = person.GetKansi(item.kansiNo);

                //年運検索
                int nenkansiNo = person.GetNenkansiNo(item.year, true);
                for (int nenCnt = 0; nenCnt < 10; nenCnt++)
                {
                    //順行のみなので、60超えたら1にするだけ
                    if (nenkansiNo > 60) nenkansiNo = 1;
                    Kansi nenunKansi = person.GetKansi(nenkansiNo);

                    int year = item.year + nenCnt;

                    ////月運検索
                    ////2月～12月,1月分を表示
                    //for (int iGetu = 0; iGetu < 12; iGetu++)
                    //{
                    //    int mMonth = Const.GetuunDispStartGetu + iGetu;
                    //    if (mMonth > 12)
                    //    {
                    //        mMonth = (mMonth - 12);
                    //        year = year + 1;
                    //    }

                    //    //月干支番号取得(節入り日無視で単純月で取得）
                    //    int gekkansiNo = tblMng.setuiribiTbl.GetGekkansiNo(year, mMonth);
                    //    Kansi getuunKansi = person.GetKansi(gekkansiNo);

                    //月運は見ないのでnull
                    Kansi getuunKansi = null;

                    Isouhou isouhou = new Isouhou(person);
                    isouhou.CreateGogyouAttrMatrix(person, getuunKansi, nenunKansi, taiunKansi);

                    if (bDispGogyou || bDispGotoku)
                    {
                        //合法反映
                        //----------------------------
                        if (person.bRefrectSigou || person.bRefrectHankai) //支合、半会 反映指定あり
                        {
                            //支の変換
                            isouhou.RefrectGouhou(
                                            getuunKansi, nenunKansi, taiunKansi,
                                            false
                                            );
                        }
                        if (person.bRefrectKangou)
                        {
                            //干合反映　干の変換
                            isouhou.RefrectKangou(
                                            getuunKansi, nenunKansi, taiunKansi,
                                            false
                                            );
                        }

                        //三合会局・方三位　反映
                        if (person.bRefrectHousani || person.bRefrectSangouKaikyoku)//方三位、三合会局 反映指定あり
                        {
                            //三合会局
                            var lstSangouKaikyoku = person.GetSangouKaikyoku(getuunKansi, nenunKansi, taiunKansi);
                            //方三位
                            var lstHouSani = person.GetHouSani(getuunKansi, nenunKansi, taiunKansi);

                            isouhou.RefrectSangouKaikyokuHousanni(
                                        lstSangouKaikyoku, lstHouSani
                                        );
                        }
                    }
                    AttrTblItem[] attrItems = new AttrTblItem[5];
                    attrItems[0] = isouhou.GetAttrTblItem(Const.enumKansiItemID.NENUN);
                    attrItems[1] = isouhou.GetAttrTblItem(Const.enumKansiItemID.TAIUN);
                    attrItems[2] = isouhou.GetAttrTblItem(Const.enumKansiItemID.NIKKANSI);
                    attrItems[3] = isouhou.GetAttrTblItem(Const.enumKansiItemID.GEKKANSI);
                    attrItems[4] = isouhou.GetAttrTblItem(Const.enumKansiItemID.NENKANSI);

                    //干に土があるか？
                    bool bKanDo = false;
                    bool bKyakkaHoukai = false;
                    foreach (var attrItem in attrItems)
                    {
                        if (attrItem.attrKan == "土")
                        {
                            bKanDo = true;
                            break;
                        }
                    }
                    if (bKanDo)
                    {
                        bKyakkaHoukai = true;
#if DEBUG
                        string dbgStr = "";
                        dbgStr = year.ToString() + " : ";
                        foreach (var attrItem in attrItems)
                        {
                            dbgStr += attrItem.attrSi + ",";
                        }
                        Trace.TraceInformation(dbgStr);

#endif
                        foreach (var attrItem in attrItems)
                        {
                            if (attrItem.attrSi != "水")
                            {
                                bKyakkaHoukai = false;
                                break;
                            }
                        }
                    }
                    if (bKyakkaHoukai)
                    {
                        //発見！
                        FindItem findItem = new FindItem(person);
                        findItem.lstItem.Add(year.ToString());//年

                        result.Add(findItem);

                    }
                    // }
                    nenkansiNo++;
                }
                return result;
            }

            ////日干支
            //Kansi[] aryKansi = new Kansi[] { person.nikkansi, person.gekkansi, person.nenkansi };
            ////大運干支
            //var lstTaiunKansi = person.GetTaiunKansiList();

            //foreach (var item in lstTaiunKansi)
            //{
            //    Kansi taiunKansi = person.GetKansi(item.kansiNo);

            //    for (int i = 0; i < aryKansi.Length; i++)
            //    {
            //        string str = "";
            //        if (mode == 0) str = person.GetNentin(aryKansi[i], taiunKansi);
            //        else str = person.GetRittin(aryKansi[i], taiunKansi);

            //        if (str == findStr[0] || str == findStr[1])
            //        {
            //            //発見！
            //            FindItem findItem = new FindItem(person);
            //            // findItem.lstItem.Add(person.name);
            //            findItem.lstItem.Add(item.year.ToString()); //年
            //            findItem.lstItem.Add("");//月
            //            findItem.lstItem.Add(Const.sTaiun); //"大運"
            //            findItem.lstItem.Add(artNitiGetuNenStr[i]);
            //            findItem.lstItem.Add(str);

            //            result.Add(findItem);
            //        }
            //    }
            //    //年運・月運検索
            //    result.Add(FindNattinOrRittinNenuni(person, item.year, findStr, aryKansi, mode));

            //}
            return result;
        }


    }
}