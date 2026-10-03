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
    public partial class Attendant__Nurse_Dashboard : Form
    {
        private DatabaseConnection databaseConnection;
        private int selectedRecordID = 0;

        private class PatientItem
        {
            public int PatientID { get; set; }
            public string PatientName { get; set; }

            public override string ToString()
            {
                return PatientName;
            }
        }
        public Attendant__Nurse_Dashboard()
        {
            InitializeComponent();

            databaseConnection = new DatabaseConnection();

            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("Checked-In");
            cmbStatus.Items.Add("Checked-Out");
            cmbStatus.Items.Add("Pending");
            cmbStatus.SelectedIndex = 0;

            cmbRoom.Items.Clear();
            cmbRoom.Items.Add("Room 101");
            cmbRoom.Items.Add("Room 102");
            cmbRoom.Items.Add("Room 103");
            cmbRoom.Items.Add("Room 104");
            cmbRoom.SelectedIndex = 0;

            btnAddCheckIn.Click += btnAddCheckIn_Click;
            btnUpdateCheckIn.Click += btnUpdateCheckIn_Click;
            btnDeleteCheckIn.Click += btnDeleteCheckIn_Click;
            btnRefreshCheckIn.Click += btnRefreshCheckIn_Click;

            cmbPatientName.SelectedIndexChanged +=
                cmbPatientName_SelectedIndexChanged;

            dgvCheckIn.CellClick += dgvCheckIn_CellClick;

            LoadPatientNames();
            LoadCheckInRecords();
        }

        private void LoadPatientNames()
        {
            try
            {
                using (SqlConnection connection =
                    databaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT PatientID, PatientName
                        FROM Patients
                        ORDER BY PatientName";

                    using (SqlCommand command =
                        new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader =
                            command.ExecuteReader())
                        {
                            cmbPatientName.Items.Clear();

                            while (reader.Read())
                            {
                                PatientItem patient = new PatientItem
                                {
                                    PatientID =
                                        Convert.ToInt32(
                                            reader["PatientID"]),

                                    PatientName =
                                        reader["PatientName"].ToString()
                                };

                                cmbPatientName.Items.Add(patient);
                            }
                        }
                    }
                }

                if (cmbPatientName.Items.Count > 0)
                {
                    cmbPatientName.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load patients.\n\n" +
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void cmbPatientName_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            if (cmbPatientName.SelectedItem
                is PatientItem patient)
            {
                txtPatientID.Text =
                    patient.PatientID.ToString();
            }
        }

        private void LoadCheckInRecords()
        {
            try
            {
                using (SqlConnection connection =
                    databaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT
                            C.RecordID,
                            C.PatientID,
                            P.PatientName,
                            C.Room,
                            C.CheckIn,
                            C.CheckOut,
                            C.Status
                        FROM PatientCheckInOut C
                        INNER JOIN Patients P
                            ON C.PatientID = P.PatientID
                        ORDER BY C.RecordID DESC";

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(
                            query,
                            connection))
                    {
                        DataTable table =
                            new DataTable();

                        adapter.Fill(table);

                        dgvCheckIn.DataSource =
                            table;
                    }
                }

                dgvCheckIn.ReadOnly = true;

                dgvCheckIn.AllowUserToAddRows = false;

                dgvCheckIn.SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect;

                dgvCheckIn.MultiSelect = false;

                dgvCheckIn.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load check-in records.\n\n" +
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnAddCheckIn_Click(
            object sender,
            EventArgs e)
        {
            if (!(cmbPatientName.SelectedItem
                is PatientItem patient))
            {
                MessageBox.Show(
                    "Please select a patient.",
                    "Check-In",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (cmbRoom.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select a room.",
                    "Check-In",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select a status.",
                    "Check-In",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DateTime? checkOut = null;

            if (cmbStatus.Text == "Checked-Out")
            {
                checkOut = dtpCheckOut.Value;
            }

            if (checkOut.HasValue &&
                checkOut.Value < dtpCheckIn.Value)
            {
                MessageBox.Show(
                    "Check-out cannot be earlier than check-in.",
                    "Invalid Date",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                using (SqlConnection connection =
                    databaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        INSERT INTO PatientCheckInOut
                        (
                            PatientID,
                            CheckIn,
                            CheckOut,
                            Room,
                            Status
                        )
                        VALUES
                        (
                            @PatientID,
                            @CheckIn,
                            @CheckOut,
                            @Room,
                            @Status
                        )";

                    using (SqlCommand command =
                        new SqlCommand(
                            query,
                            connection))
                    {
                        command.Parameters.Add(
                            "@PatientID",
                            SqlDbType.Int).Value =
                            patient.PatientID;

                        command.Parameters.Add(
                            "@CheckIn",
                            SqlDbType.DateTime).Value =
                            dtpCheckIn.Value;

                        command.Parameters.Add(
                            "@CheckOut",
                            SqlDbType.DateTime).Value =
                            checkOut.HasValue
                                ? (object)checkOut.Value
                                : DBNull.Value;

                        command.Parameters.Add(
                            "@Room",
                            SqlDbType.VarChar,
                            50).Value =
                            cmbRoom.Text;

                        command.Parameters.Add(
                            "@Status",
                            SqlDbType.VarChar,
                            30).Value =
                            cmbStatus.Text;

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Patient check-in added successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadCheckInRecords();
                ClearCheckInFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to add check-in record.\n\n" +
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dgvCheckIn_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            try
            {
                DataGridViewRow row =
                    dgvCheckIn.Rows[e.RowIndex];

                selectedRecordID =
                    Convert.ToInt32(
                        row.Cells["RecordID"].Value);

                int patientID =
                    Convert.ToInt32(
                        row.Cells["PatientID"].Value);

                txtPatientID.Text =
                    patientID.ToString();

                foreach (
                    PatientItem patient
                    in cmbPatientName.Items)
                {
                    if (patient.PatientID == patientID)
                    {
                        cmbPatientName.SelectedItem =
                            patient;

                        break;
                    }
                }

                cmbRoom.Text =
                    row.Cells["Room"].Value.ToString();

                cmbStatus.Text =
                    row.Cells["Status"].Value.ToString();

                if (row.Cells["CheckIn"].Value
                    != DBNull.Value)
                {
                    dtpCheckIn.Value =
                        Convert.ToDateTime(
                            row.Cells["CheckIn"].Value);
                }

                if (row.Cells["CheckOut"].Value
                    != DBNull.Value)
                {
                    dtpCheckOut.Value =
                        Convert.ToDateTime(
                            row.Cells["CheckOut"].Value);
                }
                else
                {
                    dtpCheckOut.Value =
                        DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to select record.\n\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnUpdateCheckIn_Click(
            object sender,
            EventArgs e)
        {
            if (selectedRecordID == 0)
            {
                MessageBox.Show(
                    "Please select a record from the table first.",
                    "Check-In",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (!(cmbPatientName.SelectedItem
                is PatientItem patient))
            {
                MessageBox.Show(
                    "Please select a patient.",
                    "Check-In",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (cmbRoom.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select a room.",
                    "Check-In",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select a status.",
                    "Check-In",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DateTime? checkOut = null;

            if (cmbStatus.Text == "Checked-Out")
            {
                checkOut = dtpCheckOut.Value;
            }

            if (checkOut.HasValue &&
                checkOut.Value < dtpCheckIn.Value)
            {
                MessageBox.Show(
                    "Check-out cannot be earlier than check-in.",
                    "Invalid Date",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                using (SqlConnection connection =
                    databaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        UPDATE PatientCheckInOut
                        SET
                            PatientID = @PatientID,
                            Room = @Room,
                            CheckIn = @CheckIn,
                            CheckOut = @CheckOut,
                            Status = @Status
                        WHERE RecordID = @RecordID";

                    using (SqlCommand command =
                        new SqlCommand(
                            query,
                            connection))
                    {
                        command.Parameters.Add(
                            "@RecordID",
                            SqlDbType.Int).Value =
                            selectedRecordID;

                        command.Parameters.Add(
                            "@PatientID",
                            SqlDbType.Int).Value =
                            patient.PatientID;

                        command.Parameters.Add(
                            "@Room",
                            SqlDbType.VarChar,
                            50).Value =
                            cmbRoom.Text;

                        command.Parameters.Add(
                            "@CheckIn",
                            SqlDbType.DateTime).Value =
                            dtpCheckIn.Value;

                        command.Parameters.Add(
                            "@CheckOut",
                            SqlDbType.DateTime).Value =
                            checkOut.HasValue
                                ? (object)checkOut.Value
                                : DBNull.Value;

                        command.Parameters.Add(
                            "@Status",
                            SqlDbType.VarChar,
                            30).Value =
                            cmbStatus.Text;

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Check-in record updated successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadCheckInRecords();
                ClearCheckInFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to update check-in record.\n\n" +
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnDeleteCheckIn_Click(
            object sender,
            EventArgs e)
        {
            if (selectedRecordID == 0)
            {
                MessageBox.Show(
                    "Please select a record from the table first.",
                    "Check-In",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult result =
                MessageBox.Show(
                    "Are you sure you want to delete this check-in record?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection connection =
                    databaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        DELETE FROM PatientCheckInOut
                        WHERE RecordID = @RecordID";

                    using (SqlCommand command =
                        new SqlCommand(
                            query,
                            connection))
                    {
                        command.Parameters.Add(
                            "@RecordID",
                            SqlDbType.Int).Value =
                            selectedRecordID;

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Check-in record deleted successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadCheckInRecords();
                ClearCheckInFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to delete check-in record.\n\n" +
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnRefreshCheckIn_Click(
            object sender,
            EventArgs e)
        {
            LoadPatientNames();
            LoadCheckInRecords();

            MessageBox.Show(
                "Check-in records refreshed.",
                "Refresh",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void ClearCheckInFields()
        {
            selectedRecordID = 0;

            txtPatientID.Clear();

            if (cmbPatientName.Items.Count > 0)
                cmbPatientName.SelectedIndex = 0;

            if (cmbRoom.Items.Count > 0)
                cmbRoom.SelectedIndex = 0;

            if (cmbStatus.Items.Count > 0)
                cmbStatus.SelectedIndex = 0;

            dtpCheckIn.Value = DateTime.Now;
            dtpCheckOut.Value = DateTime.Now;

            dgvCheckIn.ClearSelection();
        }
    }
}