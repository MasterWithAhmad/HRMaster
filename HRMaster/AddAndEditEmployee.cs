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
    public partial class AddAndEditEmployee : Form
    {
        string connectionString = "Data Source=.;Initial Catalog=HRManagementDB;Integrated Security=True;Encrypt=False";
        string photoPath = "";
        byte[] employeePhoto = null;
        public AddAndEditEmployee()
        {
            InitializeComponent();
            this.cmbStatus.SelectedIndex = 0;
            this.cmbGender.SelectedIndex = 0;
        }


        private void btnGetBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUploadPhoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the path of specified file
                    photoPath = openFileDialog.FileName;
                    txtPhotoPath.Text = photoPath;

                    // Load the image into the picture box
                    picEmployee.Image = Image.FromFile(photoPath);
                    // Convert the image file to a byte array
                    employeePhoto = File.ReadAllBytes(photoPath);
                }

            }
        }

        private void btnGetDepartment_Click(object sender, EventArgs e)
        {
            DepartmentList departmentList = new DepartmentList();
            if (departmentList.ShowDialog() == DialogResult.OK)
            {
                txtDepartmentID.Text = departmentList.SelectedDepartmentID.ToString();
                txtDepartmentName.Text = departmentList.SelectedDepartmentName;
            }
        }

        private void btnGetPosition_Click(object sender, EventArgs e)
        {
            PositionList positionList = new PositionList();
            if (positionList.ShowDialog() == DialogResult.OK)
            {
                txtPositionID.Text = positionList.SelectedPositionID.ToString();
                txtPositionName.Text = positionList.SelectedPositionName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //using (SqlConnection con = new SqlConnection(connectionString))
            //{
            //    con.Open();
            //    SqlCommand cmd = new SqlCommand();
            //    cmd.Connection = con;

            //    if (string.IsNullOrEmpty(txtEmployeeID.Text)) // Insert new employee
            //    {
            //        cmd.CommandText = "INSERT INTO Employee (FirstName, LastName, Email, Phone, Address, DateOfBirth, Gender, HireDate, DepartmentID, PositionID, Salary, Status, Photo, Notes) VALUES (@FirstName, @LastName, @Email, @Phone, @Address, @DateOfBirth, @Gender, @HireDate, @DepartmentID, @PositionID, @Salary, @Status, @Photo, @Notes)";
            //    }
            //    else // Update existing employee
            //    {
            //        cmd.CommandText = "UPDATE Employee SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Phone = @Phone, Address = @Address, DateOfBirth = @DateOfBirth, Gender = @Gender, HireDate = @HireDate, DepartmentID = @DepartmentID, PositionID = @PositionID, Salary = @Salary, Status = @Status, Photo = @Photo, Notes = @Notes WHERE EmployeeID = @EmployeeID";
            //        cmd.Parameters.AddWithValue("@EmployeeID", int.Parse(txtEmployeeID.Text));
            //    }

            //    cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
            //    cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
            //    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
            //    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
            //    cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
            //    cmd.Parameters.AddWithValue("@DateOfBirth", dtpDateOfBirth.Value);
            //    cmd.Parameters.AddWithValue("@Gender", cmbGender.SelectedItem.ToString());
            //    cmd.Parameters.AddWithValue("@HireDate", dtpHireDate.Value);
            //    cmd.Parameters.AddWithValue("@DepartmentID", int.Parse(txtDepartmentID.Text));
            //    cmd.Parameters.AddWithValue("@PositionID", int.Parse(txtPositionID.Text));
            //    cmd.Parameters.AddWithValue("@Salary", decimal.Parse(txtSalary.Text));
            //    cmd.Parameters.AddWithValue("@Status", cmbStatus.SelectedItem.ToString());
            //    cmd.Parameters.AddWithValue("@Notes", txtNotes.Text);

            //    if (!string.IsNullOrEmpty(txtPhotoPath.Text))
            //    {
            //        // Read the image file into a byte array
            //        byte[] photo = File.ReadAllBytes(txtPhotoPath.Text);
            //        cmd.Parameters.AddWithValue("@Photo", photo);
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@Photo", DBNull.Value);
            //    }

            //    cmd.ExecuteNonQuery();
            //    MessageBox.Show("Employee saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    this.DialogResult = DialogResult.OK;
            //    this.Close();
            //}

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                if (string.IsNullOrEmpty(txtEmployeeID.Text)) // Insert new employee
                {
                    cmd.CommandText = "INSERT INTO Employee (FirstName, LastName, Email, Phone, Address, DateOfBirth, Gender, HireDate, DepartmentID, PositionID, Salary, Status, Photo, Notes) VALUES (@FirstName, @LastName, @Email, @Phone, @Address, @DateOfBirth, @Gender, @HireDate, @DepartmentID, @PositionID, @Salary, @Status, @Photo, @Notes)";
                }
                else // Update existing employee
                {
                    cmd.CommandText = "UPDATE Employee SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Phone = @Phone, Address = @Address, DateOfBirth = @DateOfBirth, Gender = @Gender, HireDate = @HireDate, DepartmentID = @DepartmentID, PositionID = @PositionID, Salary = @Salary, Status = @Status, Photo = @Photo, Notes = @Notes WHERE EmployeeID = @EmployeeID";
                    cmd.Parameters.AddWithValue("@EmployeeID", int.Parse(txtEmployeeID.Text));
                }

                cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@DateOfBirth", dtpDateOfBirth.Value);
                cmd.Parameters.AddWithValue("@Gender", cmbGender.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@HireDate", dtpHireDate.Value);
                cmd.Parameters.AddWithValue("@DepartmentID", int.Parse(txtDepartmentID.Text));
                cmd.Parameters.AddWithValue("@PositionID", int.Parse(txtPositionID.Text));
                cmd.Parameters.AddWithValue("@Salary", decimal.Parse(txtSalary.Text));
                cmd.Parameters.AddWithValue("@Status", cmbStatus.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@Notes", txtNotes.Text);

                // Handle the photo
                if (!string.IsNullOrEmpty(txtPhotoPath.Text) && File.Exists(txtPhotoPath.Text))
                {
                    // Read the image file into a byte array
                    byte[] photo = File.ReadAllBytes(txtPhotoPath.Text);
                    cmd.Parameters.Add("@Photo", SqlDbType.VarBinary).Value = photo;
                }
                else if (picEmployee.Image != null)
                {
                    // If the photo path is empty but the picture box has an image (when editing an employee)
                    MemoryStream ms = new MemoryStream();
                    picEmployee.Image.Save(ms, picEmployee.Image.RawFormat);
                    cmd.Parameters.Add("@Photo", SqlDbType.VarBinary).Value = ms.ToArray();
                }
                else
                {
                    cmd.Parameters.Add("@Photo", SqlDbType.VarBinary).Value = DBNull.Value;
                }

                cmd.ExecuteNonQuery();
                MessageBox.Show("Employee saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this employee?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Employee WHERE EmployeeID = @EmployeeID", con);
                    cmd.Parameters.AddWithValue("@EmployeeID", int.Parse(txtEmployeeID.Text));
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Employee deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
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
