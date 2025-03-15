using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebSanCauLong.Models;

namespace WebSanCauLong.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataModel _dataModel = new DataModel();
        public ActionResult Index(int? page)
        {
            string query = "SELECT TOP 4 * FROM San WHERE TrangThai = 'Còn trống' ORDER BY NEWID()";
            ArrayList danhSachSan = _dataModel.get(query);
            return View(danhSachSan);
        }
        // Chi tiết sân
        public ActionResult ChiTietSan(int id)
        {
            string query = $"SELECT * FROM San WHERE SanID = {id}";
            ArrayList san = _dataModel.get(query);
            if (san.Count == 0)
            {
                return HttpNotFound();
            }
            return View(san[0]);
        }

        // Tìm kiếm sân
        public ActionResult TimKiem(string keyword)
        {
            string query = $"SELECT * FROM San WHERE TenSan LIKE '%{keyword}%' OR DiaChi LIKE '%{keyword}%'";
            ArrayList ketQua = _dataModel.get(query);
            return View("Index", ketQua);
        }
    }
}