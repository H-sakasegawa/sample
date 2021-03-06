using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WinFormsApp2
{
    public class TableMng
    {
        static TableMng tblMng = new TableMng();
        public static TableMng GetTblManage()
        {
            return tblMng;
        }

        public SetuiribiTable setuiribiTbl = new SetuiribiTable();

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
            public Jyukan this[int index]
            {
                get
                {
                    if (index >= dicJyukan.Count) return null;
                    return dicJyukan.Values.ToArray()[index];
                }
            }
            public int Count { get { return dicJyukan.Count; } }
            public List<Jyukan> ToList()
            {
                return dicJyukan.Values.ToList();
            }
            public string GetGogyo(string jukan)
            {
                return this[jukan].gogyou;
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

            /// <summary>
            /// 指定した十干と同じ五行のもう一つの十干文字（陰陽の関係にある）を取得
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            public string GetInyouOtherString( string s )
            {
                var findGogyou = dicJyukan[s].gogyou;

                var item = dicJyukan.ToList().Find(x => (x.Value.gogyou == findGogyou && x.Value.name != s));
                if (item.Equals(default(KeyValuePair<string, Jyukan>))) return null;

                return item.Value.name;

            }
            /// <summary>
            /// 自身を生じる干で陰陽違いのものを取得
            /// </summary>
            /// <param name="kan"></param>
            /// <returns></returns>
            public string GetOccursKangouStrAndOtherInyou(string kan)
            {
                if (string.IsNullOrEmpty(kan)) return null;

                var jukan = dicJyukan[kan];

                var item = dicJyukan.Values.ToList().Find(x => x.gogyou == jukan.gogyou && x.inyou != jukan.inyou);
                if (item == null) return null;

                return item.sFromName;

            }
            /// <summary>
            /// 自身が生させる干で陰陽違いのものを取得
            /// </summary>
            /// <param name="kan"></param>
            /// <returns></returns>
            public string GetCauseKangouStrAndOtherInyou(string kan)
            {
                var jukan = dicJyukan[kan];

                var item = dicJyukan.Values.ToList().Find(x => x.gogyou == jukan.gogyou && x.inyou != jukan.inyou);
                if (item == null) return null;

                return item.sToName;

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
            public List<Jyunisi> ToList(bool bMinimul=false)
            {
                return dicJyunisi.Values.ToList();
            }
            public string GetGogyo(string junisi)
            {
                return this[junisi].gogyou;
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

            public Kansi[] GetKansiList() { return dicKansi.Values.ToArray(); }
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

            public int GetMinNoDistance( Kansi kansi1, Kansi kansi2)
            {
                if (kansi1.no == kansi2.no) return 0;

                int dist1 =Math.Abs( kansi1.no - kansi2.no );
                int n1 = 0;
                int n2 = 0;
                if (kansi1.no < kansi2.no)
                {
                    n1 = kansi1.no + dicKansi.Count;
                    n2 = kansi2.no;
                }
                else
                {
                    n1 = kansi2.no + dicKansi.Count;
                    n2 = kansi1.no;
                }

                int dist2 = n1 - n2;

                return Math.Min(dist1, dist2);

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

            public string GetGogyo(string name)
            {
                var item = lstJudaiShusei.FirstOrDefault(x => x.name == name);
                if (item != null) return item.gogyou;
                return null;
            }
            public string GetInyou(string name)
            {
                var item = lstJudaiShusei.FirstOrDefault(x => x.name == name);
                if (item != null) return item.inyou;
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

            /// <summary>
            /// 干合情報取得
            /// </summary>
            /// <param name="kan1"></param>
            /// <param name="kan2"></param>
            /// <returns></returns>
            public Kangou GetKangou(string kan1, string kan2)
            {
                foreach (var val in lstKangou)
                {
                    if (val.sKangou1 == kan1 && val.sKangou2 == kan2 ||
                        val.sKangou1 == kan2 && val.sKangou2 == kan1)
                    {
                        return val;
                    }
                }
                return null;
            }
            /// <summary>
            /// 虚気文字列取得
            /// </summary>
            /// <param name="kan1"></param>
            /// <param name="kan2"></param>
            /// <returns>
            /// string[0]... kan1に該当する虚気文字
            /// string[1]... kan2に該当する虚気文字
            /// </returns>
            public string[] GetKyoki(string kan1, string kan2)
            {
                foreach (var val in lstKangou)
                {
                    if (val.sKangou1 == kan1 && val.sKangou2 == kan2)
                    {
                        return new string[] { val.kyoki[0], val.kyoki[1] };
                    }
                    else if(val.sKangou1 == kan2 && val.sKangou2 == kan1)
                    {
                        return new string[] { val.kyoki[1], val.kyoki[0] };
                    }
                }
                return null;
            }
            /// <summary>
            /// 干合かどうかを判定
            /// </summary>
            /// <param name="kan1"></param>
            /// <param name="kan2"></param>
            /// <returns></returns>
            public bool IsKangou(string kan1, string kan2)
            {
                return GetKangou(kan1, kan2) != null ? true : false;

            }
            /// <summary>
            /// 干合文字列取得
            /// </summary>
            /// <param name="kan1"></param>
            /// <param name="kan2"></param>
            /// <returns></returns>
            public string GetKangouStr(string kan1, string kan2)
            {
                // return IsKangou(kan1, kan2) ? "干合" : "";
                return IsKangou(kan1, kan2) ? Const.sKangou : "";

            }
            /// <summary>
            /// 干合文字の相手文字を取得
            /// </summary>
            /// <param name="kan"></param>
            /// <returns></returns>
            public string GetKangouOtherStr(string kan)
            {
                var findKangou = lstKangou.Find(x => x.sKangou1 == kan || x.sKangou2 == kan);
                if (findKangou == null) return null;

                if (findKangou.sKangou1 == kan) return findKangou.sKangou2;
                else  return findKangou.sKangou1;

            }

            /// <summary>
            /// 干合 五行属性取得
            /// </summary>
            /// <param name="siName1"></param>
            /// <param name="siName2"></param>
            /// <returns></returns>
            public string GetKangouAttr(string siName1, string siName2)
            {
                var kangou = GetKangou(siName1, siName2);
                if (kangou != null)
                {
                    return kangou.gogyou;
                }
                return null;
            }
        }
        /// <summary>
        /// 干合テーブル
        /// </summary>
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
                    if (val.name1 == kan1 && val.name2 == kan2 ||
                        val.name1 == kan2 && val.name2 == kan1)
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
        /// <summary>
        /// 七殺テーブル
        /// </summary>
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
            /// 天殺地冲または、納音がある場合は、"冲動"は除外されます。
            /// </summary>
            /// <param name="siName1">支名称1</param>
            /// <param name="siName2">支名称2</param>
            /// <param name="bExistTensatuTichu">true...天殺地冲あり</param>
            /// <param name="bExistNentin">true...納音あり</param>
            /// <returns></returns>
            public string[] GetGouhouSanpouEx(string siName1, string siName2, bool bExistTensatuTichu, bool bExistNentin)
            {

                var aryStr= GetGouhouSanpou(siName1, siName2);
                if (aryStr == null) return null;

                var items = aryStr.ToList();

                for(int i=items.Count-1; i>=0; i--)
                {
                    if ((bExistTensatuTichu || bExistNentin) && items[i] == Const.sOkidou) //"冲動"
                    {   //天殺地冲または、納音がある場合は、"冲動"は不要
                        items.RemoveAt(i);
                    }

                }
                return items.ToArray();

            }
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
                if (idx >= jyunisi.Length)
                    return null;

                if (!dicGouhouSanpou.ContainsKey(siName2))
                    return null;

                string value = dicGouhouSanpou[siName2][idx];

                if (value == "")
                    return null;
                return value.Split(",");

            }
            ///// <summary>
            ///// 干支の支名称の組み合わせから合法・散法テーブルに登録されている文字列をカンマ区切りで取得
            ///// </summary>
            ///// <param name="siName1">支名称1</param>
            ///// <param name="siName2">支名称2</param>
            ///// <returns></returns>
            //public string GetGouhouSanpouString(string siName1, string siName2, bool bExistTensatuTichu, bool bExistNentin)
            //{
            //    var values = GetGouhouSanpou(siName1, siName2, bExistTensatuTichu, bExistNentin);
            //    string s = "";
            //    if (values != null)
            //    {
            //        foreach (var item in values)
            //        {
            //            if (s != "") s += ",";
            //            s += item;
            //        }
            //    }
            //    return s;
            //}

        }
        public GouhouSanpouTbl gouhouSanpouTbl = null;

        class SiItems
        {
            public SiItems(string name, int bit)
            {
                si = name;
                bitFlg = bit;
            }
            public string si;
            public int bitFlg;
        }

        /// <summary>
        /// 三合会局 テーブル 管理
        /// </summary>
        public class SangouKaikyokuTbl
        {
            public List<SangouKaikyoku> lstSangouKaikyoku = null;

            public List<SangouKaikyokuResult> GetSangouKaikyoku(Kansi getuun, Kansi nenun, Kansi taiun,          
                                                                Kansi nikkansi, Kansi gekkansi, Kansi nenkansi)
            {
                List<SiItems> arySi = new List<SiItems>();
                if (getuun != null) arySi.Add(new SiItems(getuun.si, Const.bitFlgGetuun));
                arySi.Add(new SiItems(nenun.si, Const.bitFlgNenun));
                arySi.Add(new SiItems(taiun.si, Const.bitFlgTaiun));
                arySi.Add(new SiItems(nikkansi.si, Const.bitFlgNiti));
                arySi.Add(new SiItems(gekkansi.si, Const.bitFlgGetu));
                arySi.Add(new SiItems(nenkansi.si, Const.bitFlgNen));

                List<SangouKaikyokuResult> lstResult = new List<SangouKaikyokuResult>() ;
                for (int i = 0; i < arySi.Count - 2; i++)
                {
                    for (int j = i + 1; j < arySi.Count - 1; j++)
                    {
                        for (int k = j + 1; k < arySi.Count; k++)
                        {
                            if( arySi[i].si == arySi[j].si ||
                                arySi[i].si == arySi[k].si ||
                                arySi[j].si == arySi[k].si)
                            {
                                //同じ支が1組でもあれば不成立
                                continue;
                            }
                            foreach (var item in lstSangouKaikyoku)
                            {
                                if (item.IsExist(arySi[i].si) &&
                                    item.IsExist(arySi[j].si) &&
                                    item.IsExist(arySi[k].si) )
                                {
                                    SangouKaikyokuResult result = new SangouKaikyokuResult();
                                    result.sangouKaikyoku =  item;
                                    result.hitItemBit = arySi[i].bitFlg | arySi[j].bitFlg | arySi[k].bitFlg;

                                    lstResult.Add(result);
                                }
                            }
                        }
                    }
                }
                return lstResult;
            }

        }
        /// <summary>
        /// 三合会局 情報取得結果
        /// </summary>
        public class SangouKaikyokuResult
        {
            /// <summary>
            /// 三合会局データ
            /// </summary>
            public SangouKaikyoku sangouKaikyoku;
            /// <summary>
            /// 三合会局に合致した干支を指すビット情報
            /// </summary>
            public int hitItemBit;
        }
        public SangouKaikyokuTbl sangouKaikyokuTbl = new SangouKaikyokuTbl();

        /// <summary>
        /// 方三位 テーブル　管理
        /// </summary>
        public class HouSaniTbl
        {
            public List<HouSani> lstHousani = null;
            public List<HouSaniResult> GetHouSani(Kansi getuun, Kansi nenun, Kansi taiun,
                                      Kansi nikkansi, Kansi gekkansi, Kansi nenkansi)
            {

                List<SiItems> arySi = new List<SiItems>();
                if (getuun != null) arySi.Add(new SiItems(getuun.si, Const.bitFlgGetuun));
                arySi.Add(new SiItems(nenun.si, Const.bitFlgNenun));
                arySi.Add(new SiItems(taiun.si, Const.bitFlgTaiun));
                arySi.Add(new SiItems(nikkansi.si, Const.bitFlgNiti));
                arySi.Add(new SiItems(gekkansi.si, Const.bitFlgGetu));
                arySi.Add(new SiItems(nenkansi.si, Const.bitFlgNen));

                List<HouSaniResult> lstResult = new List<HouSaniResult>();
                for (int i = 0; i < arySi.Count - 2; i++)
                {
                    for (int j = i + 1; j < arySi.Count - 1; j++)
                    {
                        for (int k = j + 1; k < arySi.Count; k++)
                        {
                            if (arySi[i].si == arySi[j].si ||
                                arySi[i].si == arySi[k].si ||
                                arySi[j].si == arySi[k].si)
                            {
                                //同じ支が1組でもあれば不成立
                                continue;
                            }
                            foreach (var item in lstHousani)
                            {
                                if (item.IsExist(arySi[i].si) &&
                                    item.IsExist(arySi[j].si) &&
                                    item.IsExist(arySi[k].si))
                                {
                                    HouSaniResult result = new HouSaniResult();
                                    result.houSani = item;
                                    result.hitItemBit = arySi[i].bitFlg | arySi[j].bitFlg | arySi[k].bitFlg;

                                    lstResult.Add(result);
                                }
                            }
                        }
                    }
                }
                return lstResult;
            }
        }
        /// <summary>
        /// 方三位 情報取得結果
        /// </summary>
        public class HouSaniResult
        {
            /// <summary>
            /// 方三位データ
            /// </summary>
            public HouSani houSani;
            /// <summary>
            /// 方三位に合致した干支を指すビット情報
            /// </summary>
            public int hitItemBit;
        }

        public HouSaniTbl housanniTbl = new HouSaniTbl();

        /// <summary>
        /// 支合 テーブル
        /// </summary>
        public class SigouTbl
        {
            public List<Sigou> lstSigou = null;

            public Sigou GetSigou(string siName1, string siName2)
            {
                foreach (var item in lstSigou)
                {
                    if (item.IsMatch(siName1, siName2))
                    {
                        return item;
                    }
                }
                return null;

            }
            public bool IsSgou(string siName1, string siName2)
            {
                return GetSigou( siName1,  siName2) != null ? true : false;
            }

            /// <summary>
            /// 支合テーブルの五行（色）を取得
            /// </summary>
            /// <param name="siName1"></param>
            /// <param name="siName2"></param>
            /// <param name="bManyAttrDo">true..."土"が多い</param>
            /// <returns></returns>
            public string GetSigouAttr(string siName1, string siName2, bool bManyAttrDo)
            {
                var sigou = GetSigou( siName1,  siName2);
                if( sigou!=null)
                {
                    //"木"または"金"の場合
                    if ( sigou.gogyou==Const.sGogyouMoku || sigou.gogyou==Const.sGogyouKin)
                    {
                        if(bManyAttrDo)
                        {   //"土"が多いので"土に変換
                            return sigou.goryouSub;
                        }
                    }
                    return sigou.gogyou;
                }  
                return null;
            }

        }
        public SigouTbl sigouTbl = new SigouTbl();

        /// <summary>
        /// 五行、五徳　属性カラーテーブル
        /// </summary>
        public class AttrColorTbl
        {
            public Dictionary<string , Color> dicAttrColor;

            public Color this[string attrName]
            {
                get
                {
                    if(!dicAttrColor.ContainsKey(attrName))
                    {
                        return default(Color);
                    }
                    return dicAttrColor[attrName];
                }
            }
        }
        public AttrColorTbl gogyouAttrColorTbl = new AttrColorTbl();
        public AttrColorTbl gotokuAttrColorTbl = new AttrColorTbl();


        /// <summary>
        /// 半会 テーブル
        /// </summary>
        public class HankaiTbl
        {
            public List<Hankai> lstHankai = null;

            public Hankai GetHankai(string siName1, string siName2)
            {
                foreach (var item in lstHankai)
                {
                    if (item.IsMatch(siName1, siName2))
                    {
                        return item;
                    }
                }
                return null;

            }
            //半会テーブルの五行（色）を取得
            public string GetGogyou(string siName1, string siName2)
            {
                var sigou = GetHankai(siName1, siName2);
                if (sigou != null)
                {
                    return  sigou.gogyou;
                }
                return null;
            }

        }
        public HankaiTbl hankaiTbl = new HankaiTbl();

        /// <summary>
        /// 干支　五行情報管理テーブル
        /// </summary>
        public class KansiAttrTblMng
        {
            AttrTblItem[] attrTbl = null;

            public KansiAttrTblMng()
            {
                attrTbl = new AttrTblItem[6]; //月運、年運、大運, 日干支、月干支、年干支
                for (int i = 0; i < attrTbl.Length; i++)
                {
                    attrTbl[i] = new AttrTblItem();
                }
            }

            public AttrTblItem this[int id]
            {
                get
                {
                    return attrTbl[id];
                }
            }
            public bool IsSame(KansiAttrTblMng kansiAttrTbl)
            {
                for (int i = 0; i < attrTbl.Length; i++)
                {
                    if (attrTbl[i] != kansiAttrTbl.attrTbl[i])
                        return false;
                }
                return true;
            }
            /// <summary>
            /// 基本属性マトリクス情報を作成
            /// </summary>
            /// <param name="person"></param>
            /// <param name="getuun"></param>
            /// <param name="nenun"></param>
            /// <param name="taiun"></param>
            public void CreateGogyouAttrMatrix(Person person, Kansi getuun = null, Kansi nenun = null, Kansi taiun = null)
            {
                var tblMng = TableMng.GetTblManage();
                //月運
                if (getuun != null) attrTbl[(int)Const.enumKansiItemID.GETUUN].Init(tblMng.jyukanTbl[getuun.kan].gogyou, tblMng.jyunisiTbl[getuun.si].gogyou);
                //年運
                if (nenun != null) attrTbl[(int)Const.enumKansiItemID.NENUN].Init(tblMng.jyukanTbl[nenun.kan].gogyou, tblMng.jyunisiTbl[nenun.si].gogyou);
                //大運
                if (taiun != null) attrTbl[(int)Const.enumKansiItemID.TAIUN].Init(tblMng.jyukanTbl[taiun.kan].gogyou, tblMng.jyunisiTbl[taiun.si].gogyou);

                //日干支
                attrTbl[(int)Const.enumKansiItemID.NIKKANSI].Init(tblMng.jyukanTbl[person.nikkansi.kan].gogyou, tblMng.jyunisiTbl[person.nikkansi.si].gogyou);
                //月干支
                attrTbl[(int)Const.enumKansiItemID.GEKKANSI].Init(tblMng.jyukanTbl[person.gekkansi.kan].gogyou, tblMng.jyunisiTbl[person.gekkansi.si].gogyou);
                //年干支
                attrTbl[(int)Const.enumKansiItemID.NENKANSI].Init(tblMng.jyukanTbl[person.nenkansi.kan].gogyou, tblMng.jyunisiTbl[person.nenkansi.si].gogyou);
            }
            public KansiAttrTblMng Clone()
            {
                // Object型で返ってくるのでキャストが必要
                return (KansiAttrTblMng)MemberwiseClone();
            }

        }

        /// <summary>
        ///　五徳 管理テーブル 管理
        /// </summary>
        public class GotokuTbl
        {
            /// <summary>
            /// 主キー
            /// </summary>
            public string[] attrName;
            public Dictionary<string, string[]> dicGotoku;

            public string GetGotokuByKansi( string kan, string si)
            {
                var attr1 = tblMng.jyukanTbl[kan].gogyou;
                var attr2 = tblMng.jyunisiTbl[si].gogyou;

                return GetGotoku( attr1,  attr2);
            }

            public string GetGotoku(string attr1, string attr2)
            {
                if (!dicGotoku.ContainsKey(attr2)) return null;

                //主キーのインデックス番号取得
                for (int idxItem = 0; idxItem < attrName.Length; idxItem++)
                {
                    if (attrName[idxItem] == attr2)
                    {
                       var gotokuNames = dicGotoku[attr1];
                        return gotokuNames[idxItem];
                    }

                }
                return null;
            }

        }
        public GotokuTbl gotokuTbl = null;


        /// <summary>
        /// 守護神テーブル
        /// </summary>
        public class ShugoSinTbl
        {
            /// <summary>
            /// Key : 干支文字の組み合わせ 例："甲寅"
            /// </summary>
            public Dictionary<string, ShugoSin[]> dicShugoSin;


            public ShugoSin[] GetSugoSinItem(Kansi nikkansi, Kansi gekkansi)
            {
                string sKey = string.Format("{0}{1}", nikkansi.kan, gekkansi.si);

                if (!dicShugoSin.ContainsKey(sKey)) return null;

                return dicShugoSin[sKey];

            }



        }
        public ShugoSinTbl shugosinTbl = null;


        /// <summary>
        /// 五行属性どうしの関係（相生、相剋）テーブル
        /// </summary>
        public class GogyouAttrRerationshipTbl
        {
            public Dictionary<string, GogyouAttrRelationship> dicGogyouAttrRelationship = new Dictionary<string, GogyouAttrRelationship>();

            public GogyouAttrRelationship GetRelation(string name)
            {
                if (!dicGogyouAttrRelationship.ContainsKey(name)) return null;
                return dicGogyouAttrRelationship[name];
            }

            public string[] GetCreatedRelation(string start)
            {
                List<string> lstAttrs = new List<string>();
                lstAttrs.Add(start);
                string createFrom = start;
                do
                {
                    createFrom = dicGogyouAttrRelationship[createFrom].createFromName;
                    if (start == createFrom) break;
                    lstAttrs.Add(createFrom);
                } while (true);


                return lstAttrs.ToArray();
            }
            /// <summary>
            /// 相生 関係チェック
            /// </summary>
            /// <param name="from">生み出すもの属性</param>
            /// <param name="to">生み出されるもの属性</param>
            /// <returns></returns>
            public bool IsCreate(string fromAttr, string toAttr)
            {
                var relation = GetRelation(fromAttr);
                if (relation.createToName == toAttr) return true;
                return false;
            }
            /// <summary>
            /// 相生 関係チェック
            /// </summary>
            /// <param name="from">生み出す干</param>
            /// <param name="to">生み出される干</param>
            /// <returns></returns>
            public bool IsCreateByKan(string fromKan, string toKan)
            {
                TableMng tblMng = TableMng.GetTblManage();

                string fromAttr = tblMng.jyukanTbl.GetGogyo(fromKan);
                string toAttr = tblMng.jyukanTbl.GetGogyo(toKan);

               
                return IsCreate( fromAttr,  toAttr);
            }
            /// <summary>
            /// 相剋生 関係チェック
            /// </summary>
            /// <param name="fromAttr">剋するもの属性</param>
            /// <param name="toAttr">剋されるもの属性</param>
            /// <returns></returns>
            public bool IsDestory(string fromAttr, string toAttr)
            {
                var relation = GetRelation(fromAttr);
                if (relation.destoryToName == toAttr) return true;
                return false;
            }
            /// <summary>
            /// 相剋生 関係チェック
            /// </summary>
            /// <param name="fromKan">剋する干</param>
            /// <param name="toKan">剋される干</param>
            /// <returns></returns>
            public bool IsDestoryByKan(string fromKan, string toKan)
            {
                TableMng tblMng = TableMng.GetTblManage();

                string fromAttr = tblMng.jyukanTbl.GetGogyo(fromKan);
                string toAttr = tblMng.jyukanTbl.GetGogyo(toKan);

                return IsDestory(fromAttr, toAttr);
            }
            /// <summary>
            /// 引数で渡された属性を生み出すものを返す
            /// </summary>
            /// <param name="to">生み出される属性</param>
            /// <returns></returns>
            public string GetCreatFrom( string to)
            {
                var relation = GetRelation(to);
                return relation.createFromName;
            }
            /// <summary>
            /// 属性AからBまでの相生関連距離
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public int GetCreateDistanceFromAToB(string a, string b)
            {
                int d = 0;
                while(true)
                {
                    var relation = GetRelation(a);
                    a = relation.createToName;
                    if (a == b) break;
                    d++;
                }
                return d;
            }
        }

        public GogyouAttrRerationshipTbl gogyouAttrRelationshipTbl;

        //======================================================================
        // テーブルマネージャ初期化
        //======================================================================
        public TableMng()
        {
            //--------------------------------
            //十干
            //--------------------------------
            jyukanTbl.dicJyukan = new Dictionary<string, Jyukan>
            {
                {"甲", new Jyukan("甲","木","+", "壬","丙") },
                {"乙", new Jyukan("乙","木","-", "癸","丁") },
                {"丙", new Jyukan("丙","火","+", "甲","戊") },
                {"丁", new Jyukan("丁","火","-", "乙","己") },
                {"戊", new Jyukan("戊","土","+", "丙","庚") },
                {"己", new Jyukan("己","土","-", "丁","辛") },
                {"庚", new Jyukan("庚","金","+", "戊","壬") },
                {"辛", new Jyukan("辛","金","-", "己","癸") },
                {"壬", new Jyukan("壬","水","+", "庚","甲") },
                {"癸", new Jyukan("癸","水","-", "辛","乙") },
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
                new Kangou("甲","己","土","戊","己","甲己火土"),
                new Kangou("乙","庚","金","辛","庚","乙庚化金"),
                new Kangou("丙","辛","水","壬","癸","丙辛化水"),
                new Kangou("壬","丁","木","甲","乙","壬丁化木"),
                new Kangou("戊","癸","火","丙","丁","戊癸化火"),

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
                { "辰", new string[]{"半会"  ,"破"         ,""           ,"害"    ,"自刑",""              ,""    ,""           ,"半会"          ,"支合", " 冲動"    ,""         }},
                { "巳", new string[]{""      ,"半会"       ,"生貴刑,害"  ,""      ,""    ,""              ,""    ,""           ,"半会,破,生貴刑", "半会", ""         ,"冲動"    }},
                { "午", new string[]{"冲動"  ,"害"         ,"半会"       ,"破"    ,""    ,""              ,"自刑","支合"       ,""              ,""    , "半会"     ,""         }},
                { "未", new string[]{"害"    ,"冲動,庫気刑",""           ,"半会"  ,""    ,""              ,"支合",""           ,""              ,""    , "庫気刑,破","半会"    }},
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
                new SangouKaikyoku(new string[]{"申","子","辰"},"水"),
                new SangouKaikyoku(new string[]{"亥","卯","未"},"木"),
                new SangouKaikyoku(new string[]{"寅","午","戌"},"火"),
                new SangouKaikyoku(new string[]{"巳","酉","丑"},"金"),
            };
            //--------------------------------
            //方三位 テーブル
            //--------------------------------
            housanniTbl.lstHousani = new List<HouSani>()
            {
                new HouSani(new string[]{"亥","子","丑"},"水"),
                new HouSani(new string[]{"寅","卯","辰"},"木"),
                new HouSani(new string[]{"巳","午","未"},"火"),
                new HouSani(new string[]{"申","酉","戌"},"金"),

            };
            //--------------------------------
            //支合 テーブル
            //--------------------------------
            sigouTbl.lstSigou = new List<Sigou>()
            {
                new Sigou(new string[]{"子","丑"},"水", null),
                new Sigou(new string[]{"亥","寅"},"木", null),
                new Sigou(new string[]{"戌","卯"},"木","土"),
                new Sigou(new string[]{"酉","辰"},"金","土"),
                new Sigou(new string[]{"申","巳"},"金", null),
                new Sigou(new string[]{"未","午"},"火", null),

            };

            //--------------------------------
            //半会　属性テーブル
            //--------------------------------
            hankaiTbl.lstHankai = new List<Hankai>()
            {
                new Hankai(new string[]{"申","子" },"水"),
                new Hankai(new string[]{"申","辰" },"水"),
                new Hankai(new string[]{"子","辰" },"水"),
                new Hankai(new string[]{"亥","卯" },"木"),
                new Hankai(new string[]{"亥","未" },"木"),
                new Hankai(new string[]{"卯","未" },"木"),
                new Hankai(new string[]{"寅","午" },"火"),
                new Hankai(new string[]{"寅","戌" },"火"),
                new Hankai(new string[]{"午","戌" },"火"),
                new Hankai(new string[]{"巳","酉" },"金"),
                new Hankai(new string[]{"巳","丑" },"金"),
                new Hankai(new string[]{"酉","丑" },"金"),
            };



            //--------------------------------
            //五行属性カラー テーブル
            //--------------------------------
            gogyouAttrColorTbl.dicAttrColor = new Dictionary<string, Color>()
            {
                {"木", Color.LightGreen},
                {"火", Color.LightPink},
                {"土", Color.Orange},
                {"金", Color.LightYellow},
                {"水", Color.Gray},
            };
            //--------------------------------
            //五徳関連 テーブル
            //--------------------------------
            gotokuTbl = new GotokuTbl();
            gotokuTbl.attrName = new string[] { "水", "木", "火", "土", "金" };
            gotokuTbl.dicGotoku = new Dictionary<string, string[]>
            {
                { "水", new string[]{"福","寿","財","官","印" } },
                { "木", new string[]{"印","福","寿","財","官" } },
                { "火", new string[]{"官","印","福","寿","財" } },
                { "土", new string[]{"財","官","印","福","寿" } },
                { "金", new string[]{"寿","財","官","印","福" } },
            };
            //--------------------------------
            //五徳属性カラー テーブル
            //--------------------------------
            gotokuAttrColorTbl.dicAttrColor = new Dictionary<string, Color>()
            {
                {"福", Color.LightGreen},
                {"寿", Color.LightPink},
                {"財", Color.Orange},
                {"官", Color.LightYellow},
                {"印", Color.Gray},
            };

            //--------------------------------
            //守護神 テーブル
            //--------------------------------
            shugosinTbl = new ShugoSinTbl();
            shugosinTbl.dicShugoSin = new Dictionary<string, ShugoSin[]>
            {
                //甲-*
                { "甲寅", new ShugoSin[]{ new ShugoSin( new string[]{ "丙", "癸" },             EnmSugosinCond.None,         "金性・木性") } },
                { "甲卯", new ShugoSin[]{ new ShugoSin( new string[]{ "庚", "己", "丁" },       EnmSugosinCond.None,         "木性") } },
                { "甲辰", new ShugoSin[]{ new ShugoSin( new string[]{ "庚", "壬", "丁" },       EnmSugosinCond.None,         "木性・土性"),
                                          new ShugoSin( new string[]{ "庚", "壬", "丁" },       EnmSugosinCond.Do_2num,      "土性・木性") } },
                { "甲巳", new ShugoSin[]{ new ShugoSin( new string[]{ "癸", "丁", "庚" },       EnmSugosinCond.None,         "土性") } },
                { "甲午", new ShugoSin[]{ new ShugoSin( new string[]{ "癸", "庚", "丁" },       EnmSugosinCond.Natu_Mae,     "土性"),
                                          new ShugoSin( new string[]{ "癸", "丁", "庚" },       EnmSugosinCond.Natu_Ato,     "土性") } },
                { "甲未", new ShugoSin[]{ new ShugoSin( new string[]{ "庚", "丁", "癸" },       EnmSugosinCond.None,         "土性"),
                                          new ShugoSin( new string[]{ "庚", "癸", "丁" },       EnmSugosinCond.Hi_2num,      "土性") } },
                { "甲申", new ShugoSin[]{ new ShugoSin( new string[]{ "丁", "庚" },             EnmSugosinCond.None,         "水性",   "特に癸水が忌神") } },
                { "甲酉", new ShugoSin[]{ new ShugoSin( new string[]{ "丁", "庚", "丙" },       EnmSugosinCond.None,         "水性",   "特に癸水が忌神") } },
                { "甲戌", new ShugoSin[]{ new ShugoSin( new string[]{ "丁", "庚", "壬" },       EnmSugosinCond.None,         "土性)"),
                                          new ShugoSin( new string[]{ "丁", "庚", "壬" },       EnmSugosinCond.Ki_Toukan,    "土性・癸水"),
                                          new ShugoSin( new string[]{ "丁", "庚", "壬" },       EnmSugosinCond.Moku_2numOver,"土性・木性") } },
                { "甲亥", new ShugoSin[]{ new ShugoSin( new string[]{ "庚", "丁", "戊" },       EnmSugosinCond.None,         "水性",    "特に癸水が忌神") } },
                { "甲子", new ShugoSin[]{ new ShugoSin( new string[]{ "丁", "庚", "丙", "戊" }, EnmSugosinCond.None,         "水性",    "特に癸水が忌神") } },
                { "甲丑", new ShugoSin[]{ new ShugoSin( new string[]{ "丁", "庚", "丙" },       EnmSugosinCond.None,         "水性",    "特に癸水が忌神") } }, //<==★★Excelでは"木性(癸)" PDFは"水性"

                 //乙-*
                { "乙寅", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","癸" },               EnmSugosinCond.None,                "金性") } },
                { "乙卯", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","癸" },               EnmSugosinCond.None,                "金性") } },
                { "乙辰", new ShugoSin[]{ new ShugoSin( new string[]{ "癸","丙","甲" },          EnmSugosinCond.None,                "金性・土性") } },
                { "乙巳", new ShugoSin[]{ new ShugoSin( new string[]{ "癸","丙","辛" },          EnmSugosinCond.None,                "土性") } },
                { "乙午", new ShugoSin[]{ new ShugoSin( new string[]{ "癸","丙","辛" },          EnmSugosinCond.None,                "土性") } },
                { "乙未", new ShugoSin[]{ new ShugoSin( new string[]{ "癸","丙","甲" },          EnmSugosinCond.None,                "土性") } },
                { "乙申", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","癸","己" },          EnmSugosinCond.None,                "金性") } },
                { "乙酉", new ShugoSin[]{ new ShugoSin( new string[]{ "癸","丙" },               EnmSugosinCond.Aki_Mae,             "金性"),  
                                          new ShugoSin( new string[]{ "丙","癸" },               EnmSugosinCond.Aki_Ato,             "金性") } },
                { "乙戌", new ShugoSin[]{ new ShugoSin( new string[]{ "癸", "丙", "甲" },        EnmSugosinCond.None,                "土性" ),
                                          new ShugoSin( new string[]{ "癸", "丙", "甲", "辛" },  EnmSugosinCond.Ki_Arii,             "土性" ) } },        //宿命に癸水がある場合
                { "乙亥", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","戊" },               EnmSugosinCond.None,                "水性・金性") } },
                { "乙子", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","戊" },               EnmSugosinCond.None,                "水性・金性"),
                                          new ShugoSin( new string[]{ "丙","戊","甲" },          EnmSugosinCond.Do_Nasi,             "水性・金性"),       //宿命に土性がない場合
                                          new ShugoSin( new string[]{ "丙","戊","甲" },          EnmSugosinCond.Hei_Nasi_and_Hi_Ari, "水性・金性") } },   //宿命に丙がなくかつ丁がある場合
                { "乙丑", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","甲" },               EnmSugosinCond.None,                "水性・金性") } },
 
                 //丙-*
                { "丙寅", new ShugoSin[]{ new ShugoSin( new string[]{ "壬","庚" },          EnmSugosinCond.None,      "土性",        "特に戊土が忌神") } },
                { "丙卯", new ShugoSin[]{ new ShugoSin( new string[]{ "壬","庚" },          EnmSugosinCond.None,      "土性",        "特に戊土が忌神") } },
                { "丙辰", new ShugoSin[]{ new ShugoSin( new string[]{ "壬","甲","庚" },     EnmSugosinCond.None,      "土性") } },
                { "丙巳", new ShugoSin[]{ new ShugoSin( new string[]{ "壬","庚" },          EnmSugosinCond.None,      "土性・火",    "木性は忌神の火性を強める") } },
                { "丙午", new ShugoSin[]{ new ShugoSin( new string[]{ "壬","庚" },          EnmSugosinCond.None,      "土性・火",    "木性は忌神の火性を強める") } },
                { "丙未", new ShugoSin[]{ new ShugoSin( new string[]{ "壬","庚" },          EnmSugosinCond.None,      "土性",        "特に戌土が忌神") } },
                { "丙申", new ShugoSin[]{ new ShugoSin( new string[]{ "壬","甲" },          EnmSugosinCond.None,      "土性",        "金性が強まると忌神になる") } },
                { "丙酉", new ShugoSin[]{ new ShugoSin( new string[]{ "壬","甲" },          EnmSugosinCond.None,      "土性・金性") } },
                { "丙戌", new ShugoSin[]{ new ShugoSin( new string[]{ "甲","壬" },          EnmSugosinCond.None,      "土性・金性") } },
                { "丙亥", new ShugoSin[]{ new ShugoSin( new string[]{ "甲","壬" },          EnmSugosinCond.Mizu_1num, "金"),
                                          new ShugoSin( new string[]{ "甲","壬","(戊)" },   EnmSugosinCond.None,      "金")} },
                { "丙子", new ShugoSin[]{ new ShugoSin( new string[]{ "甲","壬","戊" },     EnmSugosinCond.Fuyu_Mae,  "金性"),      
                                          new ShugoSin( new string[]{ "壬","戊","甲" },     EnmSugosinCond.Fuyu_Ato,  "金性") } },
                { "丙丑", new ShugoSin[]{ new ShugoSin( new string[]{ "壬","甲" },          EnmSugosinCond.None,      "土性・金性") } },

                 //丁-*
                { "丁寅", new ShugoSin[]{ new ShugoSin( new string[]{ "庚","甲" },          EnmSugosinCond.None,        "水性",              "特に癸水が忌神") } },
                { "丁卯", new ShugoSin[]{ new ShugoSin( new string[]{ "庚","甲" },          EnmSugosinCond.None,        "水性",              "特に癸水が忌神") } },
                { "丁辰", new ShugoSin[]{ new ShugoSin( new string[]{ "甲","庚" },          EnmSugosinCond.None,        "癸水・土性") } },
                { "丁巳", new ShugoSin[]{ new ShugoSin( new string[]{ "甲","庚","壬" },     EnmSugosinCond.Hi_2num,     "土性・火性"), 
                                          new ShugoSin( new string[]{ "壬","庚","甲" },     EnmSugosinCond.Hi_3numOver, "火性,土性",         "丙火,戊土を特に嫌う") } },
                { "丁午", new ShugoSin[]{ new ShugoSin( new string[]{ "壬","庚","甲" },     EnmSugosinCond.None,        "火性・土性",        "丙火,戊土を特に嫌う") } },
                { "丁未", new ShugoSin[]{ new ShugoSin( new string[]{ "甲","庚","壬" },     EnmSugosinCond.None,        "火性・土性",        "特に丙火が忌神") } },
                { "丁申", new ShugoSin[]{ new ShugoSin( new string[]{ "甲","庚","丙","乙" },EnmSugosinCond.None,        "水性",              "特に癸水が忌神") } },
                { "丁酉", new ShugoSin[]{ new ShugoSin( new string[]{ "甲","庚","丙" },     EnmSugosinCond.None,        "水性",              "特に癸水が忌神") } },
                { "丁戌", new ShugoSin[]{ new ShugoSin( new string[]{ "甲","庚" },          EnmSugosinCond.None,        "水性・土性",        "特に癸水が忌神") } },
                { "丁亥", new ShugoSin[]{ new ShugoSin( new string[]{ "甲","庚" },          EnmSugosinCond.None,        "水性"),     
                                          new ShugoSin( new string[]{ "甲","庚","戊" },     EnmSugosinCond.Mizu_2num,   "水性") } },
                { "丁子", new ShugoSin[]{ new ShugoSin( new string[]{ "甲","庚","戊" },     EnmSugosinCond.None,        "水性") } },
                { "丁丑", new ShugoSin[]{ new ShugoSin( new string[]{ "甲","庚" },          EnmSugosinCond.None,        "水性・土性") } },

                 //戊-*
                { "戊寅", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","甲","癸" },      EnmSugosinCond.None,      "金性",              "特に庚金が忌神") } },
                { "戊卯", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","甲","癸" },      EnmSugosinCond.None,      "金性",              "特に庚金が忌神") } },
                { "戊辰", new ShugoSin[]{ new ShugoSin( new string[]{ "甲","癸","丙" },      EnmSugosinCond.None,      "土性・金性") } },
                { "戊巳", new ShugoSin[]{ new ShugoSin( new string[]{ "癸","甲","丙" },      EnmSugosinCond.None,      "土性") } },
                { "戊午", new ShugoSin[]{ new ShugoSin( new string[]{ "壬","甲","丙","金" }, EnmSugosinCond.Mizu_Ari,  "土性"),  
                                          new ShugoSin( new string[]{ "壬", "甲", "丙" },    EnmSugosinCond.Mizu_Nasi, "土・金") } },
                { "戊未", new ShugoSin[]{ new ShugoSin( new string[]{ "癸","甲","丙" },      EnmSugosinCond.None,      "土性・金性") } },
                { "戊申", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","癸","甲" },      EnmSugosinCond.None,      "金性") } },
                { "戊酉", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","癸" },           EnmSugosinCond.None,      "金性") } },
                { "戊戌", new ShugoSin[]{ new ShugoSin( new string[]{ "甲","癸","丙" },      EnmSugosinCond.None,      "金性・土性") } },
                { "戊亥", new ShugoSin[]{ new ShugoSin( new string[]{ "甲","丙" },           EnmSugosinCond.None,      "水性・金性") } },
                { "戊子", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","甲" },           EnmSugosinCond.None,      "水性・金性",       "水性が強い場合は、土性があった方が良い") } },
                { "戊丑", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","甲" },           EnmSugosinCond.None,      "土・水・金") } },

                 //己-*
                { "己寅", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","戊","甲" }, EnmSugosinCond.None,       "金性") } },
                { "己卯", new ShugoSin[]{ new ShugoSin( new string[]{ "甲","丙","癸" }, EnmSugosinCond.None,       "金性") } },  
                { "己辰", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","癸","甲" }, EnmSugosinCond.Do_Weak,    "土性・金性",    "土性が強ければ、甲木がより大事")} },
                { "己巳", new ShugoSin[]{ new ShugoSin( new string[]{ "癸","丙","辛" }, EnmSugosinCond.None,       "土性",             "戊・癸の干合を特に嫌う") } },
                { "己午", new ShugoSin[]{ new ShugoSin( new string[]{ "壬","丙","辛" }, EnmSugosinCond.None,       "土性") } },
                { "己未", new ShugoSin[]{ new ShugoSin( new string[]{ "癸","丙" },      EnmSugosinCond.None,       "土性・金性",       "甲木は癸水を洩らすので守護神にならない") } },
                { "己申", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","癸" },      EnmSugosinCond.None,       "金性") } },
                { "己酉", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","癸" },      EnmSugosinCond.None,       "金性") } },
                { "己戌", new ShugoSin[]{ new ShugoSin( new string[]{ "甲","丙","癸" }, EnmSugosinCond.None,       "金性") } },
                { "己亥", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","甲","戊" }, EnmSugosinCond.Mizu_Weak,  "水性"),     
                                          new ShugoSin( new string[]{ "丙","戊" },      EnmSugosinCond.Mizu_Strong,"水性") } }, //水性が強ければ戊が守②となる
                { "己子", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","甲","戊" }, EnmSugosinCond.Mizu_Weak,  "水性"),     
                                          new ShugoSin( new string[]{ "丙","戊" },     EnmSugosinCond.Mizu_Strong,"水性") } }, //水性が強ければ戊が守②となる
                { "己丑", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","甲" },      EnmSugosinCond.None,       "水・土・金") } },

                 //庚-*
                { "庚寅", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","甲" },      EnmSugosinCond.None,       "癸水") } },
                { "庚卯", new ShugoSin[]{ new ShugoSin( new string[]{ "丁","甲","庚" }, EnmSugosinCond.None,       "癸水") } },
                { "庚辰", new ShugoSin[]{ new ShugoSin( new string[]{ "甲","丁" },      EnmSugosinCond.None,       "土性・癸"),
                                          new ShugoSin( new string[]{ "甲","丁" },      EnmSugosinCond.Do_2numOver,"土性・水性・庚金"),//土性が多い（２つ以上）ときは庚金も忌神
                                          new ShugoSin( new string[]{ "甲","丁","壬" }, EnmSugosinCond.Hi_2num,    "土性・癸水") } },
                { "庚巳", new ShugoSin[]{ new ShugoSin( new string[]{ "壬","丙" },      EnmSugosinCond.Mizu_Nasi,  "土性"),    
                                          new ShugoSin( new string[]{ "壬","丙","辛" }, EnmSugosinCond.Hi_2num,    "土性") } },
                { "庚午", new ShugoSin[]{ new ShugoSin( new string[]{ "壬","辛" },      EnmSugosinCond.None,       "火性"),        
                                          new ShugoSin( new string[]{ "壬","辛" },      EnmSugosinCond.Mizu_Nasi,  "土性") } },
                { "庚未", new ShugoSin[]{ new ShugoSin( new string[]{ "丁","甲" },      EnmSugosinCond.None,       "土性・癸水"),
                                          new ShugoSin( new string[]{ "丁","甲","壬" }, EnmSugosinCond.Hi_2num,    "土性"),
                                          new ShugoSin( new string[]{ "甲","丁" },      EnmSugosinCond.Do_Toukan,  "土性",   "土性が透干") } },
                { "庚申", new ShugoSin[]{ new ShugoSin( new string[]{ "丁","甲" },      EnmSugosinCond.None,       "水性",        "特に癸水を嫌う") } },
                { "庚酉", new ShugoSin[]{ new ShugoSin( new string[]{ "丁","甲","丙" }, EnmSugosinCond.None,       "水性",        "特に癸水を嫌う") } },
                { "庚戌", new ShugoSin[]{ new ShugoSin( new string[]{ "甲","壬" },      EnmSugosinCond.None,       "土性") } },
                { "庚亥", new ShugoSin[]{ new ShugoSin( new string[]{ "丁","甲","丙" }, EnmSugosinCond.None,       "水性",        "冬の庚金は官・殺両透を忌まない") } },
                { "庚子", new ShugoSin[]{ new ShugoSin( new string[]{ "丁","甲","丙" }, EnmSugosinCond.None,       "水性",        "冬の庚金は官・殺両透を忌まない") } },
                { "庚丑", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","丁","甲" }, EnmSugosinCond.None,       "水性",        "冬の庚金は官・殺両透を忌まない") } },

                 //辛-*  
                { "辛寅", new ShugoSin[]{ new ShugoSin( new string[]{ "己","壬" },           EnmSugosinCond.None,        "丁"),               
                                          new ShugoSin( new string[]{ "己","壬","庚" },      EnmSugosinCond.Moku_Strong, "丁") } },
                { "辛卯", new ShugoSin[]{ new ShugoSin( new string[]{ "壬","甲" },           EnmSugosinCond.None,        "土性・丁",    "特に戊土を嫌う") } },
                { "辛辰", new ShugoSin[]{ new ShugoSin( new string[]{ "壬","甲" },           EnmSugosinCond.None,        "土性",         "特に戊土を嫌う") } },
                { "辛巳", new ShugoSin[]{ new ShugoSin( new string[]{ "壬","癸","庚" },      EnmSugosinCond.None,        "丁・土性") } },
                { "辛午", new ShugoSin[]{ new ShugoSin( new string[]{ "壬","己","癸" },      EnmSugosinCond.None,        "丁・土性"),
                                          new ShugoSin( new string[]{ "壬","癸" },           EnmSugosinCond.Tei_Weak,    "丁・土性"),     //丁火の影響が小さければ守護神に己土は必要ない
                                          new ShugoSin( new string[]{ "壬","己","癸","庚" }, EnmSugosinCond.Mizu_1num,   "丁・土性") } },
                { "辛未", new ShugoSin[]{ new ShugoSin( new string[]{ "壬","庚","甲" },      EnmSugosinCond.None,        "土・火",      "火性は特に丁火を嫌う"),
                                          new ShugoSin( new string[]{ "壬","庚" },           EnmSugosinCond.Do1Hi2,      "土・火") } },
                { "辛申", new ShugoSin[]{ new ShugoSin( new string[]{ "壬","甲" },           EnmSugosinCond.None,        "土・火",      "火性は特に丁火を嫌う") } },
                { "辛酉", new ShugoSin[]{ new ShugoSin( new string[]{ "壬" },                EnmSugosinCond.None,        "火",          "火性は特に丁火を嫌う"),
                                          new ShugoSin( new string[]{ "壬","甲" },           EnmSugosinCond.Do_Ari,      "土・火") } },
                { "辛戌", new ShugoSin[]{ new ShugoSin( new string[]{ "壬","甲" },           EnmSugosinCond.None,        "土性",        "甲木の価値は高い") } },
                { "辛亥", new ShugoSin[]{ new ShugoSin( new string[]{ "壬","丙" },           EnmSugosinCond.Mizu_Strong, "土性・水性"),
                                          new ShugoSin( new string[]{ "壬", "丙" },          EnmSugosinCond.Mizu_Strong, "土性") } },
                { "辛子", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","壬" },           EnmSugosinCond.None,        "土性"),
                                          new ShugoSin( new string[]{ "丙","壬" },           EnmSugosinCond.Mizu_Strong, "土性・水性"),
                                          new ShugoSin( new string[]{ "丙","壬","甲" },      EnmSugosinCond.Hei_Weak,    "土性") } },
                { "辛丑", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","壬" },           EnmSugosinCond.None,        "土性"),
                                          new ShugoSin( new string[]{ "丙", "壬"},           EnmSugosinCond.Mizu_Strong, "土性・水性"),
                                          new ShugoSin( new string[]{ "丙", "壬", "甲" },    EnmSugosinCond.Do_2num,     "土性") } },

                 //壬-*
                { "壬寅", new ShugoSin[]{ new ShugoSin( new string[]{ "戊","庚","丙" }, EnmSugosinCond.None,        "木性") } },
                { "壬卯", new ShugoSin[]{ new ShugoSin( new string[]{ "戊","辛" },      EnmSugosinCond.None,        "木性") } },
                { "壬辰", new ShugoSin[]{ new ShugoSin( new string[]{ "甲","庚" },      EnmSugosinCond.None,        "土性") } },
                { "壬巳", new ShugoSin[]{ new ShugoSin( new string[]{ "壬","庚" },      EnmSugosinCond.None,        "火性・土性") } },
                { "壬午", new ShugoSin[]{ new ShugoSin( new string[]{ "癸","庚" },      EnmSugosinCond.None,        "火性・土性") } },
                { "壬未", new ShugoSin[]{ new ShugoSin( new string[]{ "庚","甲","癸" }, EnmSugosinCond.None,        "土性・火性"),
                                          new ShugoSin( new string[]{ "癸","甲","庚" }, EnmSugosinCond.Do_2numOver, "土性・火性"),
                                          new ShugoSin( new string[]{ "甲","癸","庚" }, EnmSugosinCond.Hi_2num,     "水性・土性") } },
                { "壬申", new ShugoSin[]{ new ShugoSin( new string[]{ "戊","丁" },      EnmSugosinCond.None,        "金性") } },
                { "壬酉", new ShugoSin[]{ new ShugoSin( new string[]{ "甲","辛" },      EnmSugosinCond.None,        "土性",     "金白水清") } },
                { "壬戌", new ShugoSin[]{ new ShugoSin( new string[]{ "甲","丙" },      EnmSugosinCond.None,        "金性"),
                                          new ShugoSin( new string[]{ "甲","丙","戊" }, EnmSugosinCond.Mizu_2num,   "金性") } },
                { "壬亥", new ShugoSin[]{ new ShugoSin( new string[]{ "戊","丙" },      EnmSugosinCond.None,        "水性"),
                                          new ShugoSin( new string[]{ "戊","丙","甲" }, EnmSugosinCond.Do_Nasi,     "水性"),
                                          new ShugoSin( new string[]{ "戊","丙" },      EnmSugosinCond.Do_Ari,      "水性・木性") } },
                { "壬子", new ShugoSin[]{ new ShugoSin( new string[]{ "戊","丙" },      EnmSugosinCond.None,        "水性",              "戊土と丙火は、ほぼ同価値") } },
                { "壬丑", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","甲" },      EnmSugosinCond.None,        "水性"),
                                          new ShugoSin( new string[]{ "丙","甲","戊" }, EnmSugosinCond.Mizu_2num,   "水性") } },

                 //癸-*
                { "癸寅", new ShugoSin[]{ new ShugoSin( new string[]{ "辛","丙" },      EnmSugosinCond.None,     "土性") } },
                { "癸卯", new ShugoSin[]{ new ShugoSin( new string[]{ "辛","庚","丙" }, EnmSugosinCond.None,     "土性") } },
                { "癸辰", new ShugoSin[]{ new ShugoSin( new string[]{ "辛","丙","甲" }, EnmSugosinCond.None,     "土性") } },
                { "癸巳", new ShugoSin[]{ new ShugoSin( new string[]{ "辛","壬" },      EnmSugosinCond.None,     "火性・土性",       "丙火性は特に丁火を嫌う"),
                                          new ShugoSin( new string[]{ "辛","壬","庚" }, EnmSugosinCond.Sin_Weak, "火性・土性") } },
                { "癸午", new ShugoSin[]{ new ShugoSin( new string[]{ "辛","壬" },      EnmSugosinCond.None,     "火性・土性"),
                                          new ShugoSin( new string[]{ "辛","壬","庚" }, EnmSugosinCond.Sin_Weak, "火性・土性") } },
                { "癸未", new ShugoSin[]{ new ShugoSin( new string[]{ "辛","壬" },      EnmSugosinCond.None,     "火性・土性") } },
                { "癸申", new ShugoSin[]{ new ShugoSin( new string[]{ "丁","甲","辛" }, EnmSugosinCond.None,     "土性") } },
                { "癸酉", new ShugoSin[]{ new ShugoSin( new string[]{ "辛","丙" },      EnmSugosinCond.None,     "土性",       "金白水清") } },
                { "癸戌", new ShugoSin[]{ new ShugoSin( new string[]{ "辛","甲" },      EnmSugosinCond.None,     "土性"),
                                          new ShugoSin( new string[]{ "辛","甲","壬" }, EnmSugosinCond.Ki_Weak,  "土性") } },
                { "癸亥", new ShugoSin[]{ new ShugoSin( new string[]{ "辛","丙" },      EnmSugosinCond.None,     "水性") } },
                { "癸子", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","辛" },      EnmSugosinCond.None,     "水性") } },
                { "癸丑", new ShugoSin[]{ new ShugoSin( new string[]{ "丙","辛","甲" }, EnmSugosinCond.None,     "水性・土性") } },
              };
            //--------------------------------

            //-----------------------------------
            // 五行属性どうしの関係(相生、粗剋）)
            //-----------------------------------
            gogyouAttrRelationshipTbl = new GogyouAttrRerationshipTbl();
            gogyouAttrRelationshipTbl.dicGogyouAttrRelationship = new Dictionary<string, GogyouAttrRelationship>()
            {
                {"木", new GogyouAttrRelationship("木", "水", "火", "金","土") },
                {"火", new GogyouAttrRelationship("火", "木", "土", "水","金") },
                {"土", new GogyouAttrRelationship("土", "火", "金", "木","水") },
                {"金", new GogyouAttrRelationship("金", "土", "水", "火","木") },
                {"水", new GogyouAttrRelationship("水", "金", "木", "土","火") },
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
