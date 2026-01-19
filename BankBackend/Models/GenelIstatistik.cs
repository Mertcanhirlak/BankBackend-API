using System.ComponentModel.DataAnnotations.Schema;

namespace BankBackend.Models
{
    // Veritabanında tablosu olmayan, sadece SP sonucunu karşılayan model
    [NotMapped]
    public class GenelIstatistik
    {
        public int toplam_musteri { get; set; }
        public decimal toplam_para { get; set; }
        public int toplam_islem { get; set; }
        public int toplam_risk { get; set; }
    }
}
