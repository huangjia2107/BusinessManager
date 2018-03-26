using DataAccess.Helps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace DataAccess.Models
{
    [Serializable]
    public class SheetInfo : BaseInfo
    {
        public SheetInfo()
        {
            startDate = DateTime.Now;
        }

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; NotifyPropertyChange("ID"); }
        }

        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; NotifyPropertyChange("StartDate"); }
        }

        private DateTime? settleDate;
        public DateTime? SettleDate
        {
            get { return settleDate; }
            set
            {
                settleDate = value;
                if (settleDate == null)
                    isSettle = false;
                else
                    isSettle = true;
                NotifyPropertyChange("SettleDate");
            }
        }

        private DateTime? finishDate;
        public DateTime? FinishDate
        {
            get { return finishDate; }
            set
            {
                finishDate = value; 
                NotifyPropertyChange("FinishDate");
            }
        }

        private string from;
        public string From
        {
            get { return from; }
            set { from = value; NotifyPropertyChange("From"); }
        }

        private int accepterID;
        public int AccepterID
        {
            get { return accepterID; }
            set
            {
                accepterID = value; 
                NotifyPropertyChange("AccepterID");
            }
        }

        private string accepter;
        public string Accepter
        {
            get { return accepter; }
            set
            {
                accepter = value; 
                NotifyPropertyChange("Accepter");
            }
        }  

        //数据库中未保存该数据 仅用于显示  真正的跟单员ID存于单所对应的流程中
        private string follower;
        public string Follower
        {
            get { return follower; }
            set
            {
                follower = value; 
                NotifyPropertyChange("Follower");
            }
        } 

        private string customer;
        public string Customer
        {
            get { return customer; }
            set { customer = value; NotifyPropertyChange("Customer"); }
        }

        private string contacter;
        public string Contacter
        {
            get { return contacter; }
            set { contacter = value; NotifyPropertyChange("Contacter"); }
        }

        private string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; NotifyPropertyChange("PhoneNumber"); }
        }

        private int bsTypeID;
        public int BSTypeID
        {
            get { return bsTypeID; }
            set { bsTypeID = value; NotifyPropertyChange("BSTypeID"); }
        }

        private string bsType;
        public string BSType
        {
            get { return bsType; }
            set { bsType = value; NotifyPropertyChange("BSType"); }
        }

        private double price;
        public double Price
        {
            get { return price; }
            set { price = value; NotifyPropertyChange("Price"); }
        }

        private bool isSettle;
        public bool IsSettle
        {
            get { return isSettle; }
            set { isSettle = value; NotifyPropertyChange("IsSettle"); }
        }

        private string remark;
        public string Remark
        {
            get { return remark; }
            set
            {
                remark = value;
                NotifyPropertyChange("Remark");
            }
        }  
    }
}
