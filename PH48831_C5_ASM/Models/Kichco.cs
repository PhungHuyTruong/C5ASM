using System.ComponentModel.DataAnnotations;

namespace PH48831_C5_ASM.Models
{
    public class Kichco
    {
        [Key]
        public int KichCoId { get; set; }

        [StringLength(100, ErrorMessage = "Tên Kích cỡ không được vượt quá 50 ký tự")]
        public string TenKichCo { get; set; }

        public ICollection<MonAn> MonAns { get; set; }
    }
}
