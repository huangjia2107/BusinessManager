using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OfficeAccess.Excel
{
    public class XlsAccess
    {
        public static readonly XlsAccess xlsAccess = new XlsAccess();

        private string openFilePath = null;
        private HSSFWorkbook hssfworkbook = null;
        private ISheet sheet = null;

        private XlsAccess()
        {
        }

        public bool LoadXlsTemplate(string filePath)
        {
            if (!File.Exists(filePath))
                return false;

            openFilePath = filePath;
            try
            {
                using (FileStream file = new FileStream(openFilePath, FileMode.Open, FileAccess.Read))
                {
                    hssfworkbook = new HSSFWorkbook(file);
                }
                //create a entry of DocumentSummaryInformation
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "XXX Team";
                hssfworkbook.DocumentSummaryInformation = dsi;

                //create a entry of SummaryInformation
                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Subject = "Template Example";
                hssfworkbook.SummaryInformation = si;

                return true;
            }
            catch (Exception ex)
            {
                openFilePath = null;
                throw;
            }
        }

        public bool GetSheet(string sheetName)
        {
            if (hssfworkbook == null)
                return false;

            try
            {
                sheet = hssfworkbook.GetSheet(sheetName);
                return true;
            }
            catch { throw; }

        }

        public bool SetCellFormula(int rowNum, int cellNum, string formula)
        {
            if (sheet == null) return false;
            try
            {
                sheet.GetRow(rowNum).GetCell(cellNum).SetCellFormula(formula);
                return true;
            }
            catch { throw; }

        }

        public T GetCellValue<T>(int rowNum, int cellNum)
        {
            if (sheet == null) return default(T);

            try
            {
                Type type = typeof(T);
                if (type == typeof(double) || type == typeof(Double))
                    return (T)Convert.ChangeType(sheet.GetRow(rowNum).GetCell(cellNum).NumericCellValue, type);
                if (type == typeof(DateTime))
                    return (T)Convert.ChangeType(sheet.GetRow(rowNum).GetCell(cellNum).DateCellValue, type);
                if (type == typeof(string) || type == typeof(String))
                    return (T)Convert.ChangeType(sheet.GetRow(rowNum).GetCell(cellNum).StringCellValue, type);
                if (type == typeof(bool) || type == typeof(Boolean))
                    return (T)Convert.ChangeType(sheet.GetRow(rowNum).GetCell(cellNum).BooleanCellValue, type);
            }
            catch(Exception ex)
            {
                throw;
            }
            return default(T);
        }

        public bool SetCellValue(int rowNum, int cellNum, object value)
        {
            if (sheet == null || value == null) return false;

            try
            {
                Type type = value.GetType();
                if (type == typeof(double) || type == typeof(int) || type == typeof(float) || type == typeof(Double)
                    || type == typeof(Int16) || type == typeof(Int32) || type == typeof(Int64))
                    sheet.GetRow(rowNum).GetCell(cellNum).SetCellValue(Convert.ToDouble(value));
                if (type == typeof(byte) || type == typeof(Byte))
                    sheet.GetRow(rowNum).GetCell(cellNum).SetCellValue(Convert.ToByte(value));
                if (type == typeof(DateTime))
                    sheet.GetRow(rowNum).GetCell(cellNum).SetCellValue(Convert.ToDateTime(value));
                if (type == typeof(IRichTextString))
                    sheet.GetRow(rowNum).GetCell(cellNum).SetCellValue((IRichTextString)Convert.ChangeType(value, typeof(IRichTextString)));
                if (type == typeof(string) || type == typeof(String))
                    sheet.GetRow(rowNum).GetCell(cellNum).SetCellValue(Convert.ToString(value));
                if (type == typeof(bool) || type == typeof(Boolean))
                    sheet.GetRow(rowNum).GetCell(cellNum).SetCellValue(Convert.ToBoolean(value));
            }
            catch (Exception ex)
            {
                throw;
            }

            return true;
        }

        public bool SetPrintArea(string reference)
        {
            if (hssfworkbook == null)
                return false;

            try
            {
                hssfworkbook.SetPrintArea(0, reference);
                return true;
            }
            catch { throw; }
        }

        public bool SetPrintArea(int startColumn, int endColumn, int startRow, int endRow)
        {
            if (hssfworkbook == null)
                return false;

            try
            {
                hssfworkbook.SetPrintArea(0, startColumn, endColumn, startRow, endRow);
                return true;
            }
            catch { throw; }
        }
         
        public bool SaveAs(string filePath)
        {
            if (hssfworkbook == null)
                return false;

            try
            {
                using (FileStream file = new FileStream(filePath, FileMode.Create))
                {
                    hssfworkbook.Write(file);
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool Save()
        {
            if (hssfworkbook == null || string.IsNullOrEmpty(openFilePath))
                return false;

            try
            {
                using (FileStream file = new FileStream(openFilePath, FileMode.Create))
                {
                    hssfworkbook.Write(file);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 复制行格式并插入指定行数
        /// </summary>
        /// <param name="sheet">当前sheet</param>
        /// <param name="startRowIndex">起始行位置</param>
        /// <param name="sourceRowIndex">模板行位置</param>
        /// <param name="insertCount">插入行数</param>
        public void CopyRow(int startRowIndex, int sourceRowIndex, int insertCount)
        {
            IRow sourceRow = sheet.GetRow(sourceRowIndex);
            int sourceCellCount = sourceRow.Cells.Count;

            //1. 批量移动行,清空插入区域
            sheet.ShiftRows(startRowIndex, //开始行
                            sheet.LastRowNum, //结束行
                            insertCount, //插入行总数
                            true,        //是否复制行高
                            false        //是否重置行高
                            );

            int startMergeCell = -1; //记录每行的合并单元格起始位置
            for (int i = startRowIndex; i < startRowIndex + insertCount; i++)
            {
                IRow targetRow = null;
                ICell sourceCell = null;
                ICell targetCell = null;

                targetRow = sheet.CreateRow(i);
                targetRow.Height = sourceRow.Height;//复制行高

                for (int m = sourceRow.FirstCellNum; m < sourceRow.LastCellNum; m++)
                {
                    sourceCell = sourceRow.GetCell(m);
                    if (sourceCell == null)
                        continue;
                    targetCell = targetRow.CreateCell(m);
                    targetCell.CellStyle = sourceCell.CellStyle;//赋值单元格格式
                    targetCell.SetCellType(sourceCell.CellType);

                    //以下为复制模板行的单元格合并格式
                    if (sourceCell.IsMergedCell)
                    {
                        if (startMergeCell <= 0)
                            startMergeCell = m;
                        else if (startMergeCell > 0 && sourceCellCount == m + 1)
                        {
                            sheet.AddMergedRegion(new CellRangeAddress(i, i, startMergeCell, m));
                            startMergeCell = -1;
                        }
                    }
                    else
                    {
                        if (startMergeCell >= 0)
                        {
                            sheet.AddMergedRegion(new CellRangeAddress(i, i, startMergeCell, m - 1));
                            startMergeCell = -1;
                        }
                    }
                }
            }
        }
    }
}
