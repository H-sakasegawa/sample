using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Windows.Forms;

//共通 NPOI
using NPOI;
using NPOI.SS.UserModel;
//標準xlsバージョン
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
//拡張xlsxバージョン
using NPOI.XSSF;
using NPOI.XSSF.UserModel;

namespace WinFormsApp2
{
    /// <summary>
    /// Excelファイル読み込み機能
    /// </summary>
    class ExcelReader
    {

        /// <summary>
        /// Workbook読み込む関数
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static IWorkbook GetWorkbook(string filename, string version)
        {
            try
            {
                // ファイルストリームを生成する。
                using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    // 標準エクセル.xls
                    if ("xls".Equals(version))
                    {
                        // 標準エクセルWorkbookを割当
                        return new HSSFWorkbook(stream);
                    }
                    // 拡張エクセル.xlsx
                    else if ("xlsx".Equals(version))
                    {
                        // 拡張エクセルWorkbookを割当
                        return new XSSFWorkbook(stream);
                    }
                    // xlsとxlsxじゃなければ、エラー発生
                    throw new NotSupportedException();
                }
            }catch
            {
            }
            return null;
        }

        /// <summary>
        ///  シート(Sheet)から行を取得関数
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="rownum">行番号</param>
        /// <returns></returns>
        public static IRow GetRow(ISheet sheet, int rownum)
        {
            // シートから行を取得
            var row = sheet.GetRow(rownum);
            // 行がnullなら
            if (row == null)
            {
                // シートから行を生成する。
                row = sheet.CreateRow(rownum);
            }
            // 行をリターン
            return row;
        }
        /// <summary>
        /// .行から列を取得関数
        /// </summary>
        /// <param name="row">行番号</param>
        /// <param name="cellnum">セル番号</param>
        /// <returns></returns>
        public static ICell GetCell(IRow row, int cellnum)
        {
            // 行から列を取得
            var cell = row.GetCell(cellnum);
            // 列がnullなら
            if (cell == null)
            {
                // 行から列を生成する。
                cell = row.CreateCell(cellnum);
            }
            // 列をリターン
            return cell;
        }
        /// <summary>
        /// エクセルシート(Sheet)からセル取得関数(rownumは行、cellnumは列)
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rownum"></param>
        /// <param name="cellnum"></param>
        /// <returns></returns>
        public static ICell GetCell(ISheet sheet, int rownum, int cellnum)
        {
            // 行を取得
            var row = GetRow(sheet, rownum);
            // 行から列を取得
            return GetCell(row, cellnum);
        }
        /// <summary>
        /// エクセルのWorkbookをファイルに格納する関数
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="filepath"></param>
        public static void WriteExcel(IWorkbook workbook, string filepath)
        {
            try
            {
                // ファイルストリームを生成する。
                using (var file = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    // 格納する。
                    workbook.Write(file);
                }
            }
            catch
            {
                MessageBox.Show(string.Format("データの書き込みに失敗しました。\nファイルが書き込み不可となっています\n{0}", filepath) );
            }
        }

        /// <summary>
        /// 指定したセルの値を取得する
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="idxRow"></param>
        /// <param name="idxColumn"></param>
        /// <returns></returns>
        public static string CellValue(ISheet sheet, int idxRow, int idxColumn)
        {
            var row = sheet.GetRow(idxRow) ?? sheet.CreateRow(idxRow); //指定した行を取得できない時はエラーとならないよう新規作成している
            var cell = row.GetCell(idxColumn) ?? row.CreateCell(idxColumn); //一行上の処理の列版
            string value="";

            switch (cell.CellType)
            {
                case CellType.String:
                    value = cell.StringCellValue;
                    break;
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        value = cell.DateCellValue.ToString("yyyy/MM/dd HH:mm:ss");
                    }
                    else
                    {
                        value = cell.NumericCellValue.ToString();
                    }
                    break;
                case CellType.Boolean:
                    value = cell.BooleanCellValue.ToString();
                    break;
                case CellType.Formula:

                    switch (cell.CachedFormulaResultType)
                    {
                        case CellType.String:
                            value = cell.StringCellValue;
                            break;
                        case CellType.Numeric:
                            value = cell.NumericCellValue.ToString();
                            break;
                        case CellType.Boolean:
                            value = cell.BooleanCellValue.ToString();
                            break;
                    }
                    break;
                default:
                    value = "";
                    break;
            }
            return value;
        }

        public static bool CellBoolValue(ISheet sheet, int idxRow, int idxColumn, bool defaultValue=false)
        {
            string sFlg = ExcelReader.CellValue(sheet, idxRow, idxColumn);
            if (string.IsNullOrEmpty(sFlg))
            {
                return defaultValue;
            }
            if( sFlg.ToLower()=="true" || sFlg.ToLower()=="false")
            {
                return bool.Parse(sFlg);
            }
            return  Convert.ToBoolean( int.Parse(sFlg));
        }

        public static IWorkbook CreateWorkbook()
        {
            return new HSSFWorkbook();
        }
     }
}
