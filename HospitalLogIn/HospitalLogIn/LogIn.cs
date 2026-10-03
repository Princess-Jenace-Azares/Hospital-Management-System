using HospitalBillingSystem;
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
    public partial class LogIn : Form
    {
        private AuthService authService;

        public LogIn()
        {
            InitializeComponent();

            authService = new AuthService();

            // Hide password characters
            txtPassword.PasswordChar = '●';

            // Select Admin by default
            rbAdmin.Checked = true;

            // Connect the button event
            btnLogin.Click += btnLogin_Click;

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            string selectedRole = "";

            // Get selected role
            if (rbAdmin.Checked)
            {
                selectedRole = "Admin";
            }
            else if (rbBillingStaff.Checked)
            {
                selectedRole = "Billing Staff";
            }
            else if (rbAttendant.Checked)
            {
                selectedRole = "Attendant";
            }

            // Check username
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show(
                    "Please enter your username.",
                    "Login",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtUsername.Focus();
                return;
            }

            // Check password
            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
                    "Please enter your password.",
                    "Login",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtPassword.Focus();
                return;
            }

            // Check login credentials
            bool loginSuccessful = authService.Login(
                username,
                password,
                selectedRole
            );

            if (loginSuccessful)
            {
                MessageBox.Show(
                    "Login successful!",
                    "Welcome",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                OpenDashboard(selectedRole);
            }
            else
            {
                MessageBox.Show(
                    "Invalid username, password, or selected role.",
                    "Login Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        private void OpenDashboard(string role)
        {
            Form dashboard = null;

            switch (role)
            {
                case "Admin":
                    dashboard = new Admin_Dashboard();
                    break;

                case "Billing Staff":
                    dashboard = new Billing_Staff_Dashboard();
                    break;

                case "Attendant":
                    dashboard = new Attendant__Nurse_Dashboard();
                    break;
            }

            if (dashboard != null)
            {
                this.Hide();

                dashboard.FormClosed += Dashboard_FormClosed;

                dashboard.Show();
            }
        }

        private void Dashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
    }
}