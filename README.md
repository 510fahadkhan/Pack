# Personal Digital Pantry

## 1️⃣ **Entity Models**

```csharp
// Models/User.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DigitalPantry.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
        
        [Required]
        public string PasswordHash { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? LastLoginAt { get; set; }
        
        public bool IsActive { get; set; }
        
        public string PasswordResetToken { get; set; }
        
        public DateTime? PasswordResetTokenExpiry { get; set; }
        
        // Navigation properties
        public virtual ICollection<PantryItem> PantryItems { get; set; }
        public virtual ICollection<ShoppingListItem> ShoppingListItems { get; set; }
        public virtual NotificationPreferences NotificationPreferences { get; set; }
    }
}
```

```csharp
// Models/PantryItem.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalPantry.Models
{
    public class PantryItem
    {
        [Key]
        public int PantryItemId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string ItemName { get; set; }
        
        [Required]
        public decimal Quantity { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string Unit { get; set; } // kg, g, L, ml, pieces, etc.
        
        [Required]
        public DateTime PurchaseDate { get; set; }
        
        [Required]
        public DateTime ExpirationDate { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Category { get; set; } // dairy, vegetables, meat, pantry, etc.
        
        [Required]
        [MaxLength(20)]
        public string StorageLocation { get; set; } // fridge, freezer, cupboard
        
        [MaxLength(500)]
        public string Notes { get; set; }
        
        public string Barcode { get; set; }
        
        public string PhotoPath { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        public bool IsDeleted { get; set; }
        
        // Foreign key
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        
        // Computed property
        [NotMapped]
        public int DaysUntilExpiry => (ExpirationDate - DateTime.Today).Days;
        
        [NotMapped]
        public string ExpiryStatus
        {
            get
            {
                if (DaysUntilExpiry < 0)
                    return "Expired";
                else if (DaysUntilExpiry <= 2)
                    return "Expiring Soon";
                else if (DaysUntilExpiry <= 7)
                    return "Near Expiry";
                else
                    return "Fresh";
            }
        }
        
        [NotMapped]
        public string ExpiryColorCode
        {
            get
            {
                if (DaysUntilExpiry < 0)
                    return "#FF0000"; // Red - Expired
                else if (DaysUntilExpiry <= 2)
                    return "#FFA500"; // Orange - Expiring soon
                else if (DaysUntilExpiry <= 7)
                    return "#FFFF00"; // Yellow - Near expiry
                else
                    return "#00FF00"; // Green - Fresh
            }
        }
    }
}
```

```csharp
// Models/ShoppingListItem.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalPantry.Models
{
    public class ShoppingListItem
    {
        [Key]
        public int ShoppingListItemId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string ItemName { get; set; }
        
        [Required]
        public decimal Quantity { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string Unit { get; set; }
        
        [MaxLength(50)]
        public string Category { get; set; }
        
        public bool IsPurchased { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? PurchasedAt { get; set; }
        
        // Foreign key
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        
        // Optional: Link to pantry item when marked as purchased
        public int? PurchasedPantryItemId { get; set; }
    }
}
```

```csharp
// Models/NotificationPreferences.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalPantry.Models
{
    public class NotificationPreferences
    {
        [Key]
        public int NotificationPreferencesId { get; set; }
        
        public bool EnableEmailNotifications { get; set; } = true;
        
        public bool EnablePushNotifications { get; set; } = false;
        
        public int NotifyDaysBeforeExpiry { get; set; } = 3;
        
        public bool NotifyForExpiredItems { get; set; } = true;
        
        public bool DailyDigest { get; set; } = false;
        
        [MaxLength(10)]
        public string PreferredNotificationTime { get; set; } = "09:00";
        
        // Foreign key
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
```

```csharp
// Models/Recipe.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalPantry.Models
{
    public class Recipe
    {
        [Key]
        public int RecipeId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string RecipeName { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        public string Instructions { get; set; }
        
        public int PreparationTimeMinutes { get; set; }
        
        public int CookingTimeMinutes { get; set; }
        
        public int Servings { get; set; }
        
        public string DifficultyLevel { get; set; } // Easy, Medium, Hard
        
        public string ImageUrl { get; set; }
        
        public bool IsPublic { get; set; }
        
        public int? UserId { get; set; } // Null for system recipes
        
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        
        // Navigation properties
        public virtual ICollection<RecipeIngredient> Ingredients { get; set; }
    }
    
    public class RecipeIngredient
    {
        [Key]
        public int RecipeIngredientId { get; set; }
        
        public int RecipeId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string IngredientName { get; set; }
        
        [Required]
        public decimal Quantity { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string Unit { get; set; }
        
        [ForeignKey("RecipeId")]
        public virtual Recipe Recipe { get; set; }
    }
}
```

```csharp
// Models/WasteReport.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalPantry.Models
{
    public class WasteReport
    {
        [Key]
        public int WasteReportId { get; set; }
        
        public int UserId { get; set; }
        
        public DateTime ReportDate { get; set; }
        
        public int TotalItemsWasted { get; set; }
        
        public decimal TotalCostWasted { get; set; }
        
        public string MostWastedCategory { get; set; }
        
        public decimal EstimatedSavings { get; set; }
        
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
    
    public class WasteReportDetail
    {
        [Key]
        public int WasteReportDetailId { get; set; }
        
        public int WasteReportId { get; set; }
        
        public string ItemName { get; set; }
        
        public string Category { get; set; }
        
        public int Quantity { get; set; }
        
        public decimal EstimatedValue { get; set; }
        
        public DateTime ExpirationDate { get; set; }
        
        [ForeignKey("WasteReportId")]
        public virtual WasteReport WasteReport { get; set; }
    }
}
```

## 2️⃣ **DTOs (Data Transfer Objects)**

```csharp
// DTOs/PantryItemDTO.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace DigitalPantry.DTOs
{
    public class PantryItemCreateDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string ItemName { get; set; }
        
        [Required]
        [Range(0.01, 10000)]
        public decimal Quantity { get; set; }
        
        [Required]
        public string Unit { get; set; }
        
        [Required]
        public DateTime PurchaseDate { get; set; }
        
        [Required]
        [FutureDate(ErrorMessage = "Expiration date must be in the future")]
        public DateTime ExpirationDate { get; set; }
        
        [Required]
        public string Category { get; set; }
        
        [Required]
        public string StorageLocation { get; set; }
        
        [StringLength(500)]
        public string Notes { get; set; }
        
        public string Barcode { get; set; }
    }
    
    public class PantryItemUpdateDTO
    {
        public int PantryItemId { get; set; }
        
        public string ItemName { get; set; }
        
        [Range(0.01, 10000)]
        public decimal? Quantity { get; set; }
        
        public string Unit { get; set; }
        
        public DateTime? PurchaseDate { get; set; }
        
        [FutureDate]
        public DateTime? ExpirationDate { get; set; }
        
        public string Category { get; set; }
        
        public string StorageLocation { get; set; }
        
        public string Notes { get; set; }
    }
    
    public class PantryItemResponseDTO
    {
        public int PantryItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int DaysUntilExpiry { get; set; }
        public string ExpiryStatus { get; set; }
        public string ExpiryColorCode { get; set; }
        public string Category { get; set; }
        public string StorageLocation { get; set; }
        public string Notes { get; set; }
    }
    
    // Custom validation attribute
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;
                
            DateTime date = (DateTime)value;
            return date > DateTime.Now;
        }
    }
}
```

## 3️⃣ **Service Interfaces**

```csharp
// Services/IPantryService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using DigitalPantry.DTOs;
using DigitalPantry.Models;

namespace DigitalPantry.Services
{
    public interface IPantryService
    {
        // FR5: Add food items
        Task<PantryItem> AddItemAsync(int userId, PantryItemCreateDTO itemDto);
        
        // FR6: Edit or delete existing pantry items
        Task<PantryItem> UpdateItemAsync(int userId, PantryItemUpdateDTO itemDto);
        Task<bool> DeleteItemAsync(int userId, int itemId);
        Task<bool> SoftDeleteItemAsync(int userId, int itemId);
        
        // FR7: View all items
        Task<IEnumerable<PantryItemResponseDTO>> GetAllItemsAsync(int userId);
        
        // FR8: Search for specific items
        Task<IEnumerable<PantryItemResponseDTO>> SearchItemsAsync(int userId, string searchTerm);
        
        // FR10: Calculate days until expiration
        Task<IEnumerable<PantryItemResponseDTO>> GetItemsByExpiryStatusAsync(int userId, string status);
        
        // FR11: Color-coded indicators (handled in DTO)
        
        // FR12: Get items near expiry for notifications
        Task<IEnumerable<PantryItem>> GetItemsNearExpiryAsync(int userId, int daysThreshold);
        
        // FR17: Get items for recipe suggestions
        Task<IEnumerable<PantryItem>> GetSoonToExpireItemsAsync(int userId, int daysThreshold);
    }
}
```

```csharp
// Services/INotificationService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using DigitalPantry.Models;

namespace DigitalPantry.Services
{
    public interface INotificationService
    {
        // FR12: Send notifications for items nearing expiration
        Task SendExpirationNotificationsAsync();
        
        // FR13: User notification preferences
        Task<NotificationPreferences> GetUserPreferencesAsync(int userId);
        Task UpdateUserPreferencesAsync(int userId, NotificationPreferences preferences);
        
        // Send specific notification types
        Task SendEmailNotificationAsync(string email, string subject, string body);
        Task SendPushNotificationAsync(int userId, string title, string message);
        
        // Get pending notifications for a user
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId, bool unreadOnly = false);
        
        // Mark notification as read
        Task MarkNotificationAsReadAsync(int notificationId);
    }
    
    public class Notification
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public string NotificationType { get; set; } // Expiry, ShoppingList, etc.
        public int? RelatedItemId { get; set; }
    }
}
```

```csharp
// Services/IShoppingListService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using DigitalPantry.DTOs;
using DigitalPantry.Models;

namespace DigitalPantry.Services
{
    public interface IShoppingListService
    {
        // FR14: Generate shopping list from low-stock items
        Task<IEnumerable<ShoppingListItem>> GenerateFromLowStockAsync(int userId, decimal threshold = 0.2m);
        
        // FR15: Manually add items to shopping list
        Task<ShoppingListItem> AddItemAsync(int userId, ShoppingListItemDTO itemDto);
        
        // FR16: Mark items as purchased
        Task<ShoppingListItem> MarkAsPurchasedAsync(int userId, int shoppingListItemId);
        
        // Get all shopping list items
        Task<IEnumerable<ShoppingListItem>> GetShoppingListAsync(int userId, bool includePurchased = false);
        
        // Remove item from shopping list
        Task<bool> RemoveItemAsync(int userId, int shoppingListItemId);
        
        // Clear purchased items
        Task<int> ClearPurchasedItemsAsync(int userId);
    }
    
    public class ShoppingListItemDTO
    {
        public string ItemName { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public string Category { get; set; }
    }
}
```

```csharp
// Services/IRecipeService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using DigitalPantry.Models;

namespace DigitalPantry.Services
{
    public interface IRecipeService
    {
        // FR17: Suggest recipes based on soon-to-expire items
        Task<IEnumerable<Recipe>> GetRecipeSuggestionsAsync(int userId, int maxRecipes = 5);
        
        // FR18: Use items in recipe and update pantry
        Task<bool> UseRecipeAsync(int userId, int recipeId, decimal servingsMultiplier = 1);
        
        // Search recipes
        Task<IEnumerable<Recipe>> SearchRecipesAsync(string searchTerm, string[] ingredients = null);
        
        // Get recipe details
        Task<Recipe> GetRecipeDetailsAsync(int recipeId);
        
        // Add custom recipe
        Task<Recipe> AddRecipeAsync(int userId, Recipe recipe);
    }
}
```

```csharp
// Services/IReportService.cs
using System;
using System.Threading.Tasks;
using DigitalPantry.Models;

namespace DigitalPantry.Services
{
    public interface IReportService
    {
        // FR19: Generate waste reports
        Task<WasteReport> GenerateWasteReportAsync(int userId, DateTime startDate, DateTime endDate);
        
        // FR20: Calculate money saved
        Task<SavingsReport> CalculateSavingsAsync(int userId, DateTime startDate, DateTime endDate);
        
        // Get waste statistics
        Task<WasteStatistics> GetWasteStatisticsAsync(int userId);
        
        // Export data
        Task<byte[]> ExportUserDataAsync(int userId, string format); // CSV or PDF
    }
    
    public class SavingsReport
    {
        public int UserId { get; set; }
        public DateTime ReportPeriod { get; set; }
        public decimal TotalSaved { get; set; }
        public int ItemsSaved { get; set; }
        public string MostSavedCategory { get; set; }
        public decimal ProjectedAnnualSavings { get; set; }
    }
    
    public class WasteStatistics
    {
        public int TotalWastedItems { get; set; }
        public decimal TotalWastedValue { get; set; }
        public Dictionary<string, int> WastedByCategory { get; set; }
        public Dictionary<string, int> WastedByMonth { get; set; }
        public string MostWastedItem { get; set; }
    }
}
```

## 4️⃣ **Repository Interfaces**

```csharp
// Repositories/IPantryRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using DigitalPantry.Models;

namespace DigitalPantry.Repositories
{
    public interface IPantryRepository
    {
        Task<PantryItem> GetByIdAsync(int id);
        Task<IEnumerable<PantryItem>> GetUserItemsAsync(int userId);
        Task<IEnumerable<PantryItem>> GetExpiringItemsAsync(int userId, int daysThreshold);
        Task<PantryItem> AddAsync(PantryItem item);
        Task<PantryItem> UpdateAsync(PantryItem item);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<PantryItem>> SearchAsync(int userId, string searchTerm);
        Task<IEnumerable<PantryItem>> GetItemsByCategoryAsync(int userId, string category);
        Task<IEnumerable<PantryItem>> GetItemsByLocationAsync(int userId, string location);
    }
}
```

## 5️⃣ **Service Implementations**

```csharp
// Services/Implementations/PantryService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalPantry.DTOs;
using DigitalPantry.Models;
using DigitalPantry.Repositories;
using DigitalPantry.Services;
using Microsoft.Extensions.Logging;

namespace DigitalPantry.Services.Implementations
{
    public class PantryService : IPantryService
    {
        private readonly IPantryRepository _pantryRepository;
        private readonly ILogger<PantryService> _logger;
        
        public PantryService(IPantryRepository pantryRepository, ILogger<PantryService> logger)
        {
            _pantryRepository = pantryRepository;
            _logger = logger;
        }
        
        public async Task<PantryItem> AddItemAsync(int userId, PantryItemCreateDTO itemDto)
        {
            try
            {
                // FR5: Add food items
                var item = new PantryItem
                {
                    UserId = userId,
                    ItemName = itemDto.ItemName,
                    Quantity = itemDto.Quantity,
                    Unit = itemDto.Unit,
                    PurchaseDate = itemDto.PurchaseDate,
                    ExpirationDate = itemDto.ExpirationDate,
                    Category = itemDto.Category,
                    StorageLocation = itemDto.StorageLocation,
                    Notes = itemDto.Notes,
                    Barcode = itemDto.Barcode,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                
                var addedItem = await _pantryRepository.AddAsync(item);
                _logger.LogInformation($"Item added successfully for user {userId}: {item.ItemName}");
                
                return addedItem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding item for user {userId}");
                throw;
            }
        }
        
        public async Task<PantryItem> UpdateItemAsync(int userId, PantryItemUpdateDTO itemDto)
        {
            // FR6: Edit items
            var existingItem = await _pantryRepository.GetByIdAsync(itemDto.PantryItemId);
            
            if (existingItem == null || existingItem.UserId != userId)
                throw new UnauthorizedAccessException("Item not found or access denied");
            
            // Update only provided fields
            if (!string.IsNullOrEmpty(itemDto.ItemName))
                existingItem.ItemName = itemDto.ItemName;
                
            if (itemDto.Quantity.HasValue)
                existingItem.Quantity = itemDto.Quantity.Value;
                
            if (!string.IsNullOrEmpty(itemDto.Unit))
                existingItem.Unit = itemDto.Unit;
                
            if (itemDto.PurchaseDate.HasValue)
                existingItem.PurchaseDate = itemDto.PurchaseDate.Value;
                
            if (itemDto.ExpirationDate.HasValue)
                existingItem.ExpirationDate = itemDto.ExpirationDate.Value;
                
            if (!string.IsNullOrEmpty(itemDto.Category))
                existingItem.Category = itemDto.Category;
                
            if (!string.IsNullOrEmpty(itemDto.StorageLocation))
                existingItem.StorageLocation = itemDto.StorageLocation;
                
            if (itemDto.Notes != null)
                existingItem.Notes = itemDto.Notes;
            
            existingItem.UpdatedAt = DateTime.UtcNow;
            
            return await _pantryRepository.UpdateAsync(existingItem);
        }
        
        public async Task<bool> DeleteItemAsync(int userId, int itemId)
        {
            // FR6: Delete items
            var item = await _pantryRepository.GetByIdAsync(itemId);
            
            if (item == null || item.UserId != userId)
                throw new UnauthorizedAccessException("Item not found or access denied");
            
            return await _pantryRepository.DeleteAsync(itemId);
        }
        
        public async Task<bool> SoftDeleteItemAsync(int userId, int itemId)
        {
            // Soft delete (mark as deleted but keep in database)
            var item = await _pantryRepository.GetByIdAsync(itemId);
            
            if (item == null || item.UserId != userId)
                throw new UnauthorizedAccessException("Item not found or access denied");
            
            item.IsDeleted = true;
            item.UpdatedAt = DateTime.UtcNow;
            
            await _pantryRepository.UpdateAsync(item);
            return true;
        }
        
        public async Task<IEnumerable<PantryItemResponseDTO>> GetAllItemsAsync(int userId)
        {
            // FR7: View all items
            var items = await _pantryRepository.GetUserItemsAsync(userId);
            
            return items.Select(item => new PantryItemResponseDTO
            {
                PantryItemId = item.PantryItemId,
                ItemName = item.ItemName,
                Quantity = item.Quantity,
                Unit = item.Unit,
                PurchaseDate = item.PurchaseDate,
                ExpirationDate = item.ExpirationDate,
                DaysUntilExpiry = item.DaysUntilExpiry,
                ExpiryStatus = item.ExpiryStatus,
                ExpiryColorCode = item.ExpiryColorCode,
                Category = item.Category,
                StorageLocation = item.StorageLocation,
                Notes = item.Notes
            });
        }
        
        public async Task<IEnumerable<PantryItemResponseDTO>> SearchItemsAsync(int userId, string searchTerm)
        {
            // FR8: Search for specific items
            var items = await _pantryRepository.SearchAsync(userId, searchTerm);
            
            return items.Select(item => new PantryItemResponseDTO
            {
                PantryItemId = item.PantryItemId,
                ItemName = item.ItemName,
                Quantity = item.Quantity,
                Unit = item.Unit,
                PurchaseDate = item.PurchaseDate,
                ExpirationDate = item.ExpirationDate,
                DaysUntilExpiry = item.DaysUntilExpiry,
                ExpiryStatus = item.ExpiryStatus,
                ExpiryColorCode = item.ExpiryColorCode,
                Category = item.Category,
                StorageLocation = item.StorageLocation,
                Notes = item.Notes
            });
        }
        
        public async Task<IEnumerable<PantryItemResponseDTO>> GetItemsByExpiryStatusAsync(int userId, string status)
        {
            var allItems = await GetAllItemsAsync(userId);
            
            return status?.ToLower() switch
            {
                "expired" => allItems.Where(i => i.ExpiryStatus == "Expired"),
                "expiringsoon" => allItems.Where(i => i.ExpiryStatus == "Expiring Soon"),
                "nearexpiry" => allItems.Where(i => i.ExpiryStatus == "Near Expiry"),
                "fresh" => allItems.Where(i => i.ExpiryStatus == "Fresh"),
                _ => allItems
            };
        }
        
        public async Task<IEnumerable<PantryItem>> GetItemsNearExpiryAsync(int userId, int daysThreshold)
        {
            // FR12: Get items for notifications
            return await _pantryRepository.GetExpiringItemsAsync(userId, daysThreshold);
        }
        
        public async Task<IEnumerable<PantryItem>> GetSoonToExpireItemsAsync(int userId, int daysThreshold)
        {
            // FR17: Get items for recipe suggestions
            return await _pantryRepository.GetExpiringItemsAsync(userId, daysThreshold);
        }
    }
}
```

## 6️⃣ **Controller Examples**

```csharp
// Controllers/PantryController.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigitalPantry.DTOs;
using DigitalPantry.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DigitalPantry.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PantryController : ControllerBase
    {
        private readonly IPantryService _pantryService;
        private readonly ILogger<PantryController> _logger;
        
        public PantryController(IPantryService pantryService, ILogger<PantryController> logger)
        {
            _pantryService = pantryService;
            _logger = logger;
        }
        
        // GET: api/pantry
        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {
                var userId = GetCurrentUserId();
                var items = await _pantryService.GetAllItemsAsync(userId);
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pantry items");
                return StatusCode(500, "An error occurred while retrieving pantry items");
            }
        }
        
        // GET: api/pantry/search?term=milk
        [HttpGet("search")]
        public async Task<IActionResult> SearchItems([FromQuery] string term)
        {
            try
            {
                var userId = GetCurrentUserId();
                var items = await _pantryService.SearchItemsAsync(userId, term);
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching pantry items");
                return StatusCode(500, "An error occurred while searching");
            }
        }
        
        // GET: api/pantry/expiring?days=7
        [HttpGet("expiring")]
        public async Task<IActionResult> GetExpiringItems([FromQuery] int days = 7)
        {
            try
            {
                var userId = GetCurrentUserId();
                var items = await _pantryService.GetSoonToExpireItemsAsync(userId, days);
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting expiring items");
                return StatusCode(500, "An error occurred");
            }
        }
        
        // POST: api/pantry
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] PantryItemCreateDTO itemDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                var userId = GetCurrentUserId();
                var item = await _pantryService.AddItemAsync(userId, itemDto);
                
                return CreatedAtAction(nameof(GetAllItems), new { id = item.PantryItemId }, item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding pantry item");
                return StatusCode(500, "An error occurred while adding the item");
            }
        }
        
        // PUT: api/pantry/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] PantryItemUpdateDTO itemDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                itemDto.PantryItemId = id;
                var userId = GetCurrentUserId();
                var item = await _pantryService.UpdateItemAsync(userId, itemDto);
                
                return Ok(item);
            }
            catch (UnauthorizedAccessException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating pantry item");
                return StatusCode(500, "An error occurred while updating the item");
            }
        }
        
        // DELETE: api/pantry/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _pantryService.DeleteItemAsync(userId, id);
                
                if (result)
                    return NoContent();
                
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting pantry item");
                return StatusCode(500, "An error occurred while deleting the item");
            }
        }
        
        // GET: api/pantry/status/{status}
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetItemsByStatus(string status)
        {
            try
            {
                var userId = GetCurrentUserId();
                var items = await _pantryService.GetItemsByExpiryStatusAsync(userId, status);
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting items by status");
                return StatusCode(500, "An error occurred");
            }
        }
        
        private int GetCurrentUserId()
        {
            // Get user ID from claims
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException();
                
            return int.Parse(userIdClaim);
        }
    }
}
```

## 7️⃣ **Configuration Classes**

```csharp
// Configuration/DatabaseSettings.cs
namespace DigitalPantry.Configuration
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public bool EnableSensitiveDataLogging { get; set; }
        public int CommandTimeout { get; set; } = 30;
        public int MaxRetryCount { get; set; } = 3;
        public int MaxRetryDelay { get; set; } = 5;
    }
}
```

```csharp
// Configuration/NotificationSettings.cs
namespace DigitalPantry.Configuration
{
    public class NotificationSettings
    {
        public bool EnableEmailNotifications { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public bool EnablePushNotifications { get; set; }
        public string FirebaseServerKey { get; set; }
        public string FirebaseSenderId { get; set; }
        public int DefaultNotificationDays { get; set; } = 3;
    }
}
```

## 8️⃣ **Program.cs/Startup Configuration**

```csharp
// Program.cs
using DigitalPantry.Configuration;
using DigitalPantry.Repositories;
using DigitalPantry.Services;
using DigitalPantry.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure settings
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("Database"));
builder.Services.Configure<NotificationSettings>(
    builder.Configuration.GetSection("Notification"));

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorNumbersToAdd: null);
        }));

// Add Authentication (NFR13)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Register repositories
builder.Services.AddScoped<IPantryRepository, PantryRepository>();
// Add other repositories...

// Register services
builder.Services.AddScoped<IPantryService, PantryService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IShoppingListService, ShoppingListService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IReportService, ReportService>();

// Add caching
builder.Services.AddMemoryCache();

// Add background services for notifications (FR12)
builder.Services.AddHostedService<ExpirationNotificationService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Add response caching
builder.Services.AddResponseCaching();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts(); // NFR13: Security headers
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseResponseCaching();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();
```

## 9️⃣ **Unit Test Example**

```csharp
// Tests/PantryServiceTests.cs
using System;
using System.Threading.Tasks;
using DigitalPantry.DTOs;
using DigitalPantry.Models;
using DigitalPantry.Repositories;
using DigitalPantry.Services.Implementations;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DigitalPantry.Tests.Services
{
    public class PantryServiceTests
    {
        private readonly Mock<IPantryRepository> _mockRepo;
        private readonly Mock<ILogger<PantryService>> _mockLogger;
        private readonly PantryService _service;
        
        public PantryServiceTests()
        {
            _mockRepo = new Mock<IPantryRepository>();
            _mockLogger = new Mock<ILogger<PantryService>>();
            _service = new PantryService(_mockRepo.Object, _mockLogger.Object);
        }
        
        [Fact]
        public async Task AddItemAsync_ValidItem_ReturnsAddedItem()
        {
            // Arrange
            var userId = 1;
            var itemDto = new PantryItemCreateDTO
            {
                ItemName = "Milk",
                Quantity = 2,
                Unit = "L",
                PurchaseDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(7),
                Category = "Dairy",
                StorageLocation = "Fridge"
            };
            
            var expectedItem = new PantryItem
            {
