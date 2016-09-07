using Bonsaii.Models.Checking_in;
using BonsaiiModels;
using BonsaiiModels.App;
using BonsaiiModels.GlobalStaticVaribles;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    public class BonsaiiDbContext : DbContext
    {
        //含有连接字符串参数的构造函数，会连接参数中传递的数据库
        public BonsaiiDbContext(string conn)
            : base(conn)
        {
        }
        public BonsaiiDbContext()
            : base("DefaultConnection")
        { 
        
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public DbSet<Company> Companies { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.GroupCompany> GroupCompanies { get; set; }

        public DbSet<VerifyCode> VerifyCodes { get; set; }

        public DbSet<UserModels> Users { get; set; }


        public DbSet<UserRole> UserRoles { get; set; }
       // public System.Data.Entity.DbSet<Bonsaii.Models.> Departments { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.Phrase> Phrases { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.PhraseScene> PhraseScenes { get; set; }
        public DbSet<Staff> Staffs { get; set; }


       
        public System.Data.Entity.DbSet<Bonsaii.Models.Background> Backgrounds { get; set; }

        
        public System.Data.Entity.DbSet<Bonsaii.Models.StaffChange> StaffChanges { get; set; }
        public System.Data.Entity.DbSet<Bonsaii.Models.StaffChangeReserve> StaffChangeReserves { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.StaffApplication> StaffApplications { get; set; }
        public System.Data.Entity.DbSet<Bonsaii.Models.StaffApplicationReserve> StaffApplicationReserves { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.StaffArchive> StaffArchives { get; set; }
        public System.Data.Entity.DbSet<Bonsaii.Models.StaffArchiveReserve> StaffArchiveReserves { get; set; }

        public DbSet<BillPropertyModels> BillProperties { get; set; }

        public DbSet<ParamCodes> ParamCodes { get; set; }

        public DbSet<Params> Params { get; set; }

        public DbSet<StaffParam> StaffParams { get; set; }
        public DbSet<StaffParamType> StaffParamTypes { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.StaffBasicParam> StaffBasicParams { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.Recruitments> Recruitments { get; set; }
        public System.Data.Entity.DbSet<Bonsaii.Models.RecruitmentReserve> RecruitmentReserves { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.RecordDatetime> RecordDatetimes { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.WeekTag> WeekTags { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.Holiday> Holidays { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.Contract> Contracts { get; set; }
        public System.Data.Entity.DbSet<Bonsaii.Models.ContractReserve> ContractReserves { get; set; }


        public System.Data.Entity.DbSet<Bonsaii.Models.StaffSkill> StaffSkills { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.DepartmentReserve> DepartmentReserves { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.ReserveField> ReserveFields { get; set; }
        public System.Data.Entity.DbSet<Bonsaii.Models.StaffSkillReserve> StaffSkillReserves { get; set; }
        public System.Data.Entity.DbSet<Bonsaii.Models.StaffReserve> StaffReserves { get; set; }

   
        public System.Data.Entity.DbSet<Bonsaii.Models.Department> Departments { get; set; }
        public System.Data.Entity.DbSet<Bonsaii.Models.DataControl> DataControls { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.BillStaffMapping> BillStaffMappings { get; set; }

        public DbSet<BillSort> BillSorts { get; set; }
        public DbSet<DataSubscriptions> DataSubscriptions { get; set; }

        public DbSet<TestModels> TestModels { get; set; }
        public DbSet<HolidayRecord> HolidayRecords { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.HolidayTimeRecord> HolidayTimeRecords { get; set; }
        public System.Data.Entity.DbSet<Bonsaii.Models.HolidayTimeName> HolidayTimeNames { get; set; }


        public System.Data.Entity.DbSet<Bonsaii.Models.Audit.AuditStep> AuditSteps { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.Audit.AuditProcess> AuditProcesses { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.Audit.AuditTemplate> AuditTemplates { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.Audit.AuditApplication> AuditApplications { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.Audit.State> States { get; set; }


        public System.Data.Entity.DbSet<Bonsaii.Models.BrandTemplateModels> BrandTemplateModels { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.BrandTemplateReserve> BrandTemplateReserves { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.Brand> Brands { get; set; }
        public System.Data.Entity.DbSet<Bonsaii.Models.ConfirmedField> ConfirmedFields { get; set; }


        public DbSet<Bonsaii.Models.Works.Works> Works { get; set; }
        public DbSet<Bonsaii.Models.Works.WorkManages> WorkManages { get; set; }
        public DbSet<Bonsaii.Models.Works.WorkTimes> WorkTimes { get; set; }

        public DbSet<VacateApplies> VacateApplies { get; set; }
        public DbSet<ChargeCardApplies> ChargeCardApplies { get; set; }
        public DbSet<OvertimeApplies> OvertimeApplies { get; set; }
        public DbSet<EvectionApplies> EvectionApplies { get; set; }

        public DbSet<OriginalDataofCheckOn> OriginalDataofCheckOn { get; set; }

        public DbSet<SignInCardStatus> SignInCardStatus { get; set; }
        public DbSet<EveryDaySignInDate> EveryDaySignInDates { get; set; }


        public DbSet<ChargeCardData> ChargeCardData { get; set; }

        public DbSet<TableNameContrast> TableNameContrasts { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.TrainRecord> TrainRecords { get; set; }
        public System.Data.Entity.DbSet<Bonsaii.Models.TrainStart> TrainStarts { get; set; }
        public System.Data.Entity.DbSet<Bonsaii.Models.TrainStartReserve> TrainStartReserves { get; set; }

        public DbSet<CheckingInParams> CheckingInParams { get; set; }
        public DbSet<CheckingInParamsBool> CheckingInParamsBools { get; set; }
        public System.Data.Entity.DbSet<BonsaiiModels.HolidayTables> HolidayTables { get; set; }
        public System.Data.Entity.DbSet<BonsaiiModels.Subscribe.SubscribeAndWarning> SubscribeAndWarnings { get; set; }
        public System.Data.Entity.DbSet<BonsaiiModels.Backlog> Backlogs { get; set; }


        public DbSet<MonthSignIn> MonthSignIns { get; set; }

        public DbSet<OnDutyApplies> OnDutyApplies { get; set; }

        public DbSet<OnDutyHours> OnDutyHours { get; set; }

        public DbSet<DaysOffApplies> DaysOffApplies { get; set; }

        public System.Data.Entity.DbSet<BonsaiiModels.Devices> Devices { get; set; }
        public System.Data.Entity.DbSet<BonsaiiModels.Push> Pushes { get; set; }

        public System.Data.Entity.DbSet<BonsaiiModels.CompanyPush> CompanyPushes { get; set; }
        public System.Data.Entity.DbSet<BonsaiiModels.App.AAKaoQin> AAKaoQins { get; set; }

        public System.Data.Entity.DbSet<BonsaiiModels.App.AAYiDiKaoQin> AAYiDiKaoQins { get; set; }
        public System.Data.Entity.DbSet<BonsaiiModels.RestApply> RestApplies { get; set; }
        public System.Data.Entity.DbSet<BonsaiiModels.App.VacatePhoto> VacatePhotos { get; set; }

        public DbSet<Menu> Menus { get; set; }
        public System.Data.Entity.DbSet<BonsaiiModels.StaffVariation.StaffVariation> StaffVariations { get; set; }

        public DbSet<DeviceOriginalData> DeviceOriginalDatas { get; set; }

        public DbSet<OffSiteApplies> OffSiteApplies { get; set; }

        public System.Data.Entity.DbSet<BonsaiiModels.Staffs.SerialCodeCount> SerialCodeCounts { get; set; }

    }
}