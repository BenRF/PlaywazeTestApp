using NorthwindModels;
using Raven.Client.Documents.Indexes;
using System.Linq;

namespace PlaywazeTestApp {
    class CompanyByName: AbstractIndexCreationTask<Company> {
        public CompanyByName() {
            Map = (companies) =>
                from company in companies
                select new {
                    Name = company.Name
                };
        }
    }
}
