using AppLog4Net;
using BusinessManager.DataAccess;
using BusinessManager.Helps;
using DataAccess.Helps;
using DataAccess.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BusinessManager.UIControls
{
    /// <summary>
    /// BOPManagerControl.xaml 的交互逻辑
    /// </summary>
    public partial class BOPManagerControl : UserControl
    {
        AppLog appLog = null;
        DataManager dataManager;
        PageManager pageManager;

        ObservableCollection<BOPInfo> BalanceBOPList = null;
        ObservableCollection<BOPInfo> PaymentBOPList = null;

        public BOPManagerControl()
        {
            InitializeComponent();
            appLog = new AppLog("BOPManagerControl");
            dataManager = DataManager.dataManager;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            AlgorithmClass.InitBOPSelectionList(dataManager.AllBOPList, dataManager.AllDepartmentList, dataManager.AllEmployeeList);
            BalanceBOPList = AlgorithmClass.GetBOPListByBOPType(dataManager.AllBOPList, BOPType.B);
            PaymentBOPList = AlgorithmClass.GetBOPListByBOPType(dataManager.AllBOPList, BOPType.P);

            BalanceBOPList.CollectionChanged += BalanceBOPList_CollectionChanged;
            PaymentBOPList.CollectionChanged += PaymentBOPList_CollectionChanged;

            InitBOPUIList(BalanceBOPList, BOPType.B);
        }

        #region 分页

        private void InitBOPUIList(ObservableCollection<BOPInfo> bopList, BOPType type)
        {
            pageManager = new PageManager(bopList.Count);
            (FindResource("ViewSource") as CollectionViewSource).Source = pageManager.ReFreshCurPageCollection(bopList);
            PageManagerGrid.DataContext = pageManager;
            pageManager.RefreshEvent += pageManager_RefreshEvent;

            balanceColumn.Visibility = type == BOPType.B ? Visibility.Visible : Visibility.Collapsed;
            bkColumn.Visibility = type == BOPType.B ? Visibility.Visible : Visibility.Collapsed;

            paymentColumn.Visibility = type == BOPType.P ? Visibility.Visible : Visibility.Collapsed;
            departmentColumn.Visibility = type == BOPType.P ? Visibility.Visible : Visibility.Collapsed;
            payeeColumn.Visibility = type == BOPType.P ? Visibility.Visible : Visibility.Collapsed;
            CountCurViewPrice();
        }

        void PaymentBOPList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            (FindResource("ViewSource") as CollectionViewSource).Source = pageManager.ReFreshCurPageCollection(PaymentBOPList);
        }

        void BalanceBOPList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            (FindResource("ViewSource") as CollectionViewSource).Source = pageManager.ReFreshCurPageCollection(BalanceBOPList);
        }

        void pageManager_RefreshEvent(object sender, EventArgs e)
        {
            (FindResource("ViewSource") as CollectionViewSource).Source = (BOPType)(((DictionaryEntry)(BOPTypeList.SelectedValue)).Key) == BOPType.B ? pageManager.ReFreshCurPageCollection(BalanceBOPList) : pageManager.ReFreshCurPageCollection(PaymentBOPList);
        }

        private void FirstPageBtn_Click(object sender, RoutedEventArgs e)
        {
            (FindResource("ViewSource") as CollectionViewSource).Source = (BOPType)(((DictionaryEntry)(BOPTypeList.SelectedValue)).Key) == BOPType.B ? pageManager.GetFirstPageCollection(BalanceBOPList) : pageManager.GetFirstPageCollection(PaymentBOPList);
        }

        private void PrePageBtn_Click(object sender, RoutedEventArgs e)
        {
            (FindResource("ViewSource") as CollectionViewSource).Source = (BOPType)(((DictionaryEntry)(BOPTypeList.SelectedValue)).Key) == BOPType.B ? pageManager.GetPrePageCollection(BalanceBOPList) : pageManager.GetPrePageCollection(PaymentBOPList);
        }

        private void NextPageBtn_Click(object sender, RoutedEventArgs e)
        {
            (FindResource("ViewSource") as CollectionViewSource).Source = (BOPType)(((DictionaryEntry)(BOPTypeList.SelectedValue)).Key) == BOPType.B ? pageManager.GetNextPageCollection(BalanceBOPList) : pageManager.GetNextPageCollection(PaymentBOPList);
        }

        private void LastPageBtn_Click(object sender, RoutedEventArgs e)
        {
            (FindResource("ViewSource") as CollectionViewSource).Source = (BOPType)(((DictionaryEntry)(BOPTypeList.SelectedValue)).Key) == BOPType.B ? pageManager.GetLastPageCollection(BalanceBOPList) : pageManager.GetLastPageCollection(PaymentBOPList);
        }

        #endregion

        #region 添加 删除 保存 打印 还原

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            BOPInfo tempInfo = null;

            switch ((BOPType)(((DictionaryEntry)(BOPTypeList.SelectedValue)).Key))
            {
                case BOPType.P:
                    tempInfo = PaymentBOPList.Where(bop => bop.ID == 0).FirstOrDefault();
                    if (tempInfo == null)
                    {
                        PaymentBOPList.Add(new BOPInfo
                        {
                            BOP_Type = BOPType.P,
                            SelectionEmployeeInfoList = dataManager.AllEmployeeList.ToList(),
                            SelectionDepartmentInfoList = dataManager.AllDepartmentList.ToList()
                        });
                    }
                    else
                    {
                        MessageBox.Show("请先保存当前新添加项！");
                    }

                    break;
                case BOPType.B:
                    tempInfo = BalanceBOPList.Where(bop => bop.ID == 0).FirstOrDefault();
                    if (tempInfo == null)
                    {
                        BalanceBOPList.Add(new BOPInfo
                        {
                            BOP_Type = BOPType.B,
                            SelectionEmployeeInfoList = dataManager.AllEmployeeList.ToList()
                        });
                    }
                    else
                    {
                        MessageBox.Show("请先保存当前新添加项！");
                    }
                    break;
            }
        }

        private void DeleteNewBtn_Click(object sender, RoutedEventArgs e)
        {
            if (BOPDataGrid.SelectedItem == null)
                MessageBox.Show("请先选定要删除的项！");

            BOPInfo info = BOPDataGrid.SelectedItem as BOPInfo;
            if (info.ID == 0)
            {
                switch ((BOPType)(((DictionaryEntry)(BOPTypeList.SelectedValue)).Key))
                {
                    case BOPType.P:
                        PaymentBOPList.Remove(info);
                        break;
                    case BOPType.B:
                        BalanceBOPList.Remove(info);
                        break;
                }
            }
            else
            {
                MessageBox.Show("只能删除未保存项！");
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (BOPDataGrid.SelectedItem == null)
                return;

            MessageBoxResult mr = MessageBox.Show("删除该记录后不可恢复，确认删除？", "删除记录", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (mr == MessageBoxResult.OK)
            {
                BOPInfo info = BOPDataGrid.SelectedItem as BOPInfo;
                if (BOPDeal.DeleteBOPInfo(info.ID))
                {
                    int index = dataManager.AllBOPList.ToList().FindIndex(sheet => sheet.ID == info.ID);
                    if (index > -1)
                        dataManager.AllBOPList.RemoveAt(index);

                    switch ((BOPType)(((DictionaryEntry)(BOPTypeList.SelectedValue)).Key))
                    {
                        case BOPType.P:
                            index = PaymentBOPList.ToList().FindIndex(sheet => sheet.ID == info.ID);
                            if (index > -1)
                                PaymentBOPList.RemoveAt(index);
                            break;
                        case BOPType.B:
                            index = BalanceBOPList.ToList().FindIndex(sheet => sheet.ID == info.ID);
                            if (index > -1)
                                BalanceBOPList.RemoveAt(index);
                            break;
                    }

                    MessageBox.Show("删除完成！");
                }
            }
        }

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            string searchKeys = "";
            if (!string.IsNullOrEmpty(FitterGeneralText.Text.Trim()))
                searchKeys += "[关键字:" + FitterGeneralText.Text.Trim() + "]";
            if (StartDatePicker.SelectedDate != null && EndDatePicker.SelectedDate != null)
                searchKeys += (string.IsNullOrEmpty(searchKeys) ? "[" : ",[") + "日期:" + StartDatePicker.Text + " 至 " + EndDatePicker.Text + "]";

            CollectionViewSource cvs = FindResource("ViewSource") as CollectionViewSource;
            if (cvs == null) return;
            List<BOPInfo> bopList = new List<BOPInfo>();
            foreach (BOPInfo bopInfo in cvs.View)
            {
                bopList.Add(bopInfo);
            }

            if (bopList.Count == 0)
            {
                MessageBox.Show("当前列表为空,无法打印。");
                return;
            }

            string msg = "";
            bool isPrintSuccess = false;
            switch ((BOPType)(((DictionaryEntry)(BOPTypeList.SelectedValue)).Key))
            {
                case BOPType.P:
                    isPrintSuccess = PrintDeal.CreateInstance().PrintPayment(searchKeys, bopList, ref msg);
                    break;
                case BOPType.B:
                    isPrintSuccess = PrintDeal.CreateInstance().PrintBalance(searchKeys, bopList, ref msg);
                    break;
            }
            if (isPrintSuccess)
                MessageBox.Show("打印完成。");
            else
            {
                if (!string.IsNullOrEmpty(msg))
                    MessageBox.Show(msg);
            }

        }

        private bool CheckBOPInfoIsValid(BOPInfo bopInfo)
        {
            if (string.IsNullOrEmpty(bopInfo.BOPReason) || string.IsNullOrEmpty(bopInfo.AccountingSupervisorName) || string.IsNullOrEmpty(bopInfo.CashierName) || string.IsNullOrEmpty(bopInfo.HandlerName))
                return false;

            if (bopInfo.BOP_Type == BOPType.B && string.IsNullOrEmpty(bopInfo.BookKeepingName))
                return false;

            if (bopInfo.BOP_Type == BOPType.P && (string.IsNullOrEmpty(bopInfo.PayeeName) || string.IsNullOrEmpty(bopInfo.DepartmentName)))
                return false;

            return true;
        }

        private bool Do_Save(ref string msg)
        {
            ObservableCollection<BOPInfo> needUpdateList = null;
            BOPInfo tempInfo = null;
            bool isNeedUpdate = CheckedIsNeedSave(ref tempInfo, ref needUpdateList, (BOPType)(((DictionaryEntry)(BOPTypeList.SelectedValue)).Key));

            if (tempInfo != null)
            {
                if (!CheckBOPInfoIsValid(tempInfo))
                {
                    msg = "新增项或被编辑项的信息不完整，保存失败。";
                    return false;
                }

                //插入操作 
                int newID = 0;
                if (BOPDeal.InsertBOPInfo(tempInfo, ref newID))
                {
                    tempInfo.ID = newID;
                    dataManager.AllBOPList.Add(tempInfo);
                }
                else
                {
                    msg = "数据创建失败，请重试！";
                    return false;
                }
            }

            if (needUpdateList.Count != 0)
            {
                //更新操作
                foreach (BOPInfo info in needUpdateList)
                {
                    if (!CheckBOPInfoIsValid(tempInfo))
                    {
                        msg = "新增项或被编辑项的信息不完整，保存失败。";
                        return false;
                    }

                    if (BOPDeal.UpdateBOPInfo(info))
                    {
                        int index = dataManager.AllBOPList.ToList().FindIndex(bop => bop.ID == info.ID);
                        if (index > -1)
                        {
                            dataManager.AllBOPList.RemoveAt(index);
                            dataManager.AllBOPList.Insert(index, AlgorithmClass.DeepClone<BOPInfo>(info));
                        }
                    }
                    else
                    {
                        msg = "数据更新失败，请重试！";
                        return false;
                    }
                }
            }

            if (isNeedUpdate)
                msg = "数据保存完成！";
            else
                msg = "不存在需要保存项！";

            return true;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            string msg = "";
            Do_Save(ref msg);
            if (!string.IsNullOrEmpty(msg))
                MessageBox.Show(msg); 
        }

        private void RestoreBtn_Click(object sender, RoutedEventArgs e)
        {
            switch ((BOPType)(((DictionaryEntry)(BOPTypeList.SelectedValue)).Key))
            {
                case BOPType.P:
                    PaymentBOPList = AlgorithmClass.GetBOPListByBOPType(dataManager.AllBOPList, BOPType.P);
                    PaymentBOPList.CollectionChanged += PaymentBOPList_CollectionChanged;
                    InitBOPUIList(PaymentBOPList, BOPType.P);
                    break;
                case BOPType.B:
                    BalanceBOPList = AlgorithmClass.GetBOPListByBOPType(dataManager.AllBOPList, BOPType.B);
                    BalanceBOPList.CollectionChanged += BalanceBOPList_CollectionChanged;
                    InitBOPUIList(BalanceBOPList, BOPType.B);
                    break;
            }
        }

        private bool CheckedIsNeedSave(ref BOPInfo tempInfo, ref ObservableCollection<BOPInfo> needUpdateList, BOPType bopType)
        {
            if (dataManager == null)
            {
                tempInfo = new BOPInfo();
                needUpdateList = new ObservableCollection<BOPInfo>();
                return false;
            }

            ObservableCollection<BOPInfo> tempInfoList = null;
            bool isNeedUpdate = false;

            //检查新建项
            switch (bopType)
            {
                case BOPType.P:
                    tempInfo = PaymentBOPList.Where(bop => bop.ID == 0).FirstOrDefault();

                    tempInfoList = pageManager.ReFreshCurPageCollection(
                        AlgorithmClass.GetBOPListByBOPType(dataManager.AllBOPList, BOPType.P));

                    break;
                case BOPType.B:
                    tempInfo = BalanceBOPList.Where(bop => bop.ID == 0).FirstOrDefault();

                    tempInfoList = pageManager.ReFreshCurPageCollection(
                        AlgorithmClass.GetBOPListByBOPType(dataManager.AllBOPList, BOPType.B));

                    break;
            }

            if (tempInfo != null)
            {
                isNeedUpdate = true;
            }

            //检查并获取编辑项
            needUpdateList = AlgorithmClass.CompareCollectionIsChanged<BOPInfo>(tempInfoList, (FindResource("ViewSource") as CollectionViewSource).Source as ObservableCollection<BOPInfo>, "ID");
            if (needUpdateList.Count != 0)
            {
                isNeedUpdate = true;
            }

            return isNeedUpdate;
        }

        #endregion

        #region 搜索 过滤

        private void BOPTypeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BOPTypeList.SelectedItem == null || pageManager == null) return;

            switch ((BOPType)(((DictionaryEntry)(BOPTypeList.SelectedValue)).Key))
            {
                case BOPType.P:
                    InitBOPUIList(PaymentBOPList, BOPType.P);
                    break;
                case BOPType.B:
                    InitBOPUIList(BalanceBOPList, BOPType.B);
                    break;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            AlgorithmClass.CollectionViewSource_Refresh(this, "ViewSource");
            CountCurViewPrice();
        }

        private void RangeDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EndDatePicker == null || StartDatePicker == null)
                return;

            if (StartDatePicker.SelectedDate != null && EndDatePicker.SelectedDate != null)
            {
                if (StartDatePicker.SelectedDate > EndDatePicker.SelectedDate)
                {
                    DatePicker dataPicker = sender as DatePicker;
                    switch (dataPicker.Name)
                    {
                        case "StartDatePicker":
                            StartDatePicker.SelectedDate = EndDatePicker.SelectedDate;
                            //MessageBox.Show("请选择合法的日期范围值！");  
                            break;
                        case "EndDatePicker":
                            EndDatePicker.SelectedDate = StartDatePicker.SelectedDate;
                            //MessageBox.Show("请选择合法的日期范围值！");
                            break;
                    }
                }
            }

            AlgorithmClass.CollectionViewSource_Refresh(this, "ViewSource");
            CountCurViewPrice();
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            BOPInfo bopInfo = e.Item as BOPInfo;
            if (bopInfo == null) return;

            bool IsGeneralTextAccepted = true;
            if (!string.IsNullOrEmpty(FitterGeneralText.Text.Trim()))
            {
                IsGeneralTextAccepted = bopInfo.BOPReason.ToLower().Contains(FitterGeneralText.Text.ToLower());
            }

            bool IsDataTextAccepted = true;
            if (StartDatePicker.SelectedDate != null && EndDatePicker.SelectedDate != null)
            {
                if (StartDatePicker.SelectedDate <= EndDatePicker.SelectedDate)
                    IsDataTextAccepted = (bopInfo.BOPDate >= StartDatePicker.SelectedDate) && (bopInfo.BOPDate <= EndDatePicker.SelectedDate);
            }

            e.Accepted = IsGeneralTextAccepted && IsDataTextAccepted;
        }

        #endregion

        private void CountCurViewPrice()
        {
            if ((FindResource("ViewSource") as CollectionViewSource).View == null || BOPTypeList.SelectedValue==null)
                return;

            double price = 0d;
            foreach(BOPInfo bopInfo in (FindResource("ViewSource") as CollectionViewSource).View)
            {
                if (bopInfo.ID == 0)
                    continue;

                switch ((BOPType)(((DictionaryEntry)(BOPTypeList.SelectedValue)).Key))
                {
                    case BOPType.P:
                        price += bopInfo.Payment;
                        break;
                    case BOPType.B:
                        price += bopInfo.Balance;
                        break;
                }
            }
            CurTotalTextBlock.Text = price.ToString();
        }

        private void OperationTypeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OperationTypeList.SelectedItem == null || BOPTypeList.SelectedItem == null) return;

            OperationType type = (OperationType)(((DictionaryEntry)(OperationTypeList.SelectedValue)).Key);
            if (type == OperationType.Edit)
            {
                FitterGeneralText.Text = "";
                StartDatePicker.SelectedDate = null;
                EndDatePicker.SelectedDate = null;
            }
            else
            {
                ShowMsgForNOSave((BOPType)(((DictionaryEntry)(BOPTypeList.SelectedValue)).Key));
                CountCurViewPrice();
            }
        }

        private void ShowMsgForNOSave(BOPType bopType)
        {
            ObservableCollection<BOPInfo> needUpdateList = null;
            BOPInfo tempInfo = null;
            if (CheckedIsNeedSave(ref tempInfo, ref needUpdateList, bopType))
            {
                MessageBoxResult mr = MessageBox.Show("当前列表中存在未保存项，是否保存？", "", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (mr == MessageBoxResult.OK)
                {
                    string msg = "";
                    if(Do_Save(ref msg))
                    {
                        if (!string.IsNullOrEmpty(msg))
                            MessageBox.Show(msg); 
                    }
                    else
                    {
                        RestoreBtn_Click(null, new RoutedEventArgs());
                        if (!string.IsNullOrEmpty(msg))
                            MessageBox.Show(msg); 
                    }
                }
                else
                {
                    RestoreBtn_Click(null, new RoutedEventArgs());
                }
            }
        }

        private void FlushCurPrice_Click(object sender, RoutedEventArgs e)
        {
            CountCurViewPrice();
        }
    }
}
