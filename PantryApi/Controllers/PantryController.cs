using Microsoft.AspNetCore.Mvc;
using PantryApi.Models;
using PantryApi.Services;

namespace PantryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PantryController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        var items = FileHandler.Load();
        var result = items.Select(i => new {
            i.Name,
            i.Quantity,
            ExpiryDate = i.ExpiryDate.ToString("yyyy-MM-dd"),
            Status = i.GetStatus()
        });
        return Ok(result);
    }

    [HttpPost]
    public IActionResult AddItem([FromBody] PantryItem item)
    {
        if (string.IsNullOrWhiteSpace(item.Name))
            return BadRequest("Name is required.");

        var items = FileHandler.Load();
        items.Add(item);
        FileHandler.Save(items);
        return Ok(new { message = $"{item.Name} added successfully." });
    }

    [HttpDelete("{name}")]
    public IActionResult RemoveItem(string name)
    {
        var items = FileHandler.Load();
        var item = items.FirstOrDefault(i => i.Name.ToLower() == name.ToLower());
        if (item == null)
            return NotFound(new { message = $"{name} not found." });

        items.Remove(item);
        FileHandler.Save(items);
        return Ok(new { message = $"{name} removed successfully." });
    }

    [HttpGet("expired")]
    public IActionResult GetExpired()
    {
        var items = FileHandler.Load().Where(i => i.ExpiryDate < DateTime.Now);
        return Ok(items.Select(i => new { i.Name, i.Quantity, ExpiryDate = i.ExpiryDate.ToString("yyyy-MM-dd"), Status = "Expired" }));
    }

    [HttpGet("expiring-soon")]
    public IActionResult GetExpiringSoon()
    {
        var items = FileHandler.Load().Where(i => i.ExpiryDate >= DateTime.Now && i.ExpiryDate <= DateTime.Now.AddDays(3));
        return Ok(items.Select(i => new { i.Name, i.Quantity, ExpiryDate = i.ExpiryDate.ToString("yyyy-MM-dd"), Status = "Expiring Soon" }));
    }
}
