using AppLog4Net;
using BusinessManager.DataAccess;
using BusinessManager.Helps;
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
    /// SheetMangerControl.xaml 的交互逻辑
    /// </summary>
    public partial class SheetMangerControl : UserControl
    {
        AppLog appLog = null;
        DataManager dataManager;
        PageManager pageManager;
        bool IsSavingEdit = false;

        ObservableCollection<SheetInfo> tempSheetCollection = null;

        ObservableCollection<SheetBOPInfo> CurBOPInfoList = null;
        ObservableCollection<ProcedureInfo> CurProcedureInfoList = null;
        List<SelectionProcedureInfo> CurSelectionProcedureInfoList = null;

        //用于编辑的临时Sheet信息
        ObservableCollection<ProcedureInfo> procedureInfoListForEdit = null;
        ObservableCollection<SheetBOPInfo> bopInfoListForEdit = null;
        SheetInfo sheetInfoForEdit = null;

        //用于新建的临时Sheet信息
        ObservableCollection<ProcedureInfo> procedureInfoListForNew = null;
        ObservableCollection<SheetBOPInfo> bopInfoListForNew = null;
        SheetInfo sheetInfoForNew = null;

        public SheetMangerControl()
        {
            InitializeComponent();
            appLog = new AppLog("SheetMangerControl");
            dataManager = DataManager.dataManager;
            pageManager = new PageManager(dataManager.AllSheetList.Count, 100);

            tempSheetCollection = new ObservableCollection<SheetInfo>();

            CurProcedureInfoList = new ObservableCollection<ProcedureInfo>();
            CurSelectionProcedureInfoList = new List<SelectionProcedureInfo>();

            procedureInfoListForEdit = new ObservableCollection<ProcedureInfo>();
            procedureInfoListForNew = new ObservableCollection<ProcedureInfo>();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitSheetInfoList(dataManager.AllSheetList);
        }

        private void InitSheetInfoList(ObservableCollection<SheetInfo> sheetList)
        {
            pageManager = new PageManager(sheetList.Count);
            PageManagerGrid.DataContext = pageManager;
            (FindResource("ViewSource") as CollectionViewSource).Source = pageManager.ReFreshCurPageCollection(sheetList);
            pageManager.RefreshEvent -= pageManager_RefreshEvent;
            pageManager.RefreshEvent += pageManager_RefreshEvent;
            dataManager.AllSheetList.CollectionChanged -= AllSheetList_CollectionChanged;
            dataManager.AllSheetList.CollectionChanged += AllSheetList_CollectionChanged;

            BsTypeComboBox.ItemsSource = dataManager.AllBsTypeList;
            AccepterComboBox.ItemsSource = dataManager.AllEmployeeList;
        } 

        void pageManager_RefreshEvent(object sender, EventArgs e)
        {
            if (tempSheetCollection.Count == 0)
                (FindResource("ViewSource") as CollectionViewSource).Source = pageManager.ReFreshCurPageCollection(dataManager.AllSheetList);
            else
                (FindResource("ViewSource") as CollectionViewSource).Source = pageManager.ReFreshCurPageCollection(tempSheetCollection);
        }

        void AllSheetList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DoSearchFilter(); 
        }

        private void DoSearchFilter()
        {
            if (dataManager == null)
                return;

            tempSheetCollection = new ObservableCollection<SheetInfo>(dataManager.AllSheetList.ToList());

            if (!string.IsNullOrEmpty(FitterGeneralText.Text.Trim()))
            {
                switch ((SheetSearchType)(FitterComboBox.SelectedValue))
                {
                    case SheetSearchType.SheetID:
                        int result;
                        if (int.TryParse(FitterGeneralText.Text, out result))
                        {
                            tempSheetCollection = new ObservableCollection<SheetInfo>(tempSheetCollection.Where(sheet => sheet.ID == result));
                        }
                        break;
                    case SheetSearchType.Accepter:
                        tempSheetCollection = new ObservableCollection<SheetInfo>(tempSheetCollection.Where(sheet => sheet.Accepter.ToLower().Contains(FitterGeneralText.Text.ToLower())));
                        break;
                    case SheetSearchType.CustomerName:
                        tempSheetCollection = new ObservableCollection<SheetInfo>(tempSheetCollection.Where(sheet => sheet.Customer.ToLower().Contains(FitterGeneralText.Text.ToLower())));
                        break;
                    case SheetSearchType.BusinessType:
                        tempSheetCollection = new ObservableCollection<SheetInfo>(tempSheetCollection.Where(sheet => sheet.BSType.ToLower().Contains(FitterGeneralText.Text.ToLower())));
                        break;
                }
            }

            if (StartDatePicker.SelectedDate != null && EndDatePicker.SelectedDate != null)
            {
                if (StartDatePicker.SelectedDate <= EndDatePicker.SelectedDate)
                {
                    switch ((TimeSearchType)(FitterDataComboBox.SelectedValue))
                    {
                        case TimeSearchType.AcceptSheetTime:
                            tempSheetCollection = new ObservableCollection<SheetInfo>(tempSheetCollection.Where(sheetInfo => (sheetInfo.StartDate >= StartDatePicker.SelectedDate) && (sheetInfo.StartDate <= EndDatePicker.SelectedDate)));
                            break;
                        case TimeSearchType.ComplateSheetTime:
                            tempSheetCollection = new ObservableCollection<SheetInfo>(tempSheetCollection.Where(sheetInfo => sheetInfo.FinishDate != null));
                            tempSheetCollection = new ObservableCollection<SheetInfo>(tempSheetCollection.Where(sheetInfo => (sheetInfo.FinishDate >= StartDatePicker.SelectedDate) && (sheetInfo.FinishDate <= EndDatePicker.SelectedDate)));
                            break;
                        case TimeSearchType.SettleSheetTime:
                            tempSheetCollection = new ObservableCollection<SheetInfo>(tempSheetCollection.Where(sheetInfo => sheetInfo.SettleDate != null));
                            tempSheetCollection = new ObservableCollection<SheetInfo>(tempSheetCollection.Where(sheetInfo => (sheetInfo.SettleDate >= StartDatePicker.SelectedDate) && (sheetInfo.SettleDate <= EndDatePicker.SelectedDate)));
                            break;
                    }
                }
            }

            switch ((SheetFinishState)(SheetStateComboBox.SelectedValue))
            {
                case SheetFinishState.All:
                    break;
                case SheetFinishState.Settled:
                    tempSheetCollection = new ObservableCollection<SheetInfo>(tempSheetCollection.Where(sheetInfo => sheetInfo.IsSettle == true));
                    break;
                case SheetFinishState.UnSettled:
                    tempSheetCollection = new ObservableCollection<SheetInfo>(tempSheetCollection.Where(sheetInfo => sheetInfo.IsSettle == false));
                    break;
            }

            (FindResource("ViewSource") as CollectionViewSource).Source = pageManager.ReFreshCurPageCollection(tempSheetCollection);
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            if (e.Item == null) return;
            SheetInfo sheetInfo = e.Item as SheetInfo;
            if (sheetInfo == null) return;

            bool IsGeneralTextAccepted = true;
            if (!string.IsNullOrEmpty(FitterGeneralText.Text.Trim()))
            {
                switch ((SheetSearchType)(FitterComboBox.SelectedValue))
                {
                    case SheetSearchType.SheetID:
                        int result;
                        if (int.TryParse(FitterGeneralText.Text, out result))
                        {
                            IsGeneralTextAccepted = sheetInfo.ID == result;
                        }
                        else
                            IsGeneralTextAccepted = false;
                        break;
                    case SheetSearchType.Accepter:
                        IsGeneralTextAccepted = sheetInfo.Accepter.ToLower().Contains(FitterGeneralText.Text.ToLower());
                        break;
                    case SheetSearchType.CustomerName:
                        IsGeneralTextAccepted = sheetInfo.Customer.ToLower().Contains(FitterGeneralText.Text.ToLower());
                        break;
                    case SheetSearchType.BusinessType:
                        IsGeneralTextAccepted = sheetInfo.BSType.ToLower().Contains(FitterGeneralText.Text.ToLower());
                        break;
                }
            }

            bool IsDataTextAccepted = true;
            if (StartDatePicker.SelectedDate != null && EndDatePicker.SelectedDate != null)
            {
                if (StartDatePicker.SelectedDate <= EndDatePicker.SelectedDate)
                {
                    switch ((TimeSearchType)(FitterDataComboBox.SelectedValue))
                    {
                        case TimeSearchType.AcceptSheetTime:
                            IsDataTextAccepted = (sheetInfo.StartDate >= StartDatePicker.SelectedDate) && (sheetInfo.StartDate <= EndDatePicker.SelectedDate);
                            break;
                        case TimeSearchType.ComplateSheetTime:
                            if (sheetInfo.FinishDate != null) //未完成的单 直接过滤
                                IsDataTextAccepted = (sheetInfo.FinishDate >= StartDatePicker.SelectedDate) && (sheetInfo.FinishDate <= EndDatePicker.SelectedDate);
                            else
                                IsDataTextAccepted = false;
                            break;
                        case TimeSearchType.SettleSheetTime:
                            if (sheetInfo.SettleDate != null) //未完成的单 直接过滤
                                IsDataTextAccepted = (sheetInfo.SettleDate >= StartDatePicker.SelectedDate) && (sheetInfo.SettleDate <= EndDatePicker.SelectedDate);
                            else
                                IsDataTextAccepted = false;
                            break;
                    }
                }
            }

            bool IsFinishStateAccepted = true;
            switch ((SheetFinishState)(SheetStateComboBox.SelectedValue))
            {
                case SheetFinishState.All:
                    IsFinishStateAccepted = true;
                    break;
                case SheetFinishState.Settled:
                    IsFinishStateAccepted = sheetInfo.IsSettle == true;
                    break;
                case SheetFinishState.UnSettled:
                    IsFinishStateAccepted = sheetInfo.IsSettle == false;
                    break;
            }

            e.Accepted = IsGeneralTextAccepted && IsDataTextAccepted && IsFinishStateAccepted;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DoSearchFilter();
            //AlgorithmClass.CollectionViewSource_Refresh(this, "ViewSource");
        }

        private void FitterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DoSearchFilter();
            //AlgorithmClass.CollectionViewSource_Refresh(this, "ViewSource");
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

            DoSearchFilter();
            //AlgorithmClass.CollectionViewSource_Refresh(this, "ViewSource");
        }

        private void SheetDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SheetDataGrid.SelectedItem == null || IsSavingEdit == true)
                return;

            switch ((OperationType)(((DictionaryEntry)(OperationTypeList.SelectedValue)).Key))
            {
                case OperationType.Scan:
                    UpdateProcefureInfo();
                    break;
                case OperationType.Edit:
                    UpdateProcefureInfo();
                    sheetInfoForEdit = AlgorithmClass.DeepClone<SheetInfo>(SheetDataGrid.SelectedItem as SheetInfo);
                    SheetDetailGrid.DataContext = sheetInfoForEdit;

                    procedureInfoListForEdit = CurProcedureInfoList.Count == 0 ? new ObservableCollection<ProcedureInfo>()
                        : AlgorithmClass.DeepClone<ObservableCollection<ProcedureInfo>>(CurProcedureInfoList);
                    ProcedureDataGrid.ItemsSource = procedureInfoListForEdit;

                    bopInfoListForEdit = CurBOPInfoList.Count == 0 ? new ObservableCollection<SheetBOPInfo>()
                        : AlgorithmClass.DeepClone<ObservableCollection<SheetBOPInfo>>(CurBOPInfoList);
                    BOPDataGrid.ItemsSource = bopInfoListForEdit;

                    if (procedureInfoListForEdit.Count == CurSelectionProcedureInfoList.Count)
                        AddRowBtn.IsEnabled = false;
                    else
                        AddRowBtn.IsEnabled = true;
                    break;
            }
        }

        private void UpdateProcefureInfo()
        {
            if (SheetDataGrid.SelectedItem == null)
                return;

            SheetInfo sheetInfo = SheetDataGrid.SelectedItem as SheetInfo;
            //获取该单所对应的流程信息
            CurProcedureInfoList = SheetDeal.GetAllProcedureInfo(sheetInfo.ID);
            //获取跟单员
            sheetInfo.Follower = AlgorithmClass.GetFollowers(CurProcedureInfoList);

            BsTypeInfo bsTypeInfo = dataManager.AllBsTypeList.Where(type => type.TypeName == sheetInfo.BSType).FirstOrDefault();
            //获取该单中业务类型的全部流程信息 供选择更改
            CurSelectionProcedureInfoList = AlgorithmClass.GetSelectionProcedureInfoList(bsTypeInfo.ProcedurePrice);
            CurProcedureInfoList.ToList().ForEach(info => info.SelectionProcedureInfoList = CurSelectionProcedureInfoList);

            CurBOPInfoList = SheetDeal.GetAllSheetBOPInfo(sheetInfo.ID);
            TotalTextBlock.Text = AlgorithmClass.GetBOPTotal(CurBOPInfoList).ToString();

            //绑定
            ProcedureDataGrid.ItemsSource = CurProcedureInfoList;
            BOPDataGrid.ItemsSource = CurBOPInfoList;

            if (CurProcedureInfoList.Count == CurSelectionProcedureInfoList.Count)
                AddRowBtn.IsEnabled = false;
            else
                AddRowBtn.IsEnabled = true;
        }

        private void OperationTypeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OperationTypeList.SelectedItem == null)
                return;

            switch ((OperationType)(((DictionaryEntry)(OperationTypeList.SelectedValue)).Key))
            {
                case OperationType.Scan:
                    if (SheetDataGrid.SelectedValue != null)
                        SheetDetailGrid.SetBinding(Grid.DataContextProperty, new Binding("SelectedValue") { Source = SheetDataGrid });
                    UpdateProcefureInfo();
                    break;
                case OperationType.Edit:
                    sheetInfoForEdit = AlgorithmClass.DeepClone<SheetInfo>(SheetDataGrid.SelectedItem as SheetInfo);
                    SheetDetailGrid.DataContext = sheetInfoForEdit;

                    procedureInfoListForEdit = AlgorithmClass.DeepClone<ObservableCollection<ProcedureInfo>>(CurProcedureInfoList);
                    ProcedureDataGrid.ItemsSource = procedureInfoListForEdit;

                    bopInfoListForEdit = AlgorithmClass.DeepClone<ObservableCollection<SheetBOPInfo>>(CurBOPInfoList);
                    BOPDataGrid.ItemsSource = bopInfoListForEdit;

                    if (procedureInfoListForEdit.Count == CurSelectionProcedureInfoList.Count)
                        AddRowBtn.IsEnabled = false;
                    else
                        AddRowBtn.IsEnabled = true;
                    break;
                case OperationType.New:
                    BSTypeTextBlock.Text = "";
                    sheetInfoForNew = new SheetInfo();
                    SheetDetailGrid.DataContext = sheetInfoForNew;

                    procedureInfoListForNew = new ObservableCollection<ProcedureInfo>();
                    ProcedureDataGrid.ItemsSource = procedureInfoListForNew;
                    AddRowBtn.IsEnabled = true;

                    bopInfoListForNew = new ObservableCollection<SheetBOPInfo>();
                    BOPDataGrid.ItemsSource = bopInfoListForNew;
                    break;
            }
        }

        private void BsTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BsTypeComboBox.SelectedItem == null)
                return;

            //             if (BsTypeComboBox.IsDropDownOpen == false)
            //                 return;

            BsTypeInfo bsTypeInfo = BsTypeComboBox.SelectedItem as BsTypeInfo;
            BSTypeTextBlock.Text = bsTypeInfo.TypeName;
            CurSelectionProcedureInfoList = AlgorithmClass.GetSelectionProcedureInfoList(bsTypeInfo.ProcedurePrice);

            switch ((OperationType)(((DictionaryEntry)(OperationTypeList.SelectedValue)).Key))
            {
                case OperationType.Edit:
                    procedureInfoListForEdit.Clear();
                    AddRowBtn.IsEnabled = true;
                    break;
                case OperationType.New:
                    procedureInfoListForNew.Clear();
                    AddRowBtn.IsEnabled = true;
                    break;
            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker datePicker = sender as DatePicker;
            switch (datePicker.Name)
            {
                case "acceptDataPicker":
                    if (acceptDataPicker.IsDropDownOpen == false)
                        return;

                    if (finishDataPicker.SelectedDate != null)
                    {
                        if (datePicker.SelectedDate > finishDataPicker.SelectedDate)
                        {
                            MessageBox.Show("接单日期不能大于完成日期或结单日期！");
                            datePicker.SelectedDate = finishDataPicker.SelectedDate;
                            return;
                        }
                    }
                    if (settleDataPicker.SelectedDate != null)
                    {
                        if (datePicker.SelectedDate > settleDataPicker.SelectedDate)
                        {
                            MessageBox.Show("接单日期不能大于完成日期或结单日期！");
                            datePicker.SelectedDate = settleDataPicker.SelectedDate;
                        }
                    }
                    break;
                case "finishDataPicker":
                    if (finishDataPicker.IsDropDownOpen == false)
                        return;

                    if (acceptDataPicker.SelectedDate != null)
                    {
                        if (datePicker.SelectedDate < acceptDataPicker.SelectedDate)
                        {
                            MessageBox.Show("完成日期必须介于接单日期与结单日期之间！");
                            datePicker.SelectedDate = acceptDataPicker.SelectedDate;
                            return;
                        }
                    }
                    if (settleDataPicker.SelectedDate != null)
                    {
                        if (datePicker.SelectedDate > settleDataPicker.SelectedDate)
                        {
                            MessageBox.Show("完成日期必须介于接单日期与结单日期之间！");
                            datePicker.SelectedDate = settleDataPicker.SelectedDate;
                        }
                    }
                    break;
                case "settleDataPicker":
                    if (settleDataPicker.IsDropDownOpen == false)
                        return;

                    if (acceptDataPicker.SelectedDate != null)
                    {
                        if (datePicker.SelectedDate < acceptDataPicker.SelectedDate)
                        {
                            MessageBox.Show("结单日期不能小于接单日期及完成日期！");
                            datePicker.SelectedDate = acceptDataPicker.SelectedDate;
                            return;
                        }
                    }
                    if (finishDataPicker.SelectedDate != null)
                    {
                        if (datePicker.SelectedDate < finishDataPicker.SelectedDate)
                        {
                            MessageBox.Show("结单日期不能小于接单日期及完成日期！");
                            datePicker.SelectedDate = finishDataPicker.SelectedDate;
                        }
                    }
                    break;
            }
        }

        #region 流程的增加 与 删除

        private void AddRowBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(BSTypeTextBlock.Text))
            {
                MessageBox.Show("请先选择业务类型！");
                return;
            }

            switch ((OperationType)(((DictionaryEntry)(OperationTypeList.SelectedValue)).Key))
            {
                case OperationType.Edit:
                    procedureInfoListForEdit.Add(new ProcedureInfo
                    {
                        SelectionProcedureInfoList = CurSelectionProcedureInfoList,
                        SelectionEmployeeInfoList = dataManager.AllEmployeeList.ToList()
                    });
                    if (procedureInfoListForEdit.Count == CurSelectionProcedureInfoList.Count)
                        AddRowBtn.IsEnabled = false;
                    break;
                case OperationType.New:
                    procedureInfoListForNew.Add(new ProcedureInfo
                    {
                        SelectionProcedureInfoList = CurSelectionProcedureInfoList,
                        SelectionEmployeeInfoList = dataManager.AllEmployeeList.ToList()
                    });
                    if (procedureInfoListForNew.Count == CurSelectionProcedureInfoList.Count)
                        AddRowBtn.IsEnabled = false;
                    break;
            }
        }

        private void DeleteRowBtn_Click(object sender, RoutedEventArgs e)
        {
            switch ((OperationType)(((DictionaryEntry)(OperationTypeList.SelectedValue)).Key))
            {
                case OperationType.Edit:
                    if (procedureInfoListForEdit.Count != 0)
                    {
                        procedureInfoListForEdit.RemoveAt(procedureInfoListForEdit.Count - 1);
                        AddRowBtn.IsEnabled = true;
                    }
                    break;
                case OperationType.New:
                    if (procedureInfoListForNew.Count != 0)
                    {
                        procedureInfoListForNew.RemoveAt(procedureInfoListForNew.Count - 1);
                        AddRowBtn.IsEnabled = true;
                    }
                    break;
            }
        }

        #endregion

        #region  收支的增加 与 删除

        private void BOP_AddRowBtn_Click(object sender, RoutedEventArgs e)
        {
            switch ((OperationType)(((DictionaryEntry)(OperationTypeList.SelectedValue)).Key))
            {
                case OperationType.Edit:
                    bopInfoListForEdit.Add(new SheetBOPInfo());
                    break;
                case OperationType.New:
                    bopInfoListForNew.Add(new SheetBOPInfo());
                    break;
            }
        }

        private void BOP_DeleteRowBtn_Click(object sender, RoutedEventArgs e)
        {
            switch ((OperationType)(((DictionaryEntry)(OperationTypeList.SelectedValue)).Key))
            {
                case OperationType.Edit:
                    if (bopInfoListForEdit.Count != 0)
                        bopInfoListForEdit.RemoveAt(bopInfoListForEdit.Count - 1);
                    break;
                case OperationType.New:
                    if (bopInfoListForNew.Count != 0)
                        bopInfoListForNew.RemoveAt(bopInfoListForNew.Count - 1);
                    break;
            }
        }

        #endregion

        #region 针对Sheet的操作:保存 删除 打印

        private bool CheckSheetIsValid(SheetInfo sheetInfo)
        {
            if (string.IsNullOrEmpty(sheetInfo.Accepter) || string.IsNullOrEmpty(sheetInfo.BSType) || string.IsNullOrEmpty(sheetInfo.Contacter)
                || string.IsNullOrEmpty(sheetInfo.Customer) || string.IsNullOrEmpty(sheetInfo.PhoneNumber) || sheetInfo.StartDate == null)
                return false;
            else
                return true;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            switch ((OperationType)(((DictionaryEntry)(OperationTypeList.SelectedValue)).Key))
            {
                case OperationType.Edit:

                    if (!CheckSheetIsValid(SheetDataGrid.SelectedItem as SheetInfo))
                    {
                        MessageBox.Show("请检查该单必填信息是否完整。", "信息不完整", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    bool IsChanged = AlgorithmClass.CheckIsChanged<SheetInfo>(SheetDataGrid.SelectedItem as SheetInfo, sheetInfoForEdit);

                    //单信息
                    if (IsChanged == true)
                    {
                        IsSavingEdit = true;
                        if (SheetDeal.UpdateSheetInfo(sheetInfoForEdit) == true)
                        {
                            //获得所有跟单员
                            sheetInfoForEdit.Follower = AlgorithmClass.GetFollowers(procedureInfoListForEdit);
                            int index = dataManager.AllSheetList.ToList().FindIndex(sheet => sheet.ID == sheetInfoForEdit.ID);
                            dataManager.AllSheetList.RemoveAt(index);
                            dataManager.AllSheetList.Insert(index, AlgorithmClass.DeepClone<SheetInfo>(sheetInfoForEdit));
                            SheetDataGrid.SelectedIndex = index;
                        }
                        else
                        {
                            MessageBox.Show("单保存失败，请重试！", "操作失败", MessageBoxButton.OK, MessageBoxImage.Error);
                            IsSavingEdit = false;
                            return;
                        }
                    }

                    //流程信息
                    string procedureMap = AlgorithmClass.GetProcedureMap(procedureInfoListForEdit);
                    if (procedureMap != AlgorithmClass.GetProcedureMap(CurProcedureInfoList))
                    {
                        IsSavingEdit = true;
                        if (SheetDeal.UpdateProcedureMap(sheetInfoForEdit.ID, procedureMap) == false)
                        {
                            MessageBox.Show("流程信息保存失败，请重试！", "操作失败", MessageBoxButton.OK, MessageBoxImage.Error);
                            IsSavingEdit = false;
                            return;
                        }
                    }

                    //收支信息
                    string bopMap = AlgorithmClass.GetBOPMap(bopInfoListForEdit);
                    if (bopMap != AlgorithmClass.GetBOPMap(CurBOPInfoList))
                    {
                        IsSavingEdit = true;
                        if (SheetDeal.UpdateSheetBOPMap(sheetInfoForEdit.ID, bopMap) == false)
                        {
                            MessageBox.Show("收支信息保存失败，请重试！");
                            IsSavingEdit = false;
                            return;
                        }
                    }

                    if (IsSavingEdit == true)
                    {
                        MessageBox.Show("数据更新成功！");
                        IsSavingEdit = false;
                    }

                    break;
                case OperationType.New:

                    if (!CheckSheetIsValid(sheetInfoForNew))
                    {
                        MessageBox.Show("请检查该单必填信息是否完整。", "信息不完整", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    int insertID = 0;
                    IsSavingEdit = true;
                    //单信息
                    if (SheetDeal.InsertSheetInfo(sheetInfoForNew, ref insertID) == true)
                    {
                        sheetInfoForNew.ID = insertID;
                        sheetInfoForNew.Follower = AlgorithmClass.GetFollowers(procedureInfoListForNew);
                        dataManager.AllSheetList.Add(AlgorithmClass.DeepClone<SheetInfo>(sheetInfoForNew));
                    }
                    else
                    {
                        MessageBox.Show("新单创建失败，请重试！");
                        IsSavingEdit = false;
                        return;
                    }

                    IsSavingEdit = false;

                    //流程信息
                    if (SheetDeal.InsertProcedureMap(sheetInfoForNew.ID, AlgorithmClass.GetProcedureMap(procedureInfoListForNew)) == false)
                    {
                        MessageBox.Show("流程信息创建失败，请重试！", "操作失败", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    //收支信息
                    if (SheetDeal.InsertSheetBOPMap(sheetInfoForNew.ID, AlgorithmClass.GetBOPMap(bopInfoListForNew)) == false)
                    {
                        MessageBox.Show("收支信息创建失败，请重试！", "操作失败", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    sheetInfoForNew = new SheetInfo();
                    SheetDetailGrid.DataContext = sheetInfoForNew;
                    procedureInfoListForNew.Clear();
                    bopInfoListForNew.Clear();

                    MessageBox.Show("新单创建成功！");

                    break;
            }

        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SheetDataGrid.SelectedItem == null)
                return;

            MessageBoxResult mr = MessageBox.Show("删除该订单后不可恢复，确认删除？", "删除订单", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (mr == MessageBoxResult.OK)
            {
                FuncPasswordWindow funcPDWindows = new FuncPasswordWindow(dataManager.CurUserName);
                funcPDWindows.Owner = App.Current.MainWindow;
                if (funcPDWindows.ShowDialog() == true)
                {
                    SheetInfo info = SheetDataGrid.SelectedItem as SheetInfo;
                    if (SheetDeal.DeleteSheetInfo(info.ID))
                    {
                        int index = dataManager.AllSheetList.ToList().FindIndex(sheet => sheet.ID == info.ID);
                        if (index > -1)
                            dataManager.AllSheetList.RemoveAt(index);
                    }
                }
            }
        }

        private string GetSearchKeys()
        {
            string searchKeys = "[状态:" + SheetStateComboBox.Text + "]";
            if (!string.IsNullOrEmpty(FitterGeneralText.Text.Trim()))
                searchKeys += ",[关键字:(" + FitterComboBox + "):" + FitterGeneralText.Text.Trim() + "]";
            if (StartDatePicker.SelectedDate != null && EndDatePicker.SelectedDate != null)
                searchKeys += ",[" + FitterDataComboBox.Text + ":" + StartDatePicker.Text + " 至 " + EndDatePicker.Text + "]";

            return searchKeys;
        }

        private void PrintLeftBtn_Click(object sender, RoutedEventArgs e)
        {
            CollectionViewSource cvs = FindResource("ViewSource") as CollectionViewSource;
            if (cvs == null) return;

            List<SheetInfo> sheetList = new List<SheetInfo>();
            foreach (SheetInfo sheetInfo in cvs.View)
            {
                sheetList.Add(sheetInfo);
            }

            if (sheetList.Count == 0)
            {
                MessageBox.Show("当前列表为空,无法打印。");
                return;
            }

            string msg = "";
            if (PrintDeal.CreateInstance().PrintSheetLeftSheet(GetSearchKeys(), sheetList, ref msg))
                MessageBox.Show("打印完成。");
            else
            {
                if (!string.IsNullOrEmpty(msg))
                    MessageBox.Show(msg);
            }
        }

        private void PrintDebitBtn_Click(object sender, RoutedEventArgs e)
        {
            CollectionViewSource cvs = FindResource("ViewSource") as CollectionViewSource;
            if (cvs == null) return;

            if (cvs.View.CurrentItem == null) return;
            SheetInfo sheetInfo = cvs.View.CurrentItem as SheetInfo;
            if (sheetInfo.FinishDate == null)
            {
                MessageBox.Show("未完成的单不能打印费用清单。");
                return;
            }

            string msg = "";
            if (PrintDeal.CreateInstance().PrintDebitNote(sheetInfo, CurBOPInfoList.ToList(), ref msg))
                MessageBox.Show("打印完成。");
            else
            {
                if (!string.IsNullOrEmpty(msg))
                    MessageBox.Show(msg);
            }
        }

        private void PrintRightBtn_Click(object sender, RoutedEventArgs e)
        {
            CollectionViewSource cvs = FindResource("ViewSource") as CollectionViewSource;
            if (cvs == null) return;
            if (cvs.View.CurrentItem == null) return;

            SheetInfo sheetInfo = cvs.View.CurrentItem as SheetInfo;

            string msg = "";
            if (PrintDeal.CreateInstance().PrintSheetRightSheet(sheetInfo, CurProcedureInfoList.ToList(), CurBOPInfoList.ToList(), ref msg))
                MessageBox.Show("打印完成。");
            else
            {
                if (!string.IsNullOrEmpty(msg))
                    MessageBox.Show(msg);
            }
        }

        #endregion

        #region 分页

        private void FirstPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tempSheetCollection.Count == 0)
                (FindResource("ViewSource") as CollectionViewSource).Source = pageManager.GetFirstPageCollection(dataManager.AllSheetList);
            else
                (FindResource("ViewSource") as CollectionViewSource).Source = pageManager.GetFirstPageCollection(tempSheetCollection);
        }

        private void PrePageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tempSheetCollection.Count == 0)
                (FindResource("ViewSource") as CollectionViewSource).Source = pageManager.GetPrePageCollection(dataManager.AllSheetList);
            else
                (FindResource("ViewSource") as CollectionViewSource).Source = pageManager.GetPrePageCollection(tempSheetCollection);
        }

        private void NextPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tempSheetCollection.Count == 0)
                (FindResource("ViewSource") as CollectionViewSource).Source = pageManager.GetNextPageCollection(dataManager.AllSheetList);
            else
                (FindResource("ViewSource") as CollectionViewSource).Source = pageManager.GetNextPageCollection(tempSheetCollection);
        }

        private void LastPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tempSheetCollection.Count == 0)
                (FindResource("ViewSource") as CollectionViewSource).Source = pageManager.GetLastPageCollection(dataManager.AllSheetList);
            else
                (FindResource("ViewSource") as CollectionViewSource).Source = pageManager.GetLastPageCollection(tempSheetCollection);
        }

        #endregion

        #region 滚动内容拖动

        Point? lastDragPoint = null;

        private void ScrollViewer_MouseLeave(object sender, MouseEventArgs e)
        {
            ScrollViewer sv = sender as ScrollViewer;
            sv.Cursor = Cursors.Arrow;
            lastDragPoint = null;
        }

        private void ScrollViewer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            ScrollViewer sv = sender as ScrollViewer;
            if (lastDragPoint.HasValue)
            {
                Point posNow = e.GetPosition(sv);
                double dX = posNow.X - lastDragPoint.Value.X;
                double dY = posNow.Y - lastDragPoint.Value.Y;

                lastDragPoint = posNow;
                sv.ScrollToHorizontalOffset(sv.HorizontalOffset - dX);
                sv.ScrollToVerticalOffset(sv.VerticalOffset - dY);
            }
        }

        private void ScrollViewer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ScrollViewer sv = sender as ScrollViewer;
            var mousePos = e.GetPosition(sv);
            if (mousePos.X <= sv.ViewportWidth && mousePos.Y < sv.ViewportHeight)
            {
                sv.Cursor = Cursors.Hand;
                lastDragPoint = mousePos;
            }
        }

        private void ScrollViewer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ScrollViewer sv = sender as ScrollViewer;
            sv.Cursor = Cursors.Arrow;
            lastDragPoint = null;
        }

        #endregion
    }
}
