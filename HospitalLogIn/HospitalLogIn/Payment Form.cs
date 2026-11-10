
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HospitalLogIn
{
    public partial class Payment_Form : Form
    {
        public Payment_Form()
        {
            InitializeComponent();

            txtPatientName.ReadOnly = true;
            txtTotalBill.ReadOnly = true;
            txtAlreadyPaid.ReadOnly = true;
            txtBalance.ReadOnly = true;

            btnCheckBill.Click += btnCheckBill_Click;
            btnProcessPayment.Click += btnProcessPayment_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnRefresh.Click += btnRefresh_Click;

            dgvPayments.CellClick += dgvPayments_CellClick;

            cmbPaymentMethod.Items.Clear();

            cmbPaymentMethod.Items.AddRange(new object[]
            {
                "Cash",
                "Credit Card",
                "Debit Card",
                "Bank Transfer",
                "GCash"
            });

            this.Shown += Payment_Form_Shown;
        }

        private void Payment_Form_Shown(object sender, EventArgs e)
        {
            LoadPayments();
        }

        private void LoadPayments()
        {
            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"
                        SELECT
                            PaymentID,
                            BillNo,
                            AmountPaid,
                            PaymentMethod,
                            PaymentDate
                        FROM Payment_Records
                        ORDER BY PaymentID DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvPayments.DataSource = dt;

                    dgvPayments.AutoSizeColumnsMode =
                        DataGridViewAutoSizeColumnsMode.Fill;

                    dgvPayments.SelectionMode =
                        DataGridViewSelectionMode.FullRowSelect;

                    dgvPayments.ReadOnly = true;
                    dgvPayments.AllowUserToAddRows = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load payment records.\n" + ex.Message,
                    "Database Error"
                );
            }
        }

        private void btnCheckBill_Click(object sender, EventArgs e)
        {
            CheckBill();
        }

        private void CheckBill()
        {
            if (!int.TryParse(txtBillNo.Text, out int billNo))
            {
                MessageBox.Show("Enter a valid Bill Number.");
                return;
            }

            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"
                        SELECT
                            p.PatientName,
                            b.TotalAmount,
                            b.AmountPaid,
                            b.Balance
                        FROM Bill_Records b
                        INNER JOIN Patient_Records p
                            ON b.PatientID = p.PatientID
                        WHERE b.BillNo = @bill";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.Add("@bill", SqlDbType.Int).Value =
                        billNo;

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            MessageBox.Show("Bill number not found.");
                            ClearBillInformation();
                            return;
                        }

                        decimal total =
                            Convert.ToDecimal(reader["TotalAmount"]);

                        decimal paid =
                            reader["AmountPaid"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(reader["AmountPaid"]);

                        decimal balance =
                            reader["Balance"] == DBNull.Value
                            ? total - paid
                            : Convert.ToDecimal(reader["Balance"]);

                        txtPatientName.Text =
                            Convert.ToString(reader["PatientName"]);

                        txtTotalBill.Text =
                            total.ToString("0.00");

                        txtAlreadyPaid.Text =
                            paid.ToString("0.00");

                        txtBalance.Text =
                            balance.ToString("0.00");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to retrieve bill:\n" + ex.Message,
                    "Database Error"
                );
            }
        }

        private void btnProcessPayment_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtBillNo.Text, out int billNo) ||
                !decimal.TryParse(txtAmountPaid.Text, out decimal amount) ||
                amount <= 0 ||
                cmbPaymentMethod.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Please enter a valid Bill Number, Amount Paid, and Payment Method."
                );
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
                            decimal total;
                            decimal paid;

                            string checkQuery = @"
                                SELECT TotalAmount, AmountPaid
                                FROM Bill_Records WITH (UPDLOCK, HOLDLOCK)
                                WHERE BillNo = @bill";

                            using (SqlCommand check = new SqlCommand(
                                checkQuery, con, transaction))
                            {
                                check.Parameters.Add("@bill", SqlDbType.Int).Value =
                                    billNo;

                                using (SqlDataReader reader = check.ExecuteReader())
                                {
                                    if (!reader.Read())
                                    {
                                        throw new Exception("Bill Number not found.");
                                    }

                                    total = reader.GetDecimal(0);
                                    paid = reader.IsDBNull(1)
                                        ? 0
                                        : reader.GetDecimal(1);
                                }
                            }

                            decimal balance = total - paid;

                            if (balance <= 0)
                            {
                                throw new Exception(
                                    "This bill is already fully paid."
                                );
                            }

                            if (amount > balance)
                            {
                                throw new Exception(
                                    "Payment exceeds the remaining balance.\n" +
                                    "Remaining Balance: " +
                                    balance.ToString("0.00")
                                );
                            }

                            string insertQuery = @"
                                INSERT INTO Payment_Records
                                (BillNo, AmountPaid, PaymentMethod)
                                VALUES
                                (@bill, @amount, @method)";

                            using (SqlCommand insert = new SqlCommand(
                                insertQuery, con, transaction))
                            {
                                insert.Parameters.Add(
                                    "@bill", SqlDbType.Int).Value = billNo;

                                SqlParameter amountParam =
                                    insert.Parameters.Add(
                                        "@amount", SqlDbType.Decimal);

                                amountParam.Precision = 10;
                                amountParam.Scale = 2;
                                amountParam.Value = amount;

                                insert.Parameters.Add(
                                    "@method", SqlDbType.VarChar, 50).Value =
                                    cmbPaymentMethod.Text;

                                insert.ExecuteNonQuery();
                            }

                            string updateQuery = @"
                                UPDATE Bill_Records
                                SET
                                    AmountPaid = AmountPaid + @amount,
                                    Balance = TotalAmount - (AmountPaid + @amount),
                                    BillStatus = CASE
                                        WHEN AmountPaid + @amount >= TotalAmount
                                            THEN 'Paid'
                                        ELSE 'Partially Paid'
                                    END
                                WHERE BillNo = @bill";

                            using (SqlCommand update = new SqlCommand(
                                updateQuery, con, transaction))
                            {
                                SqlParameter amountParam =
                                    update.Parameters.Add(
                                        "@amount", SqlDbType.Decimal);

                                amountParam.Precision = 10;
                                amountParam.Scale = 2;
                                amountParam.Value = amount;

                                update.Parameters.Add(
                                    "@bill", SqlDbType.Int).Value = billNo;

                                update.ExecuteNonQuery();
                            }

                            transaction.Commit();

                            MessageBox.Show(
                                "Payment processed successfully!\n\n" +
                                "Bill Number: " + billNo +
                                "\nAmount Paid: " + amount.ToString("0.00") +
                                "\nRemaining Balance: " +
                                (balance - amount).ToString("0.00"),
                                "Payment Successful"
                            );
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                LoadPayments();
                CheckBill();
                txtAmountPaid.Clear();
                cmbPaymentMethod.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Payment Error:\n" + ex.Message,
                    "Payment Failed"
                );
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Payment editing is disabled to preserve payment history. " +
                "Use a controlled correction or refund process instead.",
                "Payment Information"
            );
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Payments cannot be deleted directly. " +
                "Use a controlled reversal process instead.",
                "Payment Information"
            );
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPayments();
            ClearFields();
        }

        private void dgvPayments_CellClick(
            object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvPayments.Rows[e.RowIndex];

            txtBillNo.Text =
                Convert.ToString(row.Cells["BillNo"].Value);

            txtAmountPaid.Clear();

            cmbPaymentMethod.SelectedIndex = -1;

            CheckBill();
        }

        private void ClearBillInformation()
        {
            txtPatientName.Clear();
            txtTotalBill.Clear();
            txtAlreadyPaid.Clear();
            txtBalance.Clear();
        }

        private void ClearFields()
        {
            txtBillNo.Clear();
            txtAmountPaid.Clear();

            cmbPaymentMethod.SelectedIndex = -1;

            ClearBillInformation();
        }

        private void Payment_Form_Load(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
