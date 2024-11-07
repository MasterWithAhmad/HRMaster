using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRMaster
{
    public partial class EmployeeList : Form
    {
        string connectionString = "Data Source=.;Initial Catalog=HRManagementDB;Integrated Security=True;Encrypt=False";
        public EmployeeList()
        {
            InitializeComponent();
        }

        private void EmployeeList_Load(object sender, EventArgs e)
        {
            LoadEmployees();
            LoadEmployees();
            cmbSort.Items.AddRange(new string[] { "FirstName", "LastName", "Email", "DepartmentID", "PositionID" });
            dgvEmployees.Columns["DepartmentID"].Visible = false;
            dgvEmployees.Columns["PositionID"].Visible = false;
            dgvEmployees.Columns["EmployeeID"].Visible = false;
            dgvEmployees.Columns["Notes"].Visible = false;
        }

        private void LoadEmployees()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT 
                        e.EmployeeID, 
                        e.FirstName, 
                        e.LastName, 
                        e.Email, 
                        e.Phone, 
                        e.Address, 
                        e.DateOfBirth, 
                        e.Gender, 
                        e.HireDate, 
                        e.DepartmentID, 
                        d.DepartmentName, 
                        e.PositionID, 
                        p.PositionName, 
                        e.Salary, 
                        e.Status, 
                        e.Photo, 
                        e.Notes
                    FROM Employee e
                    JOIN Department d ON e.DepartmentID = d.DepartmentID
                    JOIN Position p ON e.PositionID = p.PositionID", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Add an Image column to display the photo
                    DataColumn imageColumn = new DataColumn("EmployeeImage", typeof(Image));
                    dt.Columns.Add(imageColumn);

                    // Convert photo bytes to image and display in DataGridView
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["Photo"] != DBNull.Value)
                        {
                            byte[] photoArray = (byte[])row["Photo"];
                            using (MemoryStream ms = new MemoryStream(photoArray))
                            {
                                row["EmployeeImage"] = Image.FromStream(ms);
                            }
                        }
                    }

                    dgvEmployees.DataSource = dt;

                    //Set the format for the salary column
                    dgvEmployees.Columns["Salary"].DefaultCellStyle.Format = "C";

                    // Hide the original Photo column
                    dgvEmployees.Columns["Photo"].Visible = false;

                    // Set the image column's layout
                    DataGridViewImageColumn dgvImageColumn = (DataGridViewImageColumn)dgvEmployees.Columns["EmployeeImage"];
                    dgvImageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
                }
            }
        }
                   

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            (dgvEmployees.DataSource as DataTable).DefaultView.RowFilter = string.Format("FirstName LIKE '%{0}%' OR LastName LIKE '%{0}%'", txtSearch.Text);
        }

        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            (dgvEmployees.DataSource as DataTable).DefaultView.Sort = cmbSort.SelectedItem.ToString();
        }

        private void btnGetBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            AddAndEditEmployee addEditEmployeeForm = new AddAndEditEmployee();
            if (addEditEmployeeForm.ShowDialog() == DialogResult.OK)
            {
                LoadEmployees();
            }
        }

        private void dgvEmployees_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex >= 0)
            //{
            //    DataGridViewRow row = dgvEmployees.Rows[e.RowIndex];
            //    AddAndEditEmployee editEmployeeForm = new AddAndEditEmployee();

            //    // Populate the form with selected row data
            //    editEmployeeForm.txtEmployeeID.Text = row.Cells["EmployeeID"].Value.ToString();
            //    editEmployeeForm.txtFirstName.Text = row.Cells["FirstName"].Value.ToString();
            //    editEmployeeForm.txtLastName.Text = row.Cells["LastName"].Value.ToString();
            //    editEmployeeForm.txtEmail.Text = row.Cells["Email"].Value.ToString();
            //    editEmployeeForm.txtPhone.Text = row.Cells["Phone"].Value.ToString();
            //    editEmployeeForm.txtAddress.Text = row.Cells["Address"].Value.ToString();
            //    editEmployeeForm.dtpDateOfBirth.Value = Convert.ToDateTime(row.Cells["DateOfBirth"].Value);
            //    editEmployeeForm.dtpHireDate.Value = Convert.ToDateTime(row.Cells["HireDate"].Value);
            //    editEmployeeForm.cmbGender.SelectedItem = row.Cells["Gender"].Value.ToString();
            //    editEmployeeForm.txtDepartmentID.Text = row.Cells["DepartmentID"].Value.ToString();
            //    editEmployeeForm.txtDepartmentName.Text = row.Cells["DepartmentName"].Value.ToString();
            //    editEmployeeForm.txtPositionID.Text = row.Cells["PositionID"].Value.ToString();
            //    editEmployeeForm.txtPositionName.Text = row.Cells["PositionName"].Value.ToString();
            //    editEmployeeForm.txtSalary.Text = row.Cells["Salary"].Value.ToString();
            //    editEmployeeForm.cmbStatus.SelectedItem = row.Cells["Status"].Value.ToString();
            //    editEmployeeForm.txtNotes.Text = row.Cells["Notes"].Value.ToString();               

            //    if (row.Cells["Photo"].Value != DBNull.Value)
            //    {
            //        byte[] photoArray = (byte[])row.Cells["Photo"].Value;
            //        using (MemoryStream ms = new MemoryStream(photoArray))
            //        {
            //            editEmployeeForm.employeePictureBox.Image = Image.FromStream(ms);
            //        }
            //    }

            //    editEmployeeForm.ShowDialog();
            //    LoadEmployees(); // Reload the data after editing
            //}

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvEmployees.Rows[e.RowIndex];

                AddAndEditEmployee editEmployeeForm = new AddAndEditEmployee();

                editEmployeeForm.txtEmployeeID.Text = row.Cells["EmployeeID"].Value.ToString();
                editEmployeeForm.txtFirstName.Text = row.Cells["FirstName"].Value.ToString();
                editEmployeeForm.txtLastName.Text = row.Cells["LastName"].Value.ToString();
                editEmployeeForm.txtEmail.Text = row.Cells["Email"].Value.ToString();
                editEmployeeForm.txtPhone.Text = row.Cells["Phone"].Value.ToString();
                editEmployeeForm.txtAddress.Text = row.Cells["Address"].Value.ToString();
                editEmployeeForm.dtpDateOfBirth.Value = Convert.ToDateTime(row.Cells["DateOfBirth"].Value);
                editEmployeeForm.cmbGender.SelectedItem = row.Cells["Gender"].Value.ToString();
                editEmployeeForm.dtpHireDate.Value = Convert.ToDateTime(row.Cells["HireDate"].Value);
                editEmployeeForm.txtDepartmentID.Text = row.Cells["DepartmentID"].Value.ToString();
                editEmployeeForm.txtPositionID.Text = row.Cells["PositionID"].Value.ToString();
                editEmployeeForm.txtSalary.Text = row.Cells["Salary"].Value.ToString();
                editEmployeeForm.cmbStatus.SelectedItem = row.Cells["Status"].Value.ToString();
                editEmployeeForm.txtNotes.Text = row.Cells["Notes"].Value.ToString();

                // Load the photo into the PictureBox and set the photo path
                if (row.Cells["Photo"].Value != DBNull.Value)
                {
                    byte[] photoData = (byte[])row.Cells["Photo"].Value;
                    using (MemoryStream ms = new MemoryStream(photoData))
                    {
                        editEmployeeForm.picEmployee.Image = Image.FromStream(ms);
                    }

                    // Optionally, store the path if available or set a placeholder
                    editEmployeeForm.txtPhotoPath.Text = "Image loaded"; // Or store the actual path if available

                    editEmployeeForm.ShowDialog();
                    LoadEmployees(); // Reload the data after editing
                }
                else
                {
                    editEmployeeForm.picEmployee.Image = null;
                    editEmployeeForm.txtPhotoPath.Text = string.Empty;
                }
            }
        }
    }
}
