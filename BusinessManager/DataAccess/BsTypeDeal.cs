using DataAccess.Access;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BusinessManager.DataAccess
{
    class BsTypeDeal
    {
        #region 业务

        public static ObservableCollection<BsTypeInfo> GetAllBsType()
        {
            ObservableCollection<BsTypeInfo> typeList = new ObservableCollection<BsTypeInfo>(Access.GetBsTypeList());
            if (typeList == null)
            {
                return new ObservableCollection<BsTypeInfo>();
            }

            return typeList;
        }

        #endregion
    }
}
