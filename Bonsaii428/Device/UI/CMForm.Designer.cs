namespace ZDC2911Demo.UI {
    partial class CMForm {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.p2pServerTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.p2pAddrTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.p2pRadioButton = new System.Windows.Forms.RadioButton();
            this.nud_Port = new System.Windows.Forms.NumericUpDown();
            this.nud_Pwd = new System.Windows.Forms.NumericUpDown();
            this.nud_DN = new System.Windows.Forms.NumericUpDown();
            this.cbo_BaudRate = new System.Windows.Forms.ComboBox();
            this.cbo_COMM = new System.Windows.Forms.ComboBox();
            this.lbl_BaudRate = new System.Windows.Forms.Label();
            this.lbl_COMM = new System.Windows.Forms.Label();
            this.rdb_TCP = new System.Windows.Forms.RadioButton();
            this.rdb_COMM = new System.Windows.Forms.RadioButton();
            this.rdb_USB = new System.Windows.Forms.RadioButton();
            this.txt_IP = new System.Windows.Forms.TextBox();
            this.lbl_Port = new System.Windows.Forms.Label();
            this.lbl_IP = new System.Windows.Forms.Label();
            this.lbl_ConnectionPwd = new System.Windows.Forms.Label();
            this.lbl_DeviceId = new System.Windows.Forms.Label();
            this.btn_OpenDevice = new System.Windows.Forms.Button();
            this.btn_CloseDevice = new System.Windows.Forms.Button();
            this.group = new System.Windows.Forms.GroupBox();
            this.btn_GlogManagement = new System.Windows.Forms.Button();
            this.btn_SlogManagement = new System.Windows.Forms.Button();
            this.btn_AlarmSetting = new System.Windows.Forms.Button();
            this.btn_AttendanceSetting = new System.Windows.Forms.Button();
            this.btn_EnrollManagement = new System.Windows.Forms.Button();
            this.btn_AccessSetting = new System.Windows.Forms.Button();
            this.btn_SystemSetting = new System.Windows.Forms.Button();
            this.btn_RealTimeLog = new System.Windows.Forms.Button();
            this.btn_NameDataSetting = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Port)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Pwd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_DN)).BeginInit();
            this.group.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.p2pServerTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.p2pAddrTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.p2pRadioButton);
            this.groupBox1.Controls.Add(this.nud_Port);
            this.groupBox1.Controls.Add(this.nud_Pwd);
            this.groupBox1.Controls.Add(this.nud_DN);
            this.groupBox1.Controls.Add(this.cbo_BaudRate);
            this.groupBox1.Controls.Add(this.cbo_COMM);
            this.groupBox1.Controls.Add(this.lbl_BaudRate);
            this.groupBox1.Controls.Add(this.lbl_COMM);
            this.groupBox1.Controls.Add(this.rdb_TCP);
            this.groupBox1.Controls.Add(this.rdb_COMM);
            this.groupBox1.Controls.Add(this.rdb_USB);
            this.groupBox1.Controls.Add(this.txt_IP);
            this.groupBox1.Controls.Add(this.lbl_Port);
            this.groupBox1.Controls.Add(this.lbl_IP);
            this.groupBox1.Controls.Add(this.lbl_ConnectionPwd);
            this.groupBox1.Controls.Add(this.lbl_DeviceId);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(238, 309);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection Information";
            // 
            // p2pServerTextBox
            // 
            this.p2pServerTextBox.Location = new System.Drawing.Point(82, 285);
            this.p2pServerTextBox.Name = "p2pServerTextBox";
            this.p2pServerTextBox.Size = new System.Drawing.Size(144, 21);
            this.p2pServerTextBox.TabIndex = 22;
            this.p2pServerTextBox.Text = "s1.weixinac.com";
            this.p2pServerTextBox.TextChanged += new System.EventHandler(this.p2pServerTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 289);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 21;
            this.label2.Text = "P2P Server:";
            // 
            // p2pAddrTextBox
            // 
            this.p2pAddrTextBox.Location = new System.Drawing.Point(82, 257);
            this.p2pAddrTextBox.Name = "p2pAddrTextBox";
            this.p2pAddrTextBox.Size = new System.Drawing.Size(144, 21);
            this.p2pAddrTextBox.TabIndex = 20;
            this.p2pAddrTextBox.TextChanged += new System.EventHandler(this.p2pAddrTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 260);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 19;
            this.label1.Text = "P2P Addr:";
            // 
            // p2pRadioButton
            // 
            this.p2pRadioButton.AutoSize = true;
            this.p2pRadioButton.Location = new System.Drawing.Point(6, 238);
            this.p2pRadioButton.Name = "p2pRadioButton";
            this.p2pRadioButton.Size = new System.Drawing.Size(41, 16);
            this.p2pRadioButton.TabIndex = 18;
            this.p2pRadioButton.TabStop = true;
            this.p2pRadioButton.Text = "P2P";
            this.p2pRadioButton.UseVisualStyleBackColor = true;
            this.p2pRadioButton.CheckedChanged += new System.EventHandler(this.p2pRadioButton_CheckedChanged);
            // 
            // nud_Port
            // 
            this.nud_Port.Location = new System.Drawing.Point(82, 213);
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
            this.nud_Port.Size = new System.Drawing.Size(144, 21);
            this.nud_Port.TabIndex = 14;
            this.nud_Port.Value = new decimal(new int[] {
            5500,
            0,
            0,
            0});
            this.nud_Port.ValueChanged += new System.EventHandler(this.nud_Port_ValueChanged);
            // 
            // nud_Pwd
            // 
            this.nud_Pwd.Location = new System.Drawing.Point(82, 41);
            this.nud_Pwd.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nud_Pwd.Name = "nud_Pwd";
            this.nud_Pwd.Size = new System.Drawing.Size(120, 21);
            this.nud_Pwd.TabIndex = 3;
            // 
            // nud_DN
            // 
            this.nud_DN.Location = new System.Drawing.Point(82, 14);
            this.nud_DN.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nud_DN.Name = "nud_DN";
            this.nud_DN.Size = new System.Drawing.Size(120, 21);
            this.nud_DN.TabIndex = 1;
            this.nud_DN.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbo_BaudRate
            // 
            this.cbo_BaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_BaudRate.FormattingEnabled = true;
            this.cbo_BaudRate.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cbo_BaudRate.Location = new System.Drawing.Point(82, 138);
            this.cbo_BaudRate.Name = "cbo_BaudRate";
            this.cbo_BaudRate.Size = new System.Drawing.Size(120, 20);
            this.cbo_BaudRate.TabIndex = 9;
            // 
            // cbo_COMM
            // 
            this.cbo_COMM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_COMM.FormattingEnabled = true;
            this.cbo_COMM.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8"});
            this.cbo_COMM.Location = new System.Drawing.Point(82, 112);
            this.cbo_COMM.Name = "cbo_COMM";
            this.cbo_COMM.Size = new System.Drawing.Size(120, 20);
            this.cbo_COMM.TabIndex = 7;
            // 
            // lbl_BaudRate
            // 
            this.lbl_BaudRate.AutoSize = true;
            this.lbl_BaudRate.Location = new System.Drawing.Point(13, 141);
            this.lbl_BaudRate.Name = "lbl_BaudRate";
            this.lbl_BaudRate.Size = new System.Drawing.Size(59, 12);
            this.lbl_BaudRate.TabIndex = 8;
            this.lbl_BaudRate.Text = "Baudrate:";
            this.lbl_BaudRate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_COMM
            // 
            this.lbl_COMM.AutoSize = true;
            this.lbl_COMM.Location = new System.Drawing.Point(13, 115);
            this.lbl_COMM.Name = "lbl_COMM";
            this.lbl_COMM.Size = new System.Drawing.Size(53, 12);
            this.lbl_COMM.TabIndex = 6;
            this.lbl_COMM.Text = "ComPort:";
            this.lbl_COMM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rdb_TCP
            // 
            this.rdb_TCP.AutoSize = true;
            this.rdb_TCP.Location = new System.Drawing.Point(8, 164);
            this.rdb_TCP.Name = "rdb_TCP";
            this.rdb_TCP.Size = new System.Drawing.Size(131, 16);
            this.rdb_TCP.TabIndex = 10;
            this.rdb_TCP.TabStop = true;
            this.rdb_TCP.Text = "Network Connection";
            this.rdb_TCP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdb_TCP.UseVisualStyleBackColor = true;
            this.rdb_TCP.CheckedChanged += new System.EventHandler(this.rdb_TCP_CheckedChanged);
            // 
            // rdb_COMM
            // 
            this.rdb_COMM.AutoSize = true;
            this.rdb_COMM.Location = new System.Drawing.Point(8, 90);
            this.rdb_COMM.Name = "rdb_COMM";
            this.rdb_COMM.Size = new System.Drawing.Size(125, 16);
            this.rdb_COMM.TabIndex = 5;
            this.rdb_COMM.TabStop = true;
            this.rdb_COMM.Text = "Serial Connection";
            this.rdb_COMM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdb_COMM.UseVisualStyleBackColor = true;
            // 
            // rdb_USB
            // 
            this.rdb_USB.AutoSize = true;
            this.rdb_USB.Checked = true;
            this.rdb_USB.Location = new System.Drawing.Point(8, 68);
            this.rdb_USB.Name = "rdb_USB";
            this.rdb_USB.Size = new System.Drawing.Size(107, 16);
            this.rdb_USB.TabIndex = 4;
            this.rdb_USB.TabStop = true;
            this.rdb_USB.Text = "USB Connection";
            this.rdb_USB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdb_USB.UseVisualStyleBackColor = true;
            // 
            // txt_IP
            // 
            this.txt_IP.Location = new System.Drawing.Point(82, 186);
            this.txt_IP.Name = "txt_IP";
            this.txt_IP.Size = new System.Drawing.Size(144, 21);
            this.txt_IP.TabIndex = 12;
            this.txt_IP.Text = "192.168.1.225";
            this.txt_IP.TextChanged += new System.EventHandler(this.txt_IP_TextChanged);
            // 
            // lbl_Port
            // 
            this.lbl_Port.AutoSize = true;
            this.lbl_Port.Location = new System.Drawing.Point(13, 215);
            this.lbl_Port.Name = "lbl_Port";
            this.lbl_Port.Size = new System.Drawing.Size(59, 12);
            this.lbl_Port.TabIndex = 13;
            this.lbl_Port.Text = "UDP Port:";
            this.lbl_Port.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_IP
            // 
            this.lbl_IP.AutoSize = true;
            this.lbl_IP.Location = new System.Drawing.Point(6, 190);
            this.lbl_IP.Name = "lbl_IP";
            this.lbl_IP.Size = new System.Drawing.Size(71, 12);
            this.lbl_IP.TabIndex = 11;
            this.lbl_IP.Text = "IP Address:";
            this.lbl_IP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_ConnectionPwd
            // 
            this.lbl_ConnectionPwd.AutoSize = true;
            this.lbl_ConnectionPwd.Location = new System.Drawing.Point(13, 43);
            this.lbl_ConnectionPwd.Name = "lbl_ConnectionPwd";
            this.lbl_ConnectionPwd.Size = new System.Drawing.Size(59, 12);
            this.lbl_ConnectionPwd.TabIndex = 2;
            this.lbl_ConnectionPwd.Text = "Comm Key:";
            this.lbl_ConnectionPwd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_DeviceId
            // 
            this.lbl_DeviceId.AutoSize = true;
            this.lbl_DeviceId.Location = new System.Drawing.Point(13, 17);
            this.lbl_DeviceId.Name = "lbl_DeviceId";
            this.lbl_DeviceId.Size = new System.Drawing.Size(65, 12);
            this.lbl_DeviceId.TabIndex = 0;
            this.lbl_DeviceId.Text = "Device ID:";
            this.lbl_DeviceId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_OpenDevice
            // 
            this.btn_OpenDevice.Location = new System.Drawing.Point(256, 24);
            this.btn_OpenDevice.Name = "btn_OpenDevice";
            this.btn_OpenDevice.Size = new System.Drawing.Size(95, 23);
            this.btn_OpenDevice.TabIndex = 1;
            this.btn_OpenDevice.Text = "Open Device";
            this.btn_OpenDevice.UseVisualStyleBackColor = true;
            this.btn_OpenDevice.Click += new System.EventHandler(this.btn_OpenDevice_Click);
            // 
            // btn_CloseDevice
            // 
            this.btn_CloseDevice.Location = new System.Drawing.Point(357, 24);
            this.btn_CloseDevice.Name = "btn_CloseDevice";
            this.btn_CloseDevice.Size = new System.Drawing.Size(95, 23);
            this.btn_CloseDevice.TabIndex = 2;
            this.btn_CloseDevice.Text = "Close Device";
            this.btn_CloseDevice.UseVisualStyleBackColor = true;
            this.btn_CloseDevice.Click += new System.EventHandler(this.btn_CloseDevice_Click);
            // 
            // group
            // 
            this.group.Controls.Add(this.btn_GlogManagement);
            this.group.Controls.Add(this.btn_SlogManagement);
            this.group.Controls.Add(this.btn_AlarmSetting);
            this.group.Controls.Add(this.btn_AttendanceSetting);
            this.group.Controls.Add(this.btn_EnrollManagement);
            this.group.Controls.Add(this.btn_AccessSetting);
            this.group.Controls.Add(this.btn_SystemSetting);
            this.group.Location = new System.Drawing.Point(256, 53);
            this.group.Name = "group";
            this.group.Size = new System.Drawing.Size(333, 139);
            this.group.TabIndex = 3;
            this.group.TabStop = false;
            this.group.Text = "Function";
            // 
            // btn_GlogManagement
            // 
            this.btn_GlogManagement.Location = new System.Drawing.Point(6, 107);
            this.btn_GlogManagement.Name = "btn_GlogManagement";
            this.btn_GlogManagement.Size = new System.Drawing.Size(156, 23);
            this.btn_GlogManagement.TabIndex = 6;
            this.btn_GlogManagement.Text = "GLog Management";
            this.btn_GlogManagement.UseVisualStyleBackColor = true;
            this.btn_GlogManagement.Click += new System.EventHandler(this.btn_GlogManagement_Click);
            // 
            // btn_SlogManagement
            // 
            this.btn_SlogManagement.Location = new System.Drawing.Point(168, 78);
            this.btn_SlogManagement.Name = "btn_SlogManagement";
            this.btn_SlogManagement.Size = new System.Drawing.Size(156, 23);
            this.btn_SlogManagement.TabIndex = 5;
            this.btn_SlogManagement.Text = "SLog Mangement";
            this.btn_SlogManagement.UseVisualStyleBackColor = true;
            this.btn_SlogManagement.Click += new System.EventHandler(this.btn_SlogManagement_Click);
            // 
            // btn_AlarmSetting
            // 
            this.btn_AlarmSetting.Location = new System.Drawing.Point(168, 20);
            this.btn_AlarmSetting.Name = "btn_AlarmSetting";
            this.btn_AlarmSetting.Size = new System.Drawing.Size(156, 23);
            this.btn_AlarmSetting.TabIndex = 1;
            this.btn_AlarmSetting.Text = "Alarm Settings";
            this.btn_AlarmSetting.UseVisualStyleBackColor = true;
            this.btn_AlarmSetting.Click += new System.EventHandler(this.btn_AlarmSetting_Click);
            // 
            // btn_AttendanceSetting
            // 
            this.btn_AttendanceSetting.Location = new System.Drawing.Point(168, 49);
            this.btn_AttendanceSetting.Name = "btn_AttendanceSetting";
            this.btn_AttendanceSetting.Size = new System.Drawing.Size(156, 23);
            this.btn_AttendanceSetting.TabIndex = 3;
            this.btn_AttendanceSetting.Text = "Message Settings";
            this.btn_AttendanceSetting.UseVisualStyleBackColor = true;
            this.btn_AttendanceSetting.Click += new System.EventHandler(this.btn_AttendanceSetting_Click);
            // 
            // btn_EnrollManagement
            // 
            this.btn_EnrollManagement.Location = new System.Drawing.Point(6, 78);
            this.btn_EnrollManagement.Name = "btn_EnrollManagement";
            this.btn_EnrollManagement.Size = new System.Drawing.Size(156, 23);
            this.btn_EnrollManagement.TabIndex = 4;
            this.btn_EnrollManagement.Text = "Employee Management";
            this.btn_EnrollManagement.UseVisualStyleBackColor = true;
            this.btn_EnrollManagement.Click += new System.EventHandler(this.btn_EnrollManagement_Click);
            // 
            // btn_AccessSetting
            // 
            this.btn_AccessSetting.Location = new System.Drawing.Point(6, 49);
            this.btn_AccessSetting.Name = "btn_AccessSetting";
            this.btn_AccessSetting.Size = new System.Drawing.Size(156, 23);
            this.btn_AccessSetting.TabIndex = 2;
            this.btn_AccessSetting.Text = "Access Control Settings";
            this.btn_AccessSetting.UseVisualStyleBackColor = true;
            this.btn_AccessSetting.Click += new System.EventHandler(this.btn_AccessSetting_Click);
            // 
            // btn_SystemSetting
            // 
            this.btn_SystemSetting.Location = new System.Drawing.Point(6, 20);
            this.btn_SystemSetting.Name = "btn_SystemSetting";
            this.btn_SystemSetting.Size = new System.Drawing.Size(156, 23);
            this.btn_SystemSetting.TabIndex = 0;
            this.btn_SystemSetting.Text = "System Settings";
            this.btn_SystemSetting.UseVisualStyleBackColor = true;
            this.btn_SystemSetting.Click += new System.EventHandler(this.btn_SystemSetting_Click);
            // 
            // btn_RealTimeLog
            // 
            this.btn_RealTimeLog.Location = new System.Drawing.Point(262, 197);
            this.btn_RealTimeLog.Name = "btn_RealTimeLog";
            this.btn_RealTimeLog.Size = new System.Drawing.Size(156, 23);
            this.btn_RealTimeLog.TabIndex = 4;
            this.btn_RealTimeLog.Text = "Real Time Log";
            this.btn_RealTimeLog.UseVisualStyleBackColor = true;
            this.btn_RealTimeLog.Click += new System.EventHandler(this.btn_RealTimeLog_Click);
            // 
            // btn_NameDataSetting
            // 
            this.btn_NameDataSetting.Location = new System.Drawing.Point(424, 196);
            this.btn_NameDataSetting.Name = "btn_NameDataSetting";
            this.btn_NameDataSetting.Size = new System.Drawing.Size(156, 23);
            this.btn_NameDataSetting.TabIndex = 5;
            this.btn_NameDataSetting.Text = "Name Data Settings";
            this.btn_NameDataSetting.UseVisualStyleBackColor = true;
            this.btn_NameDataSetting.Click += new System.EventHandler(this.btn_NameDataSetting_Click);
            // 
            // CMForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 332);
            this.Controls.Add(this.btn_NameDataSetting);
            this.Controls.Add(this.btn_RealTimeLog);
            this.Controls.Add(this.group);
            this.Controls.Add(this.btn_CloseDevice);
            this.Controls.Add(this.btn_OpenDevice);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "CMForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ZDC2911Demo";
            this.Load += new System.EventHandler(this.CMForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Port)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Pwd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_DN)).EndInit();
            this.group.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbo_BaudRate;
        private System.Windows.Forms.ComboBox cbo_COMM;
        private System.Windows.Forms.Label lbl_BaudRate;
        private System.Windows.Forms.Label lbl_COMM;
        private System.Windows.Forms.RadioButton rdb_TCP;
        private System.Windows.Forms.RadioButton rdb_COMM;
        private System.Windows.Forms.RadioButton rdb_USB;
        private System.Windows.Forms.TextBox txt_IP;
        private System.Windows.Forms.Label lbl_Port;
        private System.Windows.Forms.Label lbl_IP;
        private System.Windows.Forms.Label lbl_ConnectionPwd;
        private System.Windows.Forms.Label lbl_DeviceId;
        private System.Windows.Forms.Button btn_OpenDevice;
        private System.Windows.Forms.Button btn_CloseDevice;
        private System.Windows.Forms.GroupBox group;
        private System.Windows.Forms.Button btn_GlogManagement;
        private System.Windows.Forms.Button btn_SlogManagement;
        private System.Windows.Forms.Button btn_AlarmSetting;
        private System.Windows.Forms.Button btn_AttendanceSetting;
        private System.Windows.Forms.Button btn_EnrollManagement;
        private System.Windows.Forms.Button btn_AccessSetting;
        private System.Windows.Forms.Button btn_SystemSetting;
        private System.Windows.Forms.Button btn_RealTimeLog;
        private System.Windows.Forms.Button btn_NameDataSetting;
        private System.Windows.Forms.NumericUpDown nud_Pwd;
        private System.Windows.Forms.NumericUpDown nud_DN;
        private System.Windows.Forms.NumericUpDown nud_Port;
        private System.Windows.Forms.TextBox p2pAddrTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton p2pRadioButton;
        private System.Windows.Forms.TextBox p2pServerTextBox;
        private System.Windows.Forms.Label label2;
    }
}

