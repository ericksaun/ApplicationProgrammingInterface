namespace Domain.AppProgrammingInt.Models
{
    public class VMApCuenta
    {
        public string? CuNumeroCuenta { get; set; } = null!;

        public string? CuTipoCuenta { get; set; } = null!;

        public double? CuSaldoInicial { get; set; }

        public bool? CuEstado { get; set; }
        public string? NombreCliente { get; set; } = null!;
    }
}
