using Play.Inventory.Service.Dtos;

namespace Play.Inventory.Service.Clients;
public sealed class CatalogClient
{
    private readonly HttpClient _httpClient;

    public CatalogClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyCollection<CatalogItemDto>> GetCatalogItemsAsymc()
    {
        var items = await _httpClient.GetFromJsonAsync<IReadOnlyCollection<CatalogItemDto>>("/api/items");
        return items;
    }
}
