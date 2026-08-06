using Expense_Management_System_MVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Expense_Management_System_MVC.DTO.Auth;

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

            ViewBag.Success = "Login Successful";

            return View();
        }
    }
}