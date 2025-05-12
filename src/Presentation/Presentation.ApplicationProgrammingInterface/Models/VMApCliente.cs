using Domain.AppProgrammingInt.Models;

namespace Presentation.ApplicationProgrammingInterface.Models
{
    public class VMApCliente
    {
        public int? ClIdCliente { get; set; }

        public int? ClIdPersona { get; set; }

        public string? ClContraseña { get; set; } = null!;

        public bool? ClEstado { get; set; }

     
    }
}
