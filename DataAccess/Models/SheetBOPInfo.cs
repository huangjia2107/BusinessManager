using DataAccess.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Models
{
    [Serializable]
    public class SheetBOPInfo : BaseInfo
    {
        public SheetBOPInfo()
        {
            bOPDate = DateTime.Now;
        }

        private bool canRecycle;
        public bool CanRecycle
        {
            get { return canRecycle; }
            set { canRecycle = value; NotifyPropertyChange("CanRecycle"); }
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

        private string bOPMap;
        public string BOPMap
        {
            get { return bOPMap; }
            set
            {
                bOPMap = value;
                UpdateOtherInfo();
                NotifyPropertyChange("BOPMap");
            }
        }

        public string GetBOPMapInfo()
        {
            return "{" + (CanRecycle ? 1 : 0).ToString() + "," + BOPDate.ToString("yyyy-MM-dd") + "," + BOPReason + "," + Balance + "," + Payment + "}";
        }

        private void UpdateOtherInfo()
        {
            string[] param = BOPMap.Trim("{}".ToCharArray()).Split(',');
            CanRecycle = int.Parse(param[0]) == 1 ? true : false;
            BOPDate = Convert.ToDateTime(param[1]);
            BOPReason = param[2];
            Balance = double.Parse(param[3]);
            Payment = double.Parse(param[4]);
        }

    }
}
