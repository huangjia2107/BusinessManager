using AppLog4Net;
using DataAccess.Access;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessManager.DataAccess
{
    class SheetDeal
    { 

        #region 单

        public static ObservableCollection<SheetInfo> GetAllSheet()
        {
            ObservableCollection<SheetInfo> sheetList = new ObservableCollection<SheetInfo>(Access.GetSheetList());
            if (sheetList == null)
            {
                return new ObservableCollection<SheetInfo>();
            }

            return sheetList;
        }

        public static bool UpdateSheetInfo(SheetInfo sheetInfo)
        {
            return Access.UpdateSheetInfo(sheetInfo) == 1 ? true : false;
        } 

        public static bool InsertSheetInfo(SheetInfo sheetInfo,ref int insertID)
        {
            insertID= Access.InsertSheet(sheetInfo);
            return insertID == 0 ? false : true;
        }

        public static bool DeleteSheetInfo(int sheetID)
        {
            return Access.DeleteSheet(sheetID) >= 1 ? true : false;
        }

        #endregion

        #region 流程

        public static ObservableCollection<ProcedureInfo> GetAllProcedureInfo(int sheetID)
        {
            string procedureMap = Access.GetProcedureMap(sheetID);
            if (string.IsNullOrEmpty(procedureMap))
            {
                return new ObservableCollection<ProcedureInfo>();
            }

            MatchCollection matchCollection = Regex.Matches(procedureMap, @"(\{.+?\})");

            ObservableCollection<ProcedureInfo> procedureInfoList = new ObservableCollection<ProcedureInfo>();
            foreach (Match match in matchCollection)
            {
                procedureInfoList.Add(new ProcedureInfo
                {
                    SelectionEmployeeInfoList = DataManager.dataManager.AllEmployeeList.ToList(),
                    ProcedureMap = match.Value
                });
            }

            return procedureInfoList;
        }
        
        public static bool UpdateProcedureMap(int sheetID, string procedureMap)
        {
            return Access.UpdateProcedureMap(sheetID, procedureMap) == 1 ? true : false;
        }

        public static bool InsertProcedureMap(int sheetID,string procedureMap)
        {
            int insertID = Access.InsertProcedureMap(sheetID,procedureMap);
            return insertID == 0 ? false : true;
        }

        #endregion

        #region 收支

        public static ObservableCollection<SheetBOPInfo> GetAllSheetBOPInfo(int sheetID)
        {
            string bopMap = Access.GetSheetBOPMap(sheetID);
            if (string.IsNullOrEmpty(bopMap))
            {
                return new ObservableCollection<SheetBOPInfo>();
            }

            MatchCollection matchCollection = Regex.Matches(bopMap, @"(\{.+?\})");

            ObservableCollection<SheetBOPInfo> bopInfoList = new ObservableCollection<SheetBOPInfo>();
            foreach (Match match in matchCollection)
            {
                bopInfoList.Add(new SheetBOPInfo
                {
                    BOPMap = match.Value
                });
            }

            return bopInfoList;
        }

        public static bool UpdateSheetBOPMap(int sheetID, string bopMap)
        {
            return Access.UpdateSheetBOPMap(sheetID, bopMap) == 1 ? true : false;
        }

        public static bool InsertSheetBOPMap(int sheetID, string bopMap)
        {
            int insertID = Access.InsertSheetBOPMap(sheetID, bopMap);
            return insertID == 0 ? false : true;
        }

        #endregion

    }
}
