namespace Play.Inventory.Service.Dtos;

public sealed record CatalogItemDto(
    Guid Id,
    string Name,
    string Description
);
