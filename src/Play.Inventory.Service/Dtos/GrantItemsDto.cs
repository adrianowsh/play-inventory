namespace Play.Inventory.Service.Dtos;
public sealed record GrantItemsDto(
    Guid UserId, Guid CatalogItemId, int Quantity
);
