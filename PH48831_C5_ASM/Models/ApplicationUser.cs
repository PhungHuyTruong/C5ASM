using Microsoft.AspNetCore.Identity;

namespace PH48831_C5_ASM.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }

        public ICollection<HoaDon> HoaDons { get; set; }  // Navigation property for HoaDon
        public ICollection<GioHang> GioHangs { get; set; }
    }
}
