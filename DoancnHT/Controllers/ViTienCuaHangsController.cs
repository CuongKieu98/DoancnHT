using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoancnHT.Models;

namespace DoancnHT.Controllers
{
    public class ViTienCuaHangsController : Controller
    {
        private dbHutechfoodContext db = new dbHutechfoodContext();

        // GET: ViTienCuaHangs
        public ActionResult Index()
        {
            return View(db.ViTienCuaHangs.ToList());
        }

        // GET: ViTienCuaHangs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViTienCuaHang viTienCuaHang = db.ViTienCuaHangs.Find(id);
            if (viTienCuaHang == null)
            {
                return HttpNotFound();
            }
            return View(viTienCuaHang);
        }

        // GET: ViTienCuaHangs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ViTienCuaHangs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaViCH,SoDu,NgayGiaoDich,SoTienGiaoDich,MaCH")] ViTienCuaHang viTienCuaHang)
        {
            if (ModelState.IsValid)
            {
                db.ViTienCuaHangs.Add(viTienCuaHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(viTienCuaHang);
        }

        // GET: ViTienCuaHangs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViTienCuaHang viTienCuaHang = db.ViTienCuaHangs.Find(id);
            if (viTienCuaHang == null)
            {
                return HttpNotFound();
            }
            return View(viTienCuaHang);
        }

        // POST: ViTienCuaHangs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaViCH,SoDu,NgayGiaoDich,SoTienGiaoDich,MaCH")] ViTienCuaHang viTienCuaHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(viTienCuaHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viTienCuaHang);
        }

        // GET: ViTienCuaHangs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViTienCuaHang viTienCuaHang = db.ViTienCuaHangs.Find(id);
            if (viTienCuaHang == null)
            {
                return HttpNotFound();
            }
            return View(viTienCuaHang);
        }

        // POST: ViTienCuaHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViTienCuaHang viTienCuaHang = db.ViTienCuaHangs.Find(id);
            db.ViTienCuaHangs.Remove(viTienCuaHang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert bg-green";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";

            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
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
