using Microsoft.AspNetCore.Mvc;
using BankBackend.DTOs;
using BankBackend.Interfaces;
using System.Threading.Tasks;

namespace BankBackend.Controllers
{
    /// <summary>
    /// Transfer Controller - Handles money transfers between accounts
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ITransferService _transferService;

        public TransferController(ITransferService transferService)
        {
            _transferService = transferService;
        }

        /// <summary>
        /// Execute money transfer between accounts
        /// </summary>
        [HttpPost("execute")]
        public async Task<IActionResult> ExecuteTransfer([FromBody] TransferDto request)
        {
            var sonuc = await _transferService.ParaTransferiYapAsync(request);

            if (sonuc.Basarili)
            {
                return Ok(new { Message = sonuc.Mesaj, NewBalance = sonuc.YeniBakiye });
            }

            return BadRequest(new { Error = sonuc.Mesaj });
        }
    }
}