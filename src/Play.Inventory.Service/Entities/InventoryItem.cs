using Play.Common;

namespace Play.Inventory.Service.Entities;

public sealed class InventoryItem : Entity
{
    private InventoryItem(Guid userId, Guid catalogItemId, int quantity, DateTimeOffset acquiredDate)
    {
        UserId = userId;
        CatalogItemId = catalogItemId;
        Quantity = quantity;
        AcquiredDate = acquiredDate;
    }

    private InventoryItem()
    {
    }
    public Guid UserId { get; private set; }

    public Guid CatalogItemId { get; private set; }

    public int Quantity { get; private set; }

    public DateTimeOffset AcquiredDate { get; private set; }

    public static InventoryItem NewInventoryItem(Guid userId, Guid catalogItemId, int quantity, DateTimeOffset acquiredDate)
    {
        return new InventoryItem(userId, catalogItemId, quantity, acquiredDate);
    }

    public void SetQuantity(int quantity)
    {
        Quantity = quantity;
    }
}
