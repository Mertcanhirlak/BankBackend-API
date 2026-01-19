using BankBackend.Data;
using BankBackend.Models;
using BankBackend.Services;
using BankBackend.Interfaces;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace BankBackend.Tests
{
    // Fake Cache Service for Testing Purposes
    public class FakeCacheService : ICacheService
    {
        public Task<T?> GetAsync<T>(string key)
        {
            return Task.FromResult(default(T)); // Always return null (cache miss)
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? duration = null)
        {
            return Task.CompletedTask; // Do nothing
        }

        public Task RemoveAsync(string key)
        {
            return Task.CompletedTask; // Do nothing
        }
    }

    public class MusteriServiceTests
    {
        private BankDbContext GetInMemoryDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<BankDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            
            return new BankDbContext(options);
        }

        [Fact]
        public async Task TumMusterileriGetirAsync_VeriVarsa_HepsiniDonmeli()
        {
            // Arrange (Hazırlık)
            var dbName = "TestDB_Listeleme";
            using (var context = GetInMemoryDbContext(dbName))
            {
                context.Musterilers.Add(new Musteriler { MusteriId = 1, Ad = "Ali", Soyad = "Yılmaz", TcKimlikNo = "123", Sifre = "pass" });
                context.Musterilers.Add(new Musteriler { MusteriId = 2, Ad = "Ayşe", Soyad = "Demir", TcKimlikNo = "456", Sifre = "pass" });
                await context.SaveChangesAsync();
            }

            using (var context = GetInMemoryDbContext(dbName))
            {
                var service = new MusteriService(context, new FakeCacheService());

                // Act (Eylem)
                var sonuc = await service.TumMusterileriGetirAsync();

                // Assert (Doğrulama)
                Assert.NotNull(sonuc);
                Assert.Equal(2, sonuc.Count());
            }
        }

        [Fact]
        public async Task EkleAsync_YeniMusteri_BasariylaEklenmeli()
        {
            // Arrange
            var dbName = "TestDB_Ekleme";
            var yeniMusteri = new Musteriler { MusteriId = 10, Ad = "Veli", Soyad = "Kaya", TcKimlikNo = "999", Sifre = "1234" };

            using (var context = GetInMemoryDbContext(dbName))
            {
                var service = new MusteriService(context, new FakeCacheService());

                // Act
                await service.EkleAsync(yeniMusteri);
            }

            // Assert
            using (var context = GetInMemoryDbContext(dbName))
            {
                var musteri = await context.Musterilers.FirstOrDefaultAsync(m => m.MusteriId == 10);
                Assert.NotNull(musteri);
                Assert.Equal("Veli", musteri.Ad);
            }
        }

        [Fact]
        public async Task SilAsync_VarolanId_SilmeliVeTrueDonmeli()
        {
            // Arrange
            var dbName = "TestDB_Silme";
            using (var context = GetInMemoryDbContext(dbName))
            {
                context.Musterilers.Add(new Musteriler { MusteriId = 5, Ad = "Silinecek", Soyad = "Kisi", TcKimlikNo = "555", Sifre = "pass" });
                await context.SaveChangesAsync();
            }

            using (var context = GetInMemoryDbContext(dbName))
            {
                var service = new MusteriService(context, new FakeCacheService());

                // Act
                var sonuc = await service.SilAsync(5);

                // Assert
                Assert.True(sonuc);
                Assert.Null(await context.Musterilers.FindAsync(5));
            }
        }

        [Fact]
        public async Task AramaYapAsync_IsimIle_DogruSonucDonmeli()
        {
            // Arrange
            var dbName = "TestDB_Arama";
            using (var context = GetInMemoryDbContext(dbName))
            {
                context.Musterilers.Add(new Musteriler { MusteriId = 1, Ad = "Mehmet", Soyad = "Yıldız", TcKimlikNo = "111", Sifre = "pass" });
                context.Musterilers.Add(new Musteriler { MusteriId = 2, Ad = "Zeynep", Soyad = "Yıldız", TcKimlikNo = "222", Sifre = "pass" });
                await context.SaveChangesAsync();
            }

            using (var context = GetInMemoryDbContext(dbName))
            {
                var service = new MusteriService(context, new FakeCacheService());

                // Act
                var sonuc = await service.AramaYapAsync("mehmet");

                // Assert
                Assert.Single(sonuc); // Sadece 1 kişi gelmeli
                Assert.Equal("Mehmet", sonuc.First().Ad);
            }
        }
    }
}
