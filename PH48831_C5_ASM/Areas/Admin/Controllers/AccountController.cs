using Microsoft.AspNetCore.Mvc;

namespace ASM_PH48831.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home", new { area = "" }); 
        }
    }
}
