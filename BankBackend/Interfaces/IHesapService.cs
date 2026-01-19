using BankBackend.Models;

namespace BankBackend.Interfaces
{
    public interface IHesapService
    {
        Task<IEnumerable<Hesaplar>> TumHesaplariGetirAsync();
        Task<Hesaplar?> IdIleHesapGetirAsync(int id);
        Task<IEnumerable<Hesaplar>> MusteriHesaplariniGetirAsync(int musteriId);
        Task<IEnumerable<Hesaplar>> MusteriVadesizHesaplariniGetirAsync(int musteriId);
        Task<Hesaplar> HesapOlusturAsync(Hesaplar hesap);
        Task<bool> HesapGuncelleAsync(int id, Hesaplar hesap);
        Task<bool> HesapSilAsync(int id);
        Task<bool> ParaYatirAsync(int hesapId, decimal miktar);
        Task<bool> ParaCekAsync(int hesapId, decimal miktar);
    }
}
