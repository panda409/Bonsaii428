using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using ZDC2911Demo.IConvert;
using ZDC2911Demo.Entity;
using ZDC2911Demo.SysEnum;
using Riss.Devices;
using System.Drawing;

namespace ZDC2911Demo.UI
{
    public partial class EnrollForm : Form
    {
        private byte[] fpBytes;
        private Device device;
        private DeviceConnection deviceConnection;
        private Zd2911EnrollFileManagement enrollFileMgr;
        private User shareUser;

        public EnrollForm(DeviceCommEty deviceEty)
        {
            InitializeComponent();
            device = deviceEty.Device;
            deviceConnection = deviceEty.DeviceConnection;
            fpBytes = new byte[Zd2911Utils.MaxFingerprintLength];
            enrollFileMgr = new Zd2911EnrollFileManagement();
        }

        private int GetPrivilege()
        {
            switch (cbo_Role.SelectedIndex)
            {
                case 0:
                    return (int)UserPrivilege.ROLE_GENERAL_USER;

                case 1:
                    return (int)UserPrivilege.ROLE_ENROLL_USER;

                case 2:
                    return (int)UserPrivilege.ROLE_VIEW_USER;

                case 3:
                    return (int)UserPrivilege.ROLE_SUPER_USER;

                case 4:
                    return (int)UserPrivilege.ROLE_CUSTOMER;
            }

            return 0;
        }

        private void AddUserInfoToViewList(User user, int no)
        {
            ListViewItem item = new ListViewItem(no.ToString());
            item.SubItems.Add(user.DIN.ToString());
            item.SubItems.Add(user.UserName);
            Enroll enroll = user.Enrolls[0];

            for (int i = 0; i < Zd2911Utils.MaxFingerprintCount; i++)
            {
                if (0 != Zd2911Utils.BitCheck((int)enroll.EnrollType, i))
                {
                    item.SubItems.Add("Y");
                }
                else
                {
                    item.SubItems.Add("N");
                }
            }

            if (0 != Zd2911Utils.BitCheck((int)enroll.EnrollType, 10))
            {
                item.SubItems.Add("Y");
            }
            else
            {
                item.SubItems.Add("N");
            }

            if (0 != Zd2911Utils.BitCheck((int)enroll.EnrollType, 11))
            {
                item.SubItems.Add("Y");
            }
            else
            {
                item.SubItems.Add("N");
            }

            string userPrivilege = ConvertObject.GetUserPrivilege((UserPrivilege)user.Privilege);
            item.SubItems.Add(userPrivilege);
            item.Tag = user;
            lvw_UserList.Items.Add(item);
        }

        #region FP Data: Read, Write, Clear
        private void btn_SaveFpData_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK.Equals(sfd_SaveFpData.ShowDialog()))
            {
                FileStream fs = new FileStream(sfd_SaveFpData.FileName, FileMode.Create);

                try
                {
                    fs.Write(fpBytes, 0, fpBytes.Length);
                    fs.Flush();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    fs.Close();
                }
            }
        }

        private void btn_OpenFpData_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK.Equals(ofd_OpenFpData.ShowDialog()))
            {
                FileStream fs = new FileStream(ofd_OpenFpData.FileName, FileMode.Open);

                try
                {
                    fs.Read(fpBytes, 0, fpBytes.Length);
                    txt_UserFpData.Text = ConvertObject.ConvertByteToHex(fpBytes).Replace("-", " ");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    fs.Close();
                }
            }
        }

        private void btn_ReadFp_Click(object sender, EventArgs e)
        {
            if (-1 == cbo_FpNo.SelectedIndex)
            {
                MessageBox.Show("Please Select Finger SN", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_FpNo.Focus();
                return;
            }

            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;

            try
            {
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                User user = new User();
                Enroll enroll = new Enroll();
                user.DIN = (UInt64)nud_DIN.Value;
                enroll.DIN = user.DIN;
                enroll.EnrollType = (EnrollType)cbo_FpNo.SelectedIndex;
                enroll.Fingerprint = new byte[Zd2911Utils.MaxFingerprintLength];
                user.Enrolls.Add(enroll);
                extraProperty = UserEnrollCommand.ReadFingerprint;
                result = deviceConnection.GetProperty(UserProperty.UserEnroll, extraProperty, ref user, ref extraData);
                if (result)
                {
                    fpBytes = user.Enrolls[0].Fingerprint;
                    txt_UserFpData.Text = ConvertObject.ConvertByteToHex(fpBytes);
                }
                else
                {
                    MessageBox.Show("Read FP Data Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                extraData = Global.DeviceIdle;
                deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
            }
        }

        private void btn_WriteFp_Click(object sender, EventArgs e)
        {
            if (-1 == cbo_FpNo.SelectedIndex)
            {
                MessageBox.Show("Please Select Finger SN", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_FpNo.Focus();
                return;
            }

            if (-1 == cbo_Role.SelectedIndex)
            {
                MessageBox.Show("Please Select Privilege", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_Role.Focus();
                return;
            }

            if (string.Empty.Equals(txt_UserFpData.Text.Trim()))
            {
                MessageBox.Show("Current FP Data is Empty", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;

            try
            {
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                User user = new User();
                Enroll enroll = new Enroll();
                user.DIN = (UInt64)nud_DIN.Value;
                user.Privilege = GetPrivilege();
                enroll.DIN = user.DIN;
                enroll.EnrollType = (EnrollType)cbo_FpNo.SelectedIndex;
                byte[] fingerprint = new byte[Zd2911Utils.MaxFingerprintLength * (cbo_FpNo.SelectedIndex + 1)];
                //<-- notice these codes for your new requirement, by Axu, 20130829
                byte[] bytesFromText = ConvertObject.ConvertStringToBytes(txt_UserFpData.Text);
                Array.Copy(bytesFromText, 0, fingerprint, cbo_FpNo.SelectedIndex * Zd2911Utils.MaxFingerprintLength, fpBytes.Length);
                //Array.Copy(fpBytes, 0, fingerprint, cbo_FpNo.SelectedIndex * Zd2911Utils.MaxFingerprintLength, fpBytes.Length);
                //-->
                enroll.Fingerprint = fingerprint;
                user.Enrolls.Add(enroll);
                extraProperty = UserEnrollCommand.WriteFingerprint;
                result = deviceConnection.SetProperty(UserProperty.UserEnroll, extraProperty, user, extraData);
                if (result)
                {
                    MessageBox.Show("Write FP Data Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Write FP Data Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                extraData = Global.DeviceIdle;
                deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
            }
        }

        private void btn_DeleteFp_Click(object sender, EventArgs e)
        {
            if (-1 == cbo_FpNo.SelectedIndex)
            {
                MessageBox.Show("Please Select Finger SN", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_FpNo.Focus();
                return;
            }

            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;

            try
            {
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                User user = new User();
                Enroll enroll = new Enroll();
                user.DIN = (UInt64)nud_DIN.Value;
                enroll.DIN = user.DIN;
                enroll.EnrollType = (EnrollType)cbo_FpNo.SelectedIndex;
                user.Enrolls.Add(enroll);
                extraProperty = UserEnrollCommand.ClearFingerprint;
                result = deviceConnection.SetProperty(UserProperty.UserEnroll, extraProperty, user, extraData);
                if (result)
                {
                    MessageBox.Show("Clear FP Data Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Clear FP Data Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                extraData = Global.DeviceIdle;
                deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
            }
        }
        #endregion

        #region Password: Read, Write, Clear
        private void btn_ReadPwd_Click(object sender, EventArgs e)
        {
            try
            {
                object extraProperty = new object();
                extraProperty = UserEnrollCommand.ReadPassword;
                object extraData = new object();
                User user = new User();
                user.DIN = (UInt64)nud_DIN.Value;
                Enroll enroll = new Enroll();
                enroll.DIN = user.DIN;
                user.Enrolls.Add(enroll);
                bool result = deviceConnection.GetProperty(UserProperty.UserEnroll, extraProperty, ref user, ref extraData);
                if (result)
                {
                    txt_Pwd.Text = user.Enrolls[0].Password.Replace("\0", "");
                }
                else
                {
                    MessageBox.Show("Read Password Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_WritePwd_Click(object sender, EventArgs e)
        {
            if (-1 == cbo_Role.SelectedIndex)
            {
                MessageBox.Show("Please Select Privilege", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_Role.Focus();
                return;
            }

            if (string.Empty.Equals(txt_Pwd.Text.Trim()))
            {
                MessageBox.Show("Please Input Password", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_Pwd.Focus();
                return;
            }

            if (false == ConvertObject.IsInt(txt_Pwd.Text.Trim()))
            {
                MessageBox.Show("Password can only enter a number", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_Pwd.Focus();
                return;
            }

            try
            {
                object extraProperty = new object();
                extraProperty = UserEnrollCommand.WritePassword;
                object extraData = new object();
                User user = new User();
                user.DIN = (UInt64)nud_DIN.Value;
                user.Privilege = GetPrivilege();
                Enroll enroll = new Enroll();
                enroll.DIN = user.DIN;
                enroll.Password = txt_Pwd.Text.Trim();
                user.Enrolls.Add(enroll);
                bool result = deviceConnection.SetProperty(UserProperty.UserEnroll, extraProperty, user, extraData);
                if (result)
                {
                    MessageBox.Show("Write Password Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Wrtie Password Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_DeletePwd_Click(object sender, EventArgs e)
        {
            try
            {
                object extraProperty = new object();
                extraProperty = UserEnrollCommand.ClearPassword;
                object extraData = new object();
                User user = new User();
                user.DIN = (UInt64)nud_DIN.Value;
                Enroll enroll = new Enroll();
                enroll.DIN = user.DIN;
                user.Enrolls.Add(enroll);
                bool result = deviceConnection.SetProperty(UserProperty.UserEnroll, extraProperty, user, extraData);
                if (result)
                {
                    MessageBox.Show("Clear Password Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Clear Password Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Card: Read, Write, Clear
        private void btn_ReadCard_Click(object sender, EventArgs e)
        {
            try
            {
                object extraProperty = new object();
                extraProperty = UserEnrollCommand.ReadCard;
                object extraData = new object();
                User user = new User();
                user.DIN = (UInt64)nud_DIN.Value;
                Enroll enroll = new Enroll();
                enroll.DIN = user.DIN;
                user.Enrolls.Add(enroll);
                bool result = deviceConnection.GetProperty(UserProperty.UserEnroll, extraProperty, ref user, ref extraData);
                if (result)
                {
                    txt_Card.Text = user.Enrolls[0].CardID;
                }
                else
                {
                    MessageBox.Show("Read Card Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_WriteCard_Click(object sender, EventArgs e)
        {
            if (-1 == cbo_Role.SelectedIndex)
            {
                MessageBox.Show("Please Select Privilege", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_Role.Focus();
                return;
            }

            if (string.Empty.Equals(txt_Card.Text.Trim()))
            {
                MessageBox.Show("Please Input Card ID", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_Card.Focus();
                return;
            }

            if (false == ConvertObject.IsInt(txt_Card.Text.Trim()))
            {
                MessageBox.Show("Card ID can only enter a number", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_Card.Focus();
                return;
            }

            try
            {
                object extraProperty = new object();
                extraProperty = UserEnrollCommand.WriteCard;
                object extraData = new object();
                User user = new User();
                user.DIN = (UInt64)nud_DIN.Value;
                user.Privilege = GetPrivilege();
                Enroll enroll = new Enroll();
                enroll.DIN = user.DIN;
                enroll.CardID = txt_Card.Text.Trim();
                user.Enrolls.Add(enroll);
                bool result = deviceConnection.SetProperty(UserProperty.UserEnroll, extraProperty, user, extraData);
                if (result)
                {
                    MessageBox.Show("Write Card Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Write Card Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_DeleteCard_Click(object sender, EventArgs e)
        {
            try
            {
                object extraProperty = new object();
                extraProperty = UserEnrollCommand.ClearCard;
                object extraData = new object();
                User user = new User();
                user.DIN = (UInt64)nud_DIN.Value;
                Enroll enroll = new Enroll();
                enroll.DIN = user.DIN;
                user.Enrolls.Add(enroll);
                bool result = deviceConnection.SetProperty(UserProperty.UserEnroll, extraProperty, user, extraData);
                if (result)
                {
                    MessageBox.Show("Clear Card Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Clear Card Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Username Settings
        private void btn_ReadUserName_Click(object sender, EventArgs e)
        {
            try
            {
                object extraProperty = new object();
                object extraData = new object();
                User user = new User();
                user.DIN = (UInt64)nud_DIN.Value;
                bool result = deviceConnection.GetProperty(UserProperty.UserName, extraProperty, ref user, ref extraData);
                if (result)
                {
                    txt_UserName.Text = user.UserName;
                }
                else
                {
                    MessageBox.Show("Get Username Fial", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_WriteUserName_Click(object sender, EventArgs e)
        {
            if (string.Empty.Equals(txt_UserName.Text.Trim()))
            {
                MessageBox.Show("Please Input Username", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_UserName.Focus();
                return;
            }

            try
            {
                object extraProperty = new object();
                object extraData = new object();
                User user = new User();
                user.DIN = (UInt64)nud_DIN.Value;
                user.UserName = txt_UserName.Text.Trim();
                bool result = deviceConnection.SetProperty(UserProperty.UserName, extraProperty, user, extraData);
                if (result)
                {
                    MessageBox.Show("Set Username Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Set Username Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Privilege Settings
        private void btn_SetRole_Click(object sender, EventArgs e)
        {
            if (-1 == cbo_Role.SelectedIndex)
            {
                MessageBox.Show("Please Select Privilege", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_Role.Focus();
                return;
            }

            try
            {
                object extraProperty = new object();
                object extraData = new object();
                User user = new User();
                user.DIN = (UInt64)nud_DIN.Value;
                user.Privilege = GetPrivilege();
                bool result = deviceConnection.SetProperty(UserProperty.Privilege, extraProperty, user, extraData);
                if (result)
                {
                    MessageBox.Show("Set Privilege Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Set Privilege Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Clear User
        private void btn_DeleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                object extraProperty = new object();
                object extraData = new object();
                extraData = (UInt64)nud_DIN.Value;
                bool result = deviceConnection.SetProperty(DeviceProperty.Enrolls, extraProperty, device, extraData);
                if (result)
                {
                    MessageBox.Show("Clear User Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Clear User Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_DeleteAllUser_Click(object sender, EventArgs e)
        {
            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;

            try
            {
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                extraData = (UInt64)0;
                result = deviceConnection.SetProperty(DeviceProperty.Enrolls, extraProperty, device, extraData);
                if (result)
                {
                    MessageBox.Show("Clear All User Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Clear All User Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                extraData = Global.DeviceIdle;
                deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
            }
        }
        #endregion

        #region Get Enroll Information
        private void btn_GetUserInfoByUserId_Click(object sender, EventArgs e)
        {
            try
            {
                object extraProperty = new object();
                extraProperty = (UInt64)nud_DIN.Value;
                object extraData = new object();
                bool result = deviceConnection.GetProperty(DeviceProperty.Enrolls, extraProperty, ref device, ref extraData);
                if (result)
                {
                    User user = (User)extraData;
                    EnrollDetailForm edfForm = new EnrollDetailForm(user);
                    edfForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Get Enroll Information Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_GetAllEnrollData_Click(object sender, EventArgs e)
        {
            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;

            try
            {
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                extraProperty = (UInt64)0;
                result = deviceConnection.GetProperty(DeviceProperty.Enrolls, extraProperty, ref device, ref extraData);
                if (result)
                {
                    if (btn_Select.Text.Equals("Deselect All"))
                    {
                        btn_Select.Text = "Select All";
                    }
                    lvw_UserList.Items.Clear();
                    List<User> userList = (List<User>)extraData;
                    int no = 1;
                    foreach (User user in userList)
                    {
                        Enroll enroll = user.Enrolls[0];
                        ListViewItem item = new ListViewItem(no.ToString());
                        item.SubItems.Add(user.DIN.ToString());
                        item.SubItems.Add(user.UserName);

                        for (int i = 0; i < Zd2911Utils.MaxFingerprintCount; i++)
                        {
                            if (0 != Zd2911Utils.BitCheck((int)enroll.EnrollType, i))
                            {
                                item.SubItems.Add("Y");
                            }
                            else
                            {
                                item.SubItems.Add("N");
                            }
                        }

                        if (0 != Zd2911Utils.BitCheck((int)enroll.EnrollType, 10))
                        {
                            item.SubItems.Add("Y");
                        }
                        else
                        {
                            item.SubItems.Add("N");
                        }

                        if (0 != Zd2911Utils.BitCheck((int)enroll.EnrollType, 11))
                        {
                            item.SubItems.Add("Y");
                        }
                        else
                        {
                            item.SubItems.Add("N");
                        }

                        string privilege = ConvertObject.GetUserPrivilege((UserPrivilege)user.Privilege);
                        item.SubItems.Add(privilege);

                        lvw_UserList.Items.Add(item);
                        no++;
                    }
                }
                else
                {
                    MessageBox.Show("Get All Enroll Information Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                extraData = Global.DeviceIdle;
                deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
            }
        }
        #endregion

        #region Select All
        private void btn_Select_Click(object sender, EventArgs e)
        {
            if (0 == lvw_UserList.Items.Count)
            {
                return;
            }

            if (btn_Select.Text.Equals("Select All"))
            {
                for (int i = 0; i < lvw_UserList.Items.Count; i++)
                {
                    lvw_UserList.Items[i].Checked = true;
                }
                btn_Select.Text = "Deselect All";
            }
            else
            {
                for (int i = 0; i < lvw_UserList.Items.Count; i++)
                {
                    lvw_UserList.Items[i].Checked = false;
                }
                btn_Select.Text = "Select All";
            }
        }
        #endregion

        #region Get All Enroll Data
        private void btn_DownloadUserData_Click(object sender, EventArgs e)
        {
            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;

            try
            {
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                extraProperty = (UInt64)0;
                result = deviceConnection.GetProperty(DeviceProperty.Enrolls, extraProperty, ref device, ref extraData);
                if (false == result)
                {
                    MessageBox.Show("Get All Enroll Data Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int no = 1;
                if (btn_Select.Text.Equals("Deselect All"))
                {
                    btn_Select.Text = "Select All";
                }
                lvw_UserList.Items.Clear();
                List<User> userList = (List<User>)extraData;
                foreach (User user in userList)
                {
                    Enroll enroll = user.Enrolls[0];
                    ListViewItem item = new ListViewItem(no.ToString());
                    item.SubItems.Add(user.DIN.ToString());

                    User u = user;
                    result = deviceConnection.GetProperty(UserProperty.Enroll, extraProperty, ref u, ref extraData);
                    if (false == result)
                    {
                        MessageBox.Show("Get All Enroll Data Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    item.SubItems.Add(u.UserName);

                    for (int i = 0; i < Zd2911Utils.MaxFingerprintCount; i++)
                    {
                        if (0 != Zd2911Utils.BitCheck((int)enroll.EnrollType, i))
                        {
                            item.SubItems.Add("Y");
                        }
                        else
                        {
                            item.SubItems.Add("N");
                        }
                    }

                    if (0 != Zd2911Utils.BitCheck((int)enroll.EnrollType, 10))
                    {
                        item.SubItems.Add("Y");
                    }
                    else
                    {
                        item.SubItems.Add("N");
                    }

                    if (0 != Zd2911Utils.BitCheck((int)enroll.EnrollType, 11))
                    {
                        item.SubItems.Add("Y");
                    }
                    else
                    {
                        item.SubItems.Add("N");
                    }

                    string userPrivilege = ConvertObject.GetUserPrivilege((UserPrivilege)user.Privilege);
                    item.SubItems.Add(userPrivilege);
                    item.Tag = u;
                    lvw_UserList.Items.Add(item);
                    no++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                extraData = Global.DeviceIdle;
                deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
            }
        }
        #endregion

        #region Set Enroll Data
        private void btn_UploadUserData_Click(object sender, EventArgs e)
        {
            if (0 == lvw_UserList.Items.Count)
            {
                MessageBox.Show("Enroll Data is Empty", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (0 == lvw_UserList.CheckedItems.Count)
            {
                MessageBox.Show("Please Select the Record After Operation", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;

            try
            {
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                foreach (ListViewItem checkItem in lvw_UserList.CheckedItems)
                {
                    User user = checkItem.Tag as User;
                    if (null == user)
                    {
                        MessageBox.Show("Enroll Data is Empty", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    extraData = false;
                    result = deviceConnection.SetProperty(UserProperty.Enroll, extraProperty, user, extraData);
                    if (false == result)
                    {
                        MessageBox.Show("Set All Enroll Data Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                MessageBox.Show("Set All Enroll Data Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                extraData = Global.DeviceIdle;
                deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
            }
        }
        #endregion

        #region Save All Enroll
        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (0 == lvw_UserList.Items.Count)
            {
                MessageBox.Show("No user enroll information, cannot be saved", "Prompt", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (0 == lvw_UserList.CheckedItems.Count)
            {
                MessageBox.Show("Please select the record after the operation", "Prompt", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            try
            {
                List<User> userList = new List<User>();
                foreach (ListViewItem checkItem in lvw_UserList.CheckedItems)
                {
                    User user = checkItem.Tag as User;
                    if (null == user)
                    {
                        MessageBox.Show("No user enroll information, cannot be saved", "Prompt", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        return;
                    }

                    userList.Add(user);
                }

                FolderBrowserDialog browser = new FolderBrowserDialog();
                if (DialogResult.OK.Equals(browser.ShowDialog()))
                {
                    string fileName = browser.SelectedPath + @"\UserData";
                    if (false == Directory.Exists(fileName))
                    {
                        Directory.CreateDirectory(fileName);
                    }

                    fileName += @"\AllEnrollData.fps";
                    if (File.Exists(fileName))
                    {
                        if (DialogResult.No.Equals(MessageBox.Show("The file AllEnrollData.fps already exists, overwrite", "Prompt",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Information)))
                        {
                            return;
                        }
                    }

                    bool result = enrollFileMgr.SaveAllUserEnrollDataAsDB(fileName, userList);
                    if (result)
                    {
                        MessageBox.Show("Save All Enroll to File Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Save All Enroll to File Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Open All Enroll
        private void btn_Open_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "AllEnrollData File|AllEnrollData.fps";
                open.FileName = "AllEnrollData.fps";
                if (DialogResult.OK.Equals(open.ShowDialog()))
                {
                    lvw_UserList.Items.Clear();
                    if (btn_Select.Text.Equals("Deselect All"))
                    {
                        btn_Select.Text = "Select All";
                    }
                    List<User> userList = new List<User>();
                    bool result = enrollFileMgr.LoadAllUserEnrollDataFromDB(open.FileName, ref userList);
                    if (false == result)
                    {
                        MessageBox.Show("打开文件失败", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    int no = 1;
                    foreach (User user in userList)
                    {
                        AddUserInfoToViewList(user, no);
                        no++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region User ExtInfo
        private void getExtInfoButton_Click(object sender, EventArgs e)
        {
            try
            {
                object extraProperty = new object();
                object extraData = new object();
                User user = new User();
                user.DIN = (UInt64)nud_DIN.Value;
                bool result = deviceConnection.GetProperty(UserProperty.UserExtInfo, extraProperty, ref user, ref extraData);
                if (result)
                {
                    ExtInfoTextBox.Text = user.Comment;
                }
                else
                {
                    MessageBox.Show("Get User Ext Info Failed.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void setExtInfoButton_Click(object sender, EventArgs e)
        {
            if (string.Empty.Equals(ExtInfoTextBox.Text.Trim()))
            {
                MessageBox.Show("Please Enter User Ext Info", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ExtInfoTextBox.Focus();
                return;
            }

            try
            {
                object extraProperty = new object();
                object extraData = new object();
                User user = new User();
                user.DIN = (UInt64)nud_DIN.Value;
                user.Comment = ExtInfoTextBox.Text.Trim();
                bool result = deviceConnection.SetProperty(UserProperty.UserExtInfo, extraProperty, user, extraData);
                if (result)
                {
                    MessageBox.Show("Set User Ext Info Success.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Set User Ext Info Failed.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Open One Enroll
        private void btn_OpenOneEnroll_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "OneEnrollData File|OD_*.db";
                open.Multiselect = true;
                if (DialogResult.OK.Equals(open.ShowDialog()))
                {
                    lvw_UserList.Items.Clear();
                    if (btn_Select.Text.Equals("Deselect All"))
                    {
                        btn_Select.Text = "Select All";
                    }

                    int no = 1;
                    foreach (string fileName in open.FileNames)
                    {
                        User user = new User();
                        bool result = enrollFileMgr.LoadUserEnrollDataFromDB(fileName, ref user);
                        if (false == result)
                        {
                            MessageBox.Show("打开文件失败", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        AddUserInfoToViewList(user, no);
                        no++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Save One Enroll
        private void btn_SaveOneEnroll_Click(object sender, EventArgs e)
        {
            if (0 == lvw_UserList.Items.Count)
            {
                MessageBox.Show("No user enroll information, cannot be saved", "Prompt", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (0 == lvw_UserList.CheckedItems.Count)
            {
                MessageBox.Show("Please select the record after the operation", "Prompt", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            try
            {
                FolderBrowserDialog browser = new FolderBrowserDialog();
                if (DialogResult.OK.Equals(browser.ShowDialog()))
                {
                    foreach (ListViewItem checkItem in lvw_UserList.CheckedItems)
                    {
                        User user = checkItem.Tag as User;
                        if (null == user)
                        {
                            MessageBox.Show("No user enroll information, cannot be saved", "Prompt", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                            return;
                        }

                        string fileName = browser.SelectedPath + @"\UserData";
                        if (false == Directory.Exists(fileName))
                        {
                            Directory.CreateDirectory(fileName);
                        }

                        fileName += @"\OD_" + user.DIN.ToString().PadLeft(18, '0') + ".db";
                        if (File.Exists(fileName))
                        {
                            if (DialogResult.No.Equals(MessageBox.Show(string.Format("The file {0} already exists, overwrite?", "OD_"
                                + user.DIN.ToString().PadLeft(18, '0') + ".db"), "Prompt",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Information)))
                            {
                                continue;
                            }
                        }

                        bool result = enrollFileMgr.SaveUserEnrollDataAsDB(fileName, user);
                        if (false == result)
                        {
                            MessageBox.Show("Save One Enroll to File Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    MessageBox.Show("Save One Enroll to File Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Get/Set User detail info.

        private void setUserDetailInfoButton_Click(object sender, EventArgs e)
        {
            object extraProperty = new object();
            object extraData = new object();
            extraData = false;
            int enrollType = 0;

            try
            {
                if (shareUser == null)
                {
                    shareUser = new User();
                    shareUser.Enrolls = new List<Enroll>();
                    Enroll enroll = new Enroll();
                    shareUser.Enrolls.Add(enroll);
                }
                shareUser.DIN = (UInt64)nud_DIN.Value;
                shareUser.Privilege = GetPrivilege();
                shareUser.Enrolls[0].DIN = shareUser.DIN;
                shareUser.Enrolls[0].Password = txt_Pwd.Text;
                enrollType += Zd2911Utils.SetBit(0, 10); //password is 10, fp0-fp9, card is 11
                shareUser.Enrolls[0].CardID = txt_Card.Text;
                enrollType += Zd2911Utils.SetBit(0, 11); //password is 10, fp0-fp9, card is 11
                shareUser.Enrolls[0].EnrollType = (EnrollType)enrollType;
                shareUser.UserName = txt_UserName.Text;
                shareUser.Comment = ExtInfoTextBox.Text;
                shareUser.Enable = Convert.ToBoolean(userEnableComboBox.SelectedIndex);
                shareUser.AttType = (int)userAttTypeIdNumericUpDown.Value;
                shareUser.AccessControl = userAccessControlComboBox.SelectedIndex;
                shareUser.AccessTimeZone = (int)userPassZoneNumericUpDown.Value;
                shareUser.Department = (int)userDeptIdNumericUpDown.Value;
                shareUser.UserGroup = (int)userGroupIdNumericUpDown.Value;
                shareUser.ValidityPeriod = Convert.ToBoolean(userValidityPeriodComboBox.SelectedIndex);
                shareUser.ValidDate = userStartDateTimePicker.Value;
                shareUser.InvalidDate = userEndDateTimePicker.Value;
                shareUser.Res = (uint)userResNumericUpDown.Value;

                bool result = deviceConnection.SetProperty(UserProperty.Enroll, extraProperty, shareUser, extraData);
                if (false == result)
                {
                    MessageBox.Show("Set user detail info. Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show("Set user detail info. Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void getUserDetailInfoButton_Click(object sender, EventArgs e)
        {
            try
            {
                object extraProperty = new object();
                object extraData = new object();

                User user = new User();
                user.DIN = (UInt64)nud_DIN.Value;
                bool result = deviceConnection.GetProperty(UserProperty.Enroll, extraProperty, ref user, ref extraData);
                if (result)
                {
                    cbo_Role.SelectedIndex = user.Privilege / 2;
                    txt_Pwd.Text = user.Enrolls[0].Password;
                    txt_Card.Text = user.Enrolls[0].CardID;
                    txt_UserName.Text = user.UserName;
                    ExtInfoTextBox.Text = user.Comment;
                    userEnableComboBox.SelectedIndex = Convert.ToInt32(user.Enable);
                    userAttTypeIdNumericUpDown.Value = user.AttType;
                    userAccessControlComboBox.SelectedIndex = user.AccessControl;
                    userPassZoneNumericUpDown.Value = user.AccessTimeZone;
                    userDeptIdNumericUpDown.Value = user.Department;
                    userGroupIdNumericUpDown.Value = user.UserGroup;
                    userValidityPeriodComboBox.SelectedIndex = Convert.ToInt32(user.ValidityPeriod);
                    userStartDateTimePicker.Value = user.ValidDate;
                    userEndDateTimePicker.Value = user.InvalidDate;

                    Array.Copy(user.Enrolls[0].Fingerprint, fpBytes, Zd2911Utils.MaxFingerprintLength);

                    txt_UserFpData.Text = ConvertObject.ConvertByteToHex(fpBytes).Replace("-", " ");

                    shareUser = user;
                }
                else
                {
                    MessageBox.Show("Get user detail info. Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion


        #region Image fp enroll

        private const int ImageFpWidth = 242;
        private const int ImageFpHeight = 266;

        public static void ConvertImageFileToFpBytes(string filename, ref byte[] data)
        {
            Image img = Image.FromFile(filename);

            Bitmap bmp = new Bitmap(ImageFpWidth, ImageFpHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            int h = bmp.Height;
            int w = bmp.Width;
            int x = (ImageFpWidth - img.Width) / 2;
            int y = (ImageFpHeight - img.Height) / 2;
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(Brushes.White, 0, 0, w, h);
            g.DrawImage(img, x, y, img.Width, img.Height);

            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    Color c = bmp.GetPixel(j, i);

                    data[i * w + j] = (byte)((float)(c.R + c.G + c.B) / 3.0f);
                }
            }
        }

        private void imageFpEnrollButton_Click(object sender, EventArgs e)
        {
            if (-1 == cbo_FpNo.SelectedIndex)
            {
                MessageBox.Show("Please Select Finger SN", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_FpNo.Focus();
                return;
            }

            if (-1 == cbo_Role.SelectedIndex)
            {
                MessageBox.Show("Please Select Privilege", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_Role.Focus();
                return;
            }

            try
            {
                if (DialogResult.OK.Equals(imageFpOpenFileDialog.ShowDialog()))
                {
                    object extraProperty = new object();
                    object extraData = new object();
                    extraData = Global.DeviceBusy;

                    try
                    {
                        bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);

                        User user = new User();
                        Enroll enroll = new Enroll();
                        user.DIN = (UInt64)nud_DIN.Value;
                        user.Privilege = GetPrivilege();
                        enroll.DIN = user.DIN;
                        enroll.EnrollType = (EnrollType)cbo_FpNo.SelectedIndex;

                        byte[] fingerprint = new byte[Zd2911Utils.ImageFpDataLength];
                        ConvertImageFileToFpBytes(imageFpOpenFileDialog.FileName, ref fingerprint);

                        enroll.Fingerprint = fingerprint;

                        user.Enrolls.Add(enroll);
                        extraProperty = UserEnrollCommand.WriteFingerprint;
                        result = deviceConnection.SetProperty(UserProperty.ImageFpEnroll, extraProperty, user, extraData);
                        if (result)
                        {
                            MessageBox.Show("Enroll Image FP Data Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Enroll Image FP Data Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        extraData = Global.DeviceIdle;
                        deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
