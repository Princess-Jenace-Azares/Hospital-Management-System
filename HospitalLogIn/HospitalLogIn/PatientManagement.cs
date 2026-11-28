
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

            cmbGender.Items.AddRange(new object[]
            {
                "Male", "Female"
            });

            cmbCivilStatus.Items.AddRange(new object[]
            {
                "Single", "Married", "Widowed", "Divorced", "Separated"
            });

            cmbBloodType.Items.AddRange(new object[]
            {
                "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-"
            });

            cmbLOC.Items.AddRange(new object[]
            {
                "Alert", "Verbal", "Pain", "Unresponsive"
            });

            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnRefresh.Click += btnRefresh_Click;
            btnSearch.Click += btnSearch_Click;

            dgvPatients.CellClick += dgvPatients_CellClick;

            this.Shown += Admin_Dashboard_Shown;
        }

        private void Admin_Dashboard_Shown(object sender, EventArgs e)
        {
            LoadPatients();
            GeneratePatientID();
        }

        // LOAD PATIENTS
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
                    dgvPatients.AutoSizeColumnsMode =
                        DataGridViewAutoSizeColumnsMode.Fill;

                    dgvPatients.ReadOnly = true;
                    dgvPatients.AllowUserToAddRows = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load patients: " + ex.Message);
            }
        }

        // AUTOMATIC PATIENT ID
        private void GeneratePatientID()
        {
            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"
                        SELECT ISNULL(MAX(PatientID), 0) + 1
                        FROM Patient_Records";

                    SqlCommand cmd = new SqlCommand(query, con);

                    con.Open();

                    int nextID = Convert.ToInt32(cmd.ExecuteScalar());

                    txtPatientID.Text = nextID.ToString("D3");
                    txtPatientID.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to generate Patient ID: " + ex.Message);
            }
        }

        // ADD PATIENT
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPatientName.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) ||
                cmbGender.SelectedIndex == -1)
            {
                MessageBox.Show("Please complete the required patient information.");
                return;
            }

            if (!decimal.TryParse(txtTemperature.Text, out decimal temperature) ||
                !decimal.TryParse(txtWeight.Text, out decimal weight) ||
                !decimal.TryParse(txtHeight.Text, out decimal height))
            {
                MessageBox.Show("Please enter valid temperature, weight, and height.");
                return;
            }

            int age = DateTime.Today.Year - dtpDateOfBirth.Value.Year;

            if (dtpDateOfBirth.Value.Date > DateTime.Today)
            {
                MessageBox.Show("Date of birth cannot be in the future.");
                return;
            }

            if (dtpDateOfBirth.Value.Date > DateTime.Today.AddYears(-age))
                age--;

            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"
                    INSERT INTO Patient_Records
                    (
                        PatientID,
                        PatientName,
                        Age,
                        Gender,
                        Address,
                        DateOfBirth,
                        CivilStatus,
                        BloodType,
                        Temperature,
                        PMH,
                        Allergy,
                        Weight,
                        Height,
                        BroughtBy,
                        RelationToThePatient,
                        LOC
                    )
                    VALUES
                    (
                        @id, @name, @age, @gender, @address,
                        @dob, @civil, @blood, @temperature,
                        @pmh, @allergy, @weight, @height,
                        @broughtby, @relation, @loc
                    )";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value =
                            int.Parse(txtPatientID.Text);

                        cmd.Parameters.Add("@name", SqlDbType.NVarChar, 150).Value =
                            txtPatientName.Text.Trim();

                        cmd.Parameters.Add("@age", SqlDbType.Int).Value = age;

                        cmd.Parameters.Add("@gender", SqlDbType.NVarChar, 20).Value =
                            cmbGender.Text;

                        cmd.Parameters.Add("@address", SqlDbType.NVarChar, 255).Value =
                            txtAddress.Text.Trim();

                        cmd.Parameters.Add("@dob", SqlDbType.Date).Value =
                            dtpDateOfBirth.Value.Date;

                        cmd.Parameters.Add("@civil", SqlDbType.NVarChar, 50).Value =
                            cmbCivilStatus.Text;

                        cmd.Parameters.Add("@blood", SqlDbType.NVarChar, 10).Value =
                            cmbBloodType.Text;

                        cmd.Parameters.Add("@temperature", SqlDbType.Decimal).Value =
                            temperature;

                        cmd.Parameters.Add("@pmh", SqlDbType.NVarChar).Value =
                            txtPMH.Text;

                        cmd.Parameters.Add("@allergy", SqlDbType.NVarChar).Value =
                            txtAllergy.Text;

                        cmd.Parameters.Add("@weight", SqlDbType.Decimal).Value =
                            weight;

                        cmd.Parameters.Add("@height", SqlDbType.Decimal).Value =
                            height;

                        cmd.Parameters.Add("@broughtby", SqlDbType.NVarChar, 150).Value =
                            txtBroughtBy.Text;

                        cmd.Parameters.Add("@relation", SqlDbType.NVarChar, 100).Value =
                            txtRelationToThePatient.Text;

                        cmd.Parameters.Add("@loc", SqlDbType.NVarChar, 50).Value =
                            cmbLOC.Text;

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Patient added successfully!");

                LoadPatients();
                ClearFields();
                GeneratePatientID();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add Patient Error: " + ex.Message);
            }
        }

        // SEARCH PATIENT BY NAME
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"
                    SELECT * FROM Patient_Records
                    WHERE PatientName LIKE @search";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);

                    da.SelectCommand.Parameters.Add("@search", SqlDbType.NVarChar, 150)
                        .Value = "%" + txtSearch.Text.Trim() + "%";

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvPatients.DataSource = dt;

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No patient found.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search Error: " + ex.Message);
            }
        }

        // SELECT PATIENT
        private void dgvPatients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvPatients.Rows[e.RowIndex];

            txtPatientID.Text = Convert.ToString(row.Cells["PatientID"].Value);
            txtPatientName.Text = Convert.ToString(row.Cells["PatientName"].Value);
            txtAge.Text = Convert.ToString(row.Cells["Age"].Value);
            cmbGender.Text = Convert.ToString(row.Cells["Gender"].Value);

            txtAddress.Text = Convert.ToString(row.Cells["Address"].Value);

            dtpDateOfBirth.Value =
                Convert.ToDateTime(row.Cells["DateOfBirth"].Value);

            cmbCivilStatus.Text = Convert.ToString(row.Cells["CivilStatus"].Value);
            cmbBloodType.Text = Convert.ToString(row.Cells["BloodType"].Value);

            txtTemperature.Text = Convert.ToString(row.Cells["Temperature"].Value);
            txtPMH.Text = Convert.ToString(row.Cells["PMH"].Value);
            txtAllergy.Text = Convert.ToString(row.Cells["Allergy"].Value);
            txtWeight.Text = Convert.ToString(row.Cells["Weight"].Value);
            txtHeight.Text = Convert.ToString(row.Cells["Height"].Value);

            txtBroughtBy.Text = Convert.ToString(row.Cells["BroughtBy"].Value);

            txtRelationToThePatient.Text =
                Convert.ToString(row.Cells["RelationToThePatient"].Value);

            cmbLOC.Text = Convert.ToString(row.Cells["LOC"].Value);
        }

        // UPDATE PATIENT
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "The Update button still needs its updated SQL query " +
                "to save all the new patient fields."
            );
        }

        // DELETE PATIENT
        private void btnDelete_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Use your existing transactional delete method here. " +
                "It must delete dependent records before deleting the patient."
            );
        }

        // REFRESH
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPatients();
            ClearFields();
            GeneratePatientID();
        }

        // CLEAR FIELDS
        private void ClearFields()
        {
            txtPatientName.Clear();
            txtAge.Clear();
            txtAddress.Clear();

            cmbGender.SelectedIndex = -1;
            cmbCivilStatus.SelectedIndex = -1;
            cmbBloodType.SelectedIndex = -1;
            cmbLOC.SelectedIndex = -1;

            dtpDateOfBirth.Value = DateTime.Today;

            txtTemperature.Clear();
            txtPMH.Clear();
            txtAllergy.Clear();
            txtWeight.Clear();
            txtHeight.Clear();
            txtBroughtBy.Clear();
            txtRelationToThePatient.Clear();
            txtSearch.Clear();
        }
    }
}
