using DataAccess.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Models
{
    [Serializable]
    public class PostInfo:BaseInfo
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; NotifyPropertyChange("ID"); }
        }

        private string postName;
        public string PostName
        {
            get { return postName; }
            set { postName = value; NotifyPropertyChange("PostName"); }
        }
    }
}
