using HospitalBillingSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalLogIn
{
    public partial class Billing_Staff_Dashboard : Form
    {
        private DatabaseConnection databaseConnection;

        private class PatientItem
        {
            public int PatientID { get; set; }
            public string PatientName { get; set; }

            public override string ToString()
            {
                return PatientName;
            }
        }
        public Billing_Staff_Dashboard()
        {

             
        
            InitializeComponent();
            databaseConnection = new DatabaseConnection();

            cmbPaymentStatus.Items.Clear();
            cmbPaymentStatus.Items.Add("Unpaid");
            cmbPaymentStatus.Items.Add("Partially Paid");
            cmbPaymentStatus.Items.Add("Paid");
            cmbPaymentStatus.SelectedIndex = 0;

            btnAddBill.Click += btnAddBill_Click;
            btnUpdateBill.Click += btnUpdateBill_Click;
            btnDeleteBill.Click += btnDeleteBill_Click;
            btnRefreshBills.Click += btnRefreshBills_Click;

            dgvBilling.CellClick += dgvBilling_CellClick;

            LoadBillingPatients();
            LoadBillingRecords();
        }

        private void LoadBillingPatients()
        {
            try
            {
                using (SqlConnection connection = databaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT PatientID, PatientName
                        FROM Patients
                        ORDER BY PatientName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            cmbBillingPatient.Items.Clear();

                            while (reader.Read())
                            {
                                PatientItem patient = new PatientItem
                                {
                                    PatientID = Convert.ToInt32(reader["PatientID"]),
                                    PatientName = reader["PatientName"].ToString()
                                };

                                cmbBillingPatient.Items.Add(patient);
                            }
                        }
                    }
                }

                if (cmbBillingPatient.Items.Count > 0)
                {
                    cmbBillingPatient.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load patients.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadBillingRecords()
        {
            try
            {
                using (SqlConnection connection = databaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT
                            B.BillID,
                            B.PatientID,
                            P.PatientName,
                            B.Amount,
                            B.PaymentStatus,
                            B.PaymentDate
                        FROM Billing B
                        INNER JOIN Patients P
                            ON B.PatientID = P.PatientID
                        ORDER BY B.BillID DESC";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dgvBilling.DataSource = table;
                    }
                }

                dgvBilling.ReadOnly = true;
                dgvBilling.AllowUserToAddRows = false;
                dgvBilling.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvBilling.MultiSelect = false;
                dgvBilling.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load billing records.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnAddBill_Click(object sender, EventArgs e)
        {
            if (!(cmbBillingPatient.SelectedItem is PatientItem patient))
            {
                MessageBox.Show(
                    "Please select a patient.",
                    "Billing",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                MessageBox.Show(
                    "Please enter a valid amount.",
                    "Billing",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtAmount.Focus();
                return;
            }

            if (amount <= 0)
            {
                MessageBox.Show(
                    "Amount must be greater than zero.",
                    "Billing",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtAmount.Focus();
                return;
            }

            if (cmbPaymentStatus.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select a payment status.",
                    "Billing",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = databaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        INSERT INTO Billing
                        (
                            PatientID,
                            Amount,
                            PaymentStatus,
                            PaymentDate
                        )
                        VALUES
                        (
                            @PatientID,
                            @Amount,
                            @PaymentStatus,
                            @PaymentDate
                        )";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@PatientID", SqlDbType.Int).Value =
                            patient.PatientID;

                        SqlParameter amountParameter =
                            command.Parameters.Add("@Amount", SqlDbType.Decimal);

                        amountParameter.Precision = 10;
                        amountParameter.Scale = 2;
                        amountParameter.Value = amount;

                        command.Parameters.Add(
                            "@PaymentStatus",
                            SqlDbType.VarChar,
                            30).Value = cmbPaymentStatus.Text;

                        command.Parameters.Add(
                            "@PaymentDate",
                            SqlDbType.Date).Value = dtpPaymentDate.Value.Date;

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Bill added successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadBillingRecords();
                ClearBillingFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to add bill.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dgvBilling_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            try
            {
                DataGridViewRow row = dgvBilling.Rows[e.RowIndex];

                txtBillID.Text = row.Cells["BillID"].Value.ToString();

                txtAmount.Text = row.Cells["Amount"].Value.ToString();

                cmbPaymentStatus.Text =
                    row.Cells["PaymentStatus"].Value.ToString();

                dtpPaymentDate.Value =
                    Convert.ToDateTime(row.Cells["PaymentDate"].Value);

                int patientID =
                    Convert.ToInt32(row.Cells["PatientID"].Value);

                foreach (PatientItem patient in cmbBillingPatient.Items)
                {
                    if (patient.PatientID == patientID)
                    {
                        cmbBillingPatient.SelectedItem = patient;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to select billing record.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnUpdateBill_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtBillID.Text, out int billID))
            {
                MessageBox.Show(
                    "Please select a bill from the table first.",
                    "Billing",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!(cmbBillingPatient.SelectedItem is PatientItem patient))
            {
                MessageBox.Show(
                    "Please select a patient.",
                    "Billing",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                MessageBox.Show(
                    "Please enter a valid amount.",
                    "Billing",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (amount <= 0)
            {
                MessageBox.Show(
                    "Amount must be greater than zero.",
                    "Billing",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = databaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        UPDATE Billing
                        SET
                            PatientID = @PatientID,
                            Amount = @Amount,
                            PaymentStatus = @PaymentStatus,
                            PaymentDate = @PaymentDate
                        WHERE BillID = @BillID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@BillID", SqlDbType.Int).Value =
                            billID;

                        command.Parameters.Add("@PatientID", SqlDbType.Int).Value =
                            patient.PatientID;

                        SqlParameter amountParameter =
                            command.Parameters.Add("@Amount", SqlDbType.Decimal);

                        amountParameter.Precision = 10;
                        amountParameter.Scale = 2;
                        amountParameter.Value = amount;

                        command.Parameters.Add(
                            "@PaymentStatus",
                            SqlDbType.VarChar,
                            30).Value = cmbPaymentStatus.Text;

                        command.Parameters.Add(
                            "@PaymentDate",
                            SqlDbType.Date).Value = dtpPaymentDate.Value.Date;

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Bill updated successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadBillingRecords();
                ClearBillingFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to update bill.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnDeleteBill_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtBillID.Text, out int billID))
            {
                MessageBox.Show(
                    "Please select a bill from the table first.",
                    "Billing",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this bill?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection connection = databaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        DELETE FROM Billing
                        WHERE BillID = @BillID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@BillID", SqlDbType.Int).Value =
                            billID;

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Bill deleted successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadBillingRecords();
                ClearBillingFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to delete bill.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnRefreshBills_Click(object sender, EventArgs e)
        {
            LoadBillingPatients();
            LoadBillingRecords();

            MessageBox.Show(
                "Billing records refreshed.",
                "Refresh",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void ClearBillingFields()
        {
            txtBillID.Clear();
            txtAmount.Clear();

            if (cmbBillingPatient.Items.Count > 0)
                cmbBillingPatient.SelectedIndex = 0;

            if (cmbPaymentStatus.Items.Count > 0)
                cmbPaymentStatus.SelectedIndex = 0;

            dtpPaymentDate.Value = DateTime.Today;

            dgvBilling.ClearSelection();
        }
    }
}
