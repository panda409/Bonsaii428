using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Bonsaii.Models;
using System.Text.RegularExpressions;
using System.Timers;
using cn.jpush.api;
using cn.jpush.api.push;
using cn.jpush.api.report;
using cn.jpush.api.common;
using cn.jpush.api.util;
using cn.jpush.api.push.mode;
using cn.jpush.api.push.notification;
using cn.jpush.api.common.resp;
using BonsaiiModels;


namespace WindowsService
{
    public partial class MessageService : ServiceBase
    {
       System.Timers.Timer timer = new System.Timers.Timer();

       //List<User> list = new List<User>();

        public MessageService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
           
                DateTime start = DateTime.Now;
                int minute = start.Minute;
                timer.Elapsed += new ElapsedEventHandler(Mess);
                timer.Interval =  5*60*1000;
                timer.Enabled = true;
        }

        private void Mess(object sender, ElapsedEventArgs e)
        {
            try
            {
                DateTime dt = new DateTime();
                TimeSpan ts = new TimeSpan();
                List<User> list = new List<User>();

                SystemDbContext db = new SystemDbContext("Data Source = 211.149.199.42,1433;Initial Catalog = BonsaiiSystem;User ID = sa;Password = admin123@;");
                var Id = db.Subscribe_Companies.ToList();
                // where sc.Id = id
                //select new {sc.SubScribeId,sc.CompanyId};


                foreach (var item in Id)
                {
                    //查找
                    SubscribeList subscribe = (from p in db.SubscribeLists where p.Id == item.SubScribeId select p).SingleOrDefault();

                    SqlConnection cnn = new SqlConnection();
                    cnn.ConnectionString = "Data Source = 211.149.199.42; uid =sa ; pwd =admin123@; database = " + "Bonsaii" + item.CompanyId;
                    cnn.Open();

                    SqlCommand query = new SqlCommand();
                    //query.CommandType = CommandType.Text;
                    //query.CommandText = subscribe.SQL;
                    //query.Connection = cnn;
                    //SqlDataReader dr = query.ExecuteReader();
                    //string messcontent = "";
                    //while (dr.Read())
                    //{
                    //    Console.Write(dr.GetInt32(0).ToString());
                    //    messcontent = messcontent + dr.GetInt32(0).ToString();
                    //}
                    //subscribe.MessContent = messcontent;
                    SqlDataAdapter da = new SqlDataAdapter(subscribe.SQL, cnn);
                    string tableName = "Result";
                    DataSet ds = new DataSet();
                    da.Fill(ds, tableName);

                    DataTable myTable = ds.Tables[tableName];
                    string result = ConvertDataTableToHtml(myTable);
                    //foreach (DataRow myRow in myTable.Rows)
                    //{
                    //    foreach (DataColumn myColumn in myTable.Columns)
                    //    {
                    //        Console.Write(myRow[myColumn]);	//遍历表中的每个单元格
                    //    }
                    //}

                    subscribe.MessContent = result;
                    da.Dispose();
                    cnn.Close();
                    cnn.Dispose();


                    SqlConnection cnn1 = new SqlConnection();
                    cnn1.ConnectionString = "Data Source = 211.149.199.42; uid =sa ; pwd =admin123@; database = " + "Bonsaii" + item.CompanyId;
                    cnn1.Open();

                    SqlCommand queryCommand = new SqlCommand();
                    queryCommand.CommandType = CommandType.Text;
                    queryCommand.CommandText = "SELECT * FROM [dbo].[SubscribeAndWarning]";
                    //queryCommand.Parameters.AddWithValue("@ID", id);
                    queryCommand.Connection = cnn1;
                    SqlDataReader reader = queryCommand.ExecuteReader();//执行SQL，返回一个“流
                    while (reader.Read())
                    {
                        User user = new User();

                        user.Id = (int)reader["Id"];
                        user.EventName = reader["EventName"].ToString();
                        //user.MessageTitle = reader["MessageTitle"].ToString();
                        //user.MessageBody = reader["MessageBody"].ToString();
                        //user.MessageAlert = reader["MessageAlert"].ToString();
                        user.Receiver = reader["Receiver"].ToString();
                        user.ReceiverTel = reader["ReceiverTel"].ToString();
                        user.ReceiverEmail = reader["ReceiverEmail"].ToString();
                        user.ReceiverType = reader["ReceiverType"].ToString();
                        user.SendToApp = (Boolean)reader["SendToApp"];
                        user.IsEmail = (Boolean)reader["IsEmail"];
                        user.CirculateMethod = (byte)reader["CirculateMethod"];

                        if (user.CirculateMethod == 0)
                        {

                            user.StartDate = dt;
                            user.RemindDate = ts;//Convert.ToDateTime(reader["RemindDate"]);
                            user.EndDate = dt;//(DateTime)reader["EndDate"];
                            if (reader["OnlyOneDate"] != System.DBNull.Value)
                            {
                                user.OnlyOneDate = (DateTime)reader["OnlyOneDate"];
                                // Console.WriteLine(reader["OnlyOneDate"]);
                            }
                            else
                                user.OnlyOneDate = null;
                        }
                        else
                        {
                            user.OnlyOneDate = dt;//(DateTime)reader["OnlyOneDate"];
                            if ((reader["StartDate"]) != System.DBNull.Value)
                            {
                                user.StartDate = (DateTime)(reader["StartDate"]);
                            }
                            else
                            {
                                user.StartDate = null;
                            }

                            if (reader["RemindDate"] != System.DBNull.Value)
                            {
                                user.RemindDate = (TimeSpan)(reader["RemindDate"]);
                            }
                            else
                            {
                                user.RemindDate = null;
                            }
                            // user.CirculateMethod = (Boolean)reader["CirculateMethod"];
                            if (reader["EndDate"] != System.DBNull.Value)
                            {
                                user.EndDate = (DateTime)reader["EndDate"];
                            }
                            else
                            {
                                user.EndDate = null;
                            }
                        }
                        //user.SQL = reader["IsEmail"].ToString();
                        //user.IsSQLLegal = (Boolean)reader["IsSQLLegal"];
                        user.SubscribeContent = (String)reader["SubscribeContent"];
                        user.IsAvailable = (Boolean)reader["IsAvailable"];
                        list.Add(user);

                    }

                    foreach (User user in list)
                    {
                        if (user.IsAvailable == true)
                        {

                            String[] subCont = user.SubscribeContent.Split(new char[] { ',' });
                            if (subCont.Contains(item.SubScribeId.ToString()))
                            {

                                if (user.IsEmail)
                                {
                                    ToEmail(user, subscribe);
                                }
                                if (user.SendToApp)
                                {
                                    ToApp(user, subscribe);
                                }
                            }
                        }
                    }

                    reader.Dispose();
                    cnn.Close();
                    cnn.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public static string ConvertDataTableToHtml(DataTable dt)
        {
            string html = "<table>";

            html += "<tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<td>" + dt.Columns[i].ColumnName + "</td>";
            html += "</tr>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }
        private static void ToEmail(User user, SubscribeList subscribeList)
        {
            //smtp.163.com
            String senderServerIp = "smtp.qq.com";//"smtp.163.com";
            String toMailAddress = null;
            String fromMailAddress = "1271808441@qq.com";//"yuanbing_IT@163.com";
            String subjectInfo = null;
            String bodyInfo = null;
            String mailUsername = "1271808441";//"yuanbing_IT";
            String mailPassword = "smtpmima123";//"yuanbing123456"; //发送邮箱的密码（）
            String mailPort = "25";

            DateTime currentTime = System.DateTime.Now;

            if (user.CirculateMethod == 1)
            {
                if (currentTime.CompareTo(user.StartDate) >= 0 && currentTime.CompareTo(user.EndDate) <= 0)
                {
                    if ((currentTime.Hour == user.OnlyOneDate.Value.Hour) && (currentTime.Minute == user.RemindDate.Value.Minutes))
                    {
                        toMailAddress = user.ReceiverEmail;
                        subjectInfo = subscribeList.MessTitle;
                        bodyInfo = subscribeList.MessContent;
                        //            //subjectInfo = user.MessageTitle;
                        //            //bodyInfo = user.MessageBody;
                        MyEmail email = new MyEmail(senderServerIp, toMailAddress, fromMailAddress, subjectInfo, bodyInfo, mailUsername, mailPassword, mailPort, false, false);
                        email.Send();
                    }
                }
            }
            else
            {
                if (currentTime != null)
                {
                    if ((currentTime.Date == user.OnlyOneDate.Value.Date) && (currentTime.Hour == user.OnlyOneDate.Value.Hour) && (currentTime.Minute == user.OnlyOneDate.Value.Minute))
                    {
                        toMailAddress = user.ReceiverEmail;
                        subjectInfo = subscribeList.MessTitle;
                        bodyInfo = subscribeList.MessContent;
                        //subjectInfo = user.MessageTitle;
                        //bodyInfo = user.MessageBody;
                        MyEmail email = new MyEmail(senderServerIp, toMailAddress, fromMailAddress, subjectInfo, bodyInfo, mailUsername, mailPassword, mailPort, false, false);
                        email.Send();
                    }
                }
            }
        }
        private static void ToApp(User user, SubscribeList subscribeList)
        {
            String title = user.MessageTitle;
            String alert = user.MessageAlert;
            String msg_content = user.MessageBody;//消息内容

            String app_key = "9c9efd93135700dfcc3c0556";
            String master_secret = "868a28e1c7b5586f99726ad5";
            Console.WriteLine("*****开始发送******");

            DateTime currentTime = System.DateTime.Now;
            JPushClient client = null;

            if (user.CirculateMethod == 1)
                if (currentTime.CompareTo(user.StartDate) >= 0 && currentTime.CompareTo(user.EndDate) <= 0)
                {
                    if ((currentTime.Hour == user.OnlyOneDate.Value.Hour) && (currentTime.Minute == user.RemindDate.Value.Minutes))
                    {
                        client = new JPushClient(app_key, master_secret);
                        PushPayload payload = PushObject_ios_tagAnd_alertWithExtrasAndMessage(user);//选择一种方式
                        try
                        {
                            var result = client.SendPush(payload);//推送
                            //由于统计数据并非非是即时的,所以等待一小段时间再执行下面的获取结果方法
                            System.Threading.Thread.Sleep(10000);
                            /*如需查询上次推送结果执行下面的代码*/
                            // var apiResult = client.getReceivedApi(result.msg_id.ToString());
                            var apiResultv3 = client.getReceivedApi_v3(result.msg_id.ToString());
                            /*如需查询某个messageid的推送结果执行下面的代码*/
                            //   var queryResultWithV2 = client.getReceivedApi("1739302794"); 
                            var querResultWithV3 = client.getReceivedApi_v3("1739302794");

                        }
                        catch (APIRequestException ex)
                        {
                            Console.WriteLine("Error response from JPush server. Should review and fix it. ");
                            Console.WriteLine("HTTP Status: " + ex.Status);
                            Console.WriteLine("Error Code: " + ex.ErrorCode);
                            Console.WriteLine("Error Message: " + ex.ErrorCode);
                        }
                        catch (APIConnectionException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    if ((currentTime.Date == user.OnlyOneDate.Value.Date) && (currentTime.Hour == user.OnlyOneDate.Value.Hour) && (currentTime.Minute == user.OnlyOneDate.Value.Minute))
                    {
                        client = new JPushClient(app_key, master_secret);
                        PushPayload payload = PushObject_ios_tagAnd_alertWithExtrasAndMessage(user);//选择一种方式
                        try
                        {
                            var result = client.SendPush(payload);//推送
                            //由于统计数据并非非是即时的,所以等待一小段时间再执行下面的获取结果方法
                            System.Threading.Thread.Sleep(10000);
                            /*如需查询上次推送结果执行下面的代码*/
                            // var apiResult = client.getReceivedApi(result.msg_id.ToString());
                            var apiResultv3 = client.getReceivedApi_v3(result.msg_id.ToString());
                            /*如需查询某个messageid的推送结果执行下面的代码*/
                            //   var queryResultWithV2 = client.getReceivedApi("1739302794"); 
                            var querResultWithV3 = client.getReceivedApi_v3("1739302794");

                        }
                        catch (APIRequestException ex)
                        {
                            Console.WriteLine("Error response from JPush server. Should review and fix it. ");
                            Console.WriteLine("HTTP Status: " + ex.Status);
                            Console.WriteLine("Error Code: " + ex.ErrorCode);
                            Console.WriteLine("Error Message: " + ex.ErrorCode);
                        }
                        catch (APIConnectionException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            Console.WriteLine("*****结束发送******");
        }
        public static PushPayload PushObject_All_All_Alert(User user)//要推送的内容
        {
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.all();//平台信息
            pushPayload.audience = Audience.all();//推送设备对象，表示一条推送可以被推送到那些设备，确认推送设备的对象;推送所有目标
            pushPayload.notification = new Notification().setAlert(user.MessageAlert);
            return pushPayload;
        }
        public static PushPayload PushObject_all_alias_alert(User user)
        {

            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.android();
            pushPayload.audience = Audience.s_alias("alias1");
            pushPayload.notification = new Notification().setAlert(user.MessageAlert);
            return pushPayload;

        }
        public static PushPayload PushObject_Android_Tag_AlertWithTitle(User user)
        {
            PushPayload pushPayload = new PushPayload();

            pushPayload.platform = Platform.android();
            // pushPayload.audience = Audience.all();
            pushPayload.audience = Audience.s_tag("0000000008");
            pushPayload.notification = Notification.android(user.MessageAlert, user.MessageTitle);

            return pushPayload;
        }
        public static PushPayload PushObject_android_and_ios(User user)
        {
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.android_ios();
            var audience = Audience.s_tag("tag1");
            pushPayload.audience = audience;
            var notification = new Notification().setAlert("alert content");
            notification.AndroidNotification = new AndroidNotification().setTitle("Android Title");
            notification.IosNotification = new IosNotification();
            notification.IosNotification.incrBadge(1);
            notification.IosNotification.AddExtra("extra_key", "extra_value");

            pushPayload.notification = notification.Check();


            return pushPayload;
        }
        public static PushPayload PushObject_ios_tagAnd_alertWithExtrasAndMessage(User user)
        {
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.android_ios();
            // pushPayload.audience = Audience.s_tag_and("tag1", "tag_all");
            pushPayload.audience = Audience.all();
            var notification = new Notification();
            notification.IosNotification = new IosNotification().setAlert(user.MessageAlert).setBadge(5).setSound("happy").AddExtra("from", "JPush");

            pushPayload.notification = notification;
            pushPayload.message = Message.content(user.MessageBody);
            return pushPayload;

        }
        public static PushPayload PushObject_ios_audienceMore_messageWithExtras(User user)
        {

            var pushPayload = new PushPayload();
            pushPayload.platform = Platform.android_ios();
            pushPayload.audience = Audience.s_tag("tag1", "tag2");
            pushPayload.message = Message.content(user.MessageBody).AddExtras("from", "JPush");
            return pushPayload;

        }


        protected override void OnStop()
        {
            this.timer.Enabled = false;
        }
       
    }
}
