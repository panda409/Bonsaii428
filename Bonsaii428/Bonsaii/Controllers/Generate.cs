using Bonsaii.Models;
using Bonsaii.Models.Audit;
using Bonsaii.Models.Works;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bonsaii.Controllers
{
    public class Generate
    {
        /// <summary>
        /// 调用存储过程生成10位流水号（流水号的初始值在存储过程当中设置，现在的初始值是1）
        /// </summary>
        /// <returns>返回创建好的企业ID号</returns>
        public static string GenerateCompanyId()
        {
            string serialNumber;
            // Create an connection instance
            string Connection = ConfigurationManager.AppSettings["SystemDbConnectionString"];//"Data Source = localhost,1433;Network Library = DBMSSOCN;Initial Catalog = BonsaiiSys_Test;User ID = test;Password = admin;";
            SqlConnection DataConn = new SqlConnection(Connection);
            // Open connection
            try
            {
                if (DataConn.State == ConnectionState.Closed)
                {
                    DataConn.Open();
                }
            }
            catch (SqlException sqex)
            {
                // Create connection failed
                return null;
            }

            SqlCommand DBCmd = new SqlCommand("GetSerialNumber", DataConn);
            DBCmd.CommandType = CommandType.StoredProcedure;

            // Output parameter（值得注意的是：这里Add的输出变量必须与存储过程里的输出变量同名，否则会报告“dpIDS_GetSerialNumber 的@SerialNumber参数not supplied” 错误！）
            SqlParameter param = new SqlParameter("@SerialNumber", SqlDbType.VarChar, 10);
            param.Direction = System.Data.ParameterDirection.Output;
            DBCmd.Parameters.Add(param);

            try
            {

                DBCmd.ExecuteNonQuery();
                serialNumber = param.Value.ToString();  //得到输出参数的值
            }
            catch (SqlException sqex)
            {
                return null;
            }

            try
            {
                if (DataConn.State == ConnectionState.Open)
                {
                    DataConn.Close();
                }
            }
            catch (SqlException sqex)
            {
                return null;
            }
            return serialNumber;
        }
        public static string GenerateTableName()
        {
            string serialNumber;
            // Create an connection instance
            string Connection = ConfigurationManager.AppSettings["SystemDbConnectionString"];//"Data Source = localhost,1433;Network Library = DBMSSOCN;Initial Catalog = BonsaiiSys_Test;User ID = test;Password = admin;";
            SqlConnection DataConn = new SqlConnection(Connection);
            // Open connection
            try
            {
                if (DataConn.State == ConnectionState.Closed)
                {
                    DataConn.Open();
                }
            }
            catch (SqlException sqex)
            {
                // Create connection failed
                return null;
            }

            SqlCommand DBCmd = new SqlCommand("GetTableName", DataConn);
            DBCmd.CommandType = CommandType.StoredProcedure;

            // Output parameter（值得注意的是：这里Add的输出变量必须与存储过程里的输出变量同名，否则会报告“dpIDS_GetSerialNumber 的@SerialNumber参数not supplied” 错误！）
            SqlParameter param = new SqlParameter("@SerialNumber", SqlDbType.VarChar, 6);
            param.Direction = System.Data.ParameterDirection.Output;
            DBCmd.Parameters.Add(param);

            try
            {

                DBCmd.ExecuteNonQuery();
                serialNumber = "Holiday" + param.Value.ToString();  //得到输出参数的值
            }
            catch (SqlException sqex)
            {
                return null;
            }

            try
            {
                if (DataConn.State == ConnectionState.Open)
                {
                    DataConn.Close();
                }
            }
            catch (SqlException sqex)
            {
                return null;
            }
            return serialNumber;
        }

        /// <summary>
        /// 将生成的企业号，转换为合法的数据库名称，作为该企业的数据库名称
        /// </summary>
        /// <param name="CompanyId">企业ID号</param>
        /// <returns>可以作为企业数据库名称的合法的字符串</returns>
        public static string GetCompanyDbName(string CompanyId)
        {
            //string result = null;
            //for (int i = 0; i < CompanyId.Length; i++)
            //{
            //    char tmp = Convert.ToChar(CompanyId[i] + 17);
            //    result += tmp;
            //}

            return "Bonsaii" + CompanyId;
        }

        public static string GenerateContractNumber(string connString)
        {
            BonsaiiDbContext db = new BonsaiiDbContext(connString);
            ParamCodes tmp = db.ParamCodes.Where(p => p.ParamName == "合同编号").Single();
            string date = DateTime.Now.ToString("yyyyMMdd");
            string SerialNumber = null;
            //为流水号补充零
            if (tmp.SerialNumber != 0)
            {
                SerialNumber = AddZero(tmp.Count, tmp.SerialNumber);
                //更员工号计数值
                tmp.Count++;
                db.Entry(tmp).State = EntityState.Modified;
                db.SaveChanges();
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
        public static string GenerateStaffNumber(string abbr,string connString)
        {
            BonsaiiDbContext db = new BonsaiiDbContext(connString);
            ParamCodes tmp = db.ParamCodes.Where(p => p.ParamName == "员工工号").Single();
            string date = DateTime.Now.ToString("yyyyMMdd");
            string SerialNumber = null;
            //为流水号补充零
            if (tmp.SerialNumber != 0)
            {
                SerialNumber = AddZero(tmp.Count, tmp.SerialNumber);
                //更员工号计数值
                tmp.Count++;
                db.Entry(tmp).State = EntityState.Modified;
                db.SaveChanges();
            }

            switch (tmp.CodeMethod)
            {
                case CodeMethod.Day:
                    return DateTime.Now.ToString("yyMMdd").ToString() + SerialNumber;
                case CodeMethod.Month:
                    return DateTime.Now.ToString("yyMM").ToString() + SerialNumber;
                case CodeMethod.Serial:
                    return tmp.Code.Substring(0, 10 - tmp.SerialNumber) + SerialNumber;
                case CodeMethod.Five:
                    return abbr + SerialNumber;
                default:
                    return "";
            }
        }
        public static string GenerateContractNumber(string abbr, string connString)
        {
            BonsaiiDbContext db = new BonsaiiDbContext(connString);
            ParamCodes tmp = db.ParamCodes.Where(p => p.ParamName == "合同编号").Single();
            string date = DateTime.Now.ToString("yyyyMMdd");
            string SerialNumber = null;
            //为流水号补充零
            if (tmp.SerialNumber != 0)
            {
                SerialNumber = AddZero(tmp.Count, tmp.SerialNumber);
                //更员工号计数值
                tmp.Count++;
                db.Entry(tmp).State = EntityState.Modified;
                db.SaveChanges();
            }

            switch (tmp.CodeMethod)
            {
                case CodeMethod.Day:
                    return DateTime.Now.ToString("yyMMdd").ToString() + SerialNumber;
                case CodeMethod.Month:
                    return DateTime.Now.ToString("yyMM").ToString() + SerialNumber;
                case CodeMethod.Serial:
                    return tmp.Code.Substring(0, 10 - tmp.SerialNumber) + SerialNumber;
                case CodeMethod.Five:
                    return abbr + SerialNumber;
                default:
                    return "";
            }
        }
        //public static string GenerateStaffNumber(string connString)
        //{
        //    BonsaiiDbContext db = new BonsaiiDbContext(connString);
        //    ParamCodes tmp = db.ParamCodes.Where(p => p.ParamName == "员工工号").Single();
        //    string date = DateTime.Now.ToString("yyyyMMdd");
        //    string SerialNumber = null;
        //    //为流水号补充零
        //    if (tmp.SerialNumber != 0)
        //    {
        //        SerialNumber = AddZero(tmp.Count, tmp.SerialNumber);
        //        //更员工号计数值
        //        tmp.Count++;
        //        db.Entry(tmp).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }

        //    switch (tmp.CodeMethod)
        //    {
        //        case CodeMethod.One:
        //            return DateTime.Now.ToString("yyyyMMdd").ToString() + SerialNumber;
        //        case CodeMethod.Two:
        //            return DateTime.Now.ToString("yyyyMM").ToString() + SerialNumber;
        //        case CodeMethod.Three:
        //            return tmp.Code.Substring(0, 10 - tmp.SerialNumber) + SerialNumber;
        //        case CodeMethod.Five:
        //            return tmp.Code.Substring(0, 10 - tmp.SerialNumber) + SerialNumber;
        //        default:
        //            return "";
        //    }
        //}

        /// <summary>
        /// 根据单据类型编号，生成单号
        /// </summary>
        /// <param name="BillTypeNumber">单据类型编号,连接字符串</param>
        /// <returns>单号</returns>
        public static string GenerateBillNumber(string BillTypeNumber, string connString)
        {
            BonsaiiDbContext db = new BonsaiiDbContext(connString);
            BillPropertyModels tmp = db.BillProperties.Where(p => p.Type == BillTypeNumber).SingleOrDefault();
            string date = DateTime.Now.ToString("yyyyMMdd");
            //为流水号补充零
            string SerialNumber = AddZero(tmp.Count, tmp.SerialNumber);
            //更新单号的计数值
            tmp.Count++;
            db.Entry(tmp).State = EntityState.Modified;
            db.SaveChanges();

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
        public static string AddZero(int SerialNumber, int length)
        {
            string tmp = SerialNumber.ToString();
            while (tmp.Length != length)
                tmp = tmp.Insert(0, "0");
            return tmp;
        }



        //获取部门信息的列表
        public static SelectList GetDepartments(string connString)
        {
            BonsaiiDbContext db = new BonsaiiDbContext(connString);
            List<Department> departments = db.Departments.ToList();

            List<SelectListItem> list = new List<SelectListItem>();
            list.Insert(0, new SelectListItem()
            {
                Text = "--请选择部门--",
            });
            foreach (Department tmp in departments)
            {
                list.Add(new SelectListItem()
                {
                    Value = tmp.DepartmentId,
                    Text = tmp.Name
                });
            }
            return new SelectList(list, "Value", "Text");
        }

        public static List<WorkDayModel> GetWorkDaysByStaffNumber(string StaffNumber, string connString)
        {
            BonsaiiDbContext db = new BonsaiiDbContext(connString);
            List<WorkDayModel> result = new List<WorkDayModel>();
            List<WorkManages> tmpWorkManages = db.WorkManages.Where(p => p.StaffNumber.Equals(StaffNumber)).OrderBy(p => p.Date).ToList();
            foreach (WorkManages tmp in tmpWorkManages)
            {
                List<WorkTimes> tmpWorkTimes = db.WorkTimes.Where(p => p.WorksId == tmp.WorksId).OrderBy(p => p.StartTime).ToList();
                string tmpWorkTime = "";
                foreach (WorkTimes tmpWT in tmpWorkTimes)
                {
                    tmpWorkTime += tmpWT.StartTime + "-" + tmpWT.EndTime + "<br/>";
                }
                result.Add(new WorkDayModel()
                {
                    Date = tmp.Date,
                    WorkTime = tmpWorkTime
                });
            }
            return result;
        }
        public static SelectList GetWorks(string connString)
        {
            BonsaiiDbContext db = new BonsaiiDbContext(connString);
            List<Works> works = db.Works.ToList();

            List<SelectListItem> list = new List<SelectListItem>();
            list.Insert(0, new SelectListItem()
            {
                Text = "-- 请选择班次--",
            });
            foreach (Works tmp in works)
            {
                list.Add(new SelectListItem()
                {
                    Value = tmp.Id.ToString(),
                    Text = tmp.Name
                });
            }
            return new SelectList(list, "Value", "Text");
        }

   




    }
}