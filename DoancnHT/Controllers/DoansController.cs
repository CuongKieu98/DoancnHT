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
    public class DoansController : Controller
    {
        string url = Constants.url;
        HttpClient client;
        public static List<Doan> listda = new List<Doan>();
        public DoansController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/jason"));
        }
        private dbHutechfoodContext db = new dbHutechfoodContext();


        // GET: Doans
        public async Task<ActionResult> Index()
        {
            HttpResponseMessage responseMessage = await client.GetAsync(url + @"Doans/");
            List<Doan> da = getAllDoan(responseMessage);
            if (da != null)
            {
                ViewBag.accept = false;
                var list = da.ToList();
                return View(list);
            }
            return View("Error");
        }
        public static List<Doan> getAllDoan(HttpResponseMessage responseMessage)
        {
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                List<Doan> doans = JsonConvert.DeserializeObject<List<Doan>>(responseData, settings);
                var listda = doans.ToList();
                return listda;
            }
            return null;
        }

        // GET: Doans/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            Doan doans = null;
            HttpResponseMessage response = await client.GetAsync(url + @"Doans/" + id);
            if (response.IsSuccessStatusCode)
            {
                doans = await response.Content.ReadAsAsync<Doan>();
            }
            return View(doans);
        }

        // GET: Doans/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Doans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Doan doans)
        {
            HttpResponseMessage response = client.PostAsJsonAsync(url + @"Doans/", doans).Result;
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                SetAlert("Thêm thành công!!!", "success");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: Doans/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            Doan doans = null;
            HttpResponseMessage response = await client.GetAsync(url + @"Doans/" + id);
            if (response.IsSuccessStatusCode)
            {
                doans = await response.Content.ReadAsAsync<Doan>();
            }
            return View(doans);
        }

        // POST: Doans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDA,TenDA,Dongia,AnhDA,MoTa,NgayCapNhat,SoLuongTon,TrangThaiDA,DanhGiaDoAn,MaDM")] Doan doan)
        {
            HttpResponseMessage response = client.PutAsJsonAsync(url + @"Doans/" + doan.MaDA, doan).Result;
            response.EnsureSuccessStatusCode();
            SetAlert("Đã lưu chỉnh sửa!!!", "success");
            return RedirectToAction("Index");
            ;
        }

        // GET: Doans/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            HttpResponseMessage response = await client.DeleteAsync(url + @"Doans/" + id);
            SetAlert("Xóa thành công!!!", "success");
            return RedirectToAction("Index", "Doans");
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
