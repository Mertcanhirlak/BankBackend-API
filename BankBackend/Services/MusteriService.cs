using BankBackend.Data;
using BankBackend.Interfaces;
using BankBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace BankBackend.Services
{
    public class MusteriService : IMusteriService
    {
        private readonly BankDbContext _context;
        private readonly ICacheService _cacheService;

        public MusteriService(BankDbContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<Musteriler>> TumMusterileriGetirAsync()
        {
            string cacheKey = "tum_musteriler";
            
            // 1. Önce Cache'e bak
            var cachedData = await _cacheService.GetAsync<IEnumerable<Musteriler>>(cacheKey);
            if (cachedData != null)
            {
                return cachedData;
            }

            // 2. Cache boşsa veritabanından çek
            var data = await _context.Musterilers.ToListAsync();

            // 3. Veriyi Cache'e yaz (örneğin 10 dakika süreyle)
            await _cacheService.SetAsync(cacheKey, data, TimeSpan.FromMinutes(10));

            return data;
        }

        public async Task<Musteriler?> IdIleMusteriGetirAsync(int id)
        {
            return await _context.Musterilers.FindAsync(id);
        }

        public async Task<IEnumerable<Musteriler>> AramaYapAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return await _context.Musterilers.ToListAsync();
            }

            term = term.Trim().ToLower();
            bool isNumeric = int.TryParse(term, out int searchId);

            var query = _context.Musterilers.AsQueryable();

            if (isNumeric)
            {
                if (term.Length < 3)
                {
                    query = query.Where(m => m.MusteriId == searchId);
                }
                else
                {
                    query = query.Where(m => m.MusteriId == searchId || m.TcKimlikNo.StartsWith(term));
                }
            }
            else
            {
                query = query.Where(m => m.Ad.ToLower().Contains(term) ||
                                         m.Soyad.ToLower().Contains(term) ||
                                         m.TcKimlikNo.Contains(term));
            }

            return await query.ToListAsync();
        }

        public async Task<object?> IstatistikGetirAsync()
        {
            // Call PostgreSQL Stored Procedure
            var istatistik = await _context.GenelIstatistikler
                                           .FromSqlRaw("SELECT * FROM sp_GenelIstatistik()")
                                           .FirstOrDefaultAsync();

            if (istatistik == null) return null;

            return new
            {
                ToplamMusteri = istatistik.toplam_musteri,
                ToplamPara = istatistik.toplam_para,
                ToplamIslem = istatistik.toplam_islem,
                ToplamRisk = istatistik.toplam_risk
            };
        }

        public async Task<bool> RolGuncelleAsync(int id, string yeniRol)
        {
            var musteri = await _context.Musterilers.FindAsync(id);
            if (musteri == null) return false;

            musteri.Rol = yeniRol?.ToUpper();
            await _context.SaveChangesAsync();
            await _cacheService.RemoveAsync("tum_musteriler"); // Clear Cache
            return true;
        }

        public async Task<Musteriler> EkleAsync(Musteriler musteri)
        {
            _context.Musterilers.Add(musteri);
            await _context.SaveChangesAsync();
            await _cacheService.RemoveAsync("tum_musteriler"); // Clear Cache
            return musteri;
        }

        public async Task<bool> GuncelleAsync(int id, Musteriler musteri)
        {
            if (id != musteri.MusteriId) return false;

            _context.Entry(musteri).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await _cacheService.RemoveAsync("tum_musteriler"); // Clear Cache
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MusterilerExists(id)) return false;
                throw;
            }
        }

        public async Task<bool> SilAsync(int id)
        {
            var musteri = await _context.Musterilers.FindAsync(id);
            if (musteri == null) return false;

            _context.Musterilers.Remove(musteri);
            await _context.SaveChangesAsync();
            await _cacheService.RemoveAsync("tum_musteriler"); // Clear Cache
            return true;
        }

        private bool MusterilerExists(int id)
        {
            return _context.Musterilers.Any(e => e.MusteriId == id);
        }
    }
}
