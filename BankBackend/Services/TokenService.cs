using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BankBackend.Interfaces;
using BankBackend.Models;
using Microsoft.IdentityModel.Tokens;

namespace BankBackend.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Musteriler musteri)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, musteri.MusteriId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, musteri.TcKimlikNo ?? ""), // TC'yi claim olarak tutuyoruz
                new Claim(ClaimTypes.Role, musteri.Rol ?? "MUSTERI"),
                new Claim("AdSoyad", $"{musteri.Ad} {musteri.Soyad}")
            };

            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["DurationInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
