using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRMaster
{
    public partial class DepartmentList : Form
    {
        string connectionString = "Data Source=.;Initial Catalog=HRManagementDB;Integrated Security=True;Encrypt=False";
        public int SelectedDepartmentID { get; set; }
        public string SelectedDepartmentName { get; set; }

        public DepartmentList()
        {
            InitializeComponent();
            LoadDepartments();
            InitializeSortComboBox();
        }

        private void LoadDepartments()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Department", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvDepartments.DataSource = dt;
            }
        }

        private void InitializeSortComboBox()
        {
            cmbSort.Items.Add("DepartmentName");
            cmbSort.Items.Add("Description");
        }


        private void btnGetBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddDepartment_Click(object sender, EventArgs e)
        {
            AddAndEditDepartment addDepartment = new AddAndEditDepartment();
            addDepartment.FormClosed += (s, args) => LoadDepartments();
            addDepartment.Show();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            (dgvDepartments.DataSource as DataTable).DefaultView.RowFilter = string.Format("DepartmentName LIKE '%{0}%' OR Description LIKE '%{0}%'", txtSearch.Text);
        }

        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            (dgvDepartments.DataSource as DataTable).DefaultView.Sort = cmbSort.SelectedItem.ToString();
        }

        private void dgvDepartments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDepartments.Rows[e.RowIndex];
                SelectedDepartmentID = Convert.ToInt32(row.Cells["DepartmentID"].Value);
                SelectedDepartmentName = row.Cells["DepartmentName"].Value.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void DepartmentList_Load(object sender, EventArgs e)
        {
            LoadDepartments();
        }
    }
}
