using AppLog4Net;
using BusinessManager.DataAccess;
using BusinessManager.Helps;
using BusinessManager.Models;
using DataAccess.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// SalaryManagerControl.xaml 的交互逻辑
    /// </summary>
    public partial class SalaryManagerControl : UserControl
    {
        AppLog appLog = new AppLog("SalaryManagerControl");
        DataManager dataManager;

        ListCollectionView lcv;
        //临时列表
        ObservableCollection<SalaryInfo> AllSalaryList = null;
        ObservableCollection<SalaryInfo> needAddList = null;
        ObservableCollection<SalaryInfo> needUpdateList = null;

        public SalaryManagerControl()
        {
            InitializeComponent();
             
            dataManager = DataManager.dataManager;
            AllSalaryList = new ObservableCollection<SalaryInfo>();

            (FindResource("YearViewSource") as CollectionViewSource).Source = AlgorithmClass.GetYearList();
            (FindResource("MonthViewSource") as CollectionViewSource).Source = AlgorithmClass.GetMonthList();

            YearComboBox.SelectedItem = DateTime.Now.Year;
            MonthComboBox.SelectedItem = DateTime.Now.Month;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitUIList();
        }

        private void InitUIList()
        {
            appLog.Info("InitUIList...");

            needAddList = new ObservableCollection<SalaryInfo>();
            needUpdateList = new ObservableCollection<SalaryInfo>();
            //添加雇员选择列表
            appLog.Info("Add EmployeeList...");
            AlgorithmClass.InitSalarySelectionList(dataManager.AllSalaryList, dataManager.AllEmployeeList, dataManager.AllDepartmentList, AlgorithmClass.GetWorkStatusList());
            AllSalaryList = AlgorithmClass.DeepClone<ObservableCollection<SalaryInfo>>(dataManager.AllSalaryList);
            lcv = new ListCollectionView(AllSalaryList);
            lcv.GroupDescriptions.Add(new PropertyGroupDescription("DepartmentName"));
            lcv.Filter = Fitter_TextChanged;
            lcv.Refresh();
            //刷新当前页面上的所有收支
            // FlushCurrentViewAllCommission();
            AddBtn.SetBinding(Button.IsEnabledProperty, new Binding("IsEmpty") { Source = lcv });
            ClearBtn.SetBinding(Button.IsEnabledProperty, new Binding("IsEmpty") { Source = lcv, Converter = new BoolConverter() });
            SaveBtn.SetBinding(Button.IsEnabledProperty, new Binding("IsEmpty") { Source = lcv, Converter = new BoolConverter() });
            RestoreBtn.SetBinding(Button.IsEnabledProperty, new Binding("IsEmpty") { Source = lcv, Converter = new BoolConverter() });
            ViewBSBtn.SetBinding(Button.IsEnabledProperty, new Binding("IsEmpty") { Source = lcv, Converter = new BoolConverter() });
            PrintBtn.SetBinding(Button.IsEnabledProperty, new Binding("IsEmpty") { Source = lcv, Converter = new BoolConverter() });
            FlushAllBtn.SetBinding(Button.IsEnabledProperty, new Binding("IsEmpty") { Source = lcv, Converter = new BoolConverter() });

            SalaryDataGrid.ItemsSource = lcv;
        }

        #region 过滤 搜索

        private bool Fitter_TextChanged(object obj)
        {
            if (obj == null) return true;
            SalaryInfo salaryInfo = obj as SalaryInfo;
            if (salaryInfo == null) return true;

            bool IsGeneralTextAccepted = true;
            if (!string.IsNullOrEmpty(FitterGeneralText.Text.Trim()))
            {
                switch ((EmployeeSearchType)(FitterSearchComboBox.SelectedValue))
                {
                    /*
                    case EmployeeSearchType.EmployeeID:
                        int result;
                        if (int.TryParse(FitterGeneralText.Text, out result))
                        {
                            IsGeneralTextAccepted = salaryInfo.ID == result;
                        }
                        else
                            IsGeneralTextAccepted = false;
                        break;
                         * */
                    case EmployeeSearchType.EmployeeName:
                        IsGeneralTextAccepted = salaryInfo.EmployeeName.ToLower().Contains(FitterGeneralText.Text.ToLower());
                        break;
                    case EmployeeSearchType.DepartmentName:
                        IsGeneralTextAccepted = salaryInfo.DepartmentName.ToLower().Contains(FitterGeneralText.Text.ToLower());
                        break;
                }
            }

            bool IsYearTextAccepted = true;
            if (YearComboBox.SelectedItem != null)
                IsYearTextAccepted = salaryInfo.Year == (int)(YearComboBox.SelectedItem);

            bool IsMonthTextAccepted = true;
            if (MonthComboBox.SelectedItem != null)
                IsMonthTextAccepted = salaryInfo.Month == (int)(MonthComboBox.SelectedItem);

            return IsGeneralTextAccepted && IsYearTextAccepted && IsMonthTextAccepted;
        }

        private void FitterSearchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lcv == null) return;
            lcv.Refresh();
        }

        private void FitterGeneralText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lcv == null) return;
            lcv.Refresh();
        }

        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (YearComboBox.SelectedItem == null || lcv == null) return;
            lcv.Refresh();
        }

        private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MonthComboBox.SelectedItem == null || lcv == null) return;
            lcv.Refresh();
        }

        #endregion

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (lcv.CanAddNewItem)
            {
                foreach (EmployeeInfo info in dataManager.AllEmployeeList)
                {
                    //离职的不允许添加
                    if (info.StatusID == 0)
                        continue;

                    appLog.Info("Add Employee,ID = " + info.ID);
                    lcv.AddNewItem(new SalaryInfo
                    {
                        Year = (int)(YearComboBox.SelectedItem),
                        Month = (int)(MonthComboBox.SelectedItem),
                        EmployeeID = info.ID,
                        SelectionEmployeeInfoList = dataManager.AllEmployeeList.ToList(),
                        SelectionDepartmentInfoList = dataManager.AllDepartmentList.ToList(),
                        SelectionWorkStatusInfoList = AlgorithmClass.GetWorkStatusList()
                    });

                    lcv.CommitNew();
                }
            }
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            bool isExistNew = false;
            foreach (SalaryInfo info in lcv)
            {
                if (info.ID == 0)
                {
                    isExistNew = true;
                    break;
                }
            }

            if (!isExistNew)
                MessageBox.Show("暂不支持删除已经存在工资记录.");
            else
            {
                while (lcv.Count > 0)
                {
                    lcv.RemoveAt(0);
                }
            }
        }

        private void RestoreBtn_Click(object sender, RoutedEventArgs e)
        {
            appLog.Info("Restore...");
            if (CheckedIsNeedSave())
                InitUIList();
            else
                MessageBox.Show("当前未做任何改动.");
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckedIsNeedSave())
            {
                MessageBox.Show("尚无需要保存的项。");
                return;
            }

            if (DoSave())
                MessageBox.Show("数据保存成功！");
            else
            {
                needAddList.Clear();
                needUpdateList.Clear();
                MessageBox.Show("数据保存失败，请重试！", "操作失败", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool DoSave()
        {
            bool isSaved = false;
            if (needAddList.Count != 0)
            {
                appLog.Info("Insert New Item.needAddList.Count = " + needAddList.Count);
                //执行插入
                foreach (SalaryInfo info in needAddList)
                {
                    int insertID = 0;
                    if (SalaryDeal.InsertSalaryInfo(info, ref insertID))
                    {
                        info.ID = insertID;
                    }
                    else
                    {
                        return false;
                    }
                }

                needAddList.Clear();
                isSaved = true;
            }

            if (needUpdateList.Count != 0)
            {
                //执行更新
                appLog.Info("Update New Item.needAddList.Count = " + needUpdateList.Count);
                foreach (SalaryInfo info in needUpdateList)
                {
                    if (!SalaryDeal.UpdateSalaryInfo(info))
                    {
                        return false;
                    }
                }

                needUpdateList.Clear();
                isSaved = true;
            }

            if (isSaved)
                dataManager.AllSalaryList = AlgorithmClass.DeepClone<ObservableCollection<SalaryInfo>>(AllSalaryList);

            return isSaved;
        }

        private void OperationTypeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            appLog.Info("OperationTypeList_SelectionChanged(...)");
            OperationType type = (OperationType)(((DictionaryEntry)(OperationTypeList.SelectedValue)).Key);
            if (type == OperationType.Scan)
            {
                if (CheckedIsNeedSave())
                {
                    MessageBoxResult mr = MessageBox.Show("数据已被更改且尚未保存，是否保存？", "是否保存", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (mr == MessageBoxResult.Yes)
                    {
                        //执行保存
                        if (DoSave())
                            MessageBox.Show("数据保存成功！");
                        else
                        {
                            InitUIList();
                            MessageBox.Show("数据保存失败！", "操作失败", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                        InitUIList();
                }
            }
            else
            {
                FitterGeneralText.Text = "";
            }
        }

        private bool CheckedIsNeedSave()
        {
            appLog.Info("Check If Need Save...");
            if (AllSalaryList == null)
            {
                appLog.Info("AllSalaryList==null,return.");
                return false;
            }
            //检查添加项
            if (AllSalaryList.Count != dataManager.AllSalaryList.Count)
            {
                appLog.InfoFormat("Check New Item,AllSalaryList.Count={0},dataManager.AllSalaryList.Count={1}", AllSalaryList.Count, dataManager.AllSalaryList.Count);
                needAddList = new ObservableCollection<SalaryInfo>(AllSalaryList.Where(s => s.ID == 0));
                return true;
            }
            else
            {
                //检查并获取编辑项
                appLog.Info("Check and Get edited Items.");
                var tempList = new ObservableCollection<SalaryInfo>(AllSalaryList.Where(s => s.Year == (int)YearComboBox.SelectedItem && s.Month == (int)MonthComboBox.SelectedItem));
                needUpdateList = AlgorithmClass.CompareCollectionIsChanged<SalaryInfo>(dataManager.AllSalaryList, tempList, "ID");
                if (needUpdateList.Count != 0)
                {
                    return true;
                }
            }

            return false;
        }

        private void ViewBSBtn_Click(object sender, RoutedEventArgs e)
        {
            if (lcv.CurrentItem == null)
                return;

            SalaryProcedureInfoWrap salaryProcedureInfoWrap = new SalaryProcedureInfoWrap();
            SalaryInfo info = Do_GetSalaryProcedureInfo(lcv.CurrentItem as SalaryInfo, ref salaryProcedureInfoWrap);
            if (info != null)
            {
                ProcedureInfoPanel.DataContext = salaryProcedureInfoWrap;
                ProcedureInfoPanel.Visibility = Visibility.Visible;
            }
        }

        private void ProcedureClose_Click(object sender, RoutedEventArgs e)
        {
            ProcedureInfoPanel.Visibility = Visibility.Collapsed;
        }

        private void ProcedurePrint_Click(object sender, RoutedEventArgs e)
        {
            if (lcv.CurrentItem == null)
                return;

            SalaryProcedureInfoWrap salaryProcedureInfoWrap = new SalaryProcedureInfoWrap();
            SalaryInfo info = Do_GetSalaryProcedureInfo(lcv.CurrentItem as SalaryInfo, ref salaryProcedureInfoWrap);
            if (info != null)
            {
                string msg = "";
                if (PrintDeal.CreateInstance().PrintSalarySheet(info, salaryProcedureInfoWrap, ref msg))
                    MessageBox.Show("打印完成。");
                else
                {
                    if (!string.IsNullOrEmpty(msg))
                        MessageBox.Show(msg);
                }
            }
        }

        private SalaryInfo Do_GetSalaryProcedureInfo(SalaryInfo salaryInfo, ref SalaryProcedureInfoWrap salaryProcedureInfoWrap)
        {
            if (salaryInfo == null) return null;

            bool isUpdateTotalPrice = false;
            salaryProcedureInfoWrap = GetSalaryProcedureInfo(salaryInfo, ref isUpdateTotalPrice);
            if (isUpdateTotalPrice)
            {
                SalaryDeal.UpdateSalaryInfo(salaryInfo);
                dataManager.AllSalaryList = AlgorithmClass.DeepClone<ObservableCollection<SalaryInfo>>(AllSalaryList);
            }

            return salaryInfo;
        }

        private SalaryProcedureInfoWrap GetSalaryProcedureInfo(SalaryInfo salaryInfo, ref bool isUpdateTotalPrice)
        {
            SalaryProcedureInfoWrap salaryProcedureInfoWrap = new SalaryProcedureInfoWrap()
            {
                EmployeeName = salaryInfo.EmployeeName,
                DepartmentName = salaryInfo.DepartmentName,
                CurrentDate = new DateTime(salaryInfo.Year, salaryInfo.Month, 1)
            };

            ObservableCollection<ProcedureInfo> CurProcedureInfoList = null;
            foreach (SheetInfo sheetInfo in dataManager.AllSheetList)
            {
                //结单了
                if (sheetInfo.SettleDate == null)
                    continue;

                //本月内
                DateTime dateValue = sheetInfo.SettleDate.Value;
                if (dateValue.Year == salaryInfo.Year && dateValue.Month == salaryInfo.Month)
                {
                    CurProcedureInfoList = SheetDeal.GetAllProcedureInfo(sheetInfo.ID);
                    if (CurProcedureInfoList == null)
                        continue;

                    CurProcedureInfoList.ToList().ForEach(procedureInfo =>
                    {
                        //该人参与该流程
                        if (procedureInfo.EmployeeID == salaryInfo.EmployeeID)
                        {
                            salaryProcedureInfoWrap.TotalPrice += procedureInfo.Price;
                            salaryProcedureInfoWrap.SalaryProcedureInfoList.Add(new SalaryProcedureInfo
                            {
                                SheetID = sheetInfo.ID,
                                SettleDate = dateValue,
                                BSType = sheetInfo.BSType,
                                Customer = sheetInfo.Customer,
                                ProcedureName = procedureInfo.ProcedureName,
                                Price = procedureInfo.Price
                            });
                        }
                    });
                }
            }
            if (salaryInfo.Commission != salaryProcedureInfoWrap.TotalPrice)
            {
                isUpdateTotalPrice = true;
                salaryInfo.Commission = salaryProcedureInfoWrap.TotalPrice;
            }

            return salaryProcedureInfoWrap;
        }

        private void FlushCommission_Click(object sender, RoutedEventArgs e)
        {
            if (lcv.CurrentItem == null) return;

            SalaryProcedureInfoWrap salaryProcedureInfoWrap = new SalaryProcedureInfoWrap();
            SalaryInfo info = Do_GetSalaryProcedureInfo(lcv.CurrentItem as SalaryInfo, ref salaryProcedureInfoWrap);
            if (info != null)
            {
                MessageBox.Show("统计完成！");
            }
        }

        private void FlushCurrentViewAllCommission()
        {
            foreach (SalaryInfo info in lcv)
            {
                SalaryProcedureInfoWrap salaryProcedureInfoWrap = new SalaryProcedureInfoWrap();
                Do_GetSalaryProcedureInfo(info, ref salaryProcedureInfoWrap);
            }
        }

        private void FlushAllBtn_Click(object sender, RoutedEventArgs e)
        {
            FlushCurrentViewAllCommission();
            MessageBox.Show("全部提成刷新完成.");
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (ProcedureInfoPanel.Visibility == Visibility.Visible)
                ProcedureInfoPanel.Visibility = Visibility.Collapsed;
        }
    }
}
