using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Site.Data;

namespace Site.Controllers
{
    //[Route("api/[controller]")]
    [Produces("application/json")]
    public class TokenController : Controller
    {
        private IConfiguration _config;
        private readonly ApplicationDbContext _db;
        private readonly IPasswordHasher<IdentityUser> _hasher;
        private readonly UserManager<IdentityUser> _userManager;

        public TokenController(IConfiguration config, ApplicationDbContext db, IPasswordHasher<IdentityUser> hasher, UserManager<IdentityUser> userManager)
        {
            _config = config;
            _db = db;
            _hasher = hasher;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("token")]
        public async Task<IActionResult> CreateToken([FromBody]LoginModel login)
        {
            IActionResult response = Unauthorized();
            var user = await Authenticate(login);

            if (user != null)
            {
                var tokenString = BuildToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string BuildToken(UserModel user)
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Birthdate, user.Birthdate.ToString("yyyy-MM-dd")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
               };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<UserModel> Authenticate([FromBody]LoginModel login)
        {
            UserModel user = null;
            var u = await _userManager.FindByEmailAsync(login.Email);

            if (u == null)
                return null;

            //Working but neeed to extend the default IdentityUser and use that as below
            //https://stackoverflow.com/questions/50428103/changing-identityuser-type-in-asp-net-core-2-1
            var valid = _hasher.VerifyHashedPassword(u, u.PasswordHash, login.Password);
            if (valid == PasswordVerificationResult.Success)
            {
                user = new UserModel { Name = u.Email, Email = u.Email, Birthdate = new DateTime(2001, 5, 13) };
            }

            return user;

        }

        public class LoginModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        private class UserModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public DateTime Birthdate { get; set; }
        }
    }
}