using DataAccess.Helps;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessManager.Helps
{
    public class ResourceMap
    {
        public static Hashtable OperationTypeHasntable = new Hashtable() 
        {
            {OperationType.Scan,    "浏览"},
            {OperationType.Edit,    "编辑"},
            {OperationType.New,     "新建"}
        };

        public static Hashtable GeneralOperationTypeHasntable = new Hashtable() 
        {
            {OperationType.Scan,    "浏览"},
            {OperationType.Edit,    "编辑"}
        };

        public static Hashtable TimeSearchTypeHasntable = new Hashtable() 
        {
            {TimeSearchType.AcceptSheetTime,      "接单"},
            {TimeSearchType.ComplateSheetTime,    "完成"},
            {TimeSearchType.SettleSheetTime,      "结单"}
        };

        public static Hashtable SheetSearchTypeHasntable = new Hashtable() 
        {
            {SheetSearchType.SheetID,         "单号"},
            {SheetSearchType.Accepter,        "接单员"},
            {SheetSearchType.CustomerName,    "客户名称"},
            {SheetSearchType.BusinessType,    "业务类型"}
        };

        public static Hashtable SheetFinishStateHasntable = new Hashtable() 
        {
            {SheetFinishState.All,           "全部"},
            {SheetFinishState.Settled,       "已结"},
            {SheetFinishState.UnSettled,     "未结"}
        };

        public static Hashtable BOPTypeHasntable = new Hashtable() 
        {
            {BOPType.P,           "支出"},
            {BOPType.B,           "收入"}
        };

        public static Hashtable EmployeeSexHasntable = new Hashtable() 
        {
            {EmployeeSexType.boy,            "男"},
            {EmployeeSexType.girl,           "女"}
        };

        public static Hashtable EmployeeSearchTypeHashtable = new Hashtable()
        {
            {EmployeeSearchType.EmployeeName,           "姓名"},
            {EmployeeSearchType.DepartmentName,         "部门"}
        };

        public static Hashtable AppFolderPathHashtable = new Hashtable() 
        {
            {AppFolderPath.Templates,              AppDomain.CurrentDomain.BaseDirectory+"Templates\\"},
            {AppFolderPath.Outputs,                AppDomain.CurrentDomain.BaseDirectory+"Outputs\\"},
        };

        public static Hashtable TemplatesNameHashtable = new Hashtable()
        {
            {TemplatesName.SheetLeft,               "SheetLeft.xls"},
            {TemplatesName.SheetRight,              "SheetRight.xls"},
            {TemplatesName.Balance,                 "Balance.xls"},
            {TemplatesName.Payment,                 "Payment.xls"},
            {TemplatesName.DebitNote,               "DebitNote.xls"},
            {TemplatesName.Salary,                  "Salary.xls"}
        }; 
    }
}
