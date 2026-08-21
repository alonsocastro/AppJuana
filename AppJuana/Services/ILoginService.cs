using AppJuana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppJuana.Services
{
    public interface ILoginService
    {
        // Tarea asíncrona que recibe credenciales y devuelve una respuesta
        Task<LoginResponse> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken = default);
    }
}
