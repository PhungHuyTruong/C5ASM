using System.ComponentModel.DataAnnotations;

namespace PH48831_C5_ASM.Models
{
    public class GioHang
    {
        [Key]
        public int GioHangId { get; set; }

        [Required(ErrorMessage = "Người dùng là bắt buộc")]
        public string NguoiDungId { get; set; }
        public ApplicationUser? User { get; set; }  // Updated to use ApplicationUser

        public ICollection<GioHangChiTiet> GioHangChiTiets { get; set; }
    }
}
