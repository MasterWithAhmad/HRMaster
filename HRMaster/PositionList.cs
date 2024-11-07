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
    public partial class PositionList : Form
    {
        string connectionString = "Data Source=.;Initial Catalog=HRManagementDB;Integrated Security=True;Encrypt=False";
        public int SelectedPositionID { get; set; }
        public string SelectedPositionName { get; set; }
        public PositionList()
        {
            InitializeComponent();
            LoadPositions();
            InitializeSortComboBox();
        }

        private void LoadPositions()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Position", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvPositions.DataSource = dt;
            }
        }

        private void InitializeSortComboBox()
        {
            cmbSort.Items.Add("PositionName");
            cmbSort.Items.Add("Description");
        }


        private void btnGetBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCreatePosition_Click(object sender, EventArgs e)
        {
            AddAndEditPosition addPosition = new AddAndEditPosition();
            addPosition.FormClosed += (s, args) => LoadPositions();
            this.Close();
            addPosition.Show();
        }

        private void dgvPositions_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPositions.Rows[e.RowIndex];
                SelectedPositionID = Convert.ToInt32(row.Cells["PositionID"].Value);
                SelectedPositionName = row.Cells["PositionName"].Value.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            (dgvPositions.DataSource as DataTable).DefaultView.RowFilter = string.Format("PositionName LIKE '%{0}%' OR Description LIKE '%{0}%'", txtSearch.Text);
        }

        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            (dgvPositions.DataSource as DataTable).DefaultView.Sort = cmbSort.SelectedItem.ToString();
        }

        private void PositionList_Load(object sender, EventArgs e)
        {
            LoadPositions();
        }
    }
}
