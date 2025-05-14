namespace Domain.AppProgrammingInt.Models
{
    public class VMApMovimiento
    {
        public string? MoTipoMovimiento { get; set; } = null!;

        public double? MoValor { get; set; }

        public double? MoSaldo { get; set; }

        public virtual VMApCuenta? MoIdCuentaNavigation { get; set; } = null!;
    }
}
