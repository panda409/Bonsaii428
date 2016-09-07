using Bonsaii.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Bonsaii.Controllers
{
    public class TmpDbContext : DbContext
    {
        private SystemDbContext db = new SystemDbContext();
        public TmpDbContext()
            : base("DefaultConnection")
        {

        }

        public DbSet<VerifyCode> verifyCodes { get; set; }
    }
}