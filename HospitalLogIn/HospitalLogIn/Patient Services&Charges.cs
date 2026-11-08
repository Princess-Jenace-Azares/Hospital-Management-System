
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HospitalLogIn
{
    public partial class Patient_Services_Charges : Form
    {
        public Patient_Services_Charges()
        {
            InitializeComponent();

            btnAddService.Click += btnAddService_Click;
            btnClear.Click += btnClear_Click;
            cmbService.SelectedIndexChanged += cmbService_SelectedIndexChanged;

            cmbService.Items.AddRange(new object[]
            {
                "Consultation",
                "Laboratory",
                "X-Ray",
                "Medical Checkup",
                "Medication",
                "Room Service"
            });

            this.Shown += Patient_Services_Charges_Shown;
        }

        private void Patient_Services_Charges_Shown(object sender, EventArgs e)
        {
            LoadPatientIDs();
            LoadServices();
        }

        private void LoadPatientIDs()
        {
            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = "SELECT PatientID FROM Patient_Records";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    cmbPatientId.DataSource = dt;
                    cmbPatientId.DisplayMember = "PatientID";
                    cmbPatientId.ValueMember = "PatientID";
                    cmbPatientId.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load Patient IDs.\n" + ex.Message,
                    "Database Error"
                );
            }
        }

        private void LoadServices()
        {
            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = "SELECT * FROM Services_Charges";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    dgvServices.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load service records.\n" + ex.Message,
                    "Database Error"
                );
            }
        }

        private void cmbService_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal price = 0;

            switch (cmbService.Text)
            {
                case "Consultation":
                    price = 500;
                    break;

                case "Laboratory":
                    price = 1000;
                    break;

                case "X-Ray":
                    price = 1500;
                    break;

                case "Medical Checkup":
                    price = 800;
                    break;

                case "Medication":
                    price = 300;
                    break;

                case "Room Service":
                    price = 2000;
                    break;
            }

            txtTotalAmount.Text = price.ToString("0.00");
        }

        private void btnAddService_Click(object sender, EventArgs e)
        {
            if (cmbPatientId.SelectedValue == null ||
                cmbService.SelectedIndex == -1 ||
                !int.TryParse(txtQuantity.Text, out int quantity) ||
                quantity <= 0 ||
                !decimal.TryParse(txtTotalAmount.Text, out decimal price) ||
                price <= 0)
            {
                MessageBox.Show("Please complete the service information.");
                return;
            }

            decimal total = price * quantity;

            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"INSERT INTO Services_Charges
                    (PatientID, ServiceName, Quantity, TotalAmount)
                    VALUES
                    (@id, @service, @quantity, @amount)";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.Add("@id", SqlDbType.Int).Value =
                        Convert.ToInt32(cmbPatientId.SelectedValue);

                    cmd.Parameters.Add("@service", SqlDbType.VarChar, 100).Value =
                        cmbService.Text;

                    cmd.Parameters.Add("@quantity", SqlDbType.Int).Value =
                        quantity;

                    cmd.Parameters.Add("@amount", SqlDbType.Decimal).Value = total;
                    cmd.Parameters["@amount"].Precision = 10;
                    cmd.Parameters["@amount"].Scale = 2;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show(
                    "Service added successfully!\nTotal Amount: " +
                    total.ToString("0.00")
                );

                LoadServices();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Add Service Error: " + ex.Message,
                    "Database Error"
                );
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            cmbPatientId.SelectedIndex = -1;
            cmbService.SelectedIndex = -1;
            txtQuantity.Clear();
            txtTotalAmount.Clear();
        }

        private void Patient_Services_Charges_Load(object sender, EventArgs e)
        {

        }

        private void dgvServices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
