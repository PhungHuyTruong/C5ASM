using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PH48831_C5_ASM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] 
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                ViewData["Layout"] = "_LayoutAdmin";
            }
            else
            {
                ViewData["Layout"] = "_Layout";
            }

            return View();
        }
    }
}
