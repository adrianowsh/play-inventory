using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers;

public sealed class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
{
    private readonly IRepository<CatalogItem> _catalogItemRepository;
    public CatalogItemUpdatedConsumer(IRepository<CatalogItem> catalogItemRepository)
    {
        _catalogItemRepository = catalogItemRepository;
    }
    public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
    {
        var message = context.Message;
        var catalogItemExisting = await _catalogItemRepository.GetAsync(
            c => c.CatalogItemId == message.ItemId);

        if (catalogItemExisting is null)
        {
            var catalogItem = CatalogItem.NewCatalogItem(
                 catalogItemId: message.ItemId,
                 message.Name,
                 message.Description
             );
            await _catalogItemRepository.CreateAsync(catalogItem);
        }
        else
        {
            catalogItemExisting.SetName(message.Name);
            catalogItemExisting.SetDescription(message.Description);
            await _catalogItemRepository.UpdateAsync(catalogItemExisting);
        }
    }
}
