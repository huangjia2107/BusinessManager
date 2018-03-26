using BusinessManager.Helps;
using DataAccess.Helps;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BusinessManager.DataAccess
{
    public class DataManager
    {
        public static readonly DataManager dataManager = new DataManager();
        private DataManager() { }

        public string CurUserName { get; set; }

        public ObservableCollection<SheetInfo> AllSheetList = null;
        public ObservableCollection<BsTypeInfo> AllBsTypeList = null;
        public ObservableCollection<EmployeeInfo> AllEmployeeList = null;
        public ObservableCollection<DepartmentInfo> AllDepartmentList = null;
        public ObservableCollection<PostInfo> AllPostList = null;
        public ObservableCollection<SalaryInfo> AllSalaryList = null;
        public ObservableCollection<BOPInfo> AllBOPList = null;

        public void BeginLoadData()
        {
            //得到所有的接单数据
            LoadSheetInfo();
            //得到所有业务类型数据
            LoadBsTypeInfo();
            //得到所有雇员信息
            LoadEmployeeInfo();
            //得到所有部门信息
            LoadDepartmentInfo();
            //得到所有职务信息
            LoadPostInfo();
            //得到所有工资信息
            LoadSalaryInfo();
            //得到所有公司收支信息
            LoadBOPInfo();
        }

        public void LoadSheetInfo()
        {
            //得到所有的接单数据
            AllSheetList = SheetDeal.GetAllSheet();
        }

        public void LoadBsTypeInfo()
        {
            //得到所有业务类型数据
            AllBsTypeList = BsTypeDeal.GetAllBsType();
        }

        public void LoadEmployeeInfo()
        {
            //得到所有雇员信息
            AllEmployeeList = EmployeeDeal.GetAllEmployee();
        }

        public void LoadDepartmentInfo()
        {
            //得到所有部门信息
            AllDepartmentList = DepartmentDeal.GetAllDepartment();
        }

        public void LoadPostInfo()
        {
            //得到所有职务信息
            AllPostList = PostDeal.GetAllPost();
        }

        public void LoadSalaryInfo()
        {
            //得到所有工资信息
            AllSalaryList = SalaryDeal.GetAllSalary();
        }

        public void LoadBOPInfo()
        {
            //得到所有公司收支信息
            AllBOPList = BOPDeal.GetAllBOP();
        }
    }
}
