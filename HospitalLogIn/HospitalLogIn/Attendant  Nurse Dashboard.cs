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
        public Attendant__Nurse_Dashboard()
        {
            InitializeComponent();
            databaseConnection = new DatabaseConnection();

            LoadPatientNames();
            LoadCheckInRecords();
            SetupDashboard();
        }

        private DatabaseConnection databaseConnection;

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
                                cmbPatientName.Items.Add(
                                    new PatientItem

                                    {
                                        PatientID =
                                            Convert.ToInt32(
                                                reader["PatientID"]
                                            ),

                                        PatientName =
                                            reader["PatientName"].ToString()
                                    }
                                );
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Database Error"
                );
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
                        new SqlDataAdapter(query, connection))
                    {
                        DataTable table = new DataTable();

                        adapter.Fill(table);

                        dgvCheckIn.DataSource = table;
                    }
                }

                dgvCheckIn.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;

                dgvCheckIn.SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect;

                dgvCheckIn.ReadOnly = true;

                dgvCheckIn.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Database Error"
                );
            }
        }
        private void SetupDashboard()
        {
            this.Text = "Attendant / Nurse Dashboard";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;

            Label title = new Label();

            title.Text = "ATTENDANT / NURSE DASHBOARD";
            title.Font = new Font(
                "Segoe UI",
                28,
                FontStyle.Bold
            );

            title.AutoSize = true;
            title.Location = new Point(50, 40);

            this.Controls.Add(title);
        }

        private void Attendant__Nurse_Dashboard_Load(object sender, EventArgs e)
        {

        }
    }
}