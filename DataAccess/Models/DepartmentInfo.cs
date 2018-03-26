using DataAccess.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Models
{
    [Serializable]
    public class DepartmentInfo : BaseInfo
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; NotifyPropertyChange("ID"); }
        }

        private string departmentName;
        public string DepartmentName
        {
            get { return departmentName; }
            set { departmentName = value; NotifyPropertyChange("DepartmentName"); }
        }
    }
}
