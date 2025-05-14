using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.AppProgrammingInt.Models;
using Refit;

namespace Infrastructure.AppProgrammingInt.Refit.Client
{
    public interface IPersonaClienteApi
    {
        [Get("/api/Cliente/GetAllPersonas")]
        Task<List<ApPersona>> GetAllPersonas();

        [Get("/api/Cliente/GetPersonaById")]
        Task<ApPersona> GetPersonaById([Query] int id);

        [Get("/api/Cliente/GetPersonaByIdName")]
        Task<object> GetPersonaByIdName([Query] string Nombre);

        [Post("/api/Cliente/AddPersona")]
        Task<ApPersona> AddPersona([Body] object vmpersona);

        [Put("/api/Cliente/UpdatePersona")]
        Task<ApPersona> UpdatePersona([Query] int id, [Body] object modificacion);

        [Delete("/api/Cliente/DeletePersona")]
        Task<ApiResponse<string>> DeletePersona([Query] int id);
    }
}
