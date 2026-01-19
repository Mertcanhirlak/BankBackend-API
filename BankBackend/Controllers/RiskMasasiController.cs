using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankBackend.Data;
using BankBackend.Models;
using BankBackend.Interfaces; // Eklendi

namespace BankBackend.Controllers
{
    /// <summary>
    /// Risk Management Controller - Handles suspicious activity logs and statistics
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RiskMasasiController : ControllerBase
    {
        private readonly BankDbContext _context;
        private readonly ICacheService _cacheService;

        public RiskMasasiController(BankDbContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Get general statistics with Redis caching
        /// </summary>
        [HttpGet("istatistikler")]
        public async Task<IActionResult> GetGenelIstatistikler()
        {
            const string cacheKey = "bank_stats";

            // 1. Check cache first
            var stats = await _cacheService.GetAsync<dynamic>(cacheKey);

            if (stats != null)
            {
                return Ok(new { Data = stats, Kaynak = "Redis Cache" });
            }

            // 2. If not in cache, calculate from database
            var toplamMusteri = await _context.Musterilers.CountAsync();
            var toplamBakiye = await _context.Hesaplars.SumAsync(h => h.Bakiye);
            var toplamIslem = await _context.IslemHareketleris.CountAsync();

            stats = new
            {
                MusteriSayisi = toplamMusteri,
                ToplamMevduat = toplamBakiye,
                IslemSayisi = toplamIslem,
                HesaplanmaTarihi = DateTime.Now
            };

            // 3. Write to cache (valid for 2 minutes)
            await _cacheService.SetAsync(cacheKey, stats, TimeSpan.FromMinutes(2));

            return Ok(new { Data = stats, Kaynak = "PostgreSQL Database" });
        }

        /// <summary>
        /// Get all risk/suspicious activity logs
        /// Admin can view all trigger logs here
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetRiskKayitlari()
        {
            // Table name might be 'RiskMasasis' or 'risk_masasi' from scaffolding
            // Check Models folder for class name if error occurs
            return await _context.RiskMasasis
                                 .OrderByDescending(r => r.OlayTarihi) // Newest first
                                 .ToListAsync();
        }
    }
}