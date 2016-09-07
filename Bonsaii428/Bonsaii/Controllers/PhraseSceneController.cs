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
    public class PhraseSceneController : BaseController
    {
        // GET: PhraseScene
        public ActionResult Index()
        {
            return View(db.PhraseScenes.ToList());
        }

        // GET: PhraseScene/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhraseScene phraseScene = db.PhraseScenes.Find(id);
            if (phraseScene == null)
            {
                return HttpNotFound();
            }
            return View(phraseScene);
        }

        // GET: PhraseScene/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PhraseScene/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SnS,SceneName")] PhraseScene phraseScene)
        {
            if (ModelState.IsValid)
            {
                db.PhraseScenes.Add(phraseScene);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(phraseScene);
        }

        // GET: PhraseScene/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhraseScene phraseScene = db.PhraseScenes.Find(id);
            if (phraseScene == null)
            {
                return HttpNotFound();
            }
            return View(phraseScene);
        }

        // POST: PhraseScene/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SnS,SceneName")] PhraseScene phraseScene)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phraseScene).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(phraseScene);
        }

        // GET: PhraseScene/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhraseScene phraseScene = db.PhraseScenes.Find(id);
            if (phraseScene == null)
            {
                return HttpNotFound();
            }
            return View(phraseScene);
        }

        // POST: PhraseScene/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhraseScene phraseScene = db.PhraseScenes.Find(id);
            db.PhraseScenes.Remove(phraseScene);
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
