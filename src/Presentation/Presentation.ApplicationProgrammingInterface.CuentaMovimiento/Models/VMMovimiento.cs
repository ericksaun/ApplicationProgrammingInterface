using Domain.AppProgrammingInt.Models;

namespace Presentation.ApplicationProgrammingInterface.CuentaMovimiento.Models
{
    public class VMMovimiento
    {
        public int? MoIdMovimientos { get; set; }

        public int? MoIdCuenta { get; set; }

        public DateTime? MoFecha { get; set; }

        public string? MoTipoMovimiento { get; set; } = null!;

        public double? MoValor { get; set; }

        public double? MoSaldo { get; set; }

        public virtual VMApCuenta? MoIdCuentaNavigation { get; set; } = null!;
    }
}
