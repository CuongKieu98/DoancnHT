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
    public class CuahangsController : Controller
    {

        string url = "http://localhost/svdoancn/api/Cuahangs";
        HttpClient client;
        public CuahangsController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/jason"));
        }
        private dbHutechfoodContext db = new dbHutechfoodContext();

        // GET: Cuahangs
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
                List<Cuahang> cuahangs = JsonConvert.DeserializeObject<List<Cuahang>>(responseData, settings);

                return View(cuahangs);
            }
            return View("Error");
        }

        // GET: Cuahangs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            Cuahang cuahangs = null;
            HttpResponseMessage response = await client.GetAsync(url + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                cuahangs = await response.Content.ReadAsAsync<Cuahang>();
            }
            return View(cuahangs);
        }

        // GET: Cuahangs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cuahangs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Create(Cuahang cuahangs)
        {
            HttpResponseMessage response = client.PostAsJsonAsync(url + "/", cuahangs).Result;
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                SetAlert("Thêm cửa hàng thành công!!!", "success");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: Cuahangs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            Cuahang cuahangs = null;
            HttpResponseMessage response = await client.GetAsync(url + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                cuahangs = await response.Content.ReadAsAsync<Cuahang>();
            }
            return View(cuahangs);
        }

        // POST: Cuahangs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaCH,TenCH,DiachiCH,DienthoaiCH,MotaCH,DanhgiaCH,MaDonHang")] Cuahang cuahangs)
        {
            HttpResponseMessage response = client.PutAsJsonAsync(url + "/" + cuahangs.MaCH, cuahangs).Result;
            response.EnsureSuccessStatusCode();
            SetAlert("Đã lưu chỉnh sửa!!!", "success");
            return RedirectToAction("Index");
           
        }

        // GET: Cuahangs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            HttpResponseMessage response = await client.DeleteAsync(url + "/" + id);
            SetAlert("Xóa thành công!!!", "success");
            return RedirectToAction("Index", "Cuahangs");
        }

        protected void SetAlert (string message, string type)
        {
            TempData["AlertMessage"] = message;
            if(type =="success")
            {
                TempData["AlertType"] = "alert bg-green";
            }
            else if (type=="warning")
            {
                TempData["AlertType"] = "alert-warning";

            }
            else if(type=="error")
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
