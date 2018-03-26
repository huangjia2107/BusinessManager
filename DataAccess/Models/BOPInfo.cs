using DataAccess.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Models
{
    [Serializable]
    public class BOPInfo : BaseInfo
    {
        public BOPInfo()
        {
            bOPDate = DateTime.Now;
        }

        private BOPType bOP_Type;
        public BOPType BOP_Type
        {
            get { return bOP_Type; }
            set
            {
                bOP_Type = value;
                BOPTypeStr = value.ToString();
                UpdatePropertyShowByBOPType();
                NotifyPropertyChange("BOP_Type");
            }
        }

        private string bopTypeStr;
        public string BOPTypeStr
        {
            get { return bopTypeStr; }
            set { bopTypeStr = value; NotifyPropertyChange("BOPTypeStr"); }
        }

        private void UpdatePropertyShowByBOPType()
        {
            switch (BOP_Type)
            {
                case BOPType.P:
                    BookKeepingName = "--";
                    break;
                case BOPType.B:
                    PayeeName = "--";
                    DepartmentName = "--";
                    break;
            }
        }

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; NotifyPropertyChange("ID"); }
        }

        private DateTime bOPDate;
        public DateTime BOPDate
        {
            get { return bOPDate; }
            set
            {
                bOPDate = value;
                NotifyPropertyChange("BOPDate");
            }
        }

        private string bOPReason;
        public string BOPReason
        {
            get { return bOPReason; }
            set { bOPReason = value; NotifyPropertyChange("BOPReason"); }
        }

        private double balance;
        public double Balance
        {
            get { return balance; }
            set { balance = value; NotifyPropertyChange("Balance"); }
        }

        private double payment;
        public double Payment
        {
            get { return payment; }
            set { payment = value; NotifyPropertyChange("Payment"); }
        }

        private int? departmentID;
        public int? DepartmentID
        {
            get { return departmentID; }
            set
            {
                departmentID = value;
                UpdateDepartmentNameByID();
                NotifyPropertyChange("DepartmentID");
            }
        }

        private string departmentName;
        public string DepartmentName
        {
            get { return departmentName; }
            set { departmentName = value; NotifyPropertyChange("DepartmentName"); }
        }


        //会计主管
        private int accountingSupervisorID;
        public int AccountingSupervisorID
        {
            get { return accountingSupervisorID; }
            set
            {
                accountingSupervisorID = value;
                UpdateEmployeeNameByID(accountingSupervisorID, EmployeeType.AccountingSupervisorName);
                NotifyPropertyChange("AccountingSupervisorID");
            }
        }

        private string accountingSupervisorName;
        public string AccountingSupervisorName
        {
            get { return accountingSupervisorName; }
            set { accountingSupervisorName = value; NotifyPropertyChange("AccountingSupervisorName"); }
        }

        //出纳
        private int cashierID;
        public int CashierID
        {
            get { return cashierID; }
            set
            {
                cashierID = value;
                UpdateEmployeeNameByID(cashierID, EmployeeType.CashierName);
                NotifyPropertyChange("CashierID");
            }
        }

        private string cashierName;
        public string CashierName
        {
            get { return cashierName; }
            set { cashierName = value; NotifyPropertyChange("CashierName"); }
        }

        //经手人
        private int handlerID;
        public int HandlerID
        {
            get { return handlerID; }
            set
            {
                handlerID = value;
                UpdateEmployeeNameByID(handlerID, EmployeeType.HandlerName);
                NotifyPropertyChange("HandlerID");
            }
        }

        private string handlerName;
        public string HandlerName
        {
            get { return handlerName; }
            set { handlerName = value; NotifyPropertyChange("HandlerName"); }
        }

        //领款人
        private int? payeeID;
        public int? PayeeID
        {
            get { return payeeID; }
            set
            {
                payeeID = value;
                UpdateEmployeeNameByID(payeeID, EmployeeType.PayeeName);
                NotifyPropertyChange("PayeeID");
            }
        }

        private string payeeName;
        public string PayeeName
        {
            get { return payeeName; }
            set { payeeName = value; NotifyPropertyChange("PayeeName"); }
        }

        //记账
        private int? bookKeepingID;
        public int? BookKeepingID
        {
            get { return bookKeepingID; }
            set
            {
                bookKeepingID = value;
                UpdateEmployeeNameByID(bookKeepingID, EmployeeType.BookKeepingName);
                NotifyPropertyChange("BookKeepingID");
            }
        }

        private string bookKeepingName;
        public string BookKeepingName
        {
            get { return bookKeepingName; }
            set { bookKeepingName = value; NotifyPropertyChange("BookKeepingName"); }
        }

        private string remark;
        public string Remark
        {
            get { return remark; }
            set { remark = value; NotifyPropertyChange("Remark"); }
        }

        private void UpdateEmployeeNameByID(int? employeeID, EmployeeType type)
        {
            if (employeeID == null || SelectionEmployeeInfoList==null)
                return;

            EmployeeInfo employeeInfo = SelectionEmployeeInfoList.Where(info => info.ID == employeeID).FirstOrDefault();
            if (employeeInfo != null)
            {
                switch (type)
                {
                    case EmployeeType.AccountingSupervisorName:
                        AccountingSupervisorName = employeeInfo.EmployeeName;
                        break;
                    case EmployeeType.CashierName:
                        CashierName = employeeInfo.EmployeeName;
                        break;
                    case EmployeeType.HandlerName:
                        HandlerName = employeeInfo.EmployeeName;
                        break;
                    case EmployeeType.PayeeName:
                        PayeeName = employeeInfo.EmployeeName;
                        break;
                    case EmployeeType.BookKeepingName:
                        BookKeepingName = employeeInfo.EmployeeName;
                        break;
                }
            }
        }

        private List<EmployeeInfo> selectionEmployeeInfoList;
        public List<EmployeeInfo> SelectionEmployeeInfoList
        {
            get { return selectionEmployeeInfoList; }
            set
            {
                selectionEmployeeInfoList = value;
                UpdateEmployeeNameByID(this.AccountingSupervisorID,EmployeeType.AccountingSupervisorName);
                UpdateEmployeeNameByID(this.CashierID, EmployeeType.CashierName);
                UpdateEmployeeNameByID(this.HandlerID, EmployeeType.HandlerName);
                UpdateEmployeeNameByID(this.PayeeID, EmployeeType.PayeeName);
                UpdateEmployeeNameByID(this.BookKeepingID, EmployeeType.BookKeepingName);
                NotifyPropertyChange("SelectionEmployeeInfoList");
            }
        }

        private void UpdateDepartmentNameByID()
        {
            if (this.DepartmentID == null || SelectionDepartmentInfoList==null)
                return;

            DepartmentInfo departmentInfo = SelectionDepartmentInfoList.Where(info => info.ID == this.DepartmentID).FirstOrDefault();
            if (departmentInfo != null)
            {
                DepartmentName = departmentInfo.DepartmentName;
            }
        }

        private List<DepartmentInfo> selectionDepartmentInfoList;
        public List<DepartmentInfo> SelectionDepartmentInfoList
        {
            get { return selectionDepartmentInfoList; }
            set
            {
                selectionDepartmentInfoList = value;
                UpdateDepartmentNameByID();
                NotifyPropertyChange("SelectionDepartmentInfoList");
            }
        }
    }
}
