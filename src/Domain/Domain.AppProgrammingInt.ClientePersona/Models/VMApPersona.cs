namespace Domain.AppProgrammingInt.Models
{
    public class VMApPersona
    {
       
        public string? PsNombre { get; set; } = null!;

        public string? PsGenero { get; set; } = null!;

        public int? PsEdad { get; set; }

        public string? PsIdentificacion { get; set; } = null!;

        public string? PsDireccion { get; set; } = null!;

        public string? PsTelefono { get; set; } = null!;

        public virtual VMApCliente? ApCliente { get; set; }
    }
}
