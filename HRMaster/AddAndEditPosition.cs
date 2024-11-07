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
    public partial class AddAndEditPosition : Form
    {
        string connectionString = "Data Source=.;Initial Catalog=HRManagementDB;Integrated Security=True;Encrypt=False";
        private int positionID = -1;
        public AddAndEditPosition()
        {
            InitializeComponent();
        }

        public void SetPositionDetails(DataGridViewRow row)
        {
            positionID = Convert.ToInt32(row.Cells["PositionID"].Value);
            txtPositionID.Text = row.Cells["PositionID"].Value.ToString();
            txtPositionName.Text = row.Cells["PositionName"].Value.ToString();
            txtDescription.Text = row.Cells["Description"].Value.ToString();
        }


        private void btnGetBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPositionName.Text))
            {
                MessageBox.Show("Position Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();

                if (positionID == -1) // Add new department
                {
                    cmd.CommandText = "INSERT INTO Position (PositionName, Description) VALUES (@PositionName, @Description)";
                }
                else // Update existing department
                {
                    cmd.CommandText = "UPDATE Position SET PositionName = @PositionName, Description = @Description WHERE PositionID = @PositionID";
                    cmd.Parameters.AddWithValue("@PositionID", positionID);
                }

                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@PositionName", txtPositionName.Text);
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Position saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (positionID == -1)
            {
                MessageBox.Show("No position selected to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this position?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Position WHERE PositionID = @PositionID", con);
                    cmd.Parameters.AddWithValue("@PositionID", positionID);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Position deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
