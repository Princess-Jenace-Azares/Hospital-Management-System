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
    public partial class Admin_Dashboard : Form
    {
        public Admin_Dashboard()
        {
            InitializeComponent();
            SetupDashboard();
        }

              private void SetupDashboard()
        {


            this.Text = "Admin Dashboard";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;

            // Dashboard title
            Label title = new Label();

            title.Text = "ADMIN DASHBOARD";
            title.Font = new Font(
                "Segoe UI",
                28,
                FontStyle.Bold
            );

            title.AutoSize = true;
            title.Location = new Point(50, 40);

            this.Controls.Add(title);
        }

        private void Admin_Dashboard_Load(object sender, EventArgs e)
        {

        }
    }
}
