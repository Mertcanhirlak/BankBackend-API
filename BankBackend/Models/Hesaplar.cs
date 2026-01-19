using System;
using System.Collections.Generic;

namespace BankBackend.Models;

public partial class Hesaplar
{
    public int HesapId { get; set; }

    public int MusteriId { get; set; }

    public string HesapNo { get; set; } = null!;

    public string? HesapTuru { get; set; }

    public decimal? Bakiye { get; set; }

    public bool? AktifMi { get; set; }

    public DateTime? OlusturmaTarihi { get; set; }

    public virtual ICollection<IslemHareketleri> IslemHareketleriAliciHesaps { get; set; } = new List<IslemHareketleri>();

    public virtual ICollection<IslemHareketleri> IslemHareketleriGonderenHesaps { get; set; } = new List<IslemHareketleri>();

    public virtual Musteriler? Musteri { get; set; }
}
