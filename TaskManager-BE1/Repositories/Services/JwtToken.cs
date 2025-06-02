using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.DataContext.Models;
using TaskManager.Repositories.Interfaces;
using System.Security.Claims;
using System.Text;
using TaskManager.DataContext.Models;
using TaskManager.Repositories.Interfaces;

namespace TaskManager.Repositories.Services
{
    public class JwtToken : IJwtToken
    {
        //IConfiguration to read from appsettings.json.
        private readonly IConfiguration _configuration;

        public JwtToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var issuer = jwtSettings["Issuer"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.FirstName + " "+user.LastName),
                new(ClaimTypes.Email, user.UserName),
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

    }
}
}