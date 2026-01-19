namespace BankBackend.DTOs
{
    public class TransferDto
    {
        public int GonderenId { get; set; }
        public int AliciId { get; set; }
        public decimal Miktar { get; set; }
        public string? Aciklama { get; set; }
    }

    public class TransferSonucDto
    {
        public bool Basarili { get; set; }
        public string Mesaj { get; set; } = string.Empty;
        public decimal? YeniBakiye { get; set; }
    }
}