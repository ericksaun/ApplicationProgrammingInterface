using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.AppProgrammingInt.Services;
using Domain.AppProgrammingInt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Presentation.ApplicationProgrammingInterface.CuentaMovimiento.Controllers;
using Xunit;


public class ApClienteTests
{
    [Fact]
    public void ApCliente_AgregarCuentas_SeReflejaEnColeccion()
    {
        // Arrange
        var cliente = new ApCliente
        {
            ClIdCliente = 1,
            ClIdPersona = 2,
            ClContraseña = "85756",
            ClEstado = true,
            ApCuenta = new List<ApCuenta>()
        };

        var cuenta1 = new ApCuenta { CuIdCuenta = 10, CuIdCliente = 1, CuNumeroCuenta = "001" };
        var cuenta2 = new ApCuenta { CuIdCuenta = 11, CuIdCliente = 1, CuNumeroCuenta = "002" };

        // Act
        cliente.ApCuenta.Add(cuenta1);
        cliente.ApCuenta.Add(cuenta2);

        // Assert
        Assert.Equal(2, cliente.ApCuenta.Count);
        Assert.Contains(cliente.ApCuenta, c => c.CuNumeroCuenta == "001");
        Assert.Contains(cliente.ApCuenta, c => c.CuNumeroCuenta == "002");
        Assert.All(cliente.ApCuenta, c => Assert.Equal(cliente.ClIdCliente, c.CuIdCliente));
    }
}
