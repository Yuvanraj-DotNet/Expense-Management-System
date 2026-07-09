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
        public IActionResult GetAllUsers
(
    string? search = "",
    int pageNumber = 1,
    int pageSize = 10
)
        {
            int totalRecords;

            var users = _userService.GetAllUsers
            (
                search,
                pageNumber,
                pageSize,
                out totalRecords
            );

            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            return Ok(new
            {
                TotalRecords = totalRecords,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                Data = users
            });
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

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, UpdateUserDto updateUserDto)
        {
            var result = _userService.UpdateUser(id, updateUserDto);

            if (result == "User Updated Successfully")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


    }
}