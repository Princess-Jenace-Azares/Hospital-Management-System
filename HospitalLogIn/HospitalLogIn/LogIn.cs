
using System;
using System.Windows.Forms;

namespace HospitalLogIn
{
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();

            txtPassword.PasswordChar = '*';

            // Prevent duplicate event connections
            btnLogin.Click -= btnLogin_Click;
            btnLogin.Click += btnLogin_Click;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string role = "";

            // Check selected role
            if (rdoAdmin.Checked)
            {
                role = "Admin";
            }
            else if (rdoBillingStaff.Checked)
            {
                role = "Billing";
            }
            else if (rdoCashier.Checked)
            {
                role = "Cashier";
            }
            else
            {
                MessageBox.Show("Please select your role.");
                return;
            }

            // Validate login credentials
            bool valid = false;

            if (role == "Admin" &&
                username == "admin" &&
                password == "admin123")
            {
                valid = true;
            }
            else if (role == "Billing" &&
                     username == "billing" &&
                     password == "billing123")
            {
                valid = true;
            }
            else if (role == "Cashier" &&
                     username == "cashier" &&
                     password == "cashier123")
            {
                valid = true;
            }

            if (valid)
            {
                try
                {
                    LandingPage landing = new LandingPage(role);

                    this.Hide();

                    landing.ShowDialog();

                    this.Close();
                }
                catch (Exception ex)
                {
                    this.Show();

                    MessageBox.Show(
                        "Unable to open the Landing Page:\n" + ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            else
            {
                MessageBox.Show(
                    "Invalid username, password, or role.",
                    "Login Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void LogIn_Load(object sender, EventArgs e)
        {
        }
    }
}
