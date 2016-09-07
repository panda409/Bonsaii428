using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using Bonsaii.Models.GlobalStaticVaribles;
using System.IO;
using OfficeOpenXml;
using System.Data.SqlClient;

namespace Bonsaii.Controllers
{
    public class DataSubscriptionsController : BaseController
    {

        // GET: DataSubscriptions
        public ActionResult Index()
        {
            return View(db.DataSubscriptions.ToList());
        }

        // GET: DataSubscriptions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataSubscriptions dataSubscriptions = db.DataSubscriptions.Find(id);
            if (dataSubscriptions == null)
            {
                return HttpNotFound();
            }
            return View(dataSubscriptions);
        }

        // GET: DataSubscriptions/Create
        public ActionResult Create()
        {
            ViewBag.List = CirculateMethod.GetCirculateMethod();
            return View();
        }

        // POST: DataSubscriptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /**
         * 参数中如果是[Bind(Include="") DataSubscriptions dataSubscriptions)的形式，含义其实是绑定某些视图中的值到模型中，并将这些值传入到控制器当中
         * */
        public ActionResult Create(DataSubscriptions dataSubscriptions)
        {
            /**
             * 要注意的是：ModelState.IsValid的真假，是和model相关的，model中对属性的要求（非空、长度等等）必须和View中传过来一致
             * 如果model中定义某个属性非空，但是在View中绑定的model中压根没有这个属性的值，ModelState.IsValid一定是为假的!
             * */
            dataSubscriptions.BillCode = "9999999999";
            dataSubscriptions.BillType = "0008";
            dataSubscriptions.NextSendDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.DataSubscriptions.Add(dataSubscriptions);
                db.SaveChanges();

                HttpPostedFileBase file = Request.Files["file"];
                if (file != null)
                {
                    string fileName = Path.Combine(Request.MapPath("~/files/tmp"), Path.GetFileName(file.FileName));
                    file.SaveAs(fileName);
                    using (ExcelPackage package = new ExcelPackage(new FileInfo(fileName)))
                    {
                        //注意！所有的起始位置都是1！所有的结束位置都是集合的元素长度值
                        ExcelWorksheet sheet = package.Workbook.Worksheets[1];

                        int colCount = sheet.Dimension.End.Column;
                        int rowCount = sheet.Dimension.End.Row;

                        for (ushort i = 2; i <= rowCount; i++)
                        {
                            BillStaffMapping mapping = new BillStaffMapping
                            {
                                BillType = dataSubscriptions.BillType,
                                BillNumber = dataSubscriptions.BillCode,
                                StaffNumber = sheet.GetValue(i,1).ToString(),    //从外部Excel表格获取员工工号
                                TelPhone = sheet.GetValue(i,3).ToString(),
                                Email = sheet.GetValue(i,4).ToString()
                            };
                            db.BillStaffMappings.Add(mapping);
                            db.SaveChanges();
                        }

                        if (!System.IO.File.Exists(fileName))
                            System.IO.File.Delete(fileName);
                    }
                }
            //    this.UploadFile(Request["Receiver"]);
                return RedirectToAction("Index");
            }

            return View(dataSubscriptions);
        }

        public void UploadFile(string file)
        {
            var tc = Request.Files;
            string tmp = file;
        }


        // GET: DataSubscriptions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataSubscriptions dataSubscriptions = db.DataSubscriptions.Find(id);
            if (dataSubscriptions == null)
            {
                return HttpNotFound();
            }
            return View(dataSubscriptions);
        }

        // POST: DataSubscriptions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DataSubscriptions dataSubscriptions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dataSubscriptions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dataSubscriptions);
        }

        // GET: DataSubscriptions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataSubscriptions dataSubscriptions = db.DataSubscriptions.Find(id);
            if (dataSubscriptions == null)
            {
                return HttpNotFound();
            }
            return View(dataSubscriptions);
        }

        // POST: DataSubscriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DataSubscriptions dataSubscriptions = db.DataSubscriptions.Find(id);
            List<BillStaffMapping> tmp = db.BillStaffMappings.Where(p => p.BillType == dataSubscriptions.BillType && p.BillNumber == dataSubscriptions.BillCode).ToList();
            db.BillStaffMappings.RemoveRange(tmp);
            db.DataSubscriptions.Remove(dataSubscriptions);

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


        public ActionResult ShowStaffs()
        {
            return View();
        }




        /// <summary>
        /// 根据传入的部门Id，递归的调用并返回部门结构的树状结构
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <param name="connString">企业的连接字符串</param>
        /// <returns>部门的树状结构</returns>
        public List<QTree> GetDepartments2(string id,string connString)
        {
            List<Department> deList = db.Departments.Where(p => p.ParentDepartmentId.Equals(id)).ToList();
            if (deList.Count == 0)
                return null;
            List<QTree> treeList = new List<QTree>();
            foreach(Department tmp in deList){
                treeList.Add(new QTree()
                {
                    id = tmp.DepartmentId,
                    text = tmp.Name,
                    check = false,
                    children = GetDepartments2(tmp.DepartmentId, connString)
                });
            }
            return treeList;
        }


        public List<QTree> GetDepartments(string id, string connString)
        {
            List<Department> deList = db.Departments.Where(p => p.ParentDepartmentId.Equals(id)).ToList();
            if (deList.Count == 0)
                return null;
            List<QTree> treeList = new List<QTree>();
            foreach (Department tmp in deList)
            {
                //获取属于该部门的员工List
                // 员工信息显示在树状图的下面，放在该部门的子部门下面
      //          List<QTree> staffs = this.GetStaffByDepartmentId(tmp.DepartmentId, base.ConnectionString);
                //获取该部门的所有子部门树状结构List
                //子部门显示在树状图的上面
                List<QTree> childs = GetDepartments(tmp.DepartmentId, connString);            

                //注意可能某一个部门没有员工！所以要进行判断
                //if (staffs != null)
                //    childs.InsertRange(0,staffs);

                treeList.Add(new QTree()
                {
                    id = tmp.DepartmentId,
                    text = "部门-" + tmp.Name,
                    check = false,
                    children = childs
                });
            }
            return treeList;
        }
        /// <summary>
        /// 根据部门ID获取部门的所有员工的List
        /// </summary>
        /// <param name="id">部门Id</param>
        /// <param name="connString">公司的连接字符串</param>
        /// <returns>封装好的员工的QTree List</returns>
        public List<QTree> GetStaffByDepartmentId(string id, string connString)
        {
            List<Staff> staffs = db.Staffs.Where(p => p.Department.Equals(id)).ToList();
            if (staffs.Count == 0)
                return null;
            List<QTree> treeList = new List<QTree>();
            foreach (Staff tmp in staffs)
            {
                treeList.Add(new QTree()
                {
                    id = tmp.StaffNumber,
                    text = tmp.Number + "-" + tmp.Name,
                    check = false,
                    children = null
                });
            }
            return treeList;
        }
        public JsonResult tree()
        {
            QTree root = new QTree()
            {
                id = base.CompanyId,
                url = null,
                text = "公司",
                children = GetDepartments(base.CompanyId, base.ConnectionString)
            };
            return Json(new
            {
                success = true,
                msg = "Get the Department tree structure",
                type = "Tree Structure",
                obj = root
            });
        }
        public ActionResult  Tc(){
            GetStaffByDepartmentId("1", base.ConnectionString);
            string str = "hello";
            return View();
        }
        public class QTree
        {
            public string id;
            public String url;
            public String text;
            public bool check;
            public List<QTree> children;
        }


        /// <summary>
        /// 根据单据性质和单号，查询相应预警SQL的结果
        /// </summary>
        /// <param name="BillType">单据性质</param>
        /// <param name="BillNumber">单号</param>
        /// <returns>暂时返回的是一个SqlReader形式的结果集</returns>
        public ActionResult TestSQL(string BillType,string BillNumber)
        {
        //    DataSubscriptions sub = db.DataSubscriptions.Find(14);
            DataSubscriptions sub = db.DataSubscriptions.Where(p => p.BillType.Equals(BillType) && p.BillCode.Equals(BillNumber)).Single();
            string ConnString = base.ConnectionString;
            SqlConnection myConn = new SqlConnection(ConnString);
            SqlCommand myCommand = new SqlCommand(sub.SQL, myConn);
            try
            {
                myConn.Open();
                SqlDataReader reader = myCommand.ExecuteReader();
                while (reader.Read())
                {
                    string str = reader["Name"].ToString();
                }
            }
            catch (System.Exception ex)
            {
                return null;
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            return View();
        }
    }
}
