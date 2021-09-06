using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    public new string name;
    public RecipeItem[] input;
    public RecipeItem[] output;
    public int time;
    public bool unlocked;
}
