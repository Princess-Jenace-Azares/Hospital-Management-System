
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HospitalLogIn
{
    public partial class Patient_Services_Charges : Form
    {
        private decimal selectedUnitPrice = 0;

        public Patient_Services_Charges()
        {
            InitializeComponent();

            btnAddService.Click += btnAddService_Click;
            btnClear.Click += btnClear_Click;

            cmbService.SelectedIndexChanged +=
                cmbService_SelectedIndexChanged;

            cmbAdmissionID.SelectedIndexChanged +=
                cmbAdmissionID_SelectedIndexChanged;

            txtQuantity.TextChanged += txtQuantity_TextChanged;

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

        // FORM LOAD
        private void Patient_Services_Charges_Shown(
            object sender, EventArgs e)
        {
            LoadActiveAdmissions();
            LoadServices();
        }

        // LOAD ACTIVE ADMISSIONS
        private void LoadActiveAdmissions()
        {
            try
            {
                using (SqlConnection con =
                    DatabaseConnection.GetConnection())
                {
                    string query = @"
                        SELECT
                            a.AdmissionID,
                            a.PatientID,
                            p.PatientName,
                            CAST(a.AdmissionID AS VARCHAR(20))
                            + ' - ' + p.PatientName AS AdmissionInfo
                        FROM Admission_Records a
                        INNER JOIN Patient_Records p
                            ON a.PatientID = p.PatientID
                        WHERE a.Status = 'Admitted'
                        ORDER BY a.AdmissionID DESC";

                    SqlDataAdapter da =
                        new SqlDataAdapter(query, con);

                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    cmbAdmissionID.DataSource = dt;
                    cmbAdmissionID.DisplayMember = "AdmissionInfo";
                    cmbAdmissionID.ValueMember = "AdmissionID";
                    cmbAdmissionID.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load active admissions.\n" + ex.Message,
                    "Database Error"
                );
            }
        }

        // WHEN AN ADMISSION IS SELECTED
        private void cmbAdmissionID_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            if (cmbAdmissionID.SelectedIndex >= 0)
            {
                DataRowView row =
                    cmbAdmissionID.SelectedItem as DataRowView;

                if (row != null)
                {
                    txtPatientID.Text =
                        row["PatientID"].ToString();

                    txtPatientName.Text =
                        row["PatientName"].ToString();
                }
            }
            else
            {
                txtPatientID.Clear();
                txtPatientName.Clear();
            }
        }

        // LOAD SERVICE RECORDS
        private void LoadServices()
        {
            try
            {
                using (SqlConnection con =
                    DatabaseConnection.GetConnection())
                {
                    string query = @"
                        SELECT
                            S.ServiceID,
                            S.AdmissionID,
                            S.PatientID,
                            P.PatientName,
                            S.ServiceName,
                            S.Quantity,
                            S.UnitPrice,
                            S.TotalAmount,
                            S.ServiceDate
                        FROM Services_Charges S
                        INNER JOIN Patient_Records P
                            ON S.PatientID = P.PatientID
                        ORDER BY S.ServiceID DESC";

                    SqlDataAdapter da =
                        new SqlDataAdapter(query, con);

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

        // SELECT SERVICE AND GET PRICE
        private void cmbService_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            selectedUnitPrice = 0;

            switch (cmbService.Text)
            {
                case "Consultation":
                    selectedUnitPrice = 500;
                    break;

                case "Laboratory":
                    selectedUnitPrice = 1000;
                    break;

                case "X-Ray":
                    selectedUnitPrice = 1500;
                    break;

                case "Medical Checkup":
                    selectedUnitPrice = 800;
                    break;

                case "Medication":
                    selectedUnitPrice = 300;
                    break;

                case "Room Service":
                    selectedUnitPrice = 2000;
                    break;
            }

            if (cmbService.SelectedIndex >= 0 &&
                string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                txtQuantity.Text = "1";
            }

            CalculateTotal();
        }

        // QUANTITY CHANGED
        private void txtQuantity_TextChanged(
            object sender, EventArgs e)
        {
            CalculateTotal();
        }

        // CALCULATE TOTAL AMOUNT
        private void CalculateTotal()
        {
            if (selectedUnitPrice > 0 &&
                int.TryParse(txtQuantity.Text, out int quantity) &&
                quantity > 0)
            {
                decimal total = selectedUnitPrice * quantity;

                txtTotalAmount.Text = total.ToString("0.00");
            }
            else
            {
                txtTotalAmount.Clear();
            }
        }

        // ADD SERVICE
        private void btnAddService_Click(
            object sender, EventArgs e)
        {
            if (cmbAdmissionID.SelectedIndex == -1 ||
                cmbService.SelectedIndex == -1 ||
                !int.TryParse(txtPatientID.Text, out int patientID) ||
                !int.TryParse(txtQuantity.Text, out int quantity) ||
                quantity <= 0 ||
                selectedUnitPrice <= 0 ||
                !decimal.TryParse(txtTotalAmount.Text, out decimal total))
            {
                MessageBox.Show(
                    "Please complete all service information correctly.",
                    "Incomplete Information"
                );

                return;
            }

            DataRowView selectedAdmission =
                cmbAdmissionID.SelectedItem as DataRowView;

            if (selectedAdmission == null)
            {
                MessageBox.Show("Please select a valid admission.");
                return;
            }

            int admissionID =
                Convert.ToInt32(selectedAdmission["AdmissionID"]);

            try
            {
                using (SqlConnection con =
                    DatabaseConnection.GetConnection())
                {
                    string query = @"
                        INSERT INTO Services_Charges
                        (
                            AdmissionID,
                            PatientID,
                            ServiceName,
                            Quantity,
                            UnitPrice,
                            TotalAmount
                        )
                        SELECT
                            A.AdmissionID,
                            A.PatientID,
                            @service,
                            @quantity,
                            @unitPrice,
                            @totalAmount
                        FROM Admission_Records A
                        WHERE A.AdmissionID = @admissionID
                          AND A.PatientID = @patientID
                          AND A.Status = 'Admitted'";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.Add("@admissionID",
                        SqlDbType.Int).Value = admissionID;

                    cmd.Parameters.Add("@patientID",
                        SqlDbType.Int).Value = patientID;

                    cmd.Parameters.Add("@service",
                        SqlDbType.NVarChar, 150).Value =
                        cmbService.Text;

                    cmd.Parameters.Add("@quantity",
                        SqlDbType.Int).Value = quantity;

                    SqlParameter unitPriceParameter =
                        cmd.Parameters.Add("@unitPrice",
                            SqlDbType.Decimal);

                    unitPriceParameter.Precision = 10;
                    unitPriceParameter.Scale = 2;
                    unitPriceParameter.Value = selectedUnitPrice;

                    SqlParameter totalParameter =
                        cmd.Parameters.Add("@totalAmount",
                            SqlDbType.Decimal);

                    totalParameter.Precision = 10;
                    totalParameter.Scale = 2;
                    totalParameter.Value = total;

                    con.Open();

                    int result = cmd.ExecuteNonQuery();

                    if (result == 0)
                    {
                        MessageBox.Show(
                            "The selected admission is no longer active. " +
                            "Please refresh and select another admission."
                        );

                        LoadActiveAdmissions();
                        return;
                    }
                }

                MessageBox.Show(
                    "Service added successfully!\n\n" +
                    "Patient: " + txtPatientName.Text +
                    "\nService: " + cmbService.Text +
                    "\nQuantity: " + quantity +
                    "\nUnit Price: " +
                    selectedUnitPrice.ToString("0.00") +
                    "\nTotal Amount: " + total.ToString("0.00"),
                    "Success"
                );

                LoadServices();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Add Service Error:\n" + ex.Message,
                    "Database Error"
                );
            }
        }

        // CLEAR BUTTON
        private void btnClear_Click(
            object sender, EventArgs e)
        {
            ClearFields();
        }

        // CLEAR ALL FIELDS
        private void ClearFields()
        {
            cmbAdmissionID.SelectedIndex = -1;
            cmbService.SelectedIndex = -1;

            txtPatientID.Clear();
            txtPatientName.Clear();
            txtQuantity.Clear();
            txtTotalAmount.Clear();

            selectedUnitPrice = 0;
        }

        private void Patient_Services_Charges_Load(
            object sender, EventArgs e)
        {
        }

        private void panel1_Paint(
            object sender, PaintEventArgs e)
        {
        }
    }
}
