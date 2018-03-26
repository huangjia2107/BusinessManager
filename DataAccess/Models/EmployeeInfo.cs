using DataAccess.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Models
{
    [Serializable]
    public class EmployeeInfo : BaseInfo
    {
        public EmployeeInfo()
        {
            EmployeeBirthday = DateTime.Now;
            EntryTime = DateTime.Now;
        }

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; NotifyPropertyChange("ID"); }
        }

        private string employeeName;
        public string EmployeeName
        {
            get { return employeeName; }
            set { employeeName = value; NotifyPropertyChange("EmployeeName"); }
        }

        private EmployeeSexType employeeSex;
        public EmployeeSexType EmployeeSex
        {
            get { return employeeSex; }
            set
            {
                employeeSex = value;

                EmployeeSexStr = employeeSex == EmployeeSexType.boy ? "男" : "女";
                NotifyPropertyChange("EmployeeSex");
            }
        }

        private string employeeSexStr;
        public string EmployeeSexStr
        {
            get { return employeeSexStr; }
            set { employeeSexStr = value; NotifyPropertyChange("EmployeeSexStr"); }
        }

        private string employeeFrom;
        public string EmployeeFrom
        {
            get { return employeeFrom; }
            set { employeeFrom = value; NotifyPropertyChange("EmployeeFrom"); }
        }

        private DateTime employeeBirthday;
        public DateTime EmployeeBirthday
        {
            get { return employeeBirthday; }
            set { employeeBirthday = value; NotifyPropertyChange("EmployeeBirthday"); }
        }

        private DateTime entryTime;
        public DateTime EntryTime
        {
            get { return entryTime; }
            set { entryTime = value; NotifyPropertyChange("EntryTime"); }
        }

        private string remark;
        public string Remark
        {
            get { return remark; }
            set { remark = value; NotifyPropertyChange("Remark"); }
        }

        private int departmentID;
        public int DepartmentID
        {
            get { return departmentID; }
            set
            {
                departmentID = value;
                UpdateDepartmentNameByID();
                NotifyPropertyChange("DepartmentID");
            }
        }

        private string departmentName;
        public string DepartmentName
        {
            get { return departmentName; }
            set { departmentName = value; NotifyPropertyChange("DepartmentName"); }
        }

        private int postID;
        public int PostID
        {
            get { return postID; }
            set
            {
                postID = value;
                UpdatePostNameByID();
                NotifyPropertyChange("PostID");
            }
        }

        private string postName;
        public string PostName
        {
            get { return postName; }
            set { postName = value; NotifyPropertyChange("PostName"); }
        }

        private int basicSalary;
        public int BasicSalary
        {
            get { return basicSalary; }
            set
            {
                basicSalary = value;
                NotifyPropertyChange("BasicSalary");
            }
        }

        private string employeeStatus;
        public string EmployeeStatus
        {
            get { return employeeStatus; }
            set { employeeStatus = value; NotifyPropertyChange("EmployeeStatus"); }
        }

        private int statusID;
        public int StatusID
        {
            get { return statusID; }
            set
            {
                statusID = value;
                UpdateStatusNameByID();
                NotifyPropertyChange("StatusID");
            }
        }

        private void UpdateStatusNameByID()
        {
            if (SelectionWorkStatusInfoList == null)
                return;

            WorkStatusInfo workStatusInfo = SelectionWorkStatusInfoList.Where(info => info.ID == this.StatusID).FirstOrDefault();
            if (workStatusInfo != null)
            {
                EmployeeStatus = workStatusInfo.StatusName;
            }
        }

        private List<WorkStatusInfo> selectionWorkStatusInfoList;
        public List<WorkStatusInfo> SelectionWorkStatusInfoList
        {
            get { return selectionWorkStatusInfoList; }
            set
            {
                selectionWorkStatusInfoList = value;
                UpdateStatusNameByID();
                NotifyPropertyChange("SelectionWorkStatusInfoList");
            }
        }



        private void UpdateDepartmentNameByID()
        {
            if (this.DepartmentID == 0 || SelectionDepartmentInfoList == null)
                return;

            DepartmentInfo departmentInfo = SelectionDepartmentInfoList.Where(info => info.ID == this.DepartmentID).FirstOrDefault();
            if (departmentInfo != null)
            {
                DepartmentName = departmentInfo.DepartmentName;
            }
        }

        private List<DepartmentInfo> selectionDepartmentInfoList;
        public List<DepartmentInfo> SelectionDepartmentInfoList
        {
            get { return selectionDepartmentInfoList; }
            set
            {
                selectionDepartmentInfoList = value;
                UpdateDepartmentNameByID();
                NotifyPropertyChange("SelectionDepartmentInfoList");
            }
        }

        private void UpdatePostNameByID()
        {
            if (this.PostID == 0 || selectionPostInfoList == null)
                return;

            PostInfo postInfo = selectionPostInfoList.Where(info => info.ID == this.PostID).FirstOrDefault();
            if (postInfo != null)
                PostName = postInfo.PostName;
        }

        private List<PostInfo> selectionPostInfoList;
        public List<PostInfo> SelectionPostInfoList
        {
            get { return selectionPostInfoList; }
            set
            {
                selectionPostInfoList = value;
                UpdatePostNameByID();
                NotifyPropertyChange("SelectionPostInfoList");
            }
        }

    }
}
