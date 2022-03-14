using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace WinFormsApp2
{
    public partial class FormOption : Form
    {
        public FormOption()
        {
            InitializeComponent();
        }

        private void FormOption_Load(object sender, EventArgs e)
        {
            LoadSetting();
        }

        private void LoadSetting()
        {
            try
            {
                 Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                SetInitCheckBox(config, "DispToday", chkDispToday);
                SetInitCheckBox(config, "Getuun", chkDispGetuun);
                SetInitCheckBox(config, "Nenun", chkDispNenun);
                SetInitCheckBox(config, "Taiun", chkDispTaiun);
                SetInitCheckBox(config, "SangouKaikyoku", chkSangouKaikyoku);
                SetInitCheckBox(config, "Gogyou", chkGogyou);
                SetInitCheckBox(config, "Gotoku", chkGotoku);

            }
            finally
            {
            }


        }

        public void SetInitCheckBox(Configuration config, string keyName, CheckBox chk)
        {
            string sValue = config.AppSettings.Settings[keyName].Value;
            if (sValue != "")
            {
                chk.Checked = bool.Parse(sValue);
            }
        }
        public void GetInitCheckBox(Configuration config, string keyName, CheckBox chk)
        {
            config.AppSettings.Settings[keyName].Value = chk.Checked.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            
            GetInitCheckBox(config, "DispToday", chkDispToday);
            GetInitCheckBox(config, "Getuun", chkDispGetuun);
            GetInitCheckBox(config, "Nenun", chkDispNenun);
            GetInitCheckBox(config, "Taiun", chkDispTaiun);
            GetInitCheckBox(config, "SangouKaikyoku", chkSangouKaikyoku);
            GetInitCheckBox(config, "Gogyou", chkGogyou);
            GetInitCheckBox(config, "Gotoku", chkGotoku);

            config.Save();
            this.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkGogyou_CheckedChanged(object sender, EventArgs e)
        {
            chkGotoku.CheckedChanged -= chkGotoku_CheckedChanged;
            chkGotoku.Checked = false;
            chkGotoku.CheckedChanged += chkGotoku_CheckedChanged;
        }

        private void chkGotoku_CheckedChanged(object sender, EventArgs e)
        {
            chkGogyou.CheckedChanged -= chkGogyou_CheckedChanged;
            chkGogyou.Checked = false;
            chkGogyou.CheckedChanged += chkGogyou_CheckedChanged;
        }
    }




}
