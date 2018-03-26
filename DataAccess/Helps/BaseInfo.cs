using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DataAccess.Helps
{
    [Serializable]
    public class BaseInfo : INotifyPropertyChanged
    {
        [field: NonSerializedAttribute()]
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
