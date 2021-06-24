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
    /// <summary>
    /// 経歴編集画面
    /// </summary>
    public partial class EditCareer : Form
    {

        public EditCareer(int _year, Person _person)
        {
            InitializeComponent();

            person = _person;
            year = _year;

            this.ActiveControl = txtCareer;

        }

        Person person;
        int year;

        private void EditCareer_Load(object sender, EventArgs e)
        {
            lblYear.Text = year.ToString();

            txtCareer.Text = person.career[year];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            person.career[year] = txtCareer.Text;
            person.career.Save();
        }
    }
}
