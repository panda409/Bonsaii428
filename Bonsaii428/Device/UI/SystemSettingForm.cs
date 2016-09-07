using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using ZDC2911Demo.Business;
using ZDC2911Demo.IConvert;
using ZDC2911Demo.Entity;
using Riss.Devices;

namespace ZDC2911Demo.UI
{
    public partial class SystemSettingForm : Form
    {
        private Device device;
        private DeviceConnection deviceConnection;

        public SystemSettingForm(DeviceCommEty deviceEty)
        {
            InitializeComponent();
            device = deviceEty.Device;
            deviceConnection = deviceEty.DeviceConnection;
        }

        private void SystemSettingForm_Load(object sender, EventArgs e)
        {
            InitData.InitTimerOnOffDN(cbo_DN);
            InitData.InitHour(cbo_PowerHour);
            InitData.InitMinuteOrSecond(cbo_PowerMinute);
        }

        #region System Parameter Settings
        private void btn_ParamGet_Click(object sender, EventArgs e)
        {
            if (-1 == cbo_Param.SelectedIndex)
            {
                MessageBox.Show("Please Select Parameter", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_Param.Focus();
                return;
            }

            try
            {
                object extraProperty = new object();
                object extraData = new object();
                byte[] paramIndex = BitConverter.GetBytes(cbo_Param.SelectedIndex);
                extraData = paramIndex;
                bool result = deviceConnection.GetProperty(DeviceProperty.SysParam, extraProperty, ref device, ref extraData);
                if (result)
                {
                    int paramValue = BitConverter.ToInt32((byte[])extraData, 0);
                    switch (cbo_Param.SelectedIndex)
                    {
                        case 26:
                        case 27:
                        case 28:
                        case 29:
                            txt_ParamValue.Text = ConvertObject.ConvertNumberToIPAddress(paramValue);
                            break;
                        default:
                            txt_ParamValue.Text = paramValue.ToString();
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Get Parameter Settings Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_ParamSet_Click(object sender, EventArgs e)
        {
            if (-1 == cbo_Param.SelectedIndex)
            {
                MessageBox.Show("Please Select Parameter", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_Param.Focus();
                return;
            }

            try
            {
                int paramValue = -1;
                switch (cbo_Param.SelectedIndex)
                {
                    case 26:
                    case 27:
                    case 28:
                    case 29:
                        if (false == ConvertObject.IsCorrenctIP(txt_ParamValue.Text.Trim()))
                        {
                            MessageBox.Show("Illegal IP address", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txt_ParamValue.Focus();
                            return;
                        }
                        paramValue = ConvertObject.ConvertIPAddressToNumber(txt_ParamValue.Text.Trim());
                        break;
                    default:
                        paramValue = Convert.ToInt32(txt_ParamValue.Text.Trim());
                        break;
                }

                object extraProperty = new object();
                object extraData = new object();
                byte[] data = new byte[8];
                Array.Copy(BitConverter.GetBytes(cbo_Param.SelectedIndex), 0, data, 0, 4);
                Array.Copy(BitConverter.GetBytes(paramValue), 0, data, 4, 4);
                extraData = data;
                bool result = deviceConnection.SetProperty(DeviceProperty.SysParam, extraProperty, device, extraData);
                if (result)
                {
                    MessageBox.Show("Set Parameter Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Set Parameter Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Timer On/Off Settings
        private void btn_PowerGet_Click(object sender, EventArgs e)
        {
            if (-1 == cbo_PowerType.SelectedIndex)
            {
                MessageBox.Show("Please Select Type", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_PowerType.Focus();
                return;
            }

            if (-1 == cbo_DN.SelectedIndex)
            {
                MessageBox.Show("Please Select SN", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_DN.Focus();
                return;
            }

            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;

            try
            {
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                extraData = Zd2911Utils.DevicePowerTimer;
                result = deviceConnection.GetProperty(DeviceProperty.PowerOnOffTime, extraProperty, ref device, ref extraData);
                if (result)
                {
                    string timerData = (string)extraData;
                    //返回字节数据长度：86
                    //索引0到47之间为12组定时开机数据，每组长度为4；
                    //索引47到85之前为12组定时关机数据，每组长度为4
                    byte[] data = Encoding.Unicode.GetBytes(timerData);
                    byte[] timer = new byte[Zd2911Utils.PowerTimeLength];
                    int index = cbo_PowerType.SelectedIndex * Zd2911Utils.PowerTimeCount * Zd2911Utils.PowerTimeLength
                        + cbo_DN.SelectedIndex * Zd2911Utils.PowerTimeLength;
                    Array.Copy(data, index, timer, 0, timer.Length);//根据序号获取相对应的数据
                    cbo_PowerHour.SelectedIndex = timer[0];
                    cbo_PowerMinute.SelectedIndex = timer[1];
                    chk_Power.Checked = Convert.ToBoolean(timer[2]);
                }
                else
                {
                    MessageBox.Show("Get Timer On/Off Settings Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void btn_PowerSet_Click(object sender, EventArgs e)
        {
            if (-1 == cbo_PowerType.SelectedIndex)
            {
                MessageBox.Show("Please Select Type", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_PowerType.Focus();
                return;
            }

            if (-1 == cbo_DN.SelectedIndex)
            {
                MessageBox.Show("Please Select SN", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_DN.Focus();
                return;
            }

            if (-1 == cbo_PowerHour.SelectedIndex)
            {
                MessageBox.Show("Please Select Time: Hour", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_PowerHour.Focus();
                return;
            }

            if (-1 == cbo_PowerMinute.SelectedIndex)
            {
                MessageBox.Show("Please Select Time: Minute", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_PowerMinute.Focus();
                return;
            }

            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;

            try
            {
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                extraData = Zd2911Utils.DevicePowerTimer;
                result = deviceConnection.GetProperty(DeviceProperty.PowerOnOffTime, extraProperty, ref device, ref extraData);
                if (result)
                {
                    string timerData = (string)extraData;
                    //返回字节数据长度：86
                    //索引0到47之间为12组定时开机数据，每组长度为4；
                    //索引47到85之前为12组定时关机数据，每组长度为4
                    byte[] data = Encoding.Unicode.GetBytes(timerData);
                    int index = cbo_PowerType.SelectedIndex * Zd2911Utils.PowerTimeCount * Zd2911Utils.PowerTimeLength
                        + cbo_DN.SelectedIndex * Zd2911Utils.PowerTimeLength;
                    //根据索引设置对应位置的数据
                    data[index + 0] = (byte)cbo_PowerHour.SelectedIndex;
                    data[index + 1] = (byte)cbo_PowerMinute.SelectedIndex;
                    data[index + 2] = Convert.ToByte(chk_Power.Checked);
                    extraProperty = Zd2911Utils.DevicePowerTimer;
                    extraData = Encoding.Unicode.GetString(data);
                    result = deviceConnection.SetProperty(DeviceProperty.PowerOnOffTime, extraProperty, device, extraData);
                    if (result)
                    {
                        MessageBox.Show("Set Timer On/Off Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Set Timer On/Off Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Set Timer On/Off Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        #region Time Settings
        private void btn_TimeGet_Click(object sender, EventArgs e)
        {
            try
            {
                object extraProperty = new object();
                object extraData = new object();
                bool result = deviceConnection.GetProperty(DeviceProperty.DeviceTime, extraProperty, ref device, ref extraData);
                if (result)
                {
                    DateTime dt = (DateTime)extraData;
                    txt_DeviceTime.Text = dt.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    MessageBox.Show("Get Device Time Settings Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_TimeSet_Click(object sender, EventArgs e)
        {
            try
            {
                object extraProperty = new object();
                object extraData = new object();
                bool result = deviceConnection.SetProperty(DeviceProperty.DeviceTime, extraProperty, device, extraData);
                if (result)
                {
                    MessageBox.Show("Synchronize the Device Time Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btn_TimeGet_Click(btn_TimeGet, e);
                }
                else
                {
                    MessageBox.Show("Synchronize the Device Time Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Device Record Information Settings
        private void btn_SystemGet_Click(object sender, EventArgs e)
        {
            try
            {
                object extraProperty = new object();
                object extraData = new object();
                bool result = deviceConnection.GetProperty(DeviceProperty.Status, extraProperty, ref device, ref extraData);
                if (result)
                {
                    UInt32[] count = (UInt32[])extraData;
                    txt_UserCount.Text = count[0].ToString();
                    txt_AdminCount.Text = count[1].ToString();
                    txt_FingerCount.Text = count[2].ToString();
                    txt_CardCount.Text = count[3].ToString();
                    txt_PwdCount.Text = count[4].ToString();
                    txt_ManageCount.Text = count[5].ToString();
                    txt_ExitCount.Text = count[6].ToString();
                    txt_HisManageCount.Text = count[7].ToString();
                    txt_HisExitCount.Text = count[8].ToString();
                }
                else
                {
                    MessageBox.Show("Get Log Information Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Get Mac Address
        private void btn_MacAddress_Click(object sender, EventArgs e)
        {
            try
            {
                object extraProperty = new object();
                object extraData = new object();
                bool result = deviceConnection.GetProperty(DeviceProperty.MacAddress, extraProperty, ref device, ref extraData);
                if (result)
                {
                    byte[] bytes = (byte[])extraData;
                    MessageBox.Show("Mac Address: " + ConvertObject.ConvertByteToHex(bytes), "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Get Mac Address Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Device Init
        private void btn_Reset_Click(object sender, EventArgs e)
        {
            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;

            try
            {
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                extraData = new object();
                result = deviceConnection.SetProperty(DeviceProperty.InitSettings, extraProperty, device, extraData);
                if (result)
                {
                    MessageBox.Show("Device Init Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Device Init Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        #region Device Title Settings
        private void btn_WelcomeGet_Click(object sender, EventArgs e)
        {
            try
            {
                object extraProperty = new object();
                object extraData = new object();
                extraData = Zd2911Utils.DeviceTile;
                bool result = deviceConnection.GetProperty(DeviceProperty.WelcomeTitle, extraProperty, ref device, ref extraData);
                if (result)
                {
                    txt_Welcome.Text = (string)extraData;
                }
                else
                {
                    MessageBox.Show("Get Welcome Title Settings Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_WelcomeSet_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txt_Welcome.Text.Trim()))
                {
                    MessageBox.Show("Please Input Welcome Title", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_Welcome.Focus();
                    return;
                }

                object extraProperty = new object();
                extraProperty = Zd2911Utils.DeviceTile;
                object extraData = new object();
                extraData = txt_Welcome.Text.Trim();
                bool result = deviceConnection.SetProperty(DeviceProperty.WelcomeTitle, extraProperty, device, extraData);
                if (result)
                {
                    MessageBox.Show("Set Welcome Title Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Set Welcome Title Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_StandbyGet_Click(object sender, EventArgs e)
        {
            try
            {
                object extraProperty = new object();
                object extraData = new object();
                extraData = Zd2911Utils.DeviceStandbyTitle;
                bool result = deviceConnection.GetProperty(DeviceProperty.StandbyTitle, extraProperty, ref device, ref extraData);
                if (result)
                {
                    txt_Standby.Text = (string)extraData;
                }
                else
                {
                    MessageBox.Show("Get Standby Title Settings Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_StandbySet_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txt_Standby.Text.Trim()))
                {
                    MessageBox.Show("Please Input Standby Title", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_Standby.Focus();
                    return;
                }

                object extraProperty = new object();
                extraProperty = Zd2911Utils.DeviceStandbyTitle;
                object extraData = new object();
                extraData = txt_Standby.Text.Trim();
                bool result = deviceConnection.SetProperty(DeviceProperty.StandbyTitle, extraProperty, device, extraData);
                if (result)
                {
                    MessageBox.Show("Set Standby Title Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Set Standby Title Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Device information:FirmwareVersion, Model
        private void btn_FirmwareVersionGet_Click(object sender, EventArgs e)
        {
            try
            {
                object extraProperty = new object();
                object extraData = new object();
                extraData = Zd2911Utils.DeviceFirmwareVersion;
                bool result = deviceConnection.GetProperty(DeviceProperty.FirmwareVersion, extraProperty, ref device, ref extraData);
                if (result)
                {
                    txt_FirmwareVersion.Text = (string)extraData;
                }
                else
                {
                    MessageBox.Show("Get Firmware Version Settings Fail.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void getSerialNoButton_Click(object sender, EventArgs e)
        {
            try
            {
                object extraProperty = new object();
                object extraData = new object();
                extraData = Zd2911Utils.DeviceSerialNo;
                bool result = deviceConnection.GetProperty(DeviceProperty.SerialNo, extraProperty, ref device, ref extraData);
                if (result)
                {
                    serialNoTextBox.Text = (string)extraData;
                }
                else
                {
                    MessageBox.Show("Get Serial number Settings Fail.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_ModelGet_Click(object sender, EventArgs e)
        {
            try
            {
                object extraProperty = new object();
                object extraData = new object();
                extraData = Zd2911Utils.DeviceModel;
                bool result = deviceConnection.GetProperty(DeviceProperty.Model, extraProperty, ref device, ref extraData);
                if (result)
                {
                    txt_Model.Text = (string)extraData;
                }
                else
                {
                    MessageBox.Show("Get Model Settings Fail.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_ModelSet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_Model.Text.Trim()))
            {
                MessageBox.Show("Please Input the Model.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_Model.Focus();
                return;
            }

            try
            {
                object extraProperty = new object();
                object extraData = new object();
                extraProperty = Zd2911Utils.DeviceModel;
                extraData = txt_Model.Text.Trim();
                bool result = deviceConnection.SetProperty(DeviceProperty.Model, extraProperty, device, extraData);
                if (result)
                {
                    MessageBox.Show("Set Model Success.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Set Model Fail.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Upload Sound
        private int dataChunk = 0;

        private void btn_UploadSound_Click(object sender, EventArgs e)
        {
            if (CommunicationType.Usb != device.CommunicationType)
            {
                MessageBox.Show("Upload Sound Only Supports USB Communication", "Prompt", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (-1 == cbo_SoundType.SelectedIndex)
            {
                MessageBox.Show("Please Selected Sound Type", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_SoundType.Focus();
                return;
            }

            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "WAV|*.wav";
                if (DialogResult.OK.Equals(open.ShowDialog()))
                {
                    FileStream fs = new FileStream(open.FileName, FileMode.Open);
                    byte[] sound = new byte[fs.Length];
                    fs.Read(sound, 0, (int)fs.Length);
                    fs.Close();

                    if (false == CheckWaveFormat(sound, ref dataChunk))
                    {
                        return;
                    }

                    if (UploadSound(sound))
                    {
                        MessageBox.Show("Upload Sound Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Upload Sound Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Upload Sound Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CheckWaveFormat(byte[] buffer, ref int offSet)
        {
            offSet = 0;
            string riff = Encoding.ASCII.GetString(buffer, 0, 4);
            string wave = Encoding.ASCII.GetString(buffer, 8, 4);
            string fmt = Encoding.ASCII.GetString(buffer, 12, 4);
            if (false == riff.Equals("RIFF") || false == wave.Equals("WAVE") || false == fmt.Equals("fmt "))
            {
                MessageBox.Show("Incorrect Wave Format", "Pormpt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            int waveFormatSize = BitConverter.ToInt32(buffer, 16);
            byte[] waveBytes = new byte[18];
            Array.Copy(buffer, 20, waveBytes, 0, waveBytes.Length);

            UInt16 formatTag = BitConverter.ToUInt16(waveBytes, 0);
            if (1 != formatTag)
            {
                MessageBox.Show("Audio Format is not PCM", "Pormpt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            UInt16 channels = BitConverter.ToUInt16(waveBytes, 2);
            if (1 != channels)
            {
                MessageBox.Show("Channels is not Stereo", "Pormpt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            UInt32 samplesPerSec = BitConverter.ToUInt32(waveBytes, 4);
            if (22050 != samplesPerSec)
            {
                MessageBox.Show("Audio sample rate is not 22.05KHZ", "Pormpt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            UInt16 bitsPerSample = BitConverter.ToUInt16(waveBytes, 14);
            if (16 != bitsPerSample)
            {
                MessageBox.Show("Audio sample size is not 8bit", "Pormpt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            int chunkHeaderLen = 0;
            int tmpOffSet = 20 + waveFormatSize;
            byte[] chunkHeader = Zd2911Utils.CreateChunkHeader(buffer, tmpOffSet);
            string fact = Encoding.ASCII.GetString(chunkHeader, 0, 4);
            int len = BitConverter.ToInt32(chunkHeader, 4);
            if (fact.Equals("fact"))
            {
                offSet = offSet + chunkHeaderLen + len;
            }
            offSet = tmpOffSet;

            chunkHeader = Zd2911Utils.CreateChunkHeader(buffer, offSet);
            string data = Encoding.ASCII.GetString(chunkHeader, 0, 4);
            len = BitConverter.ToInt32(chunkHeader, 4);
            if (false == data.Equals("data"))
            {
                MessageBox.Show("Incorrect Wave Format", "Pormpt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (1024 * 75 < len)
            {
                MessageBox.Show("Data size is too big. Must be less than 75K byte", "Pormpt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool UploadSound(byte[] soundBuffer)
        {
            int chunkHeaderLen = 0;
            byte[] chunkHeader = Zd2911Utils.CreateChunkHeader(soundBuffer, dataChunk);
            int len = BitConverter.ToInt32(chunkHeader, 4);
            int downSize = len + 4;
            byte[] downData = new byte[downSize];
            Array.Copy(chunkHeader, 4, downData, 0, 4);
            Array.Copy(soundBuffer, dataChunk + chunkHeaderLen, downData, 4, len);

            int unit = 1024 * 60, i;
            bool result = false;
            int n = (int)downSize / unit;
            object extraProperty = new object();
            object extraData = new object();

            for (i = 0; i < n; i++)
            {
                byte[] dataBytes = new byte[unit];
                Array.Copy(downData, i * unit, dataBytes, 0, unit);
                List<int> soundParam = new List<int>();
                soundParam.Add(cbo_SoundType.SelectedIndex + 8);
                soundParam.Add(i * unit);
                extraProperty = soundParam;
                extraData = dataBytes;
                result = deviceConnection.SetProperty(DeviceProperty.UploadSound, extraProperty, device, extraData);
                if (false == result)
                {
                    return false;
                }
            }

            n = downSize % unit;
            if (n > 0)
            {
                byte[] dataBytes = new byte[n];
                Array.Copy(downData, i * unit, dataBytes, 0, n);
                List<int> soundParam = new List<int>();
                soundParam.Add(cbo_SoundType.SelectedIndex + 8);
                soundParam.Add(i * unit);
                extraProperty = soundParam;
                extraData = dataBytes;
                result = deviceConnection.SetProperty(DeviceProperty.UploadSound, extraProperty, device, extraData);
                if (false == result)
                {
                    return false;
                }
            }

            return result;
        }
        #endregion

        private void restartButton_Click(object sender, EventArgs e)
        {
            try
            {
                object extraProperty = new object();
                object extraData = new object();
                byte[] paramIndex = BitConverter.GetBytes(1);
                byte[] paramValue = BitConverter.GetBytes(2);   //restart
                byte[] powerOff = new byte[paramIndex.Length + paramValue.Length];
                Array.Copy(paramIndex, 0, powerOff, 0, paramIndex.Length);
                Array.Copy(paramValue, 0, powerOff, paramIndex.Length, paramValue.Length);
                extraData = powerOff;
                bool result = deviceConnection.SetProperty(DeviceProperty.PowerOff, extraProperty, device, extraData);

                MessageBox.Show("Restart Cmd action need to reconnect the device!", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
