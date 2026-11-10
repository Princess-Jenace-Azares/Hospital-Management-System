
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace HospitalLogIn
{
    public partial class Reports_Form : Form
    {
        private DataTable reportTable = new DataTable();
        private PrintDocument printDocument = new PrintDocument();
        private int printRowIndex = 0;

        public Reports_Form()
        {
            InitializeComponent();

            cmbReportType.Items.Clear();

            cmbReportType.Items.AddRange(new object[]
            {
                "Patient Records",
                "Admission Records",
                "Billing Records",
                "Payment Records",
                "Discharge Records"
            });

            cmbReportType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbReportType.SelectedIndex = 0;

            btnLoadReport.Click += btnLoadReport_Click;
            btnRefresh.Click += btnRefresh_Click;
            btnPrintReport.Click += btnPrintReport_Click;

            printDocument.PrintPage += printDocument_PrintPage;

            dgvReports.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvReports.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvReports.ReadOnly = true;
            dgvReports.AllowUserToAddRows = false;
        }

        private void btnLoadReport_Click(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void LoadReport()
        {
            if (cmbReportType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Report Type.");
                return;
            }

            if (dtpStartDate.Value.Date > dtpEndDate.Value.Date)
            {
                MessageBox.Show(
                    "Start Date cannot be later than End Date."
                );
                return;
            }

            string tableName = "";
            string[] dateColumns = null;

            switch (cmbReportType.Text)
            {
                case "Patient Records":
                    tableName = "Patient_Records";
                    dateColumns = new string[]
                    {
                        "RegistrationDate",
                        "DateRegistered",
                        "CreatedDate"
                    };
                    break;

                case "Admission Records":
                    tableName = "Admission_Records";
                    dateColumns = new string[]
                    {
                        "AdmissionDate"
                    };
                    break;

                case "Billing Records":
                    tableName = "Bill_Records";
                    dateColumns = new string[]
                    {
                        "BillDate",
                        "BillingDate",
                        "DateCreated"
                    };
                    break;

                case "Payment Records":
                    tableName = "Payment_Records";
                    dateColumns = new string[]
                    {
                        "PaymentDate"
                    };
                    break;

                case "Discharge Records":
                    tableName = "Discharge_Records";
                    dateColumns = new string[]
                    {
                        "DischargeDate"
                    };
                    break;

                default:
                    MessageBox.Show("Invalid Report Type.");
                    return;
            }

            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    string query = "SELECT * FROM " + tableName;

                    SqlDataAdapter da = new SqlDataAdapter(query, con);

                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    string selectedDateColumn = null;

                    foreach (string column in dateColumns)
                    {
                        if (dt.Columns.Contains(column))
                        {
                            selectedDateColumn = column;
                            break;
                        }
                    }

                    if (selectedDateColumn != null)
                    {
                        if (dt.Columns[selectedDateColumn].DataType == typeof(DateTime))
                        {
                            DataTable filteredTable = dt.Clone();

                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[selectedDateColumn] != DBNull.Value)
                                {
                                    DateTime recordDate =
                                        Convert.ToDateTime(row[selectedDateColumn]);

                                    if (recordDate.Date >= dtpStartDate.Value.Date &&
                                        recordDate.Date <= dtpEndDate.Value.Date)
                                    {
                                        filteredTable.ImportRow(row);
                                    }
                                }
                            }

                            dt = filteredTable;
                        }
                    }

                    reportTable = dt;

                    dgvReports.DataSource = reportTable;

                    dgvReports.AutoSizeColumnsMode =
                        DataGridViewAutoSizeColumnsMode.Fill;

                    dgvReports.Refresh();

                    if (reportTable.Rows.Count == 0)
                    {
                        MessageBox.Show(
                            "No records found for the selected report and date range.",
                            "Report Information",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                    else
                    {
                        MessageBox.Show(
                            "Report loaded successfully!\n\n" +
                            "Report Type: " + cmbReportType.Text +
                            "\nTotal Records: " + reportTable.Rows.Count,
                            "Report Loaded",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }

                    if (selectedDateColumn == null)
                    {
                        MessageBox.Show(
                            "No recognized date column was found in " +
                            tableName + ". All records are displayed without date filtering.",
                            "Date Filter Information"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load report:\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void btnPrintReport_Click(object sender, EventArgs e)
        {
            if (reportTable == null || reportTable.Rows.Count == 0)
            {
                MessageBox.Show("Please load a report before printing.");
                return;
            }

            try
            {
                printRowIndex = 0;

                using (PrintPreviewDialog preview = new PrintPreviewDialog())
                {
                    preview.Document = printDocument;
                    preview.Width = 1000;
                    preview.Height = 700;

                    preview.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to print report:\n" + ex.Message
                );
            }
        }

        private void printDocument_PrintPage(
            object sender, PrintPageEventArgs e)
        {
            float y = 40;
            float x = 40;

            Font titleFont = new Font("Arial", 16, FontStyle.Bold);
            Font headerFont = new Font("Arial", 9, FontStyle.Bold);
            Font normalFont = new Font("Arial", 8);

            e.Graphics.DrawString(
                "HOSPITAL MANAGEMENT SYSTEM",
                titleFont,
                Brushes.Black,
                x,
                y
            );

            y += 35;

            e.Graphics.DrawString(
                "Report: " + cmbReportType.Text,
                normalFont,
                Brushes.Black,
                x,
                y
            );

            y += 20;

            e.Graphics.DrawString(
                "Date Range: " +
                dtpStartDate.Value.ToShortDateString() +
                " - " +
                dtpEndDate.Value.ToShortDateString(),
                normalFont,
                Brushes.Black,
                x,
                y
            );

            y += 25;

            float columnWidth =
                (e.MarginBounds.Width) / reportTable.Columns.Count;

            foreach (DataColumn column in reportTable.Columns)
            {
                e.Graphics.DrawString(
                    column.ColumnName,
                    headerFont,
                    Brushes.Black,
                    x,
                    y
                );

                x += columnWidth;
            }

            y += 25;
            x = 40;

            e.Graphics.DrawLine(
                Pens.Black,
                40,
                y,
                e.MarginBounds.Right,
                y
            );

            y += 8;

            while (printRowIndex < reportTable.Rows.Count)
            {
                if (y > e.MarginBounds.Bottom - 30)
                {
                    e.HasMorePages = true;
                    return;
                }

                x = 40;

                foreach (DataColumn column in reportTable.Columns)
                {
                    string value = Convert.ToString(
                        reportTable.Rows[printRowIndex][column]
                    );

                    if (value.Length > 20)
                        value = value.Substring(0, 20);

                    e.Graphics.DrawString(
                        value,
                        normalFont,
                        Brushes.Black,
                        new RectangleF(x, y, columnWidth - 5, 35)
                    );

                    x += columnWidth;
                }

                y += 30;
                printRowIndex++;
            }

            e.HasMorePages = false;
            printRowIndex = 0;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void Reports_Form_Load(object sender, EventArgs e)
        {
        }
    }
}
