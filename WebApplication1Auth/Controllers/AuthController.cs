using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Insurance.Data;
using Insurance.Dto;
using Insurance.Model;

namespace Insurance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _users;

        public AuthController(IConfiguration configuration, IUserRepository users)
        {
            _configuration = configuration;
            _users = users;
        }

        //[HttpPost("register")]
        //public async Task<ActionResult<User>> Register(UserDto request)
        //{
        //    CreatedPasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
        //    var user = new User
        //    {
        //        Username = request.Username,
        //        PasswordHash = passwordHash,
        //        PasswordSalt = passwordSalt
        //    };
        //    await _dbContext.Users.AddAsync(user);
        //    await _dbContext.SaveChangesAsync();

        //    return Ok(user);
        //}

        //[HttpPost("login")]
        //public async Task<ActionResult<User>> Login(UserDto request)
        //{
        //    var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        //    if (user == null)
        //    {
        //        return BadRequest("User doesn't exist");
        //    }

        //    if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordHash))
        //    {
        //        return BadRequest("Password doesn't match");
        //    }
        //    return Ok(user);
        //}

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

        private static void CreatedPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
