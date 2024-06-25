using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers;

public sealed class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
{
    private readonly IRepository<CatalogItem> _catalogItemRepository;
    public CatalogItemDeletedConsumer(IRepository<CatalogItem> catalogItemRepository)
    {
        _catalogItemRepository = catalogItemRepository;
    }
    public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
    {
        var message = context.Message;
        var catalogItemExisting = await _catalogItemRepository.GetAsync(message.ItemId);
        if (catalogItemExisting is null) return;
        await _catalogItemRepository.RemoveAsync(catalogItemExisting.Id);
    }
}
