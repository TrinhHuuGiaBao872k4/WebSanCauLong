using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WebSanCauLong.Models.ViewModels
{
    public class KhachHang
    {
        public int KhachHangID { get; set; }

        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^(0[3|5|7|8|9])+([0-9]{8})$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string SoDienThoai { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        [CustomValidation(typeof(KhachHang), "ValidatePassword")]
        public string MatKhau { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu")]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu nhập lại không khớp")]
        public string XacNhanMatKhau { get; set; }

        [StringLength(200, ErrorMessage = "Địa chỉ không được vượt quá 200 ký tự")]
        public string DiaChi { get; set; }

        [DataType(DataType.Date)]
        [CustomValidation(typeof(KhachHang), "ValidateNgaySinh")]
        public DateTime? NgaySinh { get; set; }

        public string GioiTinh { get; set; }

        public DateTime NgayDangKy { get; set; } = DateTime.Now;

        // Kiểm tra ngày sinh không được lớn hơn ngày hiện tại
        public static ValidationResult ValidateNgaySinh(DateTime? ngaySinh, ValidationContext context)
        {
            if (ngaySinh.HasValue && ngaySinh.Value > DateTime.Today)
            {
                return new ValidationResult("Ngày sinh không hợp lệ");
            }
            return ValidationResult.Success;
        }

        // Kiểm tra mật khẩu có chữ in hoa, ký tự đặc biệt, chữ số
        public static ValidationResult ValidatePassword(string password, ValidationContext context)
        {
            if (string.IsNullOrEmpty(password))
            {
                return new ValidationResult("Mật khẩu không được để trống.");
            }

            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                return new ValidationResult("Mật khẩu phải chứa ít nhất 1 chữ in hoa.");
            }

            if (!Regex.IsMatch(password, @"[\W_]"))
            {
                return new ValidationResult("Mật khẩu phải chứa ít nhất 1 ký tự đặc biệt.");
            }

            if (!Regex.IsMatch(password, @"\d"))
            {
                return new ValidationResult("Mật khẩu phải chứa ít nhất 1 chữ số.");
            }

            return ValidationResult.Success;
        }
    }
}