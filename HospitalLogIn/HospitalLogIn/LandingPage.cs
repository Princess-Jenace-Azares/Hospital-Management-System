
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

            // Connect label click events here ONLY if they are not
            // already connected in the Designer.
            lblPatientManagement.Click += lblPatientManagement_Click;
            lblAdmission.Click += lblAdmission_Click;
            lblPatientServices.Click += lblPatientServices_Click;
            lblBilling.Click += lblBilling_Click;
            lblPayment.Click += lblPayment_Click;
        }

        private void OpenFormInPanel(Form childForm)
        {
            try
            {
                // Close the previous child form
                if (activeChildForm != null)
                {
                    panelContent.Controls.Remove(activeChildForm);
                    activeChildForm.Close();
                    activeChildForm.Dispose();
                    activeChildForm = null;
                }

                // Set the new child form
                activeChildForm = childForm;

                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
                childForm.Dock = DockStyle.Fill;

                // Display inside the landing page panel
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



        private void lblPatientManagement_Click(object sender, EventArgs e)
        {
            OpenFormInPanel(new Admin_Dashboard());
        }


        private void lblAdmission_Click(object sender, EventArgs e)
        {
            OpenFormInPanel(new Attendant__Nurse_Dashboard());
        }

        private void lblPatientServices_Click(object sender, EventArgs e)
        {
            OpenFormInPanel(new Patient_Services_Charges());
        }

        private void lblBilling_Click(object sender, EventArgs e)
        {
            OpenFormInPanel(new BIllingStaff());
        }

        private void lblPayment_Click(object sender, EventArgs e)
        {
            OpenFormInPanel(new Payment_Form());
        }

        private void LandingPage_Load(object sender, EventArgs e)
        {

        }
    }
}
