namespace ZDC2911Demo.UI {
    partial class SystemSettingForm {
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_ParamGet = new System.Windows.Forms.Button();
            this.btn_ParamSet = new System.Windows.Forms.Button();
            this.txt_ParamValue = new System.Windows.Forms.TextBox();
            this.cbo_Param = new System.Windows.Forms.ComboBox();
            this.lbl_ParamValue = new System.Windows.Forms.Label();
            this.lbl_Param = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chk_Power = new System.Windows.Forms.CheckBox();
            this.btn_PowerGet = new System.Windows.Forms.Button();
            this.btn_PowerSet = new System.Windows.Forms.Button();
            this.cbo_PowerMinute = new System.Windows.Forms.ComboBox();
            this.cbo_PowerHour = new System.Windows.Forms.ComboBox();
            this.cbo_DN = new System.Windows.Forms.ComboBox();
            this.cbo_PowerType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_Status = new System.Windows.Forms.Label();
            this.lbl_Time = new System.Windows.Forms.Label();
            this.lbl_DN = new System.Windows.Forms.Label();
            this.lbl_PowerType = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txt_DeviceTime = new System.Windows.Forms.TextBox();
            this.lbl_DeviceTime = new System.Windows.Forms.Label();
            this.btn_TimeGet = new System.Windows.Forms.Button();
            this.btn_TimeSet = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btn_SystemGet = new System.Windows.Forms.Button();
            this.txt_PwdCount = new System.Windows.Forms.TextBox();
            this.txt_CardCount = new System.Windows.Forms.TextBox();
            this.txt_HisManageCount = new System.Windows.Forms.TextBox();
            this.txt_ManageCount = new System.Windows.Forms.TextBox();
            this.txt_HisExitCount = new System.Windows.Forms.TextBox();
            this.txt_ExitCount = new System.Windows.Forms.TextBox();
            this.txt_AdminCount = new System.Windows.Forms.TextBox();
            this.txt_FingerCount = new System.Windows.Forms.TextBox();
            this.lbl_PwdCount = new System.Windows.Forms.Label();
            this.lbl_CardCount = new System.Windows.Forms.Label();
            this.lbl_HisManageCount = new System.Windows.Forms.Label();
            this.lbl_HisExitCount = new System.Windows.Forms.Label();
            this.lbl_ManageCount = new System.Windows.Forms.Label();
            this.lbl_ExitCount = new System.Windows.Forms.Label();
            this.txt_UserCount = new System.Windows.Forms.TextBox();
            this.lbl_AdminCount = new System.Windows.Forms.Label();
            this.lbl_FingerCount = new System.Windows.Forms.Label();
            this.lbl_UserCount = new System.Windows.Forms.Label();
            this.btn_MacAddress = new System.Windows.Forms.Button();
            this.btn_Reset = new System.Windows.Forms.Button();
            this.txt_Welcome = new System.Windows.Forms.TextBox();
            this.btn_WelcomeGet = new System.Windows.Forms.Button();
            this.btn_WelcomeSet = new System.Windows.Forms.Button();
            this.lbl_WelCome = new System.Windows.Forms.Label();
            this.txt_Standby = new System.Windows.Forms.TextBox();
            this.btn_StandbyGet = new System.Windows.Forms.Button();
            this.btn_StandbySet = new System.Windows.Forms.Button();
            this.lbl_Standby = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btn_ModelSet = new System.Windows.Forms.Button();
            this.btn_ModelGet = new System.Windows.Forms.Button();
            this.txt_Model = new System.Windows.Forms.TextBox();
            this.getSerialNoButton = new System.Windows.Forms.Button();
            this.btn_FirmwareVersionGet = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_Model = new System.Windows.Forms.Label();
            this.serialNoTextBox = new System.Windows.Forms.TextBox();
            this.txt_FirmwareVersion = new System.Windows.Forms.TextBox();
            this.lbl_FirmwareVersion = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btn_UploadSound = new System.Windows.Forms.Button();
            this.cbo_SoundType = new System.Windows.Forms.ComboBox();
            this.lbl_SoundType = new System.Windows.Forms.Label();
            this.restartButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_ParamGet);
            this.groupBox1.Controls.Add(this.btn_ParamSet);
            this.groupBox1.Controls.Add(this.txt_ParamValue);
            this.groupBox1.Controls.Add(this.cbo_Param);
            this.groupBox1.Controls.Add(this.lbl_ParamValue);
            this.groupBox1.Controls.Add(this.lbl_Param);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(289, 113);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "System Parameter Settings";
            // 
            // btn_ParamGet
            // 
            this.btn_ParamGet.Location = new System.Drawing.Point(121, 79);
            this.btn_ParamGet.Name = "btn_ParamGet";
            this.btn_ParamGet.Size = new System.Drawing.Size(75, 25);
            this.btn_ParamGet.TabIndex = 4;
            this.btn_ParamGet.Text = "Get";
            this.btn_ParamGet.UseVisualStyleBackColor = true;
            this.btn_ParamGet.Click += new System.EventHandler(this.btn_ParamGet_Click);
            // 
            // btn_ParamSet
            // 
            this.btn_ParamSet.Location = new System.Drawing.Point(202, 79);
            this.btn_ParamSet.Name = "btn_ParamSet";
            this.btn_ParamSet.Size = new System.Drawing.Size(75, 25);
            this.btn_ParamSet.TabIndex = 5;
            this.btn_ParamSet.Text = "Set";
            this.btn_ParamSet.UseVisualStyleBackColor = true;
            this.btn_ParamSet.Click += new System.EventHandler(this.btn_ParamSet_Click);
            // 
            // txt_ParamValue
            // 
            this.txt_ParamValue.Location = new System.Drawing.Point(77, 50);
            this.txt_ParamValue.Name = "txt_ParamValue";
            this.txt_ParamValue.Size = new System.Drawing.Size(200, 20);
            this.txt_ParamValue.TabIndex = 3;
            // 
            // cbo_Param
            // 
            this.cbo_Param.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_Param.FormattingEnabled = true;
            this.cbo_Param.Items.AddRange(new object[] {
            "Admin Count",
            "Language Format",
            "ID Length",
            "Volume Size",
            "Auto Off Time",
            "Auto Power On",
            "Verify Mode",
            "Auto Learning",
            "Auto Return Time",
            "Standby Time",
            "Enable Alarm In Standby",
            "Card ID Type",
            "Auto Restart",
            "Enable Shutdown",
            "Enable Relay Alarm",
            "Fire Alarm",
            "One To One Security Level",
            "One To N Security Level",
            "SLog Warning Count",
            "GLog Warning Count",
            "Reverify Time",
            "Device ID",
            "Baudrate",
            "User Real Time Log",
            "UDP Port",
            "Device Password",
            "IP Address",
            "Sub Net Address",
            "Default Gate",
            "Server IP Address",
            "Server UDP Port",
            "RS485 Use",
            "Lock1 Delay Time",
            "Wiegand Mode",
            "Lock1 Check Door State",
            "Hijack OpenDoor",
            "Hijack Alarm",
            "Locks Enabled"});
            this.cbo_Param.Location = new System.Drawing.Point(77, 22);
            this.cbo_Param.Name = "cbo_Param";
            this.cbo_Param.Size = new System.Drawing.Size(200, 21);
            this.cbo_Param.TabIndex = 1;
            // 
            // lbl_ParamValue
            // 
            this.lbl_ParamValue.AutoSize = true;
            this.lbl_ParamValue.Location = new System.Drawing.Point(30, 53);
            this.lbl_ParamValue.Name = "lbl_ParamValue";
            this.lbl_ParamValue.Size = new System.Drawing.Size(37, 13);
            this.lbl_ParamValue.TabIndex = 2;
            this.lbl_ParamValue.Text = "Value:";
            this.lbl_ParamValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Param
            // 
            this.lbl_Param.AutoSize = true;
            this.lbl_Param.Location = new System.Drawing.Point(6, 25);
            this.lbl_Param.Name = "lbl_Param";
            this.lbl_Param.Size = new System.Drawing.Size(58, 13);
            this.lbl_Param.TabIndex = 0;
            this.lbl_Param.Text = "Parameter:";
            this.lbl_Param.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chk_Power);
            this.groupBox2.Controls.Add(this.btn_PowerGet);
            this.groupBox2.Controls.Add(this.btn_PowerSet);
            this.groupBox2.Controls.Add(this.cbo_PowerMinute);
            this.groupBox2.Controls.Add(this.cbo_PowerHour);
            this.groupBox2.Controls.Add(this.cbo_DN);
            this.groupBox2.Controls.Add(this.cbo_PowerType);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.lbl_Status);
            this.groupBox2.Controls.Add(this.lbl_Time);
            this.groupBox2.Controls.Add(this.lbl_DN);
            this.groupBox2.Controls.Add(this.lbl_PowerType);
            this.groupBox2.Location = new System.Drawing.Point(12, 132);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(225, 169);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Timer On/Off Settings";
            // 
            // chk_Power
            // 
            this.chk_Power.AutoSize = true;
            this.chk_Power.Location = new System.Drawing.Point(47, 106);
            this.chk_Power.Name = "chk_Power";
            this.chk_Power.Size = new System.Drawing.Size(59, 17);
            this.chk_Power.TabIndex = 8;
            this.chk_Power.Text = "Enable";
            this.chk_Power.UseVisualStyleBackColor = true;
            // 
            // btn_PowerGet
            // 
            this.btn_PowerGet.Location = new System.Drawing.Point(32, 130);
            this.btn_PowerGet.Name = "btn_PowerGet";
            this.btn_PowerGet.Size = new System.Drawing.Size(75, 25);
            this.btn_PowerGet.TabIndex = 8;
            this.btn_PowerGet.Text = "Get";
            this.btn_PowerGet.UseVisualStyleBackColor = true;
            this.btn_PowerGet.Click += new System.EventHandler(this.btn_PowerGet_Click);
            // 
            // btn_PowerSet
            // 
            this.btn_PowerSet.Location = new System.Drawing.Point(121, 130);
            this.btn_PowerSet.Name = "btn_PowerSet";
            this.btn_PowerSet.Size = new System.Drawing.Size(75, 25);
            this.btn_PowerSet.TabIndex = 9;
            this.btn_PowerSet.Text = "Set";
            this.btn_PowerSet.UseVisualStyleBackColor = true;
            this.btn_PowerSet.Click += new System.EventHandler(this.btn_PowerSet_Click);
            // 
            // cbo_PowerMinute
            // 
            this.cbo_PowerMinute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_PowerMinute.FormattingEnabled = true;
            this.cbo_PowerMinute.Location = new System.Drawing.Point(136, 78);
            this.cbo_PowerMinute.Name = "cbo_PowerMinute";
            this.cbo_PowerMinute.Size = new System.Drawing.Size(60, 21);
            this.cbo_PowerMinute.TabIndex = 7;
            // 
            // cbo_PowerHour
            // 
            this.cbo_PowerHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_PowerHour.FormattingEnabled = true;
            this.cbo_PowerHour.Location = new System.Drawing.Point(47, 78);
            this.cbo_PowerHour.Name = "cbo_PowerHour";
            this.cbo_PowerHour.Size = new System.Drawing.Size(60, 21);
            this.cbo_PowerHour.TabIndex = 5;
            // 
            // cbo_DN
            // 
            this.cbo_DN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_DN.FormattingEnabled = true;
            this.cbo_DN.Location = new System.Drawing.Point(47, 50);
            this.cbo_DN.Name = "cbo_DN";
            this.cbo_DN.Size = new System.Drawing.Size(149, 21);
            this.cbo_DN.TabIndex = 3;
            // 
            // cbo_PowerType
            // 
            this.cbo_PowerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_PowerType.FormattingEnabled = true;
            this.cbo_PowerType.Items.AddRange(new object[] {
            "Boot",
            "Shutdown"});
            this.cbo_PowerType.Location = new System.Drawing.Point(47, 22);
            this.cbo_PowerType.Name = "cbo_PowerType";
            this.cbo_PowerType.Size = new System.Drawing.Size(149, 21);
            this.cbo_PowerType.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(113, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Status
            // 
            this.lbl_Status.AutoSize = true;
            this.lbl_Status.Location = new System.Drawing.Point(18, 109);
            this.lbl_Status.Name = "lbl_Status";
            this.lbl_Status.Size = new System.Drawing.Size(0, 13);
            this.lbl_Status.TabIndex = 0;
            this.lbl_Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Time
            // 
            this.lbl_Time.AutoSize = true;
            this.lbl_Time.Location = new System.Drawing.Point(6, 80);
            this.lbl_Time.Name = "lbl_Time";
            this.lbl_Time.Size = new System.Drawing.Size(33, 13);
            this.lbl_Time.TabIndex = 4;
            this.lbl_Time.Text = "Time:";
            this.lbl_Time.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_DN
            // 
            this.lbl_DN.AutoSize = true;
            this.lbl_DN.Location = new System.Drawing.Point(18, 53);
            this.lbl_DN.Name = "lbl_DN";
            this.lbl_DN.Size = new System.Drawing.Size(25, 13);
            this.lbl_DN.TabIndex = 2;
            this.lbl_DN.Text = "SN:";
            this.lbl_DN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_PowerType
            // 
            this.lbl_PowerType.AutoSize = true;
            this.lbl_PowerType.Location = new System.Drawing.Point(6, 25);
            this.lbl_PowerType.Name = "lbl_PowerType";
            this.lbl_PowerType.Size = new System.Drawing.Size(34, 13);
            this.lbl_PowerType.TabIndex = 0;
            this.lbl_PowerType.Text = "Type:";
            this.lbl_PowerType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txt_DeviceTime);
            this.groupBox3.Controls.Add(this.lbl_DeviceTime);
            this.groupBox3.Controls.Add(this.btn_TimeGet);
            this.groupBox3.Controls.Add(this.btn_TimeSet);
            this.groupBox3.Location = new System.Drawing.Point(307, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(457, 113);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Time Settings";
            // 
            // txt_DeviceTime
            // 
            this.txt_DeviceTime.BackColor = System.Drawing.Color.White;
            this.txt_DeviceTime.Location = new System.Drawing.Point(47, 22);
            this.txt_DeviceTime.Name = "txt_DeviceTime";
            this.txt_DeviceTime.ReadOnly = true;
            this.txt_DeviceTime.Size = new System.Drawing.Size(399, 20);
            this.txt_DeviceTime.TabIndex = 1;
            // 
            // lbl_DeviceTime
            // 
            this.lbl_DeviceTime.AutoSize = true;
            this.lbl_DeviceTime.Location = new System.Drawing.Point(6, 25);
            this.lbl_DeviceTime.Name = "lbl_DeviceTime";
            this.lbl_DeviceTime.Size = new System.Drawing.Size(33, 13);
            this.lbl_DeviceTime.TabIndex = 0;
            this.lbl_DeviceTime.Text = "Time:";
            this.lbl_DeviceTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_TimeGet
            // 
            this.btn_TimeGet.Location = new System.Drawing.Point(176, 51);
            this.btn_TimeGet.Name = "btn_TimeGet";
            this.btn_TimeGet.Size = new System.Drawing.Size(75, 25);
            this.btn_TimeGet.TabIndex = 2;
            this.btn_TimeGet.Text = "Get";
            this.btn_TimeGet.UseVisualStyleBackColor = true;
            this.btn_TimeGet.Click += new System.EventHandler(this.btn_TimeGet_Click);
            // 
            // btn_TimeSet
            // 
            this.btn_TimeSet.Location = new System.Drawing.Point(257, 51);
            this.btn_TimeSet.Name = "btn_TimeSet";
            this.btn_TimeSet.Size = new System.Drawing.Size(189, 25);
            this.btn_TimeSet.TabIndex = 3;
            this.btn_TimeSet.Text = "Synchronize the device time";
            this.btn_TimeSet.UseVisualStyleBackColor = true;
            this.btn_TimeSet.Click += new System.EventHandler(this.btn_TimeSet_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btn_SystemGet);
            this.groupBox4.Controls.Add(this.txt_PwdCount);
            this.groupBox4.Controls.Add(this.txt_CardCount);
            this.groupBox4.Controls.Add(this.txt_HisManageCount);
            this.groupBox4.Controls.Add(this.txt_ManageCount);
            this.groupBox4.Controls.Add(this.txt_HisExitCount);
            this.groupBox4.Controls.Add(this.txt_ExitCount);
            this.groupBox4.Controls.Add(this.txt_AdminCount);
            this.groupBox4.Controls.Add(this.txt_FingerCount);
            this.groupBox4.Controls.Add(this.lbl_PwdCount);
            this.groupBox4.Controls.Add(this.lbl_CardCount);
            this.groupBox4.Controls.Add(this.lbl_HisManageCount);
            this.groupBox4.Controls.Add(this.lbl_HisExitCount);
            this.groupBox4.Controls.Add(this.lbl_ManageCount);
            this.groupBox4.Controls.Add(this.lbl_ExitCount);
            this.groupBox4.Controls.Add(this.txt_UserCount);
            this.groupBox4.Controls.Add(this.lbl_AdminCount);
            this.groupBox4.Controls.Add(this.lbl_FingerCount);
            this.groupBox4.Controls.Add(this.lbl_UserCount);
            this.groupBox4.Location = new System.Drawing.Point(243, 132);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(521, 169);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Device Record Information Settings";
            // 
            // btn_SystemGet
            // 
            this.btn_SystemGet.Location = new System.Drawing.Point(435, 139);
            this.btn_SystemGet.Name = "btn_SystemGet";
            this.btn_SystemGet.Size = new System.Drawing.Size(75, 25);
            this.btn_SystemGet.TabIndex = 18;
            this.btn_SystemGet.Text = "Get";
            this.btn_SystemGet.UseVisualStyleBackColor = true;
            this.btn_SystemGet.Click += new System.EventHandler(this.btn_SystemGet_Click);
            // 
            // txt_PwdCount
            // 
            this.txt_PwdCount.BackColor = System.Drawing.Color.White;
            this.txt_PwdCount.Location = new System.Drawing.Point(410, 51);
            this.txt_PwdCount.Name = "txt_PwdCount";
            this.txt_PwdCount.ReadOnly = true;
            this.txt_PwdCount.Size = new System.Drawing.Size(100, 20);
            this.txt_PwdCount.TabIndex = 7;
            // 
            // txt_CardCount
            // 
            this.txt_CardCount.BackColor = System.Drawing.Color.White;
            this.txt_CardCount.Location = new System.Drawing.Point(131, 80);
            this.txt_CardCount.Name = "txt_CardCount";
            this.txt_CardCount.ReadOnly = true;
            this.txt_CardCount.Size = new System.Drawing.Size(100, 20);
            this.txt_CardCount.TabIndex = 9;
            // 
            // txt_HisManageCount
            // 
            this.txt_HisManageCount.BackColor = System.Drawing.Color.White;
            this.txt_HisManageCount.Location = new System.Drawing.Point(131, 139);
            this.txt_HisManageCount.Name = "txt_HisManageCount";
            this.txt_HisManageCount.ReadOnly = true;
            this.txt_HisManageCount.Size = new System.Drawing.Size(100, 20);
            this.txt_HisManageCount.TabIndex = 17;
            // 
            // txt_ManageCount
            // 
            this.txt_ManageCount.BackColor = System.Drawing.Color.White;
            this.txt_ManageCount.Location = new System.Drawing.Point(131, 109);
            this.txt_ManageCount.Name = "txt_ManageCount";
            this.txt_ManageCount.ReadOnly = true;
            this.txt_ManageCount.Size = new System.Drawing.Size(100, 20);
            this.txt_ManageCount.TabIndex = 13;
            // 
            // txt_HisExitCount
            // 
            this.txt_HisExitCount.BackColor = System.Drawing.Color.White;
            this.txt_HisExitCount.Location = new System.Drawing.Point(410, 107);
            this.txt_HisExitCount.Name = "txt_HisExitCount";
            this.txt_HisExitCount.ReadOnly = true;
            this.txt_HisExitCount.Size = new System.Drawing.Size(100, 20);
            this.txt_HisExitCount.TabIndex = 15;
            // 
            // txt_ExitCount
            // 
            this.txt_ExitCount.BackColor = System.Drawing.Color.White;
            this.txt_ExitCount.Location = new System.Drawing.Point(410, 78);
            this.txt_ExitCount.Name = "txt_ExitCount";
            this.txt_ExitCount.ReadOnly = true;
            this.txt_ExitCount.Size = new System.Drawing.Size(100, 20);
            this.txt_ExitCount.TabIndex = 11;
            // 
            // txt_AdminCount
            // 
            this.txt_AdminCount.BackColor = System.Drawing.Color.White;
            this.txt_AdminCount.Location = new System.Drawing.Point(410, 22);
            this.txt_AdminCount.Name = "txt_AdminCount";
            this.txt_AdminCount.ReadOnly = true;
            this.txt_AdminCount.Size = new System.Drawing.Size(100, 20);
            this.txt_AdminCount.TabIndex = 3;
            // 
            // txt_FingerCount
            // 
            this.txt_FingerCount.BackColor = System.Drawing.Color.White;
            this.txt_FingerCount.Location = new System.Drawing.Point(131, 51);
            this.txt_FingerCount.Name = "txt_FingerCount";
            this.txt_FingerCount.ReadOnly = true;
            this.txt_FingerCount.Size = new System.Drawing.Size(100, 20);
            this.txt_FingerCount.TabIndex = 5;
            // 
            // lbl_PwdCount
            // 
            this.lbl_PwdCount.AutoSize = true;
            this.lbl_PwdCount.Location = new System.Drawing.Point(339, 54);
            this.lbl_PwdCount.Name = "lbl_PwdCount";
            this.lbl_PwdCount.Size = new System.Drawing.Size(67, 13);
            this.lbl_PwdCount.TabIndex = 6;
            this.lbl_PwdCount.Text = "PWD Count:";
            this.lbl_PwdCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_CardCount
            // 
            this.lbl_CardCount.AutoSize = true;
            this.lbl_CardCount.Location = new System.Drawing.Point(54, 83);
            this.lbl_CardCount.Name = "lbl_CardCount";
            this.lbl_CardCount.Size = new System.Drawing.Size(63, 13);
            this.lbl_CardCount.TabIndex = 8;
            this.lbl_CardCount.Text = "Card Count:";
            this.lbl_CardCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_HisManageCount
            // 
            this.lbl_HisManageCount.AutoSize = true;
            this.lbl_HisManageCount.Location = new System.Drawing.Point(6, 142);
            this.lbl_HisManageCount.Name = "lbl_HisManageCount";
            this.lbl_HisManageCount.Size = new System.Drawing.Size(101, 13);
            this.lbl_HisManageCount.TabIndex = 16;
            this.lbl_HisManageCount.Text = "History SLog Count:";
            this.lbl_HisManageCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_HisExitCount
            // 
            this.lbl_HisExitCount.AutoSize = true;
            this.lbl_HisExitCount.Location = new System.Drawing.Point(285, 113);
            this.lbl_HisExitCount.Name = "lbl_HisExitCount";
            this.lbl_HisExitCount.Size = new System.Drawing.Size(102, 13);
            this.lbl_HisExitCount.TabIndex = 14;
            this.lbl_HisExitCount.Text = "History GLog Count:";
            this.lbl_HisExitCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_ManageCount
            // 
            this.lbl_ManageCount.AutoSize = true;
            this.lbl_ManageCount.Location = new System.Drawing.Point(54, 113);
            this.lbl_ManageCount.Name = "lbl_ManageCount";
            this.lbl_ManageCount.Size = new System.Drawing.Size(66, 13);
            this.lbl_ManageCount.TabIndex = 12;
            this.lbl_ManageCount.Text = "SLog Count:";
            this.lbl_ManageCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_ExitCount
            // 
            this.lbl_ExitCount.AutoSize = true;
            this.lbl_ExitCount.Location = new System.Drawing.Point(333, 83);
            this.lbl_ExitCount.Name = "lbl_ExitCount";
            this.lbl_ExitCount.Size = new System.Drawing.Size(67, 13);
            this.lbl_ExitCount.TabIndex = 10;
            this.lbl_ExitCount.Text = "GLog Count:";
            this.lbl_ExitCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_UserCount
            // 
            this.txt_UserCount.BackColor = System.Drawing.Color.White;
            this.txt_UserCount.Location = new System.Drawing.Point(131, 22);
            this.txt_UserCount.Name = "txt_UserCount";
            this.txt_UserCount.ReadOnly = true;
            this.txt_UserCount.Size = new System.Drawing.Size(100, 20);
            this.txt_UserCount.TabIndex = 1;
            // 
            // lbl_AdminCount
            // 
            this.lbl_AdminCount.AutoSize = true;
            this.lbl_AdminCount.Location = new System.Drawing.Point(351, 25);
            this.lbl_AdminCount.Name = "lbl_AdminCount";
            this.lbl_AdminCount.Size = new System.Drawing.Size(52, 13);
            this.lbl_AdminCount.TabIndex = 2;
            this.lbl_AdminCount.Text = "Manager:";
            this.lbl_AdminCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_FingerCount
            // 
            this.lbl_FingerCount.AutoSize = true;
            this.lbl_FingerCount.Location = new System.Drawing.Point(66, 54);
            this.lbl_FingerCount.Name = "lbl_FingerCount";
            this.lbl_FingerCount.Size = new System.Drawing.Size(54, 13);
            this.lbl_FingerCount.TabIndex = 4;
            this.lbl_FingerCount.Text = "FP Count:";
            this.lbl_FingerCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_UserCount
            // 
            this.lbl_UserCount.AutoSize = true;
            this.lbl_UserCount.Location = new System.Drawing.Point(90, 25);
            this.lbl_UserCount.Name = "lbl_UserCount";
            this.lbl_UserCount.Size = new System.Drawing.Size(32, 13);
            this.lbl_UserCount.TabIndex = 0;
            this.lbl_UserCount.Text = "User:";
            this.lbl_UserCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_MacAddress
            // 
            this.btn_MacAddress.Location = new System.Drawing.Point(471, 575);
            this.btn_MacAddress.Name = "btn_MacAddress";
            this.btn_MacAddress.Size = new System.Drawing.Size(113, 25);
            this.btn_MacAddress.TabIndex = 7;
            this.btn_MacAddress.Text = "Get Mac Address";
            this.btn_MacAddress.UseVisualStyleBackColor = true;
            this.btn_MacAddress.Click += new System.EventHandler(this.btn_MacAddress_Click);
            // 
            // btn_Reset
            // 
            this.btn_Reset.Location = new System.Drawing.Point(590, 575);
            this.btn_Reset.Name = "btn_Reset";
            this.btn_Reset.Size = new System.Drawing.Size(93, 25);
            this.btn_Reset.TabIndex = 8;
            this.btn_Reset.Text = "Device Init";
            this.btn_Reset.UseVisualStyleBackColor = true;
            this.btn_Reset.Click += new System.EventHandler(this.btn_Reset_Click);
            // 
            // txt_Welcome
            // 
            this.txt_Welcome.Location = new System.Drawing.Point(101, 22);
            this.txt_Welcome.MaxLength = 32;
            this.txt_Welcome.Name = "txt_Welcome";
            this.txt_Welcome.Size = new System.Drawing.Size(238, 20);
            this.txt_Welcome.TabIndex = 1;
            // 
            // btn_WelcomeGet
            // 
            this.btn_WelcomeGet.Location = new System.Drawing.Point(345, 21);
            this.btn_WelcomeGet.Name = "btn_WelcomeGet";
            this.btn_WelcomeGet.Size = new System.Drawing.Size(75, 23);
            this.btn_WelcomeGet.TabIndex = 2;
            this.btn_WelcomeGet.Text = "Get";
            this.btn_WelcomeGet.UseVisualStyleBackColor = true;
            this.btn_WelcomeGet.Click += new System.EventHandler(this.btn_WelcomeGet_Click);
            // 
            // btn_WelcomeSet
            // 
            this.btn_WelcomeSet.Location = new System.Drawing.Point(426, 21);
            this.btn_WelcomeSet.Name = "btn_WelcomeSet";
            this.btn_WelcomeSet.Size = new System.Drawing.Size(75, 23);
            this.btn_WelcomeSet.TabIndex = 3;
            this.btn_WelcomeSet.Text = "Set";
            this.btn_WelcomeSet.UseVisualStyleBackColor = true;
            this.btn_WelcomeSet.Click += new System.EventHandler(this.btn_WelcomeSet_Click);
            // 
            // lbl_WelCome
            // 
            this.lbl_WelCome.AutoSize = true;
            this.lbl_WelCome.Location = new System.Drawing.Point(6, 25);
            this.lbl_WelCome.Name = "lbl_WelCome";
            this.lbl_WelCome.Size = new System.Drawing.Size(78, 13);
            this.lbl_WelCome.TabIndex = 0;
            this.lbl_WelCome.Text = "Welcome Title:";
            this.lbl_WelCome.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_Standby
            // 
            this.txt_Standby.Location = new System.Drawing.Point(101, 51);
            this.txt_Standby.MaxLength = 32;
            this.txt_Standby.Name = "txt_Standby";
            this.txt_Standby.Size = new System.Drawing.Size(238, 20);
            this.txt_Standby.TabIndex = 5;
            // 
            // btn_StandbyGet
            // 
            this.btn_StandbyGet.Location = new System.Drawing.Point(345, 50);
            this.btn_StandbyGet.Name = "btn_StandbyGet";
            this.btn_StandbyGet.Size = new System.Drawing.Size(75, 23);
            this.btn_StandbyGet.TabIndex = 6;
            this.btn_StandbyGet.Text = "Get";
            this.btn_StandbyGet.UseVisualStyleBackColor = true;
            this.btn_StandbyGet.Click += new System.EventHandler(this.btn_StandbyGet_Click);
            // 
            // btn_StandbySet
            // 
            this.btn_StandbySet.Location = new System.Drawing.Point(426, 50);
            this.btn_StandbySet.Name = "btn_StandbySet";
            this.btn_StandbySet.Size = new System.Drawing.Size(75, 23);
            this.btn_StandbySet.TabIndex = 7;
            this.btn_StandbySet.Text = "Set";
            this.btn_StandbySet.UseVisualStyleBackColor = true;
            this.btn_StandbySet.Click += new System.EventHandler(this.btn_StandbySet_Click);
            // 
            // lbl_Standby
            // 
            this.lbl_Standby.AutoSize = true;
            this.lbl_Standby.Location = new System.Drawing.Point(6, 54);
            this.lbl_Standby.Name = "lbl_Standby";
            this.lbl_Standby.Size = new System.Drawing.Size(72, 13);
            this.lbl_Standby.TabIndex = 4;
            this.lbl_Standby.Text = "Standby Title:";
            this.lbl_Standby.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txt_Welcome);
            this.groupBox5.Controls.Add(this.btn_StandbySet);
            this.groupBox5.Controls.Add(this.lbl_Standby);
            this.groupBox5.Controls.Add(this.btn_StandbyGet);
            this.groupBox5.Controls.Add(this.lbl_WelCome);
            this.groupBox5.Controls.Add(this.btn_WelcomeSet);
            this.groupBox5.Controls.Add(this.txt_Standby);
            this.groupBox5.Controls.Add(this.btn_WelcomeGet);
            this.groupBox5.Location = new System.Drawing.Point(12, 308);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(752, 85);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Title Settings";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btn_ModelSet);
            this.groupBox6.Controls.Add(this.btn_ModelGet);
            this.groupBox6.Controls.Add(this.txt_Model);
            this.groupBox6.Controls.Add(this.getSerialNoButton);
            this.groupBox6.Controls.Add(this.btn_FirmwareVersionGet);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.lbl_Model);
            this.groupBox6.Controls.Add(this.serialNoTextBox);
            this.groupBox6.Controls.Add(this.txt_FirmwareVersion);
            this.groupBox6.Controls.Add(this.lbl_FirmwareVersion);
            this.groupBox6.Location = new System.Drawing.Point(12, 399);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(752, 108);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Device Information";
            // 
            // btn_ModelSet
            // 
            this.btn_ModelSet.Location = new System.Drawing.Point(376, 50);
            this.btn_ModelSet.Name = "btn_ModelSet";
            this.btn_ModelSet.Size = new System.Drawing.Size(75, 25);
            this.btn_ModelSet.TabIndex = 6;
            this.btn_ModelSet.Text = "Set";
            this.btn_ModelSet.UseVisualStyleBackColor = true;
            this.btn_ModelSet.Click += new System.EventHandler(this.btn_ModelSet_Click);
            // 
            // btn_ModelGet
            // 
            this.btn_ModelGet.Location = new System.Drawing.Point(295, 50);
            this.btn_ModelGet.Name = "btn_ModelGet";
            this.btn_ModelGet.Size = new System.Drawing.Size(75, 25);
            this.btn_ModelGet.TabIndex = 5;
            this.btn_ModelGet.Text = "Get";
            this.btn_ModelGet.UseVisualStyleBackColor = true;
            this.btn_ModelGet.Click += new System.EventHandler(this.btn_ModelGet_Click);
            // 
            // txt_Model
            // 
            this.txt_Model.BackColor = System.Drawing.Color.White;
            this.txt_Model.Location = new System.Drawing.Point(113, 53);
            this.txt_Model.MaxLength = 16;
            this.txt_Model.Name = "txt_Model";
            this.txt_Model.Size = new System.Drawing.Size(176, 20);
            this.txt_Model.TabIndex = 4;
            // 
            // getSerialNoButton
            // 
            this.getSerialNoButton.Location = new System.Drawing.Point(666, 22);
            this.getSerialNoButton.Name = "getSerialNoButton";
            this.getSerialNoButton.Size = new System.Drawing.Size(60, 25);
            this.getSerialNoButton.TabIndex = 2;
            this.getSerialNoButton.Text = "Get";
            this.getSerialNoButton.UseVisualStyleBackColor = true;
            this.getSerialNoButton.Click += new System.EventHandler(this.getSerialNoButton_Click);
            // 
            // btn_FirmwareVersionGet
            // 
            this.btn_FirmwareVersionGet.Location = new System.Drawing.Point(295, 19);
            this.btn_FirmwareVersionGet.Name = "btn_FirmwareVersionGet";
            this.btn_FirmwareVersionGet.Size = new System.Drawing.Size(75, 25);
            this.btn_FirmwareVersionGet.TabIndex = 2;
            this.btn_FirmwareVersionGet.Text = "Get";
            this.btn_FirmwareVersionGet.UseVisualStyleBackColor = true;
            this.btn_FirmwareVersionGet.Click += new System.EventHandler(this.btn_FirmwareVersionGet_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(464, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "SerialNo:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_Model
            // 
            this.lbl_Model.AutoSize = true;
            this.lbl_Model.Location = new System.Drawing.Point(66, 56);
            this.lbl_Model.Name = "lbl_Model";
            this.lbl_Model.Size = new System.Drawing.Size(39, 13);
            this.lbl_Model.TabIndex = 3;
            this.lbl_Model.Text = "Model:";
            this.lbl_Model.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // serialNoTextBox
            // 
            this.serialNoTextBox.BackColor = System.Drawing.Color.White;
            this.serialNoTextBox.Location = new System.Drawing.Point(520, 25);
            this.serialNoTextBox.Name = "serialNoTextBox";
            this.serialNoTextBox.ReadOnly = true;
            this.serialNoTextBox.Size = new System.Drawing.Size(140, 20);
            this.serialNoTextBox.TabIndex = 1;
            // 
            // txt_FirmwareVersion
            // 
            this.txt_FirmwareVersion.BackColor = System.Drawing.Color.White;
            this.txt_FirmwareVersion.Location = new System.Drawing.Point(113, 22);
            this.txt_FirmwareVersion.Name = "txt_FirmwareVersion";
            this.txt_FirmwareVersion.ReadOnly = true;
            this.txt_FirmwareVersion.Size = new System.Drawing.Size(176, 20);
            this.txt_FirmwareVersion.TabIndex = 1;
            // 
            // lbl_FirmwareVersion
            // 
            this.lbl_FirmwareVersion.AutoSize = true;
            this.lbl_FirmwareVersion.Location = new System.Drawing.Point(6, 25);
            this.lbl_FirmwareVersion.Name = "lbl_FirmwareVersion";
            this.lbl_FirmwareVersion.Size = new System.Drawing.Size(87, 13);
            this.lbl_FirmwareVersion.TabIndex = 0;
            this.lbl_FirmwareVersion.Text = "FirmwareVersion:";
            this.lbl_FirmwareVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btn_UploadSound);
            this.groupBox7.Controls.Add(this.cbo_SoundType);
            this.groupBox7.Controls.Add(this.lbl_SoundType);
            this.groupBox7.Location = new System.Drawing.Point(12, 514);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(752, 55);
            this.groupBox7.TabIndex = 6;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Upload Sound";
            // 
            // btn_UploadSound
            // 
            this.btn_UploadSound.Location = new System.Drawing.Point(173, 20);
            this.btn_UploadSound.Name = "btn_UploadSound";
            this.btn_UploadSound.Size = new System.Drawing.Size(75, 25);
            this.btn_UploadSound.TabIndex = 2;
            this.btn_UploadSound.Text = "Upload";
            this.btn_UploadSound.UseVisualStyleBackColor = true;
            this.btn_UploadSound.Click += new System.EventHandler(this.btn_UploadSound_Click);
            // 
            // cbo_SoundType
            // 
            this.cbo_SoundType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_SoundType.FormattingEnabled = true;
            this.cbo_SoundType.Items.AddRange(new object[] {
            "User Defined 1",
            "User Defined 2"});
            this.cbo_SoundType.Location = new System.Drawing.Point(47, 22);
            this.cbo_SoundType.Name = "cbo_SoundType";
            this.cbo_SoundType.Size = new System.Drawing.Size(120, 21);
            this.cbo_SoundType.TabIndex = 1;
            // 
            // lbl_SoundType
            // 
            this.lbl_SoundType.AutoSize = true;
            this.lbl_SoundType.Location = new System.Drawing.Point(6, 25);
            this.lbl_SoundType.Name = "lbl_SoundType";
            this.lbl_SoundType.Size = new System.Drawing.Size(34, 13);
            this.lbl_SoundType.TabIndex = 0;
            this.lbl_SoundType.Text = "Type:";
            this.lbl_SoundType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // restartButton
            // 
            this.restartButton.Location = new System.Drawing.Point(689, 575);
            this.restartButton.Name = "restartButton";
            this.restartButton.Size = new System.Drawing.Size(75, 23);
            this.restartButton.TabIndex = 8;
            this.restartButton.Text = "Restart";
            this.restartButton.UseVisualStyleBackColor = true;
            this.restartButton.Click += new System.EventHandler(this.restartButton_Click);
            // 
            // SystemSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 611);
            this.Controls.Add(this.restartButton);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.btn_Reset);
            this.Controls.Add(this.btn_MacAddress);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SystemSettingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "System Parameter Settings";
            this.Load += new System.EventHandler(this.SystemSettingForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbo_Param;
        private System.Windows.Forms.Label lbl_Param;
        private System.Windows.Forms.Button btn_ParamGet;
        private System.Windows.Forms.Button btn_ParamSet;
        private System.Windows.Forms.TextBox txt_ParamValue;
        private System.Windows.Forms.Label lbl_ParamValue;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbo_PowerMinute;
        private System.Windows.Forms.ComboBox cbo_PowerHour;
        private System.Windows.Forms.ComboBox cbo_DN;
        private System.Windows.Forms.ComboBox cbo_PowerType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_Time;
        private System.Windows.Forms.Label lbl_DN;
        private System.Windows.Forms.Label lbl_PowerType;
        private System.Windows.Forms.Label lbl_Status;
        private System.Windows.Forms.Button btn_PowerGet;
        private System.Windows.Forms.Button btn_PowerSet;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_TimeGet;
        private System.Windows.Forms.Button btn_TimeSet;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btn_SystemGet;
        private System.Windows.Forms.TextBox txt_PwdCount;
        private System.Windows.Forms.TextBox txt_CardCount;
        private System.Windows.Forms.TextBox txt_ManageCount;
        private System.Windows.Forms.TextBox txt_ExitCount;
        private System.Windows.Forms.TextBox txt_AdminCount;
        private System.Windows.Forms.TextBox txt_FingerCount;
        private System.Windows.Forms.Label lbl_PwdCount;
        private System.Windows.Forms.Label lbl_CardCount;
        private System.Windows.Forms.Label lbl_ManageCount;
        private System.Windows.Forms.Label lbl_ExitCount;
        private System.Windows.Forms.TextBox txt_UserCount;
        private System.Windows.Forms.Label lbl_AdminCount;
        private System.Windows.Forms.Label lbl_FingerCount;
        private System.Windows.Forms.Label lbl_UserCount;
        private System.Windows.Forms.Button btn_MacAddress;
        private System.Windows.Forms.Button btn_Reset;
        private System.Windows.Forms.TextBox txt_Welcome;
        private System.Windows.Forms.Button btn_WelcomeGet;
        private System.Windows.Forms.Button btn_WelcomeSet;
        private System.Windows.Forms.Label lbl_WelCome;
        private System.Windows.Forms.TextBox txt_Standby;
        private System.Windows.Forms.Button btn_StandbyGet;
        private System.Windows.Forms.Button btn_StandbySet;
        private System.Windows.Forms.Label lbl_Standby;
        private System.Windows.Forms.TextBox txt_DeviceTime;
        private System.Windows.Forms.Label lbl_DeviceTime;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txt_HisManageCount;
        private System.Windows.Forms.TextBox txt_HisExitCount;
        private System.Windows.Forms.Label lbl_HisManageCount;
        private System.Windows.Forms.Label lbl_HisExitCount;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btn_ModelGet;
        private System.Windows.Forms.TextBox txt_Model;
        private System.Windows.Forms.Button btn_FirmwareVersionGet;
        private System.Windows.Forms.Label lbl_Model;
        private System.Windows.Forms.TextBox txt_FirmwareVersion;
        private System.Windows.Forms.Label lbl_FirmwareVersion;
        private System.Windows.Forms.Button btn_ModelSet;
        private System.Windows.Forms.CheckBox chk_Power;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button btn_UploadSound;
        private System.Windows.Forms.ComboBox cbo_SoundType;
        private System.Windows.Forms.Label lbl_SoundType;
        private System.Windows.Forms.Button getSerialNoButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox serialNoTextBox;
        private System.Windows.Forms.Button restartButton;
    }
}