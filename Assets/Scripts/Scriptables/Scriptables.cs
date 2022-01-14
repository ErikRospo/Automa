using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Loads scriptables at runtime which can then be accesed from anywhere.

public static class Scriptables
{
    // Resource paths
    public static string BuildingPath = "Scriptables/Buildings";
    public static string ItemPath = "Scriptables/Items";
    public static string DepositPath = "Scriptables/Deposits";
    public static string RecipePath = "Scriptables/Recipes";
    public static string BiomePath = "Scriptables/Biomes";
    public static string VoicelinePath = "Scriptables/Voicelines";

    // Scriptable dictionaries
    public static Dictionary<string, BuildingData> buildingDict;
    public static Dictionary<string, ItemData> itemDict;
    public static Dictionary<string, DepositData> depositDict;
    public static Dictionary<string, Recipe> recipeDict;
    public static Dictionary<string, BiomeData> biomeDict;

    // Scriptable lists
    public static List<BuildingData> buildings;
    public static List<ItemData> items;
    public static List<DepositData> deposits;
    public static List<Recipe> recipes;
    public static List<BiomeData> biomes;

    // Generate scriptables
    public static void GenerateAllScriptables()
    {
        GenerateBuildings();
        GenerateItems();
        GenerateDeposits();
        GenerateBiomes();
        GenerateVoicelines();
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
    public static void GenerateDeposits()
    {
        depositDict = new Dictionary<string, DepositData>();
        deposits = new List<DepositData>();

        List<DepositData> loaded = Resources.LoadAll(DepositPath, typeof(DepositData)).Cast<DepositData>().ToList();
        Debug.Log("Loaded " + loaded.Count + " deposits from " + DepositPath);

        foreach (DepositData mineral in loaded)
        {
            depositDict.Add(mineral.InternalID, mineral);
            deposits.Add(mineral);
            Debug.Log("Loaded " + mineral.name + " with UUID " + mineral.InternalID);
        }
    }

    // Generate biomes on startup
    public static void GenerateBiomes()
    {
        biomeDict = new Dictionary<string, BiomeData>();
        biomes = new List<BiomeData>();

        List<BiomeData> loaded = Resources.LoadAll(BiomePath, typeof(BiomeData)).Cast<BiomeData>().ToList();
        Debug.Log("Loaded " + loaded.Count + " biomes from " + BiomePath);

        foreach (BiomeData biome in loaded)
        {
            biomeDict.Add(biome.InternalID, biome);
            biomes.Add(biome);
            Debug.Log("Loaded " + biome.name + " with UUID " + biome.InternalID);
        }
    }

    // Generate voicelines on startup
    public static void GenerateVoicelines()
    {
        Voicelines.lines = new Dictionary<string, Voiceline>();

        List<Voiceline> loaded = Resources.LoadAll(VoicelinePath, typeof(Voiceline)).Cast<Voiceline>().ToList();
        Debug.Log("Loaded " + loaded.Count + " voicelines from " + VoicelinePath);

        foreach (Voiceline voiceline in loaded)
        {
            Voicelines.lines.Add(voiceline.identifier, voiceline);
            Debug.Log("Loaded " + voiceline.name + " with identifier " + voiceline.identifier);
        }
    }
}
