using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using cn.jpush.api;
using cn.jpush.api.push.mode;
using cn.jpush.api.common;
using cn.jpush.api.common.resp;

namespace Bonsaii.Controllers
{
    public class JpushController : Controller
    {
        // GET: Jpush
        public ActionResult Index()
        {
            return View();
        }

        //public static String TITLE = "Panda Test from C# v3 sdk";
       // public static String ALERT = "Panda Test from  C# v3 sdk - alert ";
      //  public static String MSG_CONTENT = "Panda Test from C# v3 sdk - msgContent";//消息内容
      //  public static String REGISTRATION_ID = "0900e8d85ef";
        public static String TAG = "tag_api";//标签
        public static String app_key = "9c9efd93135700dfcc3c0556";//毛毛
        public static String master_secret = "868a28e1c7b5586f99726ad5";//毛毛
        public static PushPayload PushObject_Android_Tag_AlertWithTitle(String Title, String Alert, String Msg_content, String Recevier)
        {
            //寻找注册Id
            //string[] a ={"xxxxx1","xxxxxx2","xxxxxx3"};
            //发送消息
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.android();
           pushPayload.audience = Audience.all();
           // pushPayload.audience = Audience.s_registrationId(Recevier);
            //pushPayload.audience = Audience.s_tag();
            pushPayload.notification = Notification.android(Alert, Title);//发送alert和title

            pushPayload.message = Message.content(Msg_content);//发送content
           
            return pushPayload;
        }

        public JsonResult JpushTest(String Title, String Alert, String Msg_content,String Recevier)//, String ALERT, String MSG_CONTENT)
        {
            JPushClient client = new JPushClient(app_key, master_secret);
            PushPayload payload = PushObject_Android_Tag_AlertWithTitle(Title, Alert,Msg_content,Recevier);//, MSG_CONTENT);//选择一种方式
            try
            {
                var result = client.SendPush(payload);//推送
                //由于统计数据并非非是即时的,所以等待一小段时间再执行下面的获取结果方法
                System.Threading.Thread.Sleep(10000);
                /*如需查询上次推送结果执行下面的代码*/
                var apiResultv3 = client.getReceivedApi_v3(result.msg_id.ToString());
                /*如需查询某个messageid的推送结果执行下面的代码*/
               // var querResultWithV3 = client.getReceivedApi_v3("1739302794");
                
                return Json(result,JsonRequestBehavior.AllowGet);

            }
            catch (APIRequestException e)//处理请求异常
            {
                var message = "Error response from JPush server. Should review and fix it." + "HTTP Status:" + e.Status + "Error Code: " + e.ErrorCode+"Error Message: " + e.ErrorCode;
                return Json( message,JsonRequestBehavior.AllowGet);
            }
            catch (APIConnectionException e)//处理连接异常
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}