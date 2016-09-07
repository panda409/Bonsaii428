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
using BonsaiiModels.Subscribe;
using BonsaiiModels.Staffs;

namespace Bonsaii.Controllers
{
    public class SubscribeAndWarningController : BaseController
    {
        public ActionResult StaffIndex() {
            List<Staff> Staffs = (from p in db.Staffs orderby p.BillNumber where p.ArchiveTag == false select p).ToList();
            //p.JobState != "离职" select p).ToList();
            foreach (Staff tmp in Staffs)
            {
                tmp.DepartmentName = (from p in db.Departments where p.DepartmentId == tmp.Department select p.Name).ToList().FirstOrDefault();
                tmp.AuditStatusName = db.States.Find(tmp.AuditStatus).Description;
            }
            return View(Staffs);//使用ToPagedList方法时，需要引入using PagedList系统集成的分页函数。
        }

        // GET: SubscribeAndWarning/SendToApp
        public ActionResult SendToApp()
        {
            List<SelectListItem> staff = (from s in db.Staffs
                                          join d in db.Departments on s.Department equals d.DepartmentId
                                          select new
                                          {
                                              StaffNumber = s.StaffNumber,
                                              StaffName = s.Name,
                                              StaffDepartment = d.Name,
                                              StaffPosition = s.Position,
                                              HomeTelNumber =  s.HomeTelNumber
                                          }).ToList().Select(s => new SelectListItem
                                          {
                                              Text = s.HomeTelNumber,
                                             // Text = s.StaffNumber + "-" + s.StaffName + "-" + s.StaffDepartment + "-" + s.StaffPosition+"-"+s.HomeTelNumber,
                                              Value = s.HomeTelNumber
                                          }).ToList();
            ViewBag.Receiver = staff;
            return View();
        }

        // POST: SubscribeAndWarning/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendToApp(SubscribeAndWarning subscribeAndWarning)
        {
            List<SelectListItem> staff = (from s in db.Staffs
                                          join d in db.Departments on s.Department equals d.DepartmentId
                                          select new
                                          {
                                              StaffNumber = s.StaffNumber,
                                              StaffName = s.Name,
                                              StaffDepartment = d.Name,
                                              StaffPosition = s.Position,
                                              HomeTelNumber =  s.HomeTelNumber
                                          }).ToList().Select(s => new SelectListItem
                                          {
                                              Text = s.StaffNumber + "-" + s.StaffName + "-" + s.StaffDepartment + "-" + s.StaffPosition + "-" + s.HomeTelNumber,
                                              Value = s.HomeTelNumber
                                          }).ToList();
            ViewBag.Receiver = staff;
            if (ModelState.IsValid)
            {
                subscribeAndWarning.Receiver = Request["Receiver"];
                db.SubscribeAndWarnings.Add(subscribeAndWarning);
                db.SaveChanges();
                //if (subscribeAndWarning.SendToApp == true) {
                //    JpushController Jpush = new JpushController();
                //    Jpush.JpushTest();
                //}
                return RedirectToAction("Index");
            }

            return View(subscribeAndWarning);
        }


        // GET: SubscribeAndWarning
        public ActionResult Index()
        {
            return View(db.SubscribeAndWarnings.ToList());
        }

        // GET: SubscribeAndWarning/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscribeAndWarning subscribeAndWarning = db.SubscribeAndWarnings.Find(id);
            if (subscribeAndWarning == null)
            {
                return HttpNotFound();
            }
            //ViewBag.StartCut = subscribeAndWarning.StartDate.ToString("yyyy-MM-dd");
            return View(subscribeAndWarning);
        }

        // GET: SubscribeAndWarning/Create
        public ActionResult Create()
        {
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
            //SelectListItem a3 = new SelectListItem
            //{
            //    Text = "每周",
            //    Value = "2"
            //};
            //SelectListItem a4 = new SelectListItem
            //{
            //    Text = "每月",
            //    Value = "3"
            //};
            //SelectListItem a5 = new SelectListItem
            //{
            //    Text = "每年",
            //    Value = "4"
            //};
            //SelectListItem a6 = new SelectListItem
            //{
            //    Text = "自定义",
            //    Value = "5"
            //};
            circulateMethod.Add(a1);
            circulateMethod.Add(a2);
            //circulateMethod.Add(a3);
            //circulateMethod.Add(a4);
            //circulateMethod.Add(a5);
            //circulateMethod.Add(a6);

            ViewBag.CirculateMethod = circulateMethod;

            //SubScribeList怎么来？SystemDb里面
            SystemDbContext systemdb = new SystemDbContext();

            List<SelectListItem> subScribeList = (from p in systemdb.SubscribeLists
                                                  where p.IsAvailable == true
                                                  select new
                                                    {
                                                        SubscribeName = p.SubscribeName,
                                                        SubscribeId = p.Id

                                                    }).ToList().Select(s => new SelectListItem
                                                      {
                                                          Text = s.SubscribeName,
                                                          Value = s.SubscribeId.ToString()
                                                      }).ToList();
            ViewBag.SubScribeList = subScribeList;

            return View();
        }

        // POST: SubscribeAndWarning/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SubscribeAndWarning subscribeAndWarning)
        {
            List<SelectListItem> staff = (from s in db.Staffs
                                          join d in db.Departments on s.Department equals d.DepartmentId
                                          select new
                                          {
                                              StaffNumber = s.StaffNumber,
                                              StaffName = s.Name,
                                              StaffEmail = s.Email,
                                              StaffPhone = s.IndividualTelNumber//手机号码
                                              //    StaffDepartment = d.Name,
                                              // StaffPosition = s.Position
                                          }).ToList().Select(s => new SelectListItem
                                          {
                                              Text = s.StaffNumber + "-" + s.StaffName,
                                              Value = s.StaffNumber + "-" + s.StaffName + "<" + s.StaffPhone + "-" + s.StaffEmail + ">"// backlog.Recipient;
                                              //Text = s.StaffNumber + "-" + s.StaffName + "-" + s.StaffDepartment + "-" + s.StaffPosition,
                                              //Value = s.StaffNumber + "-" + s.StaffName + "-" + s.StaffDepartment + "-" + s.StaffPosition
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
            //SelectListItem a3 = new SelectListItem
            //{
            //    Text = "每周",
            //    Value = "2"
            //};
            //SelectListItem a4 = new SelectListItem
            //{
            //    Text = "每月",
            //    Value = "3"
            //};
            //SelectListItem a5 = new SelectListItem
            //{
            //    Text = "每年",
            //    Value = "4"
            //};
            //SelectListItem a6 = new SelectListItem
            //{
            //    Text = "自定义",
            //    Value = "5"
            //};
            circulateMethod.Add(a1);
            circulateMethod.Add(a2);
            //circulateMethod.Add(a3);
            //circulateMethod.Add(a4);
            //circulateMethod.Add(a5);
            //circulateMethod.Add(a6);

            ViewBag.CirculateMethod = circulateMethod;

            //SubScribeList怎么来？SystemDb里面
            SystemDbContext systemdb = new SystemDbContext();
            List<SelectListItem> subScribeList = (from p in systemdb.SubscribeLists
                                                  where p.IsAvailable == true
                                                  select new
                                                  {
                                                      SubscribeName = p.SubscribeName,
                                                      SubscribeId = p.Id

                                                  }).ToList().Select(s => new SelectListItem
                                                  {
                                                      Text = s.SubscribeName,
                                                      Value = s.SubscribeId.ToString()
                                                  }).ToList();
            ViewBag.SubScribeList = subScribeList;

            if (subscribeAndWarning.CirculateMethod ==0)
            {
                subscribeAndWarning.StartDate = null;
                subscribeAndWarning.EndDate = null;

            }
            if (subscribeAndWarning.CirculateMethod == 1) {
                subscribeAndWarning.OnlyOneDate = null;
            
            }
            if (ModelState.IsValid)
            {
                //backlog中的电子邮箱和手机号码
                // backlog.TelNum = backlog.Recipient; 
                string[] sArray = subscribeAndWarning.Receiver.Split(new char[] { ',' });
                foreach (var itemsArray in sArray)
                {
                    string[] temp = itemsArray.Split(new char[3] { '-', '<', '>' });
                    subscribeAndWarning.ReceiverTel += temp[2];
                    subscribeAndWarning.ReceiverTel += ",";
                    subscribeAndWarning.ReceiverEmail += temp[3];
                    subscribeAndWarning.ReceiverEmail += ",";
                }
                //subscribeAndWarning.Receiver = Request["Receiver"];
                db.SubscribeAndWarnings.Add(subscribeAndWarning);
                db.SaveChanges();
                //if (subscribeAndWarning.SendToApp == true) {
                //    JpushController Jpush = new JpushController();
                //    Jpush.JpushTest();
                //}
                return RedirectToAction("Index");
            }
           
            return View(subscribeAndWarning);
        }

        // GET: SubscribeAndWarning/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscribeAndWarning subscribeAndWarning = db.SubscribeAndWarnings.Find(id);
            if (subscribeAndWarning == null)
            {
                return HttpNotFound();
            }
            List<SelectListItem> staff = (from s in db.Staffs
                                          join d in db.Departments on s.Department equals d.DepartmentId
                                          select new
                                          {
                                              StaffNumber = s.StaffNumber,
                                              StaffName = s.Name,
                                              StaffDepartment = d.Name,
                                              StaffPosition = s.Position
                                          }).ToList().Select(s => new SelectListItem
                                          {
                                              Text = s.StaffNumber + "-" + s.StaffName + "-" + s.StaffDepartment + "-" + s.StaffPosition,
                                              Value = s.StaffNumber + "-" + s.StaffName + "-" + s.StaffDepartment + "-" + s.StaffPosition
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
            //SubScribeList怎么来？SystemDb里面
            SystemDbContext systemdb = new SystemDbContext();

            List<SelectListItem> subScribeList = (from p in systemdb.SubscribeLists
                                                  where p.IsAvailable == true
                                                  select new
                                                  {
                                                      SubscribeName = p.SubscribeName,
                                                      SubscribeId = p.Id

                                                  }).ToList().Select(s => new SelectListItem
                                                  {
                                                      Text = s.SubscribeName,
                                                      Value = s.SubscribeId.ToString()
                                                  }).ToList();
            ViewBag.SubScribeList = subScribeList;
            return View(subscribeAndWarning);
        }

        // POST: SubscribeAndWarning/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SubscribeAndWarning subscribeAndWarning)
        {
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
            //SubScribeList怎么来？SystemDb里面
            SystemDbContext systemdb = new SystemDbContext();
            List<SelectListItem> subScribeList = (from p in systemdb.SubscribeLists
                                                  where p.IsAvailable == true
                                                  select new
                                                  {
                                                      SubscribeName = p.SubscribeName,
                                                      SubscribeId = p.Id

                                                  }).ToList().Select(s => new SelectListItem
                                                  {
                                                      Text = s.SubscribeName,
                                                      Value = s.SubscribeId.ToString()
                                                  }).ToList();
            ViewBag.SubScribeList = subScribeList;

            if (subscribeAndWarning.CirculateMethod == 0)
            {
                subscribeAndWarning.StartDate = null;
                subscribeAndWarning.EndDate = null;

            }
            if (subscribeAndWarning.CirculateMethod == 1)
            {
                subscribeAndWarning.OnlyOneDate = null;

            }
            List<SelectListItem> staff = (from s in db.Staffs
                                          join d in db.Departments on s.Department equals d.DepartmentId
                                          select new
                                          {
                                              StaffNumber = s.StaffNumber,
                                              StaffName = s.Name,
                                              StaffDepartment = d.Name,
                                              StaffPosition = s.Position
                                          }).ToList().Select(s => new SelectListItem
                                          {
                                              Text = s.StaffNumber + "-" + s.StaffName + "-" + s.StaffDepartment + "-" + s.StaffPosition,
                                              Value = s.StaffNumber + "-" + s.StaffName + "-" + s.StaffDepartment + "-" + s.StaffPosition
                                          }).ToList();
            ViewBag.Receiver = staff;
            if (ModelState.IsValid)
            {
               
                SubscribeAndWarning temp = db.SubscribeAndWarnings.Find(subscribeAndWarning.Id);
                temp.CirculateMethod = subscribeAndWarning.CirculateMethod;
                temp.EndDate = subscribeAndWarning.EndDate;
                temp.EventName = subscribeAndWarning.EventName;
                temp.SubscribeContent = subscribeAndWarning.SubscribeContent;
                temp.IsAvailable = subscribeAndWarning.IsAvailable;
                temp.IsEmail = subscribeAndWarning.IsEmail;
          
                temp.MessageBody = subscribeAndWarning.MessageBody;
                temp.MessageAlert = subscribeAndWarning.MessageAlert;
                temp.MessageTitle = subscribeAndWarning.MessageTitle;
                temp.Receiver = Request["Receiver"];
                temp.ReceiverName = subscribeAndWarning.ReceiverName;
                temp.ReceiverTel = subscribeAndWarning.ReceiverTel;
                temp.ReceiverType = subscribeAndWarning.ReceiverType;
                temp.RemindDate = subscribeAndWarning.RemindDate;
                temp.ReceiverEmail = subscribeAndWarning.ReceiverEmail;
                temp.SendToApp = subscribeAndWarning.SendToApp;
           
                temp.StartDate = subscribeAndWarning.StartDate;
                temp.CirculateMethod = subscribeAndWarning.CirculateMethod;
              
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subscribeAndWarning);
        }

        // GET: SubscribeAndWarning/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscribeAndWarning subscribeAndWarning = db.SubscribeAndWarnings.Find(id);
            if (subscribeAndWarning == null)
            {
                return HttpNotFound();
            }
            return View(subscribeAndWarning);
        }

        // POST: SubscribeAndWarning/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubscribeAndWarning subscribeAndWarning = db.SubscribeAndWarnings.Find(id);
            db.SubscribeAndWarnings.Remove(subscribeAndWarning);
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
