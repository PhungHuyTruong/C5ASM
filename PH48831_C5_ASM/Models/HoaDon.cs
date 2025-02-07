using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PH48831_C5_ASM.Models
{
    public class HoaDon
    {
        [Key]
        public int HoaDonId { get; set; }

        [Required(ErrorMessage = "Người dùng là bắt buộc")]
        public string NguoiDungId { get; set; }
        public ApplicationUser? User { get; set; } 

        [Required(ErrorMessage = "Ngày lập là bắt buộc")]
        public DateTime NgayLap { get; set; }

        [Required(ErrorMessage = "Tổng tiền là bắt buộc")]
        public decimal TongTien { get; set; }

        [Required(ErrorMessage = "Trạng thái là bắt buộc")]
        public int TrangThaiId { get; set; }

        [ForeignKey("TrangThaiId")]
        public TrangThaiHoaDon TrangThaiHoaDon { get; set; }

        public ICollection<HoaDonChiTiet> HoaDonChiTiets { get; set; }
    }
}
