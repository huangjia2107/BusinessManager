using BusinessManager.DataAccess;
using DataAccess.Helps;
using DataAccess.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Data;

namespace BusinessManager.Helps
{
    public class AlgorithmClass
    {
        public static List<WorkStatusInfo> GetWorkStatusList()
        {
            return new List<WorkStatusInfo>()
            {
                new WorkStatusInfo{ID=0,StatusName="离职"},
                new WorkStatusInfo{ID=1,StatusName="在职"}
            };
        }

        public static List<int> GetYearList()
        {
            List<int> YearList = new List<int>();
            for (int i = 2010; i < 2031; i++)
            {
                YearList.Add(i);
            }

            return YearList;
        }

        public static List<int> GetMonthList()
        {
            List<int> MonthList = new List<int>();
            for (int i = 1; i < 13; i++)
            {
                MonthList.Add(i);
            }

            return MonthList;
        }


        public static T DeepClone<T>(T obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                ms.Seek(0, 0);
                T value = (T)(bf.Deserialize(ms));

                return value;
            }
        }

        public static List<SelectionProcedureInfo> GetSelectionProcedureInfoList(string procedurePrice)
        {
            MatchCollection matchCollection = Regex.Matches(procedurePrice, @"(\{.+?\})");

            List<SelectionProcedureInfo> procedureInfoList = new List<SelectionProcedureInfo>();
            foreach (Match match in matchCollection)
            {
                string[] map = match.Value.Trim("{}".ToCharArray()).Split(',');
                procedureInfoList.Add(new SelectionProcedureInfo { Procedure = map[0], Price = double.Parse(map[1]) });
            }

            return procedureInfoList;
        }

        public static ObservableCollection<T> CompareCollectionIsChanged<T>(ObservableCollection<T> SourceList, ObservableCollection<T> DestList, string PropertyName)
        {
            ObservableCollection<T> tempList = new ObservableCollection<T>();
            foreach (T DestInfo in DestList)
            {
                T SourceInfo = SourceList.Where(info => GetPropertyValue(DestInfo, PropertyName) == GetPropertyValue(info, PropertyName)).FirstOrDefault();
                if (SourceInfo != null)
                    if (AlgorithmClass.CheckIsChanged<T>(SourceInfo, DestInfo))
                    {
                        tempList.Add(DestInfo);
                    }
            }
            return tempList;
        }

        public static string GetPropertyValue<T>(T obj, string propertyName)
        {
            return typeof(T).GetProperty(propertyName).GetValue(obj, null).ToString();
        }

        public static bool CheckIsChanged<T>(T SourceObj, T DestObj)
        {
            PropertyInfo[] propertyInfoArray = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfoArray != null)
            {
                foreach (PropertyInfo info in propertyInfoArray)
                {
                    var sourceobj = info.GetValue(SourceObj, null);
                    var destobj = info.GetValue(DestObj, null);

                    if (sourceobj == null || destobj == null)
                    {
                        if (sourceobj != null || destobj != null)
                            return true;
                        else
                            continue;
                    }

                    if (info.GetValue(SourceObj, null).ToString() != info.GetValue(DestObj, null).ToString())
                        return true;
                }
            }

            return false;
        }

        public static string GetProcedureMap(ObservableCollection<ProcedureInfo> procedureInfoList)
        {
            string procedureMap = "";
            procedureInfoList.OrderBy(procedureInfo => procedureInfo.CompleteDate).ToList().ForEach(procedureInfo =>
            {
                procedureMap += procedureInfo.GetProcedureMapInfo();
            });

            return procedureMap;
        }

        public static string GetBOPMap(ObservableCollection<SheetBOPInfo> bopInfoList)
        {
            string bopMap = "";
            bopInfoList.OrderBy(bopInfo => bopInfo.BOPDate).ToList().ForEach(bopInfo =>
            {
                bopMap += bopInfo.GetBOPMapInfo();
            });

            return bopMap;
        }

        public static string GetFollowers(ObservableCollection<ProcedureInfo> procedureInfoList)
        {
            string followers = "";
            procedureInfoList.ToList().ForEach(procedureInfo =>
            {
                string temp = procedureInfo.EmployeeName + "(" + procedureInfo.EmployeeID + ")";
                if (!followers.Contains(temp))
                    followers += temp + ",";
            });

            return followers.Trim(',');
        }

        public static double GetBOPTotal(ObservableCollection<SheetBOPInfo> bopInfoList)
        {
            double total = 0;
            bopInfoList.ToList().ForEach(bopInfo =>
            {
                total += (bopInfo.Balance - bopInfo.Payment);
            });

            return total;
        }

        public static ObservableCollection<EmployeeInfo> InitEmployeeSelectionList(ObservableCollection<EmployeeInfo> employeeList, ObservableCollection<PostInfo> postList, ObservableCollection<DepartmentInfo> departmentList, List<WorkStatusInfo> workStatusList)
        {
            foreach (EmployeeInfo info in employeeList)
            {
                info.SelectionPostInfoList = postList.ToList();
                info.SelectionDepartmentInfoList = departmentList.ToList();
                info.SelectionWorkStatusInfoList = workStatusList;
            }

            return employeeList;
        }

        public static ObservableCollection<SalaryInfo> InitSalarySelectionList(ObservableCollection<SalaryInfo> salaryList, ObservableCollection<EmployeeInfo> employeeList, ObservableCollection<DepartmentInfo> departmentList, List<WorkStatusInfo> workStatusList)
        {
            foreach (SalaryInfo info in salaryList)
            {
                info.SelectionEmployeeInfoList = employeeList.ToList();
                info.SelectionDepartmentInfoList = departmentList.ToList();
                info.SelectionWorkStatusInfoList = workStatusList;
            }

            return salaryList;
        }

        public static ObservableCollection<BOPInfo> InitBOPSelectionList(ObservableCollection<BOPInfo> bopList, ObservableCollection<DepartmentInfo> departmentList, ObservableCollection<EmployeeInfo> employeeList)
        {
            foreach (BOPInfo info in bopList)
            {
                info.SelectionDepartmentInfoList = departmentList.ToList();
                info.SelectionEmployeeInfoList = employeeList.ToList();
            }

            return bopList;
        }

        public static ObservableCollection<BOPInfo> GetBOPListByBOPType(ObservableCollection<BOPInfo> allBOPList, BOPType type)
        {
            return AlgorithmClass.DeepClone<ObservableCollection<BOPInfo>>(new ObservableCollection<BOPInfo>(allBOPList.Where(info => info.BOP_Type == type)));
        }

        public static void CollectionViewSource_Refresh(UserControl userControl, string key)
        {
            if ((userControl.FindResource(key) as CollectionViewSource).View == null)
                return;
            (userControl.FindResource(key) as CollectionViewSource).View.Refresh();
        }
    }
}
