using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebSanCauLong.Models.ViewModels
{
    public class DatSan
    {
        [Key]
        public int DatSanID { get; set; }

        [Required(ErrorMessage = "Khách hàng không được để trống.")]
        public int KhachHangID { get; set; } // Người đặt sân

        [Required(ErrorMessage = "Vui lòng chọn sân.")]
        public int SanID { get; set; } // Sân được đặt

        [Required(ErrorMessage = "Vui lòng nhập ngày đặt.")]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "Ngày đặt phải từ ngày hiện tại trở đi.")]
        public DateTime NgayDat { get; set; } // Ngày đặt sân

        [Required(ErrorMessage = "Vui lòng nhập giờ bắt đầu.")]
        [DataType(DataType.Time)]
        public TimeSpan GioBatDau { get; set; } // Giờ bắt đầu thuê sân

        [Required(ErrorMessage = "Vui lòng nhập giờ kết thúc.")]
        [DataType(DataType.Time)]
        [GreaterThan("GioBatDau", ErrorMessage = "Giờ kết thúc phải sau giờ bắt đầu.")]
        public TimeSpan GioKetThuc { get; set; } // Giờ kết thúc thuê sân

        [Required(ErrorMessage = "Tổng tiền không được để trống.")]
        [Range(1000, double.MaxValue, ErrorMessage = "Tổng tiền phải lớn hơn 1000.")]
        public decimal TongTien { get; set; } // Tổng tiền thuê sân

        [Required(ErrorMessage = "Trạng thái không được để trống.")]
        [RegularExpression("^(Đang chờ|Xác nhận|Đã hủy)$", ErrorMessage = "Trạng thái không hợp lệ.")]
        public string TrangThai { get; set; } = "Đang chờ"; // Trạng thái đặt sân: "Đang chờ", "Xác nhận", "Đã hủy"

        [ForeignKey("KhachHangID")]
        public virtual KhachHang KhachHang { get; set; }

        [ForeignKey("SanID")]
        public virtual San San { get; set; }
    }

    // Custom validation: Ngày đặt phải là tương lai hoặc hôm nay
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime < DateTime.Now.Date)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }

    // Custom validation: Giờ kết thúc phải lớn hơn giờ bắt đầu
    public class GreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public GreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
            {
                return new ValidationResult($"Không tìm thấy thuộc tính {_comparisonProperty}");
            }

            var comparisonValue = (TimeSpan)property.GetValue(validationContext.ObjectInstance);
            if (value is TimeSpan currentValue)
            {
                if (currentValue <= comparisonValue)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}