using DataAccess.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Models
{
    [Serializable]
    public class SalaryInfo : BaseInfo
    {
        public SalaryInfo()
        {
            this.Year = DateTime.Now.Year;
            this.Month = DateTime.Now.Month;
        }

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; NotifyPropertyChange("ID"); }
        }

        private int year;
        public int Year
        {
            get { return year; }
            set { year = value; NotifyPropertyChange("Year"); }
        }

        private int month;
        public int Month
        {
            get { return month; }
            set { month = value; NotifyPropertyChange("Month"); }
        }

        private int employeeID;
        public int EmployeeID
        {
            get { return employeeID; }
            set
            {
                employeeID = value;
                UpdateEmployeeInfoByID();
                UpdateBasicSalaryByID();
                NotifyPropertyChange("EmployeeID");
            }
        }

        private int statusID;
        public int StatusID
        {
            get { return statusID; }
            set
            {
                statusID = value;
                NotifyPropertyChange("StatusID");
            }
        }

        private string employeeStatus;
        public string EmployeeStatus
        {
            get { return employeeStatus; }
            set { employeeStatus = value; NotifyPropertyChange("EmployeeStatus"); }
        }

        private string employeeName;
        public string EmployeeName
        {
            get { return employeeName; }
            set { employeeName = value; NotifyPropertyChange("EmployeeName"); }
        }

        private string employeeSex;
        public string EmployeeSex
        {
            get { return employeeSex; }
            set { employeeSex = value; NotifyPropertyChange("EmployeeSex"); }
        }

        private int departmentID;
        public int DepartmentID
        {
            get { return departmentID; }
            set
            {
                departmentID = value;
                NotifyPropertyChange("DepartmentID");
            }
        }

        private string departmentName;
        public string DepartmentName
        {
            get { return departmentName; }
            set { departmentName = value; NotifyPropertyChange("DepartmentName"); }
        }

        private double basicSalary;
        public double BasicSalary
        {
            get { return basicSalary; }
            set
            {
                basicSalary = value;
                UpdateNetPayroll();
                NotifyPropertyChange("BasicSalary");
            }
        }

        private double commission;
        public double Commission
        {
            get { return commission; }
            set
            {
                commission = value;
                UpdateNetPayroll();
                NotifyPropertyChange("Commission");
            }
        }

        private double mealSupplement;
        public double MealSupplement
        {
            get { return mealSupplement; }
            set
            {
                mealSupplement = value;
                UpdateNetPayroll();
                NotifyPropertyChange("MealSupplement");
            }
        }

        private double ssb;
        public double SSB
        {
            get { return ssb; }
            set
            {
                ssb = value;
                UpdateNetPayroll();
                NotifyPropertyChange("SSB");
            }
        }

        private double otherBenefits;
        public double OtherBenefits
        {
            get { return otherBenefits; }
            set
            {
                otherBenefits = value;
                UpdateNetPayroll();
                NotifyPropertyChange("OtherBenefits");
            }
        }

        private double bounty;
        public double Bounty
        {
            get { return bounty; }
            set
            {
                bounty = value;
                UpdateNetPayroll();
                NotifyPropertyChange("Bounty");
            }
        }

        private double otherDeduction;
        public double OtherDeduction
        {
            get { return otherDeduction; }
            set
            {
                otherDeduction = value;
                UpdateNetPayroll();
                NotifyPropertyChange("OtherDeduction");
            }
        }

        private double netPayroll;
        public double NetPayroll
        {
            get { return netPayroll; }
            set { netPayroll = value; NotifyPropertyChange("NetPayroll"); }
        }

        private string remark;
        public string Remark
        {
            get { return remark; }
            set { remark = value; NotifyPropertyChange("Remark"); }
        }

        private void UpdateNetPayroll()
        {
            this.NetPayroll = BasicSalary + Commission + MealSupplement + SSB + OtherBenefits + Bounty - OtherDeduction;
        }

        private void UpdateEmployeeInfoByID()
        {
            if (SelectionEmployeeInfoList == null || SelectionDepartmentInfoList == null || SelectionWorkStatusInfoList==null)
                return;

            if (this.EmployeeID == 0)
            {
                this.EmployeeID = SelectionEmployeeInfoList[0].ID;
                this.EmployeeName = SelectionEmployeeInfoList[0].EmployeeName;
                this.EmployeeSex = SelectionEmployeeInfoList[0].EmployeeSexStr;
                this.DepartmentID = SelectionEmployeeInfoList[0].DepartmentID;
                this.StatusID = SelectionEmployeeInfoList[0].StatusID;
            }
            else
            {
                EmployeeInfo employeeInfo = SelectionEmployeeInfoList.Where(info => info.ID == this.EmployeeID).FirstOrDefault();
                if (employeeInfo != null)
                {
                    this.EmployeeName = employeeInfo.EmployeeName;
                    this.EmployeeSex = employeeInfo.EmployeeSexStr;
                    this.DepartmentID = employeeInfo.DepartmentID;
                    this.StatusID = employeeInfo.StatusID;
                }
            }

            DepartmentInfo departmentInfo = SelectionDepartmentInfoList.Where(info => info.ID == this.DepartmentID).FirstOrDefault();
            if (departmentInfo != null)
                this.DepartmentName = departmentInfo.DepartmentName;

            WorkStatusInfo workStatusInfo = SelectionWorkStatusInfoList.Where(info => info.ID == this.StatusID).FirstOrDefault();
            if (workStatusInfo != null)
                this.EmployeeStatus = workStatusInfo.StatusName;
        }

        private void UpdateBasicSalaryByID()
        {
            if (SelectionEmployeeInfoList == null)
                return;

            if (this.EmployeeID == 0)
            {
                this.EmployeeID = SelectionEmployeeInfoList[0].ID;
                this.BasicSalary = SelectionEmployeeInfoList[0].BasicSalary;
            }
            else
            {
                if(this.BasicSalary==0)
                {
                    EmployeeInfo employeeInfo = SelectionEmployeeInfoList.Where(info => info.ID == this.EmployeeID).FirstOrDefault();
                    if (employeeInfo != null)
                        this.BasicSalary = employeeInfo.BasicSalary;
                }
            }
        }

        private List<WorkStatusInfo> selectionWorkStatusInfoList;
        public List<WorkStatusInfo> SelectionWorkStatusInfoList
        {
            get { return selectionWorkStatusInfoList; }
            set
            {
                selectionWorkStatusInfoList = value;
                UpdateEmployeeInfoByID();
                NotifyPropertyChange("SelectionWorkStatusInfoList");
            }
        }

        private List<EmployeeInfo> selectionEmployeeInfoList;
        public List<EmployeeInfo> SelectionEmployeeInfoList
        {
            get { return selectionEmployeeInfoList; }
            set
            {
                selectionEmployeeInfoList = value;
                UpdateEmployeeInfoByID();
                UpdateBasicSalaryByID();
                NotifyPropertyChange("SelectionEmployeeInfoList");
            }
        }

        private List<DepartmentInfo> selectionDepartmentInfoList;
        public List<DepartmentInfo> SelectionDepartmentInfoList
        {
            get { return selectionDepartmentInfoList; }
            set
            {
                selectionDepartmentInfoList = value;
                UpdateEmployeeInfoByID();
                NotifyPropertyChange("SelectionDepartmentInfoList");
            }
        }
    }
}
