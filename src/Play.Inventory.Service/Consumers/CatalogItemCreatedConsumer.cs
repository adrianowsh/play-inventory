using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers;

public sealed class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
{
    private readonly IRepository<CatalogItem> _catalogItemRepository;

    public CatalogItemCreatedConsumer(IRepository<CatalogItem> catalogItemRepository)
    {
        _catalogItemRepository = catalogItemRepository;
    }

    public async Task Consume(ConsumeContext<CatalogItemCreated> context)
    {
        var message = context.Message;
        var item = await _catalogItemRepository.GetAsync(message.ItemId);

        if (item is null) return;

        var catalogItem = CatalogItem.NewCatalogItem(
            catalogItemId: message.ItemId,
            message.Name,
            message.Description
        );
        await _catalogItemRepository.CreateAsync(catalogItem);
    }
}
