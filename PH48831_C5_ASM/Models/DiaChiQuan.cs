using System.ComponentModel.DataAnnotations;

namespace PH48831_C5_ASM.Models
{
    public class DiaChiQuan
    {
        [Key]
        public int DiachiquanId { get; set; }

        [StringLength(100, ErrorMessage = "Tên Kích cỡ không được vượt quá 50 ký tự")]
        public string DiaChiChiTiet { get; set; }

        public ICollection<MonAn> MonAns { get; set; }
    }
}
