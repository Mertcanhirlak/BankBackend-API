using BankBackend.DTOs;
using System.Threading.Tasks;

namespace BankBackend.Interfaces
{
    public interface ITransferService
    {
        Task<TransferSonucDto> ParaTransferiYapAsync(TransferDto request);
    }
}