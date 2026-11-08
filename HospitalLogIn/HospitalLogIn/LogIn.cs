using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HospitalLogIn

{
    public partial class LogIn : Form
    {


        public LogIn()
        {
            InitializeComponent();

            txtPassword.PasswordChar = '*';
            btnLogin.Click += btnLogin_Click;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string role = "";

            if (rdoAdmin.Checked)
                role = "Admin";
            else if (rdoBillingStaff.Checked)
                role = "Billing";
            else if (rdoCashier.Checked)
                role = "Cashier";
            else
            {
                MessageBox.Show("Please select your role.");
                return;
            }

            bool valid = false;

            if (role == "Admin" &&
                username == "admin" && password == "admin123")
                valid = true;

            else if (role == "Billing" &&
                username == "billing" && password == "billing123")
                valid = true;

            else if (role == "Cashier" &&
                username == "cashier" && password == "cashier123")
                valid = true;

            if (valid)
            {
                LandingPage landing = new LandingPage(role);

                this.Hide();
                landing.ShowDialog();
                this.Close();
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


    }
}
