using Play.Common;

namespace Play.Inventory.Service.Entities;

public sealed class CatalogItem : Entity
{
    private CatalogItem(Guid cataloglItemId, string name, string description)
    {
        CatalogItemId = cataloglItemId;
        Name = name;
        Description = description;
    }

    private CatalogItem()
    {
    }
    public Guid CatalogItemId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public void SetName(string newName) => Name = newName;
    public void SetDescription(string newDescriptionName)
        => Description = newDescriptionName;
    public static CatalogItem NewCatalogItem(Guid catalogItemId, string name, string description)
        => new CatalogItem(catalogItemId, name, description);
}
