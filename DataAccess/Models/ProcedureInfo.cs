using DataAccess.Helps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DataAccess.Models
{
    [Serializable]
    public class ProcedureInfo : BaseInfo
    {
        public ProcedureInfo()
        {
            completeDate = DateTime.Now;
        }

        private DateTime completeDate;
        public DateTime CompleteDate
        {
            get { return completeDate; }
            set
            {
                completeDate = value;
                NotifyPropertyChange("CompleteDate");
            }
        }

        private string procedureName;
        public string ProcedureName
        {
            get { return procedureName; }
            set
            {
                procedureName = value;
                UpdatePriceByProcedureName();
                NotifyPropertyChange("ProcedureName");
            }
        }

        private int employeeID;
        public int EmployeeID
        {
            get { return employeeID; }
            set
            {
                employeeID = value;
                UpdateEmployeeNameByID();
                NotifyPropertyChange("EmployeeID");
            }
        }

        private string employeeName;
        public string EmployeeName
        {
            get { return employeeName; }
            set { employeeName = value; NotifyPropertyChange("EmployeeName"); }
        }

        private double price;
        public double Price
        {
            get { return price; }
            set
            {
                price = value;
                NotifyPropertyChange("Price");
            }
        }

        private string remark;
        public string Remark
        {
            get { return remark; }
            set
            {
                remark = value;
                NotifyPropertyChange("Remark");
            }
        }

        private string procedureMap;
        public string ProcedureMap
        {
            get { return procedureMap; }
            set
            {
                procedureMap = value;
                UpdateOtherInfo();
                NotifyPropertyChange("ProcedureMap");
            }
        }

        public string GetProcedureMapInfo()
        {
            return "{" + CompleteDate.ToString("yyyy-MM-dd") + "," + ProcedureName + "," + EmployeeID + "," + Price + "," + Remark + "}";
        }

        private void UpdateOtherInfo()
        {
            string[] param = ProcedureMap.Trim("{}".ToCharArray()).Split(',');
            CompleteDate = Convert.ToDateTime(param[0]);
            procedureName = param[1];
            EmployeeID = int.Parse(param[2]); //由于当EmployeeID变化时，需要更新EmployeeName，故此处赋值EmployeeID 
            Price = double.Parse(param[3]);
            Remark = param[4];
        }

        private void UpdateEmployeeNameByID()
        {
            EmployeeInfo employeeInfo = SelectionEmployeeInfoList.Where(info => info.ID == this.EmployeeID).FirstOrDefault();
            if (employeeInfo != null)
            {
                EmployeeName = employeeInfo.EmployeeName;
            }
        }

        private void UpdatePriceByProcedureName()
        {
            SelectionProcedureInfo selectionProcedureInfoInfo = SelectionProcedureInfoList.Where(info => info.Procedure == this.ProcedureName).FirstOrDefault();
            if (selectionProcedureInfoInfo != null)
            {
                Price = selectionProcedureInfoInfo.Price;
            }
        }

        private List<EmployeeInfo> selectionEmployeeInfoList;
        public List<EmployeeInfo> SelectionEmployeeInfoList
        {
            get { return selectionEmployeeInfoList; }
            set
            {
                selectionEmployeeInfoList = value;
                NotifyPropertyChange("SelectionEmployeeInfoList");
            }
        }

        private List<SelectionProcedureInfo> selectionProcedureInfoList;
        public List<SelectionProcedureInfo> SelectionProcedureInfoList
        {
            get { return selectionProcedureInfoList; }
            set
            {
                selectionProcedureInfoList = value;
                NotifyPropertyChange("SelectionProcedureInfoList");
            }
        }

    }
}
