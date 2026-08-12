using Microsoft.AspNetCore.Mvc;

namespace Expense_Management_System_MVC.Controllers
{
    public class DashboardController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var name = HttpContext.Session.GetString("Name");
            var roleId = HttpContext.Session.GetString("RoleId");
            var departmentId = HttpContext.Session.GetString("DepartmentId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.UserId = userId;
            ViewBag.Name = name;
            ViewBag.RoleId = roleId;
            ViewBag.DepartmentId = departmentId;

            return View();
        }
    }
}