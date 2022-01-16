[System.Serializable]
public class Recipe
{
    public string name;
    public RecipeItem[] input;
    public RecipeItem[] output;
    public float time;
    public bool enabled;
}
