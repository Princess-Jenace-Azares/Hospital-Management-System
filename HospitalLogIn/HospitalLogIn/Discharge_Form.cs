
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HospitalLogIn
{
    public partial class Discharge_Form : Form
    {
        private bool isLoadingAdmissions = false;

        public Discharge_Form()
        {
            InitializeComponent();

            cmbAdmissionID.DropDownStyle = ComboBoxStyle.DropDownList;

           
            btnDischarge.Click += btnDischarge_Click;
            btnRefresh.Click += btnRefresh_Click;

            cmbAdmissionID.SelectedIndexChanged += cmbAdmissionID_SelectedIndexChanged;

            this.Shown += Discharge_Form_Shown;
        }

        private void Discharge_Form_Shown(object sender, EventArgs e)
        {
            LoadAdmissions();
            LoadDischargedPatients();
        }

        private void LoadAdmissions()
        {
            try
            {
                isLoadingAdmissions = true;

                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"
                        SELECT
                            a.AdmissionID,
                            CAST(a.AdmissionID AS VARCHAR(20))
                            + ' - ' + p.PatientName AS AdmissionInfo
                        FROM Admission_Records a
                        INNER JOIN Patient_Records p
                            ON a.PatientID = p.PatientID
                        WHERE a.Status = 'Admitted'
                        ORDER BY a.AdmissionID DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbAdmissionID.DataSource = null;

                    cmbAdmissionID.DisplayMember = "AdmissionInfo";
                    cmbAdmissionID.ValueMember = "AdmissionID";
                    cmbAdmissionID.DataSource = dt;

                    cmbAdmissionID.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load admissions:\n" + ex.Message,
                    "Database Error"
                );
            }
            finally
            {
                isLoadingAdmissions = false;
            }
        }

        private void cmbAdmissionID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoadingAdmissions)
                return;

            if (cmbAdmissionID.SelectedIndex == -1)
            {
                ClearFields();
                return;
            }

            LoadAdmissionDetails(false);
        }

        private void btnCheckBill_Click(object sender, EventArgs e)
        {
            LoadAdmissionDetails(true);
        }

        private void LoadAdmissionDetails(bool showMessage)
        {
            if (cmbAdmissionID.SelectedIndex == -1 ||
                cmbAdmissionID.SelectedValue == null ||
                cmbAdmissionID.SelectedValue is DataRowView)
            {
                if (showMessage)
                {
                    MessageBox.Show("Please select an admission.");
                }

                return;
            }

            try
            {
                int admissionID = Convert.ToInt32(
                    cmbAdmissionID.SelectedValue
                );

                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"
                        SELECT
                            a.PatientID,
                            p.PatientName,
                            b.TotalAmount,
                            b.Balance
                        FROM Admission_Records a
                        INNER JOIN Patient_Records p
                            ON a.PatientID = p.PatientID
                        LEFT JOIN Bill_Records b
                            ON a.AdmissionID = b.AdmissionID
                        WHERE a.AdmissionID = @id
                          AND a.Status = 'Admitted'";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value =
                            admissionID;

                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                ClearFields();

                                if (showMessage)
                                {
                                    MessageBox.Show("Admission not found.");
                                }

                                return;
                            }

                            txtPatientID.Text =
                                Convert.ToString(reader["PatientID"]);

                            txtPatientName.Text =
                                Convert.ToString(reader["PatientName"]);

                            if (reader["TotalAmount"] == DBNull.Value)
                            {
                                txtTotalBill.Text = "0.00";
                                txtBalance.Text = "No bill";
                            }
                            else
                            {
                                decimal totalBill =
                                    Convert.ToDecimal(reader["TotalAmount"]);

                                txtTotalBill.Text =
                                    totalBill.ToString("0.00");

                                if (reader["Balance"] == DBNull.Value)
                                {
                                    txtBalance.Text = "0.00";
                                }
                                else
                                {
                                    decimal balance =
                                        Convert.ToDecimal(reader["Balance"]);

                                    txtBalance.Text =
                                        balance.ToString("0.00");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to retrieve admission information:\n" + ex.Message,
                    "Database Error"
                );
            }
        }

        private void btnDischarge_Click(object sender, EventArgs e)
        {
            if (cmbAdmissionID.SelectedIndex == -1 ||
                cmbAdmissionID.SelectedValue == null ||
                cmbAdmissionID.SelectedValue is DataRowView)
            {
                MessageBox.Show("Please select an admission.");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to discharge this patient?",
                "Confirm Discharge",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                int admissionID = Convert.ToInt32(
                    cmbAdmissionID.SelectedValue
                );

                using (SqlConnection con = DatabaseConnection.GetConnection())
                using (SqlCommand cmd = new SqlCommand(
                    "DischargePatient", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@AdmissionID", SqlDbType.Int).Value =
                        admissionID;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show(
                    "Patient discharged successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadAdmissions();
                LoadDischargedPatients();
                ClearFields();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Discharge Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void LoadDischargedPatients()
        {
            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"
                        SELECT
                            d.DischargeID,
                            d.AdmissionID,
                            d.PatientID,
                            p.PatientName,
                            d.DischargeDate,
                            d.FinalBill,
                            d.DischargeStatus
                        FROM Discharge_Records d
                        INNER JOIN Patient_Records p
                            ON d.PatientID = p.PatientID
                        ORDER BY d.DischargeID DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvDischargedPatients.DataSource = dt;

                    dgvDischargedPatients.AutoSizeColumnsMode =
                        DataGridViewAutoSizeColumnsMode.Fill;

                    dgvDischargedPatients.SelectionMode =
                        DataGridViewSelectionMode.FullRowSelect;

                    dgvDischargedPatients.ReadOnly = true;
                    dgvDischargedPatients.AllowUserToAddRows = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load discharge records:\n" + ex.Message,
                    "Database Error"
                );
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAdmissions();
            LoadDischargedPatients();
            ClearFields();
        }

        private void ClearFields()
        {
            txtPatientID.Clear();
            txtPatientName.Clear();
            txtTotalBill.Clear();
            txtBalance.Clear();
        }

        private void Discharge_Form_Load(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void dgvDischargedPatients_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
