using System;
using System.Reflection;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace PlaywazeTestApp {
    public static class DocumentStoreHolder {
        private static readonly Lazy<IDocumentStore> LazyStore =
            new Lazy<IDocumentStore>(() => {
                var store = new DocumentStore {
                    Urls = new[] { "http://localhost:8080" },
                    Database = "Northwind"
                };

                store.Initialize();
                new OrdersByDate().Execute(store);
                new CompanyByName().Execute(store);
                new Orders_ByEmployee().Execute(store);

                return store;
            });

        public static IDocumentStore Store =>
            LazyStore.Value;
    }
}