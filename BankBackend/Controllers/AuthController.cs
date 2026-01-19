using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankBackend.Data;
using BankBackend.Models;
using System.Security.Cryptography;
using System.Text;

namespace BankBackend.Controllers
{
    /// <summary>
    /// Login Model for authentication
    /// </summary>
    public class LoginModel
    {
        public string? TcKimlikNo { get; set; }
        public string? Sifre { get; set; }
    }

    /// <summary>
    /// Authentication Controller - Handles user login and JWT token generation
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly BankDbContext _context;
        private readonly BankBackend.Interfaces.ITokenService _tokenService;

        public AuthController(BankDbContext context, BankBackend.Interfaces.ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        /// <summary>
        /// User login endpoint - Authenticates user and returns JWT token
        /// </summary>
        /// <param name="model">Login credentials (ID and Password)</param>
        /// <returns>User information with JWT token</returns>
        [HttpPost("giris")]
        public async Task<IActionResult> GirisYap([FromBody] LoginModel model)
        {
            if (string.IsNullOrEmpty(model.TcKimlikNo) || string.IsNullOrEmpty(model.Sifre))
            {
                return BadRequest("ID and Password are required.");
            }

            var kullanici = await _context.Musterilers.FirstOrDefaultAsync(m => m.TcKimlikNo == model.TcKimlikNo);

            if (kullanici == null)
            {
                return Unauthorized("User not found.");
            }

            string girilenSifreHash = ComputeSha256Hash(model.Sifre);
            bool sifreDogruMu = false;
            bool sifreGuncellenmeli = false;

            string dbSifre = kullanici.Sifre?.Trim();

            if (dbSifre == girilenSifreHash)
            {
                sifreDogruMu = true;
            }
            else if (model.Sifre != null && dbSifre == model.Sifre)
            {
                sifreDogruMu = true;
                sifreGuncellenmeli = true;
            }

            if (!sifreDogruMu)
            {
                return Unauthorized("Invalid password.");
            }

            if (sifreGuncellenmeli)
            {
                try
                {
                    kullanici.Sifre = girilenSifreHash;
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    // Don't block login even if password update fails
                }
            }

            // Generate JWT Token
            var token = _tokenService.GenerateToken(kullanici);

            return Ok(new
            {
                MusteriId = kullanici.MusteriId,
                Ad = kullanici.Ad,
                Soyad = kullanici.Soyad,
                Rol = kullanici.Rol,
                Token = token,
                Mesaj = "Login Successful"
            });
        }

        /// <summary>
        /// Helper Method: SHA256 Hashing
        /// </summary>
        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}