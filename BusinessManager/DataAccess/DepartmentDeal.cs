using DataAccess.Access;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BusinessManager.DataAccess
{
    class DepartmentDeal
    {
        #region  部门

        public static ObservableCollection<DepartmentInfo> GetAllDepartment()
        {
            ObservableCollection<DepartmentInfo> departmentList = new ObservableCollection<DepartmentInfo>(Access.GetDepartmentInfoList());
            if (departmentList == null)
            {
                return new ObservableCollection<DepartmentInfo>();
            }

            return departmentList;
        }

        #endregion
    }
}
