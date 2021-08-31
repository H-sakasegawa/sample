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
            cmb.Items.Add("全て");
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
    }

}
