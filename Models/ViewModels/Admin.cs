using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebSanCauLong.Models.ViewModels
{
    public class Admin
    {
        public int AdminID { get; set; }

        [Required(ErrorMessage = "Họ và tên không được để trống")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string MatKhau { get; set; }
    }
}