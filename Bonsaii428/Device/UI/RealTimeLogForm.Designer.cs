namespace ZDC2911Demo.UI {
    partial class RealTimeLogForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.lbl_Port = new System.Windows.Forms.Label();
            this.btn_Listen = new System.Windows.Forms.Button();
            this.lvw_Logs = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nud_Port = new System.Windows.Forms.NumericUpDown();
            this.lbl_Mode = new System.Windows.Forms.Label();
            this.cbo_Mode = new System.Windows.Forms.ComboBox();
            this.lbl_SerialPort = new System.Windows.Forms.Label();
            this.cbo_SerialPort = new System.Windows.Forms.ComboBox();
            this.lbl_Baudrate = new System.Windows.Forms.Label();
            this.cbo_Baudrate = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Port)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_Port
            // 
            this.lbl_Port.AutoSize = true;
            this.lbl_Port.Location = new System.Drawing.Point(207, 14);
            this.lbl_Port.Name = "lbl_Port";
            this.lbl_Port.Size = new System.Drawing.Size(59, 12);
            this.lbl_Port.TabIndex = 2;
            this.lbl_Port.Text = "UDP Port:";
            this.lbl_Port.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_Listen
            // 
            this.btn_Listen.Location = new System.Drawing.Point(761, 8);
            this.btn_Listen.Name = "btn_Listen";
            this.btn_Listen.Size = new System.Drawing.Size(75, 23);
            this.btn_Listen.TabIndex = 8;
            this.btn_Listen.Text = "Listen";
            this.btn_Listen.UseVisualStyleBackColor = true;
            this.btn_Listen.Click += new System.EventHandler(this.btn_Listen_Click);
            // 
            // lvw_Logs
            // 
            this.lvw_Logs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.lvw_Logs.FullRowSelect = true;
            this.lvw_Logs.GridLines = true;
            this.lvw_Logs.HideSelection = false;
            this.lvw_Logs.Location = new System.Drawing.Point(12, 39);
            this.lvw_Logs.MultiSelect = false;
            this.lvw_Logs.Name = "lvw_Logs";
            this.lvw_Logs.Size = new System.Drawing.Size(824, 503);
            this.lvw_Logs.TabIndex = 9;
            this.lvw_Logs.UseCompatibleStateImageBehavior = false;
            this.lvw_Logs.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "SN";
            this.columnHeader1.Width = 40;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Device No.";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 90;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "UserID";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 120;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Name";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 100;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Status";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader5.Width = 120;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Action";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader6.Width = 150;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Datetime";
            this.columnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader7.Width = 150;
            // 
            // nud_Port
            // 
            this.nud_Port.Location = new System.Drawing.Point(272, 10);
            this.nud_Port.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nud_Port.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_Port.Name = "nud_Port";
            this.nud_Port.Size = new System.Drawing.Size(57, 21);
            this.nud_Port.TabIndex = 3;
            this.nud_Port.Value = new decimal(new int[] {
            5055,
            0,
            0,
            0});
            // 
            // lbl_Mode
            // 
            this.lbl_Mode.AutoSize = true;
            this.lbl_Mode.Location = new System.Drawing.Point(12, 15);
            this.lbl_Mode.Name = "lbl_Mode";
            this.lbl_Mode.Size = new System.Drawing.Size(83, 12);
            this.lbl_Mode.TabIndex = 0;
            this.lbl_Mode.Text = "Monitor Mode:";
            this.lbl_Mode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbo_Mode
            // 
            this.cbo_Mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_Mode.FormattingEnabled = true;
            this.cbo_Mode.Items.AddRange(new object[] {
            "UDP",
            "RS485"});
            this.cbo_Mode.Location = new System.Drawing.Point(101, 11);
            this.cbo_Mode.Name = "cbo_Mode";
            this.cbo_Mode.Size = new System.Drawing.Size(80, 20);
            this.cbo_Mode.TabIndex = 1;
            this.cbo_Mode.SelectedIndexChanged += new System.EventHandler(this.cbo_Mode_SelectedIndexChanged);
            // 
            // lbl_SerialPort
            // 
            this.lbl_SerialPort.AutoSize = true;
            this.lbl_SerialPort.Location = new System.Drawing.Point(356, 15);
            this.lbl_SerialPort.Name = "lbl_SerialPort";
            this.lbl_SerialPort.Size = new System.Drawing.Size(77, 12);
            this.lbl_SerialPort.TabIndex = 4;
            this.lbl_SerialPort.Text = "Serial Port:";
            this.lbl_SerialPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbo_SerialPort
            // 
            this.cbo_SerialPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_SerialPort.FormattingEnabled = true;
            this.cbo_SerialPort.Location = new System.Drawing.Point(439, 11);
            this.cbo_SerialPort.Name = "cbo_SerialPort";
            this.cbo_SerialPort.Size = new System.Drawing.Size(70, 20);
            this.cbo_SerialPort.TabIndex = 5;
            // 
            // lbl_Baudrate
            // 
            this.lbl_Baudrate.AutoSize = true;
            this.lbl_Baudrate.Location = new System.Drawing.Point(534, 14);
            this.lbl_Baudrate.Name = "lbl_Baudrate";
            this.lbl_Baudrate.Size = new System.Drawing.Size(59, 12);
            this.lbl_Baudrate.TabIndex = 6;
            this.lbl_Baudrate.Text = "Baudrate:";
            this.lbl_Baudrate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbo_Baudrate
            // 
            this.cbo_Baudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_Baudrate.FormattingEnabled = true;
            this.cbo_Baudrate.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cbo_Baudrate.Location = new System.Drawing.Point(599, 12);
            this.cbo_Baudrate.Name = "cbo_Baudrate";
            this.cbo_Baudrate.Size = new System.Drawing.Size(121, 20);
            this.cbo_Baudrate.TabIndex = 7;
            // 
            // RealTimeLogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(848, 554);
            this.Controls.Add(this.cbo_Baudrate);
            this.Controls.Add(this.lbl_Baudrate);
            this.Controls.Add(this.cbo_SerialPort);
            this.Controls.Add(this.lbl_SerialPort);
            this.Controls.Add(this.cbo_Mode);
            this.Controls.Add(this.lbl_Mode);
            this.Controls.Add(this.nud_Port);
            this.Controls.Add(this.lvw_Logs);
            this.Controls.Add(this.btn_Listen);
            this.Controls.Add(this.lbl_Port);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "RealTimeLogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Real Time Log";
            this.Load += new System.EventHandler(this.RealTimeLogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nud_Port)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_Port;
        private System.Windows.Forms.Button btn_Listen;
        private System.Windows.Forms.ListView lvw_Logs;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.NumericUpDown nud_Port;
        private System.Windows.Forms.Label lbl_Mode;
        private System.Windows.Forms.ComboBox cbo_Mode;
        private System.Windows.Forms.Label lbl_SerialPort;
        private System.Windows.Forms.ComboBox cbo_SerialPort;
        private System.Windows.Forms.Label lbl_Baudrate;
        private System.Windows.Forms.ComboBox cbo_Baudrate;
    }
}