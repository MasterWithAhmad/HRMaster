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
    public partial class UsersList : Form
    {
        string connectionString = "Data Source=.;Initial Catalog=HRManagementDB;Integrated Security=True;Encrypt=False";
        public UsersList()
        {
            InitializeComponent();
            LoadUsers();
            InitializeSortComboBox();
            InitializeDataGridView();
        }

        private void InitializeDataGridView()
        {
            //dgvUsers.AutoGenerateColumns = false;
            //dgvUsers.ColumnHeadersVisible = true;
            //dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //dgvUsers.AllowUserToAddRows = false;
            //dgvUsers.AllowUserToDeleteRows = false;
            //dgvUsers.MultiSelect = false;
            //dgvUsers.ReadOnly = true;
            //dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Adding Columns
            //dgvUsers.Columns.Add(CreateColumn("UserID", "UserID", false));
            //dgvUsers.Columns.Add(CreateColumn("Username", "Username"));
            //dgvUsers.Columns.Add(CreateColumn("Email", "Email"));
            //dgvUsers.Columns.Add(CreateColumn("RoleName", "Role"));

            // Apply styles
            dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsers.ColumnHeadersDefaultCellStyle.Font = new Font(dgvUsers.Font, FontStyle.Bold);
            dgvUsers.EnableHeadersVisualStyles = false;
            dgvUsers.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvUsers.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
        }

        private DataGridViewTextBoxColumn CreateColumn(string name, string headerText, bool isVisible = true)
        {
            return new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = headerText,
                DataPropertyName = name,
                Visible = isVisible
            };
        }


        private void LoadUsers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
                    SELECT u.UserID, u.Username, u.Email, r.RoleName
                    FROM [User] u
                    LEFT JOIN [Role] r ON u.RoleID = r.RoleID", conn);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvUsers.DataSource = dt;
            }
        }

        private void InitializeSortComboBox()
        {
            cmbSort.Items.Add("Username");
            cmbSort.Items.Add("Email");
            cmbSort.Items.Add("RoleName");
        }

        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            AddAndEditUser form = new AddAndEditUser();
            form.ShowDialog();
            LoadUsers();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            (dgvUsers.DataSource as DataTable).DefaultView.RowFilter = $"Username LIKE '%{txtSearch.Text}%' OR Email LIKE '%{txtSearch.Text}%'";
        }

        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sortColumn = cmbSort.SelectedItem.ToString();
            (dgvUsers.DataSource as DataTable).DefaultView.Sort = $"{sortColumn} ASC";
        }

        private void dgvUsers_DoubleClick(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow.Index != -1)
            {
                int userId = Convert.ToInt32(dgvUsers.CurrentRow.Cells["UserID"].Value);
                string username = dgvUsers.CurrentRow.Cells["Username"].Value.ToString();
                string email = dgvUsers.CurrentRow.Cells["Email"].Value.ToString();
                string roleName = dgvUsers.CurrentRow.Cells["RoleName"].Value.ToString();

                AddAndEditUser editUserForm = new AddAndEditUser
                {
                    IsEditMode = true
                };
                editUserForm.PopulateForm(userId, username, email, roleName);
                editUserForm.ShowDialog();
                LoadUsers(); // Refresh the list after editing a user
            }
        }

        private void btnGetBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
