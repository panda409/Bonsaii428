using System;
using System.IO.Ports;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using Riss.Devices;
using ZDC2911Demo.IConvert;

using System.Data.SqlClient;
using cn.jpush.api;
using cn.jpush.api.push.mode;
using cn.jpush.api.common;
using cn.jpush.api.common.resp;
using System.Data;



namespace ZDC2911Demo.UI {
    public partial class RealTimeLogForm : Form {
        private int no = 1;
        private Zd2911Monitor listener;
        private delegate void AddRecord(ListViewItem lvi);
        public static String app_key = "9c9efd93135700dfcc3c0556";
        public static String master_secret = "868a28e1c7b5586f99726ad5";
        public string conn { get; set; }

        public RealTimeLogForm() {
            InitializeComponent();
            conn = "Data Source = 211.149.199.42,1433;Initial Catalog = Bonsaii0000000008;User ID = sa;Password = admin123@;";
        }

        private string GetLocalIPAddress() {
            IPAddress[] ipAddressList = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in ipAddressList) {
                if (ip.AddressFamily.Equals(AddressFamily.InterNetwork)) {
                    return ip.ToString();
                }
            }

            return string.Empty;
        }

        private void AddRecordToListView(ListViewItem lvi) {
            lvw_Logs.Items.Add(lvi);
        }
        
        private void btn_Listen_Click(object sender, EventArgs e) {
            if (cbo_SerialPort.Enabled && -1 == cbo_SerialPort.SelectedIndex) {
                MessageBox.Show("Please Selected Serail Port", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_SerialPort.Focus();
                return;
            }

            if (cbo_Baudrate.Enabled && -1 == cbo_Baudrate.SelectedIndex) {
                MessageBox.Show("Please Selected Baudrate", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_Baudrate.Focus();
                return;
            }

            try {
                if (btn_Listen.Text.Trim().Equals("Listen")) {
                    Monitor m = new Monitor();
                    if (0 == cbo_Mode.SelectedIndex) {
                        m.UDPAddress = "192.168.0.14";// GetLocalIPAddress();
                        m.UDPPort = (int)nud_Port.Value;
                        m.Mode = 0;
                    } else {
                        m.SerialPort = Convert.ToInt32(cbo_SerialPort.SelectedItem.ToString().Replace("COM", string.Empty));
                        m.SerialBaudRate = Convert.ToInt32(cbo_Baudrate.SelectedItem);
                        m.Mode = 1;
                    }
                    
                    listener = Zd2911Monitor.CreateZd2911Monitor(m);
                    listener.ReceiveHandler += new ReceiveHandler(listener_ReceiveHandler);
                    listener.OpenListen();
                    btn_Listen.Text = "Cancel";
                } else {
                    listener.CloseListen();
                    listener = null;
                    btn_Listen.Text = "Listen";
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listener_ReceiveHandler(object sender, ReceiveEventArg e) {
            Record record = e.record;
            string verify = ConvertObject.IOMode(record.Verify);
            string action = ConvertObject.GLogType(record.Action);
            ListViewItem lvi = new ListViewItem(new string[]{no.ToString(), record.DN.ToString(), record.DIN.ToString(),
                string.Empty, verify, action, record.Clock.ToString("yyyy-MM-dd HH:mm:ss")});

            string sql = "insert into dbo.DeviceOriginalData(UserID,DeviceID,DateTime,Verify,Action,Remark,MDIN,DoorStatus,JobCode,Antipassback) " +
    "values(@UserID,@DeviceID,@DateTime,@Verify,@Action,@Remark,@MDIN,@DoorStatus,@JobCode,@Antipassback)";
            SqlConnection connection = new SqlConnection(conn);
            SqlCommand command = new SqlCommand(sql, connection);
            connection.Open();
            command.Parameters.Add(new SqlParameter("@UserID", record.DIN.ToString()));
            command.Parameters.Add(new SqlParameter("@DeviceID", record.DN));
            command.Parameters.Add(new SqlParameter("@DateTime", record.Clock.ToString("yyyy-MM-dd HH:mm:ss")));
            command.Parameters.Add(new SqlParameter("@Verify", record.Verify));
            command.Parameters.Add(new SqlParameter("@Action", record.Action));
            command.Parameters.Add(new SqlParameter("@Remark", record.Remark));
            command.Parameters.Add(new SqlParameter("@MDIN", record.MDIN.ToString()));
            command.Parameters.Add(new SqlParameter("@DoorStatus", record.DoorStatus));
            command.Parameters.Add(new SqlParameter("@JobCode", record.JobCode));
            command.Parameters.Add(new SqlParameter("@Antipassback", record.Antipassback));
            
            command.ExecuteNonQuery();           
            connection.Close();
            string sql1 = "select * from dbo.Staffs where PhysicalCardNumber='" + record.DIN.ToString() + "'";// + record.DIN.ToString();
           // connection.Open();
            SqlConnection connection1 = new SqlConnection(conn);
            connection1.Open();
            SqlCommand command1 = new SqlCommand(sql1, connection1);
            SqlDataReader dataReader = command1.ExecuteReader();
            string staffNumber = null;
            if (dataReader.Read())
            {
                staffNumber = dataReader["StaffNumber"].ToString();

            }
            else
            {

                return;
            }
            SqlConnection sdb = new SqlConnection("Data Source = 211.149.199.42,1433;Initial Catalog = BonsaiiSystem;User ID = sa;Password = admin123@;");//连接平台数据库

            string conn1 = "Data Source = 211.149.199.42,1433;User ID = sa;Password = admin123@;Initial Catalog = Bonsaii0000000008;";
            string sql2 = "select * from dbo.BindCodes where ConnectionString='" + conn1 + "' and StaffNumber='" + staffNumber + "'";
            dataReader.Close();
            connection1.Close();
            sdb.Open();
            SqlCommand command2 = new SqlCommand(sql2, sdb);
            SqlDataReader dataReader1 = command2.ExecuteReader();
            if (dataReader1.Read())
            {
                JPushClient client = new JPushClient(app_key, master_secret);
                // PushPayload payload = PushObject_all_alias_alert(dataReader1["Phone"].ToString(), record.Clock.ToString("yyyy-MM-dd HH:mm:ss"));//选择一种方式
                PushPayload pushPayload = new PushPayload();
                pushPayload.platform = Platform.android();
                pushPayload.audience = Audience.s_alias(dataReader1["Phone"].ToString());
                pushPayload.message = cn.jpush.api.push.mode.Message.content("hello").AddExtras("Tag", "2").AddExtras("ComapnyTag","3");
                pushPayload.notification = Notification.android("打卡完成！", "在" + record.Clock.ToString("yyyy-MM-dd HH:mm:ss"));
                try
                {
                    var result = client.SendPush(pushPayload);//推送


                }
                catch (APIRequestException eee)
                {
                    Console.WriteLine("Error response from JPush server. Should review and fix it. ");
                    Console.WriteLine("HTTP Status: " + eee.Status);
                    Console.WriteLine("Error Code: " + eee.ErrorCode);
                    Console.WriteLine("Error Message: " + eee.ErrorCode);
                }
                catch (APIConnectionException ee)
                {
                    Console.WriteLine(ee.Message);
                }


            }
            dataReader1.Close();
            sdb.Close();
               
            //connection.Close();
            BeginInvoke(new AddRecord(AddRecordToListView), new object[] { lvi });
            no++;
        }
        //private PushPayload PushObject_all_alias_alert(string name,string date)
        //{

        //    PushPayload pushPayload = new PushPayload();
        //    pushPayload.platform = Platform.android();
        //    pushPayload.audience = Audience.s_alias(name);
        //    pushPayload.notification = Notification.android("打卡完成！","在"+date);
        //    return pushPayload;

        //}
        private void cbo_Mode_SelectedIndexChanged(object sender, EventArgs e) {
            switch (cbo_Mode.SelectedIndex) {
                case 0:
                    nud_Port.Enabled = true;
                    cbo_SerialPort.Enabled = false;
                    cbo_Baudrate.Enabled = false;
                    break;

                case 1:
                    nud_Port.Enabled = false;
                    cbo_SerialPort.Enabled = true;
                    cbo_Baudrate.Enabled = true;
                    cbo_Baudrate.SelectedIndex = 0;
                    string[] serialNames = SerialPort.GetPortNames();
                    if (null != serialNames) {
                        cbo_SerialPort.Items.Clear();
                        foreach (string name in serialNames) {
                            cbo_SerialPort.Items.Add(name);
                        }
                        cbo_SerialPort.SelectedIndex = 0;                        
                    }
                    break;
            }
        }

        private void RealTimeLogForm_Load(object sender, EventArgs e) {
            cbo_Mode.SelectedIndex = 0;
        }
    }
}
