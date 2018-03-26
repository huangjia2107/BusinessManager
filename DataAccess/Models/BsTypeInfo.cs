using DataAccess.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Models
{
    [Serializable]
    public class BsTypeInfo : BaseInfo
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; NotifyPropertyChange("ID"); }
        }

        private string typeName;
        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; NotifyPropertyChange("TypeName"); }
        }

        private string procedurePrice;
        public string ProcedurePrice
        {
            get { return procedurePrice; }
            set { procedurePrice = value; NotifyPropertyChange("ProcedurePrice"); }
        }
    }
}
