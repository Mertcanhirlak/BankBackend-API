using BankBackend.Models;

namespace BankBackend.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Musteriler musteri);
    }
}
