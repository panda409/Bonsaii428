using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bonsaii.Controllers
{
    public class EChartController : BaseController
    {
        // GET: EChart
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult EChart()
        {
            return View();
        }

        public JsonResult GetDepartmentStaffs()
        {
            List<string> dptNames = new List<string>();
            List<int> stfCount = new List<int>();
            string sqlStr = "select departments.name,count(distinct staffs.name) as numbers from staffs left join departments on departments.departmentId=staffs.department group by departments.name;";
            /**
             * 这里有个很坑的错误是：“已有打开的与此Command相关联的DataReader”
             * 这个问题出现的原因是由于使用过的SqlDataReader没有关闭，然而在我的代码中之前并没有使用过这个东西，
             * 而且放到其他人的电脑上这段代码还是可以正常运行的，至今没有找到到底哪里打开了一个SqlDataReader……
             * 最后的解决方法是，在连接字符串当中加上MultipleActiveResultSets=True这个配置，
             * 大意是允许
             * */
            string conn = this.ConnectionString + "MultipleActiveResultSets=True";
            using (SqlConnection myConn = new SqlConnection(conn))
            {
                SqlCommand myCommand = new SqlCommand(sqlStr, myConn);
                myConn.Open();
                SqlDataReader reader = myCommand.ExecuteReader();
                while (reader.Read())
                {
                    dptNames.Add(reader.GetString(0));
                    stfCount.Add(reader.GetInt32(1));
                }
            }
            return Json(new
            {
                dptNames = dptNames,
                stfCount = stfCount
            });
        }
        public JsonResult GetStaffEducation()
        {
            var result = (from p in db.Staffs
                          group p by p.EducationBackground into g
                          where g.Key != null
                          select new
                          {
                              name = g.Key,
                              value = g.Count()
                          }).ToList();
            List<string> edu = new List<string>();
            foreach (var tmp in result)
                edu.Add(tmp.name);
            return Json(new
            {
                eduNames = edu,
                eduStaffs = result
            });
        }
    }
}