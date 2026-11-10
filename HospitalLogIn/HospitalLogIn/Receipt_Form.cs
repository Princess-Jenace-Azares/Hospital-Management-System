
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace HospitalLogIn
{
    public partial class Receipt_Form : Form
    {
        private int paymentID;

        private string receiptNo = "";
        private string billNo = "";
        private string patientName = "";
        private string paymentDate = "";
        private string paymentMethod = "";
        private string amountPaid = "";

        public Receipt_Form(int paymentID)
        {
            InitializeComponent();

            this.paymentID = paymentID;

            btnPrint.Click += btnPrint_Click;
            btnClose.Click += btnClose_Click;

            printDocument1.PrintPage += printDocument1_PrintPage;

            printPreviewDialog1.Document = printDocument1;

            this.Shown += Receipt_Form_Shown;
        }

        private void Receipt_Form_Shown(object sender, EventArgs e)
        {
            LoadReceipt();
        }

        private void LoadReceipt()
        {
            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = @"
                        SELECT
                            pay.PaymentID,
                            pay.BillNo,
                            p.PatientName,
                            pay.PaymentDate,
                            pay.PaymentMethod,
                            pay.AmountPaid
                        FROM Payment_Records pay
                        INNER JOIN Bill_Records b
                            ON pay.BillNo = b.BillNo
                        INNER JOIN Patient_Records p
                            ON b.PatientID = p.PatientID
                        WHERE pay.PaymentID = @PaymentID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add(
                            "@PaymentID", SqlDbType.Int).Value = paymentID;

                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show("Receipt not found.");

                                this.Close();
                                return;
                            }

                            receiptNo =
                                reader["PaymentID"].ToString();

                            billNo =
                                reader["BillNo"].ToString();

                            patientName =
                                Convert.ToString(reader["PatientName"]);

                            paymentDate =
                                reader["PaymentDate"] == DBNull.Value
                                ? "Not available"
                                : Convert.ToDateTime(reader["PaymentDate"])
                                    .ToString("MMMM dd, yyyy hh:mm tt");

                            paymentMethod =
                                Convert.ToString(reader["PaymentMethod"]);

                            amountPaid =
                                Convert.ToDecimal(reader["AmountPaid"])
                                .ToString("N2");

                            lblReceiptNo.Text = receiptNo;
                            lblBillNo.Text = billNo;
                            lblPatientName.Text = patientName;
                            lblPaymentDate.Text = paymentDate;
                            lblPaymentMethod.Text = paymentMethod;
                            lblAmountPaid.Text = "₱ " + amountPaid;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load receipt:\n" + ex.Message,
                    "Receipt Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(receiptNo))
            {
                MessageBox.Show("No receipt information to print.");
                return;
            }

            try
            {
                printPreviewDialog1.Document = printDocument1;
                printPreviewDialog1.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to print receipt:\n" + ex.Message,
                    "Print Error"
                );
            }
        }

        private void printDocument1_PrintPage(
            object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            Font titleFont = new Font("Arial", 18, FontStyle.Bold);
            Font headerFont = new Font("Arial", 12, FontStyle.Bold);
            Font normalFont = new Font("Arial", 11);

            float x = 70;
            float y = 60;

            g.DrawString(
                "HOSPITAL MANAGEMENT SYSTEM",
                titleFont,
                Brushes.Black,
                x,
                y
            );

            y += 50;

            g.DrawString(
                "OFFICIAL PAYMENT RECEIPT",
                headerFont,
                Brushes.Black,
                x,
                y
            );

            y += 40;

            g.DrawLine(Pens.Black, x, y, 520, y);

            y += 30;

            g.DrawString(
                "Receipt No.: " + receiptNo,
                normalFont,
                Brushes.Black,
                x,
                y
            );

            y += 35;

            g.DrawString(
                "Bill No.: " + billNo,
                normalFont,
                Brushes.Black,
                x,
                y
            );

            y += 35;

            g.DrawString(
                "Patient Name: " + patientName,
                normalFont,
                Brushes.Black,
                x,
                y
            );

            y += 35;

            g.DrawString(
                "Payment Date: " + paymentDate,
                normalFont,
                Brushes.Black,
                x,
                y
            );

            y += 35;

            g.DrawString(
                "Payment Method: " + paymentMethod,
                normalFont,
                Brushes.Black,
                x,
                y
            );

            y += 35;

            g.DrawLine(Pens.Black, x, y, 520, y);

            y += 30;

            g.DrawString(
                "Amount Paid: ₱ " + amountPaid,
                headerFont,
                Brushes.Black,
                x,
                y
            );

            y += 60;

            g.DrawString(
                "Thank you for your payment!",
                headerFont,
                Brushes.Black,
                x,
                y
            );

            y += 30;

            g.DrawString(
                "Please keep this receipt for your records.",
                normalFont,
                Brushes.Black,
                x,
                y
            );

            e.HasMorePages = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Receipt_Form_Load(object sender, EventArgs e)
        {
        }
    }
}
