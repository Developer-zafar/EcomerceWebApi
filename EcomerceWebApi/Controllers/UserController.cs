using EcommerceWebApi.Controllers.DTOs;
using EcommerceWebApi.Data;
using EcommerceWebApi.Model;
using EcommerceWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWebApi.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;

        public UserController(
        ApplicationDbContext context,
        PasswordHasher<User> passwordHasher,
        ITokenService tokenService) 
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("api/users/register")]
        public async Task<IActionResult> RegisterUser([FromBody] DTOs.RegisterUserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           
            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
            {
                return Conflict("Email already exists.");
            }
            var newUser = new Model.User
            {
                Name = userDto.Name,
                Email = userDto.Email
            };

            newUser.PasswordHash =
                _passwordHasher.HashPassword(newUser, userDto.Password);

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var userDtoresponse = new UserDto
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Email = newUser.Email
            };

            return CreatedAtAction(
                nameof(GetUser),
                new { id = newUser.Id },
                userDtoresponse
            );
        }
        [HttpPost]
        [Route("api/users/login")]
        public async Task<IActionResult> LoginUser([FromBody] DTOs.LoginUserDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest();
            }
            var user =await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }
            var verificationResult = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                loginDto.Password
            );
            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid email or password.");
            }
            return Ok(_tokenService.CreateToken(user));
        }

        [HttpGet]
        [Route("api/users/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user =await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            UserDto response = new UserDto
            { 
                Name = user.Name,
                Email = user.Email,
                Id = user.Id

            };
            return Ok(response);
        }
    }
}