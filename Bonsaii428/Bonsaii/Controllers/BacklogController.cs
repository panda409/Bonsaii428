using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using BonsaiiModels;
using BonsaiiModels.Staffs;

namespace Bonsaii.Controllers
{
    public class BacklogController : BaseController
    {

        // GET: Backlog
        public ActionResult Index()
        {
            return View(db.Backlogs.ToList().OrderByDescending(c=>c.Id));
        }

        // GET: Backlog/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Backlog backlog = db.Backlogs.Find(id);
            if (backlog == null)
            {
                return HttpNotFound();
            }
            return View(backlog);
        }

        // GET: Backlog/Create
        public ActionResult Create()
        {
            //List<SelectListItem> staff = (from s in db.Staffs
            //                              join d in db.Departments on s.Department equals d.DepartmentId
            //                              select new
            //                              {
            //                                  StaffNumber = s.StaffNumber,
            //                                  StaffName = s.Name,
            //                                  StaffEmail = s.Email,
            //                                  StaffPhone = s.IndividualTelNumber//手机号码
            //                                  // StaffDepartment = d.Name
            //                                  //StaffPosition = s.Position
            //                              }).ToList().Select(s => new SelectListItem
            //                              {
            //                                  Text = s.StaffNumber + "-" + s.StaffName,
            //                                  Value = s.StaffNumber + "-" + s.StaffName + "<" + s.StaffPhone + "-" + s.StaffEmail + ">"//s.StaffNumber + "-" + s.StaffName 
            //                              }).ToList();
            //List<SelectListItem> group = (from d in db.Departments
            //                              join s in db.Staffs on d.DepartmentId equals s.Department
            //                              select new
            //                              {
            //                                  StaffDepartment = d.Name,
            //                                  Staff1=staff
            //                              }).ToList().Select(d => new SelectListItem
            //                              {
            //                                  Text = d.StaffDepartment,
            //                                  Value = d.StaffDepartment
            //                              }).ToList();
            // var staff = (from s in db.Staffs group s by s.Department into g select g).ToList();
            //var staff = (from s in db.Staffs
            //             where s.AuditStatus == 3 && s.ArchiveTag == false
            //             select new StaffModel
            //             {
            //                 department = s.Department,
            //                 text = s.StaffNumber + "-" + s.Name,
            //                 value = s.StaffNumber + "-" + s.Name + "<" + s.IndividualTelNumber + "-" + s.Email + ">"
            //             }).ToList();
           // var user = (from p in db.Users where )
          
            var staff = (from s in db.Staffs
                         where s.AuditStatus == 3 && s.ArchiveTag == false
                         select new StaffModel
                         {
                             department = s.Department,
                             text = s.StaffNumber + "-" + s.Name,
                             value = s.StaffNumber + "-" + s.Name + "<" + s.IndividualTelNumber + "-" + s.Email + ">"
                         }).ToList();
            var group = (from d in db.Departments
                         select new StaffModel{ department = d.DepartmentId, name = d.Name }).ToList();
          
            Dictionary<string, int> sum = new Dictionary<string, int>();
            foreach(var g in group)
            {
                int count=0;
                foreach(var s in staff)
                {
                    if (s.department == g.department)
                        count++;                   
                }
                sum.Add(g.department, count); 
            }
         
            ViewBag.Count = sum;
            ViewBag.Receiver = staff;
            ViewBag.Group = group;
            List<SelectListItem> circulateMethod = new List<SelectListItem>();
            SelectListItem a1 = new SelectListItem
            {
                Text = "仅一次",
                Value = "0"
            };
            SelectListItem a2 = new SelectListItem
            {
                Text = "每天",
                Value = "1"
            };
          
            circulateMethod.Add(a1);
            circulateMethod.Add(a2);
           

            ViewBag.CirculateMethod = circulateMethod;

            return View();
        }

        // POST: Backlog/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Backlog backlog)
        {
            var staff = (from s in db.Staffs
                         select new StaffModel
                         {
                             department = s.Department,
                             text = s.StaffNumber + "-" + s.Name,
                             value = s.StaffNumber + "-" + s.Name + "<" + s.IndividualTelNumber + "-" + s.Email + ">"
                         }).ToList();
            ViewBag.Receiver = staff;
            List<SelectListItem> circulateMethod = new List<SelectListItem>();
            SelectListItem a1 = new SelectListItem
            {
                Text = "仅一次",
                Value = "0"
            };
            SelectListItem a2 = new SelectListItem
            {
                Text = "每天",
                Value = "1"
            };
          
            circulateMethod.Add(a1);
            circulateMethod.Add(a2);
          

            ViewBag.CirculateMethod = circulateMethod;
            var group = (from d in db.Departments
                         select new StaffModel { department = d.DepartmentId, name = d.Name }).ToList();

            Dictionary<string, int> sum = new Dictionary<string, int>();
            foreach (var g in group)
            {
                int count = 0;
                foreach (var s in staff)
                {
                    if (s.department == g.department)
                        count++;
                }
                sum.Add(g.department, count);
            }

            ViewBag.Count = sum;
            ViewBag.Receiver = staff;
            ViewBag.Group = group;
            if (backlog.Cycle == 0)
            {
                backlog.StartTime = null;
                backlog.QuitTime = null;

            }
            if (backlog.Cycle == 1)
            {
                backlog.OnlyOneDate = null;

            }


            if (ModelState.IsValid)
            {
                //backlog中的电子邮箱和手机号码
                // backlog.TelNum = backlog.Recipient; 
                string[] sArray = backlog.Recipient.Split(new char[] { ',' });
                foreach (var itemsArray in sArray)
                {
                    string[] temp = itemsArray.Split(new char[3] { '-', '<', '>' });
                    backlog.TelNum += temp[2];
                    backlog.TelNum += ",";
                    backlog.EmailAddr += temp[3];
                    backlog.EmailAddr += ",";
                }
                string tempAddr =  backlog.EmailAddr;
                backlog.EmailAddr = tempAddr.TrimEnd(",".ToCharArray());
                string tempTelNum = backlog.TelNum;
                backlog.TelNum = tempTelNum.TrimEnd(",".ToCharArray());
                //ViewBag.sInfoArray = sArray;
                //backlog.EmailAddr = ;
                db.Backlogs.Add(backlog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(backlog);
        }

        // GET: Backlog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Backlog backlog = db.Backlogs.Find(id);
            if (backlog == null)
            {
                return HttpNotFound();
            }
            List<SelectListItem> circulateMethod = new List<SelectListItem>();
            SelectListItem a1 = new SelectListItem
            {
                Text = "仅一次",
                Value = "0"
            };
            SelectListItem a2 = new SelectListItem
            {
                Text = "每天",
                Value = "1"
            };

            circulateMethod.Add(a1);
            circulateMethod.Add(a2);

            var staff = (from s in db.Staffs
                         select new StaffModel
                         {
                             department = s.Department,
                             text = s.StaffNumber + "-" + s.Name,
                             value = s.StaffNumber + "-" + s.Name + "<" + s.IndividualTelNumber + "-" + s.Email + ">"
                         }).ToList();

            var group = (from d in db.Departments
                         select new StaffModel { department = d.DepartmentId, name = d.Name }).ToList();

            Dictionary<string, int> sum = new Dictionary<string, int>();
            foreach (var g in group)
            {
                int count = 0;
                foreach (var s in staff)
                {
                    if (s.department == g.department)
                        count++;
                }
                sum.Add(g.department, count);
            }

            ViewBag.Count = sum;
            ViewBag.Receiver = staff;
            ViewBag.Group = group;
            ViewBag.CirculateMethod = circulateMethod;

            return View(backlog);
        }

        // POST: Backlog/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Backlog backlog)
        {
            if (ModelState.IsValid)
            {
                Backlog afterEdit = db.Backlogs.Find(backlog.Id);
                afterEdit.AcciName = backlog.AcciName;
                afterEdit.Cycle = backlog.Cycle;
                afterEdit.Email = backlog.Email;
                afterEdit.IsUse = backlog.IsUse;
                afterEdit.MessContent = backlog.MessContent;
                afterEdit.MessTitle = backlog.MessTitle;
                afterEdit.Name = backlog.Name;
                afterEdit.OnlyOneDate = backlog.OnlyOneDate;
                afterEdit.QuitTime = backlog.QuitTime;
                afterEdit.Recipient = backlog.Recipient;
                afterEdit.RemindTime = backlog.RemindTime;
                afterEdit.SendMess = backlog.SendMess;
                afterEdit.StartTime = backlog.StartTime;
                afterEdit.Type = backlog.Type;
               // db.Entry(afterEdit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<SelectListItem> circulateMethod = new List<SelectListItem>();
            SelectListItem a1 = new SelectListItem
            {
                Text = "仅一次",
                Value = "0"
            };
            SelectListItem a2 = new SelectListItem
            {
                Text = "每天",
                Value = "1"
            };

            circulateMethod.Add(a1);
            circulateMethod.Add(a2);

            var staff = (from s in db.Staffs
                         select new StaffModel
                         {
                             department = s.Department,
                             text = s.StaffNumber + "-" + s.Name,
                             value = s.StaffNumber + "-" + s.Name + "<" + s.IndividualTelNumber + "-" + s.Email + ">"
                         }).ToList();

            var group = (from d in db.Departments
                         select new StaffModel { department = d.DepartmentId, name = d.Name }).ToList();

            Dictionary<string, int> sum = new Dictionary<string, int>();
            foreach (var g in group)
            {
                int count = 0;
                foreach (var s in staff)
                {
                    if (s.department == g.department)
                        count++;
                }
                sum.Add(g.department, count);
            }

            ViewBag.Count = sum;
            ViewBag.Receiver = staff;
            ViewBag.Group = group;

            ViewBag.CirculateMethod = circulateMethod;
            return View(backlog);
        }

        // GET: Backlog/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Backlog backlog = db.Backlogs.Find(id);
            if (backlog == null)
            {
                return HttpNotFound();
            }
            return View(backlog);
        }

        // POST: Backlog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Backlog backlog = db.Backlogs.Find(id);
            db.Backlogs.Remove(backlog);
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
