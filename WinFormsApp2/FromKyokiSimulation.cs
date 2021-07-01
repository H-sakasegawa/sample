using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class FromKyokiSimulation : Form
    {

        //List<PictureBox> lstPictureBox = new List<PictureBox>();

        public FromKyokiSimulation()
        {
            InitializeComponent();

         }

        private void FromKyokiSimulation_Load(object sender, EventArgs e)
        {
        }
        public void InitDisp(Person _person, Kansi _getuunKansi, Kansi _nenunKansi, Kansi _taiunKansi,
                            bool _bDispGetuun,
                            bool _bDispSangouKaikyoku,
                            bool _bDispGogyou,
                            bool _bDispGotoku,
                            bool _bDispRefrectGouhou,
                            bool _bDispRefrectSangouKaiyoku
                            )
        {

            KyokiSimulation sim = new KyokiSimulation();

            sim.Simulation(_person, _getuunKansi, _nenunKansi, _taiunKansi, _bDispGetuun);

            int patternNum = sim.lstKansPattern.Count;

            this.SuspendLayout();
            this.DoubleBuffered = true;

            flowLayoutPanel1.Controls.Clear();
            //lstPictureBox.Clear();

            Person person = _person.Clone();

            int cnt = 0;
            foreach ( var pattern in sim.lstKansPattern)
            {
                Label lbl = new Label();
                lbl.AutoSize = true;
                if (cnt == 0)
                {
                    lbl.Text = string.Format("基本", cnt++);
                }else
                { 
                    lbl.Text = string.Format("{0}回目", cnt++);
                }
                if (pattern.bCirculation) lbl.Text += "\n\r(循環)";
                PictureBox pictureBox = new PictureBox();
                pictureBox.Width = 300;
                pictureBox.Height = 100;
                pictureBox.BorderStyle = BorderStyle.FixedSingle;

                SplitContainer sc = new SplitContainer();
                sc.IsSplitterFixed = true;
                sc.Orientation = Orientation.Vertical;
                sc.SplitterWidth = 1;
                sc.FixedPanel = FixedPanel.Panel1;
                sc.Panel1MinSize =50;
                sc.Panel1.Controls.Add(lbl);
                sc.Panel2.Controls.Add(pictureBox);
                sc.Width = 370;
                flowLayoutPanel1.Controls.Add(sc);
                
               // lstPictureBox.Add(pictureBox);
                person.nikkansi = pattern.aryKansi[(int)Const.enumKansiItemID.NIKKANSI];
                person.gekkansi = pattern.aryKansi[(int)Const.enumKansiItemID.GEKKANSI];
                person.nenkansi = pattern.aryKansi[(int)Const.enumKansiItemID.NENKANSI];

 


                DrawKoutenUn drawItem2 = null;
                drawItem2 = new DrawKoutenUn(person, pictureBox, 
                                        pattern.aryKansi[(int)Const.enumKansiItemID.TAIUN],
                                        pattern.aryKansi[(int)Const.enumKansiItemID.NENUN],
                                        pattern.aryKansi[(int)Const.enumKansiItemID.GETUUN],
                                        _bDispGetuun,
                                        _bDispSangouKaikyoku,
                                        _bDispGogyou,
                                        _bDispGotoku,
                                        _bDispRefrectGouhou,
                                        _bDispRefrectSangouKaiyoku
                                        );
                drawItem2.CalcCoord(0);

                drawItem2.DrawKyokiPattern();
            }
            this.ResumeLayout();

        }

        private void FromKyokiSimulation_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            e.Cancel=true;
        }
    }
}
