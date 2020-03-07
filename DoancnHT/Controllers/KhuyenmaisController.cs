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
    public class KhuyenmaisController : Controller
    {
        string url = "http://localhost/svdoancn/api/Khuyenmais";
        HttpClient client;
        public KhuyenmaisController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/jason"));
        }
        private dbHutechfoodContext db = new dbHutechfoodContext();

        // GET: Khuyenmais
        public async Task<ActionResult> Index()
        {
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                List<Khuyenmai> khuyenmais = JsonConvert.DeserializeObject<List<Khuyenmai>>(responseData, settings);

                return View(khuyenmais);
            }
            return View("Error");
        }

        // GET: Khuyenmais/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            Khuyenmai khuyenmais = null;
            HttpResponseMessage response = await client.GetAsync(url + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                khuyenmais = await response.Content.ReadAsAsync<Khuyenmai>();
            }
            return View(khuyenmais);
        }


        // GET: Khuyenmais/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Khuyenmais/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Khuyenmai khuyenmais)
        {
            HttpResponseMessage response = client.PostAsJsonAsync(url + "/", khuyenmais).Result;
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                SetAlert("Thêm thành công!!!", "success");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: Khuyenmais/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            Khuyenmai khuyenmais = null;
            HttpResponseMessage response = await client.GetAsync(url + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                khuyenmais = await response.Content.ReadAsAsync<Khuyenmai>();
            }
            return View(khuyenmais);
        }

        // POST: Khuyenmais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKM,TenKM,MotaKM,TgBatDau,TgKetThuc,MaCH")] Khuyenmai khuyenmai)
        {
            HttpResponseMessage response = client.PutAsJsonAsync(url + "/" + khuyenmai.MaCH, khuyenmai).Result;
            response.EnsureSuccessStatusCode();
            SetAlert("Đã lưu chỉnh sửa!!!", "success");
            return RedirectToAction("Index");

        }

        // GET: Khuyenmais/Delete/5
        
        public async Task<ActionResult> Delete(int? id)
        {
            HttpResponseMessage response = await client.DeleteAsync(url + "/" + id);
            SetAlert("Xóa thành công!!!", "success");
            return RedirectToAction("Index", "Khuyenmais");
        }

        // POST: Khuyenmais/Delete/5
        
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
