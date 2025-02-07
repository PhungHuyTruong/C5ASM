using PH48831_C5_ASM.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Security.Claims;
    using System.Linq;
using System.Globalization;

namespace PH48831_C5_ASM.Controllers
{
    public class GioHangController : Controller
    {
        private readonly PH48831DbContext _context;

        public GioHangController(PH48831DbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> LichSuHoaDon()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var hoaDons = await _context.HoaDons
                .Where(hd => hd.NguoiDungId == userId)
                .Include(hd => hd.TrangThaiHoaDon)
                .Include(hd => hd.HoaDonChiTiets)
                .ThenInclude(hdct => hdct.MonAn)
                .ToListAsync();

            return View(hoaDons);
        }


        public async Task<IActionResult> ThemVaoGioHang(int? monAnId, int? comboId, int soLuong)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            if (monAnId == null && comboId == null)
            {
                return BadRequest("Vui lòng chọn món ăn hoặc combo.");
            }

            var gioHang = await _context.GioHangs
                .Include(g => g.GioHangChiTiets)
                .FirstOrDefaultAsync(g => g.NguoiDungId == userId);

            if (gioHang == null)
            {
                gioHang = new GioHang
                {
                    NguoiDungId = userId,
                    GioHangChiTiets = new List<GioHangChiTiet>()
                };
                _context.GioHangs.Add(gioHang);
                await _context.SaveChangesAsync();
            }

            GioHangChiTiet gioHangChiTiet = null;
            if (monAnId.HasValue)
            {
                gioHangChiTiet = gioHang.GioHangChiTiets.FirstOrDefault(gct => gct.MonAnId == monAnId.Value);
            }
            else if (comboId.HasValue)
            {
                gioHangChiTiet = gioHang.GioHangChiTiets.FirstOrDefault(gct => gct.ComboId == comboId.Value);
            }

            if (gioHangChiTiet != null)
            {
                gioHangChiTiet.SoLuong += soLuong;
            }
            else
            {
                gioHangChiTiet = new GioHangChiTiet
                {
                    MonAnId = monAnId,
                    ComboId = comboId,
                    SoLuong = soLuong,
                    GioHangId = gioHang.GioHangId
                };
                _context.GioHangChiTiets.Add(gioHangChiTiet);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("XemGioHang");
        }
        public async Task<IActionResult> XemGioHang()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var gioHang = await _context.GioHangs
                                        .Include(g => g.GioHangChiTiets)
                                        .ThenInclude(ghct => ghct.MonAn) 
                                        .Include(g => g.GioHangChiTiets)
                                        .ThenInclude(ghct => ghct.Combo) 
                                        .FirstOrDefaultAsync(g => g.NguoiDungId == userId);

            if (gioHang != null && gioHang.GioHangChiTiets != null)
            {
                var tongTien = gioHang.GioHangChiTiets.Sum(item =>
                {
                    if (item.MonAn != null)
                    {
                        return item.MonAn.Gia * item.SoLuong;
                    }
                    else if (item.Combo != null)
                    {
                        return item.Combo.GiaCombo * item.SoLuong;
                    }
                    return 0; 
                });
                ViewData["TongTien"] = tongTien.ToString("C", new System.Globalization.CultureInfo("vi-VN"));
            }

            return View(gioHang);
        }


        [HttpPost]
        public async Task<IActionResult> CapNhatSoLuong(List<GioHangChiTiet> gioHangChiTiets)
        {
            if (gioHangChiTiets == null || gioHangChiTiets.Count == 0)
            {
                ModelState.AddModelError("", "Không có sản phẩm nào để cập nhật.");
                return RedirectToAction("XemGioHang");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var gioHang = await _context.GioHangs
                .Include(g => g.GioHangChiTiets)
                .ThenInclude(ghct => ghct.MonAn)
                .FirstOrDefaultAsync(g => g.NguoiDungId == userId);

            if (gioHang != null)
            {
                foreach (var item in gioHangChiTiets)
                {
                    var gioHangChiTiet = gioHang.GioHangChiTiets
                        .FirstOrDefault(ghct => ghct.GioHangChiTietId == item.GioHangChiTietId);

                    if (gioHangChiTiet != null)
                    {
                        gioHangChiTiet.SoLuong = item.SoLuong > 0 ? item.SoLuong : 1;
                    }
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("XemGioHang");
        }

        public async Task<IActionResult> ThanhToan()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account"); 
            }

            var gioHang = await _context.GioHangs
                .Include(g => g.GioHangChiTiets)
                .ThenInclude(gct => gct.MonAn) 
                .Include(g => g.GioHangChiTiets)
                .ThenInclude(gct => gct.Combo) 
                .FirstOrDefaultAsync(g => g.NguoiDungId == userId);

            if (gioHang == null || !gioHang.GioHangChiTiets.Any())
            {
                return BadRequest("Giỏ hàng trống. Vui lòng thêm sản phẩm trước khi thanh toán.");
            }

            var trangThaiDangXuLy = await _context.TrangThaiHoaDons
                .FirstOrDefaultAsync(tt => tt.TenTrangThai == "Đang xử lý");

            if (trangThaiDangXuLy == null)
            {
                return BadRequest("Không tìm thấy trạng thái 'Đang xử lý' trong hệ thống.");
            }

            var hoaDon = new HoaDon
            {
                NguoiDungId = userId,
                NgayLap = DateTime.Now,
                TongTien = 0, 
                TrangThaiId = trangThaiDangXuLy.IdTrangThai,
                HoaDonChiTiets = new List<HoaDonChiTiet>()
            };

            decimal tongTien = 0;

            foreach (var chiTiet in gioHang.GioHangChiTiets)
            {
                decimal donGia = 0;

                if (chiTiet.MonAnId.HasValue)
                {
                    donGia = chiTiet.MonAn.Gia;

                    hoaDon.HoaDonChiTiets.Add(new HoaDonChiTiet
                    {
                        HoaDonId = hoaDon.HoaDonId,
                        MonAnId = chiTiet.MonAnId.Value,
                        SoLuong = chiTiet.SoLuong,
                        DonGia = donGia
                    });
                }
                else if (chiTiet.ComboId.HasValue)
                {
                    donGia = chiTiet.Combo.GiaCombo;

                    hoaDon.HoaDonChiTiets.Add(new HoaDonChiTiet
                    {
                        HoaDonId = hoaDon.HoaDonId,
                        ComboId = chiTiet.ComboId.Value,
                        SoLuong = chiTiet.SoLuong,
                        DonGia = donGia
                    });
                }

                tongTien += donGia * chiTiet.SoLuong;
            }

            hoaDon.TongTien = tongTien;

            _context.HoaDons.Add(hoaDon);

            _context.GioHangChiTiets.RemoveRange(gioHang.GioHangChiTiets);
            _context.GioHangs.Remove(gioHang);

            await _context.SaveChangesAsync();

            return RedirectToAction("XemHoaDon", new { hoaDonId = hoaDon.HoaDonId });
        }



        [HttpPost]
        public async Task<IActionResult> HuyHoaDon(int hoaDonId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.TrangThaiHoaDon)
                .Include(h => h.HoaDonChiTiets)
                .ThenInclude(hdct => hdct.MonAn)
                .FirstOrDefaultAsync(h => h.HoaDonId == hoaDonId && h.NguoiDungId == userId);

            if (hoaDon == null)
            {
                return NotFound("Hóa đơn không tồn tại hoặc không thuộc về bạn.");
            }

            var trangThaiDangXuLy = await _context.TrangThaiHoaDons.FirstOrDefaultAsync(tt => tt.TenTrangThai == "Đang xử lý");

            if (hoaDon.TrangThaiId != trangThaiDangXuLy.IdTrangThai)
            {
                return BadRequest("Hóa đơn không thể hủy vì không ở trạng thái 'Đang xử lý'.");
            }

            var trangThaiDaHuy = await _context.TrangThaiHoaDons.FirstOrDefaultAsync(tt => tt.TenTrangThai == "Đã hủy");
            hoaDon.TrangThaiId = trangThaiDaHuy.IdTrangThai;

            await _context.SaveChangesAsync();

            return RedirectToAction("LichSuHoaDon");
        }

        public async Task<IActionResult> XemHoaDon(int hoaDonId)
        {
            var hoaDon = await _context.HoaDons
        .Include(h => h.HoaDonChiTiets)
            .ThenInclude(hct => hct.MonAn)
        .Include(h => h.HoaDonChiTiets)
            .ThenInclude(hct => hct.Combo)
        .Include(h => h.TrangThaiHoaDon)
        .FirstOrDefaultAsync(h => h.HoaDonId == hoaDonId);

            if (hoaDon == null)
            {
                return NotFound("Không tìm thấy hóa đơn.");
            }

            return View(hoaDon);
        }


        public async Task<IActionResult> HuyGioHang()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "DangNhap");
            }

            var gioHang = await _context.GioHangs
                .Include(g => g.GioHangChiTiets)
                .FirstOrDefaultAsync(g => g.NguoiDungId == userId);

            if (gioHang != null && gioHang.GioHangChiTiets.Any())
            {
                _context.GioHangChiTiets.RemoveRange(gioHang.GioHangChiTiets);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("XemGioHang");
        }
    }
}