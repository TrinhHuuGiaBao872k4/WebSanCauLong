using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSanCauLong.Models;
using WebSanCauLong.Models.ViewModels;

namespace WebSanCauLong.Controllers
{
    public class AdminController : Controller
    {
        private readonly DataModel _dataModel = new DataModel();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap");
            }
            return View();
        }
        // GET: Đăng nhập Admin
        public ActionResult DangNhap()
        {
            return View();
        }

        // POST: Đăng nhập Admin
        [HttpPost]
        public ActionResult DangNhap(string email, string matKhau)
        {
            string sql = $"SELECT * FROM Admin WHERE Email = '{email}' AND MatKhau = '{matKhau}'";
            ArrayList result = _dataModel.get(sql);

            if (result.Count > 0)
            {
                Session["Admin"] = email; // Lưu thông tin admin vào session
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu.");
            return View();
        }

        // Đăng xuất
        public ActionResult DangXuat()
        {
            Session["Admin"] = null;
            return RedirectToAction("DangNhap");
        }

        // 1. Hiển thị danh sách sân
        public ActionResult DanhSachSan()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap");
            }

            List<San> danhSachSan = _dataModel.GetDanhSachSan();
            return View(danhSachSan);
        }

        // 2. Thêm sân - Hiển thị form
        public ActionResult ThemSan()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap");
            }

            return View();
        }

        // 2.1 Xử lý thêm sân
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemSan(San san)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap");
            }

            if (ModelState.IsValid)
            {
                _dataModel.ThemSan(san);
                return RedirectToAction("DanhSachSan");
            }
            return View(san);
        }

        // 3. Sửa sân - Hiển thị form
        public ActionResult SuaSan(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap");
            }

            San san = _dataModel.GetSanById(id);
            if (san == null)
            {
                return HttpNotFound();
            }
            return View(san);
        }

        // 3.1 Xử lý cập nhật sân
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaSan(San san)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap");
            }

            if (ModelState.IsValid)
            {
                _dataModel.CapNhatSan(san);
                return RedirectToAction("DanhSachSan");
            }
            return View(san);
        }

        // 4. Xóa sân - Hiển thị xác nhận xóa
        public ActionResult XoaSan(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap");
            }

            San san = _dataModel.GetSanById(id);
            if (san == null)
            {
                return HttpNotFound();
            }
            return View(san);
        }

        // 4.1 Xử lý xóa sân
        [HttpPost, ActionName("XoaSan")]
        [ValidateAntiForgeryToken]
        public ActionResult XoaSanConfirmed(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap");
            }

            _dataModel.XoaSan(id);
            return RedirectToAction("DanhSachSan");
        }

        public ActionResult DuyetDatSan()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap");
            }

            var danhSachDatSan = _dataModel.GetAllDatSan();
            return View(danhSachDatSan);
        }
        
        public ActionResult XacNhanDatSanCuaKhach(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap");
            }

            bool thanhCong = _dataModel.XacNhanDatSan(id);

            if (thanhCong)
            {
                TempData["SuccessMessage"] = "Xác nhận đặt sân thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Xác nhận thất bại! Sân có thể đã bị đặt hoặc lỗi hệ thống.";
            }

            return RedirectToAction("DuyetDatSan");
        }
    }
}