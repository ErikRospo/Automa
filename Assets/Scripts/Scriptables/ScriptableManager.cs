using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Loads scriptables at runtime which can then be accesed from anywhere.

public class ScriptableManager : MonoBehaviour
{
    // Active instance
    public static ScriptableManager active;

    // Resource paths
    public string BuildingPath = "Scriptables/Buildings";
    public string EnemyPath = "Scriptables/Enemies";
    public string ItemPath = "Scriptables/Items";
    public string RecipePath = "Scriptables/Recipes";

    // Scriptable lists
    public List<Tile> buildings = new List<Tile>();
    public List<Item> items = new List<Item>();
    public List<Recipe> recipes = new List<Recipe>();

    public void Awake()
    {
        // Set active instance
        active = this;
    }

    // Generates buildings on run
    public void GenerateBuildings()
    {
        // Load buildings
        buildings = Resources.LoadAll(BuildingPath, typeof(Tile)).Cast<Tile>().ToList();
        Debug.Log("Loaded " + buildings.Count + " buildings from " + BuildingPath);
    }

    // Generates recipes on run
    public void GenerateRecipes()
    {
        // Load recipes
        recipes = Resources.LoadAll(RecipePath, typeof(Recipe)).Cast<Recipe>().ToList();
        Debug.Log("Loaded " + recipes.Count + " recipes from " + RecipePath);
    }

    // Retrieves a building object by name
    public GameObject RequestBuildingByName(string name)
    {
        foreach (Tile building in buildings)
            if (building.name == name) return building.obj;
        Debug.Log("Could not retrieve object with name " + name);
        return null;
    }

    // Retrieves a tile scriptable by name
    public Tile RequestTileByName(string name)
    {
        foreach (Tile building in buildings)
            if (building.name == name) return building;
        Debug.Log("Could not retrieve scriptable with name " + name);
        return null;
    }
}
