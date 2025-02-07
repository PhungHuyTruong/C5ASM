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
            // Lấy ID người dùng
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account"); // Nếu người dùng chưa đăng nhập, chuyển hướng tới trang đăng nhập
            }

            // Kiểm tra món ăn hoặc combo có hợp lệ không (ít nhất một trong hai phải có giá trị)
            if (monAnId == null && comboId == null)
            {
                return BadRequest("Vui lòng chọn món ăn hoặc combo.");
            }

            // Lấy giỏ hàng của người dùng
            var gioHang = await _context.GioHangs
                .Include(g => g.GioHangChiTiets)
                .FirstOrDefaultAsync(g => g.NguoiDungId == userId);

            if (gioHang == null)
            {
                // Nếu người dùng chưa có giỏ hàng, tạo giỏ hàng mới
                gioHang = new GioHang
                {
                    NguoiDungId = userId,
                    GioHangChiTiets = new List<GioHangChiTiet>()
                };
                _context.GioHangs.Add(gioHang);
                await _context.SaveChangesAsync();
            }

            // Tìm xem combo hoặc món ăn đã tồn tại trong giỏ hàng chưa
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
                // Nếu món ăn hoặc combo đã tồn tại trong giỏ hàng, cập nhật số lượng
                gioHangChiTiet.SoLuong += soLuong;
            }
            else
            {
                // Nếu chưa tồn tại, thêm mới vào giỏ hàng
                gioHangChiTiet = new GioHangChiTiet
                {
                    MonAnId = monAnId,
                    ComboId = comboId,
                    SoLuong = soLuong,
                    GioHangId = gioHang.GioHangId
                };
                _context.GioHangChiTiets.Add(gioHangChiTiet);
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Sau khi thêm món ăn hoặc combo vào giỏ hàng, chuyển hướng đến trang xem giỏ hàng
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
                                        .ThenInclude(ghct => ghct.MonAn) // Bao gồm MonAn
                                        .Include(g => g.GioHangChiTiets)
                                        .ThenInclude(ghct => ghct.Combo) // Bao gồm Combo
                                        .FirstOrDefaultAsync(g => g.NguoiDungId == userId);

            if (gioHang != null && gioHang.GioHangChiTiets != null)
            {
                var tongTien = gioHang.GioHangChiTiets.Sum(item =>
                {
                    if (item.MonAn != null)
                    {
                        // Tính giá cho món ăn
                        return item.MonAn.Gia * item.SoLuong;
                    }
                    else if (item.Combo != null)
                    {
                        // Tính giá cho combo
                        return item.Combo.GiaCombo * item.SoLuong;
                    }
                    return 0; // Nếu cả MonAn và Combo đều null, trả về 0
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
                return RedirectToAction("XemGioHang"); // Chuyển hướng đến trang giỏ hàng
            }

            // Cập nhật số lượng giỏ hàng
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

            return RedirectToAction("XemGioHang"); // Chuyển hướng đến trang giỏ hàng sau khi cập nhật
        }

        public async Task<IActionResult> ThanhToan()
        {
            // Lấy ID người dùng
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account"); // Chuyển hướng đến trang đăng nhập nếu chưa đăng nhập
            }

            // Lấy giỏ hàng của người dùng
            var gioHang = await _context.GioHangs
                .Include(g => g.GioHangChiTiets)
                .ThenInclude(gct => gct.MonAn) // Bao gồm món ăn
                .Include(g => g.GioHangChiTiets)
                .ThenInclude(gct => gct.Combo) // Bao gồm combo
                .FirstOrDefaultAsync(g => g.NguoiDungId == userId);

            if (gioHang == null || !gioHang.GioHangChiTiets.Any())
            {
                return BadRequest("Giỏ hàng trống. Vui lòng thêm sản phẩm trước khi thanh toán.");
            }

            // Lấy trạng thái "Đang xử lý" từ bảng TrangThaiHoaDons
            var trangThaiDangXuLy = await _context.TrangThaiHoaDons
                .FirstOrDefaultAsync(tt => tt.TenTrangThai == "Đang xử lý");

            if (trangThaiDangXuLy == null)
            {
                return BadRequest("Không tìm thấy trạng thái 'Đang xử lý' trong hệ thống.");
            }

            // Tạo hóa đơn mới với trạng thái "Đang xử lý"
            var hoaDon = new HoaDon
            {
                NguoiDungId = userId,
                NgayLap = DateTime.Now,
                TongTien = 0, // Sẽ tính sau
                TrangThaiId = trangThaiDangXuLy.IdTrangThai, // Đặt trạng thái thành 'Đang xử lý'
                HoaDonChiTiets = new List<HoaDonChiTiet>()
            };

            decimal tongTien = 0;

            // Duyệt qua từng chi tiết trong giỏ hàng để thêm vào hóa đơn
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

            // Cập nhật tổng tiền của hóa đơn
            hoaDon.TongTien = tongTien;

            // Thêm hóa đơn vào cơ sở dữ liệu
            _context.HoaDons.Add(hoaDon);

            // Xóa giỏ hàng sau khi thanh toán
            _context.GioHangChiTiets.RemoveRange(gioHang.GioHangChiTiets);
            _context.GioHangs.Remove(gioHang);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Chuyển hướng đến trang hiển thị hóa đơn
            return RedirectToAction("XemHoaDon", new { hoaDonId = hoaDon.HoaDonId });
        }



        [HttpPost]
        public async Task<IActionResult> HuyHoaDon(int hoaDonId)
        {
            // Lấy UserId từ Claims (Identity)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Tìm hóa đơn của người dùng dựa trên hoaDonId
            var hoaDon = await _context.HoaDons
                .Include(h => h.TrangThaiHoaDon)
                .Include(h => h.HoaDonChiTiets)
                .ThenInclude(hdct => hdct.MonAn)
                .FirstOrDefaultAsync(h => h.HoaDonId == hoaDonId && h.NguoiDungId == userId);

            if (hoaDon == null)
            {
                return NotFound("Hóa đơn không tồn tại hoặc không thuộc về bạn.");
            }

            // Kiểm tra trạng thái hóa đơn, chỉ cho phép hủy nếu trạng thái là "Đang xử lý"
            var trangThaiDangXuLy = await _context.TrangThaiHoaDons.FirstOrDefaultAsync(tt => tt.TenTrangThai == "Đang xử lý");

            if (hoaDon.TrangThaiId != trangThaiDangXuLy.IdTrangThai)
            {
                return BadRequest("Hóa đơn không thể hủy vì không ở trạng thái 'Đang xử lý'.");
            }

            // Thay đổi trạng thái hóa đơn thành "Đã hủy"
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
        .Include(h => h.TrangThaiHoaDon) // Include trạng thái hóa đơn
        .FirstOrDefaultAsync(h => h.HoaDonId == hoaDonId);

            if (hoaDon == null)
            {
                return NotFound("Không tìm thấy hóa đơn.");
            }

            return View(hoaDon); // Render view XemHoaDon.cshtml
        }


        public async Task<IActionResult> HuyGioHang()
        {
            // Lấy UserId từ Identity
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "DangNhap");
            }

            // Tìm giỏ hàng của người dùng dựa trên UserId
            var gioHang = await _context.GioHangs
                .Include(g => g.GioHangChiTiets)
                .FirstOrDefaultAsync(g => g.NguoiDungId == userId);

            if (gioHang != null && gioHang.GioHangChiTiets.Any())
            {
                // Xóa tất cả các chi tiết giỏ hàng
                _context.GioHangChiTiets.RemoveRange(gioHang.GioHangChiTiets);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("XemGioHang");
        }
    }
}