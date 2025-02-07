using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PH48831_C5_ASM.Models;

namespace PH48831_C5_ASM.Controllers
{
    public class MonAnController : Controller
    {
        private readonly PH48831DbContext _context;

        public MonAnController(PH48831DbContext context)
        {
            _context = context;
        }

        public IActionResult Index(HomeQueryParameters parameters)
        {

            var monAnList = _context.MonAns.Include(m => m.LoaiMonAn).AsQueryable();

            if (!string.IsNullOrEmpty(parameters.Keyword))
            {
                monAnList = monAnList.Where(m => m.TenMon.Contains(parameters.Keyword));
            }

            if (parameters.FromPrice.HasValue)
            {
                monAnList = monAnList.Where(m => m.Gia >= parameters.FromPrice);
            }
            if (parameters.ToPrice.HasValue)
            {
                monAnList = monAnList.Where(m => m.Gia <= parameters.ToPrice);
            }

            if (!string.IsNullOrEmpty(parameters.SortBy) && parameters.SortBy != "tatca")
            {
                monAnList = monAnList.Where(m => m.LoaiMonAn.TenLoaiMon == parameters.SortBy);
            }

            if (parameters.SortBy == "TenMon" && parameters.Order == "Ascending")
            {
                monAnList = monAnList.OrderBy(m => m.TenMon);
            }
            else if (parameters.SortBy == "TenMon" && parameters.Order == "Descending")
            {
                monAnList = monAnList.OrderByDescending(m => m.TenMon);
            }
            else if (parameters.SortBy == "Gia" && parameters.Order == "Ascending")
            {
                monAnList = monAnList.OrderBy(m => m.Gia);
            }
            else if (parameters.SortBy == "Gia" && parameters.Order == "Descending")
            {
                monAnList = monAnList.OrderByDescending(m => m.Gia);
            }

            int totalItems = monAnList.Count();
            int pageSize = parameters.PageSize > 0 ? parameters.PageSize : 9;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            int currentPage = parameters.Page > 0 ? parameters.Page : 1;

            monAnList = monAnList.Skip((currentPage - 1) * pageSize).Take(pageSize);

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = currentPage;

            //ViewBag.CurrentPage = pageNumber;

            return View(monAnList.ToList());
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var chitietmonan = _context.MonAns
                        .Include(m => m.LoaiMonAn)
                        .Include(m => m.ThanhPhan)
                        .Include(m => m.DiaChiQuan).FirstOrDefault(x => x.MonAnId == id);
            return View(chitietmonan);
        }
        [HttpPost]
        public IActionResult Details(MonAn monAn)
        {
            _context.MonAns.ToList();
            return RedirectToAction("Index");
        }
    }
}
