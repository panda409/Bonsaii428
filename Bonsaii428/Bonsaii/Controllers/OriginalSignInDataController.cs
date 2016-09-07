using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models.Checking_in;
namespace Bonsaii.Controllers
{
    public class OriginalSignInDataController : BaseController
    {
        public ActionResult Index()
        {
            List<OriginalSignInDataViewModel> datas = (from x in db.DeviceOriginalDatas
                                                        join y in db.Staffs on x.UserID equals y.PhysicalCardNumber
                                                        join z in db.Departments on y.Department equals z.DepartmentId
                                                        select new OriginalSignInDataViewModel()
                                                        {
                                                            StaffNumber = y.StaffNumber,
                                                            StaffName = y.Name,
                                                            DepartmentName = z.Name,
                                                            Date = x.DateTime,
                                                            Type = "考勤机打卡"
                                                        }).ToList();

            List<OriginalSignInDataViewModel> appDatas = (from x in db.OffSiteApplies
                                                          join y in db.Staffs on x.StaffNumber equals y.StaffNumber
                                                          join z in db.Departments on y.Department equals z.DepartmentId
                                                          select new OriginalSignInDataViewModel()
                                                          {
                                                              StaffNumber = y.StaffNumber,
                                                              StaffName = y.Name,
                                                              DepartmentName = z.Name,
                                                              Date = x.Date,
                                                              Type = "异地打卡"
                                                          }).ToList();

            datas.AddRange(appDatas);
            return View(datas);
        }
    }
}