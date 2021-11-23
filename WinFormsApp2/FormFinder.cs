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
    public partial class FormFinder : Form
    {
        class FindResultItem : LvItemDataBase
        {
            public FindResultItem(Finder.FindItem item )
            {
                findItem = item;
            }
            public Finder.FindItem findItem;
        }

        FormMain parentForm = null;
        TableMng tblMng;
        Persons personList;

        Person curPerson = null;
        Group curGroup = null;

        public FormFinder(FormMain _parentForm, Group group, Person person)
        {
            InitializeComponent();

            parentForm = _parentForm;
            curPerson = person;
            curGroup = group;
        }

        private void FormSerch_Load( object sender, EventArgs e)
        {
            tblMng = TableMng.GetTblManage();
            personList = Persons.GetPersons();

            var groups = personList.GetGroups();


            Common.SetGroupCombobox(personList, cmbGroup, curGroup.groupName);

            radNattin.Checked = true;

        }

        /// <summary>
        /// 検索開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, EventArgs e)
        {
            btnFind.Enabled = false;
            lblStatus.Text = "";

            try
            {
                lstFindResult.Columns.Clear();
                lstFindResult.Items.Clear();

                Finder finder = new Finder();

                //★★暫定：メインフォームで氏名を選択しないとtblMngで初期化されないので
                //すでに初期化済みは内部で処理がSKIPされます。
                curPerson.Init(tblMng);

                Finder.FindResult result = null;
                if (radNattin.Checked || radRittin.Checked)
                {
                    //納音、準納音
                    //律音、準律音
                    int mode = -1;
                    if (radNattin.Checked) mode = 0;
                    if (radRittin.Checked) mode = 1;

                    result = finder.FindNattinOrRittin(curPerson, mode, chkTenchusatu.Checked);
                    if (!result.IsFinded()) return;

                }
                else if (radKyakkaHoukai.Checked)
                {   //却下崩壊
                    result = finder.FindKyakkaHoukai(curPerson);
                    if (!result.IsFinded()) return;

                }

                if (result == null)
                {
                    lblStatus.Text = string.Format("該当項目は見つかりませんでした。");
                    return;
                }

                lblStatus.Text = string.Format("{0}件 見つかりました。", result.lstFindItems.Count);

                //表示カラム設定
                foreach (var fmt in result.lstFormat)
                {
                    var colHeader = lstFindResult.Columns.Add(fmt.title, fmt.columnWidth);
                    colHeader.Tag = fmt.type;
                    colHeader.TextAlign = fmt.alignment;

                }

                foreach (var item in result.lstFindItems)
                {

                    ListViewItem lvItem = null;
                    for (int i = 0; i < item.lstItem.Count; i++)
                    {
                        if (i == 0)
                        {
                            lvItem = lstFindResult.Items.Add(item.lstItem[i]);
                        }
                        else
                        {
                            lvItem.SubItems.Add(item.lstItem[i]);
                        }
                    }

                    lvItem.Tag = new FindResultItem(item); //FindResultItem

                }

            }
            finally
            {
                btnFind.Enabled = true;
            }
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
            foreach( var item in persons)
            {
                cmbPerson.Items.Add(item);
                if (item == curPerson)
                {
                    cmbPerson.SelectedIndex = cmbPerson.Items.Count - 1;
                }
            }
            if (cmbPerson.Items.Count > 0 && cmbPerson.SelectedIndex < 0)
            {
                cmbPerson.SelectedIndex = 0;
            }

        }

        private void cmbPeoson_SelectedIndexChanged(object sender, EventArgs e)
        {
            curPerson =(Person)cmbPerson.SelectedItem;
            lstFindResult.Items.Clear();
        }

        /// <summary>
        /// 検索結果に大運、年運の情報を連動表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstFindResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFindResult.SelectedItems.Count <= 0) return;

            var lvItem = lstFindResult.SelectedItems[0];
            Finder.FindItem findItem = ((FindResultItem)lvItem.Tag).findItem;


            //YEARタイプカラムを検索
            int iColYear = GetColIndex(Finder.ResultFormat.Type.YEAR);
            int iColMonth = GetColIndex(Finder.ResultFormat.Type.MONTH);

            if( iColYear<0) return; //YEARなし

            int year = int.Parse(findItem.lstItem[iColYear]);
            int month = -1;
            if( iColMonth>=0 && !string.IsNullOrEmpty(findItem.lstItem[iColMonth]))
            {
                month = int.Parse(findItem.lstItem[iColMonth]);
                parentForm.SelectFindResult(findItem.person, year, month);
            }
            else
            {
                parentForm.SelectFindResult(findItem.person, year);
            }

           
        }
        /// <summary>
        /// 指定タイプデータが表示されているカラムIndex取得
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private int GetColIndex(Finder.ResultFormat.Type type)
        {
            int iCol;
            for (iCol = 0; iCol < lstFindResult.Columns.Count; iCol++)
            {
                ColumnHeader colHeader = lstFindResult.Columns[iCol];
                if ((Finder.ResultFormat.Type)colHeader.Tag == type)
                {
                    return iCol;
                }
            }
            return -1;
        }

        /// <summary>
        /// 現在の大運、年運の干の出現位置ににジャンプ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuJumpForKan_Click(object sender, EventArgs e)
        {
            int year = JumpForKansi(0);
            if (year < 0) return;

            parentForm.SelectFindResult(curPerson, year);
        }
        /// <summary>
        /// 現在の大運、年運の支の出現位置ににジャンプ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuJumpForSi_Click(object sender, EventArgs e)
        {
            int year = JumpForKansi(1);
            if (year < 0) return;
            parentForm.SelectFindResult(curPerson, year);
        }

        private int JumpForKansi(int kansiMode)
        {
            if (lstFindResult.SelectedItems.Count <= 0) return -1;

            var lvItem = lstFindResult.SelectedItems[0];
            Finder.FindItem findItem = ((FindResultItem)lvItem.Tag).findItem;
            int iColYear = GetColIndex(Finder.ResultFormat.Type.YEAR);
            int iColUn = GetColIndex(Finder.ResultFormat.Type.UN);

            Finder finder = new Finder();

            int year = int.Parse(findItem.lstItem[iColYear]);
            string un = findItem.lstItem[iColUn];

            //検索対象干支取得
            Kansi kansi;
            if (un == Const.sTaiun)
            {
                kansi = curPerson.GetTaiunKansi(year);
            }
            else if (un == Const.sNenun)
            {
                kansi = curPerson.GetNenkansi(year, true);
            }
            else
            {
                return -1;
            }
            if (kansiMode == 0)
            {
                return finder.FindNextKansi(curPerson, year, kansi.kan, 0);// 干
            }
            else
            {
                return finder.FindNextKansi(curPerson, year, kansi.si, 1);// 支
            }

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (lstFindResult.SelectedItems.Count <= 0) return;

            var lvItem = lstFindResult.SelectedItems[0];
            Finder.FindItem findItem = ((FindResultItem)lvItem.Tag).findItem;
            int iColUn = GetColIndex(Finder.ResultFormat.Type.UN);
            string un = findItem.lstItem[iColUn];

            if (un == Const.sGetuun)
            {
                mnuJumpForKan.Enabled = false;
                mnuJumpForSi.Enabled = false;
            }else
            {
                mnuJumpForKan.Enabled = true;
                mnuJumpForSi.Enabled = true;
            }

        }
        private void DispCtrl()
        {
            chkTenchusatu.Enabled = (radNattin.Checked || radRittin.Checked);
        }

        private void radNattin_CheckedChanged(object sender, EventArgs e)
        {
            DispCtrl();
        }

        private void radRittin_CheckedChanged(object sender, EventArgs e)
        {
            DispCtrl();
        }

        private void radKyakkaHoukai_CheckedChanged(object sender, EventArgs e)
        {
            DispCtrl();
        }
    }
}
