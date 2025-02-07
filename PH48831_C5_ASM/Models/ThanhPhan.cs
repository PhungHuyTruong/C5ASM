using System.ComponentModel.DataAnnotations;

namespace PH48831_C5_ASM.Models
{
    public class ThanhPhan
    {
        [Key]
        public int ThanhPhanId { get; set; }
        [StringLength(100, ErrorMessage = "Tên loại món không được vượt quá 100 ký tự")]
        public string TenThanhPhan { get; set; }

        public ICollection<MonAn> MonAns { get; set; }
    }
}
