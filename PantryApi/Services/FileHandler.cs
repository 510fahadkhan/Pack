using System.Text.Json;
using PantryApi.Models;

namespace PantryApi.Services;

public static class FileHandler
{
    private static readonly string FilePath = "pantry.json";

    public static void Save(List<PantryItem> items)
    {
        string json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    public static List<PantryItem> Load()
    {
        if (!File.Exists(FilePath))
            return new List<PantryItem>();

        string json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<PantryItem>>(json) ?? new List<PantryItem>();
    }
}
