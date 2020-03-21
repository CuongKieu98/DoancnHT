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
        string url = Constants.url;
        HttpClient client;
        public static List<Khuyenmai> listkm = new List<Khuyenmai>();
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
            HttpResponseMessage responseMessage = await client.GetAsync(url + @"Khuyenmais/");
            List<Khuyenmai> km = getAllKhuyenmai(responseMessage);
            if (km != null)
            {
                ViewBag.accept = false;
                var list = km.ToList();
                return View(list);
            }
            return View("Error");
        }
        public static List<Khuyenmai> getAllKhuyenmai(HttpResponseMessage responseMessage)
        {
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                List<Khuyenmai> khuyenmais = JsonConvert.DeserializeObject<List<Khuyenmai>>(responseData, settings);
                var listkm = khuyenmais.ToList();
                return listkm;
            }
            return null;
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
            HttpResponseMessage response = client.PostAsJsonAsync(url + @"Khuyenmais/", khuyenmais).Result;
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                SetAlert("Thêm thành công!!!", "success");
                return RedirectToAction("Index");
            }
            else
            {
                SetAlert("Lỗi!!!", "error");

            }
            return RedirectToAction("Index");
        }
        // GET: Khuyenmais/Details/5
        public async Task<ActionResult> Details(int? id)
        {

            HttpResponseMessage response = await client.GetAsync(url + @"ChiTietKhuyenMais/" + id);
            if (response.IsSuccessStatusCode)
            {
                var responseData = response.Content.ReadAsStringAsync().Result;
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                var ChiTietKhuyenMais = JsonConvert.DeserializeObject<List<ChiTietKhuyenMai>>(responseData, settings);
                List<ChiTietKhuyenMai> dskm = ChiTietKhuyenMais.ToList();
                ViewBag.MaKM = id;


                // Get km
                HttpResponseMessage responseMessage = await client.GetAsync(url + @"KhuyenMais/");
                List<Khuyenmai> khuyenmais = getAllKhuyenmai(responseMessage);
                Khuyenmai km = khuyenmais.SingleOrDefault(n => n.MaKM == id);


                //getch
                responseMessage = await client.GetAsync(url + @"Cuahangs/");
                List<Cuahang> listch = CuahangsController.getAllCuaHang(responseMessage);
                List<string> dscuahang = new List<string>();
                ViewBag.tench = dscuahang;
                //get sp
                responseMessage = await client.GetAsync(url + @"Doans/");
                List<Doan> listda = DoansController.getAllDoan(responseMessage);
                List<string> dsTen = new List<string>();
                foreach (ChiTietKhuyenMai ctkm in dskm)
                {
                    string name = listda.Where(n => n.MaDA == ctkm.MaDA).SingleOrDefault().TenDA;
                    dsTen.Add(name);
                }
                ViewBag.nameDa = dsTen;
                return View(ChiTietKhuyenMais.ToList());
            }
            return View();
        }

        // GET: Khuyenmais/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            Khuyenmai khuyenmais = null;
            HttpResponseMessage response = await client.GetAsync(url + @"Khuyenmais/" + id);
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
            HttpResponseMessage response = client.PutAsJsonAsync(url + @"Khuyenmais/" + khuyenmai.MaCH, khuyenmai).Result;
            response.EnsureSuccessStatusCode();
            SetAlert("Đã lưu chỉnh sửa!!!", "success");
            return RedirectToAction("Index");

        }
        // GET: Khuyenmais/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            HttpResponseMessage response = await client.DeleteAsync(url + @"Khuyenmais/" + id);
            if (response.IsSuccessStatusCode)
            {
                SetAlert("Xóa thành công!!!", "success");
                return RedirectToAction("Index");
            }
            else
            {
                SetAlert("Lỗi!!!", "error");

            }
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
