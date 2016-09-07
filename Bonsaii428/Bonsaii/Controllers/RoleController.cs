using Bonsaii.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System;

namespace Bonsaii.Controllers
{
    public class RoleController : Controller
    {
        public RoleController()
        {
        }

        public RoleController(ApplicationUserManager userManager,
            ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        ////
        //// GET: /Roles/
        //public ActionResult Index()
        //{
        //    ApplicationDbContext context = new ApplicationDbContext();
        //    return View(context.Roles.ToList());
        //}

        //
        // GET: /Roles/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManager.FindByIdAsync(id);
            // Get the list of Users in this Role
            var users = new List<ApplicationUser>();

            // Get the list of Users in this Role
            foreach (var user in UserManager.Users.ToList())
            {
                if (await UserManager.IsInRoleAsync(user.Id, role.Name))
                {
                    users.Add(user);
                }
            }

            ViewBag.Users = users;
            ViewBag.UserCount = users.Count();
            return View(role);
        }

        ////
        //// GET: /Roles/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        public ActionResult Index()
        {

            List<IdentityRole> Roles = RoleManager.Roles.ToList();
            return View(Roles);
        }

        public ActionResult DeleteAllRoles()
        {
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var Roles = RoleManager.Roles;
            foreach (var tmp in Roles)
                RoleManager.Delete(tmp);
            return RedirectToAction("Index");
        }
        public void CreateRoles()
        {
            string[] Roles = {"admin","guest" };
            foreach (string name in Roles)
            {
                var role = new IdentityRole(name);
                var RoleManager = HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
                if (RoleManager == null)
                    return;
                var roleresult = RoleManager.Create(role);
            }
        }


        public JsonResult GetAllRoles()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<Object> obj = new List<Object>();
            var RoleNames = db.Roles;
            foreach (var name in RoleNames)
            {
                obj.Add(new { text = name.Name, id = name.Name });
            }
            return Json(obj);
        }
        //
        // POST: /Roles/Create
        //[HttpPost]
        //public async Task<ActionResult> Create(RoleViewModel roleViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var role = new IdentityRole(roleViewModel.Name);
        //        var roleresult = await RoleManager.CreateAsync(role);
        //        if (!roleresult.Succeeded)
        //        {
        //            ModelState.AddModelError("", roleresult.Errors.First());
        //            return View();
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

        //
        // POST: /Roles/Edit/5
        //[HttpPost]

        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "Name,Id")] RoleViewModel roleModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var role = await RoleManager.FindByIdAsync(roleModel.Id);
        //        role.Name = roleModel.Name;
        //        await RoleManager.UpdateAsync(role);
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

        //
        // GET: /Roles/Delete/5
        //public async Task<ActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var role = await RoleManager.FindByIdAsync(id);
        //    if (role == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(role);
        //}

        ////
        //// POST: /Roles/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(string id, string deleteUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }
        //        var role = await RoleManager.FindByIdAsync(id);
        //        if (role == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        IdentityResult result;
        //        if (deleteUser != null)
        //        {
        //            result = await RoleManager.DeleteAsync(role);
        //        }
        //        else
        //        {
        //            result = await RoleManager.DeleteAsync(role);
        //        }
        //        if (!result.Succeeded)
        //        {
        //            ModelState.AddModelError("", result.Errors.First());
        //            return View();
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

        public JsonResult GetAllActions()
        {
            SystemDbContext db = new SystemDbContext();
            List<Object> texts = new List<Object>();
            var ControllerNames = db.Actions.Select(p => p.ControllerName).Distinct();
            
            foreach (var name in ControllerNames)
            {
                SystemDbContext tmpDb = new SystemDbContext();
                //获取每一个Controller下面的所有Actoin
                var tmpActions = tmpDb.Actions.Where(p => p.ControllerName.Equals(name));
                List<Object> children = new List<Object>();
                foreach (var tmp in tmpActions)
                {
                    children.Add(new { text = tmp.ActionShowName, id = tmp.ActionId });
                }
                texts.Add(new { text = name, children = children });
            }
            return Json(texts);
            //return Json(new
            //{
            //    errorcode = "1"
            //});
        }



    }
}