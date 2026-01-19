using System;
using System.Collections.Generic;

namespace BankBackend.Models;

public partial class RiskMasasi
{
    public int RiskId { get; set; }

    public int? HesapId { get; set; }

    public string? SupheliOlay { get; set; }

    public DateTime? OlayTarihi { get; set; }
}
