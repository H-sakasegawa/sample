using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WinFormsApp2
{
    public class LvItemDataBase
    {
        public List<LvItemColor> lstItemColors = new List<LvItemColor>();
    }
    public class LvItemColor
    {
        public LvItemColor(int _col, Color _color)
        {
            colmn = _col;
            backColor = _color;
        }
        public int colmn;
        public Color backColor;
    }

    class ListViewEx : ListView
    {
        public Brush HeaderBackColor { get; set; } = Brushes.Gainsboro;


        public ListViewEx()
        {
            DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(OnDrawColumnHeader);
            DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(OnDrawItem);
            DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(OnDrawSubItem);

        }
        public void OnDrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            // e.DrawDefault = true;
            // カラムヘッダに四角を描画する。
            e.Graphics.FillRectangle(HeaderBackColor, e.Bounds);
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;
            sf.FormatFlags |= StringFormatFlags.NoWrap;
            // カラムヘッダ文字列を描画する。
            e.Graphics.DrawString(e.Header.Text, this.Font, Brushes.Black, e.Bounds, sf);


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
            Brush brs = new SolidBrush(e.Item.BackColor);
            if (e.Item.Selected)
            {
                // Hightlightで範囲を塗りつぶす
                //e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds);
                brs = new SolidBrush(Color.LightBlue);
            }
 
            {
                LvItemDataBase lvItemData = (LvItemDataBase)e.Item.Tag;
                if (lvItemData!=null && lvItemData.lstItemColors != null)
                {
                    var itemColor = lvItemData.lstItemColors.Find(x => x.colmn == e.ColumnIndex);
                    if( itemColor!=null)
                    {
                        brs = new SolidBrush(itemColor.backColor);
                    }
                }
  
            }
            // 上で設定した,brushとdrawFormatを利用して文字を描画する
            e.Graphics.FillRectangle(brs, e.Bounds);
            e.Graphics.DrawString(e.SubItem.Text, e.Item.Font, brush, e.Bounds);
            brs.Dispose();
            brush.Dispose();
        }

    }
}
