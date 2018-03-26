using DataAccess.Helps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace BusinessManager.Helps
{
    public class PageManager : BaseInfo
    {
        private int countPerPage;
        public int CountPerPage
        {
            get { return countPerPage; }
            set
            {
                if (RefreshEvent != null || value != countPerPage)
                {
                    countPerPage = value;
                    RefreshEvent(this, new EventArgs());
                }

                NotifyPropertyChange("CountPerPage");
            }
        }

        private int currentPage;
        public int CurrentPage
        {
            get { return currentPage; }
            set
            {
                if (RefreshEvent != null && value != currentPage)
                {
                    currentPage = value;
                    RefreshEvent(this, new EventArgs());
                }

                NotifyPropertyChange("CurrentPage");
            }
        }


        private int totalCount;
        public int TotalCount
        {
            get { return totalCount; }
            set { totalCount = value; NotifyPropertyChange("TotalCount"); }
        }

        private int totalPages;
        public int TotalPages
        {
            get { return totalPages; }
            set { totalPages = value; NotifyPropertyChange("TotalPages"); }
        }

        public event EventHandler RefreshEvent;

        public PageManager(int CollectionCount, int CountPerPage = 15)
        {
            currentPage = 1;
            totalCount = 0;
            totalPages = 1;
            countPerPage = CountPerPage;
            isCountPerPageEnabled = true;
            RefreshPageParams(CollectionCount);
        }

        private void RefreshPageParams(int CollectionCount)
        {
            this.TotalCount = CollectionCount;

            if (CountPerPage < CollectionCount)
            {
                int remainder = CollectionCount % CountPerPage;  //余数
                int integer = CollectionCount / CountPerPage; //整数

                this.TotalPages = CollectionCount == 0 ? 1 : (remainder == 0 ? integer : integer + 1);
            }
            else
                this.TotalPages = 1;
        }

        private ObservableCollection<T> GetSpecialPageCollection<T>(ObservableCollection<T> totalList)
        {
            if (totalList.Count == 0)
            {
                CurrentPage = 1;
                UpdatePageEnabled();
                return new ObservableCollection<T>();
            }

            CurrentPage = CurrentPage > TotalPages ? TotalPages : (CurrentPage <= 0 ? 1 : CurrentPage);
            int skipCount = (CurrentPage - 1) * CountPerPage;

            int remainder = totalList.Count % CountPerPage;  //余数
            int takeCount = CurrentPage != TotalPages ? CountPerPage : (remainder == 0 ? CountPerPage : remainder);

            UpdatePageEnabled();
            return new ObservableCollection<T>(totalList.Skip(skipCount).Take(takeCount));
        }

        public ObservableCollection<T> GetFirstPageCollection<T>(ObservableCollection<T> totalList)
        {
            CurrentPage = 1;
            return GetSpecialPageCollection<T>(totalList);
        }

        public ObservableCollection<T> GetLastPageCollection<T>(ObservableCollection<T> totalList)
        {
            CurrentPage = TotalPages;
            return GetSpecialPageCollection<T>(totalList);
        }

        public ObservableCollection<T> GetPrePageCollection<T>(ObservableCollection<T> totalList)
        {
            CurrentPage = CurrentPage == 1 ? 1 : CurrentPage - 1;
            return GetSpecialPageCollection<T>(totalList);
        }

        public ObservableCollection<T> GetNextPageCollection<T>(ObservableCollection<T> totalList)
        {
            CurrentPage = CurrentPage == TotalPages ? TotalPages : CurrentPage + 1;
            return GetSpecialPageCollection<T>(totalList);
        }

        public ObservableCollection<T> ReFreshCurPageCollection<T>(ObservableCollection<T> totalList)
        {
            RefreshPageParams(totalList.Count);
            return GetSpecialPageCollection<T>(totalList);
        }


        private void UpdatePageEnabled()
        {
            if (TotalPages == 1)
            {
                IsFirstEnabled = IsPreEnabled = IsNextEnabled = IsLastEnabled = IsJumpPageEnabled = false;
                //IsCountPerPageEnabled = false;
            }

            if (TotalPages == 2)
            {
                //IsCountPerPageEnabled = true;
                IsJumpPageEnabled = true;

                if (CurrentPage == 1)
                {
                    IsFirstEnabled = IsPreEnabled = false;
                    IsNextEnabled = IsLastEnabled = true;
                }
                else
                {
                    IsFirstEnabled = IsPreEnabled = true;
                    IsNextEnabled = IsLastEnabled = false;
                }
            }

            if (TotalPages > 2)
            {
                //IsCountPerPageEnabled = true;
                IsJumpPageEnabled = true;

                if (CurrentPage == 1)
                {
                    IsFirstEnabled = IsPreEnabled = false;
                    IsNextEnabled = IsLastEnabled = true;
                }
                else if (CurrentPage == TotalPages)
                {
                    IsFirstEnabled = IsPreEnabled = true;
                    IsNextEnabled = IsLastEnabled = false;
                }
                {
                    IsFirstEnabled = IsPreEnabled = IsNextEnabled = IsLastEnabled = true;
                }
            }
        }


        private bool isCountPerPageEnabled;
        public bool IsCountPerPageEnabled
        {
            get { return isCountPerPageEnabled; }
            set { isCountPerPageEnabled = value; NotifyPropertyChange("IsCountPerPageEnabled"); }
        }


        private bool isJumpPageEnabled;
        public bool IsJumpPageEnabled
        {
            get { return isJumpPageEnabled; }
            set { isJumpPageEnabled = value; NotifyPropertyChange(" IsJumpPageEnabled"); }
        }


        private bool isFirstEnabled;
        public bool IsFirstEnabled
        {
            get { return isFirstEnabled; }
            set { isFirstEnabled = value; NotifyPropertyChange("IsFirstEnabled"); }
        }

        private bool isPreEnabled;
        public bool IsPreEnabled
        {
            get { return isPreEnabled; }
            set { isPreEnabled = value; NotifyPropertyChange("IsPreEnabled"); }
        }

        private bool isNextEnabled;
        public bool IsNextEnabled
        {
            get { return isNextEnabled; }
            set { isNextEnabled = value; NotifyPropertyChange("IsNextEnabled"); }
        }

        private bool isLastEnabled;
        public bool IsLastEnabled
        {
            get { return isLastEnabled; }
            set { isLastEnabled = value; NotifyPropertyChange("IsLastEnabled"); }
        }

    }
}
