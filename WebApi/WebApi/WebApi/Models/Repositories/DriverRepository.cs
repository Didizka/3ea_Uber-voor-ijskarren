using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models.Orders.Repo;

namespace WebApi.Models.Repositories
{
    public class DriverRepository: IDriverRepository
    {
        private readonly OrderContext orderContext;
        private readonly UserContext userContext;

        public DriverRepository(OrderContext orderContext, UserContext userContext)
        {
            this.orderContext = orderContext;
            this.userContext = userContext;
        }
        public async void UpdateFlavoursPrice(string email, FlavourFrountend[] flavours)
        {
            foreach (var item in flavours)
            {
               // var flavour = await orderContext.Flavours.SingleOrDefaultAsync(f => f.Name == item.Name);
                //orderItemFlavour.Add(new OrderItemFlavour { FlavourID = flavour.FlavourID, OrderItem = orderItem, Amount = item.Amount });
            }
        }

    }
}
