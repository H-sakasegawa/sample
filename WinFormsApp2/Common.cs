using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    /// 共通処理
    /// </summary>
    class Common
    {

        /// <summary>
        /// 経過月数計算
        /// （流用）https://smdn.jp/programming/netfx/tips/calc_elapsed_years/
        /// </summary>
        /// <param name="baseDay"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static int GetElapsedMonths(DateTime baseDay, DateTime day)
        {
            if (day < baseDay)
                // 日付が基準日より前の場合は例外とする
                throw new ArgumentException();

            // 経過月数を求める(満月数を考慮しない単純計算)
            var elapsedMonths = (day.Year - baseDay.Year) * 12 + (day.Month - baseDay.Month);

            if (baseDay.Day <= day.Day)
                // baseDayの日部分がdayの日部分以上の場合は、その月を満了しているとみなす
                // (例:1月30日→3月30日以降の場合は満(3-1)ヶ月)
                return elapsedMonths;
            else if (day.Day == DateTime.DaysInMonth(day.Year, day.Month) && day.Day <= baseDay.Day)
                // baseDayの日部分がdayの表す月の末日以降の場合は、その月を満了しているとみなす
                // (例:1月30日→2月28日(平年2月末日)/2月29日(閏年2月末日)以降の場合は満(2-1)ヶ月)
                return elapsedMonths;
            else
                // それ以外の場合は、その月を満了していないとみなす
                // (例:1月30日→3月29日以前の場合は(3-1)ヶ月未満、よって満(3-1-1)ヶ月)
                return elapsedMonths - 1;
        }

        /// <summary>
        /// ラベル表示のBold設定
        /// </summary>
        /// <param name="label"></param>
        /// <param name="bBold"></param>
        public static void SetBold(Label label, bool bBold)
        {
            FontStyle fontStyle = FontStyle.Regular;
            if (bBold) fontStyle = FontStyle.Bold;

            label.Font = new Font(label.Font, fontStyle);
        }

        /// <summary>
        /// enumKansiItemID ⇒ 項目ビット情報変換
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int ConvEnumKansiItemIDToItemBit(Const.enumKansiItemID id)
        {
            switch (id)
            {
                case Const.enumKansiItemID.GETUUN: return Const.bitFlgGetuun;
                case Const.enumKansiItemID.NENUN: return Const.bitFlgNenun;
                case Const.enumKansiItemID.TAIUN: return Const.bitFlgTaiun;
                case Const.enumKansiItemID.NIKKANSI: return Const.bitFlgNiti;
                case Const.enumKansiItemID.GEKKANSI: return Const.bitFlgGetu;
                case Const.enumKansiItemID.NENKANSI: return Const.bitFlgNen;
            }
            return 0;
        }

        public static void SetGroupCombobox(Persons persons, ComboBox cmb, string selectGroup = null)
        {
            var groups = persons.GetGroups();
            cmb.Items.Clear();

            cmb.Items.Add(new Group("全て", Group.GroupType.ALL));

            foreach (var group in groups)
            {
                cmb.Items.Add(group);
            }
            if (string.IsNullOrEmpty(selectGroup) || selectGroup == "全て")
            {
                if (cmb.Items.Count > 0)
                {
                    cmb.SelectedIndex = 0;
                }
            }
            else
            {
                for (int i = 1; i < cmb.Items.Count; i++)
                {
                    if (((Group)cmb.Items[i]).groupName == selectGroup)
                    {
                        cmb.SelectedIndex = i;
                        break;
                    }
                }
            }


        }

        public class TaiunItems
        {
            public string title;
            public Kansi targetKansi;
            public string[] sItems = new string[Enum.GetValues(typeof(Const.ColTaiun)).Length];

            public Color colorTenchusatu = Color.Black;
            public bool bTenchusatu = false;
            public bool bShugosin = false;
            public bool bImigami = false;

        }
        public class NenunGetuunItems
        {
            public string title;
            public Kansi targetKansi;
            public string[] sItems = new string[Enum.GetValues(typeof(Const.ColNenunListView)).Length];

            public Color colorTenchusatu = Color.Black;
            public bool bTenchusatu = false;
            public bool bShugosin = false;
            public bool bImigami = false;

        }

        public static TaiunItems GetTaiunItem(Person person, string title, int kansiNo, int startNen )
        {
            TaiunItems item = new TaiunItems();
            TableMng tblMng = TableMng.GetTblManage();


            item.title = title;

            Kansi taiunKansi = person.GetKansi(kansiNo);
            item.targetKansi = taiunKansi;

            item.sItems[(int)Const.ColTaiun.COL_KANSI] = string.Format("{0}{1}", taiunKansi.kan, taiunKansi.si); //干支

            string judai = person.GetJudaiShusei(person.nikkansi.kan, taiunKansi.kan).name;
            string junidai = person.GetJunidaiShusei(person.nikkansi.kan, taiunKansi.si).name;

            //"星"を削除
            judai = judai.Replace("星", "");
            junidai = junidai.Replace("星", "");

            item.sItems[(int)Const.ColTaiun.COL_JUDAISHUSEI] = (judai); //十大主星
            item.sItems[(int)Const.ColTaiun.COL_JUNIDAIJUUSEI] = (junidai); //十二大従星

            int idxNanasatuItem = 0;

            //日
            GouhouSannpouResult[] gouhouSanpoui = person.GetGouhouSanpouEx(taiunKansi, person.nikkansi, null, null);
            string nanasatu = (person.IsNanasatu(taiunKansi, person.nikkansi, ref idxNanasatuItem) == true && idxNanasatuItem == 1) ? Const.sNanasatu : "";   //七殺
            string kangou = person.GetKangoStr(taiunKansi, person.nikkansi); //干合            
            item.sItems[(int)Const.ColTaiun.COL_GOUHOUSANPOU_NITI] = (Common.GetListViewItemString(gouhouSanpoui, kangou, nanasatu));

            //月
            gouhouSanpoui = person.GetGouhouSanpouEx(taiunKansi, person.gekkansi, null, null);
            nanasatu = (person.IsNanasatu(taiunKansi, person.gekkansi, ref idxNanasatuItem) == true && idxNanasatuItem == 1) ? Const.sNanasatu : "";   //七殺
            kangou = person.GetKangoStr(taiunKansi, person.gekkansi); //干合
            item.sItems[(int)Const.ColTaiun.COL_GOUHOUSANPOU_GETU] = (Common.GetListViewItemString(gouhouSanpoui, kangou, nanasatu));

            //年
            gouhouSanpoui = person.GetGouhouSanpouEx(taiunKansi, person.nenkansi, null, null);
            nanasatu = (person.IsNanasatu(taiunKansi, person.nenkansi, ref idxNanasatuItem) == true && idxNanasatuItem == 1) ? Const.sNanasatu : "";   //七殺
            kangou = person.GetKangoStr(taiunKansi, person.nenkansi); //干合
            item.sItems[(int)Const.ColTaiun.COL_GOUHOUSANPOU_NEN] = (Common.GetListViewItemString(gouhouSanpoui, kangou, nanasatu));

            //天中殺
            Color color = Color.Black;
            for (int i = 0; i < person.nikkansi.tenchusatu.ToArray().Length; i++)
            {
                if (taiunKansi.kan == person.nikkansi.tenchusatu[i] ||
                   taiunKansi.si == person.nikkansi.tenchusatu[i])
                {
                    item.bTenchusatu = true;
                    color = Color.Red;
                    break;
                }
            }

            item.colorTenchusatu = color;

            //干、支の属性取得
            string kanAttr = tblMng.jyukanTbl[taiunKansi.kan].gogyou;
            string siAttr = tblMng.jyunisiTbl[taiunKansi.si].gogyou;


            //守護神判定
            item.bShugosin = ShugosinUtil.IsShugosin(person, taiunKansi.kan);


            //item.bShugosin = false;
            //if (shugosinAttr.Count>0)
            //{
            //    foreach (var shugosin in shugosinAttr)
            //    {
            //        //if (kanAttr == shugosinAttr || siAttr == shugosinAttr)
            //        if (kanAttr == shugosin.gogyouAttr) //干のみ　支は見ない
            //        {
            //            item.bShugosin = true;
            //        }
            //    }
            //}
            //else
            //{
            //    if (shugosinKan != null)
            //    {
            //        foreach (var kan in shugosinKan)
            //        {
            //            if (kan == taiunKansi.kan)
            //            {
            //                item.bShugosin = true;
            //            }
            //        }
            //    }
            //}
            //忌神判定
            item.bImigami = ShugosinUtil.IsImigami(person, taiunKansi.kan);

            //item.bImigami = false;
            //foreach (var imigami in imigamiAttr)
            //{
            //    //if (kanAttr == imigamiAttr || siAttr == imigamiAttr)
            //    if (kanAttr == imigami.gogyouAttr) //干のみ　支は見ない
            //    {
            //        item.bImigami = true;
            //    }
            //}

            return item;

        }

        /// <summary>
        /// 年運、月運リスト表示データ取得
        /// </summary>
        /// <param name="person"></param>
        /// <param name="title"></param>
        /// <param name="targetkansiNo"></param>
        /// <param name="taiunKansi"></param>
        /// <returns></returns>
        public static NenunGetuunItems GetNenunGetuunItems(Person person, string title, int targetkansiNo, Kansi taiunKansi)
        {
            NenunGetuunItems item = new NenunGetuunItems();
            TableMng tblMng = TableMng.GetTblManage();

            item.title = title;

            Kansi targetKansi = person.GetKansi(targetkansiNo);
            item.targetKansi = targetKansi;

            int idxNanasatuItem = 0;


            string judai = person.GetJudaiShusei(person.nikkansi.kan, targetKansi.kan).name;
            string junidai = person.GetJunidaiShusei(person.nikkansi.kan, targetKansi.si).name;


            item.sItems[(int)Const.ColNenunListView.COL_KANSI] = string.Format("{0}{1}", targetKansi.kan, targetKansi.si); //干支

            //"星"を削除
            judai = judai.Replace("星", "");
            junidai = junidai.Replace("星", "");

            item.sItems[(int)Const.ColNenunListView.COL_JUDAISHUSEI] = judai; //十大主星
            item.sItems[(int)Const.ColNenunListView.COL_JUNIDAIJUUSEI] = junidai; //十二大従星

            //合法三法(日)
            GouhouSannpouResult[] gouhouSanpoui = person.GetGouhouSanpouEx(targetKansi, person.nikkansi, taiunKansi, targetKansi);
            string kangou = person.GetKangoStr(targetKansi, person.nikkansi); //干合            
            string nanasatu = (person.IsNanasatu(targetKansi, person.nikkansi, ref idxNanasatuItem) == true && idxNanasatuItem == 1) ? Const.sNanasatu : "";   //七殺
            item.sItems[(int)Const.ColNenunListView.COL_GOUHOUSANPOU_NITI] = GetListViewItemString(gouhouSanpoui, kangou, nanasatu);

            //合法三法(月)
            gouhouSanpoui = person.GetGouhouSanpouEx(targetKansi, person.gekkansi, taiunKansi, targetKansi);
            kangou = person.GetKangoStr(targetKansi, person.gekkansi); //干合  
            nanasatu = (person.IsNanasatu(targetKansi, person.gekkansi, ref idxNanasatuItem) == true && idxNanasatuItem == 1) ? Const.sNanasatu : "";   //七殺
            item.sItems[(int)Const.ColNenunListView.COL_GOUHOUSANPOU_GETU] = GetListViewItemString(gouhouSanpoui, kangou, nanasatu);

            //合法三法(年)
            gouhouSanpoui = person.GetGouhouSanpouEx(targetKansi, person.nenkansi, taiunKansi, targetKansi);
            kangou = person.GetKangoStr(targetKansi, person.nenkansi); //干合  
            nanasatu = (person.IsNanasatu(targetKansi, person.nenkansi, ref idxNanasatuItem) == true && idxNanasatuItem == 1) ? Const.sNanasatu : "";   //七殺
            item.sItems[(int)Const.ColNenunListView.COL_GOUHOUSANPOU_NEN] = GetListViewItemString(gouhouSanpoui, kangou, nanasatu);


            //天中殺
            item.colorTenchusatu = Color.Black;
            for (int i = 0; i < 2; i++)
            {
                if (targetKansi.IsExist(person.nikkansi.tenchusatu[i]))
                {
                    item.bTenchusatu = true;
                    item.colorTenchusatu = Color.Red;
                    break;
                }
            }


            //干、支の属性取得
            //string kanAttr = tblMng.jyukanTbl[targetKansi.kan].gogyou;
            //string siAttr = tblMng.jyunisiTbl[targetKansi.si].gogyou;


            //守護神判定
            item.bShugosin = ShugosinUtil.IsShugosin(person, targetKansi.kan);
            //item.bShugosin = false;
            //if (shugosinAttr.Count>0)
            //{
            //    foreach (var shugosin in shugosinAttr)
            //    {
            //        //if (kanAttr == shugosinAttr || siAttr == shugosinAttr)
            //        if (kanAttr == shugosin.gogyouAttr) //干のみ　支は見ない

            //        {
            //            item.bShugosin = true;
            //        }
            //    }
            //}
            //else
            //{
            //    if (choukouShugosinKan != null)
            //    {
            //        foreach (var kan in choukouShugosinKan)
            //        {
            //            if (kan == targetKansi.kan)
            //            {
            //                item.bShugosin = true;
            //            }
            //        }
            //    }
            //}
            //忌神判定
            item.bImigami = ShugosinUtil.IsImigami(person, targetKansi.kan);
            //item.bImigami = false;
            //foreach (var imigami in imigamiAttr)
            //{
            //    //if (kanAttr == imigamiAttr || siAttr == imigamiAttr)
            //    if (kanAttr == imigami.gogyouAttr)//干のみ　支は見ない
            //    {
            //        item.bImigami = true;
            //    }
            //}

            return item;
        }

        public static  string GetListViewItemString(GouhouSannpouResult[] lstGouhouSanpouResult, params string[] ary)
        {
            string result = "";
            foreach (var item in lstGouhouSanpouResult)
            {
                if (!string.IsNullOrEmpty(result)) result += " ";
                if (item.bEnable) result += item.displayName;
                else result += string.Format("[{0}]", item.displayName);
            }
            foreach (var item in ary)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    if (!string.IsNullOrEmpty(result)) result += " ";
                    result += item;
                }
            }
            return result;
        }

        public static Font FindFont(
           System.Drawing.Graphics g,
           string longString,
           Size Room,
           Font PreferedFont
        )
        {
            if (string.IsNullOrEmpty(longString)) return PreferedFont;

            // you should perform some scale functions!!!
            SizeF RealSize = g.MeasureString(longString, PreferedFont);
            float HeightScaleRatio = Room.Height / RealSize.Height;
            float WidthScaleRatio = Room.Width / RealSize.Width;

            float ScaleRatio = (HeightScaleRatio < WidthScaleRatio)
               ? ScaleRatio = HeightScaleRatio
               : ScaleRatio = WidthScaleRatio;

            float ScaleFontSize = PreferedFont.Size * ScaleRatio;
            //基準となるフォントサイズより小さい場合は、新しいサイズのフォントを返す
            if (PreferedFont.Size< ScaleFontSize) return PreferedFont;

            return new Font(PreferedFont.FontFamily, ScaleFontSize);
        }
    }

}
