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
namespace BacklogService
{
    public partial class BacklogService : ServiceBase
    {
        System.Timers.Timer timer = new System.Timers.Timer();
       // List<User> list = new List<User>();
        public BacklogService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            DateTime start = DateTime.Now;
            int minute = start.Minute;
            timer.Elapsed += new ElapsedEventHandler(Mess);
            timer.Interval = 5 * 60 * 1000;
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
                var Id = (from sc in db.Companies select sc.CompanyId).ToList();//公司Id列表
                foreach (var item in Id)///每个公司
                {
                    SqlConnection cnn = new SqlConnection();
                    cnn.ConnectionString = "Data Source = 211.149.199.42; uid =sa ; pwd =admin123@; database = "+"Bonsaii"+item;
                    cnn.Open();
                    SqlCommand queryCommand = new SqlCommand();
                    queryCommand.CommandType = CommandType.Text;
                    queryCommand.CommandText = "SELECT * FROM [dbo].[Backlog]";
                    //queryCommand.Parameters.AddWithValue("@ID", id);
                    queryCommand.Connection = cnn;
                    SqlDataReader reader = queryCommand.ExecuteReader();//执行SQL，返回一个“流”

                    while (reader.Read())
                    {
                        User user = new User();

                        user.Id = (int)reader["Id"];
                        user.AcciName = reader["AcciName"].ToString();
                        user.MessTitle = reader["MessTitle"].ToString();
                        user.MessContent = reader["MessContent"].ToString();
                        user.Recipient = reader["Recipient"].ToString();
                        user.Name = reader["Name"].ToString();
                        user.TelNum = reader["TelNum"].ToString();
                        user.EmailAddr = reader["EmailAddr"].ToString();
                       // user.Type = (Boolean)reader["Type"];
                        user.SendMess = (Boolean)reader["SendMess"];
                        user.Email = (Boolean)reader["Email"];
                        user.Cycle = (byte)reader["Cycle"];
                        if (user.Cycle == 0)
                        {
                            user.StartTime = dt;
                            user.RemindTime = ts;
                            user.QuitTime = dt;
                            if (reader["OnlyOneDate"] != System.DBNull.Value)
                            {
                                user.OnlyOneDate = (DateTime)reader["OnlyOneDate"];
                                // Console.WriteLine(reader["OnlyOneDate"]);
                            }
                            else
                                user.OnlyOneDate = null;
                           // user.OnlyOneDate = (DateTime)reader["OnlyOneDate"];
                        }
                        else
                        {
                            user.OnlyOneDate = dt;//(DateTime)reader["OnlyOneDate"];
                            //user.StartTime = (DateTime)(reader["StartDate"]);
                            if ((reader["StartTime"]) != System.DBNull.Value)
                            {
                                user.StartTime = (DateTime)(reader["StartTime"]);
                            }
                            else
                            {
                                user.StartTime = null;
                            }
                            if (reader["RemindTime"] != System.DBNull.Value)
                            {
                                user.RemindTime = (TimeSpan)(reader["RemindTime"]);
                            }
                            else
                            {
                                user.RemindTime = null;
                            }
                            if (reader["QuitTime"] != System.DBNull.Value)
                            {
                                user.QuitTime = (DateTime)(reader["QuitTime"]);
                            }
                            else
                            {
                                user.RemindTime = null;
                            }
                            //user.RemindTime = (TimeSpan)(reader["RemindDate"]);
                            //user.QuitTime = (DateTime)reader["EndDate"];
                        }

                        user.IsUse = (Boolean)reader["IsUse"];

                        list.Add(user);
                    }

                    foreach (User user in list)///某一个公司的某一个代办事项
                    {
                        ///判断该代办事项是否启用
                        if (user.IsUse == true)
                        {
                            //String[] subCont = user.MessContent.Split(new char[] { ',' });
                            //if (subCont.Contains(user.Email.ToString()))
                            //{
                                ///发送邮件
                                if (user.Email) {
                                    ///如果函数返回的结果是true，表示邮件发送成功
                                    ///我们需要在systemMessage表中记录信息;这个表是给管理员看的
                                    byte status = ToEmail(user);
                                    ///找到该公司的所有管理员，形成一个列表
                                    if (status == 1 || status == 0)
                                    {
                                        ///遍历添加
                                        List<string> companyAdminList = (from p in db.Users where p.IsRoot == true && p.CompanyId == item select p.UserName).ToList();
                                        foreach (var ItemList in companyAdminList)
                                        {
                                            SystemMessage systemMessage = new SystemMessage();
                                            systemMessage.IsRead = false;
                                            systemMessage.MessBody = user.MessContent;
                                            systemMessage.MessTime = DateTime.Now;
                                            systemMessage.UserName = ItemList;
                                            systemMessage.MessTitle = user.MessTitle;
                                            systemMessage.CompanyId = item;
                                            systemMessage.SendStatus = status;
                                            systemMessage.MessReceiver =  user.Recipient;
                                            systemMessage.MessType = "待办事项(邮件)";
                                            db.SystemMessages.Add(systemMessage);
                                        }
                                        db.SaveChanges();
                                    }
                                }
                                ///发送消息到App
                                if (user.SendMess) 
                                {
                                    byte statusApp= ToApp(user);  
                                    ///找到该公司的所有管理员，形成一个列表
                                    if (statusApp == 1 || statusApp == 0)
                                    {
                                        ///遍历添加
                                        List<string> companyAdminList = (from p in db.Users where p.IsRoot == true && p.CompanyId == item select p.UserName).ToList();
                                        foreach (var ItemList in companyAdminList)
                                        {
                                            SystemMessage systemMessage = new SystemMessage();
                                            systemMessage.IsRead = false;
                                            systemMessage.MessBody = user.MessContent;
                                            systemMessage.MessTime = DateTime.Now;
                                            systemMessage.UserName = ItemList;
                                            systemMessage.MessTitle = user.MessTitle;
                                            systemMessage.CompanyId = item;
                                            systemMessage.SendStatus = statusApp; 
                                            systemMessage.MessReceiver = user.Recipient;
                                            systemMessage.MessType = "待办事项(App)";
                                            db.SystemMessages.Add(systemMessage);
                                        }
                                        db.SaveChanges();
                                    }

                                }
                                    

                            //}
                        }
                    }
                    reader.Dispose();
                    cnn.Close();
                    cnn.Dispose();
                }
            }
            catch (InvalidCastException ex)
            {
                throw ex;
                //Console.WriteLine(ex.ToString());
            }
      
        }
        public byte ToEmail(User user)
        {
            //smtp.163.com
            String senderServerIp = "smtp.qq.com";
            String toMailAddress = null;
            String fromMailAddress = "1271808441@qq.com";
            String subjectInfo = null;
            String bodyInfo = null;
            String mailUsername = "1271808441";
            String mailPassword = "smtpmima123"; //发送邮箱的密码（）
            String mailPort = "25";

            DateTime currentTime = System.DateTime.Now;
            ///如果循环方式是1,表示每天都要发送。
            ///按照以下方式发送
            if (user.Cycle==1)
            {
                ///如果当前时间在时间段内
                ///继续执行
                if (currentTime.CompareTo(user.StartTime) >= 0 && currentTime.CompareTo(user.QuitTime) <= 0)
                {
                    ///如果当前小时和预定小时相等并且当前分钟和预定分钟相等
                    if ((currentTime.Hour == user.OnlyOneDate.Value.Hour) && (currentTime.Minute == user.RemindTime.Value.Minutes))
                    {
                        toMailAddress = user.EmailAddr;
                        subjectInfo = user.MessContent;
                        bodyInfo = user.MessTitle;
                        MyEmail email = new MyEmail(senderServerIp, toMailAddress, fromMailAddress, subjectInfo, bodyInfo, mailUsername, mailPassword, mailPort, false, false);
                        try
                        {
                            ///发送邮件成功;返回1
                            email.Send(); return 1;
                        }
                        catch
                        {
                            ///失败返回0
                            return 0;
                        }
                    }
                    else
                    {
                        return 3;
                    }
                }
                ///如果如果当前小时和预定小时相等并且当前分钟和预定分钟相等的条件不成立
                ///返回3
                else 
                {
                    return 3;
                }
            }
            ///如果循环方式是0,表示只发送一次
            ///按照以下方式发送
            else
            {
                 ////如果当前时/分/秒都和预设的时/分/秒相等
                if ((currentTime.Date == user.OnlyOneDate.Value.Date)&& (currentTime.Hour == user.OnlyOneDate.Value.Hour) && (currentTime.Minute == user.OnlyOneDate.Value.Minute))
                {
                    toMailAddress = user.EmailAddr;
                    subjectInfo = user.MessContent;
                    bodyInfo = user.MessTitle;
                    MyEmail email = new MyEmail(senderServerIp, toMailAddress, fromMailAddress, subjectInfo, bodyInfo, mailUsername, mailPassword, mailPort, false, false);
                    try
                    {
                        ///发送邮件成功;返回true
                        email.Send(); 
                        return 1;
                    }
                    catch
                    {
                        ///失败返回false
                        return 0;
                    }
                }
                else 
                {
                    return 3;
                }
            }
        }
        public byte ToApp(User user)
        {
            String title = user.MessTitle;
           // String alert = user.MessageAlert;
            String msg_content = user.MessContent;//消息内容

            String app_key = "9c9efd93135700dfcc3c0556";
            String master_secret = "868a28e1c7b5586f99726ad5";
            Console.WriteLine("*****开始发送******");

            DateTime currentTime = System.DateTime.Now;
            JPushClient client = null;
            ///如果循环方式是1,表示每天都要发送。
            ///按照以下方式发送
            if (user.Cycle==1)
            {
                ///如果当前时间在时间段内
                ///继续执行
                if (currentTime.CompareTo(user.StartTime) >= 0 && currentTime.CompareTo(user.QuitTime) <= 0)
                {
                    ///如果当前小时和预定小时相等并且当前分钟和预定分钟相等
                    if ((currentTime.Hour == user.OnlyOneDate.Value.Hour) && (currentTime.Minute == user.RemindTime.Value.Minutes))
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
                            return 1;
                        }
                        catch (APIRequestException ex)
                        {
                            Console.WriteLine("Error response from JPush server. Should review and fix it. ");
                            Console.WriteLine("HTTP Status: " + ex.Status);
                            Console.WriteLine("Error Code: " + ex.ErrorCode);
                            Console.WriteLine("Error Message: " + ex.ErrorCode);
                            return 0;
                        }
                        catch (APIConnectionException ex)
                        {
                            Console.WriteLine(ex.Message);
                            return 0;
                        }
                    }
                    ///如果当前小时和预定小时相等并且当前分钟和预定分钟相等的条件不成立
                    ///返回3
                    else
                    {
                        return 3;
                    }
                }
                else 
                {
                    return 3;
                    ///返回3
                }
            }   
            ///如果循环方式是0,表示只发送一次
            ///按照以下方式发送
            else
            {
                ////如果当前时/分/秒都和预设的时/分/秒相等
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
                        return 1;

                    }
                    catch (APIRequestException ex)
                    {
                        Console.WriteLine("Error response from JPush server. Should review and fix it. ");
                        Console.WriteLine("HTTP Status: " + ex.Status);
                        Console.WriteLine("Error Code: " + ex.ErrorCode);
                        Console.WriteLine("Error Message: " + ex.ErrorCode);
                        return 0;
                    }
                    catch (APIConnectionException ex)
                    {
                        Console.WriteLine(ex.Message);
                        return 0;
                    }
                }
                ////如果当前时/分/秒都和预设的时/分/秒相等的条件不成立
                else 
                {
                    return 3;
                }
            }
            Console.WriteLine("*****结束发送******");
        }
        ////public static PushPayload PushObject_All_All_Alert(User user)//要推送的内容
        ////{
        ////    PushPayload pushPayload = new PushPayload();
        ////    pushPayload.platform = Platform.all();//平台信息
        ////    pushPayload.audience = Audience.all();//推送设备对象，表示一条推送可以被推送到那些设备，确认推送设备的对象;推送所有目标
        ////   // pushPayload.notification = new Notification().setAlert(user.MessageAlert);
        ////    return pushPayload;
        ////}
        ////public static PushPayload PushObject_all_alias_alert(User user)
        ////{

        ////    PushPayload pushPayload = new PushPayload();
        ////    pushPayload.platform = Platform.android();
        ////    pushPayload.audience = Audience.s_alias("alias1");
        ////   // pushPayload.notification = new Notification().setAlert(user.MessageAlert);
        ////    return pushPayload;

        ////}
        ////public static PushPayload PushObject_Android_Tag_AlertWithTitle(User user)
        ////{
        ////    PushPayload pushPayload = new PushPayload();

        ////    pushPayload.platform = Platform.android();
        ////    // pushPayload.audience = Audience.all();
        ////    pushPayload.audience = Audience.s_tag("0000000008");
        ////   // pushPayload.notification = Notification.android(user.MessageAlert, user.MessTitle);

        ////    return pushPayload;
        ////}
        ////public static PushPayload PushObject_android_and_ios(User user)
        ////{
        ////    PushPayload pushPayload = new PushPayload();
        ////    pushPayload.platform = Platform.android_ios();
        ////    var audience = Audience.s_tag("tag1");
        ////    pushPayload.audience = audience;
        ////    var notification = new Notification().setAlert("alert content");
        ////    notification.AndroidNotification = new AndroidNotification().setTitle("Android Title");
        ////    notification.IosNotification = new IosNotification();
        ////    notification.IosNotification.incrBadge(1);
        ////    notification.IosNotification.AddExtra("extra_key", "extra_value");

        ////    pushPayload.notification = notification.Check();


        ////    return pushPayload;
        ////}

        /// <summary>
        /// 发送消息到app的函数
        /// 发送消息的内容:MessContent
        /// 收消息的用户:
        /// 接收消息的设备：ios/Android
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static PushPayload PushObject_ios_tagAnd_alertWithExtrasAndMessage(User user)
        {
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.android_ios();
            pushPayload.audience = Audience.all();
            var notification = new Notification();
            pushPayload.notification = notification;
            pushPayload.message = Message.content(user.MessContent);
            return pushPayload;

        }
        ////public static PushPayload PushObject_ios_audienceMore_messageWithExtras(User user)
        ////{

        ////    var pushPayload = new PushPayload();
        ////    pushPayload.platform = Platform.android_ios();
        ////    pushPayload.audience = Audience.s_tag("tag1", "tag2");
        ////    pushPayload.message = Message.content(user.MessContent).AddExtras("from", "JPush");
        ////    return pushPayload;

        ////}

        
        protected override void OnStop()
        {
              this.timer.Enabled = false;
        }
    }
}
