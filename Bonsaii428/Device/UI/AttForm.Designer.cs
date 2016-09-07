namespace ZDC2911Demo.UI {
    partial class AttForm {
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chk_MessageEnable = new System.Windows.Forms.CheckBox();
            this.btn_MessageSet = new System.Windows.Forms.Button();
            this.btn_MessageGet = new System.Windows.Forms.Button();
            this.txt_MessageContent = new System.Windows.Forms.TextBox();
            this.lbl_Content = new System.Windows.Forms.Label();
            this.cbo_MessageType = new System.Windows.Forms.ComboBox();
            this.lbl_MessageType = new System.Windows.Forms.Label();
            this.cbo_MessageSound = new System.Windows.Forms.ComboBox();
            this.lbl_MessageSound = new System.Windows.Forms.Label();
            this.dtp_MessageEndDatetime = new System.Windows.Forms.DateTimePicker();
            this.dtp_MessageBeginDatetime = new System.Windows.Forms.DateTimePicker();
            this.nud_MessageID = new System.Windows.Forms.NumericUpDown();
            this.lbl_MessageEndDate = new System.Windows.Forms.Label();
            this.lbl_MessageBeginDate = new System.Windows.Forms.Label();
            this.lbl_UserID = new System.Windows.Forms.Label();
            this.cbo_MessageSN = new System.Windows.Forms.ComboBox();
            this.lbl_MessageSN = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_MessageID)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chk_MessageEnable);
            this.groupBox3.Controls.Add(this.btn_MessageSet);
            this.groupBox3.Controls.Add(this.btn_MessageGet);
            this.groupBox3.Controls.Add(this.txt_MessageContent);
            this.groupBox3.Controls.Add(this.lbl_Content);
            this.groupBox3.Controls.Add(this.cbo_MessageType);
            this.groupBox3.Controls.Add(this.lbl_MessageType);
            this.groupBox3.Controls.Add(this.cbo_MessageSound);
            this.groupBox3.Controls.Add(this.lbl_MessageSound);
            this.groupBox3.Controls.Add(this.dtp_MessageEndDatetime);
            this.groupBox3.Controls.Add(this.dtp_MessageBeginDatetime);
            this.groupBox3.Controls.Add(this.nud_MessageID);
            this.groupBox3.Controls.Add(this.lbl_MessageEndDate);
            this.groupBox3.Controls.Add(this.lbl_MessageBeginDate);
            this.groupBox3.Controls.Add(this.lbl_UserID);
            this.groupBox3.Controls.Add(this.cbo_MessageSN);
            this.groupBox3.Controls.Add(this.lbl_MessageSN);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(532, 203);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Message Settings";
            // 
            // chk_MessageEnable
            // 
            this.chk_MessageEnable.AutoSize = true;
            this.chk_MessageEnable.Location = new System.Drawing.Point(343, 24);
            this.chk_MessageEnable.Name = "chk_MessageEnable";
            this.chk_MessageEnable.Size = new System.Drawing.Size(59, 18);
            this.chk_MessageEnable.TabIndex = 2;
            this.chk_MessageEnable.Text = "Enable";
            this.chk_MessageEnable.UseVisualStyleBackColor = true;
            // 
            // btn_MessageSet
            // 
            this.btn_MessageSet.Location = new System.Drawing.Point(445, 165);
            this.btn_MessageSet.Name = "btn_MessageSet";
            this.btn_MessageSet.Size = new System.Drawing.Size(75, 25);
            this.btn_MessageSet.TabIndex = 16;
            this.btn_MessageSet.Text = "Set";
            this.btn_MessageSet.UseVisualStyleBackColor = true;
            this.btn_MessageSet.Click += new System.EventHandler(this.btn_MessageSet_Click);
            // 
            // btn_MessageGet
            // 
            this.btn_MessageGet.Location = new System.Drawing.Point(364, 165);
            this.btn_MessageGet.Name = "btn_MessageGet";
            this.btn_MessageGet.Size = new System.Drawing.Size(75, 25);
            this.btn_MessageGet.TabIndex = 15;
            this.btn_MessageGet.Text = "Get";
            this.btn_MessageGet.UseVisualStyleBackColor = true;
            this.btn_MessageGet.Click += new System.EventHandler(this.btn_MessageGet_Click);
            // 
            // txt_MessageContent
            // 
            this.txt_MessageContent.Location = new System.Drawing.Point(107, 135);
            this.txt_MessageContent.MaxLength = 30;
            this.txt_MessageContent.Name = "txt_MessageContent";
            this.txt_MessageContent.Size = new System.Drawing.Size(413, 20);
            this.txt_MessageContent.TabIndex = 14;
            // 
            // lbl_Content
            // 
            this.lbl_Content.AutoSize = true;
            this.lbl_Content.Location = new System.Drawing.Point(48, 139);
            this.lbl_Content.Name = "lbl_Content";
            this.lbl_Content.Size = new System.Drawing.Size(47, 13);
            this.lbl_Content.TabIndex = 13;
            this.lbl_Content.Text = "Content:";
            this.lbl_Content.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbo_MessageType
            // 
            this.cbo_MessageType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_MessageType.FormattingEnabled = true;
            this.cbo_MessageType.Items.AddRange(new object[] {
            "All",
            "User",
            "Group"});
            this.cbo_MessageType.Location = new System.Drawing.Point(107, 50);
            this.cbo_MessageType.Name = "cbo_MessageType";
            this.cbo_MessageType.Size = new System.Drawing.Size(150, 21);
            this.cbo_MessageType.TabIndex = 4;
            // 
            // lbl_MessageType
            // 
            this.lbl_MessageType.AutoSize = true;
            this.lbl_MessageType.Location = new System.Drawing.Point(66, 53);
            this.lbl_MessageType.Name = "lbl_MessageType";
            this.lbl_MessageType.Size = new System.Drawing.Size(34, 13);
            this.lbl_MessageType.TabIndex = 3;
            this.lbl_MessageType.Text = "Type:";
            this.lbl_MessageType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbo_MessageSound
            // 
            this.cbo_MessageSound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_MessageSound.FormattingEnabled = true;
            this.cbo_MessageSound.Items.AddRange(new object[] {
            "Default",
            "User Defined 1",
            "User Defined 2"});
            this.cbo_MessageSound.Location = new System.Drawing.Point(107, 107);
            this.cbo_MessageSound.Name = "cbo_MessageSound";
            this.cbo_MessageSound.Size = new System.Drawing.Size(150, 21);
            this.cbo_MessageSound.TabIndex = 12;
            // 
            // lbl_MessageSound
            // 
            this.lbl_MessageSound.AutoSize = true;
            this.lbl_MessageSound.Location = new System.Drawing.Point(60, 111);
            this.lbl_MessageSound.Name = "lbl_MessageSound";
            this.lbl_MessageSound.Size = new System.Drawing.Size(41, 13);
            this.lbl_MessageSound.TabIndex = 11;
            this.lbl_MessageSound.Text = "Sound:";
            this.lbl_MessageSound.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtp_MessageEndDatetime
            // 
            this.dtp_MessageEndDatetime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtp_MessageEndDatetime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_MessageEndDatetime.Location = new System.Drawing.Point(370, 78);
            this.dtp_MessageEndDatetime.MaxDate = new System.DateTime(2099, 12, 31, 0, 0, 0, 0);
            this.dtp_MessageEndDatetime.MinDate = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            this.dtp_MessageEndDatetime.Name = "dtp_MessageEndDatetime";
            this.dtp_MessageEndDatetime.Size = new System.Drawing.Size(150, 20);
            this.dtp_MessageEndDatetime.TabIndex = 10;
            // 
            // dtp_MessageBeginDatetime
            // 
            this.dtp_MessageBeginDatetime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtp_MessageBeginDatetime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_MessageBeginDatetime.Location = new System.Drawing.Point(107, 78);
            this.dtp_MessageBeginDatetime.MaxDate = new System.DateTime(2099, 12, 31, 0, 0, 0, 0);
            this.dtp_MessageBeginDatetime.MinDate = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            this.dtp_MessageBeginDatetime.Name = "dtp_MessageBeginDatetime";
            this.dtp_MessageBeginDatetime.Size = new System.Drawing.Size(150, 20);
            this.dtp_MessageBeginDatetime.TabIndex = 8;
            // 
            // nud_MessageID
            // 
            this.nud_MessageID.Location = new System.Drawing.Point(370, 48);
            this.nud_MessageID.Maximum = new decimal(new int[] {
            -1486618625,
            232830643,
            0,
            0});
            this.nud_MessageID.Name = "nud_MessageID";
            this.nud_MessageID.Size = new System.Drawing.Size(150, 20);
            this.nud_MessageID.TabIndex = 6;
            this.nud_MessageID.Click += new System.EventHandler(this.NumericUpDown_Selected);
            this.nud_MessageID.Enter += new System.EventHandler(this.NumericUpDown_Selected);
            // 
            // lbl_MessageEndDate
            // 
            this.lbl_MessageEndDate.AutoSize = true;
            this.lbl_MessageEndDate.Location = new System.Drawing.Point(281, 85);
            this.lbl_MessageEndDate.Name = "lbl_MessageEndDate";
            this.lbl_MessageEndDate.Size = new System.Drawing.Size(74, 13);
            this.lbl_MessageEndDate.TabIndex = 9;
            this.lbl_MessageEndDate.Text = "End Datetime:";
            this.lbl_MessageEndDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_MessageBeginDate
            // 
            this.lbl_MessageBeginDate.AutoSize = true;
            this.lbl_MessageBeginDate.Location = new System.Drawing.Point(6, 85);
            this.lbl_MessageBeginDate.Name = "lbl_MessageBeginDate";
            this.lbl_MessageBeginDate.Size = new System.Drawing.Size(82, 13);
            this.lbl_MessageBeginDate.TabIndex = 7;
            this.lbl_MessageBeginDate.Text = "Begin Datetime:";
            this.lbl_MessageBeginDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_UserID
            // 
            this.lbl_UserID.AutoSize = true;
            this.lbl_UserID.Location = new System.Drawing.Point(341, 53);
            this.lbl_UserID.Name = "lbl_UserID";
            this.lbl_UserID.Size = new System.Drawing.Size(21, 13);
            this.lbl_UserID.TabIndex = 5;
            this.lbl_UserID.Text = "ID:";
            this.lbl_UserID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbo_MessageSN
            // 
            this.cbo_MessageSN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_MessageSN.FormattingEnabled = true;
            this.cbo_MessageSN.Location = new System.Drawing.Point(107, 22);
            this.cbo_MessageSN.Name = "cbo_MessageSN";
            this.cbo_MessageSN.Size = new System.Drawing.Size(150, 21);
            this.cbo_MessageSN.TabIndex = 1;
            // 
            // lbl_MessageSN
            // 
            this.lbl_MessageSN.AutoSize = true;
            this.lbl_MessageSN.Location = new System.Drawing.Point(78, 25);
            this.lbl_MessageSN.Name = "lbl_MessageSN";
            this.lbl_MessageSN.Size = new System.Drawing.Size(25, 13);
            this.lbl_MessageSN.TabIndex = 0;
            this.lbl_MessageSN.Text = "SN:";
            this.lbl_MessageSN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AttForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 233);
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AttForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Attendance Settings";
            this.Load += new System.EventHandler(this.AttForm_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_MessageID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lbl_MessageBeginDate;
        private System.Windows.Forms.Label lbl_UserID;
        private System.Windows.Forms.ComboBox cbo_MessageSN;
        private System.Windows.Forms.Label lbl_MessageSN;
        private System.Windows.Forms.NumericUpDown nud_MessageID;
        private System.Windows.Forms.DateTimePicker dtp_MessageEndDatetime;
        private System.Windows.Forms.DateTimePicker dtp_MessageBeginDatetime;
        private System.Windows.Forms.Label lbl_MessageEndDate;
        private System.Windows.Forms.TextBox txt_MessageContent;
        private System.Windows.Forms.Label lbl_Content;
        private System.Windows.Forms.ComboBox cbo_MessageSound;
        private System.Windows.Forms.Label lbl_MessageSound;
        private System.Windows.Forms.Button btn_MessageSet;
        private System.Windows.Forms.Button btn_MessageGet;
        private System.Windows.Forms.CheckBox chk_MessageEnable;
        private System.Windows.Forms.ComboBox cbo_MessageType;
        private System.Windows.Forms.Label lbl_MessageType;
    }
}