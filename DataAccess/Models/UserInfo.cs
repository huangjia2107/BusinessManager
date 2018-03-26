using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Models
{
    [Serializable]
    public class UserInfo
    { 
        public int UserID { get; set; }  
        public string UserName { get; set; }
        public string LoginPassWord { get; set; }
        public string FuncPassWord { get; set; } 
        public int Level { get; set; }
        public string Status { get; set; }
    }
}
