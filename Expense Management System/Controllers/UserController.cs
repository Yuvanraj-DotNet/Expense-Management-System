using Expense_Management_System.DTOs.User;
using Microsoft.AspNetCore.Mvc;
using Expense_Management_System.Services.User;

namespace Expense_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //-------------------------------------//

        [HttpPost]
        public IActionResult CreateUser(CreateUserDto createUserDto)
        {
            var result = _userService.CreateUser(createUserDto);

            return Ok(result);
        }
    }
}