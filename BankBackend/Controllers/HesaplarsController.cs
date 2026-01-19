using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BankBackend.Interfaces;
using BankBackend.Models;

namespace BankBackend.Controllers
{
    /// <summary>
    /// Account Management Controller
    /// Handles account operations including deposits, withdrawals, and balance queries
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize] // NOW REQUIRES TOKEN AUTHENTICATION
    public class HesaplarsController : ControllerBase
    {
        private readonly IHesapService _hesapService;

        public HesaplarsController(IHesapService hesapService)
        {
            _hesapService = hesapService;
        }

        /// <summary>
        /// Lists all accounts with customer details
        /// </summary>
        /// <returns>Account list with customer information</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hesaplar>>> GetHesaplars()
        {
            var hesaplar = await _hesapService.TumHesaplariGetirAsync();
            return Ok(hesaplar);
        }

        /// <summary>
        /// Get account by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Hesaplar>> GetHesaplar(int id)
        {
            var hesap = await _hesapService.IdIleHesapGetirAsync(id);

            if (hesap == null)
            {
                return NotFound();
            }

            return hesap;
        }

        /// <summary>
        /// Get all accounts for a specific customer
        /// </summary>
        [HttpGet("Musteri/{musteriId}")]
        public async Task<ActionResult<IEnumerable<Hesaplar>>> GetHesaplarByMusteri(int musteriId)
        {
            var musteriHesaplari = await _hesapService.MusteriHesaplariniGetirAsync(musteriId);

            if (musteriHesaplari == null || !musteriHesaplari.Any())
            {
                // Returning empty list is safer, won't cause client errors
                return Ok(new List<Hesaplar>());
            }

            return Ok(musteriHesaplari);
        }

        /// <summary>
        /// Get demand deposit accounts for a specific customer
        /// </summary>
        [HttpGet("Musteri/{musteriId}/Vadesiz")]
        public async Task<ActionResult<IEnumerable<Hesaplar>>> GetVadesizHesaplarByMusteri(int musteriId)
        {
            var vadesizHesaplar = await _hesapService.MusteriVadesizHesaplariniGetirAsync(musteriId);
            return Ok(vadesizHesaplar);
        }

        /// <summary>
        /// Update account information
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHesaplar(int id, Hesaplar hesaplar)
        {
            if (id != hesaplar.HesapId)
            {
                return BadRequest();
            }

            try
            {
                var sonuc = await _hesapService.HesapGuncelleAsync(id, hesaplar);
                if (!sonuc) return NotFound();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Create new account
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Hesaplar>> PostHesaplar(Hesaplar hesaplar)
        {
            try
            {
                var yeniHesap = await _hesapService.HesapOlusturAsync(hesaplar);
                return CreatedAtAction("GetHesaplar", new { id = yeniHesap.HesapId }, yeniHesap);
            }
            catch (Exception ex)
            {
                return Problem($"Error Details: {ex.Message} | Inner Error: {ex.InnerException?.Message}");
            }
        }

        /// <summary>
        /// Deposit money to account
        /// </summary>
        [HttpPost("ParaYatir")]
        public async Task<IActionResult> ParaYatir(int hesapId, decimal miktar)
        {
            var sonuc = await _hesapService.ParaYatirAsync(hesapId, miktar);
            if (!sonuc) return BadRequest("Deposit failed. Account not found or invalid amount.");

            // Fetch account again to return updated balance (optional)
            var hesap = await _hesapService.IdIleHesapGetirAsync(hesapId);
            return Ok(new { Mesaj = "Deposit successful.", YeniBakiye = hesap?.Bakiye });
        }

        /// <summary>
        /// Withdraw money from account
        /// </summary>
        [HttpPost("ParaCek")]
        public async Task<IActionResult> ParaCek(int hesapId, decimal miktar)
        {
            var sonuc = await _hesapService.ParaCekAsync(hesapId, miktar);
            if (!sonuc) return BadRequest("Withdrawal failed. Insufficient balance or account not found.");

            var hesap = await _hesapService.IdIleHesapGetirAsync(hesapId);
            return Ok(new { Mesaj = "Withdrawal successful.", YeniBakiye = hesap?.Bakiye });
        }

        /// <summary>
        /// Delete account by ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHesaplar(int id)
        {
            var sonuc = await _hesapService.HesapSilAsync(id);
            if (!sonuc) return NotFound();

            return NoContent();
        }
    }
}
