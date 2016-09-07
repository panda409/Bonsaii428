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
using HXComm;

namespace Bonsaii.Controllers
{
    public class AppManageController : BaseController
    {
        private SystemDbContext sdb = new SystemDbContext();
        public const string appClientID = "YXA6MfhwoG6mEeWbT0ef-BFVfw";
        public const string appClientSecret = "YXA6GUEzoaBTvze7xX3RmlJnF1wMtZA";
        public const string appName = "fuckaholic";
        public const string orgName = "fuckaholic";
        // GET: /AppManage/
        public ActionResult Index()
        {
            var bind = (from b in sdb.BindCodes where b.CompanyId == this.CompanyId select b).ToList();
            List<BindCodeViewModel> list = new List<BindCodeViewModel>();
           
            foreach (var temp in bind)
            {
                Staff staff = (from s in db.Staffs where s.StaffNumber == temp.StaffNumber select s).FirstOrDefault();
                Department department = (from d in db.Departments where d.DepartmentId == staff.Department select d).FirstOrDefault();
                BindCodeViewModel bindModel = new BindCodeViewModel();
                bindModel.Id = temp.Id;
                  bindModel.StaffNumber = temp.StaffNumber;
                  bindModel.RealName = temp.RealName;
                  bindModel.BindingCode = temp.BindingCode;
                  bindModel.Phone = temp.Phone;
                  bindModel.LastTime = temp.LastTime;
                  bindModel.BindTag = temp.BindTag;
                  bindModel.Department = department.Name;
                  bindModel.Position = staff.Position;
                  list.Add(bindModel);
            }
            //var list = (from b in sdb.BindCodes
            //            where b.CompanyId == this.CompanyId
            //            select new BindCodeViewModel { Id = b.Id, StaffNumber = b.StaffNumber, RealName = b.RealName, BindingCode = b.BindingCode, Phone = b.Phone, LastTime = b.LastTime, BindTag = b.BindTag }).ToList();
            return View(list);
        }

        // GET: /AppManage/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BindCode bindcode = sdb.BindCodes.Find(id);
            if (bindcode == null)
            {
                return HttpNotFound();
            }
            return View(bindcode);
        }

        // GET: /AppManage/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /AppManage/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CompanyId,ConnectionString,StaffNumber,RealName,BindingCode,Phone,BindTag,LastTime,IsAvail")] BindCode bindcode)
        {
            if (ModelState.IsValid)
            {
                sdb.BindCodes.Add(bindcode);
                sdb.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bindcode);
        }

        // GET: /AppManage/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BindCode bind = (from b in sdb.BindCodes
                        where b.Id == id select b).FirstOrDefault();
            Staff staff = (from s in db.Staffs where s.StaffNumber == bind.StaffNumber select s).FirstOrDefault();
            Department department = (from d in db.Departments where d.DepartmentId == staff.Department select d).FirstOrDefault();
            BindCodeViewModel bindModel = new BindCodeViewModel();
            bindModel.Id = bind.Id;
            bindModel.StaffNumber = bind.StaffNumber;
            bindModel.RealName = bind.RealName;
            bindModel.BindingCode = bind.BindingCode;
            bindModel.Phone = bind.Phone;
            bindModel.LastTime = bind.LastTime;
            bindModel.BindTag = bind.BindTag;
            bindModel.Department = department.Name;
            bindModel.Position = staff.Position;
            return View(bindModel);
        }

        // POST: /AppManage/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id)
        {
                string temp;
                int i = 0;
                do
                {
                    temp = GetRandomCode(8);
                    var count = (from s in sdb.BindCodes where s.BindingCode == temp select s).ToList();
                    i = count.Count;
                } while (i != 0);
                BindCode code = sdb.BindCodes.Find(id);
                code.BindTag = false;
                code.BindingCode = temp;
                sdb.SaveChanges();
                var friends = (from u in sdb.Users where u.CompanyId == this.CompanyId&&u.StaffNumber!=code.StaffNumber select u).ToList();
                UserModels user = (from u in sdb.Users where u.StaffNumber == code.StaffNumber select u).FirstOrDefault();
                //在环信中注册
                EaseMobDemo myEaseMobDemo = new EaseMobDemo(appClientID, appClientSecret, appName, orgName);

                foreach (var temp1 in friends)//取出每个字段
                {
                    myEaseMobDemo.AccountDelFriend(user.UserName, temp1.UserName);//删除原来账户的环信好友。
                }
                user.CompanyFullName = "GeneralStaff";//model.CompanyFullName,

                user.Name = "";   //用户注册的时候写入该名称
                user.IsProved = false;           //是否审核的标志
                user.IsAvailable = false;         //是否是可用的管理员
                user.IsRoot = false;               //注册企业号的人默认就是企业的超级管理员
                user.CompanyId = "app-id";
                user.ConnectionString = "app-ConnectionString";
                user.HuanTag = true;
                user.BindTag = false;//找回密码后就没有绑定公司
                user.StaffNumber = null;
                user.BindingCode = null;
                sdb.SaveChanges();
                return RedirectToAction("Index");
           
        }
       
        public ActionResult Reset(int id)
        {
            string temp;
            int i = 0;
            do
            {
                temp = GetRandomCode(8);
                var count = (from s in sdb.BindCodes where s.BindingCode == temp select s).ToList();
                i = count.Count;
            } while (i != 0);
            BindCode code = sdb.BindCodes.Find(id);
            code.BindTag = false;
            code.BindingCode = temp;
            sdb.SaveChanges();
           
            return RedirectToAction("Index");

        }
        // GET: /AppManage/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BindCode bindcode = sdb.BindCodes.Find(id);
            if (bindcode == null)
            {
                return HttpNotFound();
            }
            return View(bindcode);
        }

        // POST: /AppManage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BindCode bindcode = sdb.BindCodes.Find(id);
            sdb.BindCodes.Remove(bindcode);
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
