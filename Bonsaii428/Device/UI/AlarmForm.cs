using System;
using System.Text;
using System.Windows.Forms;
using ZDC2911Demo.Business;
using ZDC2911Demo.Entity;
using Riss.Devices;

namespace ZDC2911Demo.UI {
    public partial class AlarmForm : Form {
        private Device device;
        private DeviceConnection deviceConnection;

        public AlarmForm(DeviceCommEty deviceEty) {
            InitializeComponent();
            device = deviceEty.Device;
            deviceConnection = deviceEty.DeviceConnection;
        }

        private void AlarmForm_Load(object sender, EventArgs e) {
            InitData.InitAlarmDN(cbo_AlarmDN);
            InitData.InitHour(cbo_AlarmHour);
            InitData.InitMinuteOrSecond(cbo_AlarmMinute);
            InitData.InitValidAttendTimeDN(cbo_AttDN);
            InitData.InitHour(cbo_BeginHour);
            InitData.InitMinuteOrSecond(cbo_BeginMinute);
            InitData.InitHour(cbo_EndHour);
            InitData.InitMinuteOrSecond(cbo_EndMinute);
        }

        #region Alarm Settings
        private void btn_AlarmGet_Click(object sender, EventArgs e) {
            if (-1 == cbo_AlarmDN.SelectedIndex) {
                MessageBox.Show("Please Select SN", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_AlarmDN.Focus();
                return;
            }

            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;

            try {
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                extraData = Zd2911Utils.DeviceAlarmClock;
                result = deviceConnection.GetProperty(DeviceProperty.Bell, extraProperty, ref device, ref extraData);
                if (result) {
                    string bellData = (string)extraData;
                    byte[] dataBytes = Encoding.Unicode.GetBytes(bellData);//返回字节数组长度：192
                    byte[] bell = new byte[Zd2911Utils.BellLength];
                    Array.Copy(dataBytes, cbo_AlarmDN.SelectedIndex * Zd2911Utils.BellLength, 
                        bell, 0, bell.Length);//根据序号获取相应位置数据，每一组数据字节长度：8
                    cbo_AlarmHour.SelectedIndex = bell[0];
                    cbo_AlarmMinute.SelectedIndex = bell[1];
                    cbo_AlarmCycle.SelectedIndex = bell[2];
                    nud_AlarmDelay.Value = bell[3];
                } else {
                    MessageBox.Show("Get Alarm Settings Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                extraData = Global.DeviceIdle;
                deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
            }
        }

        private void btn_AlarmSet_Click(object sender, EventArgs e) {
            if (-1 == cbo_AlarmDN.SelectedIndex) {
                MessageBox.Show("Please Select SN", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_AlarmDN.Focus();
                return;
            }

            if (-1 == cbo_AlarmHour.SelectedIndex) {
                MessageBox.Show("Please Select Time: Hour", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_AlarmHour.Focus();
                return;
            }

            if (-1 == cbo_AlarmMinute.SelectedIndex) {
                MessageBox.Show("Please Select Time: Minute", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_AlarmMinute.Focus();
                return;
            }

            if (-1 == cbo_AlarmCycle.SelectedIndex) {
                MessageBox.Show("Please Select Cycle", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_AlarmCycle.Focus();
                return;
            }

            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;

            try {
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                extraData = Zd2911Utils.DeviceAlarmClock;
                result = deviceConnection.GetProperty(DeviceProperty.Bell, extraProperty, ref device, ref extraData);
                if (false == result) {
                    MessageBox.Show("Set Alarm Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string bellData = (string)extraData;
                byte[] data = Encoding.Unicode.GetBytes(bellData);
                //根据序号设置对应位置的数据
                data[cbo_AlarmDN.SelectedIndex * Zd2911Utils.BellLength + 0] = (byte)cbo_AlarmHour.SelectedIndex;
                data[cbo_AlarmDN.SelectedIndex * Zd2911Utils.BellLength + 1] = (byte)cbo_AlarmMinute.SelectedIndex;
                data[cbo_AlarmDN.SelectedIndex * Zd2911Utils.BellLength + 2] = (byte)cbo_AlarmCycle.SelectedIndex;
                data[cbo_AlarmDN.SelectedIndex * Zd2911Utils.BellLength + 3] = (byte)nud_AlarmDelay.Value;
                extraProperty = Zd2911Utils.DeviceAlarmClock;
                extraData = Encoding.Unicode.GetString(data);
                result = deviceConnection.SetProperty(DeviceProperty.Bell, extraProperty, device, extraData);
                if (result) {
                    MessageBox.Show("Set Alarm Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);                    
                } else {
                    MessageBox.Show("Set Alarm Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                extraData = Global.DeviceIdle;
                deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
            }
        }
        #endregion

        #region Valid Attendance Time Settings
        private void btn_AttGet_Click(object sender, EventArgs e) {
            if (-1 == cbo_AttType.SelectedIndex) {
                MessageBox.Show("Please Select Type", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_AttType.Focus();
                return;
            }

            if (-1 == cbo_AttDN.SelectedIndex) {
                MessageBox.Show("Please Select SN", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_AttDN.Focus();
                return;
            }

            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;

            try {
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                extraProperty = AttendanceCommand.LogTime;
                extraData = cbo_AttType.SelectedIndex;
                result = deviceConnection.GetProperty(DeviceProperty.Attendance, extraProperty, ref device, ref extraData);
                if (result) {
                    byte[] data = (byte[])extraData;//返回24组数据
                    byte[] logTime = new byte[6];
                    Array.Copy(data, cbo_AttDN.SelectedIndex * logTime.Length, logTime, 0, logTime.Length);//根据序号获取相应位置的数据
                    cbo_BeginHour.SelectedIndex = logTime[2];
                    cbo_BeginMinute.SelectedIndex = logTime[3];
                    cbo_EndHour.SelectedIndex = logTime[4];
                    cbo_EndMinute.SelectedIndex = logTime[5];
                } else {
                    MessageBox.Show("Get Valid Attendance Time Settings Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                extraData = Global.DeviceIdle;
                deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
            }
        }

        private void btn_AttSet_Click(object sender, EventArgs e) {
            if (-1 == cbo_AttType.SelectedIndex) {
                MessageBox.Show("Please Select Type", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_AttType.Focus();
                return;
            }

            if (-1 == cbo_AttDN.SelectedIndex) {
                MessageBox.Show("Please Select SN", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_AttDN.Focus();
                return;
            }

            if (-1 == cbo_BeginHour.SelectedIndex) {
                MessageBox.Show("Please Select Begin Time: Hour", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_BeginHour.Focus();
                return;
            }

            if (-1 == cbo_BeginMinute.SelectedIndex) {
                MessageBox.Show("Please Select Begin Time: Minute", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_BeginMinute.Focus();
                return;
            }

            if (-1 == cbo_EndHour.SelectedIndex) {
                MessageBox.Show("Please Select End Time: Hour", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_BeginHour.Focus();
                return;
            }

            if (-1 == cbo_EndMinute.SelectedIndex) {
                MessageBox.Show("Please Select End Time: Minute", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_EndMinute.Focus();
                return;
            }

            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;

            try {
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                extraProperty = AttendanceCommand.LogTime;
                extraData = cbo_AttType.SelectedIndex;
                result = deviceConnection.GetProperty(DeviceProperty.Attendance, extraProperty, ref device, ref extraData);
                if (false == result) {
                    MessageBox.Show("Set Valid Attendance Time Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                byte[] data = (byte[])extraData;
                data[cbo_AttDN.SelectedIndex * 6 + 2] = (byte)cbo_BeginHour.SelectedIndex;
                data[cbo_AttDN.SelectedIndex * 6 + 3] = (byte)cbo_BeginMinute.SelectedIndex;
                data[cbo_AttDN.SelectedIndex * 6 + 4] = (byte)cbo_EndHour.SelectedIndex;
                data[cbo_AttDN.SelectedIndex * 6 + 5] = (byte)cbo_EndMinute.SelectedIndex;
                extraData = data;
                result = deviceConnection.SetProperty(DeviceProperty.Attendance, extraProperty, device, extraData);
                if (result) {
                    MessageBox.Show("Set Valid Attendance Time Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } else {
                    MessageBox.Show("Set Valid Attendance Time Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                extraData = Global.DeviceIdle;
                deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
            }
        }
        #endregion

        #region User Attendance Type Settings
        private void btn_UserGet_Click(object sender, EventArgs e) {
            try {
                object extraProperty = new object();
                object extraData = new object();
                User user = new User();
                user.DIN = (UInt64)nud_DIN.Value;
                bool result = deviceConnection.GetProperty(UserProperty.Attendance, extraProperty, ref user, ref extraData);
                if (result) {
                    cbo_UserType.SelectedIndex = (int)extraData;
                } else {
                    MessageBox.Show("Get User Attendance Type Settings Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

        private void btn_UserSet_Click(object sender, EventArgs e) {
            if (-1 == cbo_UserType.SelectedIndex) {
                MessageBox.Show("Please Select Type", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_UserType.Focus();
                return;
            }

            try {
                object extraProperty = new object();
                object extraData = new object();
                extraData = cbo_UserType.SelectedIndex;
                User user = new User();
                user.DIN = (UInt64)nud_DIN.Value;
                bool result = deviceConnection.SetProperty(UserProperty.Attendance, extraProperty, user, extraData);
                if (result) {
                    MessageBox.Show("Set User Attendance Type Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);                    
                } else {
                    MessageBox.Show("Set User Attendance Type Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }
        #endregion
    }
}
