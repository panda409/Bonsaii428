using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ZDC2911Demo.Business;
using ZDC2911Demo.IConvert;
using ZDC2911Demo.Entity;
using Riss.Devices;

namespace ZDC2911Demo.UI {
    public partial class SLogForm : Form {
        private Device device;
        private DeviceConnection deviceConnection;

        public SLogForm(DeviceCommEty deviceEty) {
            InitializeComponent();
            device = deviceEty.Device;
            deviceConnection = deviceEty.DeviceConnection;
        }

        private List<DateTime> GetDateTimeList() {
            List<DateTime> dtList = new List<DateTime>();
            dtList.Add(dtp_Begin.Value);
            dtList.Add(dtp_End.Value);
            return dtList;
        }

        private void AddRecordToListView(List<Record> recordList) {
            lvw_SLogList.Items.Clear();
            int no = 1;
            foreach (Record record in recordList) {
                string operation = ConvertObject.SLogType(record.Action);
                ListViewItem item = new ListViewItem(new string[]{ no.ToString(), record.MDIN.ToString(), record.DIN.ToString(),
                    operation,record.Verify.ToString(), record.Clock.ToString("yyyy-MM-dd HH:mm:ss") });
                lvw_SLogList.Items.Add(item);
                no++;
            }
        }

        private void SLogForm_Load(object sender, EventArgs e) {
            dtp_Begin.MinDate = InitData.MinDateTime;
            dtp_Begin.MaxDate = InitData.MaxDateTime;
            dtp_Begin.Value = InitData.MinDateTime;
            dtp_End.MinDate = InitData.MinDateTime;
            dtp_End.MaxDate = InitData.MaxDateTime;
            dtp_End.Value = InitData.MaxDateTime;
        }

        private void btn_DownloadNew_Click(object sender, EventArgs e) {
            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;

            try {
                List<DateTime> dtList = GetDateTimeList();
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);                
                extraProperty = true;                
                extraData = dtList;
                result = deviceConnection.GetProperty(DeviceProperty.ManagementRecordsCount, extraProperty, ref device,
                    ref extraData);
                if (false == result) {
                    MessageBox.Show("Get New Slog Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int recordCount = (int)extraData;
                if (0 == recordCount) {//为0时说明没有新日志记录
                    lvw_SLogList.Items.Clear();
                    return;
                }

                List<bool> boolList = new List<bool>();
                boolList.Add(true);//新日志，该参数为true时，第二个参数清除新日志标记值为true或false时都将强制清除新日志标记
                boolList.Add(chk_NewFlag.Checked);//清除新日志标记
                extraProperty = boolList;
                extraData = dtList;
                result = deviceConnection.GetProperty(DeviceProperty.ManagementRecords, extraProperty, ref device, ref extraData);
                if (result) {
                    List<Record> recordList = (List<Record>)extraData;
                    AddRecordToListView(recordList);
                } else {
                    MessageBox.Show("Get New Slog Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                extraData = Global.DeviceIdle;
                deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
            }
        }

        private void btn_DownloadHistory_Click(object sender, EventArgs e) {
            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;

            try {
                List<DateTime> dtList = GetDateTimeList();
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                extraProperty = false;                
                extraData = dtList;
                result = deviceConnection.GetProperty(DeviceProperty.ManagementRecordsCount, extraProperty, ref device,
                    ref extraData);
                if (false == result) {
                    MessageBox.Show("Get All Slog Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int recordCount = (int)extraData;
                if (0 == recordCount) {//为0时说明没有新日志记录
                    lvw_SLogList.Items.Clear();
                    return;
                }

                List<bool> boolList = new List<bool>();
                boolList.Add(false);//所有日志
                boolList.Add(chk_NewFlag.Checked);//清除新日志标记，false则不清除
                extraProperty = boolList;
                extraData = dtList;
                result = deviceConnection.GetProperty(DeviceProperty.ManagementRecords, extraProperty, ref device, ref extraData);
                if (result) {
                    List<Record> recordList = (List<Record>)extraData;
                    AddRecordToListView(recordList);
                } else {
                    MessageBox.Show("Get All Slog Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                extraData = Global.DeviceIdle;
                deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
            }
        }

        private void btn_Clear_Click(object sender, EventArgs e) {
            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;

            try {
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                result = deviceConnection.SetProperty(DeviceProperty.ManagementRecords, extraProperty, device, extraData);
                if (result) {
                    lvw_SLogList.Items.Clear();
                    MessageBox.Show("Clear All Slog Success", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);                    
                } else {
                    MessageBox.Show("Clear All Slog Fail", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                extraData = Global.DeviceIdle;
                deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
            }
        }
    }
}
