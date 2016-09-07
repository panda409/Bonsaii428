using Bonsaii.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Routing;
using System.Data.Entity;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using BonsaiiModels.Staffs;
using Bonsaii.Models.Works;
namespace Bonsaii.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        ///  连接字符串，从session中获取
        /// </summary>
        
        public string UserName
        {
            get;
            private set;
        }
        public string Name
        {
            get;
            private set;
        }
        public string CompanyId
        {
            get;
            private set;
        }
        public string ConnectionString
        {
            get;
            private set;
        }
        public string CompanyFullName
        {
            get;
            private set;
        }
        //public Dictionary<int, string> StateDescription
        //{
        //    get;
        //    private set;
        //}
        //public Dictionary<int,string> StateDescription
        //{
        //    get;
        //    private set;
        //}
        public bool IsProved
        {
            get;
            private set;
        }
        public BonsaiiDbContext db
        {
            get;
            private set;
        }
        public ApplicationUserManager UserManager
        {
            get;
            set;
        }
        /// <summary>
        /// 在Controller的构造函数调用完成之前是不能够获取到Session这个对象的！因此要把获取有关Session数据的操作放到Initialize方法当中。
        /// Initialize方法调用的时候，Controller已经可以获取到Session对象了。
        /// </summary>
        public BaseController()
        {
        }
        /// <summary>
        /// 这个函数的调用顺序在：BaseController的构造函数和继承BaseController的构造函数调用之后才进行调用
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
       //     UserData = Session["UserData"] as SessionData;


            if (Session["UserName"] != null)
            {
                this.UserName = Session["UserName"] as string;
                this.Name = Session["Name"] as string;
                this.CompanyId = Session["CompanyId"] as string;
                this.ConnectionString = Session["ConnectionString"] as string;
                this.CompanyFullName = Session["CompanyFullName"] as string;
                this.IsProved = (bool)Session["IsProved"];
            }
            else
            {
                UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = UserManager.FindById(User.Identity.GetUserId());

                //既没有通过系统登录，也没有通过外部登录，跳转到登录页面
                if (user == null)
                    Rederect(requestContext, Url.Action("Login", "Account"));
                //通过外部登录或者浏览器保存的cookie和session信息自动进行了登录
                this.UserName = user.UserName;
                this.Name = user.Name;
                this.CompanyId = user.CompanyId;
                this.ConnectionString = user.ConnectionString;
                this.CompanyFullName = user.CompanyFullName;
                this.IsProved = user.IsProved;
                Session["UserName"] = this.UserName;
                Session["Name"] = this.Name;
                Session["CompanyId"] = this.CompanyId;
                Session["ConnectionString"] = this.ConnectionString;

                Session["CompanyFullName"] = this.CompanyFullName;
                Session["IsProved"] = this.IsProved;
            }
            db = new BonsaiiDbContext(this.ConnectionString);

            //StateDescription = new Dictionary<int, string>();
            //foreach (var tmp in db.States.ToList())
            //    StateDescription.Add(tmp.Id, tmp.Description);
        }

        private void Rederect(RequestContext requestContext, string action)
        {
            requestContext.HttpContext.Response.Clear();
            requestContext.HttpContext.Response.Redirect(action);
            requestContext.HttpContext.Response.End();
        }

        /// <summary>
        /// 生成普通编码
        /// </summary>
        /// <param name="code">普通编码的类型</param>
        /// <param name="DepartmentShort">部门编码的缩写</param>
        /// <returns></returns>
        public string GenerateStaffParam(string code,string DepartmentShort)
        {
            ParamCodes tmp = db.ParamCodes.Where(p => p.Type == code).Single();

            string date = DateTime.Now.ToString("yyyyMMdd");
            //为流水号补充零
            string SerialNumber = AddZero(tmp.Count, tmp.SerialNumber);
            //更新单号的计数值
            tmp.Count++;
            db.Entry(tmp).State = EntityState.Modified;
            db.SaveChanges();

            //部门缩写＋流水号的情况
            if (tmp.CodeMethod.Equals(CodeMethod.Five))
            {
                SerialNumber = AddZero(tmp.Count-1, 10 - DepartmentShort.Length);
                return DepartmentShort + SerialNumber;
            }

            switch (tmp.CodeMethod)
            {
                case CodeMethod.Day:
                    return DateTime.Now.ToString("yyyyMMdd").ToString() + SerialNumber;
                case CodeMethod.Month:
                    return DateTime.Now.ToString("yyyyMM").ToString() + SerialNumber;
                case CodeMethod.Serial:
                    return tmp.Code.Substring(0, 10 - tmp.SerialNumber) + SerialNumber;                    
                default:
                    return "";
            }
        }
        /// <summary>
        /// 根据单据类型编号，生成单号
        /// </summary>
        /// <param name="BillTypeNumber">单据类型编号</param>
        /// <returns>单号</returns>
        public string GenerateBillNumber(string BillTypeNumber)
        {
            BillPropertyModels tmp = db.BillProperties.Where(p => p.Type == BillTypeNumber).Single();
            string date = DateTime.Now.ToString("yyMMdd");
            //为流水号补充零
            string SerialNumber = AddZero(tmp.Count, tmp.SerialNumber);
            //更新单号的计数值
            tmp.Count++;
            db.Entry(tmp).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw e;
            }

            switch (tmp.CodeMethod)
            {
                case CodeMethod.Day:
                    return DateTime.Now.ToString("yyMMdd").ToString() + SerialNumber;
                case CodeMethod.Month:
                    return DateTime.Now.ToString("yyMM").ToString() + SerialNumber;
                case CodeMethod.Serial:
                    return tmp.Code.Substring(0, 10 - tmp.SerialNumber) + SerialNumber;
                default:
                    return "";
            }
        }

        /// <summary>
        /// 将SerialNumber补零凑够length长度的数值
        /// </summary>
        /// <param name="SerialNumber"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string AddZero(int SerialNumber, int length)
        {
            string tmp = SerialNumber.ToString();
            while (tmp.Length != length)
                tmp = tmp.Insert(0, "0");
            return tmp;
        }
        /// <summary>
        /// 生成部门编号
        /// </summary>
        /// <param name="numlen"></param>
        /// <returns></returns>
        public string GetRandomCode(int numlen)
        {
            char[] chars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
                           'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
            string code = string.Empty;

            for (int i = 0; i < numlen; i++)
            {
                //这里是关键，传入一个seed参数即可保证生成的随机数不同            
                //Random rnd = new Random(unchecked((int)DateTime.Now.Ticks));
                Random rnd = new Random(GetRandomSeed());
                code += chars[rnd.Next(0, 61)].ToString();
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
        /// <summary>
        /// SerialCodeCount表添加一条记录
        /// </summary>
      //  static void AddSerialCodeCount(int numlen)
       // {
        
        //}

        /// <summary>
        /// 生成物理卡号，是流水
        /// </summary>
        /// <param name="numlen"></param>
        /// 参数就是传过去的物理卡号长度
        /// <returns></returns>
        public string GetSerialNumber(int numlen) 
        {
            string code = null;
            var serialCodeCount = db.SerialCodeCounts.Find(numlen);
            if(serialCodeCount==null)//如果没有这个长度
            {
                SerialCodeCount serialCodeAdd = new SerialCodeCount();
                serialCodeAdd.Length = numlen;
                serialCodeAdd.Count = 0;
                db.SerialCodeCounts.Add(serialCodeAdd);
                db.SaveChanges();
                serialCodeCount.Count = 1;
                code = AddZero(serialCodeCount.Count, numlen);
            }
            else//如果存在这个长度
            {
                int count = serialCodeCount.Count;
                if (count == int.MaxValue)
                {
                    count = 0;
                }
                serialCodeCount.Count = count + 1;
                code = AddZero(serialCodeCount.Count, numlen);

            }
            db.SaveChanges();
            return code;
        }

        /// <summary>
        /// Excel转Table
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DataTable RenderFromExcel(string path)
        {
            try
            {
                IWorkbook workbook = new XSSFWorkbook((new System.IO.FileStream(path, FileMode.Open, FileAccess.Read)));
                ISheet sheet = workbook.GetSheetAt(0);//取第一个表  

                DataTable table = new DataTable();

                IRow headerRow = sheet.GetRow(0);//第一行为标题行  
                int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells  
                int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1  

                //handling header.  
                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                    table.Columns.Add(column);
                }

                for (int i = sheet.FirstRowNum; i <= rowCount; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = table.NewRow();

                    if (row != null)
                    {
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                                dataRow[j] = GetCellValue(row.GetCell(j));
                        }
                        table.Rows.Add(dataRow);
                    }

                }

                return table;

            }
            catch (Exception e)
            {
                return null;
            }
        }
        /// <summary>
        /// 。。。
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static string GetCellValue(ICell cell)
        {
            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.Blank:
                    return string.Empty;
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Numeric:
                case CellType.Unknown:
                default:
                    return cell.ToString();//This is a trick to get the correct value of the cell. NumericCellValue will return a numeric value no matter the cell value is a date or a number
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Formula:
                    try
                    {
                        XSSFFormulaEvaluator e = new XSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }
        /// <summary>
        /// 发起人员离职申请并且审核通过之后，执行此函数
        /// 修改员工档案，并增加一条记录到离职档案，最后修改Users表（如果需要）
        /// </summary>
        /// <param name="staffApplication"></param>
        public void StaffApplicationPassAudit(StaffApplication staffApplication)
        {
            try
            {
                staffApplication.AuditStatus = 3;

                Staff staffId = (from p in db.Staffs where p.StaffNumber == staffApplication.StaffNumber && p.ArchiveTag != true select p).ToList().Single();//根据员工工号找到这个人的人员档案，是唯一的。
                Staff staff = db.Staffs.Find(staffId.Number);
                staff.ArchiveTag = true;//true 代表离职  
                staff.BindingCode = null;
                ///看看离职档案里面有没有;没有就添加。
                var staffArchiveFind = (from p in db.StaffArchives where p.StaffNumber == staffApplication.StaffNumber select p).ToList();
                if (staffArchiveFind.Count == 0)
                {
                    StaffArchive staffArchive = new StaffArchive();
                    staffArchive.BillTypeNumber = staffApplication.BillTypeNumber;
                    staffArchive.BillTypeName = staffApplication.BillTypeName;
                    staffArchive.BillNumber = staffApplication.BillNumber;
                    staffArchive.StaffNumber = staff.StaffNumber;
                    staffArchive.StaffName = staff.Name;
                    staffArchive.LeaveDate = staffApplication.HopeLeaveDate;
                    staffArchive.Department = (from p in db.Departments where p.DepartmentId == staff.Department select p.Name).ToList().FirstOrDefault();
                    staffArchive.IdenticationNumber = staff.IdentificationNumber;
                    staffArchive.RecordPerson = staff.RecordPerson;
                    staffArchive.RecordTime = staff.RecordTime;
                    staffArchive.BlackList = false;
                    staffArchive.WorkPlus = false;
                    db.StaffArchives.Add(staffArchive);
                }
                db.SaveChanges();
                ////修改系统表
                SystemDbContext sysdb = new SystemDbContext();
                var Ucount = (from p in sysdb.BindCodes where (p.CompanyId == this.CompanyId && p.StaffNumber == staffApplication.StaffNumber) select p).SingleOrDefault();
                if (Ucount != null)
                {
                    sysdb.BindCodes.Remove(Ucount);
                    sysdb.SaveChanges();
                }

                //SystemDbContext sysdb = new SystemDbContext();
                //  var UCount = (from p in sysdb.Users where (p.CompanyId == this.CompanyId) && (p.UserName == staffId.IndividualTelNumber)&&() select p).ToList();
                //if (UCount.Count() != 0)//如果存在符合条件的User
                //{
                //    UserModels user = UCount.FirstOrDefault();
                //    user.CompanyId = "app-id";
                //    user.CompanyFullName = "GeneralStaff";
                //    user.BindingCode = null;
                //    user.StaffNumber = null;
                //    user.BindTag = null;
                //    user.ConnectionString = "app-ConnectionString";
                //    sysdb.SaveChanges();
                //}
                //db.SaveChanges();
            }
            catch (Exception e)
            {
                //throw e;
                throw new ArgumentOutOfRangeException("符合条件的员工不存在或工号不唯一");
            }
        }

        /// <summary>
        /// 针对某个新入职的员工，增加排班信息
        /// </summary>
        /// <param name="StaffNubmer"></param>
        /// <param name="WorkId"></param>
        /// <param name="DepartmentId"></param>
        public void AddDefaultWork(string StaffNubmer, int WorkId, string DepartmentId)
        {
            DateTime StartDate = DateTime.Now.Date;
            DateTime EndDate = new DateTime(StartDate.Year, StartDate.Month + 1, 1);    //下个月的第一天
            //插入新的排班情况
            int days = EndDate.DayOfYear - StartDate.DayOfYear;
            //针对本月进行排班
            for (int i = 1; i < days; i++)
            {
                db.WorkManages.Add(new WorkManages()
                {
                    WorksId = WorkId,
                    AuditStatus = 1,
                    Flag = false,        //标示是针对个人的排班
                    StaffNumber = StaffNubmer,
                    DepartmentId = DepartmentId,
                    Date = StartDate.AddDays(i),
                });
            }
            db.SaveChanges();
        }


        public void UpdateWorkAfterStaffChange(string StaffNubmer, int WorkId, string DepartmentId)
        {
            DateTime StartDate = DateTime.Now.Date;
            DateTime EndDate = new DateTime(StartDate.Year, StartDate.Month + 1, 1);    //下个月的第一天

            List<DateTime> tmpWorkManages = (from x in db.WorkManages
                                                where x.StaffNumber.Equals(StaffNubmer) && x.Date <= EndDate && x.Date >= StartDate
                                                select x.Date).ToList();
            //插入新的排班情况
            int days = EndDate.DayOfYear - StartDate.DayOfYear;
            for (int i = 0; i <= days; i++)
            {
                if (tmpWorkManages.Contains(StartDate.AddDays(i)))
                    continue;
                db.WorkManages.Add(new WorkManages()
                {
                    WorksId = WorkId,
                    AuditStatus = 1,
                    Flag = false,        //标示是针对个人的排班
                    StaffNumber = StaffNubmer,
                    DepartmentId = DepartmentId,
                   
                    Date = StartDate.AddDays(i),
                });
            }
            db.SaveChanges();
        }
        /// <summary>
        /// 发起培训并且审核通过之后，执行此函数
        /// </summary>
        /// <param name="?"></param>
        public void TrainStartPassAudit(TrainStart trainStart){
            //string joinperson = Request["JoinPerson"];
            //string[] personInfo = joinperson.Split(',');
            string[] personInfo = trainStart.JoinPerson.Split(',');
            DateTime time = Convert.ToDateTime(trainStart.StartDate.ToShortDateString());
            for (; time <= trainStart.EndDate; time = time.AddDays(1))
            {
                for (int temp2 = 0; temp2 < personInfo.Count(); temp2++)
                {
                    // string[] person = personInfo[temp2].Split('-');
                    string[] person = personInfo[temp2].Split('-', '<', '>');
                    TrainRecord trainrecord = new TrainRecord();
                    trainrecord.BillNumber = trainStart.BillNumber;
                    trainrecord.StaffNumber = person[0];
                    trainrecord.StaffName = person[1];
                    trainrecord.Tag = false;
                    trainrecord.Time = time.ToShortDateString();
                    trainrecord.BillTypeNumber = trainStart.BillTypeNumber;
                    trainrecord.TrainId = trainStart.Id;
                    db.TrainRecords.Add(trainrecord);
                    db.SaveChanges();
                }
            }
        }
    }
}