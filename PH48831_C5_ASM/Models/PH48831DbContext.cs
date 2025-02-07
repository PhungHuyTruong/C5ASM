using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PH48831_C5_ASM.Models
{
    public class PH48831DbContext : IdentityDbContext<ApplicationUser>
    {
        public PH48831DbContext(DbContextOptions<PH48831DbContext> options) : base(options)
        {
        }
        public DbSet<MonAn> MonAns { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<ComboChiTiet> ComboChiTiets { get; set; }
        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<GioHangChiTiet> GioHangChiTiets { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<HoaDonChiTiet> HoaDonChiTiets { get; set; }
        public DbSet<TrangThaiHoaDon> TrangThaiHoaDons { get; set; }
        public DbSet<DiaChiQuan> diaChiQuans { get; set; }
        public DbSet<ThanhPhan> thanhPhans { get; set; }
        public DbSet<LoaiMonAn> loaiMonAns { get; set; }
        public DbSet<Kichco> kichcos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TrangThaiHoaDon>()
                .HasKey(t => t.IdTrangThai);

            modelBuilder.Entity<HoaDon>()
                .HasOne(h => h.User)
                .WithMany(nd => nd.HoaDons)
                .HasForeignKey(h => h.NguoiDungId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HoaDon>()
                .HasOne(h => h.TrangThaiHoaDon)
                .WithMany()
                .HasForeignKey(h => h.TrangThaiId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ComboChiTiet>()
                .HasOne(cct => cct.Combo)
                .WithMany(c => c.ComboChiTiets)
                .HasForeignKey(cct => cct.ComboId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ComboChiTiet>()
                .HasOne(cct => cct.MonAn)
                .WithMany(ma => ma.ComboChiTiets)
                .HasForeignKey(cct => cct.MonAnId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ComboChiTiet>()
                .HasOne(cct => cct.MonAn)
                .WithMany(ma => ma.ComboChiTiets)
                .HasForeignKey(cct => cct.MonAnId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GioHangChiTiet>()
                .HasOne(ghct => ghct.GioHang)
                .WithMany(gh => gh.GioHangChiTiets)
                .HasForeignKey(ghct => ghct.GioHangId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GioHangChiTiet>()
                .HasOne(ghct => ghct.MonAn)
                .WithMany(ma => ma.GioHangChiTiets)
                .HasForeignKey(ghct => ghct.MonAnId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HoaDonChiTiet>()
                .HasOne(hdct => hdct.HoaDon)
                .WithMany(hd => hd.HoaDonChiTiets)
                .HasForeignKey(hdct => hdct.HoaDonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HoaDonChiTiet>()
                .HasOne(hdct => hdct.MonAn)
                .WithMany(ma => ma.HoaDonChiTiets)
                .HasForeignKey(hdct => hdct.MonAnId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MonAn>()
                .Property(ma => ma.Gia)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Combo>()
                .Property(c => c.GiaCombo)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<HoaDon>()
                .Property(h => h.TongTien)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<HoaDonChiTiet>()
                .Property(hdct => hdct.DonGia)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<HoaDonChiTiet>()
                .HasOne(hdct => hdct.Combo)
                .WithMany(c => c.HoaDonChiTiets)
                .HasForeignKey(hdct => hdct.ComboId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<GioHang>()
            .HasOne(gh => gh.User) // Mỗi GioHang có một User
            .WithMany(u => u.GioHangs) // Mỗi User có thể có nhiều GioHang
            .HasForeignKey(gh => gh.NguoiDungId) // Khóa ngoại là NguoiDungId
            .OnDelete(DeleteBehavior.Cascade); // Xóa GioHang khi xóa User

            // Cấu hình quan hệ giữa GioHang và GioHangChiTiet
            modelBuilder.Entity<GioHang>()
                .HasMany(gh => gh.GioHangChiTiets) // Mỗi GioHang có nhiều GioHangChiTiet
                .WithOne(ghct => ghct.GioHang) // Mỗi GioHangChiTiet thuộc về một GioHang
                .HasForeignKey(ghct => ghct.GioHangId) // Khóa ngoại là GioHangId
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LoaiMonAn>().HasData(
            new LoaiMonAn { LoaiMonAnId = 1, TenLoaiMon = "Đồ chay" },
            new LoaiMonAn { LoaiMonAnId = 2, TenLoaiMon = "Thịt cá" },
            new LoaiMonAn { LoaiMonAnId = 3, TenLoaiMon = "Món ăn khác" }
            );

            modelBuilder.Entity<Kichco>().HasData(
                new Kichco { KichCoId = 1, TenKichCo = "Lớn" },
                new Kichco { KichCoId = 2, TenKichCo = "Nhỏ" },
                new Kichco { KichCoId = 3, TenKichCo = "Trung Bình" }
            );
            modelBuilder.Entity<ThanhPhan>().HasData(
                new ThanhPhan { ThanhPhanId = 1, TenThanhPhan = "Thịt gà, bột mì" },
                new ThanhPhan { ThanhPhanId = 2, TenThanhPhan = "Phô mai, cà chua" },
                new ThanhPhan { ThanhPhanId = 3, TenThanhPhan = "Thịt cá" }
            );
            modelBuilder.Entity<DiaChiQuan>().HasData(
                new DiaChiQuan { DiachiquanId = 1, DiaChiChiTiet = "Hà nội" },
                new DiaChiQuan { DiachiquanId = 2, DiaChiChiTiet = "Hải phòng" },
                new DiaChiQuan { DiachiquanId = 3, DiaChiChiTiet = "Hải Dương" }
            );

            modelBuilder.Entity<MonAn>().HasData(
                new MonAn { MonAnId = 1, TenMon = "Pizza", Gia = 200000, MoTa = "Pizza truyền thống", HinhAnh = "/Images2/hosting-fpt-removebg-preview.png", LoaiMonAnId = 3, ThanhPhanId = 1, DiachiquanId = 1 },
                new MonAn { MonAnId = 2, TenMon = "Bánh mì", Gia = 15000, MoTa = "Bánh mì Việt Nam", HinhAnh = "/Images2/img-1000x600-1.jpg", LoaiMonAnId = 1, ThanhPhanId = 1, DiachiquanId = 2 },
                new MonAn { MonAnId = 3, TenMon = "Bún bò Huế", Gia = 60000, MoTa = "Món bún nổi tiếng", HinhAnh = "/Images2/istockphoto-1200064755-170667a.jpg", LoaiMonAnId = 2, ThanhPhanId = 2, DiachiquanId = 3 },
                new MonAn { MonAnId = 4, TenMon = "Phở", Gia = 50000, MoTa = "Món phở truyền thống", HinhAnh = "/Images2/istockphoto-1200064755-170667a.jpg", LoaiMonAnId = 2, ThanhPhanId = 3, DiachiquanId = 1 },
                new MonAn { MonAnId = 5, TenMon = "Gà rán", Gia = 70000, MoTa = "Gà rán giòn tan", HinhAnh = "/Images2/istockphoto-1200064755-170667a.jpg", LoaiMonAnId = 2, ThanhPhanId = 2, DiachiquanId = 2 },
                new MonAn { MonAnId = 6, TenMon = "Bánh xèo", Gia = 30000, MoTa = "Bánh xèo giòn", HinhAnh = "/Images2/istockphoto-1200064755-170667a.jpg", LoaiMonAnId = 1, ThanhPhanId = 1, DiachiquanId = 3 },
                new MonAn { MonAnId = 7, TenMon = "Hủ tiếu", Gia = 45000, MoTa = "Hủ tiếu Nam Vang", HinhAnh = "/Images2/istockphoto-1200064755-170667a.jpg", LoaiMonAnId = 2, ThanhPhanId = 2, DiachiquanId = 1 },
                new MonAn { MonAnId = 8, TenMon = "Sushi", Gia = 120000, MoTa = "Món sushi Nhật Bản", HinhAnh = "/Images2/istockphoto-1200064755-170667a.jpg", LoaiMonAnId = 2, ThanhPhanId = 3, DiachiquanId = 2 },
                new MonAn { MonAnId = 9, TenMon = "Mì Ý", Gia = 80000, MoTa = "Mì Ý sốt cà chua", HinhAnh = "/Images2/z6035875873720_114730cc66faa67749315b417c790719.jpg", LoaiMonAnId = 3, ThanhPhanId = 1, DiachiquanId = 3 },
                new MonAn { MonAnId = 10, TenMon = "Trà sữa", Gia = 25000, MoTa = "Trà sữa truyền thống", HinhAnh = "/Images2/z6035875881929_d785375a00c2dabf615d911288f4930a.jpg", LoaiMonAnId = 1, ThanhPhanId = 2, DiachiquanId = 1 },
                new MonAn { MonAnId = 11, TenMon = "Salad", Gia = 30000, MoTa = "Salad rau củ tươi ngon", HinhAnh = "/Images2/z6042058761907_810f9203bd7cd908f0e8c873ab0bbfd3.jpg", LoaiMonAnId = 1, ThanhPhanId = 3, DiachiquanId = 2 },
                new MonAn { MonAnId = 12, TenMon = "Chè", Gia = 20000, MoTa = "Chè đậu xanh", HinhAnh = "/Images2/z6042058820293_1b98c0f4626db7ed36546748783f2ca4.jpg", LoaiMonAnId = 1, ThanhPhanId = 1, DiachiquanId = 3 }
            );

            modelBuilder.Entity<Combo>().HasData(
                new Combo { ComboId = 1, TenCombo = "Combo Bữa Trưa", GiaCombo = 50000 },
                new Combo { ComboId = 2, TenCombo = "Combo Bữa Tối", GiaCombo = 100000 }
            );

            modelBuilder.Entity<ComboChiTiet>().HasData(
                new ComboChiTiet { ComboChiTietId = 1, ComboId = 1, MonAnId = 1, SoLuong = 2 },
                new ComboChiTiet { ComboChiTietId = 2, ComboId = 1, MonAnId = 2, SoLuong = 1 },
                new ComboChiTiet { ComboChiTietId = 3, ComboId = 1, MonAnId = 3, SoLuong = 1 }

                , new ComboChiTiet { ComboChiTietId = 4, ComboId = 2, MonAnId = 4, SoLuong = 1 },
                new ComboChiTiet { ComboChiTietId = 5, ComboId = 2, MonAnId = 5, SoLuong = 1 }
            );

            modelBuilder.Entity<TrangThaiHoaDon>().HasData(
                new TrangThaiHoaDon { IdTrangThai = 1, TenTrangThai = "Đang xử lý" },
                new TrangThaiHoaDon { IdTrangThai = 2, TenTrangThai = "Đang giao" },
                new TrangThaiHoaDon { IdTrangThai = 3, TenTrangThai = "Đã giao" },
                new TrangThaiHoaDon { IdTrangThai = 4, TenTrangThai = "Đã thanh toán" },
                new TrangThaiHoaDon { IdTrangThai = 5, TenTrangThai = "Đã hủy" }
            );
        }
    }
}
