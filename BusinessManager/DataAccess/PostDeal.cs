using DataAccess.Access;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BusinessManager.DataAccess
{
    class PostDeal
    {
        #region  职务

        public static ObservableCollection<PostInfo> GetAllPost()
        {
            ObservableCollection<PostInfo> postList = new ObservableCollection<PostInfo>(Access.GetPostInfoList());
            if (postList == null)
            {
                return new ObservableCollection<PostInfo>();
            }

            return postList;
        }

        #endregion
    }
}
