@echo off
chcp 65001 > nul
echo ===================================================
echo   BANKBACKEND PROJE KURULUM SIHIRBAZI
echo ===================================================
echo.
echo [1/5] Docker konteynerleri hazirlaniyor...
docker-compose up -d --build

echo.
echo [2/5] Veritabaninin hazir olmasi bekleniyor (10 saniye)...
timeout /t 10 /nobreak > nul

echo.
echo [3/5] Veritabani tablolari ve Trigger'lar olusturuluyor...
type init_db.sql | docker exec -i bank-db psql -U postgres -d bank_db > nul
type BankBackend\db_setup.sql | docker exec -i bank-db psql -U postgres -d bank_db > nul

echo.
echo [4/5] Varsayilan kullanicilar ekleniyor...

:: Admin Ekleme
curl -X POST http://localhost:8080/api/Musterilers -H "Content-Type: application/json" -d "{\"ad\": \"System\", \"soyad\": \"Admin\", \"tcKimlikNo\": \"99999999999\", \"sifre\": \"admin123\", \"rol\": \"ADMIN\"}" > nul 2>&1

:: Musteri Ekleme
curl -X POST http://localhost:8080/api/Musterilers -H "Content-Type: application/json" -d "{\"ad\": \"Ahmet\", \"soyad\": \"Yilmaz\", \"tcKimlikNo\": \"12345678901\", \"sifre\": \"123456\", \"rol\": \"MUSTERI\"}" > nul 2>&1

echo.
echo [5/5] Kurulum Tamamlandi! Tarayici aciliyor...
start http://localhost:8080

echo.
echo ===================================================
echo   GIRIS BILGILERI:
echo   ----------------------------------------
echo   Yonetici (Admin):
echo   TC: 99999999999  |  Sifre: admin123
echo.
echo   Musteri:
echo   TC: 12345678901  |  Sifre: 123456
echo ===================================================
pause
