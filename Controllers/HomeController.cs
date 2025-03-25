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
            string query = "SELECT TOP 4 * FROM San WHERE TrangThai = N'Còn trống' ORDER BY NEWID()";
            ArrayList danhSachSan = _dataModel.get(query);
            return View(danhSachSan);
        }
        public ActionResult ChiTietSan(int id)
        {
            string query = $"SELECT SanID, TenSan, DiaChi, GiaSan, LoaiSan, TrangThai, MoTa, HinhAnh FROM San WHERE SanID = {id}";
            ArrayList san = _dataModel.get(query);

            if (san.Count > 0)
            {
                ArrayList row = (ArrayList)san[0];
                ViewBag.SanID = row[0];
                ViewBag.TenSan = row[1];
                ViewBag.DiaChi = row[2];
                ViewBag.GiaSan = row[3];
                ViewBag.LoaiSan = row[4];
                ViewBag.TrangThai = row[5];
                ViewBag.MoTa = row[6];
                ViewBag.HinhAnh = row[7];
            }
            else
            {
                return HttpNotFound();
            }

            return View();
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