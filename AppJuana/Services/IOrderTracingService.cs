using AppJuana.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppJuana.Services
{
    public interface IOrderTracingService
    {
        Task<List<OrderTracing>> GetOrderTracingDataAsync(string filter, string token);
    }
}