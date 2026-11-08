
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HospitalLogIn
{
    public partial class Admin_Dashboard : Form
    {
        public Admin_Dashboard()
        {
            InitializeComponent();

            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnRefresh.Click += btnRefresh_Click;
            dgvPatients.CellClick += dgvPatients_CellClick;

            this.Shown += Admin_Dashboard_Shown;
        }

        private void Admin_Dashboard_Shown(object sender, EventArgs e)
        {
            LoadPatients();
        }

        private void LoadPatients()
        {
            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = "SELECT * FROM Patient_Records";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    dgvPatients.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load patient records.\n" + ex.Message,
                    "Database Error"
                );
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtPatientID.Text, out int id) ||
                !int.TryParse(txtAge.Text, out int age) ||
                string.IsNullOrWhiteSpace(txtPatientName.Text))
            {
                MessageBox.Show("Please enter valid patient information.");
                return;
            }

            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"INSERT INTO Patient_Records
                    (PatientID, PatientName, Age, Gender, Status)
                    VALUES
                    (@id, @name, @age, @gender, @status)";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", txtPatientName.Text);
                    cmd.Parameters.AddWithValue("@age", age);
                    cmd.Parameters.AddWithValue("@gender", cmbGender.Text);
                    cmd.Parameters.AddWithValue("@status", cmbStatus.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Patient added successfully!");

                LoadPatients();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add Patient Error: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtPatientID.Text, out int id) ||
                !int.TryParse(txtAge.Text, out int age))
            {
                MessageBox.Show("Please select a valid patient.");
                return;
            }

            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"UPDATE Patient_Records SET
                    PatientName = @name,
                    Age = @age,
                    Gender = @gender,
                    Status = @status
                    WHERE PatientID = @id";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", txtPatientName.Text);
                    cmd.Parameters.AddWithValue("@age", age);
                    cmd.Parameters.AddWithValue("@gender", cmbGender.Text);
                    cmd.Parameters.AddWithValue("@status", cmbStatus.Text);

                    con.Open();

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                        MessageBox.Show("Patient updated successfully!");
                    else
                        MessageBox.Show("Patient ID not found.");
                }

                LoadPatients();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update Error: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtPatientID.Text, out int id))
            {
                MessageBox.Show("Please select a valid patient.");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to delete this patient?",
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
                    string query = "DELETE FROM Patient_Records WHERE PatientID = @id";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", id);

                    con.Open();

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                        MessageBox.Show("Patient deleted successfully!");
                    else
                        MessageBox.Show("Patient ID not found.");
                }

                LoadPatients();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete Error: " + ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPatients();
            ClearFields();
        }

        private void dgvPatients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvPatients.Rows[e.RowIndex];

            txtPatientID.Text = Convert.ToString(row.Cells["PatientID"].Value);
            txtPatientName.Text = Convert.ToString(row.Cells["PatientName"].Value);
            txtAge.Text = Convert.ToString(row.Cells["Age"].Value);
            cmbGender.Text = Convert.ToString(row.Cells["Gender"].Value);
            cmbStatus.Text = Convert.ToString(row.Cells["Status"].Value);
        }

        private void ClearFields()
        {
            txtPatientID.Clear();
            txtPatientName.Clear();
            txtAge.Clear();
            cmbGender.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;
        }

    }

}

