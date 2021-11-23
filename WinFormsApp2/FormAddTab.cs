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
    public partial class FormAddTab : Form
    {
        public Person selectPerson = null;
        Persons personList;
        public FormAddTab(Persons persons)
        {
            InitializeComponent();
            personList = persons;
        }

        private void FormAddTab_Load(object sender, EventArgs e)
        {

            Common.SetGroupCombobox(personList, cmbGroup);

        }

        private void cmbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Person> persons = null;
            if (cmbGroup.SelectedIndex == 0)
            {
                //全て
                persons = personList.GetPersonList();
            }
            else
            {
                var item = (Group)cmbGroup.SelectedItem;
                persons = item.members;

            }
            cmbPerson.Items.Clear();
            for (int i = 0; i < persons.Count; i++)
            {
               cmbPerson.Items.Add(persons[i]);
            }
            if (cmbPerson.Items.Count > 0)
            {
                cmbPerson.SelectedIndex = 0;
            }

        }
        /// <summary>
        /// タブ追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            selectPerson = (Person) cmbPerson.SelectedItem;
            this.Close();
        }
    }
}
