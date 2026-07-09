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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _userService.CreateUser(createUserDto);

            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var result = _userService.GetAllUsers();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound("User Not Found");
            }

            return Ok(user);
        }

        [HttpGet("search")]
        public IActionResult SearchUsers(string keyword)
        {
            var users = _userService.SearchUsers(keyword);

            if (!users.Any())
            {
                return NotFound("No Users Found");
            }

            return Ok(users);
        }
    }
}