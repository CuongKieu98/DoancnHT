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
    public class DanhmucdoansController : Controller
    {
        string url = Constants.url;
        HttpClient client;
        public static List<Danhmucdoan> listdm = new List<Danhmucdoan>();
        public DanhmucdoansController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/jason"));
        }
        
        private dbHutechfoodContext db = new dbHutechfoodContext();

        // GET: Danhmucdoans
        public async Task<ActionResult> Index()
        {
            HttpResponseMessage responseMessage = await client.GetAsync(url + @"Danhmucdoans/");
            List<Danhmucdoan> dm = getAllDanhmucdoan(responseMessage);
            if (dm != null)
            {
                ViewBag.accept = false;
                var list = dm.ToList();
                return View(list);
            }
            return View("Error");
        }
        public static List<Danhmucdoan> getAllDanhmucdoan(HttpResponseMessage responseMessage)
        {
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                List<Danhmucdoan> danhmucdoans = JsonConvert.DeserializeObject<List<Danhmucdoan>>(responseData, settings);
                var listdm = danhmucdoans.ToList();
                return listdm;
            }
            return null;
        }

        // GET: Danhmucdoans/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            Danhmucdoan danhmucdoans = null;
            HttpResponseMessage response = await client.GetAsync(url + @"Danhmucdoans/" + id);
            if (response.IsSuccessStatusCode)
            {
                //dong hoac mo data table 
                ViewBag.accept = false;
                danhmucdoans = await response.Content.ReadAsAsync<Danhmucdoan>();
                // Call api
                HttpResponseMessage responseMessage = await client.GetAsync(url + @"Doans/");
                List<Doan> doans = DoansController.getAllDoan(responseMessage);
                // Check data with id customer
                doans = doans.Where(n => n.MaDA == id).ToList();
                ViewBag.da = doans;
                
            }
            return View(danhmucdoans);
        }

        // GET: Danhmucdoans/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Danhmucdoans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Danhmucdoan danhmucdoans)
        {
            HttpResponseMessage response = client.PostAsJsonAsync(url + @"Danhmucdoans/", danhmucdoans).Result;
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                SetAlert("Thêm danh mục thành công!!!", "success");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }



        // GET: Danhmucdoans/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            Danhmucdoan danhmucdoans = null;
            HttpResponseMessage response = await client.GetAsync(url + @"Danhmucdoans/" + id);
            if (response.IsSuccessStatusCode)
            {
                danhmucdoans = await response.Content.ReadAsAsync<Danhmucdoan>();
            }
            return View(danhmucdoans);
        }

        // POST: Danhmucdoans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDM,TenDM,Hinhanhdm,MaCH")] Danhmucdoan danhmucdoans)
        {
            HttpResponseMessage response = client.PutAsJsonAsync(url + @"Danhmucdoans/" + danhmucdoans.MaDM, danhmucdoans).Result;
            response.EnsureSuccessStatusCode();
            SetAlert("Đã lưu chỉnh sửa!!!", "success");
            return RedirectToAction("Index");

        }


        // GET: Danhmucdoans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Danhmucdoan danhmucdoan = db.Danhmucdoans.Find(id);
            if (danhmucdoan == null)
            {
                return HttpNotFound();
            }
            return View(danhmucdoan);
        }

        // POST: Danhmucdoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Danhmucdoan danhmucdoan = db.Danhmucdoans.Find(id);
            db.Danhmucdoans.Remove(danhmucdoan);
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
