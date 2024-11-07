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
    public partial class AddAndEditDepartment : Form
    {
        string connectionString = "Data Source=.;Initial Catalog=HRManagementDB;Integrated Security=True;Encrypt=False";
        private int departmentID = -1;
        public AddAndEditDepartment()
        {

            InitializeComponent();
        }

        private void btnGetBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void SetDepartmentDetails(DataGridViewRow row)
        {
            departmentID = Convert.ToInt32(row.Cells["DepartmentID"].Value);
            txtDepartmentID.Text = row.Cells["DepartmentID"].Value.ToString();
            txtDepartmentName.Text = row.Cells["DepartmentName"].Value.ToString();
            txtDescription.Text = row.Cells["Description"].Value.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDepartmentName.Text))
            {
                MessageBox.Show("Department Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();

                if (departmentID == -1) // Add new department
                {
                    cmd.CommandText = "INSERT INTO Department (DepartmentName, Description) VALUES (@DepartmentName, @Description)";
                }
                else // Update existing department
                {
                    cmd.CommandText = "UPDATE Department SET DepartmentName = @DepartmentName, Description = @Description WHERE DepartmentID = @DepartmentID";
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
                }

                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@DepartmentName", txtDepartmentName.Text);
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Department saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (departmentID == -1)
            {
                MessageBox.Show("No department selected to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this department?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Department WHERE DepartmentID = @DepartmentID", con);
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentID);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Department deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to cancel?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
