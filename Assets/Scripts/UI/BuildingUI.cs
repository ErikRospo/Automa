using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingUI : MonoBehaviour
{
    public TMP_Dropdown recipeSelector;

    // Start is called before the first frame update
    void Start()
    {
        UIEvents.active.addRecipe += AddRecipe;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddRecipe(Recipe recipe)
    {

    }
}
