using BankBackend.DTOs;
using System.Threading.Tasks;

namespace BankBackend.Interfaces
{
    public interface IAiAdvisoryService
    {
        Task<AiAdviceResponseDto> AnalyzeFinancialStatusAsync(int musteriId, string language = "tr");
    }
}