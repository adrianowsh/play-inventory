namespace Play.Inventory.Service.Dtos;
public sealed record InventoryItemDto(
    Guid CatalogItemId,
    string Name,
    string Description,
    int Quantity,
    DateTimeOffset AcquiredDate
);