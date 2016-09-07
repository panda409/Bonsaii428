using System;
using System.Windows.Forms;
using Riss.Devices;
using ZDC2911Demo.IConvert;
using ZDC2911Demo.Entity;

namespace ZDC2911Demo.UI {
    public partial class CMForm : Form {
        private Device device;
        private DeviceConnection deviceConnection;
        private DeviceCommEty deviceEty;

        public CMForm() {
            InitializeComponent();
        }

        public void SetButtonEnabled(bool flag) {
            foreach (Control c in group.Controls) {
                if (c.GetType().Equals(typeof(Button))) {
                    c.Enabled = flag;
                }
            }
        }

        private void CMForm_Load(object sender, EventArgs e) {
            cbo_COMM.SelectedIndex = 0;
            cbo_BaudRate.SelectedIndex = 0;
            btn_CloseDevice.Enabled = false;
            SetButtonEnabled(false);
        }

        #region Device: Open, Close
        private void btn_OpenDevice_Click(object sender, EventArgs e)
        {
            try
            {
                device = new Device();
                device.DN = (int)nud_DN.Value;
                device.Password = nud_Pwd.Value.ToString();
                device.Model = "ZDC2911";
                device.ConnectionModel = 5;//等于5时才能正确加载ZD2911通讯模块

                if (rdb_USB.Checked)
                {
                    device.CommunicationType = CommunicationType.Usb;
                }
                else if (rdb_TCP.Checked)
                {
                    if (string.IsNullOrEmpty(txt_IP.Text.Trim()))
                    {
                        MessageBox.Show("Please Input IP Address", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txt_IP.Focus();
                        return;
                    }

                    if (false == ConvertObject.IsCorrenctIP(txt_IP.Text.Trim()))
                    {
                        MessageBox.Show("Illegal IP Address", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txt_IP.Focus();
                        return;
                    }

                    device.IpAddress = txt_IP.Text.Trim();
                    device.IpPort = (int)nud_Port.Value;
                    device.CommunicationType = CommunicationType.Tcp;
                }
                else if (rdb_COMM.Checked)
                {
                    device.SerialPort = Convert.ToInt32(cbo_COMM.SelectedItem.ToString().Replace("COM", string.Empty));
                    device.Baudrate = Convert.ToInt32(cbo_BaudRate.SelectedItem);
                    device.CommunicationType = CommunicationType.Serial;
                }
                else if (p2pRadioButton.Checked)
                {
                    device.CommunicationType = CommunicationType.P2P;
                    device.SerialNumber = p2pAddrTextBox.Text.Trim();  //20130819
                    Riss.Devices.P2pUtils.SetP2pServerIpAddress(p2pServerTextBox.Text.Trim());
                }

                deviceConnection = DeviceConnection.CreateConnection(ref device);

                if (deviceConnection.Open() > 0)
                {
                    deviceEty = new DeviceCommEty();
                    deviceEty.Device = device;
                    deviceEty.DeviceConnection = deviceConnection;
                    btn_CloseDevice.Enabled = true;
                    SetButtonEnabled(true);
                    btn_OpenDevice.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Connect Device Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_CloseDevice_Click(object sender, EventArgs e) {
            deviceConnection.Close();
            btn_CloseDevice.Enabled = false;
            SetButtonEnabled(false);
            btn_OpenDevice.Enabled = true;
            device = null;
            deviceConnection = null;
            deviceEty = null;
        }
        #endregion

        #region Function
        private void btn_SystemSetting_Click(object sender, EventArgs e) {
            SystemSettingForm ssForm = new SystemSettingForm(deviceEty);
            ssForm.ShowDialog();
        }

        private void btn_AlarmSetting_Click(object sender, EventArgs e) {
            AlarmForm alarmForm = new AlarmForm(deviceEty);
            alarmForm.ShowDialog();
        }

        private void btn_AccessSetting_Click(object sender, EventArgs e) {
            //ACForm acForm = new ACForm(deviceEty);
            //acForm.ShowDialog();
            AccessControlForm acForm = new AccessControlForm(deviceEty);
            acForm.ShowDialog();
        }

        private void btn_AttendanceSetting_Click(object sender, EventArgs e) {
            AttForm attForm = new AttForm(deviceEty);
            attForm.ShowDialog();
        }

        private void btn_EnrollManagement_Click(object sender, EventArgs e) {
            EnrollForm enrollForm = new EnrollForm(deviceEty);
            enrollForm.ShowDialog();
        }

        private void btn_SlogManagement_Click(object sender, EventArgs e) {
            SLogForm slogForm = new SLogForm(deviceEty);
            slogForm.ShowDialog();
        }

        private void btn_GlogManagement_Click(object sender, EventArgs e) {
            GLogForm glogForm = new GLogForm(deviceEty);
            glogForm.ShowDialog();
        }
        #endregion

        private void btn_RealTimeLog_Click(object sender, EventArgs e) {
            RealTimeLogForm realTimeForm = new RealTimeLogForm();
            realTimeForm.ShowDialog();
        }

        private void btn_NameDataSetting_Click(object sender, EventArgs e) {
            NameDataSettingForm ndsForm = new NameDataSettingForm();
            ndsForm.ShowDialog();
        }

        private void p2pRadioButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void p2pAddrTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void p2pServerTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_IP_TextChanged(object sender, EventArgs e)
        {

        }

        private void nud_Port_ValueChanged(object sender, EventArgs e)
        {

        }

        private void rdb_TCP_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
