using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRMaster
{
    public partial class LeaveRequestList : Form
    {
        public LeaveRequestList()
        {
            InitializeComponent();
        }

        private void btnGetBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
