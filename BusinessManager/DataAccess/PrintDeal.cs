using BusinessManager.Helps;
using BusinessManager.Models;
using DataAccess.Models;
using OfficeAccess.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Forms;

namespace BusinessManager.DataAccess
{
    public class PrintDeal
    {
        public static PrintDeal _printDeal = null;

        private XlsAccess xlsAccess;
        private PrintDeal()
        {
            xlsAccess = XlsAccess.xlsAccess;

            if (!Directory.Exists(ResourceMap.AppFolderPathHashtable[AppFolderPath.Templates] as string))
                Directory.CreateDirectory(ResourceMap.AppFolderPathHashtable[AppFolderPath.Templates] as string);

            if (!Directory.Exists(ResourceMap.AppFolderPathHashtable[AppFolderPath.Outputs] as string))
                Directory.CreateDirectory(ResourceMap.AppFolderPathHashtable[AppFolderPath.Outputs] as string);
        }

        public static PrintDeal CreateInstance()
        {
            if (_printDeal == null)
            {
                _printDeal = new PrintDeal();
            }
            return _printDeal;
        }

        private bool SaveWithDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.AddExtension = true;
            saveFileDialog.Title = "请选择导出路径";
            saveFileDialog.Filter = "Excel Files(*.xls)|*.xls";
            saveFileDialog.DefaultExt = "xls";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                xlsAccess.SaveAs(saveFileDialog.FileName);
                return true;
            }

            return false;
        }

        public bool PrintSalarySheet(SalaryInfo salaryInfo, SalaryProcedureInfoWrap salaryProcedureInfoWrap, ref string msg)
        {
            if (salaryInfo == null || salaryProcedureInfoWrap == null)
                return false;
            try
            {
                if (!xlsAccess.LoadXlsTemplate(ResourceMap.AppFolderPathHashtable[AppFolderPath.Templates] as string + ResourceMap.TemplatesNameHashtable[TemplatesName.Salary] as string))
                    return false;

                if (xlsAccess.GetSheet((ResourceMap.TemplatesNameHashtable[TemplatesName.Salary] as string).Split('.')[0]))
                {
                    if (salaryProcedureInfoWrap.SalaryProcedureInfoList.Count > 1)
                        xlsAccess.CopyRow(11, 10, salaryProcedureInfoWrap.SalaryProcedureInfoList.Count - 1);

                    xlsAccess.SetCellValue(1, 1, salaryInfo.EmployeeName);
                    xlsAccess.SetCellValue(1, 4, salaryInfo.Year + " 年 " + salaryInfo.Month + " 月");
                    xlsAccess.SetCellValue(2, 1, salaryInfo.EmployeeSex);
                    xlsAccess.SetCellValue(2, 4, salaryInfo.DepartmentName);
                    xlsAccess.SetCellValue(3, 1, salaryInfo.BasicSalary);
                    xlsAccess.SetCellValue(3, 4, salaryInfo.MealSupplement);
                    xlsAccess.SetCellValue(4, 1, salaryInfo.SSB);
                    xlsAccess.SetCellValue(4, 4, salaryInfo.OtherBenefits);
                    xlsAccess.SetCellValue(5, 1, salaryInfo.Bounty);
                    xlsAccess.SetCellValue(5, 4, salaryInfo.OtherDeduction);
                    xlsAccess.SetCellValue(6, 1, salaryInfo.Commission);
                    xlsAccess.SetCellValue(6, 4, salaryInfo.NetPayroll);
                    xlsAccess.SetCellValue(7, 1, salaryInfo.Remark == null ? string.Empty : salaryInfo.Remark);

                    for (int index = 0; index < salaryProcedureInfoWrap.SalaryProcedureInfoList.Count; index++)
                    {
                        xlsAccess.SetCellValue(index + 10, 0, salaryProcedureInfoWrap.SalaryProcedureInfoList[index].SheetID);
                        xlsAccess.SetCellValue(index + 10, 1, salaryProcedureInfoWrap.SalaryProcedureInfoList[index].SettleDate.ToString("yyyy-MM-dd"));
                        xlsAccess.SetCellValue(index + 10, 2, salaryProcedureInfoWrap.SalaryProcedureInfoList[index].BSType == null ? "" : salaryProcedureInfoWrap.SalaryProcedureInfoList[index].BSType);
                        xlsAccess.SetCellValue(index + 10, 3, salaryProcedureInfoWrap.SalaryProcedureInfoList[index].Customer == null ? "" : salaryProcedureInfoWrap.SalaryProcedureInfoList[index].Customer);
                        xlsAccess.SetCellValue(index + 10, 4, salaryProcedureInfoWrap.SalaryProcedureInfoList[index].ProcedureName == null ? "" : salaryProcedureInfoWrap.SalaryProcedureInfoList[index].ProcedureName);
                        xlsAccess.SetCellValue(index + 10, 5, salaryProcedureInfoWrap.SalaryProcedureInfoList[index].Price);
                    }

                    //xlsAccess.SetPrintArea("A1:F12");
                    /*
                    xlsAccess.SaveAs(ResourceMap.AppFolderPathHashtable[AppFolderPath.Outputs] as string + salaryInfo.EmployeeName + "_" + salaryInfo.DepartmentName + "_" + salaryInfo.Year + "年" + salaryInfo.Month + "月_工资单.xls");
                     */
                    return SaveWithDialog();
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace.ToString();
                return false;
            }
        }

        public bool PrintSheetLeftSheet(string searchKeys, List<SheetInfo> sheetList, ref string msg)
        {
            if (sheetList == null) return false;
            if (sheetList.Count == 0) return false;

            try
            {
                if (!xlsAccess.LoadXlsTemplate(ResourceMap.AppFolderPathHashtable[AppFolderPath.Templates] as string + ResourceMap.TemplatesNameHashtable[TemplatesName.SheetLeft] as string))
                    return false;

                if (xlsAccess.GetSheet((ResourceMap.TemplatesNameHashtable[TemplatesName.SheetLeft] as string).Split('.')[0]))
                {
                    //搜索条件
                    xlsAccess.SetCellValue(0, 2, string.IsNullOrEmpty(searchKeys) ? "无" : searchKeys);
                    if (sheetList.Count > 1)
                        xlsAccess.CopyRow(3, 2, sheetList.Count - 1);

                    List<SheetBOPInfo> CurSheetBopList = null;
                    for (int index = 0; index < sheetList.Count; index++)
                    {
                        xlsAccess.SetCellValue(index + 2, 0, sheetList[index].ID);
                        xlsAccess.SetCellValue(index + 2, 1, sheetList[index].StartDate.ToString("yyyy-MM-dd"));
                        xlsAccess.SetCellValue(index + 2, 2, sheetList[index].FinishDate == null ? "" : ((DateTime)(sheetList[index].FinishDate)).ToString("yyyy-MM-dd"));
                        xlsAccess.SetCellValue(index + 2, 3, sheetList[index].SettleDate == null ? "" : ((DateTime)(sheetList[index].SettleDate)).ToString("yyyy-MM-dd"));
                        xlsAccess.SetCellValue(index + 2, 4, sheetList[index].Accepter == null ? "" : sheetList[index].Accepter);
                        xlsAccess.SetCellValue(index + 2, 5, sheetList[index].Customer == null ? "" : sheetList[index].Customer);
                        xlsAccess.SetCellValue(index + 2, 6, sheetList[index].BSType == null ? "" : sheetList[index].BSType);
                        xlsAccess.SetCellValue(index + 2, 7, sheetList[index].Price);

                        double totalBOP = sheetList[index].Price;
                        CurSheetBopList = SheetDeal.GetAllSheetBOPInfo(sheetList[index].ID).ToList();
                        CurSheetBopList.ForEach(info => totalBOP += (info.Balance - info.Payment));
                        xlsAccess.SetCellValue(index + 2, 8, totalBOP);
                    }

                    if (sheetList.Count != 0)
                    {
                        xlsAccess.SetCellFormula(2 + sheetList.Count, 8, "sum(I3:I" + (2 + sheetList.Count) + ")");
                    }

                    return SaveWithDialog();
                    //xlsAccess.SaveAs(ResourceMap.AppFolderPathHashtable[AppFolderPath.Outputs] as string + "SheetDetail_Left.xls");
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace.ToString();
                return false;
            }
        }

        public bool PrintDebitNote(SheetInfo sheetInfo, List<SheetBOPInfo> sheetBOPList, ref string msg)
        {
            if (sheetInfo == null) return false;

            try
            {
                if (!xlsAccess.LoadXlsTemplate(ResourceMap.AppFolderPathHashtable[AppFolderPath.Templates] as string + ResourceMap.TemplatesNameHashtable[TemplatesName.DebitNote] as string))
                    return false;

                if (xlsAccess.GetSheet((ResourceMap.TemplatesNameHashtable[TemplatesName.DebitNote] as string).Split('.')[0]))
                {
                    xlsAccess.SetCellValue(2, 0, "TO:" + (sheetInfo.Customer == null ? "" : sheetInfo.Customer));
                    xlsAccess.SetCellValue(2, 3, "DATE:" + DateTime.Now.ToString("yyyy-MM-dd"));
                    xlsAccess.SetCellValue(4, 0, sheetInfo.BSType == null ? "" : sheetInfo.BSType);
                    xlsAccess.SetCellValue(4, 1, "RMB");
                    xlsAccess.SetCellValue(4, 2, sheetInfo.Price);

                    if (sheetBOPList.Count > 0)
                    {
                        List<SheetBOPInfo> tempList = sheetBOPList.Where(info => info.CanRecycle == true).ToList();
                        if (tempList.Count > 0)
                        {
                            xlsAccess.CopyRow(5, 4, tempList.Count);
                            for (int index = 0; index < tempList.Count; index++)
                            {
                                xlsAccess.SetCellValue(index + 5, 0, tempList[index].BOPReason);
                                xlsAccess.SetCellValue(index + 5, 1, "RMB");
                                xlsAccess.SetCellValue(index + 5, 2, tempList[index].Payment);
                            }
                            xlsAccess.SetCellFormula(5 + tempList.Count, 2, "sum(C5:C" + (5 + tempList.Count) + ")");
                        }
                        else
                            xlsAccess.SetCellValue(5, 2, sheetInfo.Price);
                    }
                    else
                        xlsAccess.SetCellValue(5, 2, sheetInfo.Price);

                    return SaveWithDialog();
                    //xlsAccess.SaveAs(ResourceMap.AppFolderPathHashtable[AppFolderPath.Outputs] as string + sheetInfo.Customer + "_DebitNote.xls");
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace.ToString();
                return false;
            }
        }

        public bool PrintSheetRightSheet(SheetInfo sheetInfo, List<ProcedureInfo> procedureList, List<SheetBOPInfo> sheetBOPList, ref string msg)
        {
            if (sheetInfo == null) return false;

            try
            {
                if (!xlsAccess.LoadXlsTemplate(ResourceMap.AppFolderPathHashtable[AppFolderPath.Templates] as string + ResourceMap.TemplatesNameHashtable[TemplatesName.SheetRight] as string))
                    return false;

                if (xlsAccess.GetSheet((ResourceMap.TemplatesNameHashtable[TemplatesName.SheetRight] as string).Split('.')[0]))
                {
                    xlsAccess.SetCellValue(1, 1, sheetInfo.StartDate.ToString("yyyy-MM-dd"));
                    xlsAccess.SetCellValue(1, 3, sheetInfo.FinishDate == null ? "" : ((DateTime)(sheetInfo.FinishDate)).ToString("yyyy-MM-dd"));
                    xlsAccess.SetCellValue(1, 5, sheetInfo.SettleDate == null ? "" : ((DateTime)(sheetInfo.SettleDate)).ToString("yyyy-MM-dd"));
                    xlsAccess.SetCellValue(2, 1, sheetInfo.ID);
                    xlsAccess.SetCellValue(2, 4, sheetInfo.From == null ? "" : sheetInfo.From);
                    xlsAccess.SetCellValue(3, 1, sheetInfo.Customer == null ? "" : sheetInfo.Customer);
                    xlsAccess.SetCellValue(4, 1, sheetInfo.BSType == null ? "" : sheetInfo.BSType);
                    xlsAccess.SetCellValue(4, 4, sheetInfo.Price);
                    xlsAccess.SetCellValue(5, 1, sheetInfo.Contacter == null ? "" : sheetInfo.Contacter);
                    xlsAccess.SetCellValue(5, 4, sheetInfo.PhoneNumber == null ? "" : sheetInfo.PhoneNumber);
                    xlsAccess.SetCellValue(6, 1, sheetInfo.Accepter == null ? "" : sheetInfo.Accepter);
                    xlsAccess.SetCellValue(6, 4, sheetInfo.Follower == null ? "" : sheetInfo.Follower);
                    xlsAccess.SetCellValue(7, 1, sheetInfo.Remark == null ? "" : sheetInfo.Remark);

                    if (procedureList.Count > 1)
                        xlsAccess.CopyRow(11, 10, procedureList.Count - 1);

                    for (int procedureIndex = 0; procedureIndex < procedureList.Count; procedureIndex++)
                    {
                        xlsAccess.SetCellValue(procedureIndex + 10, 0, procedureList[procedureIndex].CompleteDate.ToString("yyyy-MM-dd"));
                        xlsAccess.SetCellValue(procedureIndex + 10, 1, procedureList[procedureIndex].ProcedureName == null ? "" : procedureList[procedureIndex].ProcedureName);
                        xlsAccess.SetCellValue(procedureIndex + 10, 3, procedureList[procedureIndex].EmployeeName == null ? "" : procedureList[procedureIndex].EmployeeName);
                        xlsAccess.SetCellValue(procedureIndex + 10, 4, procedureList[procedureIndex].Price);
                        xlsAccess.SetCellValue(procedureIndex + 10, 5, procedureList[procedureIndex].Remark == null ? "" : procedureList[procedureIndex].Remark);
                    }

                    if (sheetBOPList.Count > 0)
                    {
                        if (sheetBOPList.Count > 1)
                            xlsAccess.CopyRow(11 + procedureList.Count + 2, 10 + procedureList.Count + 2, sheetBOPList.Count - 1);

                        for (int sheetBOPIndex = 0; sheetBOPIndex < sheetBOPList.Count; sheetBOPIndex++)
                        {
                            int curRowIndex = sheetBOPIndex + 10 + procedureList.Count + 2;
                            xlsAccess.SetCellValue(curRowIndex, 0, sheetBOPList[sheetBOPIndex].BOPDate.ToString("yyyy-MM-dd"));
                            xlsAccess.SetCellValue(curRowIndex, 1, (sheetBOPList[sheetBOPIndex].BOPReason == null ? "" : sheetBOPList[sheetBOPIndex].BOPReason) + (sheetBOPList[sheetBOPIndex].CanRecycle == true ? "（是）" : "（否）"));
                            xlsAccess.SetCellValue(curRowIndex, 3, sheetBOPList[sheetBOPIndex].Balance);
                            xlsAccess.SetCellValue(curRowIndex, 4, sheetBOPList[sheetBOPIndex].Payment);

                            //xlsAccess.SetCellFormula(curRowNum, 4, "sum(D" + (curRowNum + 1) + "," + "E" + (curRowNum + 1)+")");
                            xlsAccess.SetCellValue(curRowIndex, 5, sheetBOPList[sheetBOPIndex].Balance - sheetBOPList[sheetBOPIndex].Payment);
                        }

                        xlsAccess.SetCellFormula(10 + procedureList.Count + sheetBOPList.Count + 2, 3, "sum(D" + (10 + procedureList.Count + 2 + 1) + ":" + "D" + (10 + procedureList.Count + sheetBOPList.Count + 2) + ")");

                        xlsAccess.SetCellFormula(10 + procedureList.Count + sheetBOPList.Count + 2, 4, "sum(E" + (10 + procedureList.Count + 2 + 1) + ":" + "E" + (10 + procedureList.Count + sheetBOPList.Count + 2) + ")");

                        //                     double valueSum=xlsAccess.GetCellValue<double>(10 + procedureList.Count + sheetBOPList.Count + 2,1)+xlsAccess.GetCellValue<double>(10 + procedureList.Count + sheetBOPList.Count + 2,2);
                        //                     xlsAccess.SetCellValue(10 + procedureList.Count + sheetBOPList.Count + 2, 3,valueSum);

                        xlsAccess.SetCellFormula(10 + procedureList.Count + sheetBOPList.Count + 2, 5, "sum(F" + (10 + procedureList.Count + 2 + 1) + ":" + "F" + (10 + procedureList.Count + sheetBOPList.Count + 2) + ")");
                    }

                    return SaveWithDialog();
                    //xlsAccess.SaveAs(ResourceMap.AppFolderPathHashtable[AppFolderPath.Outputs] as string + sheetInfo.ID + "_SheetDetail_Right.xls");
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace.ToString();
                return false;
            }
        }

        public bool PrintBalance(string searchKeys, List<BOPInfo> bopList, ref string msg)
        {
            if (bopList == null) return false;
            if (bopList.Count == 0) return false;

            try
            {
                if (!xlsAccess.LoadXlsTemplate(ResourceMap.AppFolderPathHashtable[AppFolderPath.Templates] as string + ResourceMap.TemplatesNameHashtable[TemplatesName.Balance] as string))
                    return false;

                if (xlsAccess.GetSheet((ResourceMap.TemplatesNameHashtable[TemplatesName.Balance] as string).Split('.')[0]))
                {
                    //搜索条件
                    xlsAccess.SetCellValue(0, 2, string.IsNullOrEmpty(searchKeys) ? "无" : searchKeys);
                    if (bopList.Count > 1)
                        xlsAccess.CopyRow(3, 2, bopList.Count - 1);

                    for (int index = 0; index < bopList.Count; index++)
                    {
                        xlsAccess.SetCellValue(index + 2, 0, bopList[index].BOPDate.ToString("yyyy-MM-dd"));
                        xlsAccess.SetCellValue(index + 2, 1, bopList[index].BOPReason == null ? "" : bopList[index].BOPReason);
                        xlsAccess.SetCellValue(index + 2, 2, bopList[index].Balance);
                        xlsAccess.SetCellValue(index + 2, 3, bopList[index].AccountingSupervisorName == null ? "" : bopList[index].AccountingSupervisorName);
                        xlsAccess.SetCellValue(index + 2, 4, bopList[index].CashierName == null ? "" : bopList[index].CashierName);
                        xlsAccess.SetCellValue(index + 2, 5, bopList[index].HandlerName == null ? "" : bopList[index].HandlerName);
                        xlsAccess.SetCellValue(index + 2, 6, bopList[index].BookKeepingName == null ? "" : bopList[index].BookKeepingName);
                        xlsAccess.SetCellValue(index + 2, 7, bopList[index].Remark == null ? "" : bopList[index].Remark);
                    }

                    if (bopList.Count != 0)
                    {
                        xlsAccess.SetCellFormula(2 + bopList.Count, 2, "sum(C3:C" + (2 + bopList.Count) + ")");
                    }

                    return SaveWithDialog();
                    //xlsAccess.SaveAs(ResourceMap.AppFolderPathHashtable[AppFolderPath.Outputs] as string + "收入清单.xls");
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace.ToString();
                return false;
            }
        }

        public bool PrintPayment(string searchKeys, List<BOPInfo> bopList, ref string msg)
        {
            if (bopList == null) return false;
            if (bopList.Count == 0) return false;

            try
            {
                if (!xlsAccess.LoadXlsTemplate(ResourceMap.AppFolderPathHashtable[AppFolderPath.Templates] as string + ResourceMap.TemplatesNameHashtable[TemplatesName.Payment] as string))
                    return false;

                if (xlsAccess.GetSheet((ResourceMap.TemplatesNameHashtable[TemplatesName.Payment] as string).Split('.')[0]))
                {
                    //搜索条件
                    xlsAccess.SetCellValue(0, 2, string.IsNullOrEmpty(searchKeys) ? "无" : searchKeys);
                    if (bopList.Count > 1)
                        xlsAccess.CopyRow(3, 2, bopList.Count - 1);

                    for (int index = 0; index < bopList.Count; index++)
                    {
                        xlsAccess.SetCellValue(index + 2, 0, bopList[index].BOPDate.ToString("yyyy-MM-dd"));
                        xlsAccess.SetCellValue(index + 2, 1, bopList[index].BOPReason == null ? "" : bopList[index].BOPReason);
                        xlsAccess.SetCellValue(index + 2, 2, bopList[index].Payment);
                        xlsAccess.SetCellValue(index + 2, 3, bopList[index].DepartmentName == null ? "" : bopList[index].DepartmentName);
                        xlsAccess.SetCellValue(index + 2, 4, bopList[index].AccountingSupervisorName == null ? "" : bopList[index].AccountingSupervisorName);
                        xlsAccess.SetCellValue(index + 2, 5, bopList[index].CashierName == null ? "" : bopList[index].CashierName);
                        xlsAccess.SetCellValue(index + 2, 6, bopList[index].HandlerName == null ? "" : bopList[index].HandlerName);
                        xlsAccess.SetCellValue(index + 2, 7, bopList[index].PayeeName == null ? "" : bopList[index].PayeeName);
                        xlsAccess.SetCellValue(index + 2, 8, bopList[index].Remark == null ? "" : bopList[index].Remark);
                    }

                    if (bopList.Count != 0)
                    {
                        xlsAccess.SetCellFormula(2 + bopList.Count, 2, "sum(C3:C" + (2 + bopList.Count) + ")");
                    }

                    return SaveWithDialog();
                    //xlsAccess.SaveAs(ResourceMap.AppFolderPathHashtable[AppFolderPath.Outputs] as string + "支出清单.xls");
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace.ToString();
                return false;
            }
        }
    }
}
