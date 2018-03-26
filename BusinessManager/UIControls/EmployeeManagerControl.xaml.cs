using AppLog4Net;
using BusinessManager.DataAccess;
using BusinessManager.Helps;
using DataAccess.Helps;
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
    /// EmployeeManagerControl.xaml 的交互逻辑
    /// </summary>
    public partial class EmployeeManagerControl : UserControl
    {
        AppLog appLog = null;
        DataManager dataManager;

        ListCollectionView lcv;
        //临时列表
        ObservableCollection<EmployeeInfo> AllEmployeeList = null;
        ObservableCollection<EmployeeInfo> needAddList = null;
        ObservableCollection<EmployeeInfo> needUpdateList = null;

        public EmployeeManagerControl()
        {
            InitializeComponent();

            appLog = new AppLog("EmployeeManagerControl");
            dataManager = DataManager.dataManager;
            AllEmployeeList = new ObservableCollection<EmployeeInfo>();

            (FindResource("DepartmentViewSource") as CollectionViewSource).Source = dataManager.AllDepartmentList;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitUIList();
        }

        private void InitUIList()
        {
            needAddList = new ObservableCollection<EmployeeInfo>();
            needUpdateList = new ObservableCollection<EmployeeInfo>();
            //添加职务、部门及在职状态选择列表
            AlgorithmClass.InitEmployeeSelectionList(dataManager.AllEmployeeList, dataManager.AllPostList, dataManager.AllDepartmentList, AlgorithmClass.GetWorkStatusList());
            AllEmployeeList = AlgorithmClass.DeepClone<ObservableCollection<EmployeeInfo>>(dataManager.AllEmployeeList);
            lcv = new ListCollectionView(AllEmployeeList);
            lcv.Filter = Fitter_TextChanged;
            lcv.Refresh();

            SaveBtn.SetBinding(Button.IsEnabledProperty, new Binding("IsEmpty") { Source = lcv, Converter = new BoolConverter() });
            RestoreBtn.SetBinding(Button.IsEnabledProperty, new Binding("IsEmpty") { Source = lcv, Converter = new BoolConverter() });
            PrintBtn.SetBinding(Button.IsEnabledProperty, new Binding("IsEmpty") { Source = lcv, Converter = new BoolConverter() });

            EmployeeDataGrid.ItemsSource = lcv;
        }

        #region 搜索 过滤

        private bool Fitter_TextChanged(object obj)
        {
            if (obj == null) return true;
            EmployeeInfo employeeInfo = obj as EmployeeInfo;

            bool IsGeneralTextAccepted = true;
            if (!string.IsNullOrEmpty(FitterGeneralText.Text.Trim()))
            {
                IsGeneralTextAccepted = employeeInfo.EmployeeName.ToLower().Contains(FitterGeneralText.Text.Trim().ToLower());
            }

            bool IsDepartmentTextAccepted = true;
            if (DepartmentComboBox.SelectedItem != null)
            {
                DepartmentInfo info = DepartmentComboBox.SelectedItem as DepartmentInfo;
                IsDepartmentTextAccepted = employeeInfo.DepartmentID == info.ID;
            }

            return IsGeneralTextAccepted && IsDepartmentTextAccepted;
        }

        private void FitterGeneralText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lcv == null) return;
            lcv.Refresh();
        }

        private void DepartmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lcv == null) return;
            lcv.Refresh();
        }

        #endregion

        private void OperationTypeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OperationType type = (OperationType)(((DictionaryEntry)(OperationTypeList.SelectedValue)).Key);
            switch (type)
            {
                case OperationType.Scan:
                    if (CheckedIsNeedSave())
                    {
                        MessageBoxResult mr = MessageBox.Show("数据已被更改且尚未保存，是否保存？", "是否保存", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (mr == MessageBoxResult.Yes)
                        {
                            //执行保存
                            string msg = "";
                            if (DoSave(ref msg))
                            {
                                if (!string.IsNullOrEmpty(msg))
                                    MessageBox.Show(msg);
                            }
                            else
                            {
                                InitUIList();
                                if (!string.IsNullOrEmpty(msg))
                                    MessageBox.Show(msg);
                            }
                        }
                        else
                            InitUIList();
                    }
                    break;
                case OperationType.Edit:
                    FitterGeneralText.Text = "";
                    break;
            }
        }

        private bool CheckedIsNeedSave()
        {
            if (AllEmployeeList == null || lcv == null) return false;

            DepartmentInfo curDepartmentInfo = DepartmentComboBox.SelectedItem as DepartmentInfo;
            //检查添加项
            if (AllEmployeeList.Count != dataManager.AllEmployeeList.Count)
            {
                foreach (EmployeeInfo info in lcv)
                {
                    if (info.ID == 0)
                        needAddList.Add(info);
                }
                return true;
            }
            else
            {
                //检查并获取编辑项
                var tempList = new ObservableCollection<EmployeeInfo>();
                foreach (EmployeeInfo info in lcv)
                {
                    if (info.ID != 0)
                        tempList.Add(info);
                }
                needUpdateList = AlgorithmClass.CompareCollectionIsChanged<EmployeeInfo>(dataManager.AllEmployeeList, tempList, "ID");
                if (needUpdateList.Count != 0)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckEmployeeInfoIsValid(EmployeeInfo employeeInfo)
        {
            if (string.IsNullOrEmpty(employeeInfo.DepartmentName) || string.IsNullOrEmpty(employeeInfo.EmployeeFrom) || string.IsNullOrEmpty(employeeInfo.EmployeeName)
                || string.IsNullOrEmpty(employeeInfo.EmployeeSexStr) || string.IsNullOrEmpty(employeeInfo.EmployeeStatus) || string.IsNullOrEmpty(employeeInfo.PostName))
                return false;

            return true;
        }

        private bool DoSave(ref string msg)
        {
            bool isSaved = false;
            if (needAddList.Count != 0)
            {
                //执行插入
                foreach (EmployeeInfo info in needAddList)
                {
                    if (!CheckEmployeeInfoIsValid(info))
                    {
                        msg = "新增项或被编辑项的信息不完整，保存失败。";
                        return false;
                    }

                    int insertID = 0;
                    if (EmployeeDeal.InsertEmployeeInfo(info, ref insertID))
                    {
                        info.ID = insertID;
                    }
                    else
                    {
                        msg = "数据创建失败，请重试！";
                        return false;
                    }
                }

                needAddList.Clear();
                isSaved = true;
            }

            if (needUpdateList.Count != 0)
            {
                //执行更新
                foreach (EmployeeInfo info in needUpdateList)
                {
                    if (!CheckEmployeeInfoIsValid(info))
                    {
                        msg = "新增项或被编辑项的信息不完整，保存失败。";
                        return false;
                    }

                    if (!EmployeeDeal.UpdateEmployeeInfo(info))
                    {
                        msg = "数据更新失败，请重试！";
                        return false;
                    }
                }

                needUpdateList.Clear();
                isSaved = true;
            }

            if (isSaved)
                dataManager.AllEmployeeList = AlgorithmClass.DeepClone<ObservableCollection<EmployeeInfo>>(AllEmployeeList);

            if (isSaved)
                msg = "数据保存完成！";
            else
                msg = "不存在需要保存项！";

            return true;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (lcv.CanAddNewItem)
            {
                lcv.AddNewItem(new EmployeeInfo
                {
                    EmployeeSex = EmployeeSexType.boy,
                    StatusID = 1,
                    DepartmentID = (DepartmentComboBox.SelectedItem as DepartmentInfo).ID,
                    PostID = 1,
                    SelectionPostInfoList = dataManager.AllPostList.ToList(),
                    SelectionDepartmentInfoList = dataManager.AllDepartmentList.ToList(),
                    SelectionWorkStatusInfoList = AlgorithmClass.GetWorkStatusList()
                });

                lcv.CommitNew();
            }
        }

        private void DeleteNewBtn_Click(object sender, RoutedEventArgs e)
        {
            if (lcv.CurrentItem == null) return;

            EmployeeInfo employeeInfo = lcv.CurrentItem as EmployeeInfo;
            if (lcv.CanRemove)
            {
                lcv.Remove(employeeInfo);
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckedIsNeedSave())
            {
                MessageBox.Show("尚无需要保存的项。");
                return;
            }

            string msg = "";
            if (!DoSave(ref msg))
            {
                needAddList.Clear();
                needUpdateList.Clear();
            }
            if (!string.IsNullOrEmpty(msg))
                MessageBox.Show(msg); 
        }

        private void RestoreBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckedIsNeedSave())
                InitUIList();
            else
                MessageBox.Show("当前未做任何改动.");
        }

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
