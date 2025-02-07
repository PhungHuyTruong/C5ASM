using Microsoft.AspNetCore.Mvc;

namespace ASM_PH48831.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        // Phương thức đăng xuất
        public IActionResult Logout()
        {
            // Xóa tất cả session
            HttpContext.Session.Clear();

            // Chuyển hướng về trang chủ hoặc bất kỳ trang nào bạn muốn
            return RedirectToAction("Index", "Home", new { area = "" }); // Trang mặc định (không có Area)
        }
    }
}
