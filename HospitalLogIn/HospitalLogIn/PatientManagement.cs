

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

                  
                    dgvPatients.DataSource = null;
                    dgvPatients.AutoGenerateColumns = true;
                    dgvPatients.DataSource = dt;
                    dgvPatients.AutoSizeColumnsMode =
                        DataGridViewAutoSizeColumnsMode.Fill;
                    dgvPatients.ReadOnly = true;
                    dgvPatients.AllowUserToAddRows = false;
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
            if (!int.TryParse(txtPatientID.Text, out int patientID))
            {
                MessageBox.Show("Please select a patient first.");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "WARNING: This will permanently delete this patient " +
                "and ALL related hospital records, including admissions, " +
                "services, bills, payments, and discharge records.\n\n" +
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
                            // 1. Delete payments connected to this patient's bills
                            string deletePayments = @"
                        DELETE FROM Payment_Records
                        WHERE BillNo IN
                        (
                            SELECT BillNo
                            FROM Bill_Records
                            WHERE PatientID = @id
                        )";

                            using (SqlCommand cmd = new SqlCommand(deletePayments, con, tx))
                            {
                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = patientID;
                                cmd.ExecuteNonQuery();
                            }

                            // 2. Delete all billing records
                            string deleteBills = @"
                        DELETE FROM Bill_Records
                        WHERE PatientID = @id";

                            using (SqlCommand cmd = new SqlCommand(deleteBills, con, tx))
                            {
                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = patientID;
                                cmd.ExecuteNonQuery();
                            }

                            // 3. Delete all services and charges
                            string deleteServices = @"
                        DELETE FROM Services_Charges
                        WHERE PatientID = @id";

                            using (SqlCommand cmd = new SqlCommand(deleteServices, con, tx))
                            {
                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = patientID;
                                cmd.ExecuteNonQuery();
                            }

                            // 4. Delete discharge records
                            string deleteDischarge = @"
                        DELETE FROM Discharge_Records
                        WHERE PatientID = @id";

                            using (SqlCommand cmd = new SqlCommand(deleteDischarge, con, tx))
                            {
                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = patientID;
                                cmd.ExecuteNonQuery();
                            }

                            // 5. Delete all admission records
                            string deleteAdmissions = @"
                        DELETE FROM Admission_Records
                        WHERE PatientID = @id";

                            using (SqlCommand cmd = new SqlCommand(deleteAdmissions, con, tx))
                            {
                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = patientID;
                                cmd.ExecuteNonQuery();
                            }

                            // 6. Permanently delete patient
                            string deletePatient = @"
                        DELETE FROM Patient_Records
                        WHERE PatientID = @id";

                            using (SqlCommand cmd = new SqlCommand(deletePatient, con, tx))
                            {
                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = patientID;

                                int result = cmd.ExecuteNonQuery();

                                if (result == 0)
                                    throw new Exception("Patient record was not found.");
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
                    "Patient and all related hospital records were permanently deleted!",
                    "Deletion Successful",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadPatients();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to delete patient:\n\n" + ex.Message,
                    "Delete Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
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

        private void Admin_Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }

}

