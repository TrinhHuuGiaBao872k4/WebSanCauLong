using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required(ErrorMessage = "Vui lòng nhập thời gian bắt đầu.")]
        [DataType(DataType.DateTime)]
        [FutureDate(ErrorMessage = "Thời gian bắt đầu phải ở tương lai.")]
        public DateTime ThoiGianBatDau { get; set; } // Thời gian bắt đầu thuê sân

        [Required(ErrorMessage = "Vui lòng nhập thời gian kết thúc.")]
        [DataType(DataType.DateTime)]
        [GreaterThan("ThoiGianBatDau", ErrorMessage = "Thời gian kết thúc phải sau thời gian bắt đầu.")]
        public DateTime ThoiGianKetThuc { get; set; } // Thời gian kết thúc thuê sân

        [Required(ErrorMessage = "Tổng tiền không được để trống.")]
        [Range(1000, double.MaxValue, ErrorMessage = "Tổng tiền phải lớn hơn 1000.")]
        public decimal TongTien { get; set; } // Tổng tiền thuê sân

        [Required(ErrorMessage = "Trạng thái không được để trống.")]
        [RegularExpression("^(Đang chờ|Đã xác nhận|Đã hủy)$", ErrorMessage = "Trạng thái không hợp lệ.")]
        public string TrangThai { get; set; } = "Đang chờ"; // Trạng thái đặt sân: "Đang chờ", "Đã xác nhận", "Đã hủy"

        public virtual KhachHang KhachHang { get; set; }
        public virtual San San { get; set; }
    }
    // Custom validation: Ngày đặt sân phải là tương lai
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime <= DateTime.Now)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }

    // Custom validation: Thời gian kết thúc phải lớn hơn thời gian bắt đầu
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

            var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);
            if (value is DateTime currentValue)
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