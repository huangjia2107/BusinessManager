using DataAccess.Access;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BusinessManager.DataAccess
{
    class EmployeeDeal
    {
        #region  雇员

        public static ObservableCollection<EmployeeInfo> GetAllEmployee()
        {
            ObservableCollection<EmployeeInfo> empluyeeList = new ObservableCollection<EmployeeInfo>(Access.GetEmpluyeeInfoList());
            if (empluyeeList == null)
            {
                return new ObservableCollection<EmployeeInfo>();
            }

            return empluyeeList;
        }

        public static bool UpdateEmployeeInfo(EmployeeInfo employeeInfo)
        {
            return Access.UpdateEmployeeInfo(employeeInfo) == 1 ? true : false;
        }

        public static bool InsertEmployeeInfo(EmployeeInfo employeeInfo, ref int insertID)
        {
            insertID = Access.InsertEmployee(employeeInfo);
            return insertID == 0 ? false : true;
        }

        public static bool DeleteEmployeeInfo(int employeeID)
        {
            int result = Access.DeleteEmployee(employeeID);
            return result == 0 ? false : true;
        }

        #endregion
    }
}
