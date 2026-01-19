using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankBackend.Data;
using BankBackend.Models;

namespace BankBackend.Controllers
{
    /// <summary>
    /// Transaction History Controller - Manages transaction records and statistics
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class IslemHareketleriController : ControllerBase
    {
        private readonly BankDbContext _context;

        public IslemHareketleriController(BankDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get recent transactions for a customer
        /// </summary>
        [HttpGet("Musteri/{musteriId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetSonIslemler(int musteriId)
        {
            // 1. Find customer's account IDs
            var musteriHesapIdleri = await _context.Hesaplars
                                                   .Where(h => h.MusteriId == musteriId)
                                                   .Select(h => h.HesapId)
                                                   .ToListAsync();

            if (!musteriHesapIdleri.Any())
            {
                return Ok(new List<object>());
            }

            // 2. Fetch transactions (both outgoing and incoming)
            // Use Include to access customer info for names
            var hamIslemler = await _context.IslemHareketleris
                                         .Include(i => i.GonderenHesap).ThenInclude(h => h.Musteri)
                                         .Include(i => i.AliciHesap).ThenInclude(h => h.Musteri)
                                         .Where(i => (i.GonderenHesapId.HasValue && musteriHesapIdleri.Contains(i.GonderenHesapId.Value)) || 
                                                     (i.AliciHesapId.HasValue && musteriHesapIdleri.Contains(i.AliciHesapId.Value)))
                                         .OrderByDescending(i => i.IslemTarihi)
                                         .Take(20) // Last 20 transactions
                                         .ToListAsync();

            // 3. Format the data
            var islemler = hamIslemler.Select(i => {
                bool gidenPara = i.GonderenHesapId.HasValue && musteriHesapIdleri.Contains(i.GonderenHesapId.Value);
                
                string? detayAciklama = i.Aciklama;
                
                // Generate more readable description
                if (gidenPara)
                {
                    // We sent it
                    if (i.AliciHesap?.Musteri != null)
                        detayAciklama = $"Sent to {i.AliciHesap.Musteri.Ad} {i.AliciHesap.Musteri.Soyad} ({i.AliciHesap.HesapNo})";
                    else
                        detayAciklama = "ATM Withdrawal"; // If no receiver, it's ATM
                }
                else
                {
                    // We received it
                    if (i.GonderenHesap?.Musteri != null)
                        detayAciklama = $"Received from {i.GonderenHesap.Musteri.Ad} {i.GonderenHesap.Musteri.Soyad} ({i.GonderenHesap.HesapNo})";
                    else
                        detayAciklama = "ATM Deposit"; // If no sender, it's ATM
                }

                // Add original description if exists
                if (!string.IsNullOrEmpty(i.Aciklama) && i.Aciklama != "Para Transferi")
                    detayAciklama += $" ({i.Aciklama})";

                return new 
                {
                    IslemId = i.IslemId,
                    // Ensuring UTC timezone
                    Tarih = i.IslemTarihi.HasValue ? DateTime.SpecifyKind(i.IslemTarihi.Value, DateTimeKind.Utc) : (DateTime?)null,
                    Miktar = i.Miktar,
                    Aciklama = detayAciklama,
                    GonderenId = i.GonderenHesapId,
                    AliciId = i.AliciHesapId
                };
            });

            return Ok(islemler);
        }

        /// <summary>
        /// Returns transaction counts for last 7 days (for charts)
        /// </summary>
        [HttpGet("GunlukIslemHacmi")]
        public async Task<ActionResult<IEnumerable<object>>> GetGunlukIslemHacmi()
        {
            var baslangicTarihi = DateTime.UtcNow.AddDays(-7);

            var veriler = await _context.IslemHareketleris
                .Where(x => x.IslemTarihi.HasValue && x.IslemTarihi >= baslangicTarihi)
                .GroupBy(x => x.IslemTarihi!.Value.Date)
                .Select(g => new
                {
                    Tarih = g.Key,
                    IslemSayisi = g.Count()
                })
                .OrderBy(x => x.Tarih)
                .ToListAsync();

            return Ok(veriler);
        }
    }
}
