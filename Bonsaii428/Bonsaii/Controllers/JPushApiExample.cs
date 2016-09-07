using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using cn.jpush.api;
using cn.jpush.api.push;
using cn.jpush.api.report;
using cn.jpush.api.common;
using cn.jpush.api.util;
using cn.jpush.api.push.mode;
using cn.jpush.api.push.notification;
using cn.jpush.api.common.resp;
namespace JpushApiExample
{
   public class JPushApiExample
    {
       public static String TITLE = "张量公司办公系统";
        public static String ALERT = "消息";
        public static String MSG_CONTENT = "我有一群好盆友 - msgContent";//消息内容
        public static String REGISTRATION_ID = "0900e8d85ef";
        public static String TAG = "tag_api";//标签
        public static String app_key = "9c9efd93135700dfcc3c0556";
        public static String master_secret = "868a28e1c7b5586f99726ad5";

        static void Main(string[] args)
        {
            Console.WriteLine("*****开始发送******");
            JPushClient client = new JPushClient(app_key, master_secret);
            PushPayload payload = PushObject_Android_Tag_AlertWithTitle();//选择一种方式
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
            catch (APIRequestException e)
            {
                Console.WriteLine("Error response from JPush server. Should review and fix it. ");
                Console.WriteLine("HTTP Status: " + e.Status);
                Console.WriteLine("Error Code: " + e.ErrorCode);
                Console.WriteLine("Error Message: " + e.ErrorCode);
            }
            catch (APIConnectionException e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("*****结束发送******");
        }
        public static PushPayload PushObject_All_All_Alert()//要推送的内容
        {
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.all();//平台信息
            pushPayload.audience = Audience.all();//推送设备对象，表示一条推送可以被推送到那些设备，确认推送设备的对象;推送所有目标
            pushPayload.notification = new Notification().setAlert(ALERT);
            return pushPayload;
        }
        public static PushPayload PushObject_all_alias_alert()
        {

            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.android();
            pushPayload.audience = Audience.s_alias("alias1");
            pushPayload.notification = new Notification().setAlert(ALERT);
            return pushPayload;
           
        }
        public static PushPayload PushObject_Android_Tag_AlertWithTitle()
        {
            PushPayload pushPayload = new PushPayload();

            pushPayload.platform = Platform.android();
           // pushPayload.audience = Audience.all();
            pushPayload.audience = Audience.s_tag_and("0000000008"); 
            pushPayload.notification =  Notification.android(ALERT,TITLE);

            return pushPayload;
        }
        public static PushPayload PushObject_android_and_ios()
        {
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.android_ios();
            var audience = Audience.s_tag("tag1");
            //Audience.s_tag_and("123","123");
            pushPayload.audience = audience;
            var notification = new Notification().setAlert("alert content");
            notification.AndroidNotification = new AndroidNotification().setTitle("Android Title");
            notification.IosNotification = new IosNotification();
            notification.IosNotification.incrBadge(1);
            notification.IosNotification.AddExtra("extra_key", "extra_value");

            pushPayload.notification = notification.Check(); 
      

            return pushPayload;
        }
        public static PushPayload PushObject_ios_tagAnd_alertWithExtrasAndMessage()
        {
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.android_ios();
           // pushPayload.audience = Audience.s_tag_and("tag1", "tag_all");
            pushPayload.audience = Audience.all();
            var notification = new Notification();
            notification.IosNotification = new IosNotification().setAlert(ALERT).setBadge(5).setSound("happy").AddExtra("from","JPush");

            pushPayload.notification = notification;
            pushPayload.message = Message.content(MSG_CONTENT);
            return pushPayload;

        }
        public static PushPayload PushObject_ios_audienceMore_messageWithExtras()
        {
            
            var pushPayload = new PushPayload();
            pushPayload.platform = Platform.android_ios();
            pushPayload.audience = Audience.s_tag("tag1","tag2");
            pushPayload.message = Message.content(MSG_CONTENT).AddExtras("from", "JPush");
            return pushPayload;

        }
        public static PushPayload PushObject_new_user(string userName)
        {

            var pushPayload = new PushPayload();
            pushPayload.platform = Platform.android();
            pushPayload.audience = Audience.s_alias(userName);
            pushPayload.message = Message.content("欢迎来到张量公司办公系统！").AddExtras("Tag", "1");
            pushPayload.notification = Notification.android("欢迎来到张量公司办公系统！", TITLE);
            return pushPayload;

        }
       //推送给只要满足任何一种标签的用户
        public static PushPayload PushObject_tags(string[] selects,string content)
        {

            var pushPayload = new PushPayload();
            pushPayload.platform = Platform.android();
            pushPayload.audience = Audience.s_tag(selects);
            pushPayload.message = Message.content(content).AddExtras("Tag", "1");
            pushPayload.notification =Notification.android(ALERT,TITLE);
            return pushPayload;

        }
        //推送给满足所有标签的用户
        public static PushPayload PushObject_tags_and(string[] selects, string content)
        {

            var pushPayload = new PushPayload();
            pushPayload.platform = Platform.android();
            pushPayload.audience = Audience.s_tag_and(selects);
            pushPayload.message = Message.content(content).AddExtras("Tag", "1");
            pushPayload.notification = Notification.android(ALERT, TITLE);
            return pushPayload;

        }

        //推送给所有用户
        public static PushPayload PushObject_all(string content)
        {

            var pushPayload = new PushPayload();
            pushPayload.platform = Platform.android();
            pushPayload.audience = Audience.all();
            pushPayload.message = Message.content(content).AddExtras("Tag", "1");
            pushPayload.notification = Notification.android(ALERT, TITLE);
            return pushPayload;

        }
        //同时推送指定多类推送目标：在深圳或者广州，并且是 ”女“ “会员
        //Audience audience= Audience.s_tag("广州", "深圳").tag("女"，"会员")；
        public static PushPayload PushObject_tags_tag_and(string[] selects,string[] selects1,string content)
        {

            var pushPayload = new PushPayload();
            pushPayload.platform = Platform.android();
            pushPayload.audience = Audience.s_tag(selects).tag(selects1);
            pushPayload.message = Message.content(content).AddExtras("Tag", "1");
            pushPayload.notification = Notification.android(ALERT, TITLE);
            return pushPayload;

        }

    }
}
