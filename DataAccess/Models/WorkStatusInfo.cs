using DataAccess.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Models
{
    [Serializable]
    public class WorkStatusInfo : BaseInfo
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; NotifyPropertyChange("ID"); }
        }

        private string statusName;
        public string StatusName
        {
            get { return statusName; }
            set { statusName = value; NotifyPropertyChange("StatusName"); }
        }
    }
}
