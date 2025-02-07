namespace PH48831_C5_ASM.Models
{
    public class TrangThaiHoaDon
    {
        public int IdTrangThai {  get; set; }
        public string TenTrangThai { get; set; }

        public ICollection<HoaDon> HoaDons { get; set; }
    }
}
