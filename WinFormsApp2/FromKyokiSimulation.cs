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
        Form1 frmParent;
        //List<PictureBox> lstPictureBox = new List<PictureBox>();
        KyokiSimulation sim = new KyokiSimulation();

        public FromKyokiSimulation( Form1 parent)
        {
            InitializeComponent();

            frmParent = parent;

         }

        private void FromKyokiSimulation_Load(object sender, EventArgs e)
        {
        
        }
        public void InitDisp(Person _person, int year,
                            Kansi _getuunKansi, Kansi _nenunKansi, Kansi _taiunKansi,
                            bool _bDispGetuun,
                            bool _bDispSangouKaikyoku,
                            bool _bDispGogyou,
                            bool _bDispGotoku,
                            bool _bDispRefrectGouhou,
                            bool _bDispRefrectSangouKaiyoku
                            )
        {
            UpdateKyokiPatternCountForYear( _person,  _bDispGetuun);
            SetYearList(year);

            UpdateKyokiPattern( _person,  year,
                             _getuunKansi,  _nenunKansi,  _taiunKansi,
                             _bDispGetuun,
                             _bDispSangouKaikyoku,
                             _bDispGogyou,
                             _bDispGotoku,
                             _bDispRefrectGouhou,
                             _bDispRefrectSangouKaiyoku);


        }

        public void UpdateKyokiPattern(Person _person, int year,
                            Kansi _getuunKansi, Kansi _nenunKansi, Kansi _taiunKansi,
                            bool _bDispGetuun,
                            bool _bDispSangouKaikyoku,
                            bool _bDispGogyou,
                            bool _bDispGotoku,
                            bool _bDispRefrectGouhou,
                            bool _bDispRefrectSangouKaiyoku)
        {

            SetYearList(year);

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
                pictureBox.Width = 290;
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
                sc.Width = 345;
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


        private void SetYearList( int year)
        {
            lvPatternNum.SelectedIndexChanged -= lvPatternNum_SelectedIndexChanged;
            string sYear = year.ToString();

            for( int i=0; i< lvPatternNum.Items.Count; i++)
            {
                if(lvPatternNum.Items[i].Text == sYear)
                {
                    lvPatternNum.Items[i].Selected = true;
                    lvPatternNum.EnsureVisible(i);
                    break;
                }
            }

            lvPatternNum.SelectedIndexChanged += lvPatternNum_SelectedIndexChanged;
        }
        /// <summary>
        /// 年毎の虚気変化数一覧
        /// </summary>
        private void UpdateKyokiPatternCountForYear(Person _person, bool _bDispGetuun)
        {
            lvPatternNum.Items.Clear();

            TableMng tblMng = TableMng.GetTblManage();

            var lstTaiunKansi = _person.GetTaiunKansiList();

            int startYear = _person.birthday.year;
            int lastYear = startYear + 100; //100年

            int iTaiun = 0;
            for (int year = startYear; year <= lastYear; year++)
            {
                //大運干支
                if(iTaiun<lstTaiunKansi.Count-1 && lstTaiunKansi[iTaiun+1].year == year)
                {
                    iTaiun++;
                }
                int gekkansiNo = lstTaiunKansi[iTaiun].kansiNo;
                Kansi taiunKansi = _person.GetKansi(gekkansiNo);

                //年運干支
                Kansi nenunKansi = _person.GetNenkansi(year);

                //月運干支
                gekkansiNo = tblMng.setuiribiTbl.GetGekkansiNo(year, 2);
                Kansi getuunKansi = _person.GetKansi(gekkansiNo);

                sim.Simulation(_person, getuunKansi, nenunKansi, taiunKansi, _bDispGetuun);

                int patternNum = sim.lstKansPattern.Count;

                var lvItem= lvPatternNum.Items.Add(string.Format("{0}", year));
                lvItem.SubItems.Add(string.Format("{0}", patternNum-1));

            }

        }
        private void FromKyokiSimulation_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            e.Cancel=true;
        }

        private void lvPatternNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            var items = lvPatternNum.SelectedItems;
            if (items.Count == 0) return;

            int year = int.Parse(items[0].Text);
            frmParent.UpdateNeunTaiunDisp(year);
        }
    }
}
