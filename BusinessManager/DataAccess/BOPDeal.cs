using DataAccess.Access;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BusinessManager.DataAccess
{
    class BOPDeal
    {
        public static ObservableCollection<BOPInfo> GetAllBOP()
        {
            ObservableCollection<BOPInfo> bopList = new ObservableCollection<BOPInfo>(Access.GetBOPList());
            if (bopList == null)
            {
                return new ObservableCollection<BOPInfo>();
            }

            return bopList;
        }

        public static bool UpdateBOPInfo(BOPInfo bopInfo)
        {
            return Access.UpdateBOPInfo(bopInfo) == 1 ? true : false;
        }

        public static bool InsertBOPInfo(BOPInfo bopInfo, ref int insertID)
        {
            insertID = Access.InsertBOP(bopInfo);
            return insertID == 0 ? false : true;
        }

        public static bool DeleteBOPInfo(int bopID)
        {
            int result = Access.DeleteBOP(bopID);
            return result == 0 ? false : true;
        }
    }
}
