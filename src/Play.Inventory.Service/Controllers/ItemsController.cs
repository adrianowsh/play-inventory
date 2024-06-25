using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Dtos;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Controllers;

[ApiController]
[Route("api/items")]
public class ItemsController : ControllerBase
{
    private readonly IRepository<InventoryItem> _inventoryItemRepository;
    private readonly IRepository<CatalogItem> _catalogItemRepository;

    private readonly CatalogClient _catalogClient;
    public ItemsController(IRepository<InventoryItem> inventoryItemRepository,
                               CatalogClient catalogClient,
                               IRepository<CatalogItem> catalogItemRepository)
    {
        _inventoryItemRepository = inventoryItemRepository;
        _catalogClient = catalogClient;
        _catalogItemRepository = catalogItemRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(Guid userId)
    {
        var inventoryItemsByUserId = await _inventoryItemRepository.GetAllAsync(item => item.UserId == userId);
        if (inventoryItemsByUserId is null) return NotFound();
        var catalogItemsIds = inventoryItemsByUserId.Select(c => c.CatalogItemId);
        var catalogItemsEntities = await _catalogItemRepository.GetAllAsync(item => catalogItemsIds.Contains(item.CatalogItemId));

        var inventoryItemDto = inventoryItemsByUserId.Select(inventoryItem =>
        {
            var catalogItem = catalogItemsEntities.Single(catalogItem => catalogItem.CatalogItemId == inventoryItem.CatalogItemId);
            return inventoryItem.AsDto(catalogItem.Name, catalogItem.Description);
        });
        return Ok(inventoryItemDto);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(GrantItemsDto grantItemsDto)
    {
        var inventoryItem = await _inventoryItemRepository.GetAsync(
            item => item.UserId == grantItemsDto.UserId &&
            grantItemsDto.CatalogItemId == grantItemsDto.CatalogItemId
        );
        if (inventoryItem is null)
        {
            var newInventoryItem = InventoryItem.NewInventoryItem(
                grantItemsDto.UserId,
                grantItemsDto.CatalogItemId,
                grantItemsDto.Quantity,
                acquiredDate: DateTimeOffset.UtcNow
            );
            await _inventoryItemRepository.CreateAsync(newInventoryItem);
            return NoContent();
        }
        inventoryItem.SetQuantity(grantItemsDto.Quantity);
        await _inventoryItemRepository.UpdateAsync(inventoryItem);
        return NoContent();
    }
}
