using System.ComponentModel.DataAnnotations;

namespace PH48831_C5_ASM.Models
{
    public class MonAn
    {
        [Key]
        public int MonAnId { get; set; }

        [Required(ErrorMessage = "Tên món là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên món không được vượt quá 100 ký tự")]
        public string TenMon { get; set; }

        [Required(ErrorMessage = "Giá là bắt buộc")]
        [Range(1000, 1000000, ErrorMessage = "Giá phải nằm trong khoảng từ 1.000 đến 1.000.000")]
        public decimal Gia { get; set; }

        [StringLength(200, ErrorMessage = "Mô tả không được vượt quá 200 ký tự")]
        public string MoTa { get; set; }

        [StringLength(255, ErrorMessage = "Đường dẫn ảnh không được vượt quá 255 ký tự")]
        public string HinhAnh { get; set; }  // Không còn [Required] ở đây
        public int LoaiMonAnId { get; set; }
        public LoaiMonAn? LoaiMonAn { get; set; }
        public int? KichCoId { get; set; }
        public Kichco? Kichco { get; set; }

        [Required(ErrorMessage = "Địa chỉ quán là bắt buộc")]
        public int DiachiquanId { get; set; }
        public DiaChiQuan? DiaChiQuan { get; set; }

        public int ThanhPhanId { get; set; }
        public ThanhPhan? ThanhPhan { get; set; }

        public ICollection<ComboChiTiet> ComboChiTiets { get; set; }
        public ICollection<GioHangChiTiet> GioHangChiTiets { get; set; }
        public ICollection<HoaDonChiTiet> HoaDonChiTiets { get; set; }
    }
}
