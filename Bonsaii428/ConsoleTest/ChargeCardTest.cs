using Bonsaii.Models;
using Bonsaii.Models.Works;
using BonsaiiModels.GlobalStaticVaribles;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bonsaii.Models.Checking_in;
namespace ConsoleTest
{
    class ChargeCardTest
    {
        //public void ChargeCardDell() {
        //    BonsaiiDbContext db = new BonsaiiDbContext("Data Source = 211.149.199.42,1433;Initial Catalog = Bonsaii0000000008;User ID = sa;Password = admin123@;");
        //    SystemDbContext sysDb = new SystemDbContext("Data Source = 211.149.199.42,1433;Initial Catalog = BonsaiiSystem;User ID = sa;Password = admin123@;");
        //    List<ChargeCardApplies> ChargeCardApplies = db.ChargeCardApplies.Where(p => p.AuditStatus == 3).ToList();
        //    ChargeCardData tmp = new ChargeCardData();
        //    foreach (ChargeCardApplies cardapply in ChargeCardApplies)
        //    {
        //        tmp.Date = cardapply.DateTime.Date; 
        //        tmp.Time = cardapply.DateTime.TimeOfDay;
        //        tmp.StaffNumber = cardapply.StaffNumber;
        //        try
        //        {
        //            tmp.StaffName = db.Staffs.Where(p => p.StaffNumber == cardapply.StaffNumber).Single().Name;
        //            var DepartmentId = db.Staffs.Where(p => p.StaffNumber == cardapply.StaffNumber).Single().Department;
        //            tmp.DepartmentName = db.Departments.Where(p => p.DepartmentId == DepartmentId).Single().Name;
        //        }
        //        catch (Exception e)
        //        {
        //           Program1.WriteErrorLog(e);
        //            return;
        //        }
        //        db.ChargeCardData.Add(tmp);
        //        db.SaveChanges();
        //    }

        //}
    }
}
