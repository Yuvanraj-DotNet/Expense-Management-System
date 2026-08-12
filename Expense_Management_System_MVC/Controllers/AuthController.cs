using Expense_Management_System_MVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Expense_Management_System_MVC.DTO.Auth;
using System.Text.Json;

namespace Expense_Management_System_MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);

            if (result == null)
            {
                ViewBag.Error = "Invalid Email or Password";
                return View(request);
            }

            HttpContext.Session.SetString("Token", result.Token);
            HttpContext.Session.SetString("UserId", result.UserId.ToString());
            HttpContext.Session.SetString("Name", result.Name);
            HttpContext.Session.SetString("Email", result.Email);
            HttpContext.Session.SetString("RoleId", result.RoleId.ToString());
            HttpContext.Session.SetString("DepartmentId", result.DepartmentId.ToString());

            return RedirectToAction("Index", "Dashboard");
        }
    }
}