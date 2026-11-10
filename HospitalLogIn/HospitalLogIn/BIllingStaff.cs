
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
            txtPatientName.ReadOnly = true;
            txtAmount.ReadOnly = true;

            cmbPaymentStatus.Items.Clear();
            cmbPaymentStatus.Items.Add("Unpaid");
            cmbPaymentStatus.Items.Add("Partially Paid");
            cmbPaymentStatus.Items.Add("Paid");

            btnAdd.Click += btnGenerateBill_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnRefresh.Click += btnRefresh_Click;

            cmbAdmissionID.SelectedIndexChanged += cmbAdmissionID_SelectedIndexChanged;
            dgvBilling.CellClick += dgvBilling_CellClick;

            this.Shown += BIllingStaff_Shown;
        }

        private void BIllingStaff_Shown(object sender, EventArgs e)
        {
            LoadAdmissions();
            LoadBilling();
        }

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
                        WHERE a.Status = 'Active'";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable table = new DataTable();

                    adapter.Fill(table);

                    cmbAdmissionID.DataSource = null;
                    cmbAdmissionID.DataSource = table;
                    cmbAdmissionID.DisplayMember = "AdmissionID";
                    cmbAdmissionID.ValueMember = "AdmissionID";
                    cmbAdmissionID.SelectedIndex = -1;

                    txtPatientName.Clear();
                    txtAmount.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load admissions.\n" + ex.Message,
                    "Database Error"
                );
            }
        }

        private void cmbAdmissionID_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            if (cmbAdmissionID.SelectedIndex == -1 ||
                cmbAdmissionID.SelectedItem == null)
                return;

            DataRowView selected =
                cmbAdmissionID.SelectedItem as DataRowView;

            if (selected != null)
            {
                txtPatientName.Text =
                    Convert.ToString(selected["PatientName"]);

                txtAmount.Clear();
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

        private void btnGenerateBill_Click(object sender, EventArgs e)
        {
            if (cmbAdmissionID.SelectedValue == null ||
                string.IsNullOrWhiteSpace(txtPatientName.Text))
            {
                MessageBox.Show("Please select an active admission.");
                return;
            }

            GenerateBill();
        }

        private void GenerateBill()
        {
            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                using (SqlCommand cmd = new SqlCommand(
                    "GenerateBillFromAdmission", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@AdmissionID", SqlDbType.Int).Value =
                        Convert.ToInt32(cmbAdmissionID.SelectedValue);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtBillID.Text =
                                reader["BillNo"].ToString();

                            txtAmount.Text =
                                Convert.ToDecimal(reader["TotalAmount"])
                                .ToString("0.00");
                        }
                        else
                        {
                            MessageBox.Show(
                                "No bill was generated. Please check the admission."
                            );
                            return;
                        }
                    }
                }

                MessageBox.Show("Bill generated successfully!");

                LoadBilling();
                LoadAdmissions();
                ClearFields();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Billing Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Billing Error: " + ex.Message,
                    "Error"
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
                        SET BillStatus = @Status
                        WHERE BillNo = @BillID";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.Add("@Status", SqlDbType.VarChar, 30).Value =
                        cmbPaymentStatus.Text;

                    cmd.Parameters.Add("@BillID", SqlDbType.Int).Value =
                        billID;

                    con.Open();

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show(
                            "Payment status updated successfully!"
                        );
                    }
                    else
                    {
                        MessageBox.Show("Billing record not found.");
                    }
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
                    {
                        MessageBox.Show(
                            "Billing record deleted successfully!"
                        );
                    }
                    else
                    {
                        MessageBox.Show("Billing record not found.");
                    }
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
            LoadAdmissions();
            LoadBilling();
            ClearFields();
        }

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
        }

        private void ClearFields()
        {
            txtBillID.Clear();
            txtPatientName.Clear();
            txtAmount.Clear();

            cmbAdmissionID.SelectedIndex = -1;
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
