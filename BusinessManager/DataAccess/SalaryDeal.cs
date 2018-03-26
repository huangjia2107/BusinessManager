using DataAccess.Access;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BusinessManager.DataAccess
{
    class SalaryDeal
    {
        #region  工资

        public static ObservableCollection<SalaryInfo> GetAllSalary()
        {
            ObservableCollection<SalaryInfo> empluyeeList = new ObservableCollection<SalaryInfo>(Access.GetSalaryInfoList());
            if (empluyeeList == null)
            {
                return new ObservableCollection<SalaryInfo>();
            }

            return empluyeeList;
        }

        public static bool UpdateSalaryInfo(SalaryInfo salaryInfo)
        {
            return Access.UpdateSalaryInfo(salaryInfo) == 1 ? true : false;
        }

        public static bool InsertSalaryInfo(SalaryInfo salaryInfo, ref int insertID)
        {
            insertID = Access.InsertSalary(salaryInfo);
            return insertID == 0 ? false : true;
        }

        public static bool DeleteSalaryInfo(int salaryID)
        {
            int result = Access.DeleteSalary(salaryID);
            return result == 0 ? false : true;
        }

        #endregion
    }
}
