namespace ZDC2911Demo.UI {
    partial class ACForm {
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
            this.cbo_ParamName = new System.Windows.Forms.ComboBox();
            this.lbl_ParamValue = new System.Windows.Forms.Label();
            this.lbl_ParamName = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chk_RoleEnable = new System.Windows.Forms.CheckBox();
            this.nud_CtrlDIN = new System.Windows.Forms.NumericUpDown();
            this.btn_RoleGet = new System.Windows.Forms.Button();
            this.btn_RoleSet = new System.Windows.Forms.Button();
            this.lbl_RoleGroupNo = new System.Windows.Forms.Label();
            this.lbl_RoleUserId = new System.Windows.Forms.Label();
            this.cbo_RoleGroupNo = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_ZoneGet = new System.Windows.Forms.Button();
            this.btn_ZoneSet = new System.Windows.Forms.Button();
            this.lbl_ZoneEndTime = new System.Windows.Forms.Label();
            this.cbo_ZoneEndHour = new System.Windows.Forms.ComboBox();
            this.cbo_ZoneEndMinute = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_Weekday = new System.Windows.Forms.Label();
            this.lbl_ZoneNo = new System.Windows.Forms.Label();
            this.cbo_Weekday = new System.Windows.Forms.ComboBox();
            this.cbo_ZoneNo = new System.Windows.Forms.ComboBox();
            this.cbo_ZoneBeginMinute = new System.Windows.Forms.ComboBox();
            this.lbl_ZoneBeginTime = new System.Windows.Forms.Label();
            this.cbo_ZoneBeginHour = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btn_GroupGet = new System.Windows.Forms.Button();
            this.btn_GroupSet = new System.Windows.Forms.Button();
            this.cbo_ZoneNoThree = new System.Windows.Forms.ComboBox();
            this.cbo_ZoneNoTwo = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbo_ZoneNoOne = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbo_GroupMultiUser = new System.Windows.Forms.ComboBox();
            this.lbl_MultiUser = new System.Windows.Forms.Label();
            this.cbo_GroupNo = new System.Windows.Forms.ComboBox();
            this.lbl_GroupZoneNo = new System.Windows.Forms.Label();
            this.lbl_GroupNo = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.dtp_EndDate = new System.Windows.Forms.DateTimePicker();
            this.dtp_BeginDate = new System.Windows.Forms.DateTimePicker();
            this.nud_PeriodDIN = new System.Windows.Forms.NumericUpDown();
            this.btn_AvailableGet = new System.Windows.Forms.Button();
            this.btn_AvaiableSet = new System.Windows.Forms.Button();
            this.lbl_EndDate = new System.Windows.Forms.Label();
            this.lbl_BeginDate = new System.Windows.Forms.Label();
            this.lbl_UserId = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_CtrlDIN)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PeriodDIN)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_ParamGet);
            this.groupBox1.Controls.Add(this.btn_ParamSet);
            this.groupBox1.Controls.Add(this.txt_ParamValue);
            this.groupBox1.Controls.Add(this.cbo_ParamName);
            this.groupBox1.Controls.Add(this.lbl_ParamValue);
            this.groupBox1.Controls.Add(this.lbl_ParamName);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(225, 108);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Access Control Parameter Settings";
            // 
            // btn_ParamGet
            // 
            this.btn_ParamGet.Location = new System.Drawing.Point(59, 73);
            this.btn_ParamGet.Name = "btn_ParamGet";
            this.btn_ParamGet.Size = new System.Drawing.Size(75, 23);
            this.btn_ParamGet.TabIndex = 4;
            this.btn_ParamGet.Text = "Get";
            this.btn_ParamGet.UseVisualStyleBackColor = true;
            this.btn_ParamGet.Click += new System.EventHandler(this.btn_ParamGet_Click);
            // 
            // btn_ParamSet
            // 
            this.btn_ParamSet.Location = new System.Drawing.Point(140, 73);
            this.btn_ParamSet.Name = "btn_ParamSet";
            this.btn_ParamSet.Size = new System.Drawing.Size(75, 23);
            this.btn_ParamSet.TabIndex = 5;
            this.btn_ParamSet.Text = "Set";
            this.btn_ParamSet.UseVisualStyleBackColor = true;
            this.btn_ParamSet.Click += new System.EventHandler(this.btn_ParamSet_Click);
            // 
            // txt_ParamValue
            // 
            this.txt_ParamValue.Location = new System.Drawing.Point(77, 46);
            this.txt_ParamValue.Name = "txt_ParamValue";
            this.txt_ParamValue.Size = new System.Drawing.Size(138, 21);
            this.txt_ParamValue.TabIndex = 3;
            // 
            // cbo_ParamName
            // 
            this.cbo_ParamName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_ParamName.FormattingEnabled = true;
            this.cbo_ParamName.Items.AddRange(new object[] {
            "Open Delay",
            "Wiegand Mode",
            "Check Door Status",
            "Menace Open Door",
            "Menace Alarm"});
            this.cbo_ParamName.Location = new System.Drawing.Point(77, 20);
            this.cbo_ParamName.Name = "cbo_ParamName";
            this.cbo_ParamName.Size = new System.Drawing.Size(138, 20);
            this.cbo_ParamName.TabIndex = 1;
            // 
            // lbl_ParamValue
            // 
            this.lbl_ParamValue.AutoSize = true;
            this.lbl_ParamValue.Location = new System.Drawing.Point(30, 49);
            this.lbl_ParamValue.Name = "lbl_ParamValue";
            this.lbl_ParamValue.Size = new System.Drawing.Size(41, 12);
            this.lbl_ParamValue.TabIndex = 2;
            this.lbl_ParamValue.Text = "Value:";
            this.lbl_ParamValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_ParamName
            // 
            this.lbl_ParamName.AutoSize = true;
            this.lbl_ParamName.Location = new System.Drawing.Point(6, 23);
            this.lbl_ParamName.Name = "lbl_ParamName";
            this.lbl_ParamName.Size = new System.Drawing.Size(65, 12);
            this.lbl_ParamName.TabIndex = 0;
            this.lbl_ParamName.Text = "Parameter:";
            this.lbl_ParamName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chk_RoleEnable);
            this.groupBox2.Controls.Add(this.nud_CtrlDIN);
            this.groupBox2.Controls.Add(this.btn_RoleGet);
            this.groupBox2.Controls.Add(this.btn_RoleSet);
            this.groupBox2.Controls.Add(this.lbl_RoleGroupNo);
            this.groupBox2.Controls.Add(this.lbl_RoleUserId);
            this.groupBox2.Controls.Add(this.cbo_RoleGroupNo);
            this.groupBox2.Location = new System.Drawing.Point(243, 20);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(509, 100);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "User Role Settings";
            // 
            // chk_RoleEnable
            // 
            this.chk_RoleEnable.AutoSize = true;
            this.chk_RoleEnable.Location = new System.Drawing.Point(77, 48);
            this.chk_RoleEnable.Name = "chk_RoleEnable";
            this.chk_RoleEnable.Size = new System.Drawing.Size(168, 16);
            this.chk_RoleEnable.TabIndex = 4;
            this.chk_RoleEnable.Text = "Use User Avaiable Period";
            this.chk_RoleEnable.UseVisualStyleBackColor = true;
            // 
            // nud_CtrlDIN
            // 
            this.nud_CtrlDIN.Location = new System.Drawing.Point(77, 21);
            this.nud_CtrlDIN.Maximum = new decimal(new int[] {
            -1486618625,
            232830643,
            0,
            0});
            this.nud_CtrlDIN.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_CtrlDIN.Name = "nud_CtrlDIN";
            this.nud_CtrlDIN.Size = new System.Drawing.Size(150, 21);
            this.nud_CtrlDIN.TabIndex = 1;
            this.nud_CtrlDIN.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btn_RoleGet
            // 
            this.btn_RoleGet.Location = new System.Drawing.Point(347, 65);
            this.btn_RoleGet.Name = "btn_RoleGet";
            this.btn_RoleGet.Size = new System.Drawing.Size(75, 23);
            this.btn_RoleGet.TabIndex = 5;
            this.btn_RoleGet.Text = "Get";
            this.btn_RoleGet.UseVisualStyleBackColor = true;
            this.btn_RoleGet.Click += new System.EventHandler(this.btn_RoleGet_Click);
            // 
            // btn_RoleSet
            // 
            this.btn_RoleSet.Location = new System.Drawing.Point(428, 65);
            this.btn_RoleSet.Name = "btn_RoleSet";
            this.btn_RoleSet.Size = new System.Drawing.Size(75, 23);
            this.btn_RoleSet.TabIndex = 6;
            this.btn_RoleSet.Text = "Set";
            this.btn_RoleSet.UseVisualStyleBackColor = true;
            this.btn_RoleSet.Click += new System.EventHandler(this.btn_RoleSet_Click);
            // 
            // lbl_RoleGroupNo
            // 
            this.lbl_RoleGroupNo.AutoSize = true;
            this.lbl_RoleGroupNo.Location = new System.Drawing.Point(276, 23);
            this.lbl_RoleGroupNo.Name = "lbl_RoleGroupNo";
            this.lbl_RoleGroupNo.Size = new System.Drawing.Size(71, 12);
            this.lbl_RoleGroupNo.TabIndex = 2;
            this.lbl_RoleGroupNo.Text = "Time Group:";
            this.lbl_RoleGroupNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_RoleUserId
            // 
            this.lbl_RoleUserId.AutoSize = true;
            this.lbl_RoleUserId.Location = new System.Drawing.Point(6, 23);
            this.lbl_RoleUserId.Name = "lbl_RoleUserId";
            this.lbl_RoleUserId.Size = new System.Drawing.Size(53, 12);
            this.lbl_RoleUserId.TabIndex = 0;
            this.lbl_RoleUserId.Text = "User ID:";
            this.lbl_RoleUserId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbo_RoleGroupNo
            // 
            this.cbo_RoleGroupNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_RoleGroupNo.FormattingEnabled = true;
            this.cbo_RoleGroupNo.Location = new System.Drawing.Point(353, 20);
            this.cbo_RoleGroupNo.Name = "cbo_RoleGroupNo";
            this.cbo_RoleGroupNo.Size = new System.Drawing.Size(150, 20);
            this.cbo_RoleGroupNo.TabIndex = 3;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_ZoneGet);
            this.groupBox3.Controls.Add(this.btn_ZoneSet);
            this.groupBox3.Controls.Add(this.lbl_ZoneEndTime);
            this.groupBox3.Controls.Add(this.cbo_ZoneEndHour);
            this.groupBox3.Controls.Add(this.cbo_ZoneEndMinute);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.lbl_Weekday);
            this.groupBox3.Controls.Add(this.lbl_ZoneNo);
            this.groupBox3.Controls.Add(this.cbo_Weekday);
            this.groupBox3.Controls.Add(this.cbo_ZoneNo);
            this.groupBox3.Controls.Add(this.cbo_ZoneBeginMinute);
            this.groupBox3.Controls.Add(this.lbl_ZoneBeginTime);
            this.groupBox3.Controls.Add(this.cbo_ZoneBeginHour);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(12, 126);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(225, 157);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Time Zone Settings";
            // 
            // btn_ZoneGet
            // 
            this.btn_ZoneGet.Location = new System.Drawing.Point(57, 124);
            this.btn_ZoneGet.Name = "btn_ZoneGet";
            this.btn_ZoneGet.Size = new System.Drawing.Size(75, 23);
            this.btn_ZoneGet.TabIndex = 12;
            this.btn_ZoneGet.Text = "Get";
            this.btn_ZoneGet.UseVisualStyleBackColor = true;
            this.btn_ZoneGet.Click += new System.EventHandler(this.btn_ZoneGet_Click);
            // 
            // btn_ZoneSet
            // 
            this.btn_ZoneSet.Location = new System.Drawing.Point(140, 124);
            this.btn_ZoneSet.Name = "btn_ZoneSet";
            this.btn_ZoneSet.Size = new System.Drawing.Size(75, 23);
            this.btn_ZoneSet.TabIndex = 13;
            this.btn_ZoneSet.Text = "Set";
            this.btn_ZoneSet.UseVisualStyleBackColor = true;
            this.btn_ZoneSet.Click += new System.EventHandler(this.btn_ZoneSet_Click);
            // 
            // lbl_ZoneEndTime
            // 
            this.lbl_ZoneEndTime.AutoSize = true;
            this.lbl_ZoneEndTime.Location = new System.Drawing.Point(18, 101);
            this.lbl_ZoneEndTime.Name = "lbl_ZoneEndTime";
            this.lbl_ZoneEndTime.Size = new System.Drawing.Size(59, 12);
            this.lbl_ZoneEndTime.TabIndex = 8;
            this.lbl_ZoneEndTime.Text = "End Time:";
            this.lbl_ZoneEndTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbo_ZoneEndHour
            // 
            this.cbo_ZoneEndHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_ZoneEndHour.FormattingEnabled = true;
            this.cbo_ZoneEndHour.Location = new System.Drawing.Point(83, 98);
            this.cbo_ZoneEndHour.Name = "cbo_ZoneEndHour";
            this.cbo_ZoneEndHour.Size = new System.Drawing.Size(49, 20);
            this.cbo_ZoneEndHour.TabIndex = 9;
            // 
            // cbo_ZoneEndMinute
            // 
            this.cbo_ZoneEndMinute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_ZoneEndMinute.FormattingEnabled = true;
            this.cbo_ZoneEndMinute.Location = new System.Drawing.Point(160, 98);
            this.cbo_ZoneEndMinute.Name = "cbo_ZoneEndMinute";
            this.cbo_ZoneEndMinute.Size = new System.Drawing.Size(55, 20);
            this.cbo_ZoneEndMinute.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(137, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Weekday
            // 
            this.lbl_Weekday.AutoSize = true;
            this.lbl_Weekday.Location = new System.Drawing.Point(18, 49);
            this.lbl_Weekday.Name = "lbl_Weekday";
            this.lbl_Weekday.Size = new System.Drawing.Size(59, 12);
            this.lbl_Weekday.TabIndex = 2;
            this.lbl_Weekday.Text = "Week Day:";
            this.lbl_Weekday.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_ZoneNo
            // 
            this.lbl_ZoneNo.AutoSize = true;
            this.lbl_ZoneNo.Location = new System.Drawing.Point(54, 23);
            this.lbl_ZoneNo.Name = "lbl_ZoneNo";
            this.lbl_ZoneNo.Size = new System.Drawing.Size(23, 12);
            this.lbl_ZoneNo.TabIndex = 0;
            this.lbl_ZoneNo.Text = "SN:";
            this.lbl_ZoneNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbo_Weekday
            // 
            this.cbo_Weekday.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_Weekday.FormattingEnabled = true;
            this.cbo_Weekday.Items.AddRange(new object[] {
            "星期日",
            "星期一",
            "星期二",
            "星期三",
            "星期四",
            "星期五",
            "星期六"});
            this.cbo_Weekday.Location = new System.Drawing.Point(83, 46);
            this.cbo_Weekday.Name = "cbo_Weekday";
            this.cbo_Weekday.Size = new System.Drawing.Size(132, 20);
            this.cbo_Weekday.TabIndex = 3;
            // 
            // cbo_ZoneNo
            // 
            this.cbo_ZoneNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_ZoneNo.FormattingEnabled = true;
            this.cbo_ZoneNo.Location = new System.Drawing.Point(83, 20);
            this.cbo_ZoneNo.Name = "cbo_ZoneNo";
            this.cbo_ZoneNo.Size = new System.Drawing.Size(132, 20);
            this.cbo_ZoneNo.TabIndex = 1;
            // 
            // cbo_ZoneBeginMinute
            // 
            this.cbo_ZoneBeginMinute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_ZoneBeginMinute.FormattingEnabled = true;
            this.cbo_ZoneBeginMinute.Location = new System.Drawing.Point(161, 72);
            this.cbo_ZoneBeginMinute.Name = "cbo_ZoneBeginMinute";
            this.cbo_ZoneBeginMinute.Size = new System.Drawing.Size(54, 20);
            this.cbo_ZoneBeginMinute.TabIndex = 7;
            // 
            // lbl_ZoneBeginTime
            // 
            this.lbl_ZoneBeginTime.AutoSize = true;
            this.lbl_ZoneBeginTime.Location = new System.Drawing.Point(6, 75);
            this.lbl_ZoneBeginTime.Name = "lbl_ZoneBeginTime";
            this.lbl_ZoneBeginTime.Size = new System.Drawing.Size(71, 12);
            this.lbl_ZoneBeginTime.TabIndex = 4;
            this.lbl_ZoneBeginTime.Text = "Begin Time:";
            this.lbl_ZoneBeginTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbo_ZoneBeginHour
            // 
            this.cbo_ZoneBeginHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_ZoneBeginHour.FormattingEnabled = true;
            this.cbo_ZoneBeginHour.Location = new System.Drawing.Point(83, 72);
            this.cbo_ZoneBeginHour.Name = "cbo_ZoneBeginHour";
            this.cbo_ZoneBeginHour.Size = new System.Drawing.Size(49, 20);
            this.cbo_ZoneBeginHour.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(138, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btn_GroupGet);
            this.groupBox4.Controls.Add(this.btn_GroupSet);
            this.groupBox4.Controls.Add(this.cbo_ZoneNoThree);
            this.groupBox4.Controls.Add(this.cbo_ZoneNoTwo);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.cbo_ZoneNoOne);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.cbo_GroupMultiUser);
            this.groupBox4.Controls.Add(this.lbl_MultiUser);
            this.groupBox4.Controls.Add(this.cbo_GroupNo);
            this.groupBox4.Controls.Add(this.lbl_GroupZoneNo);
            this.groupBox4.Controls.Add(this.lbl_GroupNo);
            this.groupBox4.Location = new System.Drawing.Point(243, 126);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(509, 157);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Time Group Settings";
            // 
            // btn_GroupGet
            // 
            this.btn_GroupGet.Location = new System.Drawing.Point(347, 70);
            this.btn_GroupGet.Name = "btn_GroupGet";
            this.btn_GroupGet.Size = new System.Drawing.Size(75, 23);
            this.btn_GroupGet.TabIndex = 10;
            this.btn_GroupGet.Text = "Get";
            this.btn_GroupGet.UseVisualStyleBackColor = true;
            this.btn_GroupGet.Click += new System.EventHandler(this.btn_GroupGet_Click);
            // 
            // btn_GroupSet
            // 
            this.btn_GroupSet.Location = new System.Drawing.Point(428, 70);
            this.btn_GroupSet.Name = "btn_GroupSet";
            this.btn_GroupSet.Size = new System.Drawing.Size(75, 23);
            this.btn_GroupSet.TabIndex = 11;
            this.btn_GroupSet.Text = "Set";
            this.btn_GroupSet.UseVisualStyleBackColor = true;
            this.btn_GroupSet.Click += new System.EventHandler(this.btn_GroupSet_Click);
            // 
            // cbo_ZoneNoThree
            // 
            this.cbo_ZoneNoThree.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_ZoneNoThree.FormattingEnabled = true;
            this.cbo_ZoneNoThree.Location = new System.Drawing.Point(233, 46);
            this.cbo_ZoneNoThree.Name = "cbo_ZoneNoThree";
            this.cbo_ZoneNoThree.Size = new System.Drawing.Size(55, 20);
            this.cbo_ZoneNoThree.TabIndex = 9;
            // 
            // cbo_ZoneNoTwo
            // 
            this.cbo_ZoneNoTwo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_ZoneNoTwo.FormattingEnabled = true;
            this.cbo_ZoneNoTwo.Location = new System.Drawing.Point(155, 46);
            this.cbo_ZoneNoTwo.Name = "cbo_ZoneNoTwo";
            this.cbo_ZoneNoTwo.Size = new System.Drawing.Size(55, 20);
            this.cbo_ZoneNoTwo.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(216, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(11, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "-";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbo_ZoneNoOne
            // 
            this.cbo_ZoneNoOne.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_ZoneNoOne.FormattingEnabled = true;
            this.cbo_ZoneNoOne.Location = new System.Drawing.Point(77, 46);
            this.cbo_ZoneNoOne.Name = "cbo_ZoneNoOne";
            this.cbo_ZoneNoOne.Size = new System.Drawing.Size(55, 20);
            this.cbo_ZoneNoOne.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(138, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "-";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbo_GroupMultiUser
            // 
            this.cbo_GroupMultiUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_GroupMultiUser.FormattingEnabled = true;
            this.cbo_GroupMultiUser.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cbo_GroupMultiUser.Location = new System.Drawing.Point(353, 20);
            this.cbo_GroupMultiUser.Name = "cbo_GroupMultiUser";
            this.cbo_GroupMultiUser.Size = new System.Drawing.Size(150, 20);
            this.cbo_GroupMultiUser.TabIndex = 3;
            // 
            // lbl_MultiUser
            // 
            this.lbl_MultiUser.AutoSize = true;
            this.lbl_MultiUser.Location = new System.Drawing.Point(240, 23);
            this.lbl_MultiUser.Name = "lbl_MultiUser";
            this.lbl_MultiUser.Size = new System.Drawing.Size(107, 12);
            this.lbl_MultiUser.TabIndex = 2;
            this.lbl_MultiUser.Text = "Multi User Count:";
            this.lbl_MultiUser.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbo_GroupNo
            // 
            this.cbo_GroupNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_GroupNo.FormattingEnabled = true;
            this.cbo_GroupNo.Location = new System.Drawing.Point(77, 20);
            this.cbo_GroupNo.Name = "cbo_GroupNo";
            this.cbo_GroupNo.Size = new System.Drawing.Size(150, 20);
            this.cbo_GroupNo.TabIndex = 1;
            // 
            // lbl_GroupZoneNo
            // 
            this.lbl_GroupZoneNo.AutoSize = true;
            this.lbl_GroupZoneNo.Location = new System.Drawing.Point(6, 49);
            this.lbl_GroupZoneNo.Name = "lbl_GroupZoneNo";
            this.lbl_GroupZoneNo.Size = new System.Drawing.Size(65, 12);
            this.lbl_GroupZoneNo.TabIndex = 4;
            this.lbl_GroupZoneNo.Text = "Time Zone:";
            this.lbl_GroupZoneNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_GroupNo
            // 
            this.lbl_GroupNo.AutoSize = true;
            this.lbl_GroupNo.Location = new System.Drawing.Point(48, 23);
            this.lbl_GroupNo.Name = "lbl_GroupNo";
            this.lbl_GroupNo.Size = new System.Drawing.Size(23, 12);
            this.lbl_GroupNo.TabIndex = 0;
            this.lbl_GroupNo.Text = "SN:";
            this.lbl_GroupNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.dtp_EndDate);
            this.groupBox5.Controls.Add(this.dtp_BeginDate);
            this.groupBox5.Controls.Add(this.nud_PeriodDIN);
            this.groupBox5.Controls.Add(this.btn_AvailableGet);
            this.groupBox5.Controls.Add(this.btn_AvaiableSet);
            this.groupBox5.Controls.Add(this.lbl_EndDate);
            this.groupBox5.Controls.Add(this.lbl_BeginDate);
            this.groupBox5.Controls.Add(this.lbl_UserId);
            this.groupBox5.Location = new System.Drawing.Point(12, 289);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(740, 110);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "User Available Period Settings";
            // 
            // dtp_EndDate
            // 
            this.dtp_EndDate.CustomFormat = "yyyy-MM-dd";
            this.dtp_EndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_EndDate.Location = new System.Drawing.Point(71, 48);
            this.dtp_EndDate.Name = "dtp_EndDate";
            this.dtp_EndDate.Size = new System.Drawing.Size(150, 21);
            this.dtp_EndDate.TabIndex = 5;
            // 
            // dtp_BeginDate
            // 
            this.dtp_BeginDate.CustomFormat = "yyyy-MM-dd";
            this.dtp_BeginDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_BeginDate.Location = new System.Drawing.Point(584, 17);
            this.dtp_BeginDate.Name = "dtp_BeginDate";
            this.dtp_BeginDate.Size = new System.Drawing.Size(150, 21);
            this.dtp_BeginDate.TabIndex = 3;
            // 
            // nud_PeriodDIN
            // 
            this.nud_PeriodDIN.Location = new System.Drawing.Point(71, 21);
            this.nud_PeriodDIN.Maximum = new decimal(new int[] {
            -1486618625,
            232830643,
            0,
            0});
            this.nud_PeriodDIN.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_PeriodDIN.Name = "nud_PeriodDIN";
            this.nud_PeriodDIN.Size = new System.Drawing.Size(150, 21);
            this.nud_PeriodDIN.TabIndex = 1;
            this.nud_PeriodDIN.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btn_AvailableGet
            // 
            this.btn_AvailableGet.Location = new System.Drawing.Point(578, 71);
            this.btn_AvailableGet.Name = "btn_AvailableGet";
            this.btn_AvailableGet.Size = new System.Drawing.Size(75, 23);
            this.btn_AvailableGet.TabIndex = 6;
            this.btn_AvailableGet.Text = "Get";
            this.btn_AvailableGet.UseVisualStyleBackColor = true;
            this.btn_AvailableGet.Click += new System.EventHandler(this.btn_AvailableGet_Click);
            // 
            // btn_AvaiableSet
            // 
            this.btn_AvaiableSet.Location = new System.Drawing.Point(659, 71);
            this.btn_AvaiableSet.Name = "btn_AvaiableSet";
            this.btn_AvaiableSet.Size = new System.Drawing.Size(75, 23);
            this.btn_AvaiableSet.TabIndex = 7;
            this.btn_AvaiableSet.Text = "Set";
            this.btn_AvaiableSet.UseVisualStyleBackColor = true;
            this.btn_AvaiableSet.Click += new System.EventHandler(this.btn_AvaiableSet_Click);
            // 
            // lbl_EndDate
            // 
            this.lbl_EndDate.AutoSize = true;
            this.lbl_EndDate.Location = new System.Drawing.Point(6, 54);
            this.lbl_EndDate.Name = "lbl_EndDate";
            this.lbl_EndDate.Size = new System.Drawing.Size(59, 12);
            this.lbl_EndDate.TabIndex = 4;
            this.lbl_EndDate.Text = "End Date:";
            this.lbl_EndDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_BeginDate
            // 
            this.lbl_BeginDate.AutoSize = true;
            this.lbl_BeginDate.Location = new System.Drawing.Point(507, 23);
            this.lbl_BeginDate.Name = "lbl_BeginDate";
            this.lbl_BeginDate.Size = new System.Drawing.Size(71, 12);
            this.lbl_BeginDate.TabIndex = 2;
            this.lbl_BeginDate.Text = "Begin Date:";
            this.lbl_BeginDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_UserId
            // 
            this.lbl_UserId.AutoSize = true;
            this.lbl_UserId.Location = new System.Drawing.Point(12, 26);
            this.lbl_UserId.Name = "lbl_UserId";
            this.lbl_UserId.Size = new System.Drawing.Size(53, 12);
            this.lbl_UserId.TabIndex = 0;
            this.lbl_UserId.Text = "User ID:";
            this.lbl_UserId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ACForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 412);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ACForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Access Control Settings";
            this.Load += new System.EventHandler(this.ACForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_CtrlDIN)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PeriodDIN)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_ParamGet;
        private System.Windows.Forms.Button btn_ParamSet;
        private System.Windows.Forms.TextBox txt_ParamValue;
        private System.Windows.Forms.ComboBox cbo_ParamName;
        private System.Windows.Forms.Label lbl_ParamValue;
        private System.Windows.Forms.Label lbl_ParamName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_RoleGet;
        private System.Windows.Forms.Button btn_RoleSet;
        private System.Windows.Forms.Label lbl_RoleGroupNo;
        private System.Windows.Forms.Label lbl_RoleUserId;
        private System.Windows.Forms.ComboBox cbo_RoleGroupNo;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lbl_Weekday;
        private System.Windows.Forms.Label lbl_ZoneNo;
        private System.Windows.Forms.ComboBox cbo_Weekday;
        private System.Windows.Forms.ComboBox cbo_ZoneNo;
        private System.Windows.Forms.Button btn_ZoneGet;
        private System.Windows.Forms.Button btn_ZoneSet;
        private System.Windows.Forms.Label lbl_ZoneEndTime;
        private System.Windows.Forms.ComboBox cbo_ZoneEndHour;
        private System.Windows.Forms.ComboBox cbo_ZoneEndMinute;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbo_ZoneBeginMinute;
        private System.Windows.Forms.Label lbl_ZoneBeginTime;
        private System.Windows.Forms.ComboBox cbo_ZoneBeginHour;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btn_GroupGet;
        private System.Windows.Forms.Button btn_GroupSet;
        private System.Windows.Forms.ComboBox cbo_ZoneNoThree;
        private System.Windows.Forms.ComboBox cbo_ZoneNoTwo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbo_ZoneNoOne;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbo_GroupMultiUser;
        private System.Windows.Forms.Label lbl_MultiUser;
        private System.Windows.Forms.ComboBox cbo_GroupNo;
        private System.Windows.Forms.Label lbl_GroupZoneNo;
        private System.Windows.Forms.Label lbl_GroupNo;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lbl_UserId;
        private System.Windows.Forms.Button btn_AvailableGet;
        private System.Windows.Forms.Button btn_AvaiableSet;
        private System.Windows.Forms.Label lbl_EndDate;
        private System.Windows.Forms.Label lbl_BeginDate;
        private System.Windows.Forms.NumericUpDown nud_PeriodDIN;
        private System.Windows.Forms.NumericUpDown nud_CtrlDIN;
        private System.Windows.Forms.CheckBox chk_RoleEnable;
        private System.Windows.Forms.DateTimePicker dtp_EndDate;
        private System.Windows.Forms.DateTimePicker dtp_BeginDate;
    }
}