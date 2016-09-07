using System;
using System.Windows.Forms;
using ZDC2911Demo.Business;
using ZDC2911Demo.Entity;
using Riss.Devices;

namespace ZDC2911Demo.UI {
    public partial class ACForm : Form {
        private Device device;
        private DeviceConnection deviceConnection;

        public ACForm(DeviceCommEty deviceEty) {
            InitializeComponent();
            device = deviceEty.Device;
            deviceConnection = deviceEty.DeviceConnection;
        }

        private void ACForm_Load(object sender, EventArgs e) {
            InitData.InitGroupNo(cbo_RoleGroupNo);
            InitData.InitZoneNo(cbo_ZoneNo);
            InitData.InitHour(cbo_ZoneBeginHour);
            InitData.InitMinuteOrSecond(cbo_ZoneBeginMinute);
            InitData.InitHour(cbo_ZoneEndHour);
            InitData.InitMinuteOrSecond(cbo_ZoneEndMinute);
            InitData.InitGroupNo(cbo_GroupNo);
            InitData.InitZoneNo(cbo_ZoneNoOne);
            InitData.InitZoneNo(cbo_ZoneNoTwo);
            InitData.InitZoneNo(cbo_ZoneNoThree);

            dtp_BeginDate.MinDate = InitData.MinDateTime;
            dtp_BeginDate.MaxDate = InitData.MaxDateTime;
            dtp_BeginDate.Value = InitData.MinDateTime;
            dtp_EndDate.MinDate = InitData.MinDateTime;
            dtp_EndDate.MaxDate = InitData.MaxDateTime;
            dtp_EndDate.Value = InitData.MaxDateTime;
        }

        #region Access Control Parameter Settings
        private void btn_ParamGet_Click(object sender, EventArgs e) {
            if (-1 == cbo_ParamName.SelectedIndex) {
                MessageBox.Show("Please Select Parameter", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_ParamName.Focus();
                return;
            }

            try {
                object extraProperty = new object();
                object extraData = new object();
                byte[] paramIndex = BitConverter.GetBytes(cbo_ParamName.SelectedIndex + 32);
                extraData = paramIndex;
                bool result = deviceConnection.GetProperty(DeviceProperty.SysParam, extraProperty, ref device, ref extraData);
                if (result) {
                    int paramValue = BitConverter.ToInt32((byte[])extraData, 0);
                    txt_ParamValue.Text = paramValue.ToString();
                } else {
                    MessageBox.Show("Get Access Control Parameter Settings Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_ParamSet_Click(object sender, EventArgs e) {
            if (-1 == cbo_ParamName.SelectedIndex) {
                MessageBox.Show("Please Select Parameter", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_ParamName.Focus();
                return;
            }

            try {
                object extraProperty = new object();
                object extraData = new object();
                byte[] data = new byte[8];
                int paramValue = Convert.ToInt32(txt_ParamValue.Text.Trim());
                Array.Copy(BitConverter.GetBytes(cbo_ParamName.SelectedIndex + 32), 0, data, 0, 4);
                Array.Copy(BitConverter.GetBytes(paramValue), 0, data, 4, 4);
                extraData = data;
                bool result = deviceConnection.SetProperty(DeviceProperty.SysParam, extraProperty, device, extraData);
                if (result) {
                    MessageBox.Show("Set Access Control Parameter Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);                    
                } else {
                    MessageBox.Show("Set Access Control Parameter Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region User Role Settings
        private void btn_RoleGet_Click(object sender, EventArgs e) {
            try {
                object extraProperty = new object();
                extraProperty = AccessContorlCommand.UserAccessCtrl;
                object extraData = new object();
                User user = new User();
                user.DIN = (UInt64)nud_CtrlDIN.Value;
                bool result = deviceConnection.GetProperty(UserProperty.AccessControlSettings, extraProperty, ref user, ref extraData);
                if (result) {
                    cbo_RoleGroupNo.SelectedIndex = user.AccessTimeZone - 1;
                    chk_RoleEnable.Checked = user.Enable;
                } else {                    
                    MessageBox.Show("Get User Role Settings Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_RoleSet_Click(object sender, EventArgs e) {
            if (-1 == cbo_RoleGroupNo.SelectedIndex) {
                MessageBox.Show("Please Select Time Group", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_RoleGroupNo.Focus();
                return;
            }

            try {
                object extraProperty = new object();
                extraProperty = AccessContorlCommand.UserAccessCtrl;
                object extraData = new object();
                User user = new User();
                user.DIN = (UInt64)nud_CtrlDIN.Value;
                user.AccessTimeZone = cbo_RoleGroupNo.SelectedIndex + 1;
                user.Enable = chk_RoleEnable.Checked;
                bool result = deviceConnection.SetProperty(UserProperty.AccessControlSettings, extraProperty, user, extraData);
                if (result) {
                    MessageBox.Show("Set User Role Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } else {                    
                    MessageBox.Show("Set User Role Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Time Zone Settings
        private void btn_ZoneGet_Click(object sender, EventArgs e) {
            if (-1 == cbo_ZoneNo.SelectedIndex) {
                MessageBox.Show("Please Select SN", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_ZoneNo.Focus();
                return;
            }

            if (-1 == cbo_Weekday.SelectedIndex) {
                MessageBox.Show("Please Select Week Day", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_Weekday.Focus();
                return;
            }

            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;

            try {
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                extraProperty = AccessContorlCommand.TimeZone;
                result = deviceConnection.GetProperty(DeviceProperty.AccessControlSettings, extraProperty, ref device, ref extraData);
                if (result) {
                    byte[] data = (byte[])extraData;//获取30 * 7组数据
                    byte[] timerZone = new byte[6];
                    int index = timerZone.Length * (cbo_ZoneNo.SelectedIndex * Zd2911Utils.TimeZoneWeekCount
                        + cbo_Weekday.SelectedIndex);//根据时段序号、星期序号获取对应位置的数据
                    Array.Copy(data, index, timerZone, 0, timerZone.Length);
                    cbo_ZoneBeginHour.SelectedIndex = timerZone[2];
                    cbo_ZoneBeginMinute.SelectedIndex = timerZone[3];
                    cbo_ZoneEndHour.SelectedIndex = timerZone[4];
                    cbo_ZoneEndMinute.SelectedIndex = timerZone[5];
                } else {
                    MessageBox.Show("Get Time Zone Settings Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                extraData = Global.DeviceIdle;
                deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
            }
        }

        private void btn_ZoneSet_Click(object sender, EventArgs e) {
            if (-1 == cbo_ZoneNo.SelectedIndex) {
                MessageBox.Show("Please Select SN", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_ZoneNo.Focus();
                return;
            }

            if (-1 == cbo_Weekday.SelectedIndex) {
                MessageBox.Show("Please Select Week Day", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_Weekday.Focus();
                return;
            }

            if (-1 == cbo_ZoneBeginHour.SelectedIndex) {
                MessageBox.Show("Please Select Begin Time: Hour", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_ZoneBeginHour.Focus();
                return;
            }

            if (-1 == cbo_ZoneBeginMinute.SelectedIndex) {
                MessageBox.Show("Please Select Begin Time: Minute", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_ZoneBeginMinute.Focus();
                return;
            }

            if (-1 == cbo_ZoneEndHour.SelectedIndex) {
                MessageBox.Show("Please Select End Time: Hour", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_ZoneEndHour.Focus();
                return;
            }

            if (-1 == cbo_ZoneEndMinute.SelectedIndex) {
                MessageBox.Show("Please Select End Time: Minute", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_ZoneEndMinute.Focus();
                return;
            }

            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;

            try {
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                extraProperty = AccessContorlCommand.TimeZone;
                result = deviceConnection.GetProperty(DeviceProperty.AccessControlSettings, extraProperty, ref device, ref extraData);
                if (false == result) {
                    MessageBox.Show("Set Time Zone Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                byte[] data = (byte[])extraData;//获取30 * 7组数据
                //根据时段序号、星期序号获取对应位置的数据
                int index = 6 * (cbo_ZoneNo.SelectedIndex * Zd2911Utils.TimeZoneWeekCount + cbo_Weekday.SelectedIndex);
                data[index + 2] = (byte)cbo_ZoneBeginHour.SelectedIndex;
                data[index + 3] = (byte)cbo_ZoneBeginMinute.SelectedIndex;
                data[index + 4] = (byte)cbo_ZoneEndHour.SelectedIndex;
                data[index + 5] = (byte)cbo_ZoneEndMinute.SelectedIndex;
                extraData = data;
                result = deviceConnection.SetProperty(DeviceProperty.AccessControlSettings, extraProperty, device, extraData);
                if (result) {
                    MessageBox.Show("Set Time Zone Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);                    
                } else {
                    MessageBox.Show("Set Time Zone Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                extraData = Global.DeviceIdle;
                deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
            }
        }
        #endregion

        #region Time Group Settings
        private void btn_GroupGet_Click(object sender, EventArgs e) {
            if (-1 == cbo_GroupNo.SelectedIndex) {
                MessageBox.Show("Please Select SN", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_GroupNo.Focus();
                return;
            }

            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;

            try {
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                extraProperty = AccessContorlCommand.GroupTime;
                result = deviceConnection.GetProperty(DeviceProperty.AccessControlSettings, extraProperty, ref device, ref extraData);
                if (result) {
                    byte[] data = (byte[])extraData;//返回30组数据
                    byte[] timerGroup = new byte[10];
                    Array.Copy(data, cbo_GroupNo.SelectedIndex * 10, timerGroup, 0, timerGroup.Length);//根据序号获取相应位置的数据
                    cbo_GroupMultiUser.SelectedIndex = timerGroup[2] - 1;
                    cbo_ZoneNoOne.SelectedIndex = timerGroup[3] - 1;
                    cbo_ZoneNoTwo.SelectedIndex = timerGroup[4] - 1;
                    cbo_ZoneNoThree.SelectedIndex = timerGroup[5] - 1;
                } else {
                    MessageBox.Show("Get Time Group Settings Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                extraData = Global.DeviceIdle;
                deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
            }
        }

        private void btn_GroupSet_Click(object sender, EventArgs e) {
            if (-1 == cbo_GroupNo.SelectedIndex) {
                MessageBox.Show("Please Select SN", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_GroupNo.Focus();
                return;
            }

            if (-1 == cbo_GroupMultiUser.SelectedIndex) {
                MessageBox.Show("Please Select Multi User Count", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_GroupMultiUser.Focus();
                return;
            }

            if (-1 == cbo_ZoneNoOne.SelectedIndex) {
                MessageBox.Show("Please Select Time Zone: First", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_ZoneNoOne.Focus();
                return;
            }

            if (-1 == cbo_ZoneNoTwo.SelectedIndex) {
                MessageBox.Show("Please Select Time Zone: Second", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_ZoneNoTwo.Focus();
                return;
            }

            if (-1 == cbo_ZoneNoThree.SelectedIndex) {
                MessageBox.Show("Please Select Time Zone: Third", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_ZoneNoThree.Focus();
                return;
            }

            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;

            try {
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                extraProperty = AccessContorlCommand.GroupTime;
                result = deviceConnection.GetProperty(DeviceProperty.AccessControlSettings, extraProperty, ref device, ref extraData);
                if (false == result) {
                    MessageBox.Show("Set Time Group Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                byte[] data = (byte[])extraData;
                data[cbo_GroupNo.SelectedIndex * 10 + 2] = (byte)(cbo_GroupMultiUser.SelectedIndex + 1);//根据序号修改相应位置的数据
                data[cbo_GroupNo.SelectedIndex * 10 + 3] = (byte)(cbo_ZoneNoOne.SelectedIndex + 1);
                data[cbo_GroupNo.SelectedIndex * 10 + 4] = (byte)(cbo_ZoneNoTwo.SelectedIndex + 1);
                data[cbo_GroupNo.SelectedIndex * 10 + 5] = (byte)(cbo_ZoneNoThree.SelectedIndex + 1);
                extraData = data;
                result = deviceConnection.SetProperty(DeviceProperty.AccessControlSettings, extraProperty, device, extraData);
                if (result) {
                    MessageBox.Show("Set Time Group Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);                    
                } else {
                    MessageBox.Show("Set Time Group Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                extraData = Global.DeviceIdle;
                deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
            }
        }
        #endregion

        #region User Available Period Settings
        private void btn_AvailableGet_Click(object sender, EventArgs e) {
            try {
                object extraProperty = new object();
                extraProperty = AccessContorlCommand.UserPeriod;
                object extraData = new object();
                User user=new User();
                user.DIN=(UInt64)nud_PeriodDIN.Value;
                bool result = deviceConnection.GetProperty(UserProperty.AccessControlSettings, extraProperty, ref user, ref extraData);
                if (result) {
                    byte[] data = (byte[])extraData;
                    dtp_BeginDate.Value = new DateTime(data[0] + 2000, data[1], data[2]);
                    dtp_EndDate.Value = new DateTime(data[3] + 2000, data[4], data[5]);
                } else {
                    MessageBox.Show("Get User Available Period Settings Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_AvaiableSet_Click(object sender, EventArgs e) {
            try {
                object extraProperty = new object();
                extraProperty = AccessContorlCommand.UserPeriod;
                object extraData = new object();
                User user = new User();
                user.DIN = (UInt64)nud_PeriodDIN.Value;
                byte[] data = new byte[8];
                data[0] = (byte)(dtp_BeginDate.Value.Year - 2000);
                data[1] = (byte)dtp_BeginDate.Value.Month;
                data[2] = (byte)dtp_BeginDate.Value.Day;
                data[3] = (byte)(dtp_EndDate.Value.Year - 2000);
                data[4] = (byte)dtp_EndDate.Value.Month;
                data[5] = (byte)dtp_EndDate.Value.Day;
                extraData = data;
                bool result = deviceConnection.SetProperty(UserProperty.AccessControlSettings, extraProperty, user, extraData);
                if (result) {
                    MessageBox.Show("Set User Available Period Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } else {
                    MessageBox.Show("Set User Available Period Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}
