using BonsaiiModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bonsaii.Controllers
{
    public class MenuController : BaseController
    {
        // GET: Menu
        [Authorize(Roles = "Admin,Menu_Index")]
        public ActionResult Index()
        {
            List<Menu> Menus = db.Menus.OrderBy(p=>p.ParentOrder).ToList();
            List<int> openMenus = Menus.Where(p => p.IsShow == true).Select(p => p.Id).ToList();
            List<MenuViewModel> menuViewModel = new List<MenuViewModel>();
            menuViewModel.Add(new MenuViewModel(Menus[0].ParentName, Menus[0].ParentClass,Menus[0].ParentOrder));
            int j = 0;
            menuViewModel[j].MenuNodes.Add(new MenuNode(Menus[0].Name, Menus[0].Href, Menus[0].IsShow,Menus[0].Id));
            for (int i = 1; i < Menus.Count; i++)
            {
                if (!Menus[i].ParentName.Equals(Menus[i-1].ParentName))
                {
                    menuViewModel.Add(new MenuViewModel(Menus[i].ParentName, Menus[i].ParentClass,Menus[i].ParentOrder));
                    j++;
                }
                menuViewModel[j].MenuNodes.Add(new MenuNode(Menus[i].Name, Menus[i].Href, Menus[i].IsShow,Menus[i].Id));
            }
            string tmp = "";
            foreach (int str in openMenus)
                tmp += str.ToString() + ";";
            ViewBag.openMenus = tmp;
            return View(menuViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Menu_Save")]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection collection)
        {
            string[] FormKeys = collection.AllKeys;
            List<Menu> menus = db.Menus.ToList();
            foreach (Menu tmp in menus)
            {
                tmp.IsShow = false;
                db.Entry(tmp).State = EntityState.Modified;
                db.SaveChanges();
            }
            foreach (string tmp in FormKeys)
            {
                if (tmp.Contains("menu_") == true)
                {
                    Menu tmpMenu = menus.Find(p=>p.Id == int.Parse(tmp.Substring(5)));
                    tmpMenu.IsShow = true;
                    db.Entry(tmpMenu).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}