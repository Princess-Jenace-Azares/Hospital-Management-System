
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
                    string query = "SELECT PatientName FROM Patient_Records";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    cmbPatientName.DataSource = dt;
                    cmbPatientName.DisplayMember = "PatientName";
                    cmbPatientName.ValueMember = "PatientName";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load patient names.\n" + ex.Message,
                    "Database Error"
                );
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

                    dgvAdmission.DataSource = dt;
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
            if (!int.TryParse(txtPatientID.Text, out int id))
            {
                MessageBox.Show("Enter a valid Patient ID.");
                return;
            }

            if (cmbPatientName.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(cmbRoom.Text) ||
                string.IsNullOrWhiteSpace(cmbStatus.Text))
            {
                MessageBox.Show("Please complete all admission information.");
                return;
            }

            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"INSERT INTO Admission_Records
                    (PatientID, AdmissionDate, PatientName, Room, Status)
                    VALUES
                    (@id, @date, @name, @room, @status)";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@date", SqlDbType.Date).Value = dtpAdmissionDate.Value.Date;
                    cmd.Parameters.Add("@name", SqlDbType.VarChar, 100).Value = cmbPatientName.Text;
                    cmd.Parameters.Add("@room", SqlDbType.VarChar, 50).Value = cmbRoom.Text;
                    cmd.Parameters.Add("@status", SqlDbType.VarChar, 30).Value = cmbStatus.Text;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Admission added successfully!");

                LoadAdmissions();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add Admission Error: " + ex.Message);
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
            if (!int.TryParse(txtPatientID.Text, out int id))
            {
                MessageBox.Show("Enter a valid Patient ID.");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to delete this admission?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = "DELETE FROM Admission_Records WHERE PatientID = @id";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    con.Open();

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                        MessageBox.Show("Admission deleted successfully!");
                    else
                        MessageBox.Show("No admission record found.");
                }

                LoadAdmissions();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete Admission Error: " + ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPatientNames();
            LoadAdmissions();
            ClearFields();
        }

        private void dgvAdmissions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvAdmission.Rows[e.RowIndex];

            txtPatientID.Text = Convert.ToString(row.Cells["PatientID"].Value);

            if (row.Cells["AdmissionDate"].Value != DBNull.Value)
            {
                dtpAdmissionDate.Value =
                    Convert.ToDateTime(row.Cells["AdmissionDate"].Value);
            }

            cmbPatientName.Text = Convert.ToString(row.Cells["PatientName"].Value);
            cmbRoom.Text = Convert.ToString(row.Cells["Room"].Value);
            cmbStatus.Text = Convert.ToString(row.Cells["Status"].Value);
        }

        private void ClearFields()
        {
            txtPatientID.Clear();
            cmbPatientName.SelectedIndex = -1;
            cmbRoom.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;
            dtpAdmissionDate.Value = DateTime.Now;
        }

     
    }
}
