using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalLogIn
{
    public partial class Billing_Staff_Dashboard : Form
    {
        public Billing_Staff_Dashboard()
        {
            InitializeComponent();
            SetupDashboard();
        }


        private void SetupDashboard()
        {
            this.Text = "Billing Staff Dashboard";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;

            Label title = new Label();

            title.Text = "BILLING STAFF DASHBOARD";
            title.Font = new Font(
                "Segoe UI",
                28,
                FontStyle.Bold
            );

            title.AutoSize = true;
            title.Location = new Point(50, 40);

            this.Controls.Add(title);
        }
    }
}