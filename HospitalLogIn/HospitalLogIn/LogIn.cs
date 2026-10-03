using HospitalBillingSystem;
using System;
using System.Windows.Forms;

namespace HospitalLogIn
{
    public partial class LogIn : Form
    {
        private AuthService authService;
        private bool dashboardOpened = false;

        public LogIn()
        {
            InitializeComponent();

            authService = new AuthService();

            txtPassword.PasswordChar = '●';
            rbAdmin.Checked = true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (dashboardOpened)
                return;

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string selectedRole = "";

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

            bool loginSuccessful = authService.Login(
                username,
                password,
                selectedRole
            );

            if (loginSuccessful)
            {
                dashboardOpened = true;

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
            else
            {
                dashboardOpened = false;

                MessageBox.Show(
                    "Dashboard could not be opened.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void Dashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        private void LogIn_Load(object sender, EventArgs e)
        {
        }
    }
}
                    