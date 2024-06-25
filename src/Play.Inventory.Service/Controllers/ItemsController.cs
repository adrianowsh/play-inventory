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
    private readonly CatalogClient _catalogClient;
    public ItemsController(IRepository<InventoryItem> inventoryItemRepository, CatalogClient catalogClient)
    {
        _inventoryItemRepository = inventoryItemRepository;
        _catalogClient = catalogClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(Guid userId)
    {
        var catalogItems = await _catalogClient.GetCatalogItemsAsymc();
        if (!catalogItems.Any()) return NotFound();
        var inventoryItemsByUserId = await _inventoryItemRepository.GetAllAsync(item => item.UserId == userId);
        if (inventoryItemsByUserId is null) return NotFound();
        var inventoryItemDto = inventoryItemsByUserId.Select(inventoryItem =>
        {
            var catalogItem = catalogItems.Single(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);
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
