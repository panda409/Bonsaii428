using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;

namespace Bonsaii.Controllers
{
    public class PhraseController : BaseController
    {
        // GET: Phrase
        public ActionResult Index()
        {
            var item1 = from p in db.Phrases 
                        //where p.PhraseScene == "聊天" 
                        orderby p.PhraseScene,p.Sn
                        select p;
            return View(item1);
            //return View(db.Phrases.ToList());
        }

        // GET: Phrase/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phrase phrase = db.Phrases.Find(id);
            if (phrase == null)
            {
                return HttpNotFound();
            }
            return View(phrase);
        }

        // GET: Phrase/Create
        public ActionResult Create()
        {
            //BonsaiiDbContext db2 = new BonsaiiDbContext(base.ConnectionString);
            List<SelectListItem> item = db.PhraseScenes.ToList().Select(c => new SelectListItem
            {
                Value = c.SceneName,
                Text = c.SceneName
            }).ToList();
                
            ViewBag.List = item;

            return View();
        }

        // POST: Phrase/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Sn,PhraseScene,PhraseContent")] Phrase phrase)
        {
            ///2015/10/13 注释
            //数据库中的表末尾带s，表中的行末尾不带s，把表转换成列表用ToList方法，List<尖括号中代表泛类型>，类（class）带的参数是类型，
            // db.PhraseScenes是一个<DbSet> 把表转换成列表用ToList方法之后选择需要的字段，如果没有映射到强类型就是一个匿名类型。
            //var x = db.PhraseScenes.Select(y => new { CZ = y.SnS.ToString(), XXX = y.SceneName }).ToList();
            //var a= x[0];
            ///作者：panda
          
            if (ModelState.IsValid)
            {
                List<SelectListItem> item = db.PhraseScenes.ToList().Select(y => new SelectListItem 
                { 
                Value = y.SceneName, 
                Text = y.SceneName 
                 }).ToList();
                 ViewBag.List = item;
                //表中行数
                 int count = db.Phrases.Count();
            
                 //foreach (var itemSn in db.Phrases)
                 //{
                 //   if (phrase.PhraseScene == itemSn.PhraseScene)
                 //   {
                 //      while(phrase.Sn == itemSn.Sn)
                 //            {
                 //               // phrase.Sn = itemSn.Sn;
                 //                 itemSn.Sn++;
                             
                 //               }
                 //   }
                 //}  
                 phrase.RecordPerson = this.Name;
                 phrase.RecordTime = DateTime.Now;
                db.Phrases.Add(phrase);
                db.SaveChanges();//模型映射错误
             
                return RedirectToAction("Index");
            }

            return View(phrase);
        }

        // GET: Phrase/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phrase phrase = db.Phrases.Find(id);
            if (phrase == null)
            {
                return HttpNotFound();
            }
     
            //Phrase p = db.Phrases.Find(phrase.Id);
            //int tmp = p.Sn;

            return View(phrase);
        }

        // POST: Phrase/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Sn,PhraseScene,PhraseContent")] Phrase phrase)
        {
       
            if (ModelState.IsValid)
            {
               // int tmp = phrase.Sn;
               // if
                
                //查找Phrases表中的行，序号相同则输出错误信息
                foreach (var itemSn in db.Phrases)
                {
                    if (phrase.PhraseScene == itemSn.PhraseScene)
                    {
                        while (phrase.Sn == itemSn.Sn)
                        {
                            itemSn.Sn = itemSn.Sn + 1;
                            //ModelState.AddModelError("", "序号不能重复");
                            //return View(phrase);
                        }
                    }
                }

                Phrase p = db.Phrases.Find(phrase.Id);
                if (p != null)
                {
                    p.Sn = phrase.Sn;
                    p.PhraseScene = phrase.PhraseScene;
                    p.PhraseContent = phrase.PhraseContent;
                    p.ChangePerson = this.Name;
                    p.ChangeTime = DateTime.Now;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("","修改失败");
                }
                //db.Entry(phrase).State = EntityState.Modified;
                //db.SaveChanges();
               // return RedirectToAction("Index");
            }
            return View(phrase);
        }

        // GET: Phrase/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phrase phrase = db.Phrases.Find(id);
            if (phrase == null)
            {
                return HttpNotFound();
            }
            return View(phrase);
        }

        // POST: Phrase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Phrase phrase = db.Phrases.Find(id);
            db.Phrases.Remove(phrase);
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
