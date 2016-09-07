namespace ZDC2911Demo.UI {
    partial class NameDataSettingForm {
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
            this.lvw_UserList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_OpenFile = new System.Windows.Forms.Button();
            this.btn_SaveFile = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_Delete = new System.Windows.Forms.Button();
            this.btn_Cancle = new System.Windows.Forms.Button();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btn_Edit = new System.Windows.Forms.Button();
            this.btn_Add = new System.Windows.Forms.Button();
            this.txt_UserName = new System.Windows.Forms.TextBox();
            this.lbl_UserName = new System.Windows.Forms.Label();
            this.lbl_UserId = new System.Windows.Forms.Label();
            this.sfd_SaveFile = new System.Windows.Forms.SaveFileDialog();
            this.ofd_OpenFile = new System.Windows.Forms.OpenFileDialog();
            this.txt_UserId = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvw_UserList
            // 
            this.lvw_UserList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvw_UserList.FullRowSelect = true;
            this.lvw_UserList.GridLines = true;
            this.lvw_UserList.HideSelection = false;
            this.lvw_UserList.Location = new System.Drawing.Point(12, 12);
            this.lvw_UserList.MultiSelect = false;
            this.lvw_UserList.Name = "lvw_UserList";
            this.lvw_UserList.Size = new System.Drawing.Size(591, 286);
            this.lvw_UserList.TabIndex = 0;
            this.lvw_UserList.UseCompatibleStateImageBehavior = false;
            this.lvw_UserList.View = System.Windows.Forms.View.Details;
            this.lvw_UserList.SelectedIndexChanged += new System.EventHandler(this.lvw_UserList_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "SN";
            this.columnHeader1.Width = 90;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "User ID";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 150;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Name";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 150;
            // 
            // btn_OpenFile
            // 
            this.btn_OpenFile.Location = new System.Drawing.Point(447, 304);
            this.btn_OpenFile.Name = "btn_OpenFile";
            this.btn_OpenFile.Size = new System.Drawing.Size(75, 23);
            this.btn_OpenFile.TabIndex = 1;
            this.btn_OpenFile.Text = "Open";
            this.btn_OpenFile.UseVisualStyleBackColor = true;
            this.btn_OpenFile.Click += new System.EventHandler(this.btn_OpenFile_Click);
            // 
            // btn_SaveFile
            // 
            this.btn_SaveFile.Location = new System.Drawing.Point(528, 304);
            this.btn_SaveFile.Name = "btn_SaveFile";
            this.btn_SaveFile.Size = new System.Drawing.Size(75, 23);
            this.btn_SaveFile.TabIndex = 2;
            this.btn_SaveFile.Text = "Save";
            this.btn_SaveFile.UseVisualStyleBackColor = true;
            this.btn_SaveFile.Click += new System.EventHandler(this.btn_SaveFile_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_Delete);
            this.groupBox1.Controls.Add(this.btn_Cancle);
            this.groupBox1.Controls.Add(this.btn_Save);
            this.groupBox1.Controls.Add(this.btn_Edit);
            this.groupBox1.Controls.Add(this.btn_Add);
            this.groupBox1.Controls.Add(this.txt_UserName);
            this.groupBox1.Controls.Add(this.lbl_UserName);
            this.groupBox1.Controls.Add(this.txt_UserId);
            this.groupBox1.Controls.Add(this.lbl_UserId);
            this.groupBox1.Location = new System.Drawing.Point(12, 333);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(591, 77);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Detail Information";
            // 
            // btn_Delete
            // 
            this.btn_Delete.Location = new System.Drawing.Point(510, 48);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(75, 23);
            this.btn_Delete.TabIndex = 8;
            this.btn_Delete.Text = "Delete";
            this.btn_Delete.UseVisualStyleBackColor = true;
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // btn_Cancle
            // 
            this.btn_Cancle.Location = new System.Drawing.Point(429, 47);
            this.btn_Cancle.Name = "btn_Cancle";
            this.btn_Cancle.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancle.TabIndex = 7;
            this.btn_Cancle.Text = "Cancel";
            this.btn_Cancle.UseVisualStyleBackColor = true;
            this.btn_Cancle.Click += new System.EventHandler(this.btn_Cancle_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(348, 47);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(75, 23);
            this.btn_Save.TabIndex = 6;
            this.btn_Save.Text = "Save";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_Edit
            // 
            this.btn_Edit.Location = new System.Drawing.Point(267, 47);
            this.btn_Edit.Name = "btn_Edit";
            this.btn_Edit.Size = new System.Drawing.Size(75, 23);
            this.btn_Edit.TabIndex = 5;
            this.btn_Edit.Text = "Edit";
            this.btn_Edit.UseVisualStyleBackColor = true;
            this.btn_Edit.Click += new System.EventHandler(this.btn_Edit_Click);
            // 
            // btn_Add
            // 
            this.btn_Add.Location = new System.Drawing.Point(186, 47);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(75, 23);
            this.btn_Add.TabIndex = 4;
            this.btn_Add.Text = "Add";
            this.btn_Add.UseVisualStyleBackColor = true;
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // txt_UserName
            // 
            this.txt_UserName.Location = new System.Drawing.Point(435, 20);
            this.txt_UserName.MaxLength = 12;
            this.txt_UserName.Name = "txt_UserName";
            this.txt_UserName.Size = new System.Drawing.Size(150, 21);
            this.txt_UserName.TabIndex = 3;
            // 
            // lbl_UserName
            // 
            this.lbl_UserName.AutoSize = true;
            this.lbl_UserName.Location = new System.Drawing.Point(394, 23);
            this.lbl_UserName.Name = "lbl_UserName";
            this.lbl_UserName.Size = new System.Drawing.Size(35, 12);
            this.lbl_UserName.TabIndex = 2;
            this.lbl_UserName.Text = "Name:";
            this.lbl_UserName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_UserId
            // 
            this.lbl_UserId.AutoSize = true;
            this.lbl_UserId.Location = new System.Drawing.Point(6, 23);
            this.lbl_UserId.Name = "lbl_UserId";
            this.lbl_UserId.Size = new System.Drawing.Size(53, 12);
            this.lbl_UserId.TabIndex = 0;
            this.lbl_UserId.Text = "User ID:";
            this.lbl_UserId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sfd_SaveFile
            // 
            this.sfd_SaveFile.Filter = "Text File|*.nm";
            // 
            // ofd_OpenFile
            // 
            this.ofd_OpenFile.Filter = "Text File|*.nm";
            // 
            // txt_UserId
            // 
            this.txt_UserId.Location = new System.Drawing.Point(65, 20);
            this.txt_UserId.MaxLength = 18;
            this.txt_UserId.Name = "txt_UserId";
            this.txt_UserId.Size = new System.Drawing.Size(150, 21);
            this.txt_UserId.TabIndex = 1;
            // 
            // NameDataSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 419);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_SaveFile);
            this.Controls.Add(this.btn_OpenFile);
            this.Controls.Add(this.lvw_UserList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "NameDataSettingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Name Data Settings";
            this.Load += new System.EventHandler(this.NameDataSettingForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvw_UserList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btn_OpenFile;
        private System.Windows.Forms.Button btn_SaveFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_Cancle;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button btn_Edit;
        private System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.TextBox txt_UserName;
        private System.Windows.Forms.Label lbl_UserName;
        private System.Windows.Forms.Label lbl_UserId;
        private System.Windows.Forms.SaveFileDialog sfd_SaveFile;
        private System.Windows.Forms.OpenFileDialog ofd_OpenFile;
        private System.Windows.Forms.Button btn_Delete;
        private System.Windows.Forms.TextBox txt_UserId;
    }
}