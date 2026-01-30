using BankBackend.DTOs;
using BankBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BankBackend.Controllers
{
    /// <summary>
    /// AI Advisory Controller - Provides AI-powered financial advice
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AiController : ControllerBase
    {
        private readonly IAiAdvisoryService _aiService;

        public AiController(IAiAdvisoryService aiService)
        {
            _aiService = aiService;
        }

        /// <summary>
        /// Get AI financial advice for a customer
        /// </summary>
        [HttpPost("analyze")]
        public async Task<IActionResult> GetFinancialAdvice([FromBody] AiAnalysisRequestDto request)
        {
            // İsteği yaparken dil bilgisini de gönderiyoruz
            var sonuc = await _aiService.AnalyzeFinancialStatusAsync(request.MusteriId, request.Language);
            return Ok(sonuc);
        }

        /// <summary>
        /// Chat with the AI Financial Advisor
        /// </summary>
        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] AiChatRequestDto request)
        {
            var response = await _aiService.ChatWithAdvisorAsync(request.MusteriId, request.UserMessage, request.Language);
            return Ok(new { Response = response });
        }
    }
}