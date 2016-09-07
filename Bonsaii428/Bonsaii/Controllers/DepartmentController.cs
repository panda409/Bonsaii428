using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using System.IO;
using System.Text.RegularExpressions;

namespace Bonsaii.Controllers
{
    public class DepartmentController : BaseController
    {

        /*显示每个部门的员工列表 待改进*/
        public ActionResult List(string id)
        {
            List<DepartmentViewModel> q = (from d in db.Departments
                                           where d.ParentDepartmentId != this.CompanyId
                                           orderby d.DepartmentOrder
                                           join x in db.Departments on d.ParentDepartmentId equals x.DepartmentId
                                              into gc
                                           from x in gc.DefaultIfEmpty()

                                           select new DepartmentViewModel
                                           {
                                               Id = d.Id,
                                               DepartmentOrder = d.DepartmentOrder,
                                               DepartmentId = d.DepartmentId,
                                               Name = d.Name,
                                               DepartmentAbbr = d.DepartmentAbbr,
                                               Remark = d.Remark,
                                               ParentDepartmentName = x.Name,
                                               StaffSize = d.StaffSize,
                                               ParentDepartmentId=d.ParentDepartmentId
                                           }).ToList();

            List<DepartmentViewModel> q2 = (from d in db.Departments
                                            where d.ParentDepartmentId == this.CompanyId
                                            orderby d.DepartmentOrder
                                            select new DepartmentViewModel
                                            {
                                                Id = d.Id,
                                                DepartmentOrder = d.DepartmentOrder,
                                                DepartmentId = d.DepartmentId,
                                                Name = d.Name,
                                                DepartmentAbbr = d.DepartmentAbbr,
                                                Remark = d.Remark,
                                                ParentDepartmentName = this.CompanyFullName,
                                                StaffSize = d.StaffSize,
                                                ParentDepartmentId=d.ParentDepartmentId
                                            }).ToList();
            q.InsertRange(0, q2);
            List<DepartmentViewModel> temp = new List<DepartmentViewModel>();
            List<DepartmentViewModel> departments = (from item in q orderby item.DepartmentOrder select item).ToList();//所有的部门
            foreach (var item in departments) {
                if (item.ParentDepartmentId == id) //孩子部门
                {
                    temp.Add(item); 
                }
            }
            return View(temp);           
            //return View(Staffs);
        }

         [Authorize(Roles = "Admin,Department_Index")]
        // GET: Department
        public ActionResult Index()
        {
            /*左联：显示所有部门表的字段*/
            // string str = "1";
            List<DepartmentViewModel> q = (from d in db.Departments where d.ParentDepartmentId!=this.CompanyId
                    orderby d.DepartmentOrder
                    join x in db.Departments on d.ParentDepartmentId equals x.DepartmentId
                       into gc
                     from x in gc.DefaultIfEmpty()
                   
                    select new DepartmentViewModel
                    {
                        Id = d.Id,
                        DepartmentOrder = d.DepartmentOrder,
                        DepartmentId = d.DepartmentId,
                        Name = d.Name,
                        DepartmentAbbr=d.DepartmentAbbr,
                        Remark = d.Remark,
                        ParentDepartmentName = x.Name,
                        StaffSize = d.StaffSize
                    }).ToList();

            List<DepartmentViewModel> q2 = (from d in db.Departments
                                            where d.ParentDepartmentId == this.CompanyId
                                            orderby d.DepartmentOrder
                                            select new DepartmentViewModel
                                            {
                                                Id = d.Id,
                                                DepartmentOrder = d.DepartmentOrder,
                                                DepartmentId = d.DepartmentId,
                                                Name = d.Name,
                                                DepartmentAbbr = d.DepartmentAbbr,
                                                Remark = d.Remark,
                                                ParentDepartmentName = this.CompanyFullName,
                                                StaffSize = d.StaffSize
                                            }).ToList();
             q.InsertRange(0,q2);
            // q.Sort(IComparer < DepartmentViewModel > DepartmentOrder);

             List<DepartmentViewModel> department = (from item in q orderby item.DepartmentOrder select item).ToList();
             return View(department);           
        }
         public FilePathResult Download()
         {
             return File("../files/download/部门导入模板.xlsx", "application/excel", "部门导入模板.xlsx");
         }
        [HttpPost]
        [ValidateAntiForgeryToken]
         public ActionResult Index(HttpPostedFileBase file)
         {
             //List<Staff> Staffs = (from p in db.Staffs orderby p.BillNumber where p.ArchiveTag == false select p).ToList();

             //foreach (Staff tmp in Staffs)
             //{
             //    tmp.DepartmentName = (from p in db.Departments where p.DepartmentId == tmp.Department select p.Name).ToList().FirstOrDefault();
             //    tmp.AuditStatusName = db.States.Find(tmp.AuditStatus).Description;
             //}

             /*左联：显示所有部门表的字段*/
             // string str = "1";
             List<DepartmentViewModel> q = (from d in db.Departments
                                            where d.ParentDepartmentId != this.CompanyId
                                            orderby d.DepartmentOrder
                                            join x in db.Departments on d.ParentDepartmentId equals x.DepartmentId
                                               into gc
                                            from x in gc.DefaultIfEmpty()

                                            select new DepartmentViewModel
                                            {
                                                Id = d.Id,
                                                DepartmentOrder = d.DepartmentOrder,
                                                DepartmentId = d.DepartmentId,
                                                Name = d.Name,
                                                DepartmentAbbr = d.DepartmentAbbr,
                                                Remark = d.Remark,
                                                ParentDepartmentName = x.Name,
                                                StaffSize = d.StaffSize
                                            }).ToList();

             List<DepartmentViewModel> q2 = (from d in db.Departments
                                             where d.ParentDepartmentId == this.CompanyId
                                             orderby d.DepartmentOrder
                                             select new DepartmentViewModel
                                             {
                                                 Id = d.Id,
                                                 DepartmentOrder = d.DepartmentOrder,
                                                 DepartmentId = d.DepartmentId,
                                                 Name = d.Name,
                                                 DepartmentAbbr = d.DepartmentAbbr,
                                                 Remark = d.Remark,
                                                 ParentDepartmentName = this.CompanyFullName,
                                                 StaffSize = d.StaffSize
                                             }).ToList();
             q.InsertRange(0, q2);

             string alert = null;
             List<string> alertMul = new List<string>();

             if (file == null)
             {
                 alert = "没有文件！"; alertMul.Add(alert);
                 ViewBag.alertMul = alertMul;
                 return View(q);
             }
             var fileAddr = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(file.FileName));
             try
             {
                 file.SaveAs(fileAddr);
                 DataTable table = new DataTable();
                 table = RenderFromExcel(fileAddr);

                 alertMul = Validation(table);

                 if (alertMul.Count() == 0)
                 {
                     alertMul = toDataBase(table, alertMul);
                     if (alertMul.Count == 0)
                     {
                         alert = "导入成功！";
                         alertMul.Add(alert);
                     }

                     ViewBag.alertMul = alertMul;
                 }
                 else
                 {
                     ViewBag.alertMul = alertMul;
                     return View(q);
                 }

                 return View(q);
             }
             catch (Exception e)
             {

                 alert = "上传异常！"; alertMul.Add(alert);
                 ViewBag.alertMul = alertMul;

                 return View(q);
             }
         }
         public List<string> Validation(DataTable table)
         {
             string alert = null;
             List<string> alertMsg = new List<string>();
             var departmentName = (from d in db.Departments
                                   select d.Name).ToList();
           
             for (int i = 1; i < table.Rows.Count; i++)
             {
                 if (table.Rows[i][0].ToString() == "")
                 {
                     alert = "第" + i + "行部门名称不能为空！";
                     alertMsg.Add(alert);
                 }
                
                 Regex depRegex = new Regex("^[0-9]*[1-9][0-9]*$");
                 if (table.Rows[i][1].ToString()== "")
                 {
                     table.Rows[i][1] = 99;
                 }
                 else
                 {
                     if (depRegex.IsMatch(table.Rows[i][1].ToString()) == false)
                     {
                         alert = "第" + i + "行部门顺序必须是正整数！"; alertMsg.Add(alert);
                     }
                 }
                
                 if (table.Rows[i][2].ToString()== "")
                 {
                     alert = "第" + i + "行编制人数不能为空！"; alertMsg.Add(alert);
                 }
                 else
                 {
                     if (depRegex.IsMatch(table.Rows[i][2].ToString()) == false)
                     {
                         alert = "第" + i + "行编制人数必须为整数！"; alertMsg.Add(alert);
                     }
                 }
                 if (table.Rows[i][3].ToString()== "")
                 {
                     alert = "第" + i + "行部门简写不能为空！"; alertMsg.Add(alert);
                 }
             }
             return alertMsg;
         }
         public List<string> toDataBase(DataTable table, List<string> alertMul)
         {
             string alert = null;
             try
             {
                 for (int i = 1; i < table.Rows.Count; i++)
                 {
                     Department dept = new Department();
                     dept.Name = table.Rows[i][0].ToString();
                     dept.DepartmentId = GetRandomCode(50);
                     //int id = table.Rows[i][1].ToString();
                     int id = Convert.ToInt32(table.Rows[i][1]);
                     dept.DepartmentOrder = id;
                   
                     dept.ParentDepartmentId = this.CompanyId;
                     int ss = Convert.ToInt32(table.Rows[i][2]);
                     dept.StaffSize = ss;
                     dept.DepartmentAbbr = table.Rows[i][3].ToString();
                     dept.Remark = table.Rows[i][4].ToString();

                     DateTime dt = DateTime.Now;
                     dept.RecordTime = dt;
                     dept.RecordPerson = this.UserName;

                     db.Departments.Add(dept);
                 }
                // alert += null;
                 if (alertMul.Count == 0)
                 {
                     db.SaveChanges();
                 }
                 return alertMul;

             }
             catch (Exception e)
             {
                 alert = "数据导入错误！";
                 alertMul.Add(alert);
                 return alertMul;
             }
         }
          [Authorize(Roles = "Admin,Department_Details")]
        // GET: Department/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //左联：查询上级部门的名称
            var q = from d in db.Departments
                    join x in db.Departments on d.ParentDepartmentId equals x.DepartmentId
                        into gc
                    from x in gc.DefaultIfEmpty()
                    where d.Id == id
                    select new { Name = x.Name };
            Department department = db.Departments.Find(id);
            DepartmentViewModel qq = new DepartmentViewModel();

            /*Step1：部门表的固定字段*/
            if (q != null)
            {
                foreach (var temp in q)
                {
                    qq.Id = department.Id;
                    qq.DepartmentOrder = department.DepartmentOrder;
                    qq.DepartmentId = department.DepartmentId;
                    qq.Name = department.Name;
                    qq.ParentDepartmentName = temp.Name;
                    qq.DepartmentAbbr = department.DepartmentAbbr;
                    qq.StaffSize = department.StaffSize;
                    qq.Remark = department.Remark;
                }
            }
            else
            {
                qq.Id = department.Id;
                qq.DepartmentOrder = department.DepartmentOrder;
                qq.DepartmentId = department.DepartmentId;
                qq.Name = department.Name;
                qq.ParentDepartmentName = null;
                qq.DepartmentAbbr = department.DepartmentAbbr;
                qq.StaffSize = department.StaffSize;
                qq.Remark = department.Remark;
            }
            /*Step2：查找部门表预留字段*/
            var p = (from df in db.DepartmentReserves
                     join rf in db.ReserveFields on df.FieldId equals rf.Id
                     where df.Number == id && rf.Status == true
                     select new DepartmentViewModel {Id= df.Number,Description = rf.Description, Value = df.Value }).ToList();
            ViewBag.List = p;

            if (qq == null)
            {
                return HttpNotFound();
            }
            return View(qq);

        }

        [HttpPost]
        public JsonResult DepartmentSearch()
        {
            try
            {
                var items = (from d in db.Departments orderby d.DepartmentOrder

                             select new
                             {
                                 text = d.Name,
                                 id = d.DepartmentId
                             }).ToList();

                return Json(items);
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }
        }
        [HttpPost]
        public JsonResult DepartmentAndCompanySearch()
        {
            try
            {
                var items = (from d in db.Departments
                             orderby d.DepartmentOrder

                             select new
                             {
                                 text = d.Name,
                                 id = d.DepartmentId
                             }).ToList();
                var company =
                new
                {
                    text = this.CompanyFullName,
                    id = this.CompanyId
                };
                items.Insert(0, company);

                return Json(items);
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }
        }

        [HttpPost]
        public JsonResult DepartmentSearchOther(string id)
        {
            try
            {
             //  var item = from p in db.Departments where p.DepartmentId == id select 
                var items = (from d in db.Departments where d.DepartmentId != id
                             select new
                             {
                                 text = d.Name,
                                 id = d.DepartmentId
                             }).ToList();

                return Json(items);
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }
        }


          [Authorize(Roles = "Admin,Department_Create")]
        // GET: Department/Create
        public ActionResult Create()
        {
            ////实现下拉列表
            //List<SelectListItem> item = db.Departments.ToList().Select(c => new SelectListItem
            //{
            //    Value = c.DepartmentId,//保存的值
            //    Text = c.Name//显示的值
            //}).ToList();

            ////增加一个null选项
            //SelectListItem i = new SelectListItem();
            //i.Value = "";
            //i.Text = "-请选择-";
            //i.Selected = true;
            //item.Add(i);

            ////传值到页面
            //ViewBag.List = item;

            /*查找预留字段表，然后获取一个列表*/
            var recordList = (from p in db.ReserveFields
                              join q in db.TableNameContrasts
                                 on p.TableNameId equals q.Id 
                              where q.TableName == "Departments" && p.Status == true
                              select p).ToList();
            ViewBag.recordList = recordList;

            return View();
        }



          public string GetRandomCode(int numlen)
          {
              char[] chars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
              string code = string.Empty;

              for (int i = 0; i < numlen; i++)
              {
                  //这里是关键，传入一个seed参数即可保证生成的随机数不同            
                  //Random rnd = new Random(unchecked((int)DateTime.Now.Ticks));
                  Random rnd = new Random(GetRandomSeed());
                  code += chars[rnd.Next(0, 9)].ToString();
              }

              return code;
          }
          /// <summary>
          /// 加密随机数生成器 生成随机种子
          /// </summary>
          /// <returns></returns>
          static int GetRandomSeed()
          {

              byte[] bytes = new byte[4];

              System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();

              rng.GetBytes(bytes);

              return BitConverter.ToInt32(bytes, 0);

          }


          [Authorize(Roles = "Admin,Department_Create")]
        // POST: Department/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department department)
        {
            //传值到页面
           // ViewBag.List = item;
            if (ModelState.IsValid)
            {


                //ModelState.AddModelError("","error");
                    /*Step1：如果上级部门为空则上级部门编号为公司Id*/
                   // if (department.ParentDepartmentId == null) { department.ParentDepartmentId = this.CompanyId; }

                    department.DepartmentId = GetRandomCode(50);
                    department.DepartmentOrder = 99;
                    department.RecordTime = DateTime.Now;
                    department.RecordPerson = this.UserName;

                    /*Step3：保存固定字段（为了生成主键Id）*/
                    db.Departments.Add(department);
                    db.SaveChanges();

                    /*Step4：显示预留字段名称*/
                    var recordList2 = (from p in db.ReserveFields
                                       join q in db.TableNameContrasts
                                     on p.TableNameId equals q.Id 
                                       where q.TableName == "Departments" && p.Status == true
                                       select p).ToList();
                    ViewBag.recordList2 = recordList2;

                    /*Step5：保存预留字段的值*/
                    foreach (var temp in recordList2)
                    {
                        DepartmentReserve dr = new DepartmentReserve();
                        dr.Number = department.Id;
                        dr.FieldId = temp.Id;
                        dr.Value = Request[temp.FieldName];
                        /*占位，为了在Index中显示整齐的格式*/
                        if (dr.Value == null) dr.Value = " ";
                        db.DepartmentReserves.Add(dr);
                        db.SaveChanges();
                    }
                   
                   //修改其他的自定义字段的列表

                    return RedirectToAction("Index");
                }
            var recordList1 = (from p in db.ReserveFields
                              join q in db.TableNameContrasts
                                 on p.TableNameId equals q.Id
                              where q.TableName == "Departments" && p.Status == true
                              select p).ToList();
            ViewBag.recordList = recordList1;
            return View(department);
        }

          [Authorize(Roles = "Admin,Department_Edit")]
        // GET: Department/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);

           //Department temp =  db.Departments.Where(c => c.DepartmentId.Equals(department.ParentDepartmentId)).SingleOrDefault();
          
           //department.ParentDepartmentName = temp.Name;

            //实现下拉列表
           var item = db.Departments.Where(p =>p.DepartmentId != department.DepartmentId).ToList().Select(c => new SelectListItem
            {
              Value = c.DepartmentId,//保存的值
               Text = c.Name//显示的值
            }).ToList();

      
            SelectListItem company = new SelectListItem();
            company.Value = this.CompanyId;
            company.Text = this.CompanyFullName;
            company.Selected = true;

            item.Insert(0, company);
          
            //传值到页面
            ViewBag.List = item;

            //DepartmentViewModel显示部门信息（部门表变化的字段）
            var pp = (from df in db.DepartmentReserves
                      join rf in db.ReserveFields on df.FieldId equals rf.Id
                      where df.Number == id && rf.Status == true
                      select new DepartmentViewModel { Description = rf.Description, Value = df.Value }).ToList();
            ViewBag.ValueList = pp;

            if (department == null)
            {
                return HttpNotFound();
            }

            return View(department);

        }
          [Authorize(Roles = "Admin,Department_Index")]
        // POST: Department/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Department department)
        {
            //实现下拉列表
            var item = db.Departments.Where(p => p.DepartmentId != department.DepartmentId).OrderBy(p=>p.DepartmentOrder).ToList().Select(c => new SelectListItem
            {
                Value = c.DepartmentId,//保存的值
                Text = c.Name//显示的值
            }).ToList();

       
            SelectListItem company = new SelectListItem();
            company.Value = this.CompanyId;
            company.Text = this.CompanyFullName;
            company.Selected = true;

            item.Insert(0, company);
       

            //传值到页面
            ViewBag.List = item;

            //如果公司的上级部门编号ParentDepartmentId为空，将它置为null
            if (department.ParentDepartmentId == "") department.ParentDepartmentId = this.CompanyId;

            //模型状态错误（为空）
            if (ModelState.IsValid)
            {
                Department d = db.Departments.Find(department.Id);
                if (d != null)
                {
                   
                    // 得到部门department.Number对应的所有动态变化的字段
                    var pp = (from df in db.DepartmentReserves
                              join rf in db.ReserveFields on df.FieldId equals rf.Id
                              where df.Number == department.Id && rf.Status == true
                              select new DepartmentViewModel { Id = df.Id, Description = rf.Description, Value = df.Value }).ToList();
                    ViewBag.ValueList = pp;

                    //该部门的孩子部门
                    var childrens = (from p in db.Departments where p.ParentDepartmentId == department.DepartmentId select p).ToList();
                    foreach (var children in childrens)
                    {
                        if (department.ParentDepartmentId == children.DepartmentId)
                        {
                            ViewBag.errorMessage = true;
                            //ModelState.AddModelError("", "该父级部门已经是子部门!");
                            return View(department);
                        }
                    }

                    //对每个动态变化的字段进行赋值
                    foreach (var temp in pp)
                    {
                        DepartmentReserve dr = db.DepartmentReserves.Find(temp.Id);
                        dr.Value = Request[temp.Description];
                        db.SaveChanges();
                    }

                    d.DepartmentOrder = department.DepartmentOrder;
                    d.ChangeTime = DateTime.Now;
                    d.ChangePerson = this.UserName;
                    d.DepartmentAbbr = department.DepartmentAbbr;
                    d.Name = department.Name;
                    d.DepartmentId = department.DepartmentId;
                    d.StaffSize = department.StaffSize;
                    d.ParentDepartmentId = department.ParentDepartmentId;
                    d.Remark = department.Remark;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                //自带的ValidationSummary提示
                ModelState.AddModelError("", "修改失败");
            }
            //DepartmentViewModel显示部门信息（部门表变化的字段）
            var pp1 = (from df in db.DepartmentReserves
                       join rf in db.ReserveFields on df.FieldId equals rf.Id
                       where df.Number == department.Id && rf.Status == true
                       select new DepartmentViewModel { Id = df.Id, Description = rf.Description, Value = df.Value }).ToList();
            ViewBag.ValueList = pp1;
            return View(department);

        }
           [Authorize(Roles = "Admin,Department_Delete")]
        // GET: Department/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //左联：查询上级部门的名称
            var q = from d in db.Departments
                    join x in db.Departments on d.ParentDepartmentId equals x.DepartmentId
                        into gc
                    from x in gc.DefaultIfEmpty()
                    where d.Id == id
                    select new { Name = x.Name };
            Department department = db.Departments.Find(id);
            DepartmentViewModel qq = new DepartmentViewModel();
            //DepartmentViewModel显示部门信息（部门表的固定字段）
            if (q != null)
            {
                foreach (var temp in q)
                {
                    qq.DepartmentOrder = department.DepartmentOrder;
                    qq.DepartmentId = department.DepartmentId;
                    qq.Name = department.Name;
                    qq.ParentDepartmentName = temp.Name;
                    qq.StaffSize = department.StaffSize;
                    qq.Remark = department.Remark;
                    qq.DepartmentAbbr = department.DepartmentAbbr;
                }
            }
            else
            {
                qq.DepartmentOrder = department.DepartmentOrder;
                qq.DepartmentId = department.DepartmentId;
                qq.Name = department.Name;
                qq.ParentDepartmentName = null;
                qq.StaffSize = department.StaffSize;
                qq.Remark = department.Remark;
                qq.DepartmentAbbr = department.DepartmentAbbr;
            }
            //DepartmentViewModel显示部门信息（部门表变化的字段）
            var pp = (from df in db.DepartmentReserves
                     join rf in db.ReserveFields on df.FieldId equals rf.Id
                     where df.Number == id && rf.Status == true
                     select new DepartmentViewModel { Description = rf.Description, Value = df.Value }).ToList();
            ViewBag.List = pp;
            if (department == null)
            {
                return HttpNotFound();
            }
            if (qq == null)
            {
                return HttpNotFound();
            }
            //该部门的孩子部门
            var childrens = (from p in db.Departments where p.ParentDepartmentId == department.DepartmentId select p).ToList();
            //该部门的员工
            var staffs = (from p in db.Staffs where p.Department == department.DepartmentId select p).ToList();

            if (childrens.Count != 0 || staffs.Count != 0)
            {
                ViewBag.stopDelete = 1;
                return View(qq);
            }
            else {
               ViewBag.stopDelete = 2;
               return View(qq);
            }
         }
        [Authorize(Roles = "Admin,Department_Delete")]
     //   POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
       {
            ViewBag.stopDelete = 0;
            /*Step1：删除预留字段*/
            // 由于主外键关系，Departments是主表，DepartmentReserves是引用Departments表的信息。
            //只有先删除对应DepartmentReserve的动态变化的字段的信息
            var item = (from dr in db.DepartmentReserves
                        where dr.Number == id
                        select new DepartmentViewModel { Id = dr.Id }).ToList();
            foreach (var temp in item)
            {
                DepartmentReserve drs = db.DepartmentReserves.Find(temp.Id);
                db.DepartmentReserves.Remove(drs);
                db.SaveChanges();
            }
            //db.SaveChanges();
            /*Step2：删除固定字段*/
            //删除Departments表对应的信息
            Department department = db.Departments.Find(id);
            db.Departments.Remove(department);
            db.SaveChanges();

            return RedirectToAction("Index");
       }

        ///*判断部门编号是否唯一*/
        //[HttpPost]
        //public JsonResult DepartIdTest(string departmentId)
        //{
        //    if (!String.IsNullOrEmpty(departmentId))
        //    {
        //        var find = (from p in db.Departments where p.DepartmentId == departmentId select p).ToList();
        //        if (find.Count != 0)
        //        {
        //            return Json(new { result = true, });
        //        }
        //        else { return Json(new { result = false, }); }
        //    }
        //   else
        //       return null;
        //}

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
