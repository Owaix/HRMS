using Com.LT.LabExpress;
using Com.LT.LabExpress.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabExpressDesktop
{
    public partial class NewLabReport : Form
    {

        #region -- Global variables start --
        LabReportsList list;
        #endregion -- Global variable end --

        #region -- Form Events Start --
        public NewLabReport()
        {
            InitializeComponent();
        }
        private void lblAdd_Click(object sender, EventArgs e)
        {
            labReportsBindingSource.AddNew();
            pnlMain.Show();
            ddlDepartment.Focus();
        }
        private void lblEdit_Click(object sender, EventArgs e)
        {
            LabReports obj = (LabReports)labReportsBindingSource.Current;
            pnlMain.Show();
            ddlDepartment.Focus();
        }
        private void NewLabReport_Load(object sender, EventArgs e)
        {
            pnlMain.Hide();
            labDepartment();
            fillGrid();
        }

        public void fillGrid()
        {
            list = new LabReportsList(true);
            labReportsBindingSource.DataSource = list.OrderBy(x => x.labDeptName).ThenBy(x => x.reportName).ToList();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (reportNameTextBox.Text == "" || chargesTextBox.Text == "" || ddlDepartment.SelectedValue.ToInt32() == 0)
            { MessageBox.Show("Please Provide Department, Report Name and Charges", "", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            else
            {
                labReportsBindingSource.EndEdit();
                LabReports obj = (LabReports)labReportsBindingSource.Current;
                obj.labDeptID = ddlDepartment.SelectedValue.ToInt32();
                obj.catID = cmbxCat.SelectedValue.ToInt32();
                obj.labDeptName = ((LabDepartment)ddlDepartment.SelectedItem).labDeptName;
                obj.reportName = reportNameTextBox.Text.Trim();
                obj.charges = chargesTextBox.Text.ToDecimal();
                obj.ETA = eTATextBox.Text.ToInt32();
                obj.crtBy = lblUserName.Text;
                obj.crtDate = DateTime.Now;
                obj.modBy = lblUserName.Text;
                obj.modDate = DateTime.Now;
                /*if (ddlDepartment.Text == "Chemistry"){*/
                obj.orderNo = orderNoTextBox.Text.ToInt32();//}
                obj.isPrintOnNextPage = chkIsprintNext.Checked;
                obj.isHidden = chkIsHidden.Checked;
                obj.Save();
                pnlMain.Hide();
                fillGrid();
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            pnlMain.Hide();
            LabReports us = (LabReports)labReportsBindingSource.Current;
            if (us.reportID == 0)
            {
                labReportsBindingSource.RemoveCurrent();
            }
        }
        private void labReportsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            labReportsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        #endregion -- Form Events End --

        #region -- Helper Method Start --
        public LabDepartmentList labDepartment()
        {
            LabDepartmentList obj = new LabDepartmentList(new LabDepartment { }.Select("active=true"));
            LabDepartment dep = new LabDepartment();
            dep.labDeptName = "-- Select --";
            dep.labDeptID = 0;
            obj.Add(dep);
            ddlDepartment.DataSource = obj.OrderByDescending(x => x.labDeptName == "-- Select --").ThenBy(x => x.labDeptName).ToList();
            ddlDepartment.DisplayMember = "labDeptName";
            ddlDepartment.ValueMember = "labDeptID";


            CategoriesList objCategoriesList = new CategoriesList(new Categories { }.Select("active=true"));
            objCategoriesList.Insert(0, new Categories { catName = "-- Select --" });
            cmbxCat.ValueMember = "catID";
            cmbxCat.DisplayMember = "catName";
            cmbxCat.DataSource = objCategoriesList;

            return obj;
        }
        public void alluser(string username)
        {
            lblUserName.Text = username;
        }

        #endregion -- Helper Method End --

        private void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtSearch.Text.Trim().Length == 0) { labReportsDataGridView.DataSource = list; }
                else
                {
                    labReportsDataGridView.DataSource = list.FindAll(x => x.reportName.ToLower().Contains(txtSearch.Text.ToLower().Trim()));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
