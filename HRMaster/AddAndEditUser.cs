// Ignore Spelling: username

using Org.BouncyCastle.Crypto.Generators;
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
    public partial class AddAndEditUser : Form
    {
        string connectionString = "Data Source=.;Initial Catalog=HRManagementDB;Integrated Security=True;Encrypt=False";
        public bool IsEditMode { get; set; }
        public int EditingUserId { get; set; }
        public AddAndEditUser()
        {
            InitializeComponent();
            InitializeRoleComboBox();
        }

        private void InitializeRoleComboBox()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT RoleID, RoleName FROM [Role]", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                cmbRole.DataSource = dt;
                cmbRole.DisplayMember = "RoleName";
                cmbRole.ValueMember = "RoleID";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd;

                if (IsEditMode)
                {
                    // Update existing user
                    cmd = new SqlCommand("UPDATE [User] SET Username=@Username, PasswordHash=@PasswordHash, Email=@Email, RoleID=@RoleID WHERE UserID=@UserID", conn);
                    cmd.Parameters.AddWithValue("@UserID", EditingUserId);
                }
                else
                {
                    // Insert new user
                    cmd = new SqlCommand("INSERT INTO [User] (Username, PasswordHash, Email, RoleID) VALUES (@Username, @PasswordHash, @Email, @RoleID)", conn);
                }

                cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@PasswordHash", BCrypt.Net.BCrypt.HashPassword(txtPassword.Text)); // Example using BCrypt
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@RoleID", cmbRole.SelectedValue);

                cmd.ExecuteNonQuery();
                MessageBox.Show(IsEditMode ? "User updated successfully!" : "User added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearForm();
                this.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (IsEditMode)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirmation", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("DELETE FROM [User] WHERE UserID=@UserID", conn);
                        cmd.Parameters.AddWithValue("@UserID", EditingUserId);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("User deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearForm();
                        this.Close();
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUsername.Text) || !string.IsNullOrEmpty(txtPassword.Text) || !string.IsNullOrEmpty(txtEmail.Text))
            {
                DialogResult result = MessageBox.Show("Are you sure you want to cancel? All unsaved changes will be lost.", "Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    ClearForm();
                    this.Close();
                }
            }
        }

        private void ClearForm()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtEmail.Clear();
            cmbRole.SelectedIndex = -1;
        }

        private void btnGetBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void PopulateForm(int userId, string username, string email, string role)
        {
            EditingUserId = userId;
            txtUsername.Text = username;
            txtEmail.Text = email;
            cmbRole.SelectedItem = role;
        }
    }
}
