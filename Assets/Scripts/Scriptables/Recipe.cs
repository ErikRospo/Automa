[System.Serializable]
public class Recipe
{
    public string name;
    public MachineData madeIn;
    public RecipeItem[] input;
    public RecipeItem[] output;
    public float time;
    public bool enabled;
}
