using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models.Orders;

namespace WebApi.Models.Repositories
{
    public class OrderRepository: IOrderRepository
    {
        private readonly OrderContext context;

        public OrderRepository(OrderContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Flavour>> GetFlavoursAsync()
        {
            return await context.Flavours.ToListAsync();
        }

        public async void PlaceOrder()
        {
            
        }
    }
}
