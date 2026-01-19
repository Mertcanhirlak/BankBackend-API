using BankBackend.Data;
using BankBackend.Interfaces;
using BankBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BankBackend.Services
{
    public class HesapService : IHesapService
    {
        private readonly BankDbContext _context;

        public HesapService(BankDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Hesaplar>> TumHesaplariGetirAsync()
        {
            return await _context.Hesaplars.Include(h => h.Musteri).ToListAsync();
        }

        public async Task<Hesaplar?> IdIleHesapGetirAsync(int id)
        {
            return await _context.Hesaplars.Include(h => h.Musteri).FirstOrDefaultAsync(h => h.HesapId == id);
        }

        public async Task<IEnumerable<Hesaplar>> MusteriHesaplariniGetirAsync(int musteriId)
        {
            return await _context.Hesaplars
                                 .Include(h => h.Musteri)
                                 .Where(h => h.MusteriId == musteriId)
                                 .OrderBy(h => h.HesapId)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Hesaplar>> MusteriVadesizHesaplariniGetirAsync(int musteriId)
        {
            // "VADESIZ" veya null olanları getir (geriye dönük uyumluluk için null kontrolü)
            return await _context.Hesaplars
                                 .Include(h => h.Musteri)
                                 .Where(h => h.MusteriId == musteriId && (h.HesapTuru == "VADESIZ" || h.HesapTuru == null))
                                 .OrderBy(h => h.HesapId)
                                 .ToListAsync();
        }

        public async Task<Hesaplar> HesapOlusturAsync(Hesaplar hesap)
        {
            // 1. Otomatik Hesap No Üretimi (TR + 10 Hane Random)
            if (string.IsNullOrEmpty(hesap.HesapNo))
            {
                hesap.HesapNo = HesapNoUret();
            }

            // 2. Varsayılan Değerler
            if (hesap.OlusturmaTarihi == null) hesap.OlusturmaTarihi = DateTime.UtcNow;
            if (hesap.AktifMi == null) hesap.AktifMi = true;
            if (string.IsNullOrEmpty(hesap.HesapTuru)) hesap.HesapTuru = "VADESIZ";

            // 3. Ekleme
            _context.Hesaplars.Add(hesap);
            await _context.SaveChangesAsync();

            return hesap;
        }

        public async Task<bool> HesapGuncelleAsync(int id, Hesaplar hesap)
        {
            if (id != hesap.HesapId) return false;

            _context.Entry(hesap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HesapExists(id)) return false;
                throw;
            }
        }

        public async Task<bool> HesapSilAsync(int id)
        {
            var hesap = await _context.Hesaplars.FindAsync(id);
            if (hesap == null) return false;

            // İlişkili kayıtları temizle (IslemHareketleri ve RiskMasasi)
            var iliskiliIslemler = await _context.IslemHareketleris
                .Where(i => i.GonderenHesapId == id || i.AliciHesapId == id)
                .ToListAsync();

            foreach (var islem in iliskiliIslemler)
            {
                if (islem.GonderenHesapId == id) islem.GonderenHesapId = null;
                if (islem.AliciHesapId == id) islem.AliciHesapId = null;
            }

            var riskKayitlari = await _context.RiskMasasis
                .Where(r => r.HesapId == id)
                .ToListAsync();

            foreach (var risk in riskKayitlari)
            {
                risk.HesapId = null;
            }

            _context.Hesaplars.Remove(hesap);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ParaYatirAsync(int hesapId, decimal miktar)
        {
            if (miktar <= 0) return false;

            var hesap = await _context.Hesaplars.FindAsync(hesapId);
            if (hesap == null) return false;

            hesap.Bakiye = (hesap.Bakiye ?? 0) + miktar;

            var islem = new IslemHareketleri
            {
                AliciHesapId = hesapId,
                GonderenHesapId = null,
                Miktar = miktar,
                IslemTarihi = DateTime.UtcNow,
                Aciklama = "ATM Para Yatırma"
            };

            _context.IslemHareketleris.Add(islem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ParaCekAsync(int hesapId, decimal miktar)
        {
            if (miktar <= 0) return false;

            var hesap = await _context.Hesaplars.FindAsync(hesapId);
            if (hesap == null) return false;

            if (hesap.Bakiye < miktar) return false;

            hesap.Bakiye -= miktar;

            var islem = new IslemHareketleri
            {
                AliciHesapId = null,
                GonderenHesapId = hesapId,
                Miktar = miktar,
                IslemTarihi = DateTime.UtcNow,
                Aciklama = "ATM Para Çekme"
            };

            _context.IslemHareketleris.Add(islem);
            await _context.SaveChangesAsync();
            return true;
        }

        private bool HesapExists(int id)
        {
            return _context.Hesaplars.Any(e => e.HesapId == id);
        }

        private string HesapNoUret()
        {
            var random = new Random();
            var sb = new StringBuilder("TR");
            for (int i = 0; i < 10; i++)
            {
                sb.Append(random.Next(0, 10));
            }
            return sb.ToString();
        }
    }
}
