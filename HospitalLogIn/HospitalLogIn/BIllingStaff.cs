
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HospitalLogIn
{
    public partial class BIllingStaff : Form
    {
        public BIllingStaff()
        {
            InitializeComponent();

            txtBillID.ReadOnly = true;

            cmbPaymentStatus.Items.Clear();
            cmbPaymentStatus.Items.Add("Unpaid");
            cmbPaymentStatus.Items.Add("Partially Paid");
            cmbPaymentStatus.Items.Add("Paid");

            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnRefresh.Click += btnRefresh_Click;
            dgvBilling.CellClick += dgvBilling_CellClick;

            this.Shown += BIllingStaff_Shown;
        }

        private void BIllingStaff_Shown(object sender, EventArgs e)
        {
            LoadPatients();
            LoadBilling();
        }

        private void LoadPatients()
        {
            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"SELECT PatientID, PatientName
                                     FROM Patient_Records";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable table = new DataTable();

                    adapter.Fill(table);

                    cmbPatientName.DataSource = table;
                    cmbPatientName.DisplayMember = "PatientName";
                    cmbPatientName.ValueMember = "PatientID";
                    cmbPatientName.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load patients.\n" + ex.Message,
                    "Database Error"
                );
            }
        }

        private void LoadBilling()
        {
            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"
                        SELECT
                            b.BillNo AS BillID,
                            b.PatientID,
                            p.PatientName,
                            b.TotalAmount AS Amount,
                            b.AmountPaid,
                            b.Balance,
                            b.BillStatus AS PaymentStatus
                        FROM Bill_Records b
                        INNER JOIN Patient_Records p
                        ON b.PatientID = p.PatientID";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable table = new DataTable();

                    adapter.Fill(table);

                    dgvBilling.DataSource = table;

                    dgvBilling.AutoSizeColumnsMode =
                        DataGridViewAutoSizeColumnsMode.Fill;

                    dgvBilling.SelectionMode =
                        DataGridViewSelectionMode.FullRowSelect;

                    dgvBilling.ReadOnly = true;
                    dgvBilling.AllowUserToAddRows = false;

                    if (dgvBilling.Columns["PatientID"] != null)
                    {
                        dgvBilling.Columns["PatientID"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load billing records.\n" + ex.Message,
                    "Database Error"
                );
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbPatientName.SelectedValue == null ||
                string.IsNullOrWhiteSpace(txtAmount.Text) ||
                cmbPaymentStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please complete all billing fields.");
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount) ||
                amount <= 0)
            {
                MessageBox.Show("Please enter a valid amount.");
                return;
            }

            try
            {
                int patientID =
                    Convert.ToInt32(cmbPatientName.SelectedValue);

                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"
                        INSERT INTO Bill_Records
                        (PatientID, TotalAmount, AmountPaid, BillStatus)
                        VALUES
                        (@PatientID, @Amount, 0, @Status);

                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value =
                        patientID;

                    SqlParameter amountParam =
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal);

                    amountParam.Precision = 10;
                    amountParam.Scale = 2;
                    amountParam.Value = amount;

                    cmd.Parameters.Add("@Status", SqlDbType.VarChar, 30).Value =
                        cmbPaymentStatus.Text;

                    con.Open();

                    int billID = Convert.ToInt32(cmd.ExecuteScalar());

                    txtBillID.Text = billID.ToString();
                }

                MessageBox.Show("Billing record added successfully!");

                LoadBilling();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Add Billing Error:\n" + ex.Message,
                    "Database Error"
                );
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtBillID.Text, out int billID))
            {
                MessageBox.Show("Please select a billing record.");
                return;
            }

            if (cmbPatientName.SelectedValue == null ||
                !decimal.TryParse(txtAmount.Text, out decimal amount) ||
                amount <= 0 ||
                cmbPaymentStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter valid billing information.");
                return;
            }

            try
            {
                int patientID =
                    Convert.ToInt32(cmbPatientName.SelectedValue);

                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"
                        UPDATE Bill_Records
                        SET PatientID = @PatientID,
                            TotalAmount = @Amount,
                            BillStatus = @Status
                        WHERE BillNo = @BillID";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value =
                        patientID;

                    SqlParameter amountParam =
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal);

                    amountParam.Precision = 10;
                    amountParam.Scale = 2;
                    amountParam.Value = amount;

                    cmd.Parameters.Add("@Status", SqlDbType.VarChar, 30).Value =
                        cmbPaymentStatus.Text;

                    cmd.Parameters.Add("@BillID", SqlDbType.Int).Value =
                        billID;

                    con.Open();

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                        MessageBox.Show("Billing record updated successfully!");
                    else
                        MessageBox.Show("Billing record not found.");
                }

                LoadBilling();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Update Billing Error:\n" + ex.Message,
                    "Database Error"
                );
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtBillID.Text, out int billID))
            {
                MessageBox.Show("Please select a billing record.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this billing record?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query =
                        "DELETE FROM Bill_Records WHERE BillNo = @BillID";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.Add("@BillID", SqlDbType.Int).Value =
                        billID;

                    con.Open();

                    int deleted = cmd.ExecuteNonQuery();

                    if (deleted > 0)
                        MessageBox.Show("Billing record deleted successfully!");
                    else
                        MessageBox.Show("Billing record not found.");
                }

                LoadBilling();
                ClearFields();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "This bill cannot be deleted because it may have existing payment records.\n\n"
                    + ex.Message,
                    "Delete Error"
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Delete Billing Error:\n" + ex.Message,
                    "Database Error"
                );
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPatients();
            LoadBilling();
            ClearFields();
        }

        private void dgvBilling_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvBilling.Rows[e.RowIndex];

            txtBillID.Text =
                Convert.ToString(row.Cells["BillID"].Value);

            cmbPatientName.SelectedValue =
                Convert.ToInt32(row.Cells["PatientID"].Value);

            txtAmount.Text =
                Convert.ToDecimal(row.Cells["Amount"].Value).ToString("0.00");

            cmbPaymentStatus.Text =
                Convert.ToString(row.Cells["PaymentStatus"].Value);
        }

        private void ClearFields()
        {
            txtBillID.Clear();
            cmbPatientName.SelectedIndex = -1;
            txtAmount.Clear();
            cmbPaymentStatus.SelectedIndex = -1;
        }

        private void BIllingStaff_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
