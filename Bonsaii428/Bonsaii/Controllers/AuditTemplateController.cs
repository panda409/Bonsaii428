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
using BonsaiiModels.Staffs;

namespace Bonsaii.Controllers
{
    public class AuditTemplateController : BaseController
    {
        public ActionResult UnifyCreate()
        {

            return View();

        }
        public AuditStepViewModel GetAuditStepViewModel(AuditStep tmp)
        {
            using (base.db)
            {
                return new AuditStepViewModel()
                {
                    SId = tmp.SId,
                    TId = tmp.TId,
                    Name = tmp.Name,
                    Description = tmp.Description,
                    //        PreName = db.AuditSteps.Find(tmp.PreId).Name,
                    ApprovedToSIdName = db.AuditSteps.Find(tmp.ApprovedToSId).Name,
                    NotApprovedToSIdName = db.AuditSteps.Find(tmp.NotApprovedToSId).Name,
                    Days = tmp.Days,
                    Approver = tmp.Approver
                };
            }
        }
        /// <summary>
        /// 将AuditStepModel转换为AuditStepViewModel
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<AuditStepViewModel> GetAutidStepViewModelList(List<AuditStep> list)
        {
            List<AuditStepViewModel> AuditStepViewModels = new List<AuditStepViewModel>();
            using (base.db)
            {
                foreach (AuditStep tmp in list)
                {
                    AuditStepViewModels.Add(new AuditStepViewModel()
                    {
                        SId = tmp.SId,
                        TId = tmp.TId,
                        Name = tmp.Name,
                        Description = tmp.Description,
                        //          PreName = db.AuditSteps.Find(tmp.PreId).Name,
                        ApprovedToSIdName = db.AuditSteps.Find(tmp.ApprovedToSId).Name,
                        NotApprovedToSIdName = db.AuditSteps.Find(tmp.NotApprovedToSId).Name,
                        Days = tmp.Days,
                        Approver = tmp.Approver
                    });
                }
            }
            return AuditStepViewModels;
        }
        // GET: AuditTemplate/AuditStepIndex
        public ActionResult AuditStepIndex(int? id)
        {

            List<AuditStep> item = db.AuditSteps.Where(p => p.TId == id).ToList();

            //var item = from p in db.AuditSteps
            //           where p.TId == id
            //           //join q in db.AuditSteps on p.SId equals q.PreId into r
            //           //from x in r.DefaultIfEmpty()
            //           orderby p.TId
            //           select new AuditStepViewModel
            //           {
            //               SId = p.SId,
            //               TId = p.TId,`16ty23wygh
            //               Name = p.Name,
            //               //PreName = x.Name,
            //               //PreName = p.Name,
            //               ApprovedToSId = p.ApprovedToSId,
            //               NotApprovedToSId = p.NotApprovedToSId,
            //               Approver = p.Approver,
            //               Description = p.Description,
            //               Days = p.Days
            //           };
            ViewBag.TemplateId = id;

            return View(GetAutidStepViewModelList(item));
        }

        // GET: AuditStep/AuditStepCreate
        public ActionResult AuditStepCreate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.TemplateId = id;

            //实现下拉列表
            List<SelectListItem> itemPreId = db.AuditSteps.ToList().Select(c => new SelectListItem
            {
                Value = c.SId.ToString(),//保存的值
                Text = c.Name//显示的值
            }).ToList();

            //传值到页面
            ViewBag.PreIdList = itemPreId;

            //SystemDbContext db2 = new SystemDbContext();
            //List<SelectListItem> item2 = db2.Users.Where(p => p.CompanyId == this.CompanyId).ToList().Select(c => new SelectListItem
            //{
            //    Value = c.UserName,
            //    Text = c.UserName
            //}).ToList();
            //SelectListItem i = new SelectListItem();
            //i.Value = "";
            //i.Text = "-请选择-";
            //i.Selected = true;
            //item2.Add(i);
            //ViewBag.List2 = item2;
            //ViewBag.List2 = item2; 
            //实现下拉列表：下一步骤
            List<SelectListItem> itemNext = db.AuditSteps.Where(p => p.TId == id || p.SId == -1).ToList().Select(c => new SelectListItem
            {
                Value = c.SId.ToString(),//保存的值
                Text = c.Name//显示的值
            }).ToList();

            //传值到页面
            ViewBag.ListNext = itemNext;

            //实现下拉列表：上一步骤
            List<SelectListItem> itemLast = db.AuditSteps.Where(p => p.TId == id || p.SId == -1).ToList().Select(c => new SelectListItem
            {
                Value = c.SId.ToString(),//保存的值
                Text = c.Name//显示的值
            }).ToList();

            //传值到页面
            ViewBag.ListLast = itemLast;
            //List<SelectListItem> staff = (from s in db.Staffs
            //                                                           //   join d in db.Departments on s.Department equals d.DepartmentId
            //                                                           select new
            //                                                           {
            //                                                               StaffNumber = s.StaffNumber,
            //                                                               StaffName = s.Name,//,
            //                                                               StaffEmail = s.Email,
            //                                                               StaffPhone = s.IndividualTelNumber//手机号码
            //                                                               //    StaffDepartment = d.Name,
            //                                                               // StaffPosition = s.Position
            //                                                           }).ToList().Select(s => new SelectListItem
            //                                                           {
            //                                                               Text = s.StaffNumber + "-" + s.StaffName,
            //                                                               Value = s.StaffNumber + "-" + s.StaffName + "<" + s.StaffPhone + "-" + s.StaffEmail + ">"// backlog.Recipient;

            //                                                           }).ToList();
            SystemDbContext sysdb = new SystemDbContext();

            var staffDbs = (from s in db.Staffs where s.AuditStatus == 3 && s.ArchiveTag == false select s).ToList();
            var staffBindingCodes = (from p in sysdb.BindCodes where p.CompanyId == this.CompanyId select p).ToList();
            List<StaffModel> staffModelList = new List<StaffModel>();
            foreach (var staffBindingCode in staffBindingCodes)
            {

                //var tempStaff=(from p in sysdb.BindCodes join r in sysdb.Users on p.StaffNumber equals r.StaffNumber
                //            where staffDb.StaffNumber ==sysdb.Staff
                string position = (from p in staffDbs
                                   where p.StaffNumber == staffBindingCode.StaffNumber
                                   select p.Position).ToList().Single();

                StaffModel staffModel = new StaffModel();
                if (staffBindingCode.BindTag == true)
                {
                    staffModel.text = staffBindingCode.StaffNumber + "-" + staffBindingCode.RealName + "-" + position + "(" + "已绑定APP" + ")";
                    string username = (from p in sysdb.Users where p.CompanyId == this.CompanyId && p.StaffNumber == staffBindingCode.StaffNumber select p.UserName).ToList().Single();
                    staffModel.value = username + "-" + staffBindingCode.StaffNumber + "-" + staffBindingCode.RealName;
                }
                else
                {
                    staffModel.text = staffBindingCode.StaffNumber + "-" + staffBindingCode.RealName + "-" + position + "(" + "未绑定APP" + ")";
                    staffModel.value = staffBindingCode.StaffNumber + "-" + staffBindingCode.RealName;
                }


                staffModel.department = (from p in staffDbs
                                         join q in db.Departments on p.Department equals q.DepartmentId
                                         where p.StaffNumber == staffBindingCode.StaffNumber
                                         select q.DepartmentId).ToList().Single();
                staffModelList.Add(staffModel);
            }
            //var staff = (from p in sysdb.BindCodes
            //              join q in staffs
            //                  on p.StaffNumber equals q.StaffNumber
            //              join r in sysdb.Users on p.StaffNumber equals r.StaffNumber
            //              where r.CompanyId == this.CompanyId
            //              where q.AuditStatus == 3 && q.ArchiveTag == false
            //              where p.BindTag == true && p.IsAvail == true
            //              select new StaffModel
            //              {
            //                  department = q.Department,
            //                  text = q.StaffNumber + "-" + q.Name + "(" + p.BindTag + ")",
            //                  value = q.StaffNumber + "-" + q.Name
            //              }).ToList();  

            var group = (from d in db.Departments
                         select new StaffModel { department = d.DepartmentId, name = d.Name }).ToList();

            Dictionary<string, int> sum = new Dictionary<string, int>();
            foreach (var g in group)
            {
                int count = 0;
                foreach (var s in staffModelList)
                {
                    if (s.department == g.department)
                        count++;
                }
                sum.Add(g.department, count);
            }

            ViewBag.Count = sum;
            ViewBag.Receiver = staffModelList;
            ViewBag.Group = group;
            return View();
        }

        // POST: AuditStep/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AuditStepCreate(AuditStep auditSteps)
        {
            List<SelectListItem> staff = (from s in db.Staffs
                                          //   join d in db.Departments on s.Department equals d.DepartmentId
                                          select new
                                          {
                                              StaffNumber = s.StaffNumber,
                                              StaffName = s.Name,//,
                                              StaffEmail = s.Email,
                                              StaffPhone = s.IndividualTelNumber//手机号码
                                              //    StaffDepartment = d.Name,
                                              // StaffPosition = s.Position
                                          }).ToList().Select(s => new SelectListItem
                                          {
                                              Text = s.StaffNumber + "-" + s.StaffName,
                                              Value = s.StaffNumber + "-" + s.StaffName + "<" + s.StaffPhone + "-" + s.StaffEmail + ">"// backlog.Recipient;

                                          }).ToList();
            ViewBag.Receiver = staff;
            if (ModelState.IsValid)
            {
                int id = int.Parse(Request["AuditTemplateId"]);
                auditSteps.TId = id;
                db.AuditSteps.Add(auditSteps);
                //先插入一个Step节点
                db.SaveChanges();
                //更新插入新节点后Template中的FirstStepSId
                var coutSteps = (from p in db.AuditSteps where p.TId == auditSteps.TId select p).ToList();
                //找到该节点对应的模板
                AuditTemplate template = db.AuditTemplates.Find(auditSteps.TId);
                template.StepCount = coutSteps.Count();
                db.SaveChanges();
                if (template.FirstStepSId == -1)
                {
                    template.FirstStepSId = auditSteps.SId;

                    db.SaveChanges();
                }
                return RedirectToActionPermanent("AuditStepIndex", "AuditTemplate", new { id = id });
            }
            return View(auditSteps);
        }

        // GET: AuditStep/Delete/5
        public ActionResult AuditStepDelete(int? id, int? id2)
        {
            ViewBag.TemplateId = id2;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditStep auditSteps = db.AuditSteps.Find(id);
            if (auditSteps == null)
            {
                return HttpNotFound();
            }
            return View(GetAuditStepViewModel(auditSteps));
        }

        // POST: AuditStep/Delete/5
        [HttpPost, ActionName("AuditStepDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult AuditStepDeleteConfirmed(FormCollection collection)
        {
            /*删除AuditSteps表*/
            AuditStep auditSteps = db.AuditSteps.Find(int.Parse(collection["SId"]));
            db.AuditSteps.Remove(auditSteps);
            db.SaveChanges();

            return RedirectToActionPermanent("AuditStepIndex", "AuditTemplate", new { id = int.Parse(collection["AuditTemplateId"]) });
        }

        // GET: AuditStep/Details/5
        public ActionResult AuditStepDetails(int? id, int? id2)
        {

            ViewBag.TemplateId = id2;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditStep auditSteps = db.AuditSteps.Find(id);

            if (auditSteps == null)
            {
                return HttpNotFound();
            }


            return View(GetAuditStepViewModel(auditSteps));

        }

        // GET: AuditTemplates/AuditStepEdit/5
        public ActionResult AuditStepEdit(int? id, int? id2)
        {
            ViewBag.TemplateId = id2;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditStep auditSteps = db.AuditSteps.Find(id);
            if (auditSteps == null)
            {
                return HttpNotFound();
            }

            //实现下拉列表：下一步骤
            List<SelectListItem> itemNext = db.AuditSteps.Where(p => (p.TId == id2) || p.SId == -1).ToList().Select(c => new SelectListItem
            {
                Value = c.SId.ToString(),//保存的值
                Text = c.Name//显示的值
            }).ToList();

            //传值到页面
            ViewBag.ListNext = itemNext;

            //实现下拉列表：上一步骤
            List<SelectListItem> itemLast = db.AuditSteps.Where(p => (p.TId == id2) || p.SId == -1).ToList().Select(c => new SelectListItem
            {
                Value = c.SId.ToString(),//保存的值
                Text = c.Name//显示的值
            }).ToList();

            //传值到页面
            ViewBag.ListLast = itemLast;
            SystemDbContext db2 = new SystemDbContext();
            List<SelectListItem> item2 = db2.Users.Where(p => p.CompanyId == this.CompanyId).ToList().Select(c => new SelectListItem
            {
                Value = c.UserName,
                Text = c.UserName
            }).ToList();
            //增加一个null选项
            SelectListItem i = new SelectListItem();
            i.Value = "";
            i.Text = "-请选择-";
            i.Selected = true;
            item2.Add(i);
            ViewBag.List2 = item2;

            SystemDbContext sysdb = new SystemDbContext();

            var staffDbs = (from s in db.Staffs where s.AuditStatus == 3 && s.ArchiveTag == false select s).ToList();
            var staffBindingCodes = (from p in sysdb.BindCodes where p.CompanyId == this.CompanyId select p).ToList();
            List<StaffModel> staffModelList = new List<StaffModel>();
            foreach (var staffBindingCode in staffBindingCodes)
            {

                //var tempStaff=(from p in sysdb.BindCodes join r in sysdb.Users on p.StaffNumber equals r.StaffNumber
                //            where staffDb.StaffNumber ==sysdb.Staff
                string position = (from p in staffDbs
                                   where p.StaffNumber == staffBindingCode.StaffNumber
                                   select p.Position).ToList().SingleOrDefault();
               


                StaffModel staffModel = new StaffModel();
                if (staffBindingCode.BindTag == true)
                {
                    staffModel.text = staffBindingCode.StaffNumber + "-" + staffBindingCode.RealName + "-" + position + "(" + "已绑定APP" + ")";
                    string username = (from p in sysdb.Users where p.CompanyId == this.CompanyId && p.StaffNumber == staffBindingCode.StaffNumber select p.UserName).ToList().Single();
                    staffModel.value = username + "-" + staffBindingCode.StaffNumber + "-" + staffBindingCode.RealName;
                }
                else
                {
                    staffModel.text = staffBindingCode.StaffNumber + "-" + staffBindingCode.RealName + "-" + position + "(" + "未绑定APP" + ")";
                    staffModel.value = staffBindingCode.StaffNumber + "-" + staffBindingCode.RealName;
                }


                staffModel.department = (from p in staffDbs
                                         join q in db.Departments on p.Department equals q.DepartmentId
                                         where p.StaffNumber == staffBindingCode.StaffNumber
                                         select q.DepartmentId).ToList().SingleOrDefault();
                staffModelList.Add(staffModel);
            }
            //var staff = (from p in sysdb.BindCodes
            //              join q in staffs
            //                  on p.StaffNumber equals q.StaffNumber
            //              join r in sysdb.Users on p.StaffNumber equals r.StaffNumber
            //              where r.CompanyId == this.CompanyId
            //              where q.AuditStatus == 3 && q.ArchiveTag == false
            //              where p.BindTag == true && p.IsAvail == true
            //              select new StaffModel
            //              {
            //                  department = q.Department,
            //                  text = q.StaffNumber + "-" + q.Name + "(" + p.BindTag + ")",
            //                  value = q.StaffNumber + "-" + q.Name
            //              }).ToList();  

            var group = (from d in db.Departments
                         select new StaffModel { department = d.DepartmentId, name = d.Name }).ToList();

            Dictionary<string, int> sum = new Dictionary<string, int>();
            foreach (var g in group)
            {
                int count = 0;
                foreach (var s in staffModelList)
                {
                    if (s.department == g.department)
                        count++;
                }
                sum.Add(g.department, count);
            }

            ViewBag.Count = sum;
            ViewBag.Receiver = staffModelList;
            ViewBag.Group = group;

            return View(auditSteps);
        }

        // POST: AuditTemplates/AuditStepEdit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AuditStepEdit(AuditStep auditSteps)
        {
            List<SelectListItem> staff = (from s in db.Staffs
                                          //   join d in db.Departments on s.Department equals d.DepartmentId
                                          select new
                                          {
                                              StaffNumber = s.StaffNumber,
                                              StaffName = s.Name,//,
                                              StaffEmail = s.Email,
                                              StaffPhone = s.IndividualTelNumber//手机号码
                                              //    StaffDepartment = d.Name,
                                              // StaffPosition = s.Position
                                          }).ToList().Select(s => new SelectListItem
                                          {
                                              Text = s.StaffNumber + "-" + s.StaffName,
                                              Value = s.StaffNumber + "-" + s.StaffName + "<" + s.StaffPhone + "-" + s.StaffEmail + ">"// backlog.Recipient;

                                          }).ToList();
            ViewBag.Receiver = staff;
            //List<SelectListItem> staff = (from s in db.Staffs
            //                              //   join d in db.Departments on s.Department equals d.DepartmentId
            //                              select new
            //                              {
            //                                  StaffNumber = s.StaffNumber,
            //                                  StaffName = s.Name,//,
            //                                  StaffEmail = s.Email,
            //                                  StaffPhone = s.IndividualTelNumber//手机号码
            //                                  //    StaffDepartment = d.Name,
            //                                  // StaffPosition = s.Position
            //                              }).ToList().Select(s => new SelectListItem
            //                              {
            //                                  Text = s.StaffNumber + "-" + s.StaffName,
            //                                  Value = s.StaffNumber + "-" + s.StaffName + "<" + s.StaffPhone + "-" + s.StaffEmail + ">"// backlog.Recipient;

            //                              }).ToList();
            //ViewBag.Receiver = staff;
            if (ModelState.IsValid)
            {
                int id = int.Parse(Request["AuditTemplateId"]);

                /*修改记录*/
                AuditStep auditStepsEdit = db.AuditSteps.Find(auditSteps.SId);
                auditStepsEdit.TId = id;
                auditStepsEdit.Name = auditSteps.Name;
                auditStepsEdit.Description = auditSteps.Description;
                auditStepsEdit.ApprovedToSId = auditSteps.ApprovedToSId;
                auditStepsEdit.NotApprovedToSId = auditSteps.NotApprovedToSId;
                auditStepsEdit.Days = auditSteps.Days;
                auditStepsEdit.Approver = auditSteps.Approver;
                // db.Entry(auditSteps).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToActionPermanent("AuditStepIndex", "AuditTemplate", new { id = id });
            }
            return View(auditSteps);
        }


        /// <summary>
        /// 测试一下新功能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AuditStep(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.TemplateId = id;

            //实现下拉列表
            List<SelectListItem> itemPreId = db.AuditSteps.ToList().Select(c => new SelectListItem
            {
                Value = c.SId.ToString(),//保存的值
                Text = c.Name//显示的值
            }).ToList();

            //传值到页面
            ViewBag.PreIdList = itemPreId;

            //实现下拉列表
            List<SelectListItem> itemApprovedToSId = db.AuditSteps.ToList().Select(c => new SelectListItem
            {
                Value = c.SId.ToString(),//保存的值
                Text = c.Name//显示的值
            }).ToList();

            //传值到页面
            ViewBag.ApprovedToSIdList = itemApprovedToSId;
            // (from p in db.Users where p.CompanyId == this.CompanyId  select p.UserName).ToList();

            //实现下拉列表
            List<SelectListItem> itemNotApprovedToSId = db.AuditSteps.ToList().Select(c => new SelectListItem
            {
                Value = c.SId.ToString(),//保存的值
                Text = c.Name//显示的值
            }).ToList();

            //传值到页面
            ViewBag.NotApprovedToSIdList = itemNotApprovedToSId;
            return View();
        }

        // POST: AuditStep/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        /// <summary>
        /// 测试新功能
        /// </summary>
        /// <param name="auditSteps"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AuditStep(AuditStep auditSteps)
        {
            if (ModelState.IsValid)
            {
                int id = int.Parse(Request["AuditTemplateId"]);
                auditSteps.TId = id;
                db.AuditSteps.Add(auditSteps);
                //先插入一个Step节点
                db.SaveChanges();
                //更新插入新节点后Template中的FirstStepSId
                //找到该节点对应的模板
                AuditTemplate template = db.AuditTemplates.Find(auditSteps.TId);
                if (template.FirstStepSId == -1)
                {
                    template.FirstStepSId = auditSteps.SId;
                    db.SaveChanges();
                }
                return RedirectToActionPermanent("AuditStepIndex", "AuditTemplate", new { id = id });
            }
            return View(auditSteps);
        }

        // GET: AuditTemplate
        public ActionResult Index()
        {
            var template = (from p in db.AuditTemplates where p.Id != -1 orderby p.BType select p).ToList();
            //  return View(db.AuditTemplates.ToList());
            return View(template.OrderByDescending(c=>c.Id));
        }

        // GET: AuditTemplate/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditTemplate auditTemplate = db.AuditTemplates.Find(id);
            if (auditTemplate == null)
            {
                return HttpNotFound();
            }

            return View(auditTemplate);
        }

        // GET: AuditTemplate/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuditTemplate/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AuditTemplate auditTemplate)
        {
            auditTemplate.CreateDate = DateTime.Now;
            auditTemplate.Creator = this.UserName;
            if (ModelState.IsValid)
            {
                if (auditTemplate.LongTime == true)
                {
                    auditTemplate.ExpireTime = DateTime.MaxValue.Date;
                }
                else
                {
                    if (auditTemplate.ExpireTime == null)
                    {
                        ModelState.AddModelError("", "请输入过期时间！");
                        return View(auditTemplate);
                    }
                }
                if (auditTemplate.StartTime >= auditTemplate.ExpireTime)
                {
                    ModelState.AddModelError("", "日期范围不正确！");
                    return View(auditTemplate);
                }

                var item = (from p in db.AuditTemplates where p.BType == auditTemplate.BType select p).ToList();
                int flagWrong = 0;
                foreach (var p in item)
                {
                    if (p.StartTime > auditTemplate.ExpireTime || p.ExpireTime < auditTemplate.StartTime)
                    {
                        //  flagRight = 0;
                    }
                    else
                    {
                        flagWrong++;

                        //ModelState.AddModelError("", "日期范围不正确！");
                    }

                }
                if (flagWrong != 0)
                {
                    ModelState.AddModelError("", "该日期范围已被占用！");
                    return View(auditTemplate);
                }
                //if ((auditTemplate.StartTime < auditTemplate.CreateDate))
                //{
                //    ModelState.AddModelError("", "请选择正确的生效时间！");
                //}

                //if ((auditTemplate.ExpireTime < auditTemplate.CreateDate) || (auditTemplate.ExpireTime < auditTemplate.StartTime))
                //{
                //    ModelState.AddModelError("", "请选择正确的过期时间！");
                //}

                auditTemplate.FirstStepSId = -1;
                db.AuditTemplates.Add(auditTemplate);
                db.SaveChanges();
                /*插入节点*/
                for (int i = 0; i < auditTemplate.StepCount; i++)
                {

                    AuditStep auditstep = new AuditStep();
                    auditstep.TId = auditTemplate.Id;
                    int n = i + 1;
                    auditstep.Name = "第" + n + "个步骤";
                    auditstep.Days = 1;
                    auditstep.ApprovedToSId = -1;
                    auditstep.NotApprovedToSId = -1;
                    db.AuditSteps.Add(auditstep);
                    db.SaveChanges();
                }
                var shit = (from p in db.AuditSteps where p.TId == auditTemplate.Id select p.SId).FirstOrDefault();
                /*修改*/
                auditTemplate.FirstStepSId = shit;
                db.SaveChanges();
                //创建成功后进入第二步：修改审批节点
                // return RedirectToAction("Index");
                return RedirectToActionPermanent("AuditStepIndex", "AuditTemplate", new { id = auditTemplate.Id });
                //return RedirectToAction("AuditStepIndex","AuditTemplate",id=auditTemplate.Id);

            }

            return View(auditTemplate);
        }

        // GET: AuditTemplate/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditTemplate auditTemplate = db.AuditTemplates.Find(id);
            if (auditTemplate == null)
            {
                return HttpNotFound();
            }
            return View(auditTemplate);
        }

        // POST: AuditTemplate/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AuditTemplate auditTemplate)
        {
            if (ModelState.IsValid)
            {
                if (auditTemplate.StartTime >= auditTemplate.ExpireTime)
                {
                    ModelState.AddModelError("", "日期范围不正确！");
                    return View(auditTemplate);
                }

                var item = (from p in db.AuditTemplates where p.BType == auditTemplate.BType && p.Id != auditTemplate.Id select p).ToList();
                int flagWrong = 0;
                foreach (var p in item)
                {
                    if (p.StartTime > auditTemplate.ExpireTime || p.ExpireTime < auditTemplate.StartTime)
                    {
                        //  flagRight = 0;
                    }
                    else
                    {
                        flagWrong++;

                        //ModelState.AddModelError("", "日期范围不正确！");
                    }

                }
                if (flagWrong != 0)
                {
                    ModelState.AddModelError("", "该日期范围已被占用！");
                    return View(auditTemplate);
                }

                /*修改记录*/
                AuditTemplate auditTemplateEdit = db.AuditTemplates.Find(auditTemplate.Id);

                auditTemplateEdit.Name = auditTemplate.Name;
                auditTemplateEdit.StartTime = auditTemplate.StartTime;
                auditTemplateEdit.TypeName = auditTemplate.TypeName;
                auditTemplateEdit.BType = auditTemplate.BType;
                auditTemplateEdit.CreateDate = auditTemplate.CreateDate;
                auditTemplateEdit.Creator = auditTemplate.Creator;
                auditTemplateEdit.Description = auditTemplate.Description;
                auditTemplateEdit.ExpireTime = auditTemplate.ExpireTime;


                auditTemplateEdit.FirstStepSId = auditTemplate.FirstStepSId;//重要。不然修改后就找不到第一个节点了~

                //db.Entry(auditTemplate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(auditTemplate);
        }

        // GET: AuditTemplate/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditTemplate auditTemplate = db.AuditTemplates.Find(id);
            if (auditTemplate == null)
            {
                return HttpNotFound();
            }
            return View(auditTemplate);
        }

        // POST: AuditTemplate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AuditTemplate auditTemplate = db.AuditTemplates.Find(id);
            var auditApplications = (from p in db.AuditApplications where p.TId == id select p).ToList();

            //if (auditApplications.Count == 0)
            //{
            /*删除节点*/
            var shits = (from p in db.AuditSteps
                         where p.TId == id
                         orderby p.SId descending
                         select p).ToList();
            foreach (var shit in shits)
            {
                db.AuditSteps.Remove(shit); db.SaveChanges();
            }
            foreach (var auditApplication in auditApplications)
            {

                var auditProcesses = (from p in db.AuditProcesses where p.AId == auditApplication.Id select p).ToList();
                foreach (var auditProcess in auditProcesses)
                {
                    db.AuditProcesses.Remove(auditProcess);

                } db.SaveChanges();
                db.AuditApplications.Remove(auditApplication);
            }
            db.AuditTemplates.Remove(auditTemplate);
            db.SaveChanges();
            return RedirectToAction("Index");
            //}
            //else {
            //    ViewBag.ErrorMessage = true;
            //    return View(auditTemplate);
            //}
        }

        [HttpPost]
        public JsonResult GetApprovers(string number)
        {
            try
            {
                List<string> users = new List<string>();
                using (SystemDbContext db = new SystemDbContext())
                {
                    users = (from x in db.Users where x.CompanyId.Equals(this.CompanyId) select x.UserName + "-" + x.Name).ToList();
                }
                return Json(new
                {
                    success = true,
                    data = users
                });
            }
            catch (Exception e)
            {
                return Json(new { success = false, msg = e.Message });
            }
        }

        /*实现单据类别搜索：显示所有单别编号*/
        [HttpPost]
        public JsonResult BillTypeNumberSearch(string billSort)
        {
            try
            {
                var items = (from b in db.BillProperties
                             // where b.BillSort == billSort
                             select new
                             {
                                 text = b.Type + " " + b.TypeName,
                                 id = b.Type
                             }).ToList();

                return Json(items);
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }
        }
        //Ystep节点是Json数据
        [HttpPost]
        public JsonResult YSteps(int? id)
        {
            try
            {
                var items = (from p in db.AuditSteps
                             where p.TId == id
                             select new
                             {
                                 title = p.Name,
                                 content = p.Description,
                                 id = p.SId


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
