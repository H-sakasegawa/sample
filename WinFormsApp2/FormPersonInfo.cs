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
    public partial class FormPersonInfo : Form
    {
        public enum Mode
        {
            MODE_NEW=0,
            MODE_UPDATE
        }
        Mode mode;
        Persons personList;
        Person updatePerson;
        string initGroupName=null;

        public FormPersonInfo(Persons _personList, string _initGroupName, Mode _mode)
        {
            InitializeComponent();
            mode = _mode;
            personList = _personList;
            initGroupName =_initGroupName;
        }
        public FormPersonInfo(Persons _personList, Person person, Mode _mode)
        {
            InitializeComponent();
            mode = _mode;
            personList = _personList;
            updatePerson = person;
        }

        private void FormPersonInfo_Load(object sender, EventArgs e)
        {
            var groups = personList.GetGroups();
            foreach (var group in groups)
            {
                cmbGroup.Items.Add(group);
            }


            if ( mode == Mode.MODE_NEW )
            {
                //初期選択グループ名が指定されていない場合
                if (string.IsNullOrEmpty(initGroupName))
                {
                    if (cmbGroup.Items.Count > 0)
                    {
                        cmbGroup.SelectedIndex = 0;
                    }
                }
                else
                {
                    //初期選択グループ名が指定されている場合
                    cmbGroup.Text = initGroupName;
                }
            }
            else
            {
                cmbGroup.Text = updatePerson.group;
                txtName.Text = updatePerson.name;

                Birthday birthday = updatePerson.birthday;
                txtYear.Text = birthday.year.ToString();
                txtMonth.Text = birthday.month.ToString();
                txtDay.Text = birthday.day.ToString();
                if (updatePerson.gender == Gender.MAN) radMan.Checked = true;
                else radWoman.Checked = true;


            }


        }

        private void button1_Click(object sender, EventArgs e)
        {

            int year = 0;
            int month = 0;
            int day = 0;
            if (string.IsNullOrEmpty(cmbGroup.Text))
            {
                MessageBox.Show("入力値エラー(グループ)");
                return;
            }
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("入力値エラー(氏名)");
                return;
            }
            if (!int.TryParse(txtYear.Text, out year))
            {
                MessageBox.Show("入力値エラー(年)");
                return;
            }
            if (!int.TryParse(txtMonth.Text, out month))
            {
                MessageBox.Show("入力値エラー(月)");
                return;
            }
            if (!int.TryParse(txtDay.Text, out day))
            {
                MessageBox.Show("入力値エラー(日)");
                return;
            }

            Person person = null;
            if (mode == Mode.MODE_UPDATE)
            {
                //更新
                person = updatePerson;
                updatePerson.name = txtName.Text;
                updatePerson.birthday.SetBirthday(year, month, day);
                updatePerson.gender = (radMan.Checked ? Gender.MAN : Gender.WOMAN);
                updatePerson.group = cmbGroup.Text;

                //強制初期化
                updatePerson.Init2(true);
            }
            else
            {
                //追加登録
                Birthday birthday = new Birthday(year, month, day);
                Gender gender = (radMan.Checked ? Gender.MAN : Gender.WOMAN);
                person = new Person(
                                        txtName.Text, birthday, gender, cmbGroup.Text
                                        );
                personList.Add(person);
            }
            //グループディクショナリ
            personList.Add(person.group, person);

            //ファイル書き出し
            personList.WritePersonList();

            DialogResult = DialogResult.OK;
            this.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
