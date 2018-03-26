using DataAccess.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessManager.Models
{
    [Serializable]
    public class SalaryProcedureInfo:BaseInfo
    { 
        private int sheetID;
        public int SheetID
        {
            get { return sheetID; }
            set { sheetID = value; NotifyPropertyChange("SheetID"); }
        }

        private DateTime settleDate;
        public DateTime SettleDate
        {
            get { return settleDate; }
            set { settleDate = value; NotifyPropertyChange("SettleDate"); }
        }

        private string bsType;
        public string BSType
        {
            get { return bsType; }
            set { bsType = value; NotifyPropertyChange("BSType"); }
        }

        private string customer;
        public string Customer
        {
            get { return customer; }
            set { customer = value; NotifyPropertyChange("Customer"); }
        }

        private string procedureName;
        public string ProcedureName
        {
            get { return procedureName; }
            set { procedureName = value; NotifyPropertyChange("ProcedureName"); }
        }

        private double price;
        public double Price
        {
            get { return price; }
            set { price = value; NotifyPropertyChange("Price"); }
        }


    }
}
