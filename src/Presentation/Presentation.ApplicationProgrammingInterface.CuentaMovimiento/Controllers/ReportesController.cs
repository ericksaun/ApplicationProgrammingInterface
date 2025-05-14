using Application.AppProgrammingInt.Services;
using Domain.AppProgrammingInt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.ApplicationProgrammingInterface.CuentaMovimiento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly IApMovimientoServices _apMovimientoServices;
        private readonly IApCuentaServices _apCuentaServices;
        private readonly ILogger<ReportesController> _logger;
        public ReportesController(IApMovimientoServices apMovimientoServices, IApCuentaServices apCuentaServices, ILogger<ReportesController> logger)
        {
            apMovimientoServices = apMovimientoServices ?? throw new ArgumentNullException(nameof(apMovimientoServices));
            _apCuentaServices = apCuentaServices ?? throw new ArgumentNullException(nameof(apCuentaServices));
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> ReporteMovimientos([FromQuery] DateTime fecha, [FromQuery] string NombrePersona)
        {
            _logger.LogInformation("Inicio: Generar reporte de movimientos para {NombrePersona} en la fecha {Fecha}.", NombrePersona, fecha);

            try
            {
                _logger.LogDebug("Buscando persona por nombre: {NombrePersona}", NombrePersona);
                ApPersona apPersona = await _apCuentaServices.GetPersonaClientebyNameAsync(NombrePersona);

                if (apPersona == null || apPersona.ApCliente == null)
                {
                    _logger.LogWarning("No se encontró la persona o el cliente asociado: {NombrePersona}", NombrePersona);
                    return NotFound("Persona o cliente no encontrado.");
                }

                _logger.LogDebug("Obteniendo cuentas del cliente con ID: {IdCliente}", apPersona.ApCliente.ClIdCliente);
                IEnumerable<ApCuenta> apCuentas = await _apCuentaServices.GetCuentasIdClienteComplete(apPersona.ApCliente.ClIdCliente);

                if (!apCuentas.Any())
                {
                    _logger.LogWarning("No se encontraron cuentas para el cliente: {NombrePersona}", NombrePersona);
                    return NotFound("No se encontraron cuentas para el cliente.");
                }

                _logger.LogDebug("Filtrando movimientos por fecha: {Fecha}", fecha);
                var apMovimientosDate = apCuentas
                    .SelectMany(cuenta => cuenta.ApMovimientos
                        .Where(mov => mov.MoFecha.Date == fecha.Date)
                        .Select(mov => new { Cuenta = cuenta, Movimiento = mov }))
                    .ToList();

                if (!apMovimientosDate.Any())
                {
                    _logger.LogWarning("No se encontraron movimientos para la fecha {Fecha} y cliente {NombrePersona}.", fecha, NombrePersona);
                    return NotFound("No se encontraron movimientos para la fecha indicada.");
                }

                _logger.LogDebug("Construyendo reporte de movimientos.");
                var reportes = apMovimientosDate.Select(x => new
                {
                    Fecha = fecha,
                    Cliente = apPersona.PsNombre,
                    NumeroCuenta = x.Cuenta.CuNumeroCuenta,
                    Tipo = x.Cuenta.CuTipoCuenta,
                    Estado = x.Cuenta.CuEstado,
                    Movimiento = x.Movimiento.MoTipoMovimiento.ToUpperInvariant() == "RETIRO" ? -Math.Abs(x.Movimiento.MoValor) : x.Movimiento.MoValor,
                    SaldoDisponible = x.Movimiento.MoSaldo
                }).ToList();

                _logger.LogInformation("Reporte generado correctamente. Total de movimientos: {Cantidad}", reportes.Count);
                return Ok(reportes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los movimientos para {NombrePersona} en la fecha {Fecha}.", NombrePersona, fecha);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener los movimientos: {ex.Message}");
            }
        }


    }
}
