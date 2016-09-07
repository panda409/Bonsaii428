using System;
using System.Text;
using System.Windows.Forms;
using ZDC2911Demo.Business;
using ZDC2911Demo.Entity;
using Riss.Devices;

namespace ZDC2911Demo.UI {
    public partial class AttForm : Form {
        private Device device;
        private DeviceConnection deviceConnection;

        public AttForm(DeviceCommEty deviceEty) {
            InitializeComponent();
            device = deviceEty.Device;
            deviceConnection = deviceEty.DeviceConnection;
        }

        private void AttForm_Load(object sender, EventArgs e) {

            InitData.InitMessageSN(cbo_MessageSN);
            dtp_MessageBeginDatetime.MinDate = InitData.MinDateTime;
            dtp_MessageBeginDatetime.MaxDate = InitData.MaxDateTime;
            dtp_MessageEndDatetime.MinDate = InitData.MinDateTime;
            dtp_MessageEndDatetime.MaxDate = InitData.MaxDateTime;
            dtp_MessageEndDatetime.Value = dtp_MessageBeginDatetime.Value.AddDays(1);
        }

        private void NumericUpDown_Selected(object sender, EventArgs e) {
            NumericUpDown nud = sender as NumericUpDown;
            int length = nud.Value.ToString().Length;
            nud.Select(0, length);
        }

        #region Message Settings
        private void btn_MessageGet_Click(object sender, EventArgs e) {
            if (-1 == cbo_MessageSN.SelectedIndex) {
                MessageBox.Show("Please select SN", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_MessageSN.Focus();
                return;
            }

            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;

            try {
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                extraData = Zd2911Utils.DeviceMessage;
                result = deviceConnection.GetProperty(DeviceProperty.Message, extraProperty, ref device, ref extraData);
                if (result) {
                    byte[] data = Encoding.Unicode.GetBytes((string)extraData);
                    byte[] message = new byte[Zd2911Utils.MaxDeviceMessageLength];
                    Array.Copy(data, cbo_MessageSN.SelectedIndex * Zd2911Utils.MaxDeviceMessageLength,
                        message, 0, message.Length);
                    chk_MessageEnable.Checked = Convert.ToBoolean(message[0]);
                    cbo_MessageType.SelectedIndex = message[1];
                    cbo_MessageSound.SelectedIndex = message[2];
                    dtp_MessageBeginDatetime.Value = new DateTime(message[6] + 2000, message[7], message[8], message[9],
                        message[10], 0);
                    dtp_MessageEndDatetime.Value = new DateTime(message[11] + 2000, message[12], message[13], message[14],
                        message[15], 0);
                    nud_MessageID.Value = BitConverter.ToUInt64(message, 16);
                    txt_MessageContent.Text = Encoding.Unicode.GetString(message, 24, 30 * 2).Replace("\0", "");
                } else {
                    MessageBox.Show("Get Message Settings Fail.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                extraData = Global.DeviceIdle;
                deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
            }
        }

        private void btn_MessageSet_Click(object sender, EventArgs e) {
            if (-1 == cbo_MessageSN.SelectedIndex) {
                MessageBox.Show("Please select SN", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_MessageSN.Focus();
                return;
            }

            if (-1 == cbo_MessageType.SelectedIndex) {
                MessageBox.Show("Please select Type", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_MessageType.Focus();
                return;
            }

            if (0 != cbo_MessageType.SelectedIndex) {
                if (0 == nud_MessageID.Value) {
                    MessageBox.Show("Please input ID", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    nud_MessageID.Focus();
                    return;
                }
            }

            if (-1 == cbo_MessageSound.SelectedIndex) {
                MessageBox.Show("Please select Sound", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_MessageSound.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txt_MessageContent.Text.Trim())) {
                MessageBox.Show("Please Input Content.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_MessageContent.Focus();
                return;
            }

            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;

            try {
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                extraData = Zd2911Utils.DeviceMessage;
                result = deviceConnection.GetProperty(DeviceProperty.Message, extraProperty, ref device, ref extraData);
                if (false == result) {
                    MessageBox.Show("Set Message Fail.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                byte[] data = Encoding.Unicode.GetBytes((string)extraData);
                data[cbo_MessageSN.SelectedIndex * Zd2911Utils.MaxDeviceMessageLength + 0] = 
                    Convert.ToByte(chk_MessageEnable.Checked);
                data[cbo_MessageSN.SelectedIndex * Zd2911Utils.MaxDeviceMessageLength + 1] =
                    (byte)cbo_MessageType.SelectedIndex;
                data[cbo_MessageSN.SelectedIndex * Zd2911Utils.MaxDeviceMessageLength + 2] =
                    (byte)cbo_MessageSound.SelectedIndex;
                data[cbo_MessageSN.SelectedIndex * Zd2911Utils.MaxDeviceMessageLength + 3] = 0;
                data[cbo_MessageSN.SelectedIndex * Zd2911Utils.MaxDeviceMessageLength + 4] = 0;
                data[cbo_MessageSN.SelectedIndex * Zd2911Utils.MaxDeviceMessageLength + 5] = 0;
                data[cbo_MessageSN.SelectedIndex * Zd2911Utils.MaxDeviceMessageLength + 6] =
                    (byte)(dtp_MessageBeginDatetime.Value.Year - 2000);
                data[cbo_MessageSN.SelectedIndex * Zd2911Utils.MaxDeviceMessageLength + 7] =
                    (byte)dtp_MessageBeginDatetime.Value.Month;
                data[cbo_MessageSN.SelectedIndex * Zd2911Utils.MaxDeviceMessageLength + 8] =
                    (byte)dtp_MessageBeginDatetime.Value.Day;
                data[cbo_MessageSN.SelectedIndex * Zd2911Utils.MaxDeviceMessageLength + 9] =
                    (byte)dtp_MessageBeginDatetime.Value.Hour;
                data[cbo_MessageSN.SelectedIndex * Zd2911Utils.MaxDeviceMessageLength + 10] =
                    (byte)dtp_MessageBeginDatetime.Value.Minute;
                data[cbo_MessageSN.SelectedIndex * Zd2911Utils.MaxDeviceMessageLength + 11] =
                    (byte)(dtp_MessageEndDatetime.Value.Year - 2000);
                data[cbo_MessageSN.SelectedIndex * Zd2911Utils.MaxDeviceMessageLength + 12] =
                    (byte)dtp_MessageEndDatetime.Value.Month;
                data[cbo_MessageSN.SelectedIndex * Zd2911Utils.MaxDeviceMessageLength + 13] =
                    (byte)dtp_MessageEndDatetime.Value.Day;
                data[cbo_MessageSN.SelectedIndex * Zd2911Utils.MaxDeviceMessageLength + 14] =
                    (byte)dtp_MessageEndDatetime.Value.Hour;
                data[cbo_MessageSN.SelectedIndex * Zd2911Utils.MaxDeviceMessageLength + 15] =
                    (byte)dtp_MessageEndDatetime.Value.Minute;
                byte[] IDBytes = BitConverter.GetBytes((UInt64)nud_MessageID.Value);
                Array.Copy(IDBytes, 0, data, cbo_MessageSN.SelectedIndex * Zd2911Utils.MaxDeviceMessageLength + 16,
                    IDBytes.Length);
                byte[] contentBytes = Encoding.Unicode.GetBytes(txt_MessageContent.Text.Trim());
                Array.Copy(contentBytes, 0, data, cbo_MessageSN.SelectedIndex * Zd2911Utils.MaxDeviceMessageLength + 24,
                    contentBytes.Length);
                extraData = Encoding.Unicode.GetString(data);
                extraProperty = Zd2911Utils.DeviceMessage;
                result = deviceConnection.SetProperty(DeviceProperty.Message, extraProperty, device, extraData);
                if (result) {
                    MessageBox.Show("Set Message Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                } else {
                    MessageBox.Show("Set Message Fail.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                extraData = Global.DeviceIdle;
                deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
            }
        }
        #endregion
    }
}
