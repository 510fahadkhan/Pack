namespace PantryApi.Models;

public class PantryItem
{
    public string Name { get; set; } = "";
    public int Quantity { get; set; }
    public DateTime ExpiryDate { get; set; }

    public string GetStatus()
    {
        if (ExpiryDate < DateTime.Now)
            return "Expired";
        else if (ExpiryDate <= DateTime.Now.AddDays(3))
            return "Expiring Soon";
        else
            return "Fresh";
    }
}
