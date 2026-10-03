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
    public partial class Attendant__Nurse_Dashboard : Form
    {
        public Attendant__Nurse_Dashboard()
        {
            InitializeComponent();
            SetupDashboard();
        }

        private void SetupDashboard()
        {
            this.Text = "Attendant / Nurse Dashboard";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;

            Label title = new Label();

            title.Text = "ATTENDANT / NURSE DASHBOARD";
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