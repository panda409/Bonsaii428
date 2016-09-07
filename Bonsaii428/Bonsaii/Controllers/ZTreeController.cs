using Bonsaii.Models;
using Bonsaii.Models.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bonsaii.Controllers
{
    public class ZTreeController : BaseController
    {
        public ActionResult Index() {
            return View();
        }
        public ActionResult Test()
        {
            return View();
        }
        public ActionResult zTree() {
            return View();
        }
        // GET: ZTree；公司、部门树状图
        [HttpPost]
        public JsonResult CDTree()
        {
            var departments = (from p in db.Departments orderby p.DepartmentOrder select p).ToList();
            foreach (var department in departments)
            {
                var staffCounts = (from p in db.Staffs where p.Department == department.DepartmentId select p).ToList();
                department.RealSize = staffCounts.Count();
            }

            //查找到所有的部门。让他们变为zTree格式。
            var rootsall = (from p in departments
                            orderby p.DepartmentOrder
                            select new zTree
                            {
                                id = p.DepartmentId,
                                pid = p.ParentDepartmentId,
                                name = p.Name + "(" + p.RealSize + "/" + p.StaffSize + ")",
                                isParent = false,
                                open = false
                            }).ToList();
            //初始化result。是所有部门结果。
            List<zTree> result = new List<zTree>();

            //把所有的部门节点成为result的孩子
            foreach (var item in rootsall)
            {
                result.Add(item);
            }
            //两层循环。resul孩子只剩下父部门，其他部门都变成了它们的孩子。
            foreach (var item in rootsall)
            {
                foreach (var a in rootsall)
                {
                    if (item.id == a.pid)
                    {
                        item.isParent = true;
                        item.children.Add(a);
                        result.Remove(a);
                    }
                }
            }

            //初始化根节点。没有放孩子节点进去。
            zTree root = new zTree
            {
                name = this.CompanyFullName,
                isParent = false,
                open = true
            };
            //把根节点加入result。
            // result.Add(root);
            // foreach()
            List<zTree> cdresult = new List<zTree>();
            cdresult.Add(root);

            foreach (var item in result)
            {
                root.children.Add(item);
            }
            return Json(cdresult);
        }
      
        // GET: ZTree；公司、部门、员工树状图
        [HttpPost]
        public JsonResult CDSTree()
        {
            var departments  = (from p in db.Departments orderby p.DepartmentOrder select p).ToList();
            foreach (var department in departments) {
                var staffCounts = (from p in db.Staffs where p.Department==department.DepartmentId select p).ToList();
                department.RealSize = staffCounts.Count();
            }

            //查找到所有的部门。让他们变为zTree格式。
            var rootsall = (from p in departments//db.Departments
                            //from q in db.Staffs where q.Department == p.DepartmentId
                            select new zTree
                            {
                                id = p.DepartmentId,
                                pid = p.ParentDepartmentId,
                                name = p.Name + "(" + p.RealSize +"/"+ p.StaffSize + ")",
                                url = "/Staff/List?id="+p.DepartmentId,
                                //url = "/Staff/List?id=" + p.DepartmentId,
                                target = "_self",
                                isParent = false,
                                open=false
                            }).ToList();

            //查找到所有的员工。让他们变为zTree格式。
            var staffs = (from p in db.Staffs
                          select new zTree
                          {
                              id = p.Number.ToString(),
                              pid = p.Department,
                              name = p.Name,
                              url = "/Staff/Details?id="+p.Number,
                              target = "_self",
                              isParent = false,
                              open = false
                          }).ToList();
            //把员工给部门。
            foreach (var itemstaff in staffs)
            {
                foreach (var item in rootsall)
                {
                    var a = 0;
                    if (itemstaff.pid == item.id)
                    {
                        a++;
                        //
                        //item.name = 
                        item.isParent = true;
                        item.children.Add(itemstaff);
                    }
                }

            }
            //初始化result。是所有部门结果。
            List<zTree> result = new List<zTree>();
      
            //所有的部门节点成为result的孩子
        
            foreach (var item in rootsall)
            {
            //要统计每个部门下面有多少人。shit。
            // List<zTree> result = new List<zTree>();
            // foreach (var department in departments) {
            //   foreach(var staffCount in staffCounts)
            //   {
            //       var a = 0;
            //       if (staffCount.Department == department.DepartmentId) {
            //           a++;
            //       }
            //       item.url = "/Staff/List?id=" + a + "/" + department.DepartmentId;
            //   }
            //}
                result.Add(item);
            }
            //两层循环。result孩子只剩下父部门，其他部门都变成了它们的孩子。
            foreach (var item in rootsall)
            {
                foreach (var a in rootsall)
                {
                    if (item.id == a.pid)
                    {
                        item.isParent = true;
                        item.children.Add(a);
                        result.Remove(a);
                    }
                }
            }

            //初始化根节点。没有放孩子节点进去。
            zTree root = new zTree
            {
                name = this.CompanyFullName,
                isParent = false,
                url = "/Staff/Index",
                target = "_self",
                open = true
            };
            //把根节点加入result。
           // result.Add(root);
           // foreach()
            List<zTree> cdresult = new List<zTree>();
            cdresult.Add(root);
           
            foreach (var item in result)
            {
                root.children.Add(item);
            }

            return Json(cdresult);
        }

        // GET: ZTree；公司、部门、员工树状图;用于需要选择员工的情况
        [HttpPost]
        public JsonResult StaffIndexTree()
        {
            //查找到所有的部门。让他们变为zTree格式。
            var rootsall = (from p in db.Departments
                            select new zTree
                            {
                                id = p.DepartmentId,
                                pid = p.ParentDepartmentId,
                                name = p.Name + "(" + p.StaffSize + ")",
                                url = "/StaffForChoose/List?id=" + p.DepartmentId,
                                //url = "/Staff/List?id=" + p.DepartmentId,
                                target = "_self",
                                isParent = false,
                                open = false
                            }).ToList();
            //查找到所有的员工。让他们变为zTree格式。
            var staffs = (from p in db.Staffs
                          select new zTree
                          {
                              id = p.Number.ToString(),
                              pid = p.Department,
                              name = p.Name,
                              url = "/StaffForChoose/Details?id=" + p.Number,
                              target = "_self",
                              isParent = false,
                              open = false
                          }).ToList();
            //把员工给部门。
            foreach (var itemstaff in staffs)
            {
                foreach (var item in rootsall)
                {
                    if (itemstaff.pid == item.id)
                    {
                        item.isParent = true;
                        item.children.Add(itemstaff);
                    }
                }
            }
            //初始化result。是所有部门结果。
            List<zTree> result = new List<zTree>();

            //所有的部门节点成为result的孩子
            foreach (var item in rootsall)
            {
                result.Add(item);
            }
            //两层循环。result孩子只剩下父部门，其他部门都变成了它们的孩子。
            foreach (var item in rootsall)
            {
                foreach (var a in rootsall)
                {
                    if (item.id == a.pid)
                    {
                        item.isParent = true;
                        item.children.Add(a);
                        result.Remove(a);
                    }
                }
            }

            //初始化根节点。没有放孩子节点进去。
            zTree root = new zTree
            {
                name = this.CompanyFullName,
                isParent = false,
                url = "/StaffForChoose/Index",
                target = "_self",
                open = false
            };
            //把根节点加入result。
            // result.Add(root);
            // foreach()
            List<zTree> cdresult = new List<zTree>();
            cdresult.Add(root);

            foreach (var item in result)
            {
                root.children.Add(item);
            }

            return Json(cdresult);
        }
       
        /// <summary>
        /// 给审批流程中的各个步骤排序
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //public JsonResult AuditStepSort(int TId) {

        //    List<AuditStep> steps = (from p in db.AuditSteps where p.TId == TId select p).ToList();
        //    var rootsall = (from p in steps
        //                    select new zTree
        //                        {
        //                            id = p.SId.ToString(),
        //                            pid = null,
        //                            name = p.Name,
        //                            isParent = false
        //                        }).ToList();
        //    List<zTree> result = new List<zTree>();
        //   foreach(zTree item in rootsall){
        //        result.Add(item);
        //   }
        //    foreach(var item in rootsall){
        //        foreach (var a in rootsall)
        //        {
        //            if (item.id == a.children)
        //            {
        //                item.isParent = true;
        //                item.children.Add(a);
        //                result.Remove(a);
        //            }
        //        }
        //    }
        //}
        // GET: ZTree；公司、部门树状图
        [HttpPost]
        public JsonResult SpaceTree()
        {
            var departments = (from p in db.Departments orderby p.DepartmentOrder select p).ToList();
            foreach (var department in departments)
            {
                var staffCounts = (from p in db.Staffs where p.Department == department.DepartmentId select p).ToList();
                department.RealSize = staffCounts.Count();
            }

            //查找到所有的部门。让他们变为zTree格式。
            var rootsall = (from p in departments
                            orderby p.DepartmentOrder
                            select new zTree
                            {
                                id = p.DepartmentId,
                                pid = p.ParentDepartmentId,
                                name = p.Name + "(" + p.RealSize + "/" + p.StaffSize + ")",
                                isParent = false,

                            }).ToList();
            //初始化result。是所有部门结果。
            List<zTree> result = new List<zTree>();

            //把所有的部门节点成为result的孩子
            foreach (var item in rootsall)
            {
                result.Add(item);
            }
            //两层循环。resul孩子只剩下父部门，其他部门都变成了它们的孩子。
            foreach (var item in rootsall)
            {
                foreach (var a in rootsall)
                {
                    if (item.id == a.pid)
                    {
                        item.isParent = true;
                        item.children.Add(a);
                        result.Remove(a);
                    }
                }
            }

            //初始化根节点。没有放孩子节点进去。
            zTree root = new zTree
            {
                id="0",
                name = this.CompanyFullName,
                isParent = true,
            };
            //把根节点加入result。
            // result.Add(root);
            // foreach()
            List<zTree> cdresult = new List<zTree>();


            foreach (var item in result)
            {
                root.children.Add(item);
            }
      //      cdresult.Add(root);
            //List<STree> stree = (from p in cdresult
            //                     select new STree
            //                     {
            //                         id = p.id,
            //                         name = p.name,
            //                         data = "",
            //                     }).ToList();          
           return Json(root);
        }
      
    }
}