using Microsoft.AspNetCore.Mvc;
using Basic_Auth.Model;
using Basic_Auth.Model.dto;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Basic_Auth.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _Config;

        public UserController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
            _Config = config;
        }

        // Create User
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] Userdto user)
        {
            if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Name, Email, and Password are required.");
            }

            var result = await _userService.CreateUserAsync(user);
            if (result == null)
            {
                return Conflict(new {data = "", message = "User Already Exist!"});
            }
            return Ok(new {data = new { id= result.Id, Name= result.Name, Email = result.Email, created_At = result.CreatedAt  }, message = "User Created Successfully!"});
        }

        // Update User
        [HttpPut("/{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] Namedto body)
        {
            var result = await _userService.UpdateUserAsync(id, body.Name);
            if (result == null)
            {
                return NotFound(new { data = result, message = "User not found!" });
            }
            return Ok(new { data = new { id = result.Id, Name = result.Name, Email = result.Email, created_At = result.CreatedAt }, message = "User Updated Successfully!" });
        }

        // Delete User
        [HttpDelete("/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result == null)
            {
                return NotFound(new { data = result, message = "User not found!" });
            }
            return Ok(new { data = result, message = "User deleted Successfully!" });
        }

        // Find User by Email
        [HttpGet("/{id}")]
        [Authorize]
        public async Task<IActionResult> FindUser(Guid id)
        {
            var user = await _userService.FindUserAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            return Ok(user);
        }

        [HttpPost("/login")]
        public async Task<IActionResult> LoginUser([FromBody] Logindto logindata)
        {
            var result = await _userService.FindUserByEmailAsync(logindata.Email);
            if(result == null)
            {
                return NotFound(new { data = "", message = $"User with email {logindata.Email} not found!" });
            }
            var IsPasswordMatched = _userService.VerifyPassword(logindata.Password, result.Password);
            if (!IsPasswordMatched)
            {
                return Unauthorized(new { data = "", message = "Wrong password" });
            }
            var jwt = GenerateJwtToken(result.Id, result.Name, result.Email);
            return Ok(new { data = new { id = result.Id, Name = result.Name, Email = result.Email, created_At = result.CreatedAt }, accessToken = jwt, message = "Logged in Successfully", status = "success" });
        }
        private string GenerateJwtToken(Guid Id, string Name, string Email)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, Email),
            new Claim("userId", Id.ToString()),
            };
            Console.WriteLine(_Config["Jwt:ValidIssuer"]);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Config["Jwt:JWTSECRET"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _Config["Jwt:ValidIssuer"],
                audience: _Config["Jwt:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static (string? Id, string? Name, string? Email) DecodeJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Extract claims
            string? Id = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            string? Name = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            string? Email = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;

            return  (Id, Name, Email);

        }
    }
}
