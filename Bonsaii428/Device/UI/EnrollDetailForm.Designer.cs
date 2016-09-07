namespace ZDC2911Demo.UI {
    partial class EnrollDetailForm {
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
            this.cbo_Role = new System.Windows.Forms.ComboBox();
            this.txt_UserId = new System.Windows.Forms.TextBox();
            this.lbl_Role = new System.Windows.Forms.Label();
            this.lbl_UserId = new System.Windows.Forms.Label();
            this.clb_Fp = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // cbo_Role
            // 
            this.cbo_Role.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_Role.Enabled = false;
            this.cbo_Role.FormattingEnabled = true;
            this.cbo_Role.Items.AddRange(new object[] {
            "普通用户",
            "超级管理员",
            "登记管理者",
            "查询管理者"});
            this.cbo_Role.Location = new System.Drawing.Point(83, 33);
            this.cbo_Role.Name = "cbo_Role";
            this.cbo_Role.Size = new System.Drawing.Size(173, 20);
            this.cbo_Role.TabIndex = 3;
            // 
            // txt_UserId
            // 
            this.txt_UserId.BackColor = System.Drawing.Color.White;
            this.txt_UserId.Location = new System.Drawing.Point(83, 6);
            this.txt_UserId.Name = "txt_UserId";
            this.txt_UserId.ReadOnly = true;
            this.txt_UserId.Size = new System.Drawing.Size(173, 21);
            this.txt_UserId.TabIndex = 1;
            // 
            // lbl_Role
            // 
            this.lbl_Role.AutoSize = true;
            this.lbl_Role.Location = new System.Drawing.Point(12, 36);
            this.lbl_Role.Name = "lbl_Role";
            this.lbl_Role.Size = new System.Drawing.Size(65, 12);
            this.lbl_Role.TabIndex = 2;
            this.lbl_Role.Text = "Privilege:";
            this.lbl_Role.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_UserId
            // 
            this.lbl_UserId.AutoSize = true;
            this.lbl_UserId.Location = new System.Drawing.Point(24, 9);
            this.lbl_UserId.Name = "lbl_UserId";
            this.lbl_UserId.Size = new System.Drawing.Size(53, 12);
            this.lbl_UserId.TabIndex = 0;
            this.lbl_UserId.Text = "User ID:";
            this.lbl_UserId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // clb_Fp
            // 
            this.clb_Fp.Enabled = false;
            this.clb_Fp.FormattingEnabled = true;
            this.clb_Fp.Items.AddRange(new object[] {
            "FP 0",
            "FP 1",
            "FP 2",
            "FP 3",
            "FP 4",
            "FP 5",
            "FP 6",
            "FP 7",
            "FP 8",
            "FP 9",
            "PWD",
            "Card"});
            this.clb_Fp.Location = new System.Drawing.Point(12, 59);
            this.clb_Fp.MultiColumn = true;
            this.clb_Fp.Name = "clb_Fp";
            this.clb_Fp.Size = new System.Drawing.Size(244, 116);
            this.clb_Fp.TabIndex = 4;
            // 
            // EnrollDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(269, 190);
            this.Controls.Add(this.clb_Fp);
            this.Controls.Add(this.cbo_Role);
            this.Controls.Add(this.txt_UserId);
            this.Controls.Add(this.lbl_Role);
            this.Controls.Add(this.lbl_UserId);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "EnrollDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Enroll Information";
            this.Load += new System.EventHandler(this.EnrollDetailForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbo_Role;
        private System.Windows.Forms.TextBox txt_UserId;
        private System.Windows.Forms.Label lbl_Role;
        private System.Windows.Forms.Label lbl_UserId;
        private System.Windows.Forms.CheckedListBox clb_Fp;
    }
}