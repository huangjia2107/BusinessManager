using DataAccess.Helps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BusinessManager.Models
{
    [Serializable]
    public class SalaryProcedureInfoWrap:BaseInfo
    {
        public SalaryProcedureInfoWrap()
        {
            SalaryProcedureInfoList = new ObservableCollection<SalaryProcedureInfo>();
        }

        private string employeeName;
        public string EmployeeName
        {
            get { return employeeName; }
            set { employeeName = value; NotifyPropertyChange("EmployeeName"); }
        }

        private string departmentName;
        public string DepartmentName
        {
            get { return departmentName; }
            set { departmentName = value; NotifyPropertyChange("DepartmentName"); }
        }

        private DateTime currentDate;
        public DateTime CurrentDate
        {
            get { return currentDate; }
            set { currentDate = value; NotifyPropertyChange("CurrentDate"); }
        }

        private double totalPrice;
        public double TotalPrice
        {
            get { return totalPrice; }
            set { totalPrice = value; NotifyPropertyChange("TotalPrice"); }
        }

        private ObservableCollection<SalaryProcedureInfo> salaryProcedureInfoList;
        public ObservableCollection<SalaryProcedureInfo> SalaryProcedureInfoList
        {
            get { return salaryProcedureInfoList; }
            set { salaryProcedureInfoList = value; NotifyPropertyChange("SalaryProcedureInfoList"); }
        }
    }
}
