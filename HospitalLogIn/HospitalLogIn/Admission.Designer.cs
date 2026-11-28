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
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtAdmissionID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.dtpAdmissionDate = new System.Windows.Forms.DateTimePicker();
            this.cmbRoom = new System.Windows.Forms.ComboBox();
            this.cmbPatientName = new System.Windows.Forms.ComboBox();
            this.txtPatientID = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.dgvAdmission = new System.Windows.Forms.DataGridView();
            this.txtRoomSearch = new System.Windows.Forms.TextBox();
            this.btnRoomSearch = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtReasonOfAdmission = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtTransferredRoom = new System.Windows.Forms.TextBox();
            this.cmbDoctor = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cmbNurse = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cmbServices = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdmission)).BeginInit();
            this.SuspendLayout();
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
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.panel1.Controls.Add(this.label3);
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(-2, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1316, 32);
            this.panel1.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(15, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Admission";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label15);
            this.panel2.Controls.Add(this.cmbServices);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.cmbNurse);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.cmbDoctor);
            this.panel2.Controls.Add(this.txtTransferredRoom);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.txtReasonOfAdmission);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.btnRoomSearch);
            this.panel2.Controls.Add(this.txtRoomSearch);
            this.panel2.Controls.Add(this.txtAdmissionID);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btnRefresh);
            this.panel2.Controls.Add(this.dtpAdmissionDate);
            this.panel2.Controls.Add(this.cmbRoom);
            this.panel2.Controls.Add(this.cmbPatientName);
            this.panel2.Controls.Add(this.txtPatientID);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(-2, 39);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1313, 151);
            this.panel2.TabIndex = 3;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // txtAdmissionID
            // 
            this.txtAdmissionID.Location = new System.Drawing.Point(968, 36);
            this.txtAdmissionID.Name = "txtAdmissionID";
            this.txtAdmissionID.Size = new System.Drawing.Size(227, 20);
            this.txtAdmissionID.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(867, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Admission No.:";
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.Silver;
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(832, 60);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(123, 27);
            this.btnRefresh.TabIndex = 16;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Silver;
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(618, 196);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(129, 27);
            this.btnDelete.TabIndex = 15;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.Silver;
            this.btnUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.Location = new System.Drawing.Point(391, 196);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(129, 27);
            this.btnUpdate.TabIndex = 14;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = false;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(187, 196);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(125, 27);
            this.btnAdd.TabIndex = 13;
            this.btnAdd.Text = "Add Patient";
            this.btnAdd.UseVisualStyleBackColor = false;
            // 
            // cmbStatus
            // 
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(993, 201);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(227, 21);
            this.cmbStatus.TabIndex = 12;
            // 
            // dtpAdmissionDate
            // 
            this.dtpAdmissionDate.Location = new System.Drawing.Point(568, 8);
            this.dtpAdmissionDate.Name = "dtpAdmissionDate";
            this.dtpAdmissionDate.Size = new System.Drawing.Size(181, 20);
            this.dtpAdmissionDate.TabIndex = 10;
            // 
            // cmbRoom
            // 
            this.cmbRoom.FormattingEnabled = true;
            this.cmbRoom.Location = new System.Drawing.Point(525, 34);
            this.cmbRoom.Name = "cmbRoom";
            this.cmbRoom.Size = new System.Drawing.Size(205, 21);
            this.cmbRoom.TabIndex = 9;
            // 
            // cmbPatientName
            // 
            this.cmbPatientName.FormattingEnabled = true;
            this.cmbPatientName.Location = new System.Drawing.Point(124, 35);
            this.cmbPatientName.Name = "cmbPatientName";
            this.cmbPatientName.Size = new System.Drawing.Size(236, 21);
            this.cmbPatientName.TabIndex = 8;
            // 
            // txtPatientID
            // 
            this.txtPatientID.Location = new System.Drawing.Point(124, 9);
            this.txtPatientID.Name = "txtPatientID";
            this.txtPatientID.Size = new System.Drawing.Size(236, 20);
            this.txtPatientID.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(409, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "Admission Date:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(947, 210);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Status:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(454, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Room Type:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Patient Name:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Patient ID:";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.panel3.Controls.Add(this.label10);
            this.panel3.ForeColor = System.Drawing.Color.White;
            this.panel3.Location = new System.Drawing.Point(12, 229);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1316, 32);
            this.panel3.TabIndex = 3;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(16, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(142, 16);
            this.label10.TabIndex = 0;
            this.label10.Text = "Admission Records";
            // 
            // dgvAdmission
            // 
            this.dgvAdmission.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAdmission.Location = new System.Drawing.Point(12, 253);
            this.dgvAdmission.Name = "dgvAdmission";
            this.dgvAdmission.Size = new System.Drawing.Size(1286, 316);
            this.dgvAdmission.TabIndex = 0;
            // 
            // txtRoomSearch
            // 
            this.txtRoomSearch.Location = new System.Drawing.Point(484, 74);
            this.txtRoomSearch.Name = "txtRoomSearch";
            this.txtRoomSearch.Size = new System.Drawing.Size(227, 20);
            this.txtRoomSearch.TabIndex = 19;
            // 
            // btnRoomSearch
            // 
            this.btnRoomSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnRoomSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRoomSearch.ForeColor = System.Drawing.Color.White;
            this.btnRoomSearch.Location = new System.Drawing.Point(717, 67);
            this.btnRoomSearch.Name = "btnRoomSearch";
            this.btnRoomSearch.Size = new System.Drawing.Size(83, 24);
            this.btnRoomSearch.TabIndex = 16;
            this.btnRoomSearch.Text = "Search";
            this.btnRoomSearch.UseVisualStyleBackColor = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(390, 74);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Available Rooms:";
            // 
            // txtReasonOfAdmission
            // 
            this.txtReasonOfAdmission.Location = new System.Drawing.Point(484, 99);
            this.txtReasonOfAdmission.Name = "txtReasonOfAdmission";
            this.txtReasonOfAdmission.Size = new System.Drawing.Size(227, 20);
            this.txtReasonOfAdmission.TabIndex = 21;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(369, 106);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(109, 13);
            this.label11.TabIndex = 22;
            this.label11.Text = "Reason of Admission:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(597, 129);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(76, 13);
            this.label12.TabIndex = 24;
            this.label12.Text = "Transferred to:";
            // 
            // txtTransferredRoom
            // 
            this.txtTransferredRoom.Location = new System.Drawing.Point(669, 122);
            this.txtTransferredRoom.Name = "txtTransferredRoom";
            this.txtTransferredRoom.Size = new System.Drawing.Size(151, 20);
            this.txtTransferredRoom.TabIndex = 25;
            // 
            // cmbDoctor
            // 
            this.cmbDoctor.FormattingEnabled = true;
            this.cmbDoctor.Location = new System.Drawing.Point(124, 66);
            this.cmbDoctor.Name = "cmbDoctor";
            this.cmbDoctor.Size = new System.Drawing.Size(205, 21);
            this.cmbDoctor.TabIndex = 26;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(94, 74);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(24, 13);
            this.label13.TabIndex = 27;
            this.label13.Text = "Dr.:";
            // 
            // cmbNurse
            // 
            this.cmbNurse.FormattingEnabled = true;
            this.cmbNurse.Location = new System.Drawing.Point(124, 93);
            this.cmbNurse.Name = "cmbNurse";
            this.cmbNurse.Size = new System.Drawing.Size(205, 21);
            this.cmbNurse.TabIndex = 28;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(57, 99);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(61, 13);
            this.label14.TabIndex = 29;
            this.label14.Text = "Assistance:";
            // 
            // cmbServices
            // 
            this.cmbServices.FormattingEnabled = true;
            this.cmbServices.Location = new System.Drawing.Point(124, 122);
            this.cmbServices.Name = "cmbServices";
            this.cmbServices.Size = new System.Drawing.Size(205, 21);
            this.cmbServices.TabIndex = 30;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(57, 129);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(46, 13);
            this.label15.TabIndex = 31;
            this.label15.Text = "Service:";
            // 
            // Attendant__Nurse_Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(1310, 581);
            this.Controls.Add(this.dgvAdmission);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnAdd);
            this.Name = "Attendant__Nurse_Dashboard";
            this.Text = "Attendant__Nurse_Dashboard";
            this.Load += new System.EventHandler(this.Attendant__Nurse_Dashboard_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdmission)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbRoom;
        private System.Windows.Forms.ComboBox cmbPatientName;
        private System.Windows.Forms.TextBox txtPatientID;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.DateTimePicker dtpAdmissionDate;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridView dgvAdmission;
        private System.Windows.Forms.TextBox txtAdmissionID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnRoomSearch;
        private System.Windows.Forms.TextBox txtRoomSearch;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtReasonOfAdmission;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtTransferredRoom;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cmbDoctor;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cmbNurse;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cmbServices;
    }
}