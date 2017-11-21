using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models.Orders;

namespace WebApi.Models.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Flavour>> GetFlavoursAsync();
    }
}