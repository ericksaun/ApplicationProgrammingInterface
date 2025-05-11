using System;
using System.Collections.Generic;

namespace Domain.AppProgrammingInt.Models;

public partial class ApCuenta
{
    public int CuIdCuenta { get; set; }

    public int CuIdCliente { get; set; }

    public string CuNumeroCuenta { get; set; } = null!;

    public string CuTipoCuenta { get; set; } = null!;

    public double CuSaldoInicial { get; set; }

    public bool CuEstado { get; set; }

    public virtual ICollection<ApMovimiento> ApMovimientos { get; set; } = new List<ApMovimiento>();

    public virtual ApCliente CuIdClienteNavigation { get; set; } = null!;
}
