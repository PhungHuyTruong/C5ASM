using PH48831_C5_ASM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace PH48831_C5_ASM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MonAnController : Controller
    {
        private readonly PH48831DbContext _context;

        public MonAnController(PH48831DbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()

        {
            ViewData["Layout"] = "_LayoutAdmin";
            var monAns = await _context.MonAns.Include(m => m.LoaiMonAn).ToListAsync();
            return View(monAns);
        }

        public async Task<IActionResult> Details(int id)
        {
            var monAn = await _context.MonAns
                .Include(m => m.LoaiMonAn)
                .FirstOrDefaultAsync(m => m.MonAnId == id);
            if (monAn == null) return NotFound();

            return View(monAn);
        }

        public IActionResult Create()
        {
            ViewData["Layout"] = "_LayoutAdmin";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MonAn monAn)
        {
            if (ModelState.IsValid)
            {
                _context.Add(monAn);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var loaiMonAnList = await _context.loaiMonAns.ToListAsync();
            var kichCoList = await _context.kichcos.ToListAsync();
            var diaChiQuanList = await _context.diaChiQuans.ToListAsync();
            var thanhPhanList = await _context.thanhPhans.ToListAsync();

            ViewBag.LoaiMonAnList = new SelectList(loaiMonAnList, "LoaiMonAnId", "TenLoaiMon");
            ViewBag.KichCoList = new SelectList(kichCoList, "KichCoId", "TenKichCo");
            ViewBag.DiaChiQuanList = new SelectList(diaChiQuanList, "DiachiquanId", "DiaChi");
            ViewBag.ThanhPhanList = new SelectList(thanhPhanList, "ThanhPhanId", "TenThanhPhan");

            return View(monAn);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var monAn = await _context.MonAns.FindAsync(id);
            if (monAn == null) return NotFound();

            return View(monAn);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MonAn monAn)
        {
            if (id != monAn.MonAnId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(monAn);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.MonAns.Any(e => e.MonAnId == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(monAn);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var monAn = await _context.MonAns
                .Include(m => m.LoaiMonAn)
                .FirstOrDefaultAsync(m => m.MonAnId == id);
            if (monAn == null) return NotFound();

            return View(monAn);
        }

        // 5. Xóa món ăn (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var monAn = await _context.MonAns.FindAsync(id);
            if (monAn != null)
            {
                _context.MonAns.Remove(monAn);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
