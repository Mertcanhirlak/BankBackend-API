using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankBackend.Interfaces;
using BankBackend.Models;

namespace BankBackend.Controllers
{
    /// <summary>
    /// Customer Management Controller - Handles CRUD operations for customers
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MusterilersController : ControllerBase
    {
        private readonly IMusteriService _musteriService;

        public MusterilersController(IMusteriService musteriService)
        {
            _musteriService = musteriService;
        }

        /// <summary>
        /// Get all customers
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Musteriler>>> GetMusterilers()
        {
            return Ok(await _musteriService.TumMusterileriGetirAsync());
        }

        /// <summary>
        /// Get customer by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Musteriler>> GetMusteriler(int id)
        {
            var musteri = await _musteriService.IdIleMusteriGetirAsync(id);

            if (musteri == null)
            {
                return NotFound();
            }

            return musteri;
        }

        /// <summary>
        /// Search customers by name, surname, or ID
        /// </summary>
        [HttpGet("Ara")]
        public async Task<ActionResult<IEnumerable<Musteriler>>> Ara([FromQuery] string term)
        {
            return Ok(await _musteriService.AramaYapAsync(term));
        }

        /// <summary>
        /// Get system statistics (total customers, balance, transactions, risks)
        /// </summary>
        [HttpGet("Istatistik")]
        public async Task<ActionResult<object>> GetIstatistik()
        {
            var istatistik = await _musteriService.IstatistikGetirAsync();

            if (istatistik == null) return NotFound();

            return Ok(istatistik);
        }

        /// <summary>
        /// Update customer role (ADMIN, PATRON, MUSTERI)
        /// </summary>
        [HttpPut("{id}/Rol")]
        public async Task<IActionResult> UpdateRol(int id, [FromBody] string yeniRol)
        {
            var sonuc = await _musteriService.RolGuncelleAsync(id, yeniRol);
            if (!sonuc) return NotFound();
            
            return Ok(new { Mesaj = "Role updated successfully." });
        }

        /// <summary>
        /// Update customer information
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMusteriler(int id, Musteriler musteriler)
        {
            var sonuc = await _musteriService.GuncelleAsync(id, musteriler);
            
            if (!sonuc)
            {
                // Simple check, detailed error handling can be added
                return NotFound(); 
            }

            return NoContent();
        }

        /// <summary>
        /// Create new customer
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Musteriler>> PostMusteriler(Musteriler musteriler)
        {
            var yeniMusteri = await _musteriService.EkleAsync(musteriler);
            return CreatedAtAction("GetMusteriler", new { id = yeniMusteri.MusteriId }, yeniMusteri);
        }

        /// <summary>
        /// Delete customer by ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMusteriler(int id)
        {
            var sonuc = await _musteriService.SilAsync(id);
            if (!sonuc) return NotFound();

            return NoContent();
        }
    }
}