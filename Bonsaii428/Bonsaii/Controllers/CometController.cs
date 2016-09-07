using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Bonsaii.Controllers
{
    /// <summary>
    /// Comet服务器推送控制器(需设置NoAsyncTimeout,防止长时间请求挂起超时错误)
    /// 需要继承自异步的AsyncController
    /// </summary>
    [NoAsyncTimeout,SessionState(SessionStateBehavior.ReadOnly)]
    public class CometController :AsyncController
    {
       /// <summary>
       /// 当异步线程完成时向客户端发送响应
       /// </summary>
       /// <param name="info"></param>
       /// <returns></returns>
        public ActionResult IndexComplated(string info)
        {
            return Json(info,JsonRequestBehavior.AllowGet);
        }
        public void IndexAsync() 
        {
            ///告诉ASP.NET接下来将进行一个异步操作
            AsyncManager.OutstandingOperations.Increment();
            ///保存将要传递给LongPollingCompleted的参数
            AsyncManager.Parameters["info"] = "已更新";
            ///告诉ASP.NET异步操作已完成，进行LongPollingCompleted方法的调用
            AsyncManager.OutstandingOperations.Decrement();
        }
    }
}