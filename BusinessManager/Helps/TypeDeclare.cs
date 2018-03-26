using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessManager.Helps
{
    enum OperationType
    {
        New,
        Edit,
        Scan
    }

    enum TimeSearchType
    {
        SettleSheetTime,
        ComplateSheetTime,
        AcceptSheetTime
    }

    enum SheetSearchType
    {
        BusinessType,
        CustomerName,
        Accepter,
        SheetID
    }

    enum SheetFinishState
    {
        UnSettled,
        Settled,
        All
    } 

    enum EmployeeSearchType
    {
        DepartmentName,
        EmployeeName,
        EmployeeID
    }

    enum AppFolderPath
    {
        Templates,
        Outputs
    }

    enum TemplatesName
    {
        SheetLeft,
        SheetRight,
        Balance,
        Payment,
        DebitNote,
        Salary
    } 
}
