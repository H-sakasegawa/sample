using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp2
{
    class TabControlEx : TabControl
    {
        int CloseBtnW = 16;

        public class EventCloseTab : EventArgs
        {
            public int tabIndex;
        }

        // タブの閉じるボタンクリックイベント
        public event EventHandler onTabCloseButtonClick;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TabControlEx()
        {
        }

        /// <summary>
        /// タブの閉じるボタンクリックイベント
        /// </summary>
        /// <param name="e"></param>
        protected void OnCloseButtonClick(EventArgs e)
        {

           this.onTabCloseButtonClick?.Invoke(this, e);
        }

        /// <summary>
        /// OnMouseUp
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Point pt = new Point(e.X, e.Y);
            Rectangle rect = new Rectangle();
            int tabIndex = this.GetTabCloseButtonRect(pt, rect);
            if (tabIndex>=0)
            {
                EventCloseTab ev = new EventCloseTab();
                ev.tabIndex = tabIndex;
                this.OnCloseButtonClick(ev);
                this.Invalidate(rect);
            }
        }

        /// <summary>
        /// タブの閉じるボタン場所を取得
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        private int GetTabCloseButtonRect(Point pt,Rectangle rect)
        {
            for (int i = 0; i < base.TabCount; i++)
            {
                rect = this.GetTabCloseButtonRect(i);
                if (rect.Contains(pt))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// タブの閉じるボタン場所を取得
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Rectangle GetTabCloseButtonRect(int index)
        {
            Rectangle rect = this.GetTabRect(index);
            rect.X = rect.Right - 20;
            rect.Y = rect.Top + 2;
            rect.Width = CloseBtnW;
            rect.Height = 16;

            return rect;
        }

        /// <summary>
        /// タブに閉じるボタンを描画
        /// </summary>
        private void DrawTabCloseButton()
        {
            Graphics g = this.CreateGraphics();
            Rectangle rect = Rectangle.Empty;
            Point pt = this.PointToClient(Cursor.Position);
            for (int i = 0; i < this.TabPages.Count; i++)
            {
                if (i == 0) continue;
                rect = this.GetTabCloseButtonRect(i);
                // 閉じるボタン描画
                ControlPaint.DrawCaptionButton(g, rect, CaptionButton.Close, ButtonState.Flat);
            }
            g = null;
        }

        /// <summary>
        /// WndProc
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            const int WM_PAINT = 0x000F;

            switch (m.Msg)
            {
                case WM_PAINT:
                    this.DrawTabCloseButton();
                    break;
                default:
                    break;
            }
        }
    }
}
