using System.ComponentModel.DataAnnotations;

namespace PH48831_C5_ASM.Models
{
    public class LoaiMonAn
    {
        [Key]
        public int LoaiMonAnId { get; set; }

        [Required(ErrorMessage = "Tên loại món là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên loại món không được vượt quá 100 ký tự")]
        public string TenLoaiMon { get; set; }

        public ICollection<MonAn> MonAns { get; set; }
    }
}
