using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using NPOI.XSSF.UserModel;

namespace WinFormsApp2
{
    public partial class FormExcelPictureTest : Form
    {
        
        public class ExplanationData
        {
            public ExplanationData(string _id, string _explanation, ExcelReader.PictureInfo _pictureInfo)
            {
                id = _id;
                explanation = _explanation;
                pictureInfos.Add( _pictureInfo );
            }
            public void AddPictureInfo(ExcelReader.PictureInfo _pictureInfo)
            {
                pictureInfos.Add(_pictureInfo);
            }
            public string id;
            public string explanation;
            public List<ExcelReader.PictureInfo> pictureInfos = new List<ExcelReader.PictureInfo>();
        }



        public List<ExcelReader.PictureInfo> lstCellInfos = null;

        public Dictionary<string, ExplanationData> dic = new Dictionary<string, ExplanationData>();




        public FormExcelPictureTest()
        {
            InitializeComponent();
        }

        private void FormExcelPictureTest_Load(object sender, EventArgs e)
        {
            string exePath = FormMain.GetExePath();

            var workbook = ExcelReader.GetWorkbook(exePath + @"\Contents.xlsx", "xlsx");
            XSSFSheet sheet = (XSSFSheet)((XSSFWorkbook)workbook).GetSheetAt(0);
            lstCellInfos = ExcelReader.GetPicture(sheet);

            int iRow = 0;
            while (true)
            {

                //ID
                string id = ExcelReader.CellValue(sheet, iRow, 0);
                if (string.IsNullOrEmpty(id)) break;

                string Explanation = ExcelReader.CellValue(sheet, iRow, 1);

                List<ExcelReader.PictureInfo> pictureInfos = lstCellInfos.FindAll(x => x.row == iRow);
                if (pictureInfos != null)
                {
                    foreach (var info in pictureInfos)
                    {
                        if (dic.ContainsKey(id))
                        {
                            dic[id].AddPictureInfo(info);
                        }
                        else
                        {
                            dic.Add(id, new ExplanationData(id, Explanation, info));
                        }
                    }
                }
                iRow++;
            }

            dataGridView1.Rows.Clear();

            foreach (var info in dic)
            {
                int idx = dataGridView1.Rows.Add();
                if(dataGridView1.ColumnCount< info.Value.pictureInfos.Count +2)
                {
                    for (int i = 0; i < info.Value.pictureInfos.Count+2- dataGridView1.ColumnCount; i++)
                    {

                        DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
                        imageColumn.Name = "Column1";
                        imageColumn.HeaderText = "画像";
                        imageColumn.Width = 300;
                        int n = dataGridView1.Columns.Add(imageColumn);
                    }
                }

                dataGridView1[0, idx].Value = info.Value.id;
                dataGridView1[1, idx].Value = info.Value.explanation;
                int iCol = 2;
                for (int i = 0; i < info.Value.pictureInfos.Count; i++)
                {
                    dataGridView1[iCol++, idx].Value = info.Value.pictureInfos[i].pictureData.Data;
                }
                // dataGridView1.Rows[idx].Height = info.Value.pictureInfo.height;
            }

            dataGridView1.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
        }

        //MemoryStream stmBLOBData = new MemoryStream(((FormMain)mainForm).lstCellInfos[0].pictureData.Data);
        //pictureBox4.Image = Image.FromStream(stmBLOBData);

    }
}
