using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Items/Recipe")]
public class Recipe : ScriptableObject
{
    public new string name;
    public RecipeItem[] input;
    public RecipeItem[] output;
    public float time;
    public bool enabled;
}
