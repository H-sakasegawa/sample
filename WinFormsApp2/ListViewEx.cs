using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WinFormsApp2
{
    class ListViewEx : ListView
    {
        public ListViewEx()
        {
            DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(OnDrawColumnHeader);
            DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(OnDrawItem);
            DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(OnDrawSubItem);

        }
        public void OnDrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        public void OnDrawItem(object sender, DrawListViewItemEventArgs e)
        {
            if ((e.State & ListViewItemStates.Selected) == ListViewItemStates.Selected)
            {
                e.DrawFocusRectangle();
            }
            // View.DetailsならばDrawSubItemイベントで描画するため、ここでは描画しない
            if (View != View.Details)
            {
                e.DrawText();
            }
        }

        public void OnDrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            Brush brush = new SolidBrush(e.Item.ForeColor);
            if (e.Item.Selected)
            {
                // Hightlightで範囲を塗りつぶす
                e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds);
            }
            else
            {
                var brs = new SolidBrush(e.Item.BackColor);
                e.Graphics.FillRectangle(brs, e.Bounds);
            }
            // 上で設定した,brushとdrawFormatを利用して文字を描画する
            e.Graphics.DrawString(e.SubItem.Text, e.Item.Font, brush, e.Bounds);
            brush.Dispose();
        }

    }
}
