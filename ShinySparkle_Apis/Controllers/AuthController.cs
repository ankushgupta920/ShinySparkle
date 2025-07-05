using Common;
using DataAcess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShinySparkle_Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginVM model)
        {
            // Find user by email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "User not found" });
            }

            // Validate password
            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            // Get user roles
            var roles = await _userManager.GetRolesAsync(user);

            // Build claims
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Get key and generate token
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }


        //public async Task<IActionResult> Login([FromBody] LoginVM model)
        //{
        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user == null)
        //        return Unauthorized(new { message = "User not found" });

        //    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);
        //    if (result.Succeeded)
        //    {
        //        var token = await GenerateJwtToken(user);
        //        return Ok(new { token });
        //    }
        //    return Unauthorized(new { message = "Invalid credentials" });
        //}

        //private async Task<string> GenerateJwtToken(ApplicationUser user)
        //{
        //    var userRoles = await _userManager.GetRolesAsync(user);
        //    var claims = new List<Claim>
        //    {
        //    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        //    new Claim(JwtRegisteredClaimNames.Email, user.Email),
        //    new Claim(ClaimTypes.Name, user.UserName),
        //    //new Claim(ClaimTypes.Role, "Admin") // ✅ Add this
        //};
        //    foreach (var role in userRoles)
        //    {
        //        claims.Add(new Claim(ClaimTypes.Role, role)); // 🔥 This adds the user's roles
        //    }
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    var token = new JwtSecurityToken(
        //        _config["Jwt:Issuer"],
        //        _config["Jwt:Issuer"],
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddHours(1),
        //        signingCredentials: creds
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
     
    }
}
