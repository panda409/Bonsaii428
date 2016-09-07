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
using cn.jpush.api.push.mode;
using JpushApiExample;
using cn.jpush.api;
using cn.jpush.api.common;
using cn.jpush.api.common.resp;
using System.IO;
using BonsaiiModels.Staffs;

namespace Bonsaii.Controllers
{
    public class CompanyPushController : BaseController
    {
        private string Url_Url = "http://192.168.0.19:8888/files/push/";//推送主题图片，存储的地址。
        //private BonsaiiDbContext db = new BonsaiiDbContext();
        private SystemDbContext sdb = new SystemDbContext();
        // GET: /CompanyPush/
        public ActionResult Index()
        {
            var list = (from c in db.CompanyPushes orderby c.RecordTime descending select c).ToList();//list
            return View(list);
        }

        // GET: /CompanyPush/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyPush companypush = db.CompanyPushes.Find(id);
            if (companypush == null)
            {
                return HttpNotFound();
            }
            return View(companypush);
        }

        // GET: /CompanyPush/Create
        public ActionResult Create()
        {
            var staff = (from s in db.Staffs
                         select new StaffModel
                         {
                             department = s.Department,
                             text = s.StaffNumber + "-" + s.Name,
                             value = s.StaffNumber
                         }).ToList();

            var group = (from d in db.Departments
                         select new StaffModel { department = d.DepartmentId, name = d.Name }).ToList();
            var number = (from s in db.Staffs group s by s.Department into g select new { department = g.Key, number = g.Count() }).ToList();
            ViewBag.Receiver = staff;
            ViewBag.Group = group;
            List<SelectListItem> strict = (from u in db.StaffParams//获取所有地区
                                           where u.StaffParamTypeId==13
                                           select new { Strict = u.Value }).Distinct().ToList().Select(s => new SelectListItem
                                           {
                                               Text = s.Strict,
                                               Value = s.Strict
                                           }).Distinct().ToList();
            List<SelectListItem> department = (from u in db.Departments//获取所有部门
                                               select new { Department = u.Name }).Distinct().ToList().Select(s => new SelectListItem
                                           {
                                               Text = s.Department,
                                               Value = s.Department
                                           }).Distinct().ToList();
            ViewBag.strict = strict;
            ViewBag.department = department;
            return View();
        }

        // POST: /CompanyPush/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateInput(false)]// [ValidateAntiForgeryToken]
        public ActionResult Create(CompanyPush companypush,HttpPostedFileBase file)
        {
            var staff = (from s in db.Staffs
                         select new StaffModel
                         {
                             department = s.Department,
                             text = s.StaffNumber + "-" + s.Name,
                             value = s.StaffNumber
                         }).ToList();

            var group = (from d in db.Departments
                         select new StaffModel { department = d.DepartmentId, name = d.Name }).ToList();
            var number = (from s in db.Staffs group s by s.Department into g select new { department = g.Key, number = g.Count() }).ToList();
            ViewBag.Receiver = staff;
            ViewBag.Group = group;
            List<SelectListItem> strict = (from u in db.StaffParams//获取所有地区
                                           where u.StaffParamTypeId == 13
                                           select new { Strict = u.Value }).Distinct().ToList().Select(s => new SelectListItem
                                           {
                                               Text = s.Strict,
                                               Value = s.Strict
                                           }).Distinct().ToList();
            List<SelectListItem> department = (from u in db.Departments//获取所有部门
                                               select new { Department = u.Name }).Distinct().ToList().Select(s => new SelectListItem
                                               {
                                                   Text = s.Department,
                                                   Value = s.Department
                                               }).Distinct().ToList();
            ViewBag.strict = strict;
            ViewBag.department = department;
            if (ModelState.IsValid)
            {
                string miniType = file.ContentType;
                Stream fileStream = file.InputStream;
                string path = AppDomain.CurrentDomain.BaseDirectory + "files\\push\\";
                string filename = Path.GetFileName(file.FileName);
                file.SaveAs(Path.Combine(path, filename));
                CompanyPush p = new CompanyPush();
                p.Type = Request["Type"];
                p.Tag = Request["Tag"].TrimEnd(',');
                p.TagContent = companypush.TagContent;
                p.TagTitle = companypush.TagTitle;
                p.Target = companypush.Target;
                p.PersonName = this.Name;
                p.RecordPerson = this.UserName;
                p.RecordTime = DateTime.Now;
                p.Url = "http://" + Request.Url.Host + ":" + Request.Url.Port + "/files/push/" +filename;
               
                string[] selects = p.Tag.Split(',');
                PushPayload payload = new PushPayload();
                if (p.Target.Equals("所有用户"))
                {
                    payload = CompanyPushAPI.PushObject_all(this.CompanyId, p.TagContent);//选择一种方式
                }
                else if (p.Target.Equals("选择用户(只要符合其中一种条件的推送)"))
                {
                    payload = CompanyPushAPI.PushObject_tags(this.CompanyId,selects, p.TagContent);//选择一种方式
                }
                else if (p.Target.Equals("指定用户"))
                {
                    string[] person = new string[selects.Length];
                    for (int i = 0; i < selects.Length; i++)
                    {
                        string temp = selects[i];
                        UserModels user = (from u in sdb.Users where u.CompanyId == this.CompanyId && u.StaffNumber == temp select u).FirstOrDefault();
                        if (user != null)
                        {
                            person[i] = user.UserName;
                        }
                    }
                    payload = CompanyPushAPI.PushObject_alias(person, p.TagContent);//选择一种方式
                }
                else
                {
                    payload = CompanyPushAPI.PushObject_tags_and(this.CompanyId, selects, p.TagContent);//选择一种方式
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
                   // var querResultWithV3 = client.getReceivedApi_v3("1739302794");

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
                db.CompanyPushes.Add(p);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(companypush);
        }

        public ActionResult CreateEvery()
        {
            var staff = (from s in db.Staffs
                         select new StaffModel
                         {
                             department = s.Department,
                             text = s.StaffNumber + "-" + s.Name,
                             value = s.StaffNumber
                         }).ToList();

            var group = (from d in db.Departments
                         select new StaffModel { department = d.DepartmentId, name = d.Name }).ToList();
            var number = (from s in db.Staffs group s by s.Department into g select new {department=g.Key,number=g.Count() }).ToList();
            ViewBag.Receiver = staff;
            ViewBag.Group = group;
            return View();
        }

        // POST: /CompanyPush/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateInput(false)]// [ValidateAntiForgeryToken]
        public ActionResult CreateEvery(CompanyPush companypush, HttpPostedFileBase file)
        {
            var staff = (from s in db.Staffs
                         select new StaffModel
                         {
                             department = s.Department,
                             text = s.StaffNumber + "-" + s.Name,
                             value = s.StaffNumber
                         }).ToList();

            var group = (from d in db.Departments
                         select new StaffModel { department = d.DepartmentId, name = d.Name }).ToList();
            var number = (from s in db.Staffs group s by s.Department into g select new { department = g.Key, number = g.Count() }).ToList();
            ViewBag.Receiver = staff;
            ViewBag.Group = group;
            if (ModelState.IsValid)
            {
                string miniType = file.ContentType;
                Stream fileStream = file.InputStream;
                string path = AppDomain.CurrentDomain.BaseDirectory + "files\\push\\";
                string filename = Path.GetFileName(file.FileName);
                file.SaveAs(Path.Combine(path, filename));
                CompanyPush p = new CompanyPush();
                p.Type = Request["Type"];
                p.Tag = Request["Tag"];
                p.TagContent = companypush.TagContent;
                p.TagTitle = companypush.TagTitle;
                p.Target = companypush.Target;
                p.PersonName = this.Name;
                p.RecordPerson = this.UserName;
                p.RecordTime = DateTime.Now;
                p.Url = Url_Url + filename;
                db.CompanyPushes.Add(p);
                db.SaveChanges();
                string[] selects = p.Tag.Split(',');//取出推送的员工的工号
                string[] person=new string[selects.Length];
                for (int i = 0; i < selects.Length; i++)
                {
                    string temp = selects[i];
                    UserModels user = (from u in sdb.Users where u.CompanyId == this.CompanyId && u.StaffNumber == temp select u).FirstOrDefault();
                    if (user != null)
                    {
                        person[i] = user.UserName;
                    }
                }
                PushPayload payload = new PushPayload();
               
                    payload = CompanyPushAPI.PushObject_alias(person, p.TagContent);//选择一种方式
                
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
                    // var querResultWithV3 = client.getReceivedApi_v3("1739302794");

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
                return RedirectToAction("Index");
            }

            return View(companypush);
        }
        // GET: /CompanyPush/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyPush companypush = db.CompanyPushes.Find(id);
            if (companypush == null)
            {
                return HttpNotFound();
            }
            return View(companypush);
        }

        // POST: /CompanyPush/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Target,TagContent,Tag,RecordTime,RecordPerson,PersonName,TagTitle")] CompanyPush companypush)
        {
            if (ModelState.IsValid)
            {
                db.Entry(companypush).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(companypush);
        }

        // GET: /CompanyPush/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyPush companypush = db.CompanyPushes.Find(id);
            if (companypush == null)
            {
                return HttpNotFound();
            }
            return View(companypush);
        }

        // POST: /CompanyPush/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CompanyPush companypush = db.CompanyPushes.Find(id);
            db.CompanyPushes.Remove(companypush);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
