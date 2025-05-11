using System;
using System.Collections.Generic;

namespace Domain.AppProgrammingInt.Models;

public partial class ApMovimiento
{
    public int MoIdMovimientos { get; set; }

    public int MoIdCuenta { get; set; }

    public DateTime MoFecha { get; set; }

    public string MoTipoMovimiento { get; set; } = null!;

    public double MoValor { get; set; }

    public double MoSaldo { get; set; }

    public virtual ApCuenta MoIdCuentaNavigation { get; set; } = null!;
}
