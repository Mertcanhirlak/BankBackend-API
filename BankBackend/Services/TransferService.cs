using BankBackend.Data;
using BankBackend.DTOs;
using BankBackend.Interfaces;
using BankBackend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BankBackend.Services
{
    public class TransferService : ITransferService
    {
        private readonly BankDbContext _context;

        public TransferService(BankDbContext context)
        {
            _context = context;
        }

        public async Task<TransferSonucDto> ParaTransferiYapAsync(TransferDto request)
        {
            // 1. Temel Validasyonlar
            if (request.GonderenId == request.AliciId)
                return new TransferSonucDto { Basarili = false, Mesaj = "Kendinize transfer için 'Virman' kullanın veya farklı hesap seçin." };

            if (request.Miktar <= 0)
                return new TransferSonucDto { Basarili = false, Mesaj = "Transfer miktarı 0'dan büyük olmalıdır." };

            // TRANSACTION (Atomik İşlem)
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var gonderenHesap = await _context.Hesaplars.FindAsync(request.GonderenId);
                var aliciHesap = await _context.Hesaplars.FindAsync(request.AliciId);

                if (gonderenHesap == null) return new TransferSonucDto { Basarili = false, Mesaj = "Gönderen hesap bulunamadı." };
                if (aliciHesap == null) return new TransferSonucDto { Basarili = false, Mesaj = "Alıcı hesap bulunamadı." };

                if (gonderenHesap.Bakiye < request.Miktar)
                    return new TransferSonucDto { Basarili = false, Mesaj = "Yetersiz bakiye." };

                // Para Aktarımı
                gonderenHesap.Bakiye -= request.Miktar;
                aliciHesap.Bakiye += request.Miktar;

                // İşlem Logu
                var islem = new IslemHareketleri
                {
                    GonderenHesapId = request.GonderenId,
                    AliciHesapId = request.AliciId,
                    Miktar = request.Miktar,
                    IslemTarihi = DateTime.UtcNow,
                    Aciklama = string.IsNullOrEmpty(request.Aciklama) ? "Para Transferi" : request.Aciklama
                };

                _context.IslemHareketleris.Add(islem);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new TransferSonucDto 
                { 
                    Basarili = true, 
                    Mesaj = "Transfer başarıyla gerçekleşti.", 
                    YeniBakiye = gonderenHesap.Bakiye 
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Gerçek hayatta burada loglama yapılır (örneğin Serilog ile)
                return new TransferSonucDto { Basarili = false, Mesaj = $"Bir hata oluştu: {ex.Message}" };
            }
        }
    }
}