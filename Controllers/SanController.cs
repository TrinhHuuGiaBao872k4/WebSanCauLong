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
        public ActionResult DanhSachSanTrong(DateTime? ngayDat, TimeSpan? gioBatDau, TimeSpan? gioKetThuc)
        {
            // Kiểm tra nếu thời gian nhập vào không hợp lệ
            if (!ngayDat.HasValue || !gioBatDau.HasValue || !gioKetThuc.HasValue || gioBatDau >= gioKetThuc)
            {
                ViewBag.ErrorMessage = "Thời gian bắt đầu và kết thúc không hợp lệ.";
                return View(new List<San>());
            }

            // Gọi phương thức GetSanTrong để lấy danh sách sân trống dựa trên ngày và giờ đặt sân
            var danhSachSanTrong = dataModel.GetSanTrong(ngayDat.Value, gioBatDau.Value, gioKetThuc.Value);

            return View(danhSachSanTrong);
        }

        [HttpGet]
        public ActionResult DatSan(int sanId, DateTime ngayDat, TimeSpan gioBatDau, TimeSpan gioKetThuc)
        {
            var user = (KhachHang)Session["User"];
            if (user == null)
            {
                return RedirectToAction("DangNhap", "AccountUser");
            }

            var san = dataModel.GetSanById(sanId);
            if (san == null)
            {
                return HttpNotFound();
            }

            var model = new DatSan
            {
                SanID = san.SanID,
                NgayDat = ngayDat, // Chỉ lấy ngày đặt
                GioBatDau = gioBatDau,
                GioKetThuc = gioKetThuc,
                TongTien = (decimal)((gioKetThuc - gioBatDau).TotalHours * (double)san.GiaSan)
            };

            return View(model);
        }

        // Xử lý đặt sân (Kiểm tra đăng nhập thủ công)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DatSan(DatSan model)
        {
            var user = (KhachHang)Session["User"];
            if (user == null)
            {
                return RedirectToAction("DangNhap", "AccountUser");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kiểm tra thời gian hợp lệ
            if (model.GioBatDau >= model.GioKetThuc)
            {
                ModelState.AddModelError("", "Giờ kết thúc phải lớn hơn giờ bắt đầu.");
                return View(model);
            }

            model.KhachHangID = user.KhachHangID;
            model.TrangThai = "Đang chờ";

            bool success = dataModel.DatSan(model);

            if (success && model.DatSanID > 0)
            {
                return RedirectToAction("XacNhanDatSan", new { id = model.DatSanID });
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
            datSan.San = dataModel.GetSanById(datSan.SanID);
            return View(datSan);
        }

        [HttpPost]
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

        public ActionResult LichSuDatSan()
        {
            var user = (KhachHang)Session["User"];
            if (user == null)
            {
                return RedirectToAction("DangNhap", "AccountUser");
            }

            var lichSu = dataModel.GetLichSuDatSan(user.KhachHangID);
            return View(lichSu);
        }
    }
}