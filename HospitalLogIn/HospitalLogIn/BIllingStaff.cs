


                                int billNo =
                                    Convert.ToInt32(cmd.ExecuteScalar());

                                txtBillID.Text = billNo.ToString();
                            }

                            transaction.Commit();

                            MessageBox.Show(
                                "Bill generated successfully!\nBill No: " +
                                txtBillID.Text,
                                "Success",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );

                            LoadBilling();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to generate bill:\n\n" + ex.Message,
                    "Billing Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // UPDATE BILL AMOUNT AND PAYMENT STATUS
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtBillID.Text, out int billID))
            {
                MessageBox.Show("Please select or generate a bill first.");
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount) ||
                amount < 0)
            {
                MessageBox.Show("Please enter a valid amount.");
                return;
            }

            if (cmbPaymentStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a payment status.");
                return;
            }

            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"
                        UPDATE Bill_Records
                        SET TotalAmount = @Amount,
                            Balance = @Amount - AmountPaid,
                            BillStatus = @Status
                        WHERE BillNo = @BillNo
                          AND @Amount >= AmountPaid";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal)
                            .Value = amount;

                        cmd.Parameters["@Amount"].Precision = 10;
                        cmd.Parameters["@Amount"].Scale = 2;

                        cmd.Parameters.Add("@Status", SqlDbType.VarChar, 30)
                            .Value = cmbPaymentStatus.Text;

                        cmd.Parameters.Add("@BillNo", SqlDbType.Int)
                            .Value = billID;

                        con.Open();

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Billing record updated successfully!");
                            LoadBilling();
                        }
                        else
                        {
                            MessageBox.Show(
                                "Update failed. The amount cannot be less than the amount already paid."
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Update Error:\n" + ex.Message,
                    "Database Error"
                );
            }
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtBillID.Text, out int billNo))
            {
                MessageBox.Show("Please select a bill first.");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Permanently delete this bill and all its payment records?\n\n" +
                "This action cannot be undone.",
                "Confirm Delete",
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

                    using (SqlTransaction transaction = con.BeginTransaction())
                    {
                        try
                        {
                            // 1. Delete payments linked to this bill
                            string deletePayments = @"
                        DELETE FROM Payment_Records
                        WHERE BillNo = @BillNo";

                            using (SqlCommand cmd = new SqlCommand(
                                deletePayments, con, transaction))
                            {
                                cmd.Parameters.Add("@BillNo", SqlDbType.Int)
                                    .Value = billNo;

                                cmd.ExecuteNonQuery();
                            }

                            // 2. Delete the bill
                            string deleteBill = @"
                        DELETE FROM Bill_Records
                        WHERE BillNo = @BillNo";

                            using (SqlCommand cmd = new SqlCommand(
                                deleteBill, con, transaction))
                            {
                                cmd.Parameters.Add("@BillNo", SqlDbType.Int)
                                    .Value = billNo;

                                int result = cmd.ExecuteNonQuery();

                                if (result == 0)
                                    throw new Exception("Bill record not found.");
                            }

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                MessageBox.Show(
                    "Bill and its payment records permanently deleted!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadBilling();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Delete Billing Error:\n\n" + ex.Message,
                    "Delete Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAdmissions();
            LoadBilling();
        }

        // SELECT BILL FROM DATAGRIDVIEW
        private void dgvBilling_CellClick(
            object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvBilling.Rows[e.RowIndex];

            txtBillID.Text =
                Convert.ToString(row.Cells["BillID"].Value);

            txtPatientName.Text =
                Convert.ToString(row.Cells["PatientName"].Value);

            txtAmount.Text =
                Convert.ToDecimal(row.Cells["Amount"].Value)
                .ToString("0.00");

            cmbPaymentStatus.Text =
                Convert.ToString(row.Cells["PaymentStatus"].Value);

            selectedAdmissionID =
                Convert.ToInt32(row.Cells["AdmissionID"].Value);

            cmbAdmissionID.SelectedValue = selectedAdmissionID;
        }

        // CLEAR FIELDS
        private void ClearFields()
        {
            txtBillID.Clear();
            txtPatientName.Clear();
            txtAmount.Clear();

            cmbAdmissionID.SelectedIndex = -1;
            cmbPaymentStatus.SelectedIndex = -1;

            selectedAdmissionID = 0;
        }

        private void BIllingStaff_Load(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
