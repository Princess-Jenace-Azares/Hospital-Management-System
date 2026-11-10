
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

        private bool HasAccess(string requiredRole)
        {
            return NormalizeRole(userRole) ==
                   NormalizeRole(requiredRole);
        }

        private string NormalizeRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return "";

            return role.Replace(" ", "")
                       .Replace("_", "")
                       .Trim()
                       .ToLowerInvariant();
        }

        private void OpenFormInPanel(
            Form childForm,
            string requiredRole = null)
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

                bool viewOnly = string.IsNullOrEmpty(requiredRole)
                    || !HasAccess(requiredRole);

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
                "Admin"
            );
        }

        private void lblReports_Click(
            object sender, EventArgs e)
        {
            OpenFormInPanel(
                new Reports_Form()
            );
        }

      
        private void LandingPage_Load(object sender, EventArgs e)
        {
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
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
                    if (btn.Name == "btnRefresh")
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
