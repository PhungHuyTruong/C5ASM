using System.ComponentModel.DataAnnotations;

namespace PH48831_C5_ASM.Models
{
    public class ComboChiTiet
    {
        [Key]
        public int ComboChiTietId { get; set; }

        [Required(ErrorMessage = "Combo là bắt buộc")]
        public int ComboId { get; set; }
        public Combo? Combo { get; set; }

        [Required(ErrorMessage = "Món ăn là bắt buộc")]
        public int MonAnId { get; set; }
        public MonAn? MonAn { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, 100, ErrorMessage = "Số lượng phải từ 1 đến 100")]
        public int SoLuong { get; set; }
    }
}
