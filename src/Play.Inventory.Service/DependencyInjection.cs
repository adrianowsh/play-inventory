using MongoDB.Driver;
using Play.Common;
using Play.Common.MongoDb;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IRepository<InventoryItem>>(static serviceProvider =>
        {
            var database = serviceProvider.GetService<IMongoDatabase>();
            return new MongoRepository<InventoryItem>(database);
        });
        return services;
    }


    public static IServiceCollection AddHttpCatalogClient(this IServiceCollection services)
    {
        services.AddHttpClient<CatalogClient>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:5001");
        });
        return services;
    }
}
