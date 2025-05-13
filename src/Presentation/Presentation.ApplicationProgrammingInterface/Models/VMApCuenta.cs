namespace Presentation.ApplicationProgrammingInterface.PersonaCliente.Models
{
    public class VMApCuenta
    {
        public int? CuIdCuenta { get; set; }

        public int? CuIdCliente { get; set; }

        public string? CuNumeroCuenta { get; set; } = null!;

        public string? CuTipoCuenta { get; set; } = null!;

        public double? CuSaldoInicial { get; set; }

        public bool? CuEstado { get; set; }
    }
}
