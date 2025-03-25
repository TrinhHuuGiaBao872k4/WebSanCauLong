using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebSanCauLong.Models.ViewModels
{
    public class San
    {
        public int SanID { get; set; }

        [Required]
        public string TenSan { get; set; }

        [Required]
        public string DiaChi { get; set; }

        [Required]
        public string LoaiSan { get; set; }

        [Required]
        public decimal GiaSan { get; set; }

        public string MoTa { get; set; }

        public string TrangThai { get; set; }

        public string HinhAnh { get; set; }
    }
}