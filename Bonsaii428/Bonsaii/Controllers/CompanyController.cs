using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using PagedList;

namespace Bonsaii.Controllers
{
    public class CompanyController : BaseController
    {

        private SystemDbContext db = new SystemDbContext();

       [Authorize(Roles = "Admin,Company_Index")]
        // GET: Company/Details/5
        public ActionResult Index(string id,bool? alertMessage)
        {

            id = this.CompanyId; //获取当前企业的企业ID
            Company company = db.Companies.Find(id);//在系统数据库的Companies表中找到该企业对应的行。
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            ViewBag.alertMessage = alertMessage;
            return View(company);
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public FileContentResult GetImage(string CompanyId)
        {
            Company com = db.Companies.FirstOrDefault(p => p.CompanyId == CompanyId);
            if (com != null)
            {
                return File(com.Logo, com.LogoType);//File方法直接将二进制转化为指定类型了。
            }
            else
            {
                return null;
            }
        }
        public FileContentResult GetImage1(string CompanyId)
        {
            Company com = db.Companies.FirstOrDefault(p => p.CompanyId == CompanyId);
            if (com != null)
            {
                return File(com.BusinessLicense, com.BusinessLicenseType);//File方法直接将二进制转化为指定类型了。
            }
            else
            {
                return null;
            }
        }
                  [Authorize(Roles = "Admin,Company_Edit")]
        // GET: Company/Edit/5
        public ActionResult Edit(string id)
        {
            id = this.CompanyId;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Company/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Company_Edit")]
        public ActionResult Edit(Company company1, HttpPostedFileBase image, HttpPostedFileBase image1)
        {
           
            if (ModelState.IsValid)
            {
                
                    Company company =db.Companies.Find(company1.CompanyId);
                    company.ChangeTime = DateTime.Now;
                    company.ChangePerson = this.Name;
                    //company.FullName = company1.FullName;
                    company.TelNumber = company1.TelNumber;
                    company.UserName = company1.UserName;
                    company.ParentCompany = company1.ParentCompany;
                    company.ParentCompanyId = company1.ParentCompanyId;
                    company.ShortName = company1.ShortName;
                    company.EnglishName = company1.EnglishName;
                    company.LegalRepresentative = company1.LegalRepresentative;
                    company.EstablishDate = company1.EstablishDate;
                    company.Email = company1.Email;
                    company.Address = company1.Address;
                    company.Url = company1.Url;
                    company.Remark = company1.Remark;
                    company.IsGroupCompany = company1.IsGroupCompany;
                   

                    if (image != null)
                    {
                        company.LogoType = image.ContentType;//获取图片类型
                        company.Logo = new byte[image.ContentLength];//新建一个长度等于图片大小的二进制地址
                        image.InputStream.Read(company.Logo, 0, image.ContentLength);//将image读取到Logo中

                    }
                    if (image1 != null)
                    {
                        company.BusinessLicenseType = image1.ContentType;//获取图片类型
                        company.BusinessLicense = new byte[image1.ContentLength];//新建一个长度等于图片大小的二进制地址
                        image1.InputStream.Read(company.BusinessLicense, 0, image1.ContentLength);//将image读取到Logo中

                    }
                    if (company.IsGroupCompany == false)
                    {
                        company.GroupCompanyNumber = null;
                    }
                    else
                    {
                        company.GroupCompanyNumber = company1.GroupCompanyNumber; 
                    }

                   //如果用户修改了公司名字 则执行如下代码
                    if (company.FullName != company1.FullName)
                    {
                        company.FullName = company1.FullName;
                        SystemDbContext systemdb = new SystemDbContext();
                        var thisCompany = (from p in systemdb.Users where this.CompanyId == p.CompanyId select p).ToList();
                        foreach (var item in thisCompany)
                        {
                            item.CompanyFullName = company1.FullName;
                        }
                        systemdb.SaveChanges();
                        db.SaveChanges();
                        return RedirectToAction("Index", "Company", new { alertMessage = true });
                    }
                    else
                    {
                        db.SaveChanges();
                        return RedirectToAction("Index", "Company", new { alertMessage = false });
                    }

                
            }
            return View(company1);
        }

        // GET: Company/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Company company = db.Companies.Find(id);
        //    if (company == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(company);
        //}
         [Authorize(Roles = "Admin,Company_Delete")]
        // POST: Company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
