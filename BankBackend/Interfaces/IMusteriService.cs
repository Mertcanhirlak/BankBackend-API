using BankBackend.Models;

namespace BankBackend.Interfaces
{
    public interface IMusteriService
    {
        Task<IEnumerable<Musteriler>> TumMusterileriGetirAsync();
        Task<Musteriler?> IdIleMusteriGetirAsync(int id);
        Task<IEnumerable<Musteriler>> AramaYapAsync(string term);
        Task<object?> IstatistikGetirAsync();
        Task<bool> RolGuncelleAsync(int id, string yeniRol);
        Task<Musteriler> EkleAsync(Musteriler musteri);
        Task<bool> GuncelleAsync(int id, Musteriler musteri);
        Task<bool> SilAsync(int id);
    }
}
