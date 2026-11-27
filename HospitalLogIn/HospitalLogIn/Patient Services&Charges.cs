

            if (cmbService.SelectedIndex >= 0 &&
                string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                txtQuantity.Text = "1";
            }

            CalculateTotal();
        }

        // QUANTITY CHANGED
        private void txtQuantity_TextChanged(
            object sender, EventArgs e)
        {
            CalculateTotal();
        }

        // CALCULATE TOTAL AMOUNT
        private void CalculateTotal()
        {
            if (selectedUnitPrice > 0 &&
                int.TryParse(txtQuantity.Text, out int quantity) &&
                quantity > 0)
            {
                decimal total = selectedUnitPrice * quantity;

                txtTotalAmount.Text = total.ToString("0.00");
            }
            else
            {
                txtTotalAmount.Clear();
            }
        }

        // ADD SERVICE
        private void btnAddService_Click(
            object sender, EventArgs e)
        {
            if (cmbAdmissionID.SelectedIndex == -1 ||
                cmbService.SelectedIndex == -1 ||
                !int.TryParse(txtPatientID.Text, out int patientID) ||
                !int.TryParse(txtQuantity.Text, out int quantity) ||
                quantity <= 0 ||
                selectedUnitPrice <= 0 ||
                !decimal.TryParse(txtTotalAmount.Text, out decimal total))
            {
                MessageBox.Show(
                    "Please complete all service information correctly.",
                    "Incomplete Information"
                );

                return;
            }

            DataRowView selectedAdmission =
                cmbAdmissionID.SelectedItem as DataRowView;

            if (selectedAdmission == null)
            {
                MessageBox.Show("Please select a valid admission.");
                return;
            }

            int admissionID =
                Convert.ToInt32(selectedAdmission["AdmissionID"]);

            try
            {
                using (SqlConnection con =
                    DatabaseConnection.GetConnection())
                {
                    string query = @"
                        INSERT INTO Services_Charges
                        (
                            AdmissionID,
                            PatientID,
                            ServiceName,
                            Quantity,
                            UnitPrice,
                            TotalAmount
                        )
                        SELECT
                            A.AdmissionID,
                            A.PatientID,
                            @service,
                            @quantity,
                            @unitPrice,
                            @totalAmount
                        FROM Admission_Records A
                        WHERE A.AdmissionID = @admissionID
                          AND A.PatientID = @patientID
                          AND A.Status = 'Admitted'";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.Add("@admissionID",
                        SqlDbType.Int).Value = admissionID;

                    cmd.Parameters.Add("@patientID",
                        SqlDbType.Int).Value = patientID;

                    cmd.Parameters.Add("@service",
                        SqlDbType.NVarChar, 150).Value =
                        cmbService.Text;

                    cmd.Parameters.Add("@quantity",
                        SqlDbType.Int).Value = quantity;

                    SqlParameter unitPriceParameter =
                        cmd.Parameters.Add("@unitPrice",
                            SqlDbType.Decimal);

                    unitPriceParameter.Precision = 10;
                    unitPriceParameter.Scale = 2;
                    unitPriceParameter.Value = selectedUnitPrice;

                    SqlParameter totalParameter =
                        cmd.Parameters.Add("@totalAmount",
                            SqlDbType.Decimal);

                    totalParameter.Precision = 10;
                    totalParameter.Scale = 2;
                    totalParameter.Value = total;

                    con.Open();

                    int result = cmd.ExecuteNonQuery();

                    if (result == 0)
                    {
                        MessageBox.Show(
                            "The selected admission is no longer active. " +
                            "Please refresh and select another admission."
                        );

                        LoadActiveAdmissions();
                        return;
                    }
                }

                MessageBox.Show(
                    "Service added successfully!\n\n" +
                    "Patient: " + txtPatientName.Text +
                    "\nService: " + cmbService.Text +
                    "\nQuantity: " + quantity +
                    "\nUnit Price: " +
                    selectedUnitPrice.ToString("0.00") +
                    "\nTotal Amount: " + total.ToString("0.00"),
                    "Success"
                );

                LoadServices();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Add Service Error:\n" + ex.Message,
                    "Database Error"
                );
            }
        }

        // CLEAR BUTTON
        private void btnClear_Click(
            object sender, EventArgs e)
        {
            ClearFields();
        }

        // CLEAR ALL FIELDS
        private void ClearFields()
        {
            cmbAdmissionID.SelectedIndex = -1;
            cmbService.SelectedIndex = -1;

            txtPatientID.Clear();
            txtPatientName.Clear();
            txtQuantity.Clear();
            txtTotalAmount.Clear();

            selectedUnitPrice = 0;
        }

        private void Patient_Services_Charges_Load(
            object sender, EventArgs e)
        {
        }

        private void panel1_Paint(
            object sender, PaintEventArgs e)
        {
        }
    }
}
