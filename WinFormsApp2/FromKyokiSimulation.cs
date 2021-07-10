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
    public  delegate void CloseHandler();

    public partial class FromKyokiSimulation : Form
    {
        Form1 frmParent;
        //List<PictureBox> lstPictureBox = new List<PictureBox>();
        KyokiSimulation sim = new KyokiSimulation();

        /// <summary>
        /// 年毎のパターン変化数一覧に月運を考慮するかどうかのフラグ
        /// </summary>
        private const bool bReflectGetuunToYearList = false;

        public event CloseHandler OnClose;

        public FromKyokiSimulation( Form1 parent)
        {
            InitializeComponent();

            frmParent = parent;

         }

        private void FromKyokiSimulation_Load(object sender, EventArgs e)
        {
        
        }
        public void UpdateAll(Person _person, int year,
                            Kansi _getuunKansi, Kansi _nenunKansi, Kansi _taiunKansi,
                            bool _bDispGetuun,
                            bool _bDispSangouKaikyoku,
                            bool _bDispGogyou,
                            bool _bDispGotoku,
                            bool _bDispRefrectGouhou,
                            bool _bDispRefrectSangouKaiyoku
                            )
        {
            //----------------------------------------------------------
            //年毎の変化数リスト表示
            //----------------------------------------------------------
            //月運は年リストに考慮しない
            UpdateKyokiPatternCountForYear(_person, bReflectGetuunToYearList);

            //----------------------------------------------------------
            //虚気変化パターン表示
            //----------------------------------------------------------
            UpdateKyokiPatternOnly( _person,  year,
                             _getuunKansi,  _nenunKansi,  _taiunKansi,
                             _bDispGetuun,
                             _bDispSangouKaikyoku,
                             _bDispGogyou,
                             _bDispGotoku,
                             _bDispRefrectGouhou,
                             _bDispRefrectSangouKaiyoku);


        }

        /// <summary>
        /// 虚気変化パターン表示
        /// </summary>
        /// <param name="_person">人情報</param>
        /// <param name="year">表示対象年</param>
        /// <param name="_getuunKansi">月運干支</param>
        /// <param name="_nenunKansi">年運干支</param>
        /// <param name="_taiunKansi">大運干支</param>
        /// <param name="_bDispGetuun">true...月運表示</param>
        /// <param name="_bDispSangouKaikyoku">true...三合会局・方三位表示（ライン描画はしないので現在は未使用）</param>
        /// <param name="_bDispGogyou">true...五行反映</param>
        /// <param name="_bDispGotoku">true... 五徳反映</param>
        /// <param name="_bDispRefrectGouhou">true...五行/五徳反映時の合法反映表示</param>
        /// <param name="_bDispRefrectSangouKaiyoku">true...五行/五徳反映時の三合会局反映表示</param>
        public void UpdateKyokiPatternOnly(Person _person, int year,
                            Kansi _getuunKansi, Kansi _nenunKansi, Kansi _taiunKansi,
                            bool _bDispGetuun,
                            bool _bDispSangouKaikyoku,
                            bool _bDispGogyou,
                            bool _bDispGotoku,
                            bool _bDispRefrectGouhou,
                            bool _bDispRefrectSangouKaiyoku)
        {

            //対象年を選択（リスト選択イベントは発生しません）
            SetYearList(year);

            sim.Simulation(_person, _getuunKansi, _nenunKansi, _taiunKansi, _bDispGetuun);


            this.SuspendLayout();
            this.DoubleBuffered = true;

            flowLayoutPanel1.Controls.Clear();
            //lstPictureBox.Clear();

            Person person = _person.Clone();

            int cnt = 0;

            int[] kansiBit = new int[] {
                Const.bitFlgGetuun, Const.bitFlgNenun, Const.bitFlgTaiun,
                Const.bitFlgNiti, Const.bitFlgGetu, Const.bitFlgNen
            };

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
                sc.FixedPanel = FixedPanel.Panel1;  //分割パネル左側を固定パネルにシチエ
                sc.FixedPanel = FixedPanel.Panel1;  //分割パネル左側を固定パネルにシチエ
                sc.Panel1MinSize =50;               //分割パネル左側の幅設定
                sc.Panel1.Controls.Add(lbl);        //分割パネル左側に回数ラベルを追加
                sc.Panel2.Controls.Add(pictureBox); //分割パネル右側に干支図表示ピクチャーボックスを追加
                sc.Width = 345;

                //フローレイアウトパネルに分割パネルを追加
                flowLayoutPanel1.Controls.Add(sc);
                
                person.nikkansi = pattern.aryKansi[(int)Const.enumKansiItemID.NIKKANSI].kansi;
                person.gekkansi = pattern.aryKansi[(int)Const.enumKansiItemID.GEKKANSI].kansi;
                person.nenkansi = pattern.aryKansi[(int)Const.enumKansiItemID.NENKANSI].kansi;



                //----------------------------------------------------------
                //干支情報の干で変化している箇所のビットを設定
                //----------------------------------------------------------
                //干の変化位置 管理ビット情報
                int ChangeKansiBit = 0;

                for (int i=0; i< pattern.aryKansi.Length; i++)
                {
                    if (pattern.aryKansi[i].bChange )
                    {
                        //aryKansiの並びは、kansiBitの定義の並びになっている
                        //変化のあった箇所のビットをON
                        ChangeKansiBit |= kansiBit[i];
                    }
                }


                //----------------------------------------------------------
                //後天運 表示
                //----------------------------------------------------------
                DrawKoutenUn drawItem2 = null;
                drawItem2 = new DrawKoutenUn(person, pictureBox, 
                                        pattern.aryKansi[(int)Const.enumKansiItemID.TAIUN].kansi,
                                        pattern.aryKansi[(int)Const.enumKansiItemID.NENUN].kansi,
                                        pattern.aryKansi[(int)Const.enumKansiItemID.GETUUN].kansi,
                                        _bDispGetuun,
                                        _bDispSangouKaikyoku,
                                        _bDispGogyou,
                                        _bDispGotoku,
                                        _bDispRefrectGouhou,
                                        _bDispRefrectSangouKaiyoku
                                        );
                drawItem2.CalcCoord(0);
                //虚気変化パターン表示用の描画関数呼び出し
                drawItem2.DrawKyokiPattern(ChangeKansiBit);
            }
            this.ResumeLayout();

        }


        public void UpdateKyokiPatternYearList(Person _person)
        {
            UpdateKyokiPatternCountForYear(_person, bReflectGetuunToYearList);

        }


        private void SetYearList( int year)
        {
            //現在選択されている年と同じなら何もしない
            if (lvPatternNum.SelectedItems.Count>0)
            {
                if (lvPatternNum.SelectedItems[0].Text == year.ToString())
                {
                    return;
                }
            }

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

                int rc = sim.Simulation(_person, getuunKansi, nenunKansi, taiunKansi, _bDispGetuun);

                int patternNum = sim.lstKansPattern.Count;

                var lvItem= lvPatternNum.Items.Add(string.Format("{0}", year));
                lvItem.SubItems.Add(string.Format("{0}", patternNum-1));

                if(rc==1)
                {
                    lvItem.BackColor = Color.LightYellow;
                }

            }

        }

        private void lvPatternNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            var items = lvPatternNum.SelectedItems;
            if (items.Count == 0) return;

            int year = int.Parse(items[0].Text);
            frmParent.UpdateNeunTaiunDisp(year);
        }

        private void FromKyokiSimulation_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnClose?.Invoke();
        }
    }
}
