using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using NPOI.XSSF.UserModel;

namespace WinFormsApp2
{
    /// <summary>
    /// 説明用Excelファイルから画像データを管理提供するクラス
    /// </summary>
    class ExplanationReader
    {

        public class ExplanationData
        {
            public ExplanationData(string _id, ExcelReader.PictureInfo _pictureInfo)
            {
                id = _id;
                pictureInfos.Add(_pictureInfo);
            }
            public void AddPictureInfo(ExcelReader.PictureInfo _pictureInfo)
            {
                pictureInfos.Add(_pictureInfo);
            }
            public int GetMaxWidth()
            {
                int maxW = 0;
                foreach(var inf in pictureInfos)
                {
                    if (maxW < inf.width) maxW = inf.width;
                }
                return maxW;
            }
            public int GetMaxHeight()
            {
                int maxH = 0;
                foreach (var inf in pictureInfos)
                {
                    if (maxH < inf.height) maxH = inf.height;
                }
                return maxH;

            }
            public string id;
            public List<ExcelReader.PictureInfo> pictureInfos = new List<ExcelReader.PictureInfo>();
        }

        //Key:項目キー名
        private Dictionary<string, ExplanationData> dic = new Dictionary<string, ExplanationData>();


        public int ReadExcel(string excelFilePath)
        {
            if( !File.Exists(excelFilePath))
            {
                return -1;
            }


            string exePath = FormMain.GetExePath();

            var workbook = ExcelReader.GetWorkbook(excelFilePath, "xlsx");
            XSSFSheet sheet = (XSSFSheet)((XSSFWorkbook)workbook).GetSheetAt(0);
            List<ExcelReader.PictureInfo> lstCellInfos = ExcelReader.GetPicture(sheet);

            int iRow = 0;
            while (true)
            {

                //説明項目キー文字
                string sKeyItem = ExcelReader.CellValue(sheet, iRow, 0);
                if (string.IsNullOrEmpty(sKeyItem)) break;

                var sKeyAry = sKeyItem.Split('\n');
                //string Explanation = ExcelReader.CellValue(sheet, iRow, 1);

                List<ExcelReader.PictureInfo> pictureInfos = lstCellInfos.FindAll(x => x.row == iRow+1)
                                                                         .OrderBy(x=> x.col).ToList();
                foreach (var sKey in sKeyAry)
                {
                    if (pictureInfos != null && pictureInfos.Count>0)
                    {
                        foreach (var info in pictureInfos)
                        {
                            if (dic.ContainsKey(sKey))
                            {
                                dic[sKey].AddPictureInfo(info);
                            }
                            else
                            {
                                dic.Add(sKey, new ExplanationData(sKey, info));
                            }
                        }
                    }
                    else
                    {
                        dic.Add(sKey, new ExplanationData(sKey, null));
                    }
                }
                iRow++;
            }

            return 0;

        }
   
        public void Clear()
        {
            dic.Clear();
        }

        public ExplanationData GetExplanation(string sKey)
        {
            if (!dic.ContainsKey(sKey)) return null;
            return dic[sKey];
        }

        public List<string> GetExplanationKeys()
        {
            return dic.Keys.ToList();
        }
    }
}
