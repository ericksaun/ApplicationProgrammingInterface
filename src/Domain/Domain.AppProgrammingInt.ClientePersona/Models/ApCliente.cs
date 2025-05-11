using System;
using System.Collections.Generic;

namespace Domain.AppProgrammingInt.Models;

public partial class ApCliente
{
    public int ClIdCliente { get; set; }

    public int ClIdPersona { get; set; }

    public string ClContraseña { get; set; } = null!;

    public bool ClEstado { get; set; }

    public virtual ICollection<ApCuenta> ApCuenta { get; set; } = new List<ApCuenta>();

    public virtual ApPersona ClIdPersonaNavigation { get; set; } = null!;
}
