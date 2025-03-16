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
    public class AccountUserController : Controller
    {
        private readonly DataModel _dataModel = new DataModel();
        // GET: AccountUser
        public ActionResult Index()
        {
            return View();
        }
        // Đăng ký (GET)
        public ActionResult DangKy()
        {
            return View();
        }

        // Đăng ký (POST)
        [HttpPost]
        public ActionResult DangKy(KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra Email đã tồn tại chưa
                string sqlCheckEmail = $"SELECT COUNT(*) FROM KhachHang WHERE Email = '{khachHang.Email}'";
                ArrayList result = _dataModel.get(sqlCheckEmail);

                int count = 0; // Mặc định chưa có email trùng
                if (result.Count > 0)
                {
                    ArrayList row = (ArrayList)result[0];
                    count = Convert.ToInt32(row[0]);
                }

                if (count > 0)
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại.");
                    return View(khachHang);
                }

                // Gọi phương thức Đăng ký từ DataModel
                bool isRegistered = _dataModel.DangKy(khachHang);
                if (isRegistered)
                {
                    TempData["Success"] = "Đăng ký thành công! Hãy đăng nhập.";
                    return RedirectToAction("DangNhap");
                }
                ModelState.AddModelError("", "Đăng ký thất bại, vui lòng thử lại.");
            }
            return View(khachHang);
        }

        // Đăng nhập (GET)
        public ActionResult DangNhap()
        {
            return View();
        }

        // Đăng nhập (POST)
        [HttpPost]
        public ActionResult DangNhap(string email, string matKhau)
        {
            var user = _dataModel.DangNhap(email, matKhau);
            if (user != null)
            {
                Session["User"] = user;
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu.");
            return View();
        }

        // Đăng xuất
        public ActionResult DangXuat()
        {
            Session["User"] = null;
            return RedirectToAction("DangNhap");
        }
    }
}