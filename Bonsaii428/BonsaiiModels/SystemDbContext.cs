using BonsaiiModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    public class SystemDbContext : DbContext
    {
         public SystemDbContext()
            : base("DefaultConnection")
        {
        }

         public SystemDbContext(string conn) : base(conn) { }
        //public static ApplicationDbContext Create()
        //{
        //    return new ApplicationDbContext();
        //}

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public DbSet<Company> Companies { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.GroupCompany> GroupCompanies { get; set; }

        public DbSet<VerifyCode> VerifyCodes { get; set; }

        public DbSet<UserModels> Users { get; set; }

        public DbSet<Actions> Actions { get; set; }


        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RoleModels> Roles { get; set; }
        public DbSet<Dimension> Dimensions { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.UserViewModels> UserViewModels { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.StaffChange> StaffChanges { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.UserPasswordInfo> UserPasswordInfos { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.AdviceBack> AdviceBacks { get; set; }

        public DbSet<Authorize> Authorizes { get; set; }
        public DbSet<Bonsaii.Models.App.App_User> App_Users { get; set; }
        public DbSet<Bonsaii.Models.App.App_Sn> App_Sns { get; set; }

        public System.Data.Entity.DbSet<BonsaiiModels.SubscribeList> SubscribeLists { get; set; }
        public System.Data.Entity.DbSet<BonsaiiModels.Push> Pushes { get; set; }
        public System.Data.Entity.DbSet<BonsaiiModels.Subscribe.Subscribe_Company> Subscribe_Companies { get; set; }

        public DbSet<AuthorizeGroup> AuthorizeGroups { get; set; }
       
        public DbSet<BonsaiiModels.BindCode> BindCodes { get; set; }
        public System.Data.Entity.DbSet<BonsaiiModels.SystemMessage> SystemMessages { get; set; }
        public DbSet<BonsaiiModels.Nation> nations { get; set; }
    }
}