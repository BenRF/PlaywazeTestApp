using NorthwindModels;
using Raven.Client.Documents.Indexes;
using System.Linq;

namespace PlaywazeTestApp {
    class OrdersByDate: AbstractIndexCreationTask<Order> {
        public OrdersByDate() {
            Map = (orders) =>
                from order in orders
                select new {
                    OrderedAt = order.OrderedAt
                };
        }
    }
}
