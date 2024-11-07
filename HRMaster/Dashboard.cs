using LiveCharts;
using LiveCharts.Wpf;
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
    public partial class Dashboard : Form
    {
        string connectionString = "Data Source=.;Initial Catalog=HRManagementDB;Integrated Security=True;Encrypt=False";
        private string fullName;
        private int userId;
        private const decimal SalaryIncreaseAmount = 500; // Define the new salary amount for employees with zero salary
        public Dashboard()
        {
            InitializeComponent();
            LoadDashboardData();

        }

        private void LoadDashboardData()
        {
            LoadEmployeeData();
            LoadDepartmentData();
            LoadLeaveRequestsData();
            IncreaseZeroSalaries();
        }

        private void IncreaseZeroSalaries()
        {
            string query = @"
                UPDATE Employee
                SET Salary = @NewSalary
                WHERE Salary = 0;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NewSalary", SalaryIncreaseAmount);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void LoadEmployeeData()
        {
            string query = @"
                SELECT 
                    COUNT(*) AS TotalEmployees,
                    MAX(Salary) AS MaxSalary,
                    MIN(Salary) AS MinSalary,
                    SUM(Salary) AS TotalSalary
                   
                FROM Employee;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int totalEmployees = reader.GetInt32(0);
                    decimal maxSalary = reader.GetDecimal(1);
                    decimal minSalary = reader.GetDecimal(2);
                    decimal totalSalary = reader.GetDecimal(3);

                    // Update PieChart for employee data
                    pieChart1.Series = new SeriesCollection
                    {
                        new PieSeries
                        {
                            Title = "Total Employees",
                            Values = new ChartValues<int> { totalEmployees },
                            DataLabels = true,
                            Fill = System.Windows.Media.Brushes.Orange // Total Employees color
                        },
                        new PieSeries
                        {
                            Title = "Total Salary",
                            Values = new ChartValues<decimal> { totalSalary },
                            DataLabels = true,
                            Fill = System.Windows.Media.Brushes.Blue // Total Salary color
                        },
                        new PieSeries
                        {
                            Title = "Max Salary",
                            Values = new ChartValues<decimal> { maxSalary },
                            DataLabels = true,
                            Fill = System.Windows.Media.Brushes.Green // Max Salary color
                        },
                        new PieSeries
                        {
                            Title = "Min Salary",
                            Values = new ChartValues<decimal> { minSalary },
                            DataLabels = true,
                            Fill = System.Windows.Media.Brushes.Red // Min Salary color
                        }
                    };
                }
            }
        }


        private void LoadDepartmentData()
        {
            string query = @"
                SELECT 
                    d.DepartmentName,
                    COUNT(e.EmployeeID) AS NumberOfEmployees
                FROM Department d
                LEFT JOIN Employee e ON d.DepartmentID = e.DepartmentID
                GROUP BY d.DepartmentName;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Update CartesianChart for department data
                cartesianChart1.Series = new SeriesCollection();

                foreach (DataRow row in dt.Rows)
                {
                    string departmentName = row["DepartmentName"].ToString();
                    int numberOfEmployees = Convert.ToInt32(row["NumberOfEmployees"]);

                    cartesianChart1.Series.Add(new ColumnSeries
                    {
                        Title = departmentName,
                        Values = new ChartValues<int> { numberOfEmployees }
                    });
                }
            }
        }

        private void LoadLeaveRequestsData()
        {
            string query = @"
        SELECT 
            lr.LeaveRequestID,
            e.FirstName + ' ' + e.LastName AS EmployeeName,
            lr.StartDate,
            lr.EndDate,
            lr.Reason,
            lr.Status,
            lr.RequestedDate
        FROM LeaveRequest lr
        JOIN Employee e ON lr.EmployeeID = e.EmployeeID;
        
        SELECT 
            Status, 
            COUNT(*) AS StatusCount
        FROM LeaveRequest
        GROUP BY Status;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                // Update DataGridView for leave requests data
                DataGridView1.DataSource = ds.Tables[0];

                // Optional: Format the DataGridView columns as needed
                DataGridView1.Columns["StartDate"].DefaultCellStyle.Format = "d";
                DataGridView1.Columns["EndDate"].DefaultCellStyle.Format = "d";
                DataGridView1.Columns["RequestedDate"].DefaultCellStyle.Format = "d";

                // Update PieChart for leave requests status
                DataTable statusTable = ds.Tables[1];
                SeriesCollection leaveRequestSeries = new SeriesCollection();
                foreach (DataRow row in statusTable.Rows)
                {
                    string status = row["Status"].ToString();
                    int count = Convert.ToInt32(row["StatusCount"]);

                    System.Windows.Media.Brush color = System.Windows.Media.Brushes.Gray; // Default color
                    if (status == "Approved")
                    {
                        color = System.Windows.Media.Brushes.Green; // Approved color
                    }
                    else if (status == "Pending")
                    {
                        color = System.Windows.Media.Brushes.Orange; // Pending color
                    }
                    else if (status == "Rejected")
                    {
                        color = System.Windows.Media.Brushes.Red; // Rejected color
                    }

                    leaveRequestSeries.Add(new PieSeries
                    {
                        Title = status,
                        Values = new ChartValues<int> { count },
                        DataLabels = true,
                        Fill = color
                    });
                }

                pieChart2.Series = leaveRequestSeries;
            }
        }


        public Dashboard(string fullName, int userId)
        {
            InitializeComponent();
            this.fullName = fullName;
            this.userId = userId;
            //lblWelcome.Text = $"Welcome, {fullName}";
            this.IsMdiContainer = true;
        }

        private void OpenNewForm(Form newForm)
        {
            // Open the new form
            newForm.MdiParent = this;
            newForm.Show();
        }

        private void OpenForm(Form form)
        {
            form.MdiParent = this;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            // Implement the corresponding form and uncomment the line below
            // OpenForm(new ReportsForm());            
        }

        private void btnEmployeeManagement_Click(object sender, EventArgs e)
        {
            //OpenForm(new EmployeeList());(
            OpenNewForm(new EmployeeList());
        }

        private void btnPositionManagement_Click(object sender, EventArgs e)
        {
            //OpenForm(new PositionList());
            OpenNewForm(new DepartmentList());
        }

        private void btnLeaveRequests_Click(object sender, EventArgs e)
        {
            //OpenForm(new LeaveRequestList());
            OpenNewForm(new LeaveRequestList());
        }

        private void btnUserManagement_Click(object sender, EventArgs e)
        {
            //OpenNewForm(new UsersList());
            OpenNewForm(new UsersList());
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Close();
            Login loginForm = new Login();
            loginForm.Show();
        }

        private void btnPromotions_Click(object sender, EventArgs e)
        {
            OpenNewForm(new PositionList());
        }
    }
}
