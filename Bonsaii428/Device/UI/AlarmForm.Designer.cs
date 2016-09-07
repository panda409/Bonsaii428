namespace ZDC2911Demo.UI {
    partial class AlarmForm {
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
            this.nud_AlarmDelay = new System.Windows.Forms.NumericUpDown();
            this.btn_AlarmGet = new System.Windows.Forms.Button();
            this.btn_AlarmSet = new System.Windows.Forms.Button();
            this.cbo_AlarmMinute = new System.Windows.Forms.ComboBox();
            this.cbo_AlarmHour = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_AlarmDelay = new System.Windows.Forms.Label();
            this.lbl_AlarmTime = new System.Windows.Forms.Label();
            this.cbo_AlarmCycle = new System.Windows.Forms.ComboBox();
            this.cbo_AlarmDN = new System.Windows.Forms.ComboBox();
            this.lbl_AlarmCycle = new System.Windows.Forms.Label();
            this.lbl_AlarmDN = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_AttGet = new System.Windows.Forms.Button();
            this.btn_AttSet = new System.Windows.Forms.Button();
            this.cbo_EndMinute = new System.Windows.Forms.ComboBox();
            this.cbo_EndHour = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_EndTime = new System.Windows.Forms.Label();
            this.cbo_BeginMinute = new System.Windows.Forms.ComboBox();
            this.cbo_BeginHour = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_BeginTime = new System.Windows.Forms.Label();
            this.cbo_AttDN = new System.Windows.Forms.ComboBox();
            this.lbl_AttDN = new System.Windows.Forms.Label();
            this.cbo_AttType = new System.Windows.Forms.ComboBox();
            this.lbl_AttType = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.nud_DIN = new System.Windows.Forms.NumericUpDown();
            this.btn_UserGet = new System.Windows.Forms.Button();
            this.btn_UserSet = new System.Windows.Forms.Button();
            this.cbo_UserType = new System.Windows.Forms.ComboBox();
            this.lbl_UserType = new System.Windows.Forms.Label();
            this.lbl_UserId = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_AlarmDelay)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_DIN)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nud_AlarmDelay);
            this.groupBox1.Controls.Add(this.btn_AlarmGet);
            this.groupBox1.Controls.Add(this.btn_AlarmSet);
            this.groupBox1.Controls.Add(this.cbo_AlarmMinute);
            this.groupBox1.Controls.Add(this.cbo_AlarmHour);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lbl_AlarmDelay);
            this.groupBox1.Controls.Add(this.lbl_AlarmTime);
            this.groupBox1.Controls.Add(this.cbo_AlarmCycle);
            this.groupBox1.Controls.Add(this.cbo_AlarmDN);
            this.groupBox1.Controls.Add(this.lbl_AlarmCycle);
            this.groupBox1.Controls.Add(this.lbl_AlarmDN);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(544, 118);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Alarm Settings";
            // 
            // nud_AlarmDelay
            // 
            this.nud_AlarmDelay.Location = new System.Drawing.Point(378, 53);
            this.nud_AlarmDelay.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nud_AlarmDelay.Name = "nud_AlarmDelay";
            this.nud_AlarmDelay.Size = new System.Drawing.Size(150, 20);
            this.nud_AlarmDelay.TabIndex = 9;
            // 
            // btn_AlarmGet
            // 
            this.btn_AlarmGet.Location = new System.Drawing.Point(372, 81);
            this.btn_AlarmGet.Name = "btn_AlarmGet";
            this.btn_AlarmGet.Size = new System.Drawing.Size(75, 25);
            this.btn_AlarmGet.TabIndex = 10;
            this.btn_AlarmGet.Text = "Get";
            this.btn_AlarmGet.UseVisualStyleBackColor = true;
            this.btn_AlarmGet.Click += new System.EventHandler(this.btn_AlarmGet_Click);
            // 
            // btn_AlarmSet
            // 
            this.btn_AlarmSet.Location = new System.Drawing.Point(453, 81);
            this.btn_AlarmSet.Name = "btn_AlarmSet";
            this.btn_AlarmSet.Size = new System.Drawing.Size(75, 25);
            this.btn_AlarmSet.TabIndex = 11;
            this.btn_AlarmSet.Text = "Set";
            this.btn_AlarmSet.UseVisualStyleBackColor = true;
            this.btn_AlarmSet.Click += new System.EventHandler(this.btn_AlarmSet_Click);
            // 
            // cbo_AlarmMinute
            // 
            this.cbo_AlarmMinute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_AlarmMinute.FormattingEnabled = true;
            this.cbo_AlarmMinute.Location = new System.Drawing.Point(468, 22);
            this.cbo_AlarmMinute.Name = "cbo_AlarmMinute";
            this.cbo_AlarmMinute.Size = new System.Drawing.Size(60, 21);
            this.cbo_AlarmMinute.TabIndex = 5;
            // 
            // cbo_AlarmHour
            // 
            this.cbo_AlarmHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_AlarmHour.FormattingEnabled = true;
            this.cbo_AlarmHour.Location = new System.Drawing.Point(378, 22);
            this.cbo_AlarmHour.Name = "cbo_AlarmHour";
            this.cbo_AlarmHour.Size = new System.Drawing.Size(60, 21);
            this.cbo_AlarmHour.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(448, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_AlarmDelay
            // 
            this.lbl_AlarmDelay.AutoSize = true;
            this.lbl_AlarmDelay.Location = new System.Drawing.Point(331, 55);
            this.lbl_AlarmDelay.Name = "lbl_AlarmDelay";
            this.lbl_AlarmDelay.Size = new System.Drawing.Size(37, 13);
            this.lbl_AlarmDelay.TabIndex = 8;
            this.lbl_AlarmDelay.Text = "Delay:";
            this.lbl_AlarmDelay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_AlarmTime
            // 
            this.lbl_AlarmTime.AutoSize = true;
            this.lbl_AlarmTime.Location = new System.Drawing.Point(337, 25);
            this.lbl_AlarmTime.Name = "lbl_AlarmTime";
            this.lbl_AlarmTime.Size = new System.Drawing.Size(33, 13);
            this.lbl_AlarmTime.TabIndex = 2;
            this.lbl_AlarmTime.Text = "Time:";
            this.lbl_AlarmTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbo_AlarmCycle
            // 
            this.cbo_AlarmCycle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_AlarmCycle.FormattingEnabled = true;
            this.cbo_AlarmCycle.Items.AddRange(new object[] {
            "Every day",
            "Sunday",
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday"});
            this.cbo_AlarmCycle.Location = new System.Drawing.Point(53, 52);
            this.cbo_AlarmCycle.Name = "cbo_AlarmCycle";
            this.cbo_AlarmCycle.Size = new System.Drawing.Size(150, 21);
            this.cbo_AlarmCycle.TabIndex = 7;
            // 
            // cbo_AlarmDN
            // 
            this.cbo_AlarmDN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_AlarmDN.FormattingEnabled = true;
            this.cbo_AlarmDN.Location = new System.Drawing.Point(53, 22);
            this.cbo_AlarmDN.Name = "cbo_AlarmDN";
            this.cbo_AlarmDN.Size = new System.Drawing.Size(150, 21);
            this.cbo_AlarmDN.TabIndex = 1;
            // 
            // lbl_AlarmCycle
            // 
            this.lbl_AlarmCycle.AutoSize = true;
            this.lbl_AlarmCycle.Location = new System.Drawing.Point(6, 55);
            this.lbl_AlarmCycle.Name = "lbl_AlarmCycle";
            this.lbl_AlarmCycle.Size = new System.Drawing.Size(36, 13);
            this.lbl_AlarmCycle.TabIndex = 6;
            this.lbl_AlarmCycle.Text = "Cycle:";
            this.lbl_AlarmCycle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_AlarmDN
            // 
            this.lbl_AlarmDN.AutoSize = true;
            this.lbl_AlarmDN.Location = new System.Drawing.Point(24, 25);
            this.lbl_AlarmDN.Name = "lbl_AlarmDN";
            this.lbl_AlarmDN.Size = new System.Drawing.Size(25, 13);
            this.lbl_AlarmDN.TabIndex = 0;
            this.lbl_AlarmDN.Text = "SN:";
            this.lbl_AlarmDN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_AttGet);
            this.groupBox2.Controls.Add(this.btn_AttSet);
            this.groupBox2.Controls.Add(this.cbo_EndMinute);
            this.groupBox2.Controls.Add(this.cbo_EndHour);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.lbl_EndTime);
            this.groupBox2.Controls.Add(this.cbo_BeginMinute);
            this.groupBox2.Controls.Add(this.cbo_BeginHour);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.lbl_BeginTime);
            this.groupBox2.Controls.Add(this.cbo_AttDN);
            this.groupBox2.Controls.Add(this.lbl_AttDN);
            this.groupBox2.Controls.Add(this.cbo_AttType);
            this.groupBox2.Controls.Add(this.lbl_AttType);
            this.groupBox2.Location = new System.Drawing.Point(12, 138);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(544, 117);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Valid Attendance Time Settings";
            this.groupBox2.Visible = false;
            // 
            // btn_AttGet
            // 
            this.btn_AttGet.Location = new System.Drawing.Point(372, 78);
            this.btn_AttGet.Name = "btn_AttGet";
            this.btn_AttGet.Size = new System.Drawing.Size(75, 25);
            this.btn_AttGet.TabIndex = 12;
            this.btn_AttGet.Text = "Get";
            this.btn_AttGet.UseVisualStyleBackColor = true;
            this.btn_AttGet.Click += new System.EventHandler(this.btn_AttGet_Click);
            // 
            // btn_AttSet
            // 
            this.btn_AttSet.Location = new System.Drawing.Point(453, 78);
            this.btn_AttSet.Name = "btn_AttSet";
            this.btn_AttSet.Size = new System.Drawing.Size(75, 25);
            this.btn_AttSet.TabIndex = 13;
            this.btn_AttSet.Text = "Set";
            this.btn_AttSet.UseVisualStyleBackColor = true;
            this.btn_AttSet.Click += new System.EventHandler(this.btn_AttSet_Click);
            // 
            // cbo_EndMinute
            // 
            this.cbo_EndMinute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_EndMinute.FormattingEnabled = true;
            this.cbo_EndMinute.Location = new System.Drawing.Point(468, 50);
            this.cbo_EndMinute.Name = "cbo_EndMinute";
            this.cbo_EndMinute.Size = new System.Drawing.Size(60, 21);
            this.cbo_EndMinute.TabIndex = 11;
            // 
            // cbo_EndHour
            // 
            this.cbo_EndHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_EndHour.FormattingEnabled = true;
            this.cbo_EndHour.Location = new System.Drawing.Point(378, 50);
            this.cbo_EndHour.Name = "cbo_EndHour";
            this.cbo_EndHour.Size = new System.Drawing.Size(60, 21);
            this.cbo_EndHour.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(448, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_EndTime
            // 
            this.lbl_EndTime.AutoSize = true;
            this.lbl_EndTime.Location = new System.Drawing.Point(313, 53);
            this.lbl_EndTime.Name = "lbl_EndTime";
            this.lbl_EndTime.Size = new System.Drawing.Size(55, 13);
            this.lbl_EndTime.TabIndex = 8;
            this.lbl_EndTime.Text = "End Time:";
            this.lbl_EndTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbo_BeginMinute
            // 
            this.cbo_BeginMinute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_BeginMinute.FormattingEnabled = true;
            this.cbo_BeginMinute.Location = new System.Drawing.Point(172, 50);
            this.cbo_BeginMinute.Name = "cbo_BeginMinute";
            this.cbo_BeginMinute.Size = new System.Drawing.Size(60, 21);
            this.cbo_BeginMinute.TabIndex = 7;
            // 
            // cbo_BeginHour
            // 
            this.cbo_BeginHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_BeginHour.FormattingEnabled = true;
            this.cbo_BeginHour.Location = new System.Drawing.Point(83, 50);
            this.cbo_BeginHour.Name = "cbo_BeginHour";
            this.cbo_BeginHour.Size = new System.Drawing.Size(60, 21);
            this.cbo_BeginHour.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(149, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_BeginTime
            // 
            this.lbl_BeginTime.AutoSize = true;
            this.lbl_BeginTime.Location = new System.Drawing.Point(6, 53);
            this.lbl_BeginTime.Name = "lbl_BeginTime";
            this.lbl_BeginTime.Size = new System.Drawing.Size(63, 13);
            this.lbl_BeginTime.TabIndex = 4;
            this.lbl_BeginTime.Text = "Begin Time:";
            this.lbl_BeginTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbo_AttDN
            // 
            this.cbo_AttDN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_AttDN.FormattingEnabled = true;
            this.cbo_AttDN.Location = new System.Drawing.Point(378, 22);
            this.cbo_AttDN.Name = "cbo_AttDN";
            this.cbo_AttDN.Size = new System.Drawing.Size(150, 21);
            this.cbo_AttDN.TabIndex = 3;
            // 
            // lbl_AttDN
            // 
            this.lbl_AttDN.AutoSize = true;
            this.lbl_AttDN.Location = new System.Drawing.Point(349, 25);
            this.lbl_AttDN.Name = "lbl_AttDN";
            this.lbl_AttDN.Size = new System.Drawing.Size(25, 13);
            this.lbl_AttDN.TabIndex = 2;
            this.lbl_AttDN.Text = "SN:";
            this.lbl_AttDN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbo_AttType
            // 
            this.cbo_AttType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_AttType.FormattingEnabled = true;
            this.cbo_AttType.Items.AddRange(new object[] {
            "Type 1",
            "Type 2"});
            this.cbo_AttType.Location = new System.Drawing.Point(83, 22);
            this.cbo_AttType.Name = "cbo_AttType";
            this.cbo_AttType.Size = new System.Drawing.Size(150, 21);
            this.cbo_AttType.TabIndex = 1;
            // 
            // lbl_AttType
            // 
            this.lbl_AttType.AutoSize = true;
            this.lbl_AttType.Location = new System.Drawing.Point(42, 25);
            this.lbl_AttType.Name = "lbl_AttType";
            this.lbl_AttType.Size = new System.Drawing.Size(34, 13);
            this.lbl_AttType.TabIndex = 0;
            this.lbl_AttType.Text = "Type:";
            this.lbl_AttType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.nud_DIN);
            this.groupBox3.Controls.Add(this.btn_UserGet);
            this.groupBox3.Controls.Add(this.btn_UserSet);
            this.groupBox3.Controls.Add(this.cbo_UserType);
            this.groupBox3.Controls.Add(this.lbl_UserType);
            this.groupBox3.Controls.Add(this.lbl_UserId);
            this.groupBox3.Location = new System.Drawing.Point(12, 261);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(544, 87);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "User Attendance Type Settings";
            this.groupBox3.Visible = false;
            // 
            // nud_DIN
            // 
            this.nud_DIN.Location = new System.Drawing.Point(65, 23);
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
            this.nud_DIN.Size = new System.Drawing.Size(150, 20);
            this.nud_DIN.TabIndex = 1;
            this.nud_DIN.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btn_UserGet
            // 
            this.btn_UserGet.Location = new System.Drawing.Point(372, 50);
            this.btn_UserGet.Name = "btn_UserGet";
            this.btn_UserGet.Size = new System.Drawing.Size(75, 25);
            this.btn_UserGet.TabIndex = 4;
            this.btn_UserGet.Text = "Get";
            this.btn_UserGet.UseVisualStyleBackColor = true;
            this.btn_UserGet.Click += new System.EventHandler(this.btn_UserGet_Click);
            // 
            // btn_UserSet
            // 
            this.btn_UserSet.Location = new System.Drawing.Point(453, 50);
            this.btn_UserSet.Name = "btn_UserSet";
            this.btn_UserSet.Size = new System.Drawing.Size(75, 25);
            this.btn_UserSet.TabIndex = 5;
            this.btn_UserSet.Text = "Set";
            this.btn_UserSet.UseVisualStyleBackColor = true;
            this.btn_UserSet.Click += new System.EventHandler(this.btn_UserSet_Click);
            // 
            // cbo_UserType
            // 
            this.cbo_UserType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_UserType.FormattingEnabled = true;
            this.cbo_UserType.Items.AddRange(new object[] {
            "Type 1",
            "Type 2"});
            this.cbo_UserType.Location = new System.Drawing.Point(378, 22);
            this.cbo_UserType.Name = "cbo_UserType";
            this.cbo_UserType.Size = new System.Drawing.Size(150, 21);
            this.cbo_UserType.TabIndex = 3;
            // 
            // lbl_UserType
            // 
            this.lbl_UserType.AutoSize = true;
            this.lbl_UserType.Location = new System.Drawing.Point(337, 25);
            this.lbl_UserType.Name = "lbl_UserType";
            this.lbl_UserType.Size = new System.Drawing.Size(34, 13);
            this.lbl_UserType.TabIndex = 2;
            this.lbl_UserType.Text = "Type:";
            this.lbl_UserType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_UserId
            // 
            this.lbl_UserId.AutoSize = true;
            this.lbl_UserId.Location = new System.Drawing.Point(6, 25);
            this.lbl_UserId.Name = "lbl_UserId";
            this.lbl_UserId.Size = new System.Drawing.Size(46, 13);
            this.lbl_UserId.TabIndex = 0;
            this.lbl_UserId.Text = "User ID:";
            this.lbl_UserId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AlarmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 361);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AlarmForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alarm Settings";
            this.Load += new System.EventHandler(this.AlarmForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_AlarmDelay)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_DIN)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbo_AlarmDN;
        private System.Windows.Forms.Label lbl_AlarmDN;
        private System.Windows.Forms.ComboBox cbo_AlarmMinute;
        private System.Windows.Forms.ComboBox cbo_AlarmHour;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_AlarmDelay;
        private System.Windows.Forms.Label lbl_AlarmTime;
        private System.Windows.Forms.ComboBox cbo_AlarmCycle;
        private System.Windows.Forms.Label lbl_AlarmCycle;
        private System.Windows.Forms.Button btn_AlarmGet;
        private System.Windows.Forms.Button btn_AlarmSet;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_AttGet;
        private System.Windows.Forms.Button btn_AttSet;
        private System.Windows.Forms.ComboBox cbo_EndMinute;
        private System.Windows.Forms.ComboBox cbo_EndHour;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_EndTime;
        private System.Windows.Forms.ComboBox cbo_BeginMinute;
        private System.Windows.Forms.ComboBox cbo_BeginHour;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_BeginTime;
        private System.Windows.Forms.ComboBox cbo_AttDN;
        private System.Windows.Forms.Label lbl_AttDN;
        private System.Windows.Forms.ComboBox cbo_AttType;
        private System.Windows.Forms.Label lbl_AttType;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_UserGet;
        private System.Windows.Forms.Button btn_UserSet;
        private System.Windows.Forms.ComboBox cbo_UserType;
        private System.Windows.Forms.Label lbl_UserType;
        private System.Windows.Forms.Label lbl_UserId;
        private System.Windows.Forms.NumericUpDown nud_AlarmDelay;
        private System.Windows.Forms.NumericUpDown nud_DIN;
    }
}