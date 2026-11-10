
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HospitalLogIn
{
    public partial class BIllingStaff : Form
    {
        private int selectedAdmissionID = 0;

        public BIllingStaff()
        {
            InitializeComponent();

            txtBillID.ReadOnly = true;
            txtPatientName.ReadOnly = true;

            // Amount must be editable
            txtAmount.ReadOnly = false;

            cmbAdmissionID.DropDownStyle = ComboBoxStyle.DropDownList;

            cmbPaymentStatus.Items.Clear();
            cmbPaymentStatus.Items.Add("Unpaid");
            cmbPaymentStatus.Items.Add("Partially Paid");
            cmbPaymentStatus.Items.Add("Paid");

            btnAdd.Click -= btnGenerateBill_Click;
            btnAdd.Click += btnGenerateBill_Click;

            btnUpdate.Click -= btnUpdate_Click;
            btnUpdate.Click += btnUpdate_Click;

            btnDelete.Click -= btnDelete_Click;
            btnDelete.Click += btnDelete_Click;

            btnRefresh.Click -= btnRefresh_Click;
            btnRefresh.Click += btnRefresh_Click;

            cmbAdmissionID.SelectedIndexChanged -= cmbAdmissionID_SelectedIndexChanged;
            cmbAdmissionID.SelectedIndexChanged += cmbAdmissionID_SelectedIndexChanged;

            dgvBilling.CellClick -= dgvBilling_CellClick;
            dgvBilling.CellClick += dgvBilling_CellClick;

            this.Shown += BIllingStaff_Shown;
        }

        private void BIllingStaff_Shown(object sender, EventArgs e)
        {
            LoadAdmissions();
            LoadBilling();
        }

        // LOAD ADMISSION IDS
        private void LoadAdmissions()
        {
            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"
                        SELECT
                            a.AdmissionID,
                            a.PatientID,
                            p.PatientName
                        FROM Admission_Records a
                        INNER JOIN Patient_Records p
                            ON a.PatientID = p.PatientID
                        WHERE a.Status IN ('Admitted', 'Active')
                        ORDER BY a.AdmissionID DESC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable table = new DataTable();

                    adapter.Fill(table);

                    cmbAdmissionID.DataSource = null;
                    cmbAdmissionID.DisplayMember = "AdmissionID";
                    cmbAdmissionID.ValueMember = "AdmissionID";
                    cmbAdmissionID.DataSource = table;
                    cmbAdmissionID.SelectedIndex = -1;

                    txtPatientName.Clear();
                    txtBillID.Clear();
                    txtAmount.Clear();

                    selectedAdmissionID = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load Admission IDs:\n" + ex.Message,
                    "Database Error"
                );
            }
        }

        // WHEN ADMISSION ID IS SELECTED
        private void cmbAdmissionID_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            if (cmbAdmissionID.SelectedIndex == -1)
                return;

            DataRowView selected =
                cmbAdmissionID.SelectedItem as DataRowView;

            if (selected == null)
                return;

            selectedAdmissionID =
                Convert.ToInt32(selected["AdmissionID"]);

            txtPatientName.Text =
                Convert.ToString(selected["PatientName"]);

            LoadSelectedAdmissionBilling(selectedAdmissionID);
        }

        // LOAD PATIENT, EXISTING BILL NUMBER AND SERVICE TOTAL
        private void LoadSelectedAdmissionBilling(int admissionID)
        {
            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"
                        SELECT
                            ISNULL(SUM(s.TotalAmount), 0) AS ServiceTotal,
                            MAX(b.BillNo) AS BillNo,
                            MAX(b.TotalAmount) AS ExistingAmount
                        FROM Admission_Records a
                        LEFT JOIN Services_Charges s
                            ON a.AdmissionID = s.AdmissionID
                        LEFT JOIN Bill_Records b
                            ON a.AdmissionID = b.AdmissionID
                        WHERE a.AdmissionID = @AdmissionID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@AdmissionID", SqlDbType.Int)
                            .Value = admissionID;

                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader["BillNo"] != DBNull.Value)
                                {
                                    txtBillID.Text =
                                        reader["BillNo"].ToString();

                                    txtAmount.Text =
                                        Convert.ToDecimal(reader["ExistingAmount"])
                                        .ToString("0.00");
                                }
                                else
                                {
                                    // New bill: show calculated service charges.
                                    // The actual BillNo is assigned when Generate Bill is clicked.
                                    txtBillID.Clear();

                                    txtAmount.Text =
                                        Convert.ToDecimal(reader["ServiceTotal"])
                                        .ToString("0.00");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load billing information:\n" + ex.Message,
                    "Database Error"
                );
            }
        }

        // LOAD BILLING RECORDS
        private void LoadBilling()
        {
            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"
                        SELECT
                            b.BillNo AS BillID,
                            b.AdmissionID,
                            b.PatientID,
                            p.PatientName,
                            b.TotalAmount AS Amount,
                            b.AmountPaid,
                            b.Balance,
                            b.BillStatus AS PaymentStatus
                        FROM Bill_Records b
                        INNER JOIN Patient_Records p
                            ON b.PatientID = p.PatientID
                        ORDER BY b.BillNo DESC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable table = new DataTable();

                    adapter.Fill(table);

                    dgvBilling.DataSource = table;
                    dgvBilling.AutoSizeColumnsMode =
                        DataGridViewAutoSizeColumnsMode.Fill;

                    dgvBilling.SelectionMode =
                        DataGridViewSelectionMode.FullRowSelect;

                    dgvBilling.MultiSelect = false;
                    dgvBilling.ReadOnly = true;
                    dgvBilling.AllowUserToAddRows = false;

                    if (dgvBilling.Columns["PatientID"] != null)
                        dgvBilling.Columns["PatientID"].Visible = false;

                    if (dgvBilling.Columns["AdmissionID"] != null)
                        dgvBilling.Columns["AdmissionID"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load billing records:\n" + ex.Message,
                    "Database Error"
                );
            }
        }

        // GENERATE BILL
        private void btnGenerateBill_Click(object sender, EventArgs e)
        {
            if (selectedAdmissionID == 0)
            {
                MessageBox.Show("Please select an Admission ID.");
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                MessageBox.Show("Please enter a valid bill amount.");
                txtAmount.Focus();
                return;
            }

            if (amount < 0)
            {
                MessageBox.Show("Amount cannot be negative.");
                return;
            }

            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    con.Open();

                    using (SqlTransaction transaction = con.BeginTransaction())
                    {
                        try
                        {
                            string query = @"
                                DECLARE @PatientID INT;
                                DECLARE @BillNo INT;

                                SELECT @PatientID = PatientID
                                FROM Admission_Records
                                WHERE AdmissionID = @AdmissionID
                                  AND Status IN ('Admitted', 'Active');

                                IF @PatientID IS NULL
                                    THROW 50001, 'Admission not found or is not active.', 1;

                                IF EXISTS (
                                    SELECT 1
                                    FROM Bill_Records
                                    WHERE AdmissionID = @AdmissionID
                                )
                                BEGIN
                                    SELECT @BillNo = BillNo
                                    FROM Bill_Records
                                    WHERE AdmissionID = @AdmissionID;

                                    UPDATE Bill_Records
                                    SET TotalAmount = @Amount,
                                        Balance = @Amount - AmountPaid,
                                        BillStatus =
                                            CASE
                                                WHEN AmountPaid >= @Amount THEN 'Paid'
                                                WHEN AmountPaid > 0 THEN 'Partially Paid'
                                                ELSE 'Unpaid'
                                            END
                                    WHERE BillNo = @BillNo;
                                END
                                ELSE
                                BEGIN
                                    INSERT INTO Bill_Records
                                    (
                                        AdmissionID,
                                        PatientID,
                                        TotalAmount,
                                        AmountPaid,
                                        Balance,
                                        BillStatus
                                    )
                                    VALUES
                                    (
                                        @AdmissionID,
                                        @PatientID,
                                        @Amount,
                                        0,
                                        @Amount,
                                        'Unpaid'
                                    );

                                    SET @BillNo = SCOPE_IDENTITY();
                                END;

                                SELECT @BillNo AS BillNo;";

                            using (SqlCommand cmd =
                                new SqlCommand(query, con, transaction))
                            {
                                cmd.Parameters.Add("@AdmissionID", SqlDbType.Int)
                                    .Value = selectedAdmissionID;

                                cmd.Parameters.Add("@Amount", SqlDbType.Decimal)
                                    .Value = amount;

                                cmd.Parameters["@Amount"].Precision = 10;
                                cmd.Parameters["@Amount"].Scale = 2;

                                int billNo =
                                    Convert.ToInt32(cmd.ExecuteScalar());

                                txtBillID.Text = billNo.ToString();
                            }

                            transaction.Commit();

                            MessageBox.Show(
                                "Bill generated successfully!\nBill No: " +
                                txtBillID.Text,
                                "Success",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );

                            LoadBilling();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to generate bill:\n\n" + ex.Message,
                    "Billing Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // UPDATE BILL AMOUNT AND PAYMENT STATUS
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtBillID.Text, out int billID))
            {
                MessageBox.Show("Please select or generate a bill first.");
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount) ||
                amount < 0)
            {
                MessageBox.Show("Please enter a valid amount.");
                return;
            }

            if (cmbPaymentStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a payment status.");
                return;
            }

            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"
                        UPDATE Bill_Records
                        SET TotalAmount = @Amount,
                            Balance = @Amount - AmountPaid,
                            BillStatus = @Status
                        WHERE BillNo = @BillNo
                          AND @Amount >= AmountPaid";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal)
                            .Value = amount;

                        cmd.Parameters["@Amount"].Precision = 10;
                        cmd.Parameters["@Amount"].Scale = 2;

                        cmd.Parameters.Add("@Status", SqlDbType.VarChar, 30)
                            .Value = cmbPaymentStatus.Text;

                        cmd.Parameters.Add("@BillNo", SqlDbType.Int)
                            .Value = billID;

                        con.Open();

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Billing record updated successfully!");
                            LoadBilling();
                        }
                        else
                        {
                            MessageBox.Show(
                                "Update failed. The amount cannot be less than the amount already paid."
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Update Error:\n" + ex.Message,
                    "Database Error"
                );
            }
        }

        // DELETE BILL
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
                        "DELETE FROM Bill_Records WHERE BillNo = @BillNo";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@BillNo", SqlDbType.Int)
                            .Value = billID;

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Billing record deleted successfully!");

                ClearFields();
                LoadBilling();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to delete bill. It may have payment records.\n\n" +
                    ex.Message,
                    "Delete Error"
                );
            }
        }

        // REFRESH
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAdmissions();
            LoadBilling();
        }

        // SELECT BILL FROM DATAGRIDVIEW
        private void dgvBilling_CellClick(
            object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvBilling.Rows[e.RowIndex];

            txtBillID.Text =
                Convert.ToString(row.Cells["BillID"].Value);

            txtPatientName.Text =
                Convert.ToString(row.Cells["PatientName"].Value);

            txtAmount.Text =
                Convert.ToDecimal(row.Cells["Amount"].Value)
                .ToString("0.00");

            cmbPaymentStatus.Text =
                Convert.ToString(row.Cells["PaymentStatus"].Value);

            selectedAdmissionID =
                Convert.ToInt32(row.Cells["AdmissionID"].Value);

            cmbAdmissionID.SelectedValue = selectedAdmissionID;
        }

        // CLEAR FIELDS
        private void ClearFields()
        {
            txtBillID.Clear();
            txtPatientName.Clear();
            txtAmount.Clear();

            cmbAdmissionID.SelectedIndex = -1;
            cmbPaymentStatus.SelectedIndex = -1;

            selectedAdmissionID = 0;
        }

        private void BIllingStaff_Load(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
