using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;

namespace Infrastructure.AppProgrammingInt.Refit.Client
{
    public interface ICuentaMovimientoApi
    {
        // Métodos de Cuentas
        [Get("/api/Cuentas")]
        Task<List<object>> GetCuentas();

        [Get("/api/Cuentas/{id}")]
        Task<object> GetCuenta(int id);

        [Post("/api/Cuentas")]
        Task<object> CreateCuenta([Body] object cuenta);

        [Put("/api/Cuentas/{id}")]
        Task<object> UpdateCuenta(int id, [Body] object modificacion);

        [Delete("/api/Cuentas/{id}")]
        Task<ApiResponse<string>> DeleteCuenta(int id);

        // Métodos de Movimientos
        [Get("/api/Movimientos/GetAllMovimientos")]
        Task<List<object>> GetAllMovimientos();

        [Get("/api/Movimientos/GetMovimientoById/{id}")]
        Task<object> GetMovimientoById(int id);

        [Post("/api/Movimientos/AddMovimiento")]
        Task<object> AddMovimiento([Body] object movimiento);

        [Put("/api/Movimientos/UpdateMovimiento/{id}")]
        Task<object> UpdateMovimiento(int id, [Body] object modificacion);

        [Delete("/api/Movimientos/DeleteMovimiento/{id}")]
        Task<ApiResponse<string>> DeleteMovimiento(int id);
    }
}
