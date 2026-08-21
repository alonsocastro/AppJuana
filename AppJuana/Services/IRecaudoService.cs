using AppJuana.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppJuana.Services
{
    public interface IRecaudoService
    {
        Task<List<Recaudo>> GetRecaudosAsync(System.DateTime fechaInicial, System.DateTime fechaFinal, string token);
    }
}
