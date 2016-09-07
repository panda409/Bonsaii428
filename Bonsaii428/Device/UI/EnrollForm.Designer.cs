namespace ZDC2911Demo.UI {
    partial class EnrollForm {
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
            this.lbl_UserId = new System.Windows.Forms.Label();
            this.lbl_FpNo = new System.Windows.Forms.Label();
            this.cbo_FpNo = new System.Windows.Forms.ComboBox();
            this.lbl_Role = new System.Windows.Forms.Label();
            this.cbo_Role = new System.Windows.Forms.ComboBox();
            this.lbl_Pwd = new System.Windows.Forms.Label();
            this.txt_Pwd = new System.Windows.Forms.TextBox();
            this.lbl_Card = new System.Windows.Forms.Label();
            this.txt_Card = new System.Windows.Forms.TextBox();
            this.lbl_UserName = new System.Windows.Forms.Label();
            this.txt_UserName = new System.Windows.Forms.TextBox();
            this.lbl_FpData = new System.Windows.Forms.Label();
            this.txt_UserFpData = new System.Windows.Forms.TextBox();
            this.btn_SaveFpData = new System.Windows.Forms.Button();
            this.btn_OpenFpData = new System.Windows.Forms.Button();
            this.btn_ReadFp = new System.Windows.Forms.Button();
            this.btn_WriteFp = new System.Windows.Forms.Button();
            this.btn_DeleteFp = new System.Windows.Forms.Button();
            this.btn_ReadPwd = new System.Windows.Forms.Button();
            this.btn_WritePwd = new System.Windows.Forms.Button();
            this.btn_DeletePwd = new System.Windows.Forms.Button();
            this.btn_ReadCard = new System.Windows.Forms.Button();
            this.btn_WriteCard = new System.Windows.Forms.Button();
            this.btn_DeleteCard = new System.Windows.Forms.Button();
            this.btn_ReadUserName = new System.Windows.Forms.Button();
            this.btn_WriteUserName = new System.Windows.Forms.Button();
            this.btn_DeleteUser = new System.Windows.Forms.Button();
            this.btn_DeleteAllUser = new System.Windows.Forms.Button();
            this.btn_SetRole = new System.Windows.Forms.Button();
            this.btn_GetAllEnrollData = new System.Windows.Forms.Button();
            this.btn_GetUserInfoByUserId = new System.Windows.Forms.Button();
            this.btn_UploadUserData = new System.Windows.Forms.Button();
            this.btn_DownloadUserData = new System.Windows.Forms.Button();
            this.lvw_UserList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_Select = new System.Windows.Forms.Button();
            this.btn_Open = new System.Windows.Forms.Button();
            this.btn_Save = new System.Windows.Forms.Button();
            this.sfd_SaveFpData = new System.Windows.Forms.SaveFileDialog();
            this.ofd_OpenFpData = new System.Windows.Forms.OpenFileDialog();
            this.ExtInfoLabel = new System.Windows.Forms.Label();
            this.ExtInfoTextBox = new System.Windows.Forms.TextBox();
            this.getExtInfoButton = new System.Windows.Forms.Button();
            this.setExtInfoButton = new System.Windows.Forms.Button();
            this.nud_DIN = new System.Windows.Forms.NumericUpDown();
            this.btn_SaveOneEnroll = new System.Windows.Forms.Button();
            this.btn_OpenOneEnroll = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.userEnableComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.userAttTypeIdNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.userDeptIdNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.userGroupIdNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.userPassZoneNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.userAccessControlComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.userValidityPeriodComboBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.userStartDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.userEndDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.getUserDetailInfoButton = new System.Windows.Forms.Button();
            this.setUserDetailInfoButton = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.userResNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.imageFpEnrollButton = new System.Windows.Forms.Button();
            this.imageFpOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.nud_DIN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userAttTypeIdNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userDeptIdNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userGroupIdNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userPassZoneNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userResNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_UserId
            // 
            this.lbl_UserId.AutoSize = true;
            this.lbl_UserId.Location = new System.Drawing.Point(9, 10);
            this.lbl_UserId.Name = "lbl_UserId";
            this.lbl_UserId.Size = new System.Drawing.Size(46, 13);
            this.lbl_UserId.TabIndex = 0;
            this.lbl_UserId.Text = "User ID:";
            this.lbl_UserId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_FpNo
            // 
            this.lbl_FpNo.AutoSize = true;
            this.lbl_FpNo.Location = new System.Drawing.Point(9, 39);
            this.lbl_FpNo.Name = "lbl_FpNo";
            this.lbl_FpNo.Size = new System.Drawing.Size(57, 13);
            this.lbl_FpNo.TabIndex = 2;
            this.lbl_FpNo.Text = "Finger SN:";
            this.lbl_FpNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbo_FpNo
            // 
            this.cbo_FpNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_FpNo.FormattingEnabled = true;
            this.cbo_FpNo.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.cbo_FpNo.Location = new System.Drawing.Point(83, 36);
            this.cbo_FpNo.Name = "cbo_FpNo";
            this.cbo_FpNo.Size = new System.Drawing.Size(136, 21);
            this.cbo_FpNo.TabIndex = 3;
            // 
            // lbl_Role
            // 
            this.lbl_Role.AutoSize = true;
            this.lbl_Role.Location = new System.Drawing.Point(9, 67);
            this.lbl_Role.Name = "lbl_Role";
            this.lbl_Role.Size = new System.Drawing.Size(50, 13);
            this.lbl_Role.TabIndex = 4;
            this.lbl_Role.Text = "Privilege:";
            this.lbl_Role.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbo_Role
            // 
            this.cbo_Role.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_Role.FormattingEnabled = true;
            this.cbo_Role.Items.AddRange(new object[] {
            "User",
            "Enroll User",
            "View User",
            "Super User",
            "Customer"});
            this.cbo_Role.Location = new System.Drawing.Point(83, 64);
            this.cbo_Role.Name = "cbo_Role";
            this.cbo_Role.Size = new System.Drawing.Size(136, 21);
            this.cbo_Role.TabIndex = 5;
            // 
            // lbl_Pwd
            // 
            this.lbl_Pwd.AutoSize = true;
            this.lbl_Pwd.Location = new System.Drawing.Point(9, 95);
            this.lbl_Pwd.Name = "lbl_Pwd";
            this.lbl_Pwd.Size = new System.Drawing.Size(56, 13);
            this.lbl_Pwd.TabIndex = 6;
            this.lbl_Pwd.Text = "Password:";
            this.lbl_Pwd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_Pwd
            // 
            this.txt_Pwd.Location = new System.Drawing.Point(83, 92);
            this.txt_Pwd.MaxLength = 8;
            this.txt_Pwd.Name = "txt_Pwd";
            this.txt_Pwd.Size = new System.Drawing.Size(136, 20);
            this.txt_Pwd.TabIndex = 7;
            // 
            // lbl_Card
            // 
            this.lbl_Card.AutoSize = true;
            this.lbl_Card.Location = new System.Drawing.Point(9, 125);
            this.lbl_Card.Name = "lbl_Card";
            this.lbl_Card.Size = new System.Drawing.Size(46, 13);
            this.lbl_Card.TabIndex = 8;
            this.lbl_Card.Text = "Card ID:";
            this.lbl_Card.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_Card
            // 
            this.txt_Card.Location = new System.Drawing.Point(83, 121);
            this.txt_Card.MaxLength = 10;
            this.txt_Card.Name = "txt_Card";
            this.txt_Card.Size = new System.Drawing.Size(136, 20);
            this.txt_Card.TabIndex = 9;
            // 
            // lbl_UserName
            // 
            this.lbl_UserName.AutoSize = true;
            this.lbl_UserName.Location = new System.Drawing.Point(9, 154);
            this.lbl_UserName.Name = "lbl_UserName";
            this.lbl_UserName.Size = new System.Drawing.Size(58, 13);
            this.lbl_UserName.TabIndex = 10;
            this.lbl_UserName.Text = "Username:";
            this.lbl_UserName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_UserName
            // 
            this.txt_UserName.Location = new System.Drawing.Point(83, 151);
            this.txt_UserName.Name = "txt_UserName";
            this.txt_UserName.Size = new System.Drawing.Size(136, 20);
            this.txt_UserName.TabIndex = 11;
            // 
            // lbl_FpData
            // 
            this.lbl_FpData.AutoSize = true;
            this.lbl_FpData.Location = new System.Drawing.Point(12, 243);
            this.lbl_FpData.Name = "lbl_FpData";
            this.lbl_FpData.Size = new System.Drawing.Size(88, 13);
            this.lbl_FpData.TabIndex = 12;
            this.lbl_FpData.Text = "Fringerprint Data:";
            this.lbl_FpData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_UserFpData
            // 
            this.txt_UserFpData.BackColor = System.Drawing.Color.White;
            this.txt_UserFpData.Location = new System.Drawing.Point(12, 260);
            this.txt_UserFpData.Multiline = true;
            this.txt_UserFpData.Name = "txt_UserFpData";
            this.txt_UserFpData.ReadOnly = true;
            this.txt_UserFpData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_UserFpData.Size = new System.Drawing.Size(207, 388);
            this.txt_UserFpData.TabIndex = 13;
            // 
            // btn_SaveFpData
            // 
            this.btn_SaveFpData.Location = new System.Drawing.Point(125, 654);
            this.btn_SaveFpData.Name = "btn_SaveFpData";
            this.btn_SaveFpData.Size = new System.Drawing.Size(95, 25);
            this.btn_SaveFpData.TabIndex = 15;
            this.btn_SaveFpData.Text = "Save FP Data";
            this.btn_SaveFpData.UseVisualStyleBackColor = true;
            this.btn_SaveFpData.Click += new System.EventHandler(this.btn_SaveFpData_Click);
            // 
            // btn_OpenFpData
            // 
            this.btn_OpenFpData.Location = new System.Drawing.Point(24, 654);
            this.btn_OpenFpData.Name = "btn_OpenFpData";
            this.btn_OpenFpData.Size = new System.Drawing.Size(95, 25);
            this.btn_OpenFpData.TabIndex = 14;
            this.btn_OpenFpData.Text = "Open FP Data";
            this.btn_OpenFpData.UseVisualStyleBackColor = true;
            this.btn_OpenFpData.Click += new System.EventHandler(this.btn_OpenFpData_Click);
            // 
            // btn_ReadFp
            // 
            this.btn_ReadFp.Location = new System.Drawing.Point(412, 4);
            this.btn_ReadFp.Name = "btn_ReadFp";
            this.btn_ReadFp.Size = new System.Drawing.Size(95, 25);
            this.btn_ReadFp.TabIndex = 16;
            this.btn_ReadFp.Text = "Read FP Data";
            this.btn_ReadFp.UseVisualStyleBackColor = true;
            this.btn_ReadFp.Click += new System.EventHandler(this.btn_ReadFp_Click);
            // 
            // btn_WriteFp
            // 
            this.btn_WriteFp.Location = new System.Drawing.Point(513, 5);
            this.btn_WriteFp.Name = "btn_WriteFp";
            this.btn_WriteFp.Size = new System.Drawing.Size(95, 25);
            this.btn_WriteFp.TabIndex = 17;
            this.btn_WriteFp.Text = "Write FP Data";
            this.btn_WriteFp.UseVisualStyleBackColor = true;
            this.btn_WriteFp.Click += new System.EventHandler(this.btn_WriteFp_Click);
            // 
            // btn_DeleteFp
            // 
            this.btn_DeleteFp.Location = new System.Drawing.Point(614, 5);
            this.btn_DeleteFp.Name = "btn_DeleteFp";
            this.btn_DeleteFp.Size = new System.Drawing.Size(95, 25);
            this.btn_DeleteFp.TabIndex = 18;
            this.btn_DeleteFp.Text = "Clear FP Data";
            this.btn_DeleteFp.UseVisualStyleBackColor = true;
            this.btn_DeleteFp.Click += new System.EventHandler(this.btn_DeleteFp_Click);
            // 
            // btn_ReadPwd
            // 
            this.btn_ReadPwd.Location = new System.Drawing.Point(412, 36);
            this.btn_ReadPwd.Name = "btn_ReadPwd";
            this.btn_ReadPwd.Size = new System.Drawing.Size(95, 25);
            this.btn_ReadPwd.TabIndex = 19;
            this.btn_ReadPwd.Text = "Read PWD";
            this.btn_ReadPwd.UseVisualStyleBackColor = true;
            this.btn_ReadPwd.Click += new System.EventHandler(this.btn_ReadPwd_Click);
            // 
            // btn_WritePwd
            // 
            this.btn_WritePwd.Location = new System.Drawing.Point(513, 34);
            this.btn_WritePwd.Name = "btn_WritePwd";
            this.btn_WritePwd.Size = new System.Drawing.Size(95, 25);
            this.btn_WritePwd.TabIndex = 20;
            this.btn_WritePwd.Text = "Write PWD";
            this.btn_WritePwd.UseVisualStyleBackColor = true;
            this.btn_WritePwd.Click += new System.EventHandler(this.btn_WritePwd_Click);
            // 
            // btn_DeletePwd
            // 
            this.btn_DeletePwd.Location = new System.Drawing.Point(614, 36);
            this.btn_DeletePwd.Name = "btn_DeletePwd";
            this.btn_DeletePwd.Size = new System.Drawing.Size(95, 25);
            this.btn_DeletePwd.TabIndex = 21;
            this.btn_DeletePwd.Text = "Clear PWD";
            this.btn_DeletePwd.UseVisualStyleBackColor = true;
            this.btn_DeletePwd.Click += new System.EventHandler(this.btn_DeletePwd_Click);
            // 
            // btn_ReadCard
            // 
            this.btn_ReadCard.Location = new System.Drawing.Point(412, 67);
            this.btn_ReadCard.Name = "btn_ReadCard";
            this.btn_ReadCard.Size = new System.Drawing.Size(95, 25);
            this.btn_ReadCard.TabIndex = 22;
            this.btn_ReadCard.Text = "Read Card";
            this.btn_ReadCard.UseVisualStyleBackColor = true;
            this.btn_ReadCard.Click += new System.EventHandler(this.btn_ReadCard_Click);
            // 
            // btn_WriteCard
            // 
            this.btn_WriteCard.Location = new System.Drawing.Point(513, 67);
            this.btn_WriteCard.Name = "btn_WriteCard";
            this.btn_WriteCard.Size = new System.Drawing.Size(95, 25);
            this.btn_WriteCard.TabIndex = 23;
            this.btn_WriteCard.Text = "Write Card";
            this.btn_WriteCard.UseVisualStyleBackColor = true;
            this.btn_WriteCard.Click += new System.EventHandler(this.btn_WriteCard_Click);
            // 
            // btn_DeleteCard
            // 
            this.btn_DeleteCard.Location = new System.Drawing.Point(614, 67);
            this.btn_DeleteCard.Name = "btn_DeleteCard";
            this.btn_DeleteCard.Size = new System.Drawing.Size(95, 25);
            this.btn_DeleteCard.TabIndex = 24;
            this.btn_DeleteCard.Text = "Clear Card";
            this.btn_DeleteCard.UseVisualStyleBackColor = true;
            this.btn_DeleteCard.Click += new System.EventHandler(this.btn_DeleteCard_Click);
            // 
            // btn_ReadUserName
            // 
            this.btn_ReadUserName.Location = new System.Drawing.Point(412, 99);
            this.btn_ReadUserName.Name = "btn_ReadUserName";
            this.btn_ReadUserName.Size = new System.Drawing.Size(95, 25);
            this.btn_ReadUserName.TabIndex = 25;
            this.btn_ReadUserName.Text = "Get Username";
            this.btn_ReadUserName.UseVisualStyleBackColor = true;
            this.btn_ReadUserName.Click += new System.EventHandler(this.btn_ReadUserName_Click);
            // 
            // btn_WriteUserName
            // 
            this.btn_WriteUserName.Location = new System.Drawing.Point(513, 99);
            this.btn_WriteUserName.Name = "btn_WriteUserName";
            this.btn_WriteUserName.Size = new System.Drawing.Size(95, 25);
            this.btn_WriteUserName.TabIndex = 26;
            this.btn_WriteUserName.Text = "Set Username";
            this.btn_WriteUserName.UseVisualStyleBackColor = true;
            this.btn_WriteUserName.Click += new System.EventHandler(this.btn_WriteUserName_Click);
            // 
            // btn_DeleteUser
            // 
            this.btn_DeleteUser.Location = new System.Drawing.Point(614, 99);
            this.btn_DeleteUser.Name = "btn_DeleteUser";
            this.btn_DeleteUser.Size = new System.Drawing.Size(95, 25);
            this.btn_DeleteUser.TabIndex = 27;
            this.btn_DeleteUser.Text = "Clear User";
            this.btn_DeleteUser.UseVisualStyleBackColor = true;
            this.btn_DeleteUser.Click += new System.EventHandler(this.btn_DeleteUser_Click);
            // 
            // btn_DeleteAllUser
            // 
            this.btn_DeleteAllUser.Location = new System.Drawing.Point(715, 99);
            this.btn_DeleteAllUser.Name = "btn_DeleteAllUser";
            this.btn_DeleteAllUser.Size = new System.Drawing.Size(99, 25);
            this.btn_DeleteAllUser.TabIndex = 28;
            this.btn_DeleteAllUser.Text = "Clear All User";
            this.btn_DeleteAllUser.UseVisualStyleBackColor = true;
            this.btn_DeleteAllUser.Click += new System.EventHandler(this.btn_DeleteAllUser_Click);
            // 
            // btn_SetRole
            // 
            this.btn_SetRole.Location = new System.Drawing.Point(412, 130);
            this.btn_SetRole.Name = "btn_SetRole";
            this.btn_SetRole.Size = new System.Drawing.Size(95, 25);
            this.btn_SetRole.TabIndex = 29;
            this.btn_SetRole.Text = "Set Privilege";
            this.btn_SetRole.UseVisualStyleBackColor = true;
            this.btn_SetRole.Click += new System.EventHandler(this.btn_SetRole_Click);
            // 
            // btn_GetAllEnrollData
            // 
            this.btn_GetAllEnrollData.Location = new System.Drawing.Point(513, 130);
            this.btn_GetAllEnrollData.Name = "btn_GetAllEnrollData";
            this.btn_GetAllEnrollData.Size = new System.Drawing.Size(196, 25);
            this.btn_GetAllEnrollData.TabIndex = 30;
            this.btn_GetAllEnrollData.Text = "Get All Enroll Information";
            this.btn_GetAllEnrollData.UseVisualStyleBackColor = true;
            this.btn_GetAllEnrollData.Click += new System.EventHandler(this.btn_GetAllEnrollData_Click);
            // 
            // btn_GetUserInfoByUserId
            // 
            this.btn_GetUserInfoByUserId.Location = new System.Drawing.Point(412, 161);
            this.btn_GetUserInfoByUserId.Name = "btn_GetUserInfoByUserId";
            this.btn_GetUserInfoByUserId.Size = new System.Drawing.Size(297, 25);
            this.btn_GetUserInfoByUserId.TabIndex = 31;
            this.btn_GetUserInfoByUserId.Text = "Get Enroll Information By User ID";
            this.btn_GetUserInfoByUserId.UseVisualStyleBackColor = true;
            this.btn_GetUserInfoByUserId.Click += new System.EventHandler(this.btn_GetUserInfoByUserId_Click);
            // 
            // btn_UploadUserData
            // 
            this.btn_UploadUserData.Location = new System.Drawing.Point(563, 223);
            this.btn_UploadUserData.Name = "btn_UploadUserData";
            this.btn_UploadUserData.Size = new System.Drawing.Size(145, 25);
            this.btn_UploadUserData.TabIndex = 32;
            this.btn_UploadUserData.Text = "Set All Enroll Data";
            this.btn_UploadUserData.UseVisualStyleBackColor = true;
            this.btn_UploadUserData.Click += new System.EventHandler(this.btn_UploadUserData_Click);
            // 
            // btn_DownloadUserData
            // 
            this.btn_DownloadUserData.Location = new System.Drawing.Point(412, 223);
            this.btn_DownloadUserData.Name = "btn_DownloadUserData";
            this.btn_DownloadUserData.Size = new System.Drawing.Size(145, 25);
            this.btn_DownloadUserData.TabIndex = 33;
            this.btn_DownloadUserData.Text = "Get All Enroll Data";
            this.btn_DownloadUserData.UseVisualStyleBackColor = true;
            this.btn_DownloadUserData.Click += new System.EventHandler(this.btn_DownloadUserData_Click);
            // 
            // lvw_UserList
            // 
            this.lvw_UserList.CheckBoxes = true;
            this.lvw_UserList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader14,
            this.columnHeader15,
            this.columnHeader16});
            this.lvw_UserList.FullRowSelect = true;
            this.lvw_UserList.GridLines = true;
            this.lvw_UserList.HideSelection = false;
            this.lvw_UserList.Location = new System.Drawing.Point(239, 259);
            this.lvw_UserList.Name = "lvw_UserList";
            this.lvw_UserList.Size = new System.Drawing.Size(571, 388);
            this.lvw_UserList.TabIndex = 35;
            this.lvw_UserList.UseCompatibleStateImageBehavior = false;
            this.lvw_UserList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "SN";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "User ID";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Username";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 120;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "FP 0";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "FP 1";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "FP 2";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "FP 3";
            this.columnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "FP 4";
            this.columnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "FP 5";
            this.columnHeader9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "FP 6";
            this.columnHeader10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "FP 7";
            this.columnHeader11.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "FP 8";
            this.columnHeader12.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "FP 9";
            this.columnHeader13.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "PWD";
            this.columnHeader14.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "Card";
            this.columnHeader15.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "Privilege";
            this.columnHeader16.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader16.Width = 120;
            // 
            // btn_Select
            // 
            this.btn_Select.Location = new System.Drawing.Point(261, 657);
            this.btn_Select.Name = "btn_Select";
            this.btn_Select.Size = new System.Drawing.Size(105, 25);
            this.btn_Select.TabIndex = 36;
            this.btn_Select.Text = "Select All";
            this.btn_Select.UseVisualStyleBackColor = true;
            this.btn_Select.Click += new System.EventHandler(this.btn_Select_Click);
            // 
            // btn_Open
            // 
            this.btn_Open.Location = new System.Drawing.Point(483, 657);
            this.btn_Open.Name = "btn_Open";
            this.btn_Open.Size = new System.Drawing.Size(105, 25);
            this.btn_Open.TabIndex = 37;
            this.btn_Open.Text = "Open All Enroll";
            this.btn_Open.UseVisualStyleBackColor = true;
            this.btn_Open.Click += new System.EventHandler(this.btn_Open_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(372, 657);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(105, 25);
            this.btn_Save.TabIndex = 38;
            this.btn_Save.Text = "Save All Enroll";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // sfd_SaveFpData
            // 
            this.sfd_SaveFpData.Filter = "Template File|*.fpt";
            // 
            // ofd_OpenFpData
            // 
            this.ofd_OpenFpData.Filter = "Template File|*.fpt";
            // 
            // ExtInfoLabel
            // 
            this.ExtInfoLabel.AutoSize = true;
            this.ExtInfoLabel.Location = new System.Drawing.Point(9, 181);
            this.ExtInfoLabel.Name = "ExtInfoLabel";
            this.ExtInfoLabel.Size = new System.Drawing.Size(43, 13);
            this.ExtInfoLabel.TabIndex = 10;
            this.ExtInfoLabel.Text = "ExtInfo:";
            this.ExtInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ExtInfoTextBox
            // 
            this.ExtInfoTextBox.Location = new System.Drawing.Point(83, 178);
            this.ExtInfoTextBox.Name = "ExtInfoTextBox";
            this.ExtInfoTextBox.Size = new System.Drawing.Size(136, 20);
            this.ExtInfoTextBox.TabIndex = 11;
            // 
            // getExtInfoButton
            // 
            this.getExtInfoButton.Location = new System.Drawing.Point(715, 132);
            this.getExtInfoButton.Name = "getExtInfoButton";
            this.getExtInfoButton.Size = new System.Drawing.Size(99, 23);
            this.getExtInfoButton.TabIndex = 39;
            this.getExtInfoButton.Text = "Get ext info";
            this.getExtInfoButton.UseVisualStyleBackColor = true;
            this.getExtInfoButton.Click += new System.EventHandler(this.getExtInfoButton_Click);
            // 
            // setExtInfoButton
            // 
            this.setExtInfoButton.Location = new System.Drawing.Point(715, 161);
            this.setExtInfoButton.Name = "setExtInfoButton";
            this.setExtInfoButton.Size = new System.Drawing.Size(99, 23);
            this.setExtInfoButton.TabIndex = 40;
            this.setExtInfoButton.Text = "Set ext info";
            this.setExtInfoButton.UseVisualStyleBackColor = true;
            this.setExtInfoButton.Click += new System.EventHandler(this.setExtInfoButton_Click);
            // 
            // nud_DIN
            // 
            this.nud_DIN.Location = new System.Drawing.Point(83, 8);
            this.nud_DIN.Maximum = new decimal(new int[] {
            -1486618625,
            232830643,
            0,
            0});
            this.nud_DIN.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_DIN.Name = "nud_DIN";
            this.nud_DIN.Size = new System.Drawing.Size(136, 20);
            this.nud_DIN.TabIndex = 1;
            this.nud_DIN.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btn_SaveOneEnroll
            // 
            this.btn_SaveOneEnroll.Location = new System.Drawing.Point(594, 657);
            this.btn_SaveOneEnroll.Name = "btn_SaveOneEnroll";
            this.btn_SaveOneEnroll.Size = new System.Drawing.Size(105, 25);
            this.btn_SaveOneEnroll.TabIndex = 41;
            this.btn_SaveOneEnroll.Text = "Save One Enroll";
            this.btn_SaveOneEnroll.UseVisualStyleBackColor = true;
            this.btn_SaveOneEnroll.Click += new System.EventHandler(this.btn_SaveOneEnroll_Click);
            // 
            // btn_OpenOneEnroll
            // 
            this.btn_OpenOneEnroll.Location = new System.Drawing.Point(705, 657);
            this.btn_OpenOneEnroll.Name = "btn_OpenOneEnroll";
            this.btn_OpenOneEnroll.Size = new System.Drawing.Size(105, 25);
            this.btn_OpenOneEnroll.TabIndex = 42;
            this.btn_OpenOneEnroll.Text = "Open One Enroll";
            this.btn_OpenOneEnroll.UseVisualStyleBackColor = true;
            this.btn_OpenOneEnroll.Click += new System.EventHandler(this.btn_OpenOneEnroll_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(236, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enable:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // userEnableComboBox
            // 
            this.userEnableComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.userEnableComboBox.FormattingEnabled = true;
            this.userEnableComboBox.Items.AddRange(new object[] {
            "No",
            "Yes"});
            this.userEnableComboBox.Location = new System.Drawing.Point(311, 7);
            this.userEnableComboBox.Name = "userEnableComboBox";
            this.userEnableComboBox.Size = new System.Drawing.Size(81, 21);
            this.userEnableComboBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(236, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Att type ID:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // userAttTypeIdNumericUpDown
            // 
            this.userAttTypeIdNumericUpDown.Location = new System.Drawing.Point(311, 36);
            this.userAttTypeIdNumericUpDown.Name = "userAttTypeIdNumericUpDown";
            this.userAttTypeIdNumericUpDown.Size = new System.Drawing.Size(81, 20);
            this.userAttTypeIdNumericUpDown.TabIndex = 43;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(236, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Dept ID:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // userDeptIdNumericUpDown
            // 
            this.userDeptIdNumericUpDown.Location = new System.Drawing.Point(311, 62);
            this.userDeptIdNumericUpDown.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.userDeptIdNumericUpDown.Name = "userDeptIdNumericUpDown";
            this.userDeptIdNumericUpDown.Size = new System.Drawing.Size(81, 20);
            this.userDeptIdNumericUpDown.TabIndex = 43;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(236, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Group ID:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // userGroupIdNumericUpDown
            // 
            this.userGroupIdNumericUpDown.Location = new System.Drawing.Point(311, 92);
            this.userGroupIdNumericUpDown.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.userGroupIdNumericUpDown.Name = "userGroupIdNumericUpDown";
            this.userGroupIdNumericUpDown.Size = new System.Drawing.Size(81, 20);
            this.userGroupIdNumericUpDown.TabIndex = 43;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(236, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Pass-zone:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // userPassZoneNumericUpDown
            // 
            this.userPassZoneNumericUpDown.Location = new System.Drawing.Point(311, 121);
            this.userPassZoneNumericUpDown.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.userPassZoneNumericUpDown.Name = "userPassZoneNumericUpDown";
            this.userPassZoneNumericUpDown.Size = new System.Drawing.Size(81, 20);
            this.userPassZoneNumericUpDown.TabIndex = 43;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(236, 154);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "lock control:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // userAccessControlComboBox
            // 
            this.userAccessControlComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.userAccessControlComboBox.FormattingEnabled = true;
            this.userAccessControlComboBox.Items.AddRange(new object[] {
            "None",
            "lock 1 only",
            "lock 2 only",
            "lock 1 & 2"});
            this.userAccessControlComboBox.Location = new System.Drawing.Point(311, 150);
            this.userAccessControlComboBox.Name = "userAccessControlComboBox";
            this.userAccessControlComboBox.Size = new System.Drawing.Size(81, 21);
            this.userAccessControlComboBox.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(236, 181);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Validity period:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // userValidityPeriodComboBox
            // 
            this.userValidityPeriodComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.userValidityPeriodComboBox.FormattingEnabled = true;
            this.userValidityPeriodComboBox.Items.AddRange(new object[] {
            "No",
            "Yes"});
            this.userValidityPeriodComboBox.Location = new System.Drawing.Point(311, 177);
            this.userValidityPeriodComboBox.Name = "userValidityPeriodComboBox";
            this.userValidityPeriodComboBox.Size = new System.Drawing.Size(81, 21);
            this.userValidityPeriodComboBox.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 210);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Start:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // userStartDateTimePicker
            // 
            this.userStartDateTimePicker.CustomFormat = "yyyy-MM-dd HH:mm";
            this.userStartDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.userStartDateTimePicker.Location = new System.Drawing.Point(83, 206);
            this.userStartDateTimePicker.Name = "userStartDateTimePicker";
            this.userStartDateTimePicker.Size = new System.Drawing.Size(136, 20);
            this.userStartDateTimePicker.TabIndex = 44;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(230, 210);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "End:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // userEndDateTimePicker
            // 
            this.userEndDateTimePicker.CustomFormat = "yyyy-MM-dd HH:mm";
            this.userEndDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.userEndDateTimePicker.Location = new System.Drawing.Point(265, 206);
            this.userEndDateTimePicker.Name = "userEndDateTimePicker";
            this.userEndDateTimePicker.Size = new System.Drawing.Size(127, 20);
            this.userEndDateTimePicker.TabIndex = 45;
            // 
            // getUserDetailInfoButton
            // 
            this.getUserDetailInfoButton.Location = new System.Drawing.Point(412, 192);
            this.getUserDetailInfoButton.Name = "getUserDetailInfoButton";
            this.getUserDetailInfoButton.Size = new System.Drawing.Size(145, 25);
            this.getUserDetailInfoButton.TabIndex = 32;
            this.getUserDetailInfoButton.Text = "Get user detail info.";
            this.getUserDetailInfoButton.UseVisualStyleBackColor = true;
            this.getUserDetailInfoButton.Click += new System.EventHandler(this.getUserDetailInfoButton_Click);
            // 
            // setUserDetailInfoButton
            // 
            this.setUserDetailInfoButton.Location = new System.Drawing.Point(563, 192);
            this.setUserDetailInfoButton.Name = "setUserDetailInfoButton";
            this.setUserDetailInfoButton.Size = new System.Drawing.Size(145, 25);
            this.setUserDetailInfoButton.TabIndex = 33;
            this.setUserDetailInfoButton.Text = "Set user detail info.";
            this.setUserDetailInfoButton.UseVisualStyleBackColor = true;
            this.setUserDetailInfoButton.Click += new System.EventHandler(this.setUserDetailInfoButton_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(236, 233);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(50, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Reserve:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // userResNumericUpDown
            // 
            this.userResNumericUpDown.Location = new System.Drawing.Point(311, 229);
            this.userResNumericUpDown.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.userResNumericUpDown.Name = "userResNumericUpDown";
            this.userResNumericUpDown.Size = new System.Drawing.Size(81, 20);
            this.userResNumericUpDown.TabIndex = 43;
            // 
            // imageFpEnrollButton
            // 
            this.imageFpEnrollButton.Location = new System.Drawing.Point(715, 194);
            this.imageFpEnrollButton.Name = "imageFpEnrollButton";
            this.imageFpEnrollButton.Size = new System.Drawing.Size(100, 23);
            this.imageFpEnrollButton.TabIndex = 47;
            this.imageFpEnrollButton.Text = "Image Fp Enroll";
            this.imageFpEnrollButton.UseVisualStyleBackColor = true;
            this.imageFpEnrollButton.Click += new System.EventHandler(this.imageFpEnrollButton_Click);
            // 
            // imageFpOpenFileDialog
            // 
            this.imageFpOpenFileDialog.FileName = "openFileDialog1";
            this.imageFpOpenFileDialog.Filter = "Image|*.jpg;*.bmp;*.png;*.gif|JPEG(.jpg)|*.jpg|PNG(.png)|*.png|GIF(.gif)|*.gif";
            // 
            // EnrollForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 699);
            this.Controls.Add(this.imageFpEnrollButton);
            this.Controls.Add(this.userEndDateTimePicker);
            this.Controls.Add(this.userStartDateTimePicker);
            this.Controls.Add(this.userResNumericUpDown);
            this.Controls.Add(this.userPassZoneNumericUpDown);
            this.Controls.Add(this.userGroupIdNumericUpDown);
            this.Controls.Add(this.userDeptIdNumericUpDown);
            this.Controls.Add(this.userAttTypeIdNumericUpDown);
            this.Controls.Add(this.btn_OpenOneEnroll);
            this.Controls.Add(this.btn_SaveOneEnroll);
            this.Controls.Add(this.nud_DIN);
            this.Controls.Add(this.setExtInfoButton);
            this.Controls.Add(this.getExtInfoButton);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.btn_Open);
            this.Controls.Add(this.btn_Select);
            this.Controls.Add(this.lvw_UserList);
            this.Controls.Add(this.btn_DeleteFp);
            this.Controls.Add(this.btn_WriteFp);
            this.Controls.Add(this.btn_DeletePwd);
            this.Controls.Add(this.btn_WritePwd);
            this.Controls.Add(this.btn_DeleteCard);
            this.Controls.Add(this.btn_WriteCard);
            this.Controls.Add(this.btn_DeleteAllUser);
            this.Controls.Add(this.btn_DeleteUser);
            this.Controls.Add(this.btn_WriteUserName);
            this.Controls.Add(this.setUserDetailInfoButton);
            this.Controls.Add(this.btn_DownloadUserData);
            this.Controls.Add(this.getUserDetailInfoButton);
            this.Controls.Add(this.btn_UploadUserData);
            this.Controls.Add(this.btn_GetUserInfoByUserId);
            this.Controls.Add(this.btn_GetAllEnrollData);
            this.Controls.Add(this.btn_SetRole);
            this.Controls.Add(this.btn_ReadUserName);
            this.Controls.Add(this.btn_ReadCard);
            this.Controls.Add(this.btn_ReadPwd);
            this.Controls.Add(this.btn_ReadFp);
            this.Controls.Add(this.btn_OpenFpData);
            this.Controls.Add(this.btn_SaveFpData);
            this.Controls.Add(this.userAccessControlComboBox);
            this.Controls.Add(this.userValidityPeriodComboBox);
            this.Controls.Add(this.userEnableComboBox);
            this.Controls.Add(this.cbo_Role);
            this.Controls.Add(this.cbo_FpNo);
            this.Controls.Add(this.txt_UserFpData);
            this.Controls.Add(this.ExtInfoTextBox);
            this.Controls.Add(this.txt_UserName);
            this.Controls.Add(this.txt_Card);
            this.Controls.Add(this.txt_Pwd);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.ExtInfoLabel);
            this.Controls.Add(this.lbl_FpData);
            this.Controls.Add(this.lbl_UserName);
            this.Controls.Add(this.lbl_Card);
            this.Controls.Add(this.lbl_Pwd);
            this.Controls.Add(this.lbl_Role);
            this.Controls.Add(this.lbl_FpNo);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_UserId);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "EnrollForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Enroll Data Management";
            ((System.ComponentModel.ISupportInitialize)(this.nud_DIN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userAttTypeIdNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userDeptIdNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userGroupIdNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userPassZoneNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userResNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_UserId;
        private System.Windows.Forms.Label lbl_FpNo;
        private System.Windows.Forms.ComboBox cbo_FpNo;
        private System.Windows.Forms.Label lbl_Role;
        private System.Windows.Forms.ComboBox cbo_Role;
        private System.Windows.Forms.Label lbl_Pwd;
        private System.Windows.Forms.TextBox txt_Pwd;
        private System.Windows.Forms.Label lbl_Card;
        private System.Windows.Forms.TextBox txt_Card;
        private System.Windows.Forms.Label lbl_UserName;
        private System.Windows.Forms.TextBox txt_UserName;
        private System.Windows.Forms.Label lbl_FpData;
        private System.Windows.Forms.TextBox txt_UserFpData;
        private System.Windows.Forms.Button btn_SaveFpData;
        private System.Windows.Forms.Button btn_OpenFpData;
        private System.Windows.Forms.Button btn_ReadFp;
        private System.Windows.Forms.Button btn_WriteFp;
        private System.Windows.Forms.Button btn_DeleteFp;
        private System.Windows.Forms.Button btn_ReadPwd;
        private System.Windows.Forms.Button btn_WritePwd;
        private System.Windows.Forms.Button btn_DeletePwd;
        private System.Windows.Forms.Button btn_ReadCard;
        private System.Windows.Forms.Button btn_WriteCard;
        private System.Windows.Forms.Button btn_DeleteCard;
        private System.Windows.Forms.Button btn_ReadUserName;
        private System.Windows.Forms.Button btn_WriteUserName;
        private System.Windows.Forms.Button btn_DeleteUser;
        private System.Windows.Forms.Button btn_DeleteAllUser;
        private System.Windows.Forms.Button btn_SetRole;
        private System.Windows.Forms.Button btn_GetAllEnrollData;
        private System.Windows.Forms.Button btn_GetUserInfoByUserId;
        private System.Windows.Forms.Button btn_UploadUserData;
        private System.Windows.Forms.Button btn_DownloadUserData;
        private System.Windows.Forms.ListView lvw_UserList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.ColumnHeader columnHeader16;
        private System.Windows.Forms.Button btn_Select;
        private System.Windows.Forms.Button btn_Open;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.SaveFileDialog sfd_SaveFpData;
        private System.Windows.Forms.OpenFileDialog ofd_OpenFpData;
        private System.Windows.Forms.Label ExtInfoLabel;
        private System.Windows.Forms.TextBox ExtInfoTextBox;
        private System.Windows.Forms.Button getExtInfoButton;
        private System.Windows.Forms.Button setExtInfoButton;
        private System.Windows.Forms.NumericUpDown nud_DIN;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btn_SaveOneEnroll;
        private System.Windows.Forms.Button btn_OpenOneEnroll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox userEnableComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown userAttTypeIdNumericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown userDeptIdNumericUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown userGroupIdNumericUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown userPassZoneNumericUpDown;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox userAccessControlComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox userValidityPeriodComboBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker userStartDateTimePicker;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker userEndDateTimePicker;
        private System.Windows.Forms.Button getUserDetailInfoButton;
        private System.Windows.Forms.Button setUserDetailInfoButton;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown userResNumericUpDown;
        private System.Windows.Forms.Button imageFpEnrollButton;
        private System.Windows.Forms.OpenFileDialog imageFpOpenFileDialog;
    }
}