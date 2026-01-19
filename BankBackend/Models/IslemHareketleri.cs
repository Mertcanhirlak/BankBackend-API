using System;
using System.Collections.Generic;

namespace BankBackend.Models;

public partial class IslemHareketleri
{
    public int IslemId { get; set; }

    public int? GonderenHesapId { get; set; }

    public int? AliciHesapId { get; set; }

    public decimal Miktar { get; set; }

    public string? Aciklama { get; set; }

    public DateTime? IslemTarihi { get; set; }

    public virtual Hesaplar? AliciHesap { get; set; }

    public virtual Hesaplar? GonderenHesap { get; set; }
}
