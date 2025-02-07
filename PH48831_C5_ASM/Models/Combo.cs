using System.ComponentModel.DataAnnotations;

namespace PH48831_C5_ASM.Models
{
    public class Combo
    {
        [Key]
        public int ComboId { get; set; }

        [Required(ErrorMessage = "Tên combo là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên combo không được vượt quá 100 ký tự")]
        public string TenCombo { get; set; }

        [Required(ErrorMessage = "Giá combo là bắt buộc")]
        [Range(10000, 10000000, ErrorMessage = "Giá combo phải nằm trong khoảng từ 10.000 đến 10.000.000")]
        public decimal GiaCombo { get; set; }

        // Danh sách chi tiết combo
        public ICollection<ComboChiTiet> ComboChiTiets { get; set; }

        // Giỏ hàng chi tiết tham chiếu đến combo này
        public ICollection<GioHangChiTiet> GioHangChiTiets { get; set; }

        // Danh sách hóa đơn chi tiết tham chiếu đến combo này
        public ICollection<HoaDonChiTiet> HoaDonChiTiets { get; set; }
    }
}
