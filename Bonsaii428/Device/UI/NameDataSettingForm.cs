using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ZDC2911Demo.IConvert;
using Riss.Devices;

namespace ZDC2911Demo.UI {
    public partial class NameDataSettingForm : Form {
        private string operationType;
        private Zd2911EnrollFileManagement fileMgr;

        public NameDataSettingForm() {
            InitializeComponent();
            fileMgr = new Zd2911EnrollFileManagement();
        }

        private void ClearText() {
            txt_UserId.Text = string.Empty;
            txt_UserName.Text = string.Empty;
        }

        private bool CheckData() {
            if (txt_UserId.Text.Trim().Equals(string.Empty)) {
                MessageBox.Show("Please Input the User ID", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_UserId.Focus();
                return false;
            }

            if (false == ConvertObject.IsInt(txt_UserId.Text.Trim())) {
                MessageBox.Show("User ID can only enter a number", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_UserId.Focus();
                return false;
            }

            foreach (ListViewItem lvi in lvw_UserList.Items) {
                if (txt_UserId.Text.Trim().Equals(lvi.SubItems[1].Text)) {
                    MessageBox.Show("The user ID already exists, please modify", "Prompt", MessageBoxButtons.OK, 
                        MessageBoxIcon.Warning);
                    txt_UserId.Focus();
                    return false;
                }
            }

            if (txt_UserName.Text.Trim().Equals(string.Empty)) {
                MessageBox.Show("Please input the user name", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_UserName.Focus();
                return false;
            }
            return true;
        }

        private void NameDataSettingForm_Load(object sender, EventArgs e) {
            txt_UserId.ReadOnly = true;
            txt_UserName.ReadOnly = true;
            btn_Save.Enabled = false;
            btn_Cancle.Enabled = false;
        }

        private void btn_Add_Click(object sender, EventArgs e) {
            operationType = "Add";
            txt_UserId.ReadOnly = false;
            txt_UserName.ReadOnly = false;            
            btn_Add.Enabled = false;
            btn_Edit.Enabled = false;
            btn_Save.Enabled = true;
            btn_Cancle.Enabled = true;
            btn_Delete.Enabled = false;
            ClearText();
        }

        private void btn_Edit_Click(object sender, EventArgs e) {
            if (0 == lvw_UserList.SelectedItems.Count) {
                MessageBox.Show("Please select the record after the operation", "Prompt", MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                return;
            }

            operationType = "Edit";
            txt_UserId.ReadOnly = true;
            txt_UserName.ReadOnly = false;
            btn_Add.Enabled = false;
            btn_Edit.Enabled = false;
            btn_Save.Enabled = true;
            btn_Cancle.Enabled = true;
            btn_Delete.Enabled = false;
        }

        private void btn_Save_Click(object sender, EventArgs e) {
            if (operationType.Equals("Add")) {
                if (false == CheckData()) {
                    return;
                }
                int index = 1;
                if (lvw_UserList.Items.Count > 0) {
                    index = Convert.ToInt32(lvw_UserList.Items[lvw_UserList.Items.Count - 1].SubItems[0].Text) + 1;
                }
                ListViewItem lvi = new ListViewItem(new string[]{ index.ToString(), txt_UserId.Text.Trim(),
                    txt_UserName.Text.Trim() });
                lvw_UserList.Items.Add(lvi);
                ClearText();                
            } else {
                if (txt_UserName.Text.Trim().Equals(string.Empty)) {
                    MessageBox.Show("Please input the user name", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_UserName.Focus();
                    return;
                }

                lvw_UserList.SelectedItems[0].SubItems[2].Text = txt_UserName.Text.Trim();
            }

            txt_UserId.ReadOnly = true;
            txt_UserName.ReadOnly = true;
            btn_Add.Enabled = true;
            btn_Edit.Enabled = true;
            btn_Save.Enabled = false;
            btn_Cancle.Enabled = false;
            btn_Delete.Enabled = true;
        }

        private void btn_Cancle_Click(object sender, EventArgs e) {
            if (operationType.Equals("Add")) {
                ClearText();
            } else {
                txt_UserId.Text = lvw_UserList.SelectedItems[0].SubItems[1].Text;
                txt_UserName.Text = lvw_UserList.SelectedItems[0].SubItems[2].Text;
            }

            txt_UserId.ReadOnly = true;
            txt_UserName.ReadOnly = true;
            btn_Add.Enabled = true;
            btn_Edit.Enabled = true;
            btn_Save.Enabled = false;
            btn_Cancle.Enabled = false;
            btn_Delete.Enabled = true;
        }

        private void btn_Delete_Click(object sender, EventArgs e) {
            if (0 == lvw_UserList.SelectedItems.Count) {
                MessageBox.Show("Please select the record after the operation", "Prompt", MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete", "Prompt", MessageBoxButtons.YesNo, 
                MessageBoxIcon.Information).Equals(DialogResult.Yes)) {
                lvw_UserList.Items.Remove(lvw_UserList.SelectedItems[0]);
                ClearText();
            }
        }

        private void lvw_UserList_SelectedIndexChanged(object sender, EventArgs e) {
            if (lvw_UserList.SelectedItems.Count > 0) {
                txt_UserId.Text = lvw_UserList.SelectedItems[0].SubItems[1].Text;
                txt_UserName.Text = lvw_UserList.SelectedItems[0].SubItems[2].Text;
            }
        }

        private void btn_SaveFile_Click(object sender, EventArgs e) {
            if (0 == lvw_UserList.Items.Count) {
                return;
            }

            try {
                List<User> userList = new List<User>();
                foreach (ListViewItem lvi in lvw_UserList.Items) {
                    User user = new User();
                    user.DIN = Convert.ToUInt64(lvi.SubItems[1].Text);
                    user.UserName = lvi.SubItems[2].Text;
                    userList.Add(user);
                }

                if (sfd_SaveFile.ShowDialog().Equals(DialogResult.OK)) {
                    bool result = fileMgr.SaveUserNameData(sfd_SaveFile.FileName, userList);
                    if (false == result) {
                        MessageBox.Show("Save File Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show("Save File Occur Error：" +ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_OpenFile_Click(object sender, EventArgs e) {
            try {
                List<User> userList = new List<User>();
                if (ofd_OpenFile.ShowDialog().Equals(DialogResult.OK)) {
                    bool result = fileMgr.LoadUserNameData(ofd_OpenFile.FileName, ref userList);
                    if (false == result) {
                        MessageBox.Show("Open File Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    } 

                    lvw_UserList.Items.Clear();
                    int index = 1;
                    foreach (User user in userList) {
                        ListViewItem lvi = new ListViewItem(new string[] { index.ToString(), user.DIN.ToString(), user.UserName });
                        lvw_UserList.Items.Add(lvi);
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show("Open File Occur Error：" + ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
