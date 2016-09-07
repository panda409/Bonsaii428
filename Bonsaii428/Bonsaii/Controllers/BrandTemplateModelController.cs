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
    public class BrandTemplateModelController : BaseController
    {
        [Authorize(Roles = "Admin,BrandTemplateModel_BTemplateReserveIndex")]
        /*节点列表*/
        public ActionResult BTemplateReserveIndex(int? id)
        {
            List<BrandTemplateReserve> list = db.BrandTemplateReserves.Where(p => p.Number == id).ToList();
            foreach (BrandTemplateReserve tmp in list)
            {
                tmp.FieldDescription = db.ConfirmedFields.Find(tmp.FieldId).Description;
            }
            ViewBag.TemplateId = id;

            return View(list);
        }
        [Authorize(Roles = "Admin,BrandTemplateModel_BTemplateReserveDetails")]
        /*删除节点*/
        public ActionResult BTemplateReserveDetails(int? id1, int? id2)
        {
            ViewBag.TemplateId = id2;
            if (id1 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BrandTemplateReserve brandTemplateReserve = db.BrandTemplateReserves.Find(id1);
            if (brandTemplateReserve == null)
            {
                return HttpNotFound();
            }

            brandTemplateReserve.FieldDescription = db.ConfirmedFields.Find(brandTemplateReserve.FieldId).Description;

            return View(brandTemplateReserve);
        }
        //[HttpPost, ActionName("BTemplateReserveDetails")]
        //[ValidateAntiForgeryToken]
        //public ActionResult BTemplateReserveDetails(int id1, int id2)
        //{
        //    BrandTemplateReserve brandTemplateReserve = db.BrandTemplateReserves.Find(id1);
        //    db.BrandTemplateReserves.Remove(brandTemplateReserve);
        //    db.SaveChanges();
        //    return RedirectToActionPermanent("BTemplateReserveIndex", "BrandTemplateModel", new { id = id2 });
        //}
        [Authorize(Roles = "Admin,BrandTemplateModel_BTemplateReserveCreate")]
       /*创建节点*/
        public ActionResult BTemplateReserveCreate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.TemplateId = id;

            //实现下拉列表(字段)。。。。已选择的就不用显示了
            List<SelectListItem> item2 = db.ConfirmedFields.ToList().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),//保存的值
                Text = c.Description//显示的值
            }).ToList();

            //增加一个null选项
            SelectListItem i2 = new SelectListItem();
            i2.Value = "";
            i2.Text = "-请选择-";
            i2.Selected = true;
            item2.Add(i2);

            //传值到页面
            ViewBag.List2 = item2;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BTemplateReserveCreate([Bind(Include = "Id,FieldId,Number,Value")] BrandTemplateReserve brandTemplateReserve)
        {
            if (ModelState.IsValid)
            {
                int id = int.Parse(Request["BrandTemplateId"]);
                brandTemplateReserve.Number = id;
                db.BrandTemplateReserves.Add(brandTemplateReserve);
                db.SaveChanges();
                return RedirectToActionPermanent("BTemplateReserveIndex", "BrandTemplateModel", new { id = id });
            }
            //实现下拉列表(字段)。。。。已选择的就不用显示了
            List<SelectListItem> item2 = db.ConfirmedFields.ToList().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),//保存的值
                Text = c.Description//显示的值
            }).ToList();

            //增加一个null选项
            SelectListItem i2 = new SelectListItem();
            i2.Value = "";
            i2.Text = "-请选择-";
            i2.Selected = true;
            item2.Add(i2);

            //传值到页面
            ViewBag.List2 = item2;
            return View(brandTemplateReserve);
        }

        [Authorize(Roles = "Admin,BrandTemplateModel_BTemplateReserveEdit")]
        // GET: BrandTemplateModel/Edit/5
        public ActionResult BTemplateReserveEdit(int? id1,int? id2)
        {
            if (id1 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BrandTemplateReserve brandTemplateReserve = db.BrandTemplateReserves.Find(id1);
            if (brandTemplateReserve == null)
            {
                return HttpNotFound();
            }
            ViewBag.TemplateId = id2;
            //实现下拉列表(字段)。。。。已选择的就不用显示了
            List<SelectListItem> item2 = db.ConfirmedFields.ToList().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),//保存的值
                Text = c.Description//显示的值
            }).ToList();

            //增加一个null选项
            SelectListItem i2 = new SelectListItem();
            i2.Value = "";
            i2.Text = "-请选择-";
            i2.Selected = true;
            item2.Add(i2);

            //传值到页面
            ViewBag.List2 = item2;
            return View(brandTemplateReserve);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BTemplateReserveEdit(BrandTemplateReserve brandTemplateReserve)
        {
            if (ModelState.IsValid)
            {
                int id = int.Parse(Request["BrandTemplateId"]);
                db.Entry(brandTemplateReserve).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToActionPermanent("BTemplateReserveIndex", "BrandTemplateModel", new { id = id });
            }
            //实现下拉列表(字段)。。。。已选择的就不用显示了
            List<SelectListItem> item2 = db.ConfirmedFields.ToList().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),//保存的值
                Text = c.Description//显示的值
            }).ToList();

            //增加一个null选项
            SelectListItem i2 = new SelectListItem();
            i2.Value = "";
            i2.Text = "-请选择-";
            i2.Selected = true;
            item2.Add(i2);

            //传值到页面
            ViewBag.List2 = item2;
            return View(brandTemplateReserve);
        }
        [Authorize(Roles = "Admin,BrandTemplateModel_BTemplateReserveDelete")]
        /*删除节点*/ 
        public ActionResult BTemplateReserveDelete(int? id1,int? id2)
        {
            ViewBag.TemplateId = id2;
            if (id1 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BrandTemplateReserve brandTemplateReserve = db.BrandTemplateReserves.Find(id1);
            if (brandTemplateReserve == null)
            {
                return HttpNotFound();
            }

            brandTemplateReserve.FieldDescription = db.ConfirmedFields.Find(brandTemplateReserve.FieldId).Description;

            return View(brandTemplateReserve);
        }
        [HttpPost, ActionName("BTemplateReserveDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult BTemplateReserveDeleteConfirmed(int id1,int id2)
        {
            BrandTemplateReserve brandTemplateReserve = db.BrandTemplateReserves.Find(id1);
            db.BrandTemplateReserves.Remove(brandTemplateReserve);
            db.SaveChanges();
            return RedirectToActionPermanent("BTemplateReserveIndex", "BrandTemplateModel", new { id = id2 });
        }
        [Authorize(Roles = "Admin,BrandTemplateModel_Index")]
        // GET: BrandTemplateModel
        public ActionResult Index()
        {
            return View(db.BrandTemplateModels.ToList());
        }

        // GET: BrandTemplateModel/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BrandTemplateModels brandTemplateModels = db.BrandTemplateModels.Find(id);
            if (brandTemplateModels == null)
            {
                return HttpNotFound();
            }
            return View(brandTemplateModels);
        }
        [Authorize(Roles = "Admin,BrandTemplateModel_Create")]
        // GET: BrandTemplateModel/Create
        public ActionResult Create()
        {
            return View();
        }

        /*获取图片*/
        public FileContentResult GetImage(int id)
        {
            // BrandTemplateModels info = new BrandTemplateModels();
            BrandTemplateModels info = db.BrandTemplateModels.FirstOrDefault(p => p.Id == id);

            if (info != null)
            {

                return File(info.BrandTemplate, info.BrandTemplateType);//File方法直接将二进制转化为指定类型了。
            }
            else
            {
                return null;
            }
        }

        // POST: BrandTemplateModel/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BrandTemplateModels brandTemplateModels, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                db.BrandTemplateModels.Add(brandTemplateModels);

                if (image != null)
                {
                    brandTemplateModels.BrandTemplateType = image.ContentType;
                    brandTemplateModels.BrandTemplate = new byte[image.ContentLength];
                    image.InputStream.Read(brandTemplateModels.BrandTemplate, 0, image.ContentLength);

                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(brandTemplateModels);
        }
        [Authorize(Roles = "Admin,BrandTemplateModel_Edit")]
        // GET: BrandTemplateModel/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BrandTemplateModels brandTemplateModels = db.BrandTemplateModels.Find(id);
            if (brandTemplateModels == null)
            {
                return HttpNotFound();
            }
            return View(brandTemplateModels);
        }

        // POST: BrandTemplateModel/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BrandTemplateModels brandTemplateModels,HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    brandTemplateModels.BrandTemplateType = image.ContentType;
                    brandTemplateModels.BrandTemplate = new byte[image.ContentLength];
                    image.InputStream.Read(brandTemplateModels.BrandTemplate, 0, image.ContentLength);

                }
                db.Entry(brandTemplateModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(brandTemplateModels);
        }
        [Authorize(Roles = "Admin,BrandTemplateModel_Delete")]
        // GET: BrandTemplateModel/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BrandTemplateModels brandTemplateModels = db.BrandTemplateModels.Find(id);
            if (brandTemplateModels == null)
            {
                return HttpNotFound();
            }
            return View(brandTemplateModels);
        }

        // POST: BrandTemplateModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var shits = (from p in db.BrandTemplateReserves where p.Number == id select p).ToList();
            foreach (var shit in shits)
            {
                db.BrandTemplateReserves.Remove(shit);

            }
            db.SaveChanges();

            BrandTemplateModels brandTemplateModels = db.BrandTemplateModels.Find(id);
            db.BrandTemplateModels.Remove(brandTemplateModels);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin,BrandTemplateModel_BrandIndex")]
        // GET: Brand
        public ActionResult BrandIndex(int id)
        {
            List<Brand> list = (from p in db.Brands 
                                where p.BrandId == id
                                select p).ToList();

            foreach (var temp in list)
            {
                temp.BrandName = db.BrandTemplateModels.Find(temp.BrandId).Description;
                temp.StaffName = db.Staffs.Find(temp.StaffId).Name;
            }

            ViewBag.BrandId = id;//传到视图；
            

            return View(list);
        }


        [Authorize(Roles = "Admin,BrandTemplateModel_BrandCreate")]
        // GET: Brand/Create
        public ActionResult BrandCreate(int? id)
        {
            ViewBag.BrandId = id;//传到视图；


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
        [Authorize(Roles = "Admin,BrandTemplateModel_BrandCreate")]
        // POST: Brand/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BrandCreate(Brand brand)
        {
            if (ModelState.IsValid)
            {
                int id = int.Parse(Request["BrandId"]);
                brand.BrandId = id;
                db.Brands.Add(brand);
                db.SaveChanges();
                return RedirectToAction("BrandIndex", "BrandTemplateModel", new { id = id });
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

            var staffs = (from p in db.Staffs where p.ArchiveTag == false && p.AuditStatus == 3 select p).ToList();

            List<SelectListItem> itemStaffNumber = staffs.Select(c => new SelectListItem
            {
                Value = c.Number.ToString(),
                Text = c.Name
            }
            ).ToList();
            itemStaffNumber.Add(i);
            ViewBag.StaffNumberList = itemStaffNumber;
            return View(brand);
        }
        [Authorize(Roles = "Admin,BrandTemplateModel_BrandDelete")]
        // GET: Brand/Delete/5
        public ActionResult BrandDelete(int? id,int? id2)
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
        [Authorize(Roles = "Admin,BrandTemplateModel_BrandDelete")]
        // POST: Brand/Delete/5
        [HttpPost, ActionName("BrandDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult BrandDeleteConfirmed(FormCollection collection)
        {
            Brand brand = db.Brands.Find(int.Parse(collection["Id"]));
            db.Brands.Remove(brand);
            db.SaveChanges();
            return RedirectToAction("BrandIndex", "BrandTemplateModel", new { id = int.Parse(collection["BrandId"]) });
        }

        public ActionResult Print(int id,int id2)
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
