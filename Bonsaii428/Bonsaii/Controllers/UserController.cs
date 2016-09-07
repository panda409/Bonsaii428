using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using BonsaiiModels;
namespace Bonsaii.Controllers
{
    public class UserController : BaseController
    {
        private SystemDbContext SysContext = new SystemDbContext();

        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
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

        public void DeleteUser(string UserName)
        {
            ApplicationUser user = UserManager.FindByName(UserName);
            IList<string> roles = UserManager.GetRoles(user.Id);
            UserManager.RemoveFromRoles(user.Id, roles.ToArray());
            UserManager.Delete(user);
            db.SaveChanges();
        }


        public ActionResult CreateRoles()
        {
            //string[] roles = {
            //                      "Admin",
            //                      "User",
            //                      "ParamCodes",
            //                      "StaffParams",
            //                      "StaffBasicParams",
            //                      "StaffParamTypes",
            //                      "BillPropertyModels",
            //                      "DataControl",
            //                      "ReserveField",
            //                      "Company",
            //                      "Department",
            //                      "Staff",
            //                      "StaffChange",
            //                      "StaffApplication",
            //                      "StaffApplicationManage",
            //                      "StaffArchive",
            //                      "StaffSkill",
            //                      "Contract",
            //                      "BrandTemplateModel",
            //                      "Brand",
            //                      "Recruitments",
            //                      "ChargeCardApplies",
            //                      "EvectionApplies",
            //                      "VacateApplies",
            //                      "OvertimeApplies",
            //                      "TrainStart",
            //                      "TrainRecord",
            //                      "Works",
            //                      "WorkTimes",
            //                  };
            //List<string> commonRoles = ControllerAdd(roles);
            //string[] roles2 = { 
            //                      "Admin",
            //                        "zTree_Index",
            //                        "Staff_IndexMain",
            //                        "Recruitments_PublishRecruitments",
            //                        "Recruitments_PreviewRecruitments",
            //                        "WorkManages_PersonalCreate",
            //                        "WorkManages_PersonalIndex",
            //                        "WorkManages_DepartmentCreate",
            //                        "WorkManages_DepartmentIndex"
            //                    };

            //foreach (string tmp in commonRoles)
            //    RoleManager.Create(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole(tmp));
            //foreach (string tmp in roles2)
            //    RoleManager.Create(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole(tmp));


            string[] roles3 = {
                              "OnDutyApplies_Index",
                              "OnDutyApplies_Edit",
                              "OnDutyApplies_Create",
                              "OnDutyApplies_Delete",
                              "DaysOffApplies_Index",
                              "DaysOffApplies_Create",
                              "DaysOffApplies_Delete",
                              };
            foreach (string tmp in roles3)
                RoleManager.Create(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole(tmp));
            return RedirectToAction("IndexRoles");
        }

        public void TmpAddRole()
        {
            RoleManager.Create(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole("Staff_IndexMain"));
        }

        public List<string> ControllerAdd(string[] str)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < str.Length; i++)
            {
                list.Add(str[i] + "_Index");
                list.Add(str[i] + "_Create");
                list.Add(str[i] + "_Edit");
                list.Add(str[i] + "_Delete");
            }
            return list;
        }



        /** 一些增加、删除角色的函数**/
        public ActionResult DeleteRoles()
        {
            var roles = RoleManager.Roles.ToList();
            /**这里千万要注意：一定要加上ToList这个方法！否则在下面的循环中调用Delete的时候会报错误：“已有打开的与此Command相关联的DataReader”。
             * 出现这个错误的原因大概是：這是因为EF內部是使用DataReader作资料存取，没有ToList的时候，查询到的结果并不会写入内存，可以认为数据还在DataReader当中待取用
             * 当在下面调用Delete的时候也会调用DataReader来进行操作，这样两个操作都用通过一个Connection的DataReader来进行，就会报错了
             * */
            foreach (var tmp in roles)
            {
                if (tmp.Users.Count == 0)
                    RoleManager.Delete(tmp);
            }
            return RedirectToAction("IndexRoles");
        }


        public ActionResult IndexRoles()
        {

            return View(RoleManager.Roles.OrderBy(p => p.Name).ToList());
        }
        /**     上面是一些增加、删除角色的函数     **/



        [Authorize(Roles = "Admin,User_Create")]
        public ActionResult Create()
        {

            SystemDbContext db = new SystemDbContext();
            List<BonsaiiModels.Authorize> tmpList = db.Authorizes.ToList();
            List<string> ModuleNames = tmpList.Select(p => p.ModuleName).Distinct().ToList();

            List<ModuleViewModel> result = new List<ModuleViewModel>();
            foreach (string tmpModule in ModuleNames)
            {
                ModuleViewModel tmpModuleviewModel = new ModuleViewModel();
                tmpModuleviewModel.ModuleName = tmpModule;
                tmpModuleviewModel.UnitCount = tmpList.Where(p => p.ModuleName.Equals(tmpModule)).Select(p => p.UnitName).Distinct().Count();
                List<string> UnitNames = tmpList.Where(p => p.ModuleName.Equals(tmpModule)).Select(p => p.UnitName).Distinct().ToList();

                List<UnitViewModel> units = new List<UnitViewModel>();
                foreach (string tmpUnit in UnitNames)
                {
                    UnitViewModel tmpUnitViewModel = new UnitViewModel();
                    tmpUnitViewModel.UnitName = tmpUnit;
                    tmpUnitViewModel.UnitId = tmpList.Where(p => p.UnitName.Equals(tmpUnit)).Select(p => p.UnitId).First();
                    tmpUnitViewModel.Actions = tmpList.Where(p => p.UnitName.Equals(tmpUnit)).Select(p => new AuthorizeViewModel()
                    {
                        ActionName = p.ActionName,
                        ActionValue = p.ActionValue,
                        Name = p.Name
                    }).ToList();
                    units.Add(tmpUnitViewModel);
                }
                tmpModuleviewModel.Units = units;


                List<AuthorizeGroup> tmpAuthorizeGroup = db.AuthorizeGroups.ToList();
                Dictionary<string, string> authMap = new Dictionary<string, string>();
                foreach (AuthorizeGroup tmp in tmpAuthorizeGroup)
                {
                    if (!authMap.ContainsKey(tmp.GroupName))
                        authMap.Add(tmp.GroupName, tmp.ActionName);
                    else
                        authMap[tmp.GroupName] += "," + tmp.ActionName;
                }


                ViewBag.authGroup = authMap;

                result.Add(tmpModuleviewModel);
            }
            ViewBag.Roles = result;
            return View();
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


        // POST: Users/Create
        [Authorize(Roles = "Admin,User_Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FormCollection model)
        {

            var user = new ApplicationUser
            {
                CompanyFullName = base.CompanyFullName,
                CompanyId = base.CompanyId,
                ConnectionString = base.ConnectionString,
                Name = model["Name"],
                PhoneNumber = model["UserName"],
                UserName = model["UserName"],
                IsProved = true,           //是否审核的标志
                IsAvailable = true,         //是否是可用的管理员
                IsRoot = false               //非企业号的注册人默认就是非企业的超级管理员
            };

            var result = await UserManager.CreateAsync(user, model["Password"]);

            if (result.Succeeded)
            {
                var editUser = UserManager.FindByName(user.UserName);           //根据用户提交的信息获取用户信息
                //遍历用户选择的所有角色，将用于添加到每一个角色当中
                string[] FormKeys = model.AllKeys;
                
                //是超级管理员
                if (FormKeys.Contains("Admin"))
                    UserManager.AddToRole(editUser.Id, "Admin");
                else
                    foreach (string tmp in FormKeys)
                        if (tmp.Contains("Auth_"))
                            UserManager.AddToRole(editUser.Id, tmp.Substring(5));
                return RedirectToAction("Index");
            }

            AddErrors(result);
            //如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }





        [Authorize(Roles = "Admin,User_Index")]
        // GET: Users
        public ActionResult Index()
        {
            var user = UserManager.FindByName(base.UserName);
            //     UserManager.RemoveFromRole(user.Id, "Admin");
            //     UserManager.AddToRole(user.Id, "Admin");
            var ro = user.Roles.ToList();
            IList<string> roles = UserManager.GetRoles(user.Id);
            var users = SysContext.Users.Where(p => p.CompanyId == CompanyId);
            return View(users.ToList());
        }

        public ActionResult Test()
        {
            var user = UserManager.FindByName(base.UserName);
            UserManager.AddToRole(user.Id, "Admin");

            return RedirectToAction("Index");
        }
        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserModels user = SysContext.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [Authorize(Roles = "Admin,User_Edit")]
        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserModels user = SysContext.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            string[] roles = (from d in SysContext.Roles
                              join x in SysContext.UserRoles on d.Id equals x.RoleId into temp
                              from tt in temp.DefaultIfEmpty()
                              where tt.UserId == id
                              select d.Name).ToArray();
            if (roles.Contains("Admin"))
                ViewBag.Roles = "Admin";
            else
                foreach (string tmp in roles)
                    ViewBag.Roles += tmp + ";";


            SystemDbContext db = new SystemDbContext();
            List<BonsaiiModels.Authorize> tmpList = db.Authorizes.ToList();
            List<string> ModuleNames = tmpList.Select(p => p.ModuleName).Distinct().ToList();

            List<ModuleViewModel> result = new List<ModuleViewModel>();
            foreach (string tmpModule in ModuleNames)
            {
                ModuleViewModel tmpModuleviewModel = new ModuleViewModel();
                tmpModuleviewModel.ModuleName = tmpModule;
                tmpModuleviewModel.UnitCount = tmpList.Where(p => p.ModuleName.Equals(tmpModule)).Select(p => p.UnitName).Distinct().Count();
                List<string> UnitNames = tmpList.Where(p => p.ModuleName.Equals(tmpModule)).Select(p => p.UnitName).Distinct().ToList();

                List<UnitViewModel> units = new List<UnitViewModel>();
                foreach (string tmpUnit in UnitNames)
                {
                    UnitViewModel tmpUnitViewModel = new UnitViewModel();
                    tmpUnitViewModel.UnitName = tmpUnit;
                    tmpUnitViewModel.UnitId = tmpList.Where(p => p.UnitName.Equals(tmpUnit)).Select(p => p.UnitId).First();
                    tmpUnitViewModel.Actions = tmpList.Where(p => p.UnitName.Equals(tmpUnit)).Select(p => new AuthorizeViewModel()
                    {
                        ActionName = p.ActionName,
                        ActionValue = p.ActionValue,
                        Name = p.Name
                    }).ToList();
                    units.Add(tmpUnitViewModel);
                }
                tmpModuleviewModel.Units = units;
                result.Add(tmpModuleviewModel);
            }
            ViewBag.AllRoles = result;

            List<AuthorizeGroup> tmpAuthorizeGroup = db.AuthorizeGroups.ToList();
            Dictionary<string, string> authMap = new Dictionary<string, string>();
            foreach (AuthorizeGroup tmp in tmpAuthorizeGroup)
            {
                if (!authMap.ContainsKey(tmp.GroupName))
                    authMap.Add(tmp.GroupName, tmp.ActionName);
                else
                    authMap[tmp.GroupName] += "," + tmp.ActionName;
            }
            ViewBag.authGroup = authMap;
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,User_Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection model)
        {
            var User = UserManager.FindById(model["Id"]);
            User.Name = model["Name"];


            //删除原来的角色
            string[] roles = (from d in SysContext.Roles
                              join x in SysContext.UserRoles on d.Id equals x.RoleId into temp
                              from tt in temp.DefaultIfEmpty()
                              where tt.UserId == User.Id
                              select d.Name).ToArray();
            foreach (string tmp in roles)
                UserManager.RemoveFromRole(User.Id, tmp);

            string[] FormKeys = model.AllKeys;
            if (FormKeys.Contains("Admin"))
                UserManager.AddToRole(User.Id, "Admin");
            else
                foreach (string tmp in FormKeys)
                    if (tmp.Contains("Auth_"))
                        UserManager.AddToRole(User.Id, tmp.Substring(5));
            //    修改密码
            if (!model["Password"].Trim().Equals(""))
            {
                ApplicationUser user = UserManager.FindByName(model["UserName"]);
                var code = UserManager.GeneratePasswordResetToken(user.Id);
                var result = UserManager.ResetPassword(user.Id, code, model["Password"]);
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin,User_Delete")]
        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserModels user = SysContext.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [Authorize(Roles = "Admin,User_Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserModels user = SysContext.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            string[] q = (from d in SysContext.Roles
                          join x in SysContext.UserRoles on d.Id equals x.RoleId into temp
                          from tt in temp.DefaultIfEmpty()
                          where tt.UserId == id
                          select d.Name).ToArray();
            //删除用户的角色
            //删除用户原来属于的角色
            for (int i = 0; i < q.Length; i++)
            {
                UserManager.RemoveFromRole(user.Id, q[i]);
            }
            SysContext.Users.Remove(user);
            SysContext.SaveChanges();
            return RedirectToAction("Index");
        }

        //Get
        public ActionResult Auth(string id)
        {
            List<string> q = (from d in SysContext.Roles
                              join x in SysContext.UserRoles on d.Id equals x.RoleId into temp
                              from tt in temp.DefaultIfEmpty()
                              where tt.UserId == id
                              select d.Name).ToList();

            /** 查询SQL
                    select Roles.Name
                    from Roles left join UserRoles
                    on Roles.Id = UserRoles.RoleId
                    where UserRoles.UserId = '8e3dcc4d-17b8-4f89-8821-fed9969b221c';
             **/

            var allRoles = SysContext.Roles.ToList();
            ViewBag.Roles = q;
            ViewBag.UserId = id;
            return View(allRoles);
        }
        [HttpPost]
        public ActionResult Auth(FormCollection collection)
        {
            string id = collection["UserId"];
            //获取用户原来属于的角色
            string[] preRoles = (from d in SysContext.Roles
                                 join x in SysContext.UserRoles on d.Id equals x.RoleId into temp
                                 from tt in temp.DefaultIfEmpty()
                                 where tt.UserId == id
                                 select d.Name).ToArray();
            //获取用户新勾选的角色
            string[] newRoles = collection["SelectedRoles"].Split(new char[] { ',' });
            //再给某一个用户赋予新的角色之前，要把他之前所属的角色全部删除掉

            //删除用户原来属于的角色
            for (int i = 0; i < preRoles.Length; i++)
            {
                UserManager.RemoveFromRole(id, preRoles[i]);
            }
            if (newRoles[0] != "")
                //将用户添加到新选择的角色当中
                UserManager.AddToRoles(id, newRoles);
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SysContext.Dispose();
            }
            base.Dispose(disposing);
        }


        /**
 * 管理员信息管理函数
 * */

        // GET: Users/Create
        public ActionResult CreateAdmin()
        {

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAdmin(UserViewModels model)
        {
            //   var CurUser = UserManager.FindByName(base.UserName);
            //     string[] roles = Request["userRoles"].Split(new Char[] { ',' });      //获得该用户对应的角色集合

            var user = new ApplicationUser
            {
                CompanyFullName = base.CompanyFullName,
                CompanyId = base.CompanyId,
                ConnectionString = base.ConnectionString,
                PhoneNumber = model.UserName,
                UserName = model.UserName,
                IsProved = base.IsProved,           //是否审核的标志
                IsAvailable = true,         //是否是可用的管理员
                IsRoot = false               //非企业号的注册人默认就是非企业的超级管理员
            };
            var result = await UserManager.CreateAsync(user, model.UserName.Substring(5, 6));


            if (result.Succeeded)
            {
                //var editUser = UserManager.FindByName(user.UserName);           //根据用户提交的信息获取用户信息
                ////遍历用户选择的所有角色，将用于添加到每一个角色当中
                //for (int i = 0; i < roles.Length - 1; i++)
                //    UserManager.AddToRole(editUser.Id, roles[i]);

                return RedirectToAction("Index");
            }
            AddErrors(result);
            return View(model);
        }


        public ActionResult SwitchIsAvailable(string id)
        {
            using (SystemDbContext con = new SystemDbContext())
            {
                UserModels user = con.Users.Find(id);
                user.IsAvailable = user.IsAvailable ? false : true;
                con.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult TestCheckbox()
        {
            SystemDbContext db = new SystemDbContext();
            List<BonsaiiModels.Authorize> tmpList = db.Authorizes.ToList();
            List<string> ModuleNames = tmpList.Select(p => p.ModuleName).Distinct().ToList();

            List<ModuleViewModel> result = new List<ModuleViewModel>();
            foreach (string tmpModule in ModuleNames)
            {
                ModuleViewModel tmpModuleviewModel = new ModuleViewModel();
                tmpModuleviewModel.ModuleName = tmpModule;
                tmpModuleviewModel.UnitCount = tmpList.Where(p => p.ModuleName.Equals(tmpModule)).Select(p => p.UnitName).Distinct().Count();
                List<string> UnitNames = tmpList.Where(p => p.ModuleName.Equals(tmpModule)).Select(p => p.UnitName).Distinct().ToList();

                List<UnitViewModel> units = new List<UnitViewModel>();
                foreach (string tmpUnit in UnitNames)
                {
                    UnitViewModel tmpUnitViewModel = new UnitViewModel();
                    tmpUnitViewModel.UnitName = tmpUnit;
                    tmpUnitViewModel.UnitId = tmpList.Where(p => p.UnitName.Equals(tmpUnit)).Select(p => p.UnitId).First();
                    tmpUnitViewModel.Actions = tmpList.Where(p => p.UnitName.Equals(tmpUnit)).Select(p => new AuthorizeViewModel()
                    {
                        ActionName = p.ActionName,
                        ActionValue = p.ActionValue,
                        Name = p.Name
                    }).ToList();
                    units.Add(tmpUnitViewModel);
                }
                tmpModuleviewModel.Units = units;
                result.Add(tmpModuleviewModel);
            }
            return View(result);
        }


        public ActionResult CustomAuthGroup()
        {
            SystemDbContext db = new SystemDbContext();
            List<BonsaiiModels.Authorize> tmpList = db.Authorizes.ToList();
            List<string> ModuleNames = tmpList.Select(p => p.ModuleName).Distinct().ToList();

            List<ModuleViewModel> result = new List<ModuleViewModel>();
            foreach (string tmpModule in ModuleNames)
            {
                ModuleViewModel tmpModuleviewModel = new ModuleViewModel();
                tmpModuleviewModel.ModuleName = tmpModule;
                tmpModuleviewModel.UnitCount = tmpList.Where(p => p.ModuleName.Equals(tmpModule)).Select(p => p.UnitName).Distinct().Count();
                List<string> UnitNames = tmpList.Where(p => p.ModuleName.Equals(tmpModule)).Select(p => p.UnitName).Distinct().ToList();

                List<UnitViewModel> units = new List<UnitViewModel>();
                foreach (string tmpUnit in UnitNames)
                {
                    UnitViewModel tmpUnitViewModel = new UnitViewModel();
                    tmpUnitViewModel.UnitName = tmpUnit;
                    tmpUnitViewModel.UnitId = tmpList.Where(p => p.UnitName.Equals(tmpUnit)).Select(p => p.UnitId).First();
                    tmpUnitViewModel.Actions = tmpList.Where(p => p.UnitName.Equals(tmpUnit)).Select(p => new AuthorizeViewModel()
                    {
                        ActionName = p.ActionName,
                        ActionValue = p.ActionValue,
                        Name = p.Name
                    }).ToList();
                    units.Add(tmpUnitViewModel);
                }
                tmpModuleviewModel.Units = units;


                List<AuthorizeGroup> tmpAuthorizeGroup = db.AuthorizeGroups.ToList();
                Dictionary<string, string> authMap = new Dictionary<string, string>();
                foreach (AuthorizeGroup tmp in tmpAuthorizeGroup)
                {
                    if (!authMap.ContainsKey(tmp.GroupName))
                        authMap.Add(tmp.GroupName, tmp.ActionName);
                    else
                        authMap[tmp.GroupName] += "," + tmp.ActionName;
                }


                ViewBag.authGroup = authMap;
                result.Add(tmpModuleviewModel);
            }
            ViewBag.Roles = result;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomAuthGroup(FormCollection collection)
        {
            string groupId = collection["GroupId"];
            string groupName = collection["GroupName"];
            //遍历用户选择的所有角色，将用于添加到每一个角色当中
            string[] FormKeys = collection.AllKeys;
            List<AuthorizeGroup> groups = new List<AuthorizeGroup>();
            foreach (string tmp in FormKeys)
                if (tmp.Contains("Auth_"))
                    groups.Add(new AuthorizeGroup()
                    {
                        GroupId = groupId,
                        GroupName = groupName,
                        ActionName = tmp
                    });

            using (SystemDbContext db = new SystemDbContext())
            {
                db.AuthorizeGroups.AddRange(groups);
                db.SaveChanges();
            }
            return View();
        }





    }
}
