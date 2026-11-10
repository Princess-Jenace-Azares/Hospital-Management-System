
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace HospitalLogIn
{
    public partial class Receipt_Form : Form
    {
        private int paymentID;

        public Receipt_Form(int id)
        {
            InitializeComponent();

            paymentID = id;

            btnPrint.Click += btnPrint_Click;
            btnClose.Click += (s, e) => this.Close();

            printDocument1.PrintPage += printDocument1_PrintPage;

            LoadReceipt();
        }

        private void LoadReceipt()
        {
            using (SqlConnection con = DatabaseConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        py.PaymentID,
                        py.BillNo,
                        p.PatientName,
                        py.PaymentDate,
                        py.PaymentMethod,
                        py.AmountPaid
                    FROM Payment_Records py
                    INNER JOIN Bill_Records b
                        ON py.BillNo = b.BillNo
                    INNER JOIN Patient_Records p
                        ON b.PatientID = p.PatientID
                    WHERE py.PaymentID = @id";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value =
                    paymentID;

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        MessageBox.Show("Receipt record not found.");
                        Close();
                        return;
                    }

                    lblReceiptNo.Text =
                        reader["PaymentID"].ToString();

                    lblBillNo.Text =
                        reader["BillNo"].ToString();

                    lblPatientName.Text =
                        reader["PatientName"].ToString();

                    lblPaymentDate.Text =
                        Convert.ToDateTime(reader["PaymentDate"])
                        .ToString("MMMM dd, yyyy hh:mm tt");

                    lblPaymentMethod.Text =
                        reader["PaymentMethod"].ToString();

                    lblAmountPaid.Text =
                        Convert.ToDecimal(reader["AmountPaid"])
                        .ToString("C2");
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(
            object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            Font title = new Font("Arial", 18, FontStyle.Bold);
            Font normal = new Font("Arial", 11);
            Font bold = new Font("Arial", 11, FontStyle.Bold);

            int y = 60;

            g.DrawString(
                "HOSPITAL PAYMENT RECEIPT",
                title,
                Brushes.Black,
                70, y);

            y += 60;

            g.DrawString("Receipt No: " + lblReceiptNo.Text,
                normal, Brushes.Black, 70, y);
            y += 30;

            g.DrawString("Bill No: " + lblBillNo.Text,
                normal, Brushes.Black, 70, y);
            y += 30;

            g.DrawString("Patient: " + lblPatientName.Text,
                normal, Brushes.Black, 70, y);
            y += 30;

            g.DrawString("Payment Date: " + lblPaymentDate.Text,
                normal, Brushes.Black, 70, y);
            y += 30;

            g.DrawString("Payment Method: " + lblPaymentMethod.Text,
                normal, Brushes.Black, 70, y);
            y += 45;

            g.DrawString("Amount Paid: " + lblAmountPaid.Text,
                bold, Brushes.Black, 70, y);
            y += 50;

            g.DrawString("Thank you for your payment!",
                normal, Brushes.Black, 70, y);
        }
    }
}
