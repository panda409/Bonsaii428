using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using Bonsaii.Models.Audit;
using System.Collections;


namespace Bonsaii.Controllers
{
    public class AuditApplicationController : BaseController
    {
        // GET: AuditApplication
        public ActionResult Index()
        {
            List<AuditApplication> list = (from p in db.AuditApplications where p.Creator == this.UserName select p).ToList();
               
            //List<AuditApplication> list = db.AuditApplications.ToList();
            foreach (AuditApplication tmp in list)
            {
                tmp.TemplateName = db.AuditTemplates.Find(tmp.TId).Name;
                tmp.StateDescription = db.States.Find(tmp.State).Description;
            }
            return View(list.OrderByDescending(c=>c.Id));
        }
        /// <summary>
        /// info内容已经转换成为string[]
        /// 现在需要把string[]转换成一段html代码
        /// </summary>
        /// <param name="Array"></param>
        /// <returns></returns>
        public static string ConvertDataTableToHtml(string[] arrays)
        {
            // "<table>";
            string html = null;
            foreach (string array in arrays)
            {
                html += "<tr>";
                string[] arrayColumns = array.Split(new char[] { '：' });
                foreach (var arrayColumn in arrayColumns)
                {
                    html += "<td>" + arrayColumn + "</td>";
                }
                html += "</tr>";
            }
            return html;
        }
        // GET: AuditApplication/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditApplication auditApplication = db.AuditApplications.Find(id);
            if (auditApplication == null)
            {
                return HttpNotFound();
            }
            string[] sArray = auditApplication.Info.Split(new char[] { ';' });
            ///string转html有必要吗
            int len = sArray.Length;
            ArrayList newList = new ArrayList(sArray);
            newList.RemoveAt(len - 1);
            sArray = (string[])newList.ToArray(typeof(string));
            ViewBag.sInfoArray = ConvertDataTableToHtml(sArray);
            //ViewBag.sInfoArray = sArray;

            auditApplication.StateDescription = db.States.Find(auditApplication.State).Description;
            db.SaveChanges();

            //根据AuditApplication的Id寻找AuditProcess,然后寻找结果列表
            var resultList = (from p in db.AuditProcesses where p.AId == id orderby p.SId select p).ToList();
            ViewBag.resultlist = resultList;//存放结果列表
            foreach (var resultListItem in resultList) {
                resultListItem.ResultDescription = db.States.Find(resultListItem.Result).Description;
            }

            return View(auditApplication);
        }

        public ActionResult Submit(int id)
        {
            AuditApplication auditApplication = db.AuditApplications.Find(id);
            if (auditApplication == null)
            {
                return HttpNotFound();
            }

            //标记该application的状态为等待审核
            auditApplication.State = 1;

            AuditTemplate template = db.AuditTemplates.Find(auditApplication.TId);
            //找到Template下的第一个Step节点
            AuditStep step = db.AuditSteps.Find(template.FirstStepSId);
            AuditProcess auditProcess = new AuditProcess();
            auditProcess.AId = auditApplication.Id;
            auditProcess.SId = step.SId;
            auditProcess.TId = step.TId;
            auditProcess.CreateDate = DateTime.Now;
            auditProcess.Result = 1;
            auditProcess.AuditDate = DateTime.Now.AddDays(step.Days);//记录一下该节点最晚的审核时间；
            auditProcess.Approver = step.Approver;
            db.AuditProcesses.Add(auditProcess);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: AuditApplication/Create
        public ActionResult Create()
        {
            //实现下拉列表
            List<SelectListItem> itemTId = db.AuditTemplates.ToList().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),//保存的值
                Text = c.Name//显示的值
            }).ToList();

            //传值到页面
            ViewBag.TIdList = itemTId;
            return View();
        }

        // POST: AuditApplication/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AuditApplication auditApplication)
        {
            if (ModelState.IsValid)
            {
                auditApplication.CreateDate = DateTime.Now;
                auditApplication.Creator = this.UserName;
                auditApplication.State = 0 ;
                db.AuditApplications.Add(auditApplication);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(auditApplication);
        }

        // GET: AuditApplication/Edit/5
        public ActionResult Edit(int? id)
        {
            List<SelectListItem> itemTId = db.AuditTemplates.ToList().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),//保存的值
                Text = c.Name//显示的值
            }).ToList();

            //传值到页面
            ViewBag.TIdList = itemTId;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditApplication auditApplication = db.AuditApplications.Find(id);
            if (auditApplication == null)
            {
                return HttpNotFound();
            }
            return View(auditApplication);
        }

        // POST: AuditApplication/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AuditApplication auditApplication)
        {
            if (ModelState.IsValid)
            {
                db.Entry(auditApplication).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(auditApplication);
        }

        // GET: AuditApplication/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditApplication auditApplication = db.AuditApplications.Find(id);
            if (auditApplication == null)
            {
                return HttpNotFound();
            }
            auditApplication.StateDescription = db.States.Find(auditApplication.State).Description;
           // List<AuditApplication> list = db.AuditApplications.ToList();
            //foreach (AuditApplication tmp in list)
            //{
            //    tmp.TemplateName = db.AuditTemplates.Find(tmp.TId).Name;
            //    tmp.StateDescription = db.States.Find(tmp.State).Description;
            //}

            var auditProcesses = (from p in db.AuditProcesses where p.AId == auditApplication.Id select p).ToList();
            if (auditProcesses.Count != 0) {
                ViewBag.errorMessage = true;
                return View(auditApplication);
            
            }
            
            return View(auditApplication);
        }

        // POST: AuditApplication/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AuditApplication auditApplication = db.AuditApplications.Find(id);
            db.AuditApplications.Remove(auditApplication);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //Ystep节点是Json数据
        [HttpPost]
        public JsonResult YSteps(int? id)
        {
            try
            {   
                var items = (from p in db.AuditProcesses where p.AId == id orderby p.SId
                             join q in db.AuditSteps on p.TId equals q.TId
                    
                    //(from p in db.AuditSteps
                  //           where application.TId == p.TId
                  // join q in db.AuditProcesses on id  equals q.AId
                                 //join q in db.AuditProcesses on p.SId equals q.SId
                 
                    //(from p in db.AuditProcesses join q in db.AuditSteps on p.SId equals q.SId
                            // where p.TId == id
                             select new
                             {
                                 title = q.Name,
                                 content = p.AuditPerson + p.Comment + p.Result + p.AuditDate,
                                 id = p.Result

                             }).ToList();
                return Json(items);
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }


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
