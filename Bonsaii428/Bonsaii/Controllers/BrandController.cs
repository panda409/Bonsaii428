using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;

namespace Bonsaii.Controllers
{
    public class BrandController : BaseController
    {
           [Authorize(Roles = "Admin,Brand_Index")]
        // GET: Brand
        public ActionResult Index()
        {
            List<Brand> list = (from p in db.Brands select p).ToList();

            foreach (var temp in list)
            {
                temp.BrandName = db.BrandTemplateModels.Find(temp.BrandId).Description;
                temp.StaffName = db.Staffs.Find(temp.StaffId).Name;
            }
            return View(list);


        }

        // GetImageHead


        /*后台获取数据库的图片*/
        public FileContentResult GetImageHead(string Id)
        {
            Staff com = db.Staffs.FirstOrDefault(p => p.StaffNumber == Id);

            if (com.Head != null)
            {
                return File(com.Head, com.HeadType);//File方法直接将二进制转化为指定类型了。
            }
            else
            {
                SystemDbContext sdb = new SystemDbContext();

                Company company = sdb.Companies.Find(this.CompanyId);
                return File(company.Logo, company.LogoType);
            }
        }


        /*后台获取数据库的图片*/
        public FileContentResult GetImage(int Id)
        {
            BrandTemplateModels com = db.BrandTemplateModels.FirstOrDefault(p => p.Id == Id);
            //Company com = db.Companies.FirstOrDefault(p => p.CompanyId == CompanyId);
            if (com != null)
            {
                return File(com.BrandTemplate, com.BrandTemplateType);//File方法直接将二进制转化为指定类型了。
            }
            else
            {
                return null;
            }
        }

        public ActionResult Print(int? id)
        {
            Brand com = db.Brands.Find(id);
            var model = (from p in db.BrandTemplateModels where p.Id == com.BrandId select p).FirstOrDefault();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (com == null)
            {
                return HttpNotFound();
            }
            //得到该模板的字段
            var reserve = (from b in db.Brands
                           join q in db.BrandTemplateReserves on b.BrandId equals q.Number
                           join p in db.ConfirmedFields on q.FieldId equals p.Id
                           where q.Number == com.BrandId
                           select new BrandTemplateViewModels
                           {
                               Description = p.Description,
                               Value = p.FieldName

                           }).ToList();
            /*员工工号*/
            foreach (var temp in reserve)
            {

                if (temp.Value == "StaffNumber")
                {
                    var description1 = (
                          from b in db.Brands
                          join q in db.BrandTemplateReserves on b.BrandId equals q.Number
                          join s in db.Staffs on b.StaffId equals s.Number
                          join p in db.ConfirmedFields on q.FieldId equals p.Id
                          where q.Number == com.BrandId && p.FieldName == "StaffNumber" && s.Number == com.StaffId
                          select new BrandTemplateViewModels
                          {

                              Description = p.Description,
                              Value = s.StaffNumber

                          }).ToList();
                    ViewBag.fieldValueList1 = description1;
                    break;

                }
                else
                {
                    ViewBag.fieldValueList1 = new List<string>();
                }
            }
            /*性别*/
            foreach (var temp in reserve)
            {
                if (temp.Value == "Gender")
                {
                    var description2 = (
                              from b in db.Brands
                              join q in db.BrandTemplateReserves on b.BrandId equals q.Number
                              join s in db.Staffs on b.StaffId equals s.Number
                              join p in db.ConfirmedFields on q.FieldId equals p.Id
                              where q.Number == com.BrandId && p.FieldName == "Gender" && s.Number == com.StaffId
                              select new BrandTemplateViewModels
                              {

                                  Description = p.Description,
                                  Value = s.Gender

                              }).ToList();
                    ViewBag.fieldValueList2 = description2;
                    break;
                }
                else
                {
                    ViewBag.fieldValueList2 = new List<string>();
                }
            }
            /*姓名*/
            foreach (var temp in reserve)
            {
                if (temp.Value == "Name")
                {
                    var description3 = (
                              from b in db.Brands
                              join q in db.BrandTemplateReserves on b.BrandId equals q.Number
                              join s in db.Staffs on b.StaffId equals s.Number
                              join p in db.ConfirmedFields on q.FieldId equals p.Id
                              where q.Number == com.BrandId && p.FieldName == "Name" && s.Number == com.StaffId
                              select new BrandTemplateViewModels
                              {

                                  Description = p.Description,
                                  Value = s.Name

                              }).ToList();
                    ViewBag.fieldValueList3 = description3;
                    break;
                }
                else
                {
                    ViewBag.fieldValueList3 = new List<string>();
                }
            }
            foreach (var temp in reserve)
            {
                if (temp.Value == "Department")
                {
                    var description44 = (
                              from b in db.Brands
                              join q in db.BrandTemplateReserves on b.BrandId equals q.Number
                              join s in db.Staffs on b.StaffId equals s.Number
                              join p in db.ConfirmedFields on q.FieldId equals p.Id
                              where q.Number == com.BrandId && p.FieldName == "Department" && s.Number == com.StaffId
                              select new BrandTemplateViewModels
                              {

                                  Description = p.Description,
                                  Value = s.Department

                              }).ToList();
                    foreach (var temp1 in description44)
                    {

                        var description4 = (
                                            from d in db.Departments
                                            where d.DepartmentId.Trim() == temp1.Value.Trim()
                                            select new BrandTemplateViewModels
                                            {
                                                Description = temp1.Description,
                                                Value = d.Name
                                            }).ToList();
                        ViewBag.fieldValueList4 = description4;
                        break;

                    }

                }
                else
                {
                    ViewBag.fieldValueList4 = new List<string>();
                }
            }
            foreach (var temp in reserve)
            {
                if (temp.Value == "WorkType")
                {
                    var description5 = (
                              from b in db.Brands
                              join q in db.BrandTemplateReserves on b.BrandId equals q.Number
                              join s in db.Staffs on b.StaffId equals s.Number
                              join p in db.ConfirmedFields on q.FieldId equals p.Id
                              where q.Number == com.BrandId && p.FieldName == "WorkType" && s.Number == com.StaffId
                              select new BrandTemplateViewModels
                              {

                                  Description = p.Description,
                                  Value = s.WorkType

                              }).ToList();
                    ViewBag.fieldValueList5 = description5;
                    break;
                }
                else
                {
                    ViewBag.fieldValueList5 = new List<string>();
                }
            }
            foreach (var temp in reserve)
            {
                if (temp.Value == "Position")
                {
                    var description6 = (
                              from b in db.Brands
                              join q in db.BrandTemplateReserves on b.BrandId equals q.Number
                              join s in db.Staffs on b.StaffId equals s.Number
                              join p in db.ConfirmedFields on q.FieldId equals p.Id
                              where q.Number == com.BrandId && p.FieldName == "Position" && s.Number == com.StaffId
                              select new BrandTemplateViewModels
                              {

                                  Description = p.Description,
                                  Value = s.Position

                              }).ToList();
                    ViewBag.fieldValueList6 = description6;
                    break;
                }
                else
                {
                    ViewBag.fieldValueList6 = new List<string>();
                }
            }
            return View(model);

        }
           [Authorize(Roles = "Admin,Brand_Details")]
        // GET: Brand/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

           [Authorize(Roles = "Admin,Brand_Create")]
        // GET: Brand/Create
        public ActionResult Create()
        {
            SelectListItem i = new SelectListItem();
            i.Text = "-请选择-";
            i.Value = "";
            i.Selected = true;

            List<SelectListItem> itemBrandId = db.BrandTemplateModels.ToList().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Description
            }
            ).ToList();
            itemBrandId.Add(i);
            ViewBag.BrandIdList = itemBrandId;

            var staffs = (from p in db.Staffs where p.ArchiveTag == false && p.AuditStatus == 3 select p).ToList();
            List<SelectListItem> itemStaffNumber = staffs.Select(c => new SelectListItem
            {
                Value = c.Number.ToString(),
                Text = c.Name
            }
            ).ToList();
            itemStaffNumber.Add(i);
            ViewBag.StaffNumberList = itemStaffNumber;

            return View();
        }
           [Authorize(Roles = "Admin,Brand_Create")]
        // POST: Brand/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BrandId,StaffId")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                db.Brands.Add(brand);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            SelectListItem i = new SelectListItem();
            i.Text = "-请选择-";
            i.Value = "";
            i.Selected = true;

            List<SelectListItem> itemBrandId = db.BrandTemplateModels.ToList().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Description
            }
            ).ToList();
            itemBrandId.Add(i);
            ViewBag.BrandIdList = itemBrandId;

            List<SelectListItem> itemStaffNumber = db.Staffs.ToList().Select(c => new SelectListItem
            {
                Value = c.Number.ToString(),
                Text = c.Name
            }
            ).ToList();
            itemStaffNumber.Add(i);
            ViewBag.StaffNumberList = itemStaffNumber;
            return View(brand);
        }
           [Authorize(Roles = "Admin,Brand_Edit")]
        // GET: Brand/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            brand.StaffName = db.Staffs.Find(brand.StaffId).Name;
            if (brand == null)
            {
                return HttpNotFound();
            }
            ViewBag.list = GetBrandTemplates(id);
            return View(brand);
        }


        public  SelectList GetBrandTemplates(int?  BrandId)
        {
            List<BrandTemplateModels> brands = db.BrandTemplateModels.ToList();

            List<SelectListItem> list = new List<SelectListItem>();
            foreach (BrandTemplateModels tmp in brands)
            {
                SelectListItem tmpItem = new SelectListItem()
                {
                    Value = tmp.Id.ToString(),
                    Text = tmp.Description
                };
                if (tmp.Id == BrandId)
                    tmpItem.Selected = true;
                list.Add(tmpItem);
            }
            return new SelectList(list, "Value", "Text");
        }
        // POST: Brand/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
           [Authorize(Roles = "Admin,Brand_Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BrandId,StaffId")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                db.Entry(brand).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(brand);
        }
           [Authorize(Roles = "Admin,Brand_Delete")]
        // GET: Brand/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }
        [Authorize(Roles = "Admin,Brand_Delete")]
        // POST: Brand/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Brand brand = db.Brands.Find(id);
            db.Brands.Remove(brand);
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
