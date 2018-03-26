using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Models
{
    [Serializable]
    public class SelectionProcedureInfo
    {
        private string procedure;
        public string Procedure
        {
            get { return procedure; }
            set
            {
                procedure = value;  
            }
        }

        private double price;
        public double Price
        {
            get { return price; }
            set
            {
                price = value;  
            }
        }
    }
}
