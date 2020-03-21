using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DoancnHT.Models;
using Newtonsoft.Json;

namespace DoancnHT.Controllers
{
    public class ChiTietKhuyenMaisController : Controller
    {
        string url = Constants.url;
        HttpClient client;
        public static List<ChiTietKhuyenMai> listkm = new List<ChiTietKhuyenMai>();
        public ChiTietKhuyenMaisController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/jason"));
        }
        private dbHutechfoodContext db = new dbHutechfoodContext();

        // GET: ChiTietKhuyenMais
        public ActionResult Index()
        {
            return View(db.ChiTietKhuyenMais.ToList());
        }

        // GET: ChiTietKhuyenMais/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietKhuyenMai chiTietKhuyenMai = db.ChiTietKhuyenMais.Find(id);
            if (chiTietKhuyenMai == null)
            {
                return HttpNotFound();
            }
            return View(chiTietKhuyenMai);
        }

        // GET: ChiTietKhuyenMais/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChiTietKhuyenMais/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKM,MaCH,GiaKM,SoLuongKM,MaDA")] ChiTietKhuyenMai chiTietKhuyenMai)
        {
            if (ModelState.IsValid)
            {
                db.ChiTietKhuyenMais.Add(chiTietKhuyenMai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chiTietKhuyenMai);
        }

        // GET: ChiTietKhuyenMais/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietKhuyenMai chiTietKhuyenMai = db.ChiTietKhuyenMais.Find(id);
            if (chiTietKhuyenMai == null)
            {
                return HttpNotFound();
            }
            return View(chiTietKhuyenMai);
        }

        // POST: ChiTietKhuyenMais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKM,MaCH,GiaKM,SoLuongKM,MaDA")] ChiTietKhuyenMai chiTietKhuyenMai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chiTietKhuyenMai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chiTietKhuyenMai);
        }

        // GET: ChiTietKhuyenMais/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietKhuyenMai chiTietKhuyenMai = db.ChiTietKhuyenMais.Find(id);
            if (chiTietKhuyenMai == null)
            {
                return HttpNotFound();
            }
            return View(chiTietKhuyenMai);
        }

        // POST: ChiTietKhuyenMais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChiTietKhuyenMai chiTietKhuyenMai = db.ChiTietKhuyenMais.Find(id);
            db.ChiTietKhuyenMais.Remove(chiTietKhuyenMai);
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
