
using System;
using System.Windows.Forms;

namespace HospitalLogIn
{
    public partial class LandingPage : Form
    {
        private string userRole;
        private Form activeChildForm;

        public LandingPage(string role)
        {
            InitializeComponent();

            userRole = role;

            lblPatientManagement.Click += lblPatientManagement_Click;
            lblAdmission.Click += lblAdmission_Click;
            lblPatientServices.Click += lblPatientServices_Click;
            lblBilling.Click += lblBilling_Click;
            lblPayment.Click += lblPayment_Click;
            lblDischarge.Click += lblDischarge_Click;
            lblReports.Click += lblReports_Click;
            lblLogOut.Click += lblLogOut_Click;

            ApplyRolePermissions();
        }

        private void ApplyRolePermissions()
        {
            lblPatientManagement.Visible = true;
            lblAdmission.Visible = true;
            lblPatientServices.Visible = true;
            lblBilling.Visible = true;
            lblPayment.Visible = true;
            lblDischarge.Visible = true;
            lblReports.Visible = true;
        }

        private bool HasAccess(params string[] requiredRoles)
        {
            string currentRole = NormalizeRole(userRole);

            foreach (string role in requiredRoles)
            {
                if (currentRole == NormalizeRole(role))
                    return true;
            }

            return false;
        }

        private string NormalizeRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return "";

            string normalized = role.Replace(" ", "")
                                    .Replace("_", "")
                                    .Trim()
                                    .ToLowerInvariant();

            if (normalized == "billingstaff")
                return "billing";

            if (normalized == "administrator")
                return "admin";

            return normalized;
        }

        private void OpenFormInPanel(
            Form childForm,
            params string[] requiredRoles)
        {
            try
            {
                if (activeChildForm != null)
                {
                    panelContent.Controls.Remove(activeChildForm);
                    activeChildForm.Close();
                    activeChildForm.Dispose();
                    activeChildForm = null;
                }

                bool viewOnly = requiredRoles.Length == 0 ||
                                !HasAccess(requiredRoles);

                if (viewOnly)
                {
                    RoleAccessHelper.SetViewOnly(childForm);
                }

                activeChildForm = childForm;

                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
                childForm.Dock = DockStyle.Fill;

                panelContent.Controls.Add(childForm);

                childForm.Show();
                childForm.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to open the form: " + ex.Message,
                    "Form Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void lblPatientManagement_Click(
            object sender, EventArgs e)
        {
            OpenFormInPanel(
                new Admin_Dashboard(),
                "Admin"
            );
        }

        private void lblAdmission_Click(
            object sender, EventArgs e)
        {
            OpenFormInPanel(
                new Attendant__Nurse_Dashboard(),
                "Admin"
            );
        }

        private void lblPatientServices_Click(
            object sender, EventArgs e)
        {
            OpenFormInPanel(
                new Patient_Services_Charges(),
                "Billing"
            );
        }

        private void lblBilling_Click(
            object sender, EventArgs e)
        {
            OpenFormInPanel(
                new BIllingStaff(),
                "Billing"
            );
        }

        private void lblPayment_Click(
            object sender, EventArgs e)
        {
            OpenFormInPanel(
                new Payment_Form(),
                "Cashier"
            );
        }

        private void lblDischarge_Click(
            object sender, EventArgs e)
        {
            OpenFormInPanel(
                new Discharge_Form(),
                "Admin",
                "Cashier"
            );
        }

        private void lblReports_Click(
            object sender, EventArgs e)
        {
            OpenFormInPanel(
                new Reports_Form(),
                "Admin",
                "Cashier"
            );
        }

        private void LandingPage_Load(object sender, EventArgs e)
        {
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }


        private void lblLogOut_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to log out?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                LogIn login = new LogIn();

                login.FormClosed += (s, args) =>
                {
                    this.Close();
                };

                login.Show();
                this.Hide();
            }
        }


        public static class RoleAccessHelper
        {
            public static void SetViewOnly(Form form)
            {
                SetControlsViewOnly(form.Controls);
            }

            private static void SetControlsViewOnly(
                Control.ControlCollection controls)
            {
                foreach (Control control in controls)
                {
                    if (control is DataGridView dgv)
                    {
                        dgv.ReadOnly = true;
                        dgv.AllowUserToAddRows = false;
                        dgv.AllowUserToDeleteRows = false;
                        dgv.SelectionMode =
                            DataGridViewSelectionMode.FullRowSelect;
                    }
                    else if (control is TextBox txt)
                    {
                        txt.ReadOnly = true;
                    }
                    else if (control is RichTextBox richText)
                    {
                        richText.ReadOnly = true;
                    }
                    else if (control is ComboBox ||
                             control is DateTimePicker ||
                             control is NumericUpDown ||
                             control is CheckBox ||
                             control is RadioButton ||
                             control is LinkLabel)
                    {
                        control.Enabled = false;
                    }
                    else if (control is Button btn)
                    {
                        if (btn.Name.StartsWith(
                            "btnRefresh",
                            StringComparison.OrdinalIgnoreCase))
                        {
                            btn.Enabled = true;
                        }
                        else
                        {
                            btn.Enabled = false;
                        }
                    }

                    if (control.HasChildren)
                    {
                        SetControlsViewOnly(control.Controls);
                    }
                }
            }
        }
    }
}

