using System;
using System.Collections.Generic;

namespace BankBackend.Models;

public partial class Musteriler
{
    public int MusteriId { get; set; }

    public string Ad { get; set; } = null!;

    public string Soyad { get; set; } = null!;

    public string TcKimlikNo { get; set; } = null!;

    public string Sifre { get; set; } = null!;

    public string? Rol { get; set; }

    public DateTime? KayitTarihi { get; set; }

    public virtual ICollection<Hesaplar> Hesaplars { get; set; } = new List<Hesaplar>();
}
