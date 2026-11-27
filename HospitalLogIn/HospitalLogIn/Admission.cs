using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HospitalLogIn
{
    public partial class Attendant__Nurse_Dashboard : Form
    {

        public Attendant__Nurse_Dashboard()
        {
            InitializeComponent();

            cmbRoom.Items.Clear();
            cmbRoom.Items.AddRange(new object[]
            {
    "Room 101",
    "Room 102",
    "Room 103",
    "Room 104",
    "Room 105"
            });

            cmbStatus.Items.Clear();
            cmbStatus.Items.AddRange(new object[]
            {
    "Admitted",
    "Discharged",
    "Cancelled"
            });


            cmbPatientName.SelectedIndexChanged +=
    cmbPatientName_SelectedIndexChanged;

            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnRefresh.Click += btnRefresh_Click;
            dgvAdmission.CellClick += dgvAdmissions_CellClick;



            this.Shown += Attendant__Nurse_Dashboard_Shown;
        }

        private void Attendant__Nurse_Dashboard_Shown(object sender, EventArgs e)
        {
            LoadPatientNames();
            LoadAdmissions();
        }


        private void LoadPatientNames()
        {
            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"
                SELECT PatientID, PatientName
                FROM Patient_Records
                ORDER BY PatientName";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbPatientName.DataSource = dt;
                    cmbPatientName.DisplayMember = "PatientName";
                    cmbPatientName.ValueMember = "PatientID";
                    cmbPatientName.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load patients: " + ex.Message);
            }
        }

        private void LoadAdmissions()
        {
            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = "SELECT * FROM Admission_Records";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    dgvAdmission.DataSource = null;
                    dgvAdmission.AutoGenerateColumns = true;
                    dgvAdmission.DataSource = dt;
                    dgvAdmission.AutoSizeColumnsMode =
                        DataGridViewAutoSizeColumnsMode.Fill;
                    dgvAdmission.ReadOnly = true;
                    dgvAdmission.AllowUserToAddRows = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load admission records.\n" + ex.Message,
                    "Database Error"
                );
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbPatientName.SelectedValue == null ||
                !int.TryParse(
                    Convert.ToString(cmbPatientName.SelectedValue),
                    out int patientID) ||
                string.IsNullOrWhiteSpace(cmbRoom.Text) ||
                string.IsNullOrWhiteSpace(cmbStatus.Text))
            {
                MessageBox.Show("Please complete all admission information.");
                return;
            }

            if (dtpAdmissionDate.Value.Date > DateTime.Today)
            {
                MessageBox.Show("Admission date cannot be in the future.");
                return;
            }

            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string check = @"
                SELECT COUNT(*)
                FROM Admission_Records
                WHERE PatientID = @id
                  AND Status = 'Admitted'";

                    con.Open();

                    using (SqlCommand checkCmd = new SqlCommand(check, con))
                    {
                        checkCmd.Parameters.Add("@id", SqlDbType.Int).Value = patientID;

                        if ((int)checkCmd.ExecuteScalar() > 0)
                        {
                            MessageBox.Show(
                                "This patient already has an active admission.");
                            return;
                        }
                    }

                    string query = @"
                INSERT INTO Admission_Records
                (PatientID, AdmissionDate, PatientName, Room, Status)
                SELECT @id, @date, PatientName, @room, 'Admitted'
                FROM Patient_Records
                WHERE PatientID = @id";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = patientID;
                        cmd.Parameters.Add("@date", SqlDbType.Date).Value =
                            dtpAdmissionDate.Value.Date;
                        cmd.Parameters.Add("@room", SqlDbType.VarChar, 50).Value =
                            cmbRoom.Text;

                        int result = cmd.ExecuteNonQuery();

                        if (result == 0)
                        {
                            MessageBox.Show("Patient record not found.");
                            return;
                        }
                    }
                }

                MessageBox.Show("Admission added successfully!");
                LoadAdmissions();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Admission Error: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtPatientID.Text, out int id))
            {
                MessageBox.Show("Enter a valid Patient ID.");
                return;
            }

            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"UPDATE Admission_Records SET
                    AdmissionDate = @date,
                    PatientName = @name,
                    Room = @room,
                    Status = @status
                    WHERE PatientID = @id";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@date", SqlDbType.Date).Value = dtpAdmissionDate.Value.Date;
                    cmd.Parameters.Add("@name", SqlDbType.VarChar, 100).Value = cmbPatientName.Text;
                    cmd.Parameters.Add("@room", SqlDbType.VarChar, 50).Value = cmbRoom.Text;
                    cmd.Parameters.Add("@status", SqlDbType.VarChar, 30).Value = cmbStatus.Text;

                    con.Open();

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                        MessageBox.Show("Admission updated successfully!");
                    else
                        MessageBox.Show("No admission record found for this Patient ID.");
                }

                LoadAdmissions();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update Admission Error: " + ex.Message);
            }
        }



        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtAdmissionID.Text, out int admissionID))
            {
                MessageBox.Show("Please select an admission record first.");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "WARNING: This will permanently delete the admission " +
                "and ALL related services, bills, payments, and discharge records.\n\n" +
                "Are you sure you want to continue?",
                "Confirm Permanent Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    con.Open();

                    using (SqlTransaction tx = con.BeginTransaction())
                    {
                        try
                        {
                            // 1. Delete payments connected to bills
                            string deletePayments = @"
                        DELETE FROM Payment_Records
                        WHERE BillNo IN
                        (
                            SELECT BillNo
                            FROM Bill_Records
                            WHERE AdmissionID = @id
                        )";

                            using (SqlCommand cmd = new SqlCommand(deletePayments, con, tx))
                            {
                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = admissionID;
                                cmd.ExecuteNonQuery();
                            }

                            // 2. Delete billing records
                            string deleteBills = @"
                        DELETE FROM Bill_Records
                        WHERE AdmissionID = @id";

                            using (SqlCommand cmd = new SqlCommand(deleteBills, con, tx))
                            {
                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = admissionID;
                                cmd.ExecuteNonQuery();
                            }

                            // 3. Delete services and charges
                            string deleteServices = @"
                        DELETE FROM Services_Charges
                        WHERE AdmissionID = @id";

                            using (SqlCommand cmd = new SqlCommand(deleteServices, con, tx))
                            {
                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = admissionID;
                                cmd.ExecuteNonQuery();
                            }

                            // 4. Delete discharge records
                            string deleteDischarge = @"
                        DELETE FROM Discharge_Records
                        WHERE AdmissionID = @id";

                            using (SqlCommand cmd = new SqlCommand(deleteDischarge, con, tx))
                            {
                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = admissionID;
                                cmd.ExecuteNonQuery();
                            }

                            // 5. Delete admission record
                            string deleteAdmission = @"
                        DELETE FROM Admission_Records
                        WHERE AdmissionID = @id";

                            using (SqlCommand cmd = new SqlCommand(deleteAdmission, con, tx))
                            {
                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = admissionID;

                                int result = cmd.ExecuteNonQuery();

                                if (result == 0)
                                    throw new Exception("Admission record was not found.");
                            }

                            tx.Commit();
                        }
                        catch
                        {
                            tx.Rollback();
                            throw;
                        }
                    }
                }

                MessageBox.Show(
                    "Admission and all its related records were permanently deleted!",
                    "Deletion Successful",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadAdmissions();
                LoadPatientNames();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to delete admission:\n\n" + ex.Message,
                    "Delete Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPatientNames();
            LoadAdmissions();
            ClearFields();
        }


        private void dgvAdmissions_CellClick(
            object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvAdmission.Rows[e.RowIndex];

            txtAdmissionID.Text =
                Convert.ToString(row.Cells["AdmissionID"].Value);

            txtPatientID.Text =
                Convert.ToString(row.Cells["PatientID"].Value);

            if (row.Cells["AdmissionDate"].Value != DBNull.Value)
            {
                dtpAdmissionDate.Value =
                    Convert.ToDateTime(row.Cells["AdmissionDate"].Value);
            }

            cmbPatientName.Text =
                Convert.ToString(row.Cells["PatientName"].Value);

            cmbRoom.Text =
                Convert.ToString(row.Cells["Room"].Value);

            cmbStatus.Text =
                Convert.ToString(row.Cells["Status"].Value);
        }

        private void ClearFields()
        {
            txtPatientID.Clear();
            cmbPatientName.SelectedIndex = -1;
            cmbRoom.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;
            dtpAdmissionDate.Value = DateTime.Now;
        }

        private void cmbPatientName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPatientName.SelectedIndex >= 0 &&
                cmbPatientName.SelectedValue != null)
            {
                txtPatientID.Text = cmbPatientName.SelectedValue.ToString();
            }
        }

        private void Attendant__Nurse_Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
