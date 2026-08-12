using Microsoft.AspNetCore.Mvc;

namespace Expense_Management_System_MVC.Controllers
{
    public class ExpenseController : Controller
    {
        [HttpGet]
        public IActionResult MyExpenses()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }
    }
}