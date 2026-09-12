using EcommerceWebApi.Model;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration; // Naya namespace configuration ke liye
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EcommerceWebApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        // Constructor ke zariye configuration inject ki taake appsettings.json read ho sake
        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>
         {
           new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                 new Claim(ClaimTypes.Name, user.Name),
             new Claim(ClaimTypes.Email, user.Email),
             new Claim(ClaimTypes.Role, user.Role) // <--- Yeh claim add karna tha!
          };

            // Ab key direct appsettings.json se aayegi, dono files sync ho gayin!
            var secretKey = _config.GetValue<string>("JWT:Secret");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = creds,
                Issuer = _config.GetValue<string>("JWT:ValidIssuer"),      // <--- Yeh lazmi hai
                Audience = _config.GetValue<string>("JWT:ValidAudience")   // <--- Yeh lazmi hai
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
