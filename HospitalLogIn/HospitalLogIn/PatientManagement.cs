

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

