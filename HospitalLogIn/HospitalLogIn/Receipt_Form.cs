


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
