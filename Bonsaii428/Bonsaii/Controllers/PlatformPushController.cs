using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BonsaiiModels;
using Bonsaii.Models;
using cn.jpush.api;
using JpushApiExample;
using cn.jpush.api.push.mode;
using cn.jpush.api.common;
using cn.jpush.api.common.resp;
using System.IO;

namespace Bonsaii.Controllers
{
    public class PlatformPushController : Controller
    {
        private string Url_Url = "http://211.149.199.42:88/files/push/";//"http://192.168.0.19:8888/files/push/";
        private SystemDbContext sdb = new SystemDbContext();
      
        // GET: /PlatformPush/
        public ActionResult Index()
        {
            var list = (from p in sdb.Pushes orderby p.RecordTime descending select p).ToList();
            return View(list);
        }
        public ActionResult Look(int? id)
        {
            return View(sdb.Pushes.Find(id));
        }

        // GET: /PlatformPush/Details/5
        public ActionResult Details(int? id, string connString)
        {
               BonsaiiDbContext bonsaii = new BonsaiiDbContext(connString);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyPush companypush = bonsaii.CompanyPushes.Find(id);
            if (companypush == null)
            {
                return HttpNotFound();
            }
            return View(companypush);
        }

        // GET: /PlatformPush/Create
        public ActionResult Create()
        {
            List<SelectListItem> strict = (from u in sdb.nations//获取所有地区
                                           select new { Strict = u.Value }).Distinct().ToList().Select(s => new SelectListItem
                                           {
                                               Text = s.Strict,
                                               Value = s.Strict
                                           }).Distinct().ToList();
          
            ViewBag.strict = strict;
            return View();
        }

        // POST: /PlatformPush/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateInput(false)]// [ValidateAntiForgeryToken]
        public ActionResult Create(Push push,HttpPostedFileBase file)
        {
            List<SelectListItem> strict = (from u in sdb.nations//获取所有地区
                                           select new { Strict = u.Value }).Distinct().ToList().Select(s => new SelectListItem
                                           {
                                               Text = s.Strict,
                                               Value = s.Strict
                                           }).Distinct().ToList();

            ViewBag.strict = strict;
            if (ModelState.IsValid)
            {  string miniType = file.ContentType;
                Stream fileStream = file.InputStream;
                string path = AppDomain.CurrentDomain.BaseDirectory + "files\\push\\";
                string filename = Path.GetFileName(file.FileName);
                file.SaveAs(Path.Combine(path, filename));
                Push p = new Push();
                p.Type = Request["Type"];
                p.Tag = Request["Tag"];
                p.TagContent = push.TagContent;
                p.TagTitle = push.TagTitle;
                p.Target = push.Target;
                p.PersonName = "张量办公";
                p.RecordPerson = "88888888888";
                p.RecordTime = DateTime.Now;
                p.Url = "http://" + Request.Url.Host + ":" + Request.Url.Port + "/files/push/" + filename;
                
                string[] selects = p.Tag.Split(',');

              


                PushPayload payload = new PushPayload();
                if (p.Target.Equals("所有用户"))
                { 
                    payload = JPushApiExample.PushObject_all(p.TagContent);//选择一种方式
                }
                else if (p.Target.Equals("选择用户(只要符合其中一种条件的推送)"))
                {
                    payload = JPushApiExample.PushObject_tags(selects, p.TagContent);//选择一种方式
                }
                else
                {
                    payload = JPushApiExample.PushObject_tags_and(selects, p.TagContent);//选择一种方式
                }
                JPushClient client = new JPushClient(JPushApiExample.app_key, JPushApiExample.master_secret);
               
                try
                {
                    var result1 = client.SendPush(payload);//推送
                    //由于统计数据并非非是即时的,所以等待一小段时间再执行下面的获取结果方法
                    //System.Threading.Thread.Sleep(10000);
                    /*如需查询上次推送结果执行下面的代码*/
                    // var apiResult = client.getReceivedApi(result.msg_id.ToString());
                    //var apiResultv3 = client.getReceivedApi_v3(result1.msg_id.ToString());
                    /*如需查询某个messageid的推送结果执行下面的代码*/
                    //   var queryResultWithV2 = client.getReceivedApi("1739302794"); 
                    //var querResultWithV3 = client.getReceivedApi_v3("1739302794");

                }
                catch (APIRequestException e)//处理请求异常
                {
                    if (e.ErrorCode.Equals(1011))
                    {
                        var message1 = "您发送的推送目标组合条件不成立";
                        return Json(message1, JsonRequestBehavior.AllowGet);
                    }
                    var message = "Error response from JPush server. Should review and fix it." + "HTTP Status:" + e.Status + "Error Code: " + e.ErrorCode + "Error Message: " + e.ErrorCode;
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                catch (APIConnectionException e)//处理连接异常
                {
                    var message = e.Message;
                    return Json(e.Message, JsonRequestBehavior.AllowGet);
                }
                sdb.Pushes.Add(p);
                sdb.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(push);
        }

        // GET: /PlatformPush/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Push push = sdb.Pushes.Find(id);
            if (push == null)
            {
                return HttpNotFound();
            }
            return View(push);
        }

        // POST: /PlatformPush/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Target,TagContent,Tag,RecordTime,RecordPerson,PersonName,TagTitle")] Push push)
        {
            if (ModelState.IsValid)
            {
                sdb.Entry(push).State = EntityState.Modified;
                sdb.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(push);
        }

        // GET: /PlatformPush/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Push push = sdb.Pushes.Find(id);
            if (push == null)
            {
                return HttpNotFound();
            }
            return View(push);
        }

        // POST: /PlatformPush/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Push push = sdb.Pushes.Find(id);
            sdb.Pushes.Remove(push);
            sdb.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                sdb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
