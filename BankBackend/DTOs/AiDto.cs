using System.Collections.Generic;

namespace BankBackend.DTOs
{
    // Yapay Zekaya göndereceğimiz özet veri
    public class AiAnalysisRequestDto
    {
        public int MusteriId { get; set; }
        public string? OzelSoru { get; set; } // Kullanıcı "Araba alabilir miyim?" diye sorabilir
        public string Language { get; set; } = "tr"; // Varsayılan: Türkçe
    }

    // Yapay Zekadan dönecek yapısal veri
    public class AiAdviceResponseDto
    {
        public string Baslik { get; set; } = string.Empty;
        public List<string> Tavsiyeler { get; set; } = new List<string>();
        public string GenelYorum { get; set; } = string.Empty;
    }
}