using HIB.Outlook.Helper.Common;
using HIB.Outlook.Sync.Common;
using Microsoft.Office.Interop.Excel;
using System;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace HIB.Outlook.Service
{
    public class ExportLogger
    {

        NPOI.SS.UserModel.IWorkbook WorkBook = null;
        public string[] EXPORT_DATATABLE_TO_EXCEL_XLS_USE_NPOI(System.Data.DataTable DTABLE, string SHEET_NAME, string FILE_EXCEL_XLS, bool EXPORT_HEADER = true, int Row_Begin = 1, int Column_Begin = 1)
        {

            string[] KQ = { "OK", "" };

            if (Row_Begin <= 0 || Column_Begin <= 0)
            {
                KQ[0] = "ERROR";
                KQ[1] = "REQUIRE ROW AND COLUMN BEGIN GREATER THAN 0";
                return KQ;
            }
            if (SHEET_NAME.Trim() == "")
            {
                KQ[0] = "ERROR";
                KQ[1] = "Sheet Name Cannot be Empty";
                return KQ;
            }
            if (FILE_EXCEL_XLS.Trim() == "")
            {
                KQ[0] = "ERROR";
                KQ[1] = "File Name Cannot be empty";
                return KQ;
            }

            try
            {
                System.IO.FileStream XFileExists = null;
                if (File.Exists(FILE_EXCEL_XLS.Trim()))
                {
                    XFileExists = new System.IO.FileStream(FILE_EXCEL_XLS.Trim(), System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
                    WorkBook = new NPOI.HSSF.UserModel.HSSFWorkbook(XFileExists);
                }
                else
                {
                    WorkBook = new NPOI.HSSF.UserModel.HSSFWorkbook();
                }

                int Column_Begin_TEMP = 0;
                NPOI.SS.UserModel.ISheet SHEET = null;
                NPOI.SS.UserModel.ISheet Existing_SHEET = WorkBook.GetSheet(SHEET_NAME.Trim());
                if (Existing_SHEET != null)
                {
                    SHEET = Existing_SHEET;
                    Column_Begin = 0;
                    Row_Begin = SHEET.LastRowNum + 1;
                }
                else
                {
                    SHEET = WorkBook.CreateSheet(SHEET_NAME.Trim());
                    Row_Begin = 0;
                    Column_Begin = 0;

                    Column_Begin_TEMP = Column_Begin;


                    if (EXPORT_HEADER == true)
                    {
                        Column_Begin_TEMP = Column_Begin;
                        NPOI.SS.UserModel.IRow Row = SHEET.CreateRow(Row_Begin);
                        for (int iCol = 0; iCol < DTABLE.Columns.Count; iCol++)
                        {
                            NPOI.SS.UserModel.ICell Cell = Row.CreateCell(Column_Begin_TEMP);
                            string ColumnName = DTABLE.Columns[iCol].ToString();
                            Cell.SetCellValue(ColumnName);
                            Column_Begin_TEMP += 1;
                        }
                        Row_Begin += 1;
                    }
                }

                for (int iRow = 0; iRow < DTABLE.Rows.Count; iRow++)
                {
                    Column_Begin_TEMP = Column_Begin;
                    NPOI.SS.UserModel.IRow Row = SHEET.CreateRow(Row_Begin);
                    for (int iCol = 0; iCol < DTABLE.Columns.Count; iCol++)
                    {
                        NPOI.SS.UserModel.ICell Cell = Row.CreateCell(Column_Begin_TEMP);
                        Cell.SetCellValue(DTABLE.Rows[iRow][iCol].ToString());
                        Column_Begin_TEMP += 1;
                    }
                    Row_Begin += 1;
                }
                System.IO.FileStream XFile = new System.IO.FileStream(FILE_EXCEL_XLS.Trim(), System.IO.FileMode.Create, System.IO.FileAccess.Write);
                WorkBook.Write(XFile);
                XFile.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                if (ex.Message.ToLower().IndexOf("the process cannot access the file") != -1 && ex.Message.ToLower().IndexOf("because it is being used by another process") != -1)
                {
                    KQ[0] = "ERROR";
                    KQ[1] = "the process cannot access the file because it is being used by another process";
                }
                else
                {
                    KQ[0] = "ERROR";
                    KQ[1] = ex.ToString();
                }
            }
            return KQ;
        }
    }
}
