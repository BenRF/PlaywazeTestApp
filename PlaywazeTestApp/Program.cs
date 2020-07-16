using System;
using System.Linq;
using Raven.Client.Documents;
using NorthwindModels;
using Raven.Client.Documents.Session;

namespace PlaywazeTestApp {
    class Program {
        static void Main(string[] args) {
            new Program().run();
        }

        public void run() {
            while (true) {
                Console.WriteLine("Please pick an option (1-3)");
                var rawChoice = Console.ReadLine();
                int choice = Convert.ToInt32(rawChoice);
                if (choice == 0) {
                    break;
                } else if (choice == 1) {
                    this.getOrdersByDate();
                } else if (choice == 2) {
                    this.countOrdersByEmployee();
                } else if (choice == 3) {
                    this.findCompany();
                } else {
                    Console.WriteLine("Please pick one of the three options");
                }
                Console.WriteLine(" ");
            }
            Console.WriteLine("Goodbye");
        }

        void getOrdersByDate() {
            Console.WriteLine("What date would you like to check for?");
            DateTime d = Convert.ToDateTime(Console.ReadLine());
            using (var session = DocumentStoreHolder.Store.OpenSession()) {
                var orders = (
                    from order in session.Query<Order>()
                                            .Include(o => o.Company)
                                            .Include(o => o.Employee)
                    where order.OrderedAt == d
                    select order
                    ).ToList();
                if (orders.Count < 1) {
                    Console.WriteLine("No results found");
                } else {
                    foreach (var order in orders) {
                        var company = session.Load<Company>(order.Company);
                        var employee = session.Load<Employee>(order.Employee);
                        Console.WriteLine(company.Name + ": " + employee.FirstName + " " + employee.LastName + ": " + order.Freight + ": " + order.RequireAt.Date + ": " + order.ShipVia);
                    }
                }
            }
        }

        void countOrdersByEmployee() {
            Console.WriteLine("What employee number would you like to see?");
            string input = Console.ReadLine();
            int id;
            int.TryParse(input, out id);
            
            using (var session = DocumentStoreHolder.Store.OpenSession()) {
                var results = session
                    .Query<Orders_ByEmployee.Result, Orders_ByEmployee>()
                    .ToList();

                foreach (var result in results) {
                    if (result.Employee == "employees/" + id + "-A") {
                        Console.WriteLine("Employee " + id + " has " + result.Count + " orders");
                    }
                }
            }
        }

        void findCompany() {
            Console.WriteLine("What company would you like to search for?");
            string input = Console.ReadLine();
            using (IDocumentSession session = DocumentStoreHolder.Store.OpenSession()) {
                var companies = session
                    .Query<Company>()
                    .Search(x => x.Name, input)
                    .ToList();

                if (companies.Count < 1) {
                    Console.WriteLine("No results found");
                } else {
                    foreach (var company in companies) {
                        Console.WriteLine(company.Name);
                    }
                }
            }
        }
    }
}
