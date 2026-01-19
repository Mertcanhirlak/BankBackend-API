-- Örnek Müşteriler
INSERT INTO musteriler (ad, soyad, tc_kimlik_no, sifre, rol, kayit_tarihi)
VALUES 
('Admin', 'User', '11111111111', '12345', 'ADMIN', NOW()),
('Ahmet', 'Yilmaz', '22222222222', '12345', 'MUSTERI', NOW()),
('Ayse', 'Demir', '33333333333', '12345', 'MUSTERI', NOW());

-- Örnek Hesaplar (ID'leri varsayilan sirayla aliyoruz: 1, 2, 3)
-- Admin Hesabi
INSERT INTO hesaplar (musteri_id, hesap_no, hesap_turu, bakiye, aktif_mi, olusturma_tarihi)
VALUES (1, 'TR0001', 'VADESIZ', 100000.00, true, NOW());

-- Ahmet'in Hesabi
INSERT INTO hesaplar (musteri_id, hesap_no, hesap_turu, bakiye, aktif_mi, olusturma_tarihi)
VALUES (2, 'TR0002', 'VADESIZ', 5000.00, true, NOW());

-- Ayse'nin Hesabi
INSERT INTO hesaplar (musteri_id, hesap_no, hesap_turu, bakiye, aktif_mi, olusturma_tarihi)
VALUES (3, 'TR0003', 'VADESIZ', 12500.50, true, NOW());

-- Örnek İşlem (Ahmet Ayşe'ye para göndermiş olsun)
INSERT INTO islem_hareketleri (gonderen_hesap_id, alici_hesap_id, miktar, aciklama, islem_tarihi)
VALUES (2, 3, 500.00, 'Kira odemesi', NOW());
