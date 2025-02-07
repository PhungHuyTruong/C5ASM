using System.ComponentModel.DataAnnotations;

namespace PH48831_C5_ASM.Models
{
    public class GioHangChiTiet
    {
        [Key]
        public int GioHangChiTietId { get; set; }

        [Required(ErrorMessage = "Giỏ hàng là bắt buộc")]
        public int GioHangId { get; set; }
        public GioHang? GioHang { get; set; }

        // Thêm một khóa ngoại để tham chiếu đến Món ăn
        public int? MonAnId { get; set; }  // Nullable, vì có thể không phải món ăn
        public MonAn? MonAn { get; set; }

        // Thêm một khóa ngoại để tham chiếu đến Combo
        public int? ComboId { get; set; }  // Nullable, vì có thể không phải combo
        public Combo? Combo { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, 100, ErrorMessage = "Số lượng phải nằm trong khoảng từ 1 đến 100")]
        public int SoLuong { get; set; }
    }

}