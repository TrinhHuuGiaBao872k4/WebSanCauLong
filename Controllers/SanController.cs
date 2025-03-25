using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using WebSanCauLong.Models;
using WebSanCauLong.Models.ViewModels;

namespace WebSanCauLong.Controllers
{
    public class SanController : Controller
    {
        private DataModel dataModel = new DataModel();
        // Lấy danh sách các sân còn trống
        public ActionResult DanhSachSanTrong(DateTime? thoiGianBatDau, DateTime? thoiGianKetThuc)
        {
            // Kiểm tra nếu thời gian không hợp lệ
            if (!thoiGianBatDau.HasValue || !thoiGianKetThuc.HasValue || thoiGianBatDau >= thoiGianKetThuc)
            {
                ViewBag.ErrorMessage = "Thời gian bắt đầu và kết thúc không hợp lệ.";
                return View(new List<San>());
            }

            // Lấy ngày từ thời gian bắt đầu
            DateTime ngay = thoiGianBatDau.Value.Date;

            // Lấy giờ bắt đầu và giờ kết thúc (chuyển về TimeSpan)
            TimeSpan gioBatDau = thoiGianBatDau.Value.TimeOfDay;
            TimeSpan gioKetThuc = thoiGianKetThuc.Value.TimeOfDay;

            // Gọi phương thức GetSanTrong với đúng kiểu dữ liệu
            var danhSachSanTrong = dataModel.GetSanTrong(ngay, gioBatDau, gioKetThuc);

            return View(danhSachSanTrong);
        }
        // Hiển thị form đặt sân
        [Authorize] // Người dùng phải đăng nhập mới được đặt sân
        public ActionResult DatSan(int sanId, DateTime thoiGianBatDau, DateTime thoiGianKetThuc)
        {
            var san = dataModel.GetSanById(sanId);
            if (san == null)
            {
                return HttpNotFound();
            }

            var model = new DatSan
            {
                SanID = san.SanID,
                ThoiGianBatDau = thoiGianBatDau,
                ThoiGianKetThuc = thoiGianKetThuc,
                TongTien = (decimal)((thoiGianKetThuc - thoiGianBatDau).TotalHours * (double)san.GiaSan)
            };

            return View(model);
        }

        // Xử lý đặt sân
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DatSan(DatSan model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = (KhachHang)Session["User"];
            if (user == null)
            {
                return RedirectToAction("DangNhap", "KhachHang");
            }

            // Kiểm tra thời gian đặt sân hợp lệ
            if (model.ThoiGianBatDau >= model.ThoiGianKetThuc)
            {
                ModelState.AddModelError("", "Thời gian kết thúc phải lớn hơn thời gian bắt đầu.");
                return View(model);
            }

            model.KhachHangID = user.KhachHangID;
            model.TrangThai = "Đang chờ";

            bool success = dataModel.DatSan(model);
            int datSanID = model.DatSanID;
            if (success)
            {
                return RedirectToAction("XacNhanDatSan", new { id = datSanID });
            }
            else
            {
                ModelState.AddModelError("", "Không thể đặt sân. Vui lòng thử lại.");
                return View(model);
            }
        }
        public ActionResult XacNhanDatSan(int id)
        {
            var datSan = dataModel.GetDatSanById(id);
            if (datSan == null)
            {
                return HttpNotFound();
            }
            return View(datSan);
        }

        [HttpPost]
        [Authorize]
        public ActionResult HuyDatSan(int id)
        {
            var datSan = dataModel.GetDatSanById(id);
            if (datSan == null)
            {
                return HttpNotFound();
            }

            // Kiểm tra nếu trạng thái không thể hủy (ví dụ: đã xác nhận)
            if (datSan.TrangThai == "Đã xác nhận")
            {
                TempData["ErrorMessage"] = "Bạn không thể hủy sân đã được xác nhận!";
                return RedirectToAction("LichSuDatSan");
            }

            // Cập nhật trạng thái thành "Đã hủy"
            bool isCancelled = dataModel.HuyDatSan(id);
            if (isCancelled)
            {
                TempData["SuccessMessage"] = "Hủy đặt sân thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Hủy đặt sân thất bại!";
            }

            return RedirectToAction("LichSuDatSan");
        }

        [Authorize]
        public ActionResult LichSuDatSan()
        {
            int khachHangID = Convert.ToInt32(Session["KhachHangID"]); // Lấy ID khách hàng từ session
            if (khachHangID == 0)
            {
                return RedirectToAction("DangNhap", "KhachHang");
            }

            var lichSu = dataModel.GetLichSuDatSan(khachHangID);
            return View(lichSu);
        }
    }
}