using Bonsaii.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bonsaii.Controllers
{
    public class WeekTagController : BaseController
    {
        // private BonsaiiDbContext db = new BonsaiiDbContext();
        // GET: WeekTag
        [Authorize(Roles = "Admin,WeekTag_Index")]
        public ActionResult Index()
        {

            return View();
        }
        [Authorize(Roles = "Admin,WeekTag_Index")]
        public ActionResult IndexDepartment()
        {
            var department = (from wt in db.WeekTags
                              join d in db.Departments on wt.Range equals d.DepartmentId
                              select new StaffViewModel { Number = wt.Id, Value = d.Name, Department = d.DepartmentId }).ToList();
            ViewBag.departmentList = department;
            return View(db.WeekTags.ToList());
        }
         [Authorize(Roles = "Admin,WeekTag_Index")]
        public ActionResult IndexStaff()
        {
            var staff = (from wt in db.WeekTags
                         join s in db.Staffs on wt.Range equals s.StaffNumber
                         select new StaffViewModel { Number = wt.Id, StaffNumber = s.StaffNumber, Name = s.Name }).ToList();
            ViewBag.staffList = staff;
            return View(db.WeekTags.ToList());
        }
         [Authorize(Roles = "Admin,WeekTag_Create")]
        public ActionResult CreateCompany()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,WeekTag_Create")]
        public ActionResult CreateCompany([Bind(Include = "Nian,Range,Week1,Week2,Week3,week4,Week5,Week6,Week7")] WeekTag weektag)
        {

            string date = weektag.Nian;
            DateTime begindate = Convert.ToDateTime(date + "-01-01");
            //DateTime begindate = Convert.ToDateTime(date+"01-01");
            //DateTime enddate = Convert.ToDateTime(date+"12-31");
            //System.TimeSpan tsdiffer = enddate.Date - begindate.Date;
            // int intdiffer = tsdiffer.Days + 1;
            int intdiffer;
            if (DateTime.IsLeapYear(int.Parse(weektag.Nian)))
            {
                intdiffer = 366;
            }
            else
                intdiffer = 365;

            List<DateTime> list = new List<DateTime>();
            for (int i = 0; i < intdiffer; i++)
            {
                DateTime dttemp = begindate.Date.AddDays(i);

                if ((dttemp.DayOfWeek == System.DayOfWeek.Monday && "1" == weektag.Week1) ||
                    (dttemp.DayOfWeek == System.DayOfWeek.Tuesday && "1" == weektag.Week2) ||
                    (dttemp.DayOfWeek == System.DayOfWeek.Wednesday && "1" == weektag.Week3) ||
                    (dttemp.DayOfWeek == System.DayOfWeek.Thursday && "1" == weektag.Week4) ||
                    (dttemp.DayOfWeek == System.DayOfWeek.Friday && "1" == weektag.Week5) ||
                    (dttemp.DayOfWeek == System.DayOfWeek.Saturday && "1" == weektag.Week6) ||
                    (dttemp.DayOfWeek == System.DayOfWeek.Sunday && "1" == weektag.Week7))
                {
                    RecordDatetime temp = new RecordDatetime();
                    temp.Recordtime = dttemp;
                    temp.Tag = "1";
                    temp.Year = dttemp.Year.ToString();
                    temp.Month = dttemp.Month.ToString();
                    temp.Day = dttemp.Day.ToString();
                    db.RecordDatetimes.Add(temp);
                    db.SaveChanges();
                }
                else
                {
                    RecordDatetime temp = new RecordDatetime();
                    temp.Recordtime = dttemp;
                    temp.Tag = "0";
                    temp.Year = dttemp.Year.ToString();
                    temp.Month = dttemp.Month.ToString();
                    temp.Day = dttemp.Day.ToString();
                    db.RecordDatetimes.Add(temp);
                    db.SaveChanges();
                }

            }
            db.WeekTags.Add(weektag);
            db.SaveChanges();

            return RedirectToAction("Index");

        }
         [Authorize(Roles = "Admin,WeekTag_Create")]
        public ActionResult CreateDepartment()
        {
            List<SelectListItem> departmentList = db.Departments.ToList().Select(d => new SelectListItem
                {
                    Value = d.DepartmentId,
                    Text = d.Name,
                }).ToList();
            ViewBag.departmentList = departmentList;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,WeekTag_Create")]
        public ActionResult CreateDepartment([Bind(Include = "Nian,Range,Week1,Week2,Week3,week4,Week5,Week6,Week7")] WeekTag weektag)
        {

            string date = weektag.Nian;
            var year = (from wt in db.WeekTags
                        where wt.Nian == date && wt.Range == weektag.Range
                        select wt).ToList();
            if (year.Count() != 0)
            {
                List<SelectListItem> departmentList = db.Departments.ToList().Select(d => new SelectListItem
                {
                    Value = d.DepartmentId,
                    Text = d.Name,
                }).ToList();
                ViewBag.departmentList = departmentList;
                ModelState.AddModelError("", "本部门或者公司的该年日历已经存在！");

                return View(weektag);
            }
            else
            {
                DateTime begindate = Convert.ToDateTime(date + "-01-01");
                //DateTime begindate = Convert.ToDateTime(date+"01-01");
                //DateTime enddate = Convert.ToDateTime(date+"12-31");
                //System.TimeSpan tsdiffer = enddate.Date - begindate.Date;
                // int intdiffer = tsdiffer.Days + 1;
                int intdiffer;
                if (DateTime.IsLeapYear(int.Parse(weektag.Nian)))
                {
                    intdiffer = 366;
                }
                else
                    intdiffer = 365;
                //生成对应人员或者部门或者公司的假日表
                string tablename = Generate.GenerateTableName();
                var flag = CreateTableForHoliday(tablename, this.ConnectionString);
                HolidayRecord hr = new HolidayRecord();
                hr.HolidayName = tablename;
                hr.Number = weektag.Range;
                db.HolidayRecords.Add(hr);
                db.SaveChanges();
                //数据库连接
                SqlConnection conn = new SqlConnection(this.ConnectionString);
                conn.Open();
                List<DateTime> list = new List<DateTime>();
                for (int i = 0; i < intdiffer; i++)
                {
                    DateTime dttemp = begindate.Date.AddDays(i);

                    if ((dttemp.DayOfWeek == System.DayOfWeek.Monday && "1" == weektag.Week1) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Tuesday && "1" == weektag.Week2) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Wednesday && "1" == weektag.Week3) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Thursday && "1" == weektag.Week4) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Friday && "1" == weektag.Week5) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Saturday && "1" == weektag.Week6) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Sunday && "1" == weektag.Week7))
                    {
                        //插入数据
                        DataTable dtInsert = new DataTable();
                        dtInsert.Columns.Add("Recordtime", typeof(DateTime));
                        dtInsert.Columns.Add("Tag", typeof(string));
                        dtInsert.Columns.Add("Year", typeof(string));
                        dtInsert.Columns.Add("Month", typeof(string));
                        dtInsert.Columns.Add("Day", typeof(string));
                        dtInsert.Rows.Add(new object[] { dttemp, "1", dttemp.Year.ToString(), dttemp.Month.ToString(), dttemp.Day.ToString() });
                        string sql = "insert into " + tablename + "(Recordtime,Tag,Year,Month,Day) values(@Recordtime,@Tag,@Year,@Month,@Day)";
                        SqlDataAdapter dr = new SqlDataAdapter();//上面两句可以合并成这一  
                        dr.InsertCommand = new SqlCommand(sql, conn);
                        dr.SelectCommand = new SqlCommand("select * from Staffs where 1=0", conn);//仅为了获得框架
                        dr.InsertCommand.Parameters.Add("Recordtime", SqlDbType.DateTime);
                        dr.InsertCommand.Parameters.Add("Tag", SqlDbType.VarChar);
                        dr.InsertCommand.Parameters.Add("Year", SqlDbType.VarChar);
                        dr.InsertCommand.Parameters.Add("Month", SqlDbType.VarChar);
                        dr.InsertCommand.Parameters.Add("Day", SqlDbType.VarChar);

                        dr.InsertCommand.Parameters["Recordtime"].SourceColumn = "Recordtime";
                        dr.InsertCommand.Parameters["Tag"].SourceColumn = "Tag";
                        dr.InsertCommand.Parameters["Year"].SourceColumn = "Year";
                        dr.InsertCommand.Parameters["Month"].SourceColumn = "Month";
                        dr.InsertCommand.Parameters["Day"].SourceColumn = "Day";
                        dr.InsertCommand.UpdatedRowSource = UpdateRowSource.None;

                        if (dtInsert.Rows.Count > 0)
                        {
                            dr.Update(dtInsert);
                        }
                        //RecordDatetime temp = new RecordDatetime();
                        //temp.Recordtime = dttemp;
                        //temp.Tag = "1";
                        //temp.Year = dttemp.Year.ToString();
                        //temp.Month = dttemp.Month.ToString();
                        //temp.Day = dttemp.Day.ToString();
                        //db.RecordDatetimes.Add(temp);
                        //db.SaveChanges();
                    }
                    else
                    {
                        //插入数据
                        DataTable dtInsert = new DataTable();
                        dtInsert.Columns.Add("Recordtime", typeof(DateTime));
                        dtInsert.Columns.Add("Tag", typeof(string));
                        dtInsert.Columns.Add("Year", typeof(string));
                        dtInsert.Columns.Add("Month", typeof(string));
                        dtInsert.Columns.Add("Day", typeof(string));
                        dtInsert.Rows.Add(new object[] { dttemp, "0", dttemp.Year.ToString(), dttemp.Month.ToString(), dttemp.Day.ToString() });
                        string sql = "insert into " + tablename + "(Recordtime,Tag,Year,Month,Day) values(@Recordtime,@Tag,@Year,@Month,@Day)";
                        SqlDataAdapter dr = new SqlDataAdapter();//上面两句可以合并成这一  
                        dr.InsertCommand = new SqlCommand(sql, conn);
                        dr.SelectCommand = new SqlCommand("select * from Staffs where 1=0", conn);//仅为了获得框架
                        dr.InsertCommand.Parameters.Add("Recordtime", SqlDbType.DateTime);
                        dr.InsertCommand.Parameters.Add("Tag", SqlDbType.VarChar);
                        dr.InsertCommand.Parameters.Add("Year", SqlDbType.VarChar);
                        dr.InsertCommand.Parameters.Add("Month", SqlDbType.VarChar);
                        dr.InsertCommand.Parameters.Add("Day", SqlDbType.VarChar);

                        dr.InsertCommand.Parameters["Recordtime"].SourceColumn = "Recordtime";
                        dr.InsertCommand.Parameters["Tag"].SourceColumn = "Tag";
                        dr.InsertCommand.Parameters["Year"].SourceColumn = "Year";
                        dr.InsertCommand.Parameters["Month"].SourceColumn = "Month";
                        dr.InsertCommand.Parameters["Day"].SourceColumn = "Day";
                        dr.InsertCommand.UpdatedRowSource = UpdateRowSource.None;

                        if (dtInsert.Rows.Count > 0)
                        {
                            dr.Update(dtInsert);
                        }


                    }
                }
                db.WeekTags.Add(weektag);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

        }
         [Authorize(Roles = "Admin,WeekTag_Create")]
        public ActionResult CreatePerson()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,WeekTag_Create")]
        public ActionResult CreatePerson([Bind(Include = "Nian,Range,Week1,Week2,Week3,week4,Week5,Week6,Week7")] WeekTag weektag)
        {

            string date = weektag.Nian;
            var year = (from wt in db.WeekTags
                        where wt.Nian == date && wt.Range == weektag.Range
                        select wt).ToList();
            if (year.Count() != 0)
            {
                //List<SelectListItem> departmentList = db.Departments.ToList().Select(d => new SelectListItem
                //{
                //    Value = d.DepartmentId,
                //    Text = d.Name,
                //}).ToList();
                //ViewBag.departmentList = departmentList;
                ModelState.AddModelError("", "本员工的该年日历已经存在！");

                return View(weektag);
            }
            else
            {
                DateTime begindate = Convert.ToDateTime(date + "-01-01");
                //DateTime begindate = Convert.ToDateTime(date+"01-01");
                //DateTime enddate = Convert.ToDateTime(date+"12-31");
                //System.TimeSpan tsdiffer = enddate.Date - begindate.Date;
                // int intdiffer = tsdiffer.Days + 1;
                int intdiffer;
                if (DateTime.IsLeapYear(int.Parse(weektag.Nian)))
                {
                    intdiffer = 366;
                }
                else
                    intdiffer = 365;
                //生成对应人员或者部门或者公司的假日表
                string tablename = Generate.GenerateTableName();
                var flag = CreateTableForHoliday(tablename, this.ConnectionString);
                HolidayRecord hr = new HolidayRecord();
                hr.HolidayName = tablename;
                hr.Number = weektag.Range;
                db.HolidayRecords.Add(hr);
                db.SaveChanges();
                //数据库连接
                SqlConnection conn = new SqlConnection(this.ConnectionString);
                conn.Open();
                List<DateTime> list = new List<DateTime>();
                for (int i = 0; i < intdiffer; i++)
                {
                    DateTime dttemp = begindate.Date.AddDays(i);

                    if ((dttemp.DayOfWeek == System.DayOfWeek.Monday && "1" == weektag.Week1) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Tuesday && "1" == weektag.Week2) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Wednesday && "1" == weektag.Week3) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Thursday && "1" == weektag.Week4) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Friday && "1" == weektag.Week5) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Saturday && "1" == weektag.Week6) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Sunday && "1" == weektag.Week7))
                    {
                        //插入数据
                        DataTable dtInsert = new DataTable();
                        dtInsert.Columns.Add("Recordtime", typeof(DateTime));
                        dtInsert.Columns.Add("Tag", typeof(string));
                        dtInsert.Columns.Add("Year", typeof(string));
                        dtInsert.Columns.Add("Month", typeof(string));
                        dtInsert.Columns.Add("Day", typeof(string));
                        dtInsert.Rows.Add(new object[] { dttemp, "1", dttemp.Year.ToString(), dttemp.Month.ToString(), dttemp.Day.ToString() });
                        string sql = "insert into " + tablename + "(Recordtime,Tag,Year,Month,Day) values(@Recordtime,@Tag,@Year,@Month,@Day)";
                        SqlDataAdapter dr = new SqlDataAdapter();//上面两句可以合并成这一  
                        dr.InsertCommand = new SqlCommand(sql, conn);
                        dr.SelectCommand = new SqlCommand("select * from Staffs where 1=0", conn);//仅为了获得框架
                        dr.InsertCommand.Parameters.Add("Recordtime", SqlDbType.DateTime);
                        dr.InsertCommand.Parameters.Add("Tag", SqlDbType.VarChar);
                        dr.InsertCommand.Parameters.Add("Year", SqlDbType.VarChar);
                        dr.InsertCommand.Parameters.Add("Month", SqlDbType.VarChar);
                        dr.InsertCommand.Parameters.Add("Day", SqlDbType.VarChar);

                        dr.InsertCommand.Parameters["Recordtime"].SourceColumn = "Recordtime";
                        dr.InsertCommand.Parameters["Tag"].SourceColumn = "Tag";
                        dr.InsertCommand.Parameters["Year"].SourceColumn = "Year";
                        dr.InsertCommand.Parameters["Month"].SourceColumn = "Month";
                        dr.InsertCommand.Parameters["Day"].SourceColumn = "Day";
                        dr.InsertCommand.UpdatedRowSource = UpdateRowSource.None;

                        if (dtInsert.Rows.Count > 0)
                        {
                            dr.Update(dtInsert);
                        }
                        //RecordDatetime temp = new RecordDatetime();
                        //temp.Recordtime = dttemp;
                        //temp.Tag = "1";
                        //temp.Year = dttemp.Year.ToString();
                        //temp.Month = dttemp.Month.ToString();
                        //temp.Day = dttemp.Day.ToString();
                        //db.RecordDatetimes.Add(temp);
                        //db.SaveChanges();
                    }
                    else
                    {
                        //插入数据
                        DataTable dtInsert = new DataTable();
                        dtInsert.Columns.Add("Recordtime", typeof(DateTime));
                        dtInsert.Columns.Add("Tag", typeof(string));
                        dtInsert.Columns.Add("Year", typeof(string));
                        dtInsert.Columns.Add("Month", typeof(string));
                        dtInsert.Columns.Add("Day", typeof(string));
                        dtInsert.Rows.Add(new object[] { dttemp, "0", dttemp.Year.ToString(), dttemp.Month.ToString(), dttemp.Day.ToString() });
                        string sql = "insert into " + tablename + "(Recordtime,Tag,Year,Month,Day) values(@Recordtime,@Tag,@Year,@Month,@Day)";
                        SqlDataAdapter dr = new SqlDataAdapter();//上面两句可以合并成这一  
                        dr.InsertCommand = new SqlCommand(sql, conn);
                        dr.SelectCommand = new SqlCommand("select * from Staffs where 1=0", conn);//仅为了获得框架
                        dr.InsertCommand.Parameters.Add("Recordtime", SqlDbType.DateTime);
                        dr.InsertCommand.Parameters.Add("Tag", SqlDbType.VarChar);
                        dr.InsertCommand.Parameters.Add("Year", SqlDbType.VarChar);
                        dr.InsertCommand.Parameters.Add("Month", SqlDbType.VarChar);
                        dr.InsertCommand.Parameters.Add("Day", SqlDbType.VarChar);

                        dr.InsertCommand.Parameters["Recordtime"].SourceColumn = "Recordtime";
                        dr.InsertCommand.Parameters["Tag"].SourceColumn = "Tag";
                        dr.InsertCommand.Parameters["Year"].SourceColumn = "Year";
                        dr.InsertCommand.Parameters["Month"].SourceColumn = "Month";
                        dr.InsertCommand.Parameters["Day"].SourceColumn = "Day";
                        dr.InsertCommand.UpdatedRowSource = UpdateRowSource.None;

                        if (dtInsert.Rows.Count > 0)
                        {
                            dr.Update(dtInsert);
                        }


                    }
                }
                db.WeekTags.Add(weektag);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

        }

        public JsonResult Result(string month, string year)
        {
            List<Object> obj = new List<Object>();
            var temp = (from rd in db.RecordDatetimes
                        where rd.Month == month && rd.Year == year//&&rd.Recordtime.Year.ToString()==year//从日期里面获取年，月，日
                        select new { Tag = rd.Tag }).ToList();

            foreach (var t in temp)
            {
                obj.Add(new { Tag = t.Tag });

            }
            return Json(obj);
        }
        public JsonResult ResultHoliday(string month, string year, string staffNumber)
        {
            List<Object> obj = new List<Object>();     //实例化一个集合对象为测试对象
            var staff = (from hr in db.HolidayRecords
                         where hr.Number == staffNumber
                         select hr).ToList();
            //Staff staff_department =(from s in db.Staffs
            //                     where s.StaffNumber==staffNumber
            //                     select s).SingleOrDefault();
            //var staffDepartment= (from hr in db.HolidayRecords
            //                 where hr.Number==staff_department.Department
            //                 select hr).ToList();
            //Department staff_company=(from d in db.Departments
            //                              where d.DepartmentId==staff_department.Department 
            //                              select d).SingleOrDefault();
            //var staffCompany=(from hr in db.HolidayRecords
            //                 where hr.Number==staff_company.ParentDepartmentId
            //                 select hr).ToList();
            if (staff.Count() != 0)//部门和员工都在HolidayRecords
            {
                foreach (var staff1 in staff)
                {
                    string tablename = staff1.HolidayName;
                    string sql = " select * from " + tablename + " where Year=" + year + " and Month=" + month;
                    string str = this.ConnectionString;
                    SqlConnection conn = new SqlConnection(str);
                    SqlDataAdapter dr = new SqlDataAdapter(sql, conn);//上面两句可以合并成这一       
                    DataSet ds = new DataSet();//创建数据集；
                    dr.Fill(ds); //填充数据集               
                    string a;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)          //循环取出ds.tables中的值
                    {
                        a = ds.Tables[0].Rows[i]["Tag"].ToString();
                        obj.Add(new RecordDatetime { Tag = a });
                    }
                }
            }
            else
            {
                Staff staff_department = (from s in db.Staffs
                                          where s.StaffNumber == staffNumber
                                          select s).SingleOrDefault();//得到员工信息
                if (staff_department != null)//判断员工是否存在
                {
                    var staffDepartment = (from hr in db.HolidayRecords
                                           where hr.Number == staff_department.Department
                                           select hr).ToList();//得到员工所在的部门的假日表
                    if (staffDepartment.Count() != 0)//判断员工所在部门的假日表是否存在
                    {
                        foreach (var staffDepartment1 in staffDepartment)
                        {
                            string tablename = staffDepartment1.HolidayName;
                            string sql = " select * from " + tablename + " where Year=" + year + " and Month=" + month;
                            string str = this.ConnectionString;
                            SqlConnection conn = new SqlConnection(str);
                            SqlDataAdapter dr = new SqlDataAdapter(sql, conn);//上面两句可以合并成这一       
                            DataSet ds = new DataSet();//创建数据集；
                            dr.Fill(ds); //填充数据集                    
                            string a;
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)          //循环取出ds.tables中的值
                            {
                                a = ds.Tables[0].Rows[i]["Tag"].ToString();
                                obj.Add(new RecordDatetime { Tag = a });
                            }
                        }
                    }
                    else
                    {
                        Department staff_company = (from d in db.Departments
                                                    where d.DepartmentId == staff_department.Department
                                                    select d).SingleOrDefault();//得到员工所在部门的部门信息
                        var staffCompany = (from hr in db.HolidayRecords
                                            where hr.Number == staff_company.ParentDepartmentId
                                            select hr).ToList();//得到员工所在部门的上级部门的假日表信息
                        if (staffCompany.Count != 0)
                        {
                            foreach (var staffCompany1 in staffCompany)
                            {
                                string tablename = staffCompany1.HolidayName;
                                string sql = " select * from " + tablename + " where Year=" + year + " and Month=" + month;
                                string str = this.ConnectionString;
                                SqlConnection conn = new SqlConnection(str);
                                SqlDataAdapter dr = new SqlDataAdapter(sql, conn);//上面两句可以合并成这一       
                                DataSet ds = new DataSet();//创建数据集；
                                dr.Fill(ds); //填充数据集                   
                                string a;
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)          //循环取出ds.tables中的值
                                {
                                    a = ds.Tables[0].Rows[i]["Tag"].ToString();
                                    obj.Add(new RecordDatetime { Tag = a });
                                }

                            }
                        }
                        else
                        {
                            Department staff_company1 = (from d in db.Departments
                                                         where d.DepartmentId == staff_company.ParentDepartmentId
                                                         select d).SingleOrDefault();//得到员工所在部门的上级部门信息
                            var staffCompany1 = (from hr in db.HolidayRecords
                                                 where hr.Number == staff_company1.ParentDepartmentId
                                                 select hr).ToList();//得到员工所在部门的上级部门的上级部门的假日表信息
                            foreach (var staffCompany2 in staffCompany1)
                            {
                                string tablename = staffCompany2.HolidayName;
                                string sql = " select * from " + tablename + " where Year=" + year + " and Month=" + month;
                                string str = this.ConnectionString;
                                SqlConnection conn = new SqlConnection(str);
                                SqlDataAdapter dr = new SqlDataAdapter(sql, conn);//上面两句可以合并成这一       
                                DataSet ds = new DataSet();//创建数据集；
                                dr.Fill(ds); //填充数据集                   
                                string a;
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)          //循环取出ds.tables中的值
                                {
                                    a = ds.Tables[0].Rows[i]["Tag"].ToString();
                                    obj.Add(new RecordDatetime { Tag = a });
                                }

                            }

                        }
                    }
                }
                else
                {
                    Department staff_company = (from d in db.Departments
                                                where d.DepartmentId == staffNumber
                                                select d).SingleOrDefault();//得到部门的部门信息
                    var staffCompany = (from hr in db.HolidayRecords
                                        where hr.Number == staff_company.ParentDepartmentId
                                        select hr).ToList();//得到部门的上级部门的假日表信息
                    if (staffCompany.Count != 0)
                    {
                        foreach (var staffCompany1 in staffCompany)
                        {
                            string tablename = staffCompany1.HolidayName;
                            string sql = " select * from " + tablename + " where Year=" + year + " and Month=" + month;
                            string str = this.ConnectionString;
                            SqlConnection conn = new SqlConnection(str);
                            SqlDataAdapter dr = new SqlDataAdapter(sql, conn);//上面两句可以合并成这一       
                            DataSet ds = new DataSet();//创建数据集；
                            dr.Fill(ds); //填充数据集                   
                            string a;
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)          //循环取出ds.tables中的值
                            {
                                a = ds.Tables[0].Rows[i]["Tag"].ToString();
                                obj.Add(new RecordDatetime { Tag = a });
                            }

                        }
                    }
                    else
                    {
                        Department staff_company1 = (from d in db.Departments
                                                     where d.DepartmentId == staff_company.ParentDepartmentId
                                                     select d).SingleOrDefault();//得到所在部门的上级部门的部门信息
                        var staffCompany1 = (from hr in db.HolidayRecords
                                             where hr.Number == staff_company1.ParentDepartmentId
                                             select hr).ToList();//得到员工所在部门的上级部门的上级部门的假日表信息
                        foreach (var staffCompany2 in staffCompany1)
                        {
                            string tablename = staffCompany2.HolidayName;
                            string sql = " select * from " + tablename + " where Year=" + year + " and Month=" + month;
                            string str = this.ConnectionString;
                            SqlConnection conn = new SqlConnection(str);
                            SqlDataAdapter dr = new SqlDataAdapter(sql, conn);//上面两句可以合并成这一       
                            DataSet ds = new DataSet();//创建数据集；
                            dr.Fill(ds); //填充数据集                   
                            string a;
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)          //循环取出ds.tables中的值
                            {
                                a = ds.Tables[0].Rows[i]["Tag"].ToString();
                                obj.Add(new RecordDatetime { Tag = a });
                            }

                        }

                    }
                }


            }
            return Json(obj);

        }
         [Authorize(Roles = "Admin,WeekTag_Create")]
        public bool CreateTableForHoliday(string TableName, string ConnectionString)
        {
            String str;
            //string ConnString = ConfigurationManager.AppSettings["MasterDbConnectionString"];
            SqlConnection myConn = new SqlConnection(ConnectionString);
            str = "CREATE TABLE " + TableName + "([Id] int NOT NULL IDENTITY(1,1) PRIMARY KEY,[Recordtime] date NOT NULL ,[Tag] nvarchar(2) NULL ,[Month] nvarchar(2) NULL ,[Day] nvarchar(2) NULL ,[Year] varchar(4) NULL )";
            SqlCommand myCommand = new SqlCommand(str, myConn);
            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                return false;
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            return true;
        }
        public JsonResult DepartmentList()
        {
            List<Object> obj = new List<Object>();
            var departmentId = db.Departments.ToList();
            foreach (var temp in departmentId)
            {
                obj.Add(new { Number = temp.DepartmentId });
            }
            return Json(obj);
        }
        public ActionResult TestSql()
        {
            string tablename = Generate.GenerateTableName();
            var flag = CreateTableForHoliday(tablename, this.ConnectionString);
            HolidayRecord hr = new HolidayRecord();
            hr.HolidayName = tablename;
            hr.Number = "";
            db.HolidayRecords.Add(hr);
            db.SaveChanges();
            if (flag == true)
            {
                @ViewBag.test = tablename;
            }
            return View();
            //string tablename = "Staffs";
            //string sql = " select * from " + tablename;
            ////if exists(select name from sysobjects where Name = " + tablename + " and XType = 'U')
            //string str = this.ConnectionString;
            //SqlConnection conn = new SqlConnection(str);
            ////conn.Open(); //使用 SqlDataAdapter（数据适配器）不用写 
            ////SqlCommand comm = new SqlCommand(sql, conn);
            ////SqlDataAdapter dr = new SqlDataAdapter(comm); 
            //SqlDataAdapter dr = new SqlDataAdapter(sql, conn);//上面两句可以合并成这一       
            //DataSet ds = new DataSet();//创建数据集；
            //dr.Fill(ds); //填充数据集
            //List<Object> obj = new List<Object>();   //实例化一个集合对象为测试对象
            //string a;
            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)          //循环取出ds.tables中的值
            //{
            //    a = ds.Tables[0].Rows[i]["Name"].ToString();
            //    obj.Add(new Staff { Name = a });
            //}
            //ViewBag.list = obj;
            //return View();
        }
    }
}