using System;
using System.Collections.Generic;

namespace Domain.AppProgrammingInt.Models;

public partial class ApPersona
{
    public int PsIdPersona { get; set; }

    public string PsNombre { get; set; } = null!;

    public string PsGenero { get; set; } = null!;

    public int PsEdad { get; set; }

    public string PsIdentificacion { get; set; } = null!;

    public string PsDireccion { get; set; } = null!;

    public string PsTelefono { get; set; } = null!;

    public virtual ApCliente? ApCliente { get; set; }
}
