using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Loads scriptables at runtime which can then be accesed from anywhere.

public static class Scriptables
{
    // Resource paths
    public static string BuildingPath = "Scriptables/Buildings";
    public static string ItemPath = "Scriptables/Items";
    public static string MineralPath = "Scriptables/Minerals";
    public static string RecipePath = "Scriptables/Recipes";
    public static string BiomePath = "Scriptables/Biomes";

    // Scriptable dictionaries
    public static Dictionary<string, BuildingData> buildingDict;
    public static Dictionary<string, ItemData> itemDict;
    public static Dictionary<string, MineralData> mineralDict;
    public static Dictionary<string, Recipe> recipeDict;
    public static Dictionary<string, Biome> biomeDict;

    // Scriptable lists
    public static List<BuildingData> buildings;
    public static List<ItemData> items;
    public static List<MineralData> minerals;
    public static List<Recipe> recipes;
    public static List<Biome> biomes;

    // Generate scriptables
    public static void GenerateAllScriptables()
    {
        GenerateBuildings();
        GenerateItems();
        GenerateMinerals();
        GenerateBiomes();
    }

    // Generate buildings on startup
    public static void GenerateBuildings()
    {
        buildingDict = new Dictionary<string, BuildingData>();
        buildings = new List<BuildingData>();

        List<BuildingData> loaded = Resources.LoadAll(BuildingPath, typeof(BuildingData)).Cast<BuildingData>().ToList();
        Debug.Log("Loaded " + loaded.Count + " buildings from " + BuildingPath);

        foreach (BuildingData building in loaded)
        {
            buildingDict.Add(building.InternalID, building);
            buildings.Add(building);
            Debug.Log("Loaded " + building.name + " with UUID " + building.InternalID);
        }
    }

    // Generate items on startup
    public static void GenerateItems()
    {
        itemDict = new Dictionary<string, ItemData>();
        items = new List<ItemData>();

        List<ItemData> loaded = Resources.LoadAll(ItemPath, typeof(ItemData)).Cast<ItemData>().ToList();
        Debug.Log("Loaded " + loaded.Count + " items from " + ItemPath);

        foreach (ItemData item in loaded)
        {
            itemDict.Add(item.InternalID, item);
            items.Add(item);
            Debug.Log("Loaded " + item.name + " with UUID " + item.InternalID);
        }
    }

    // Generate minerals on startup
    public static void GenerateMinerals()
    {
        mineralDict = new Dictionary<string, MineralData>();
        minerals = new List<MineralData>();

        List<MineralData> loaded = Resources.LoadAll(MineralPath, typeof(MineralData)).Cast<MineralData>().ToList();
        Debug.Log("Loaded " + loaded.Count + " minerals from " + MineralPath);

        foreach (MineralData mineral in loaded)
        {
            mineralDict.Add(mineral.InternalID, mineral);
            minerals.Add(mineral);
            Debug.Log("Loaded " + mineral.name + " with UUID " + mineral.InternalID);
        }
    }

    // Generate biomes on startup
    public static void GenerateBiomes()
    {
        biomeDict = new Dictionary<string, Biome>();
        biomes = new List<Biome>();

        List<Biome> loaded = Resources.LoadAll(BiomePath, typeof(Biome)).Cast<Biome>().ToList();
        Debug.Log("Loaded " + loaded.Count + " biomes from " + BiomePath);

        foreach (Biome biome in loaded)
        {
            biomeDict.Add(biome.InternalID, biome);
            biomes.Add(biome);
            Debug.Log("Loaded " + biome.name + " with UUID " + biome.InternalID);
        }
    }
}
