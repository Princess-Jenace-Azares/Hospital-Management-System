
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HospitalLogIn
{
    public partial class Discharge_Form : Form
    {
        public Discharge_Form()
        {
            InitializeComponent();

            btnCheckBill.Click += btnCheckBill_Click;
            btnDischarge.Click += btnDischarge_Click;
            btnRefresh.Click += btnRefresh_Click;

            this.Shown += Discharge_Form_Shown;
        }

        private void Discharge_Form_Shown(object sender, EventArgs e)
        {
            LoadAdmissions();
            LoadDischargedPatients();
        }

        private void LoadAdmissions()
        {
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

                cmbAdmissionID.DataSource = dt;
                cmbAdmissionID.DisplayMember = "AdmissionInfo";
                cmbAdmissionID.ValueMember = "AdmissionID";
                cmbAdmissionID.SelectedIndex = -1;
            }
        }

        private void btnCheckBill_Click(object sender, EventArgs e)
        {
            if (cmbAdmissionID.SelectedValue == null)
            {
                MessageBox.Show("Please select an admission.");
                return;
            }

            try
            {
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

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.Add("@id", SqlDbType.Int).Value =
                        Convert.ToInt32(cmbAdmissionID.SelectedValue);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            MessageBox.Show("Admission not found.");
                            return;
                        }

                        txtPatientID.Text =
                            reader["PatientID"].ToString();

                        txtPatientName.Text =
                            reader["PatientName"].ToString();

                        if (reader["Balance"] == DBNull.Value)
                        {
                            txtTotalBill.Text = "0.00";
                            txtBalance.Text = "No bill";
                        }
                        else
                        {
                            txtTotalBill.Text =
                                Convert.ToDecimal(reader["TotalAmount"])
                                .ToString("0.00");

                            txtBalance.Text =
                                Convert.ToDecimal(reader["Balance"])
                                .ToString("0.00");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Check Bill Error: " + ex.Message);
            }
        }

        private void btnDischarge_Click(object sender, EventArgs e)
        {
            if (cmbAdmissionID.SelectedValue == null)
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
                using (SqlConnection con = DatabaseConnection.GetConnection())
                using (SqlCommand cmd = new SqlCommand(
                    "DischargePatient", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@AdmissionID", SqlDbType.Int).Value =
                        Convert.ToInt32(cmbAdmissionID.SelectedValue);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Patient discharged successfully!");

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
        }

        private void LoadDischargedPatients()
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
            cmbAdmissionID.SelectedIndex = -1;
            txtPatientID.Clear();
            txtPatientName.Clear();
            txtTotalBill.Clear();
            txtBalance.Clear();
        }
    }
}
