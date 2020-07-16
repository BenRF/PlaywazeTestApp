using NorthwindModels;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaywazeTestApp {
    class Orders_ByEmployee : AbstractIndexCreationTask<Order, Orders_ByEmployee.Result> {
        public class Result {
            public string Employee { get; set; }
            public int Count { get; set; }
        }

        public Orders_ByEmployee() {
            Map = orders =>
                from order in orders
                select new {
                    Employee = order.Employee,
                    Count = 1
                };

            Reduce = results =>
                from result in results
                group result by result.Employee into e
                select new {
                    Employee = e.Key,
                    Count = e.Sum(x => x.Count)
                };
        }
    }
}
