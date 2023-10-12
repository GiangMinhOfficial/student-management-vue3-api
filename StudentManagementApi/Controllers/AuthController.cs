using Microsoft.AspNetCore.Mvc;
using StudentManagementApi.Models;

namespace StudentManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();

        [HttpPost("register")]
        public ActionResult<User> Register(UserDTO userDTO)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);

            user.Username = userDTO.Username;
            user.PasswordHash = passwordHash;

            return Ok(user);
        }

        [HttpPost("login")]
        public ActionResult<User> Login(UserDTO userDTO)
        {
            if (user.Username != userDTO.Username)
            {
                return BadRequest("User not found.");
            }

            if (!BCrypt.Net.BCrypt.Verify(userDTO.Password, user.PasswordHash))
            {
                return BadRequest("Wrong password.");
            }

            return Ok(user);
        }
    }
}
