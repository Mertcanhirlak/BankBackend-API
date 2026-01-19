-- STEP 1: Stored Procedure for General Statistics
CREATE OR REPLACE FUNCTION sp_GenelIstatistik()
RETURNS TABLE (
    toplam_musteri INTEGER,
    toplam_para NUMERIC,
    toplam_islem INTEGER,
    toplam_risk INTEGER
) AS $$
BEGIN
    RETURN QUERY SELECT
        (SELECT COUNT(*)::INTEGER FROM musteriler),
        (SELECT COALESCE(SUM(bakiye), 0) FROM hesaplar),
        (SELECT COUNT(*)::INTEGER FROM islem_hareketleri),
        (SELECT COUNT(*)::INTEGER FROM risk_masasi);
END;
$$ LANGUAGE plpgsql;

-- STEP 2: Trigger Function (Suspicious Transaction Check)
CREATE OR REPLACE FUNCTION fn_SupheliIslemKontrol()
RETURNS TRIGGER AS $$
BEGIN
    -- If transaction amount exceeds 50,000 TL, log to Risk Table
    IF NEW.miktar > 50000 THEN
        INSERT INTO risk_masasi (hesap_id, supheli_olay, olay_tarihi)
        VALUES (NEW.gonderen_hesap_id, 'High Amount Transfer: ' || NEW.miktar, NOW());
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- STEP 3: Bind Trigger to Table
DROP TRIGGER IF EXISTS trg_SupheliIslem ON islem_hareketleri;

CREATE TRIGGER trg_SupheliIslem
AFTER INSERT ON islem_hareketleri
FOR EACH ROW
EXECUTE FUNCTION fn_SupheliIslemKontrol();
