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
    public partial class Billing_Staff_Dashboard : Form
    {
        public Billing_Staff_Dashboard()
        {
             
        
            InitializeComponent();

            databaseConnection = new DatabaseConnection();

            LoadBillingPatients();
            LoadBillingRecords();
        }

        private void LoadBillingPatients()
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
                            cmbBillingPatient.Items.Clear();

                            while (reader.Read())
                            {
                                cmbBillingPatient.Items.Add(
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
        private class PatientItem
        {
            public int PatientID { get; set; }

            public string PatientName { get; set; }

            public override string ToString()
            {
                return PatientName;
            }
        }
        private void LoadBillingRecords()
        {
            try
            {
                using (SqlConnection connection =
                    databaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                SELECT
                    B.BillID,
                    B.PatientID,
                    P.PatientName,
                    B.Amount,
                    B.PaymentStatus,
                    B.PaymentDate
                FROM Billing B
                INNER JOIN Patients P
                    ON B.PatientID = P.PatientID
                ORDER BY B.BillID DESC";

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(query, connection))
                    {
                        DataTable table = new DataTable();

                        adapter.Fill(table);

                        dgvBilling.DataSource = table;
                    }
                }

                dgvBilling.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;

                dgvBilling.SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect;

                dgvBilling.ReadOnly = true;

                dgvBilling.AllowUserToAddRows = false;
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
            this.Text = "Billing Staff Dashboard";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;

            Label title = new Label();

            title.Text = "BILLING STAFF DASHBOARD";
            title.Font = new Font(
                "Segoe UI",
                28,
                FontStyle.Bold
            );

            title.AutoSize = true;
            title.Location = new Point(50, 40);

            this.Controls.Add(title);
        }

        
            private DatabaseConnection databaseConnection;

      
    }
    }
