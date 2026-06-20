using Microsoft.AspNetCore.Mvc;
using Expense_Management_System.Services.Auth;
using Expense_Management_System.DTOs.Auth;

namespace Expense_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        //----------LOGIN-----------//

        [HttpPost("login")]
        public IActionResult Login(LoginDto loginDto)
        {
            var result = _authService.Login(loginDto);

            return Ok(result);

        }



    }
}