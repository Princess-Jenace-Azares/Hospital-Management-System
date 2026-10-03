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
    public partial class Admin_Dashboard : Form
    {

        private DatabaseConnection databaseConnection;

        public Admin_Dashboard()
        {
            InitializeComponent();

            cmbGender.Items.Add("Male");
            cmbGender.Items.Add("Female");

            cmbRoom.Items.Add("Room 101");
            cmbRoom.Items.Add("Room 102");
            cmbRoom.Items.Add("Room 103");
            cmbRoom.Items.Add("Room 104");

            cmbStatus.Items.Add("Admitted");
            cmbStatus.Items.Add("Discharged");
            cmbStatus.Items.Add("Pending");


            databaseConnection = new DatabaseConnection();

            SetupDashboard();

            LoadPatients();
        }

        private void ClearPatientFields()
        {
            txtPatientID.Clear();
            txtPatientName.Clear();
            txtAge.Clear();

            cmbGender.SelectedIndex = -1;
            cmbRoom.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;

            dtpAdmissionDate.Value = DateTime.Today;
        }
        private void LoadPatients()
        {
            try
            {
                using (SqlConnection connection =
                    databaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                SELECT
                    PatientID,
                    PatientName,
                    Age,
                    Gender,
                    Room,
                    AdmissionDate,
                    Status
                FROM Patients
                ORDER BY PatientID";

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(query, connection))
                    {
                        DataTable table = new DataTable();

                        adapter.Fill(table);

                        dgvPatients.DataSource = table;
                    }
                }

                dgvPatients.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;

                dgvPatients.SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect;

                dgvPatients.MultiSelect = false;

                dgvPatients.ReadOnly = true;

                dgvPatients.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading patients:\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        private void SetupDashboard()
        {


            this.Text = "Admin Dashboard";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;

            // Dashboard title
            Label title = new Label();

            title.Text = "ADMIN DASHBOARD";
            title.Font = new Font(
                "Segoe UI",
                28,
                FontStyle.Bold
            );

            title.AutoSize = true;
            title.Location = new Point(50, 40);

            this.Controls.Add(title);
        }

        private void Admin_Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnRefreshPatients_Click(object sender, EventArgs e)
        {
            LoadPatients();
        }

        private void btnAddPatient_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtPatientID.Text) ||
                    string.IsNullOrWhiteSpace(txtPatientName.Text) ||
                    string.IsNullOrWhiteSpace(txtAge.Text) ||
                    cmbGender.SelectedIndex == -1 ||
                    cmbRoom.SelectedIndex == -1 ||
                    cmbStatus.SelectedIndex == -1)
                {
                    MessageBox.Show(
                        "Please complete all patient information.",
                        "Missing Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }

                using (SqlConnection connection =
                    databaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                INSERT INTO Patients
                (
                    PatientID,
                    PatientName,
                    Age,
                    Gender,
                    Room,
                    AdmissionDate,
                    Status
                )
                VALUES
                (
                    @PatientID,
                    @PatientName,
                    @Age,
                    @Gender,
                    @Room,
                    @AdmissionDate,
                    @Status
                )";

                    using (SqlCommand command =
                        new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@PatientID",
                            int.Parse(txtPatientID.Text)
                        );

                        command.Parameters.AddWithValue(
                            "@PatientName",
                            txtPatientName.Text
                        );

                        command.Parameters.AddWithValue(
                            "@Age",
                            int.Parse(txtAge.Text)
                        );

                        command.Parameters.AddWithValue(
                            "@Gender",
                            cmbGender.Text
                        );

                        command.Parameters.AddWithValue(
                            "@Room",
                            cmbRoom.Text
                        );

                        command.Parameters.AddWithValue(
                            "@AdmissionDate",
                            dtpAdmissionDate.Value.Date
                        );

                        command.Parameters.AddWithValue(
                            "@Status",
                            cmbStatus.Text
                        );

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Patient added successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadPatients();
                ClearPatientFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error adding patient:\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void dgvPatients_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row =
                    dgvPatients.Rows[e.RowIndex];

                txtPatientID.Text =
                    row.Cells["PatientID"].Value.ToString();

                txtPatientName.Text =
                    row.Cells["PatientName"].Value.ToString();

                txtAge.Text =
                    row.Cells["Age"].Value.ToString();

                cmbGender.Text =
                    row.Cells["Gender"].Value.ToString();

                cmbRoom.Text =
                    row.Cells["Room"].Value.ToString();

                dtpAdmissionDate.Value =
                    Convert.ToDateTime(
                        row.Cells["AdmissionDate"].Value
                    );

                cmbStatus.Text =
                    row.Cells["Status"].Value.ToString();
            }
        }

        private void btnUpdatePatient_Click(object sender, EventArgs e)
        {
         
            try
            {
                using (SqlConnection connection =
                    databaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                UPDATE Patients
                SET
                    PatientName = @PatientName,
                    Age = @Age,
                    Gender = @Gender,
                    Room = @Room,
                    AdmissionDate = @AdmissionDate,
                    Status = @Status
                WHERE PatientID = @PatientID";

                    using (SqlCommand command =
                        new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@PatientID",
                            int.Parse(txtPatientID.Text)
                        );

                        command.Parameters.AddWithValue(
                            "@PatientName",
                            txtPatientName.Text
                        );

                        command.Parameters.AddWithValue(
                            "@Age",
                            int.Parse(txtAge.Text)
                        );

                        command.Parameters.AddWithValue(
                            "@Gender",
                            cmbGender.Text
                        );

                        command.Parameters.AddWithValue(
                            "@Room",
                            cmbRoom.Text
                        );

                        command.Parameters.AddWithValue(
                            "@AdmissionDate",
                            dtpAdmissionDate.Value.Date
                        );

                        command.Parameters.AddWithValue(
                            "@Status",
                            cmbStatus.Text
                        );

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Patient updated successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadPatients();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error updating patient:\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnDeletePatient_Click(object sender, EventArgs e)
        {
           
            if (string.IsNullOrWhiteSpace(txtPatientID.Text))
            {
                MessageBox.Show(
                    "Please select a patient first.",
                    "Delete Patient",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this patient?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection connection =
                    databaseConnection.GetConnection())
                {
                    connection.Open();

                    string query =
                        "DELETE FROM Patients WHERE PatientID = @PatientID";

                    using (SqlCommand command =
                        new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@PatientID",
                            int.Parse(txtPatientID.Text)
                        );

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Patient deleted successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadPatients();
                ClearPatientFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to delete patient.\n\n" +
                    "The patient may have existing check-in or billing records.\n\n" +
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
    }
    
    
    

