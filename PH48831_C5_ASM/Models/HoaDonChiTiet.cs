using System.ComponentModel.DataAnnotations;

namespace PH48831_C5_ASM.Models
{
    public class HoaDonChiTiet
    {
        [Key]
        public int HoaDonChiTietId { get; set; }

        [Required(ErrorMessage = "Hóa đơn là bắt buộc")]
        public int HoaDonId { get; set; }
        public HoaDon? HoaDon { get; set; }

        public int? MonAnId { get; set; } // Nullable, vì có thể không phải món ăn
        public MonAn? MonAn { get; set; }

        public int? ComboId { get; set; } // Nullable, vì có thể không phải combo
        public Combo? Combo { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, 100, ErrorMessage = "Số lượng phải nằm trong khoảng từ 1 đến 100")]
        public int SoLuong { get; set; }

        [Required(ErrorMessage = "Đơn giá là bắt buộc")]
        public decimal DonGia { get; set; }
    }
}
