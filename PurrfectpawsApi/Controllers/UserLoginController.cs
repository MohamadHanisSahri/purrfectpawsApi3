﻿using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PurrfectpawsApi.DatabaseDbContext;
using PurrfectpawsApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PurrfectpawsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly PurrfectpawsContext _context;

        public UserLoginController(IConfiguration config, PurrfectpawsContext context)
        {
            _configuration = config;
            _context = context;
        }

        // GET: api/TUsers/Login
        [HttpPost("Login")]
        public async Task<ActionResult<TUserLogin>> PostUserLogin([FromBody] TUserLogin login)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }
            var isUserExist = await _context.TUsers.SingleOrDefaultAsync(u => u.Email == login.email);
            if (isUserExist == null)
            {
                return NotFound("Invalid user");
            }

            var roleName = _context.MRoles
                            .Where(r => r.RoleId == isUserExist.RoleId)
                            .Select(r => r.RoleName)
                            .FirstOrDefault();

            if (BCrypt.Net.BCrypt.Verify(login.Password, isUserExist.Password))
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("UserId", isUserExist.UserId.ToString()),
                    new Claim("UserName", isUserExist.Name),
                    new Claim("Email", isUserExist.Email),
                    new Claim("RoleName", roleName)

                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: signIn);
                var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
                isUserExist.AccessToken = accessToken;
                await _context.SaveChangesAsync();

                return Ok(accessToken);

            } else
            {
                return BadRequest("Invalid credentials");
            }

        }

        // DELETE: api/TUsers/Logout/5
        [HttpDelete("Logout/{id}")]
        public async Task<IActionResult> DeleteTUserToken(int id)
        {
            var tUser = await _context.TUsers.FindAsync(id);
            if (tUser == null)
            {
                return NotFound();
            }

            tUser.AccessToken = null;
            
            await _context.SaveChangesAsync();

            return Ok("Token is deleted");
        }
    }
}
