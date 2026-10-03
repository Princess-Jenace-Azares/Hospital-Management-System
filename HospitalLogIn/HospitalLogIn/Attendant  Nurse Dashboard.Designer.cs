namespace HospitalLogIn
{
    partial class Attendant__Nurse_Dashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnRefreshCheckIn = new System.Windows.Forms.Button();
            this.btnDeleteCheckIn = new System.Windows.Forms.Button();
            this.btnUpdateCheckIn = new System.Windows.Forms.Button();
            this.btnAddCheckIn = new System.Windows.Forms.Button();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.dtpCheckOut = new System.Windows.Forms.DateTimePicker();
            this.dtpCheckIn = new System.Windows.Forms.DateTimePicker();
            this.cmbRoom = new System.Windows.Forms.ComboBox();
            this.cmbPatientName = new System.Windows.Forms.ComboBox();
            this.txtPatientID = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dgvCheckIn = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCheckIn)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Green;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Welcome, Attendant!";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(37, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 15);
            this.label2.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Green;
            this.panel1.Controls.Add(this.label3);
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(30, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(734, 32);
            this.panel1.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Patient Check-In / Out";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnRefreshCheckIn);
            this.panel2.Controls.Add(this.btnDeleteCheckIn);
            this.panel2.Controls.Add(this.btnUpdateCheckIn);
            this.panel2.Controls.Add(this.btnAddCheckIn);
            this.panel2.Controls.Add(this.cmbStatus);
            this.panel2.Controls.Add(this.dtpCheckOut);
            this.panel2.Controls.Add(this.dtpCheckIn);
            this.panel2.Controls.Add(this.cmbRoom);
            this.panel2.Controls.Add(this.cmbPatientName);
            this.panel2.Controls.Add(this.txtPatientID);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(30, 60);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(734, 157);
            this.panel2.TabIndex = 3;
            // 
            // btnRefreshCheckIn
            // 
            this.btnRefreshCheckIn.BackColor = System.Drawing.Color.Silver;
            this.btnRefreshCheckIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefreshCheckIn.Location = new System.Drawing.Point(555, 109);
            this.btnRefreshCheckIn.Name = "btnRefreshCheckIn";
            this.btnRefreshCheckIn.Size = new System.Drawing.Size(156, 37);
            this.btnRefreshCheckIn.TabIndex = 16;
            this.btnRefreshCheckIn.Text = "Refresh";
            this.btnRefreshCheckIn.UseVisualStyleBackColor = false;
            // 
            // btnDeleteCheckIn
            // 
            this.btnDeleteCheckIn.BackColor = System.Drawing.Color.Silver;
            this.btnDeleteCheckIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteCheckIn.Location = new System.Drawing.Point(372, 109);
            this.btnDeleteCheckIn.Name = "btnDeleteCheckIn";
            this.btnDeleteCheckIn.Size = new System.Drawing.Size(156, 37);
            this.btnDeleteCheckIn.TabIndex = 15;
            this.btnDeleteCheckIn.Text = "Delete";
            this.btnDeleteCheckIn.UseVisualStyleBackColor = false;
            // 
            // btnUpdateCheckIn
            // 
            this.btnUpdateCheckIn.BackColor = System.Drawing.Color.Silver;
            this.btnUpdateCheckIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateCheckIn.Location = new System.Drawing.Point(195, 109);
            this.btnUpdateCheckIn.Name = "btnUpdateCheckIn";
            this.btnUpdateCheckIn.Size = new System.Drawing.Size(156, 37);
            this.btnUpdateCheckIn.TabIndex = 14;
            this.btnUpdateCheckIn.Text = "Update";
            this.btnUpdateCheckIn.UseVisualStyleBackColor = false;
            // 
            // btnAddCheckIn
            // 
            this.btnAddCheckIn.BackColor = System.Drawing.Color.Green;
            this.btnAddCheckIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddCheckIn.ForeColor = System.Drawing.Color.White;
            this.btnAddCheckIn.Location = new System.Drawing.Point(19, 109);
            this.btnAddCheckIn.Name = "btnAddCheckIn";
            this.btnAddCheckIn.Size = new System.Drawing.Size(156, 37);
            this.btnAddCheckIn.TabIndex = 13;
            this.btnAddCheckIn.Text = "Add Patient";
            this.btnAddCheckIn.UseVisualStyleBackColor = false;
            // 
            // cmbStatus
            // 
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(415, 82);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(200, 21);
            this.cmbStatus.TabIndex = 12;
            // 
            // dtpCheckOut
            // 
            this.dtpCheckOut.Location = new System.Drawing.Point(415, 47);
            this.dtpCheckOut.Name = "dtpCheckOut";
            this.dtpCheckOut.Size = new System.Drawing.Size(200, 20);
            this.dtpCheckOut.TabIndex = 11;
            // 
            // dtpCheckIn
            // 
            this.dtpCheckIn.Location = new System.Drawing.Point(415, 14);
            this.dtpCheckIn.Name = "dtpCheckIn";
            this.dtpCheckIn.Size = new System.Drawing.Size(200, 20);
            this.dtpCheckIn.TabIndex = 10;
            // 
            // cmbRoom
            // 
            this.cmbRoom.FormattingEnabled = true;
            this.cmbRoom.Location = new System.Drawing.Point(93, 82);
            this.cmbRoom.Name = "cmbRoom";
            this.cmbRoom.Size = new System.Drawing.Size(226, 21);
            this.cmbRoom.TabIndex = 9;
            // 
            // cmbPatientName
            // 
            this.cmbPatientName.FormattingEnabled = true;
            this.cmbPatientName.Location = new System.Drawing.Point(92, 47);
            this.cmbPatientName.Name = "cmbPatientName";
            this.cmbPatientName.Size = new System.Drawing.Size(227, 21);
            this.cmbPatientName.TabIndex = 8;
            // 
            // txtPatientID
            // 
            this.txtPatientID.Location = new System.Drawing.Point(92, 17);
            this.txtPatientID.Name = "txtPatientID";
            this.txtPatientID.Size = new System.Drawing.Size(226, 20);
            this.txtPatientID.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(341, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "Check-In:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(341, 55);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Check-Out:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(341, 85);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Status:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Room:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Patient Name:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Patient ID:";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Green;
            this.panel3.Controls.Add(this.label10);
            this.panel3.ForeColor = System.Drawing.Color.White;
            this.panel3.Location = new System.Drawing.Point(30, 223);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(734, 32);
            this.panel3.TabIndex = 3;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(16, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(254, 16);
            this.label10.TabIndex = 0;
            this.label10.Text = "Patient Location / Check-In Records";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dgvCheckIn);
            this.panel4.Location = new System.Drawing.Point(30, 251);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(734, 203);
            this.panel4.TabIndex = 4;
            // 
            // dgvCheckIn
            // 
            this.dgvCheckIn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCheckIn.Location = new System.Drawing.Point(19, 10);
            this.dgvCheckIn.Name = "dgvCheckIn";
            this.dgvCheckIn.Size = new System.Drawing.Size(692, 190);
            this.dgvCheckIn.TabIndex = 0;
            // 
            // Attendant__Nurse_Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Attendant__Nurse_Dashboard";
            this.Text = "Attendant__Nurse_Dashboard";
           
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCheckIn)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbRoom;
        private System.Windows.Forms.ComboBox cmbPatientName;
        private System.Windows.Forms.TextBox txtPatientID;
        private System.Windows.Forms.Button btnAddCheckIn;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.DateTimePicker dtpCheckOut;
        private System.Windows.Forms.DateTimePicker dtpCheckIn;
        private System.Windows.Forms.Button btnRefreshCheckIn;
        private System.Windows.Forms.Button btnDeleteCheckIn;
        private System.Windows.Forms.Button btnUpdateCheckIn;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridView dgvCheckIn;
    }
}