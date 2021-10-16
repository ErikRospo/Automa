using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;

public class Tablet : MonoBehaviour
{
    // Input / Output slot class (for recipes)
    [System.Serializable]
    public class IOSlot
    {
        public GameObject obj;
        public ButtonManagerBasicIcon button;
        public TextMeshProUGUI amount;
    }

    // Canvas component
    public CanvasGroup canvasGroup;

    // Building variables
    [HideInInspector]
    public Constructor constructor;
    public Image buildingIcon;
    public TextMeshProUGUI buildingName;
    public TextMeshProUGUI buildingDesc;

    // Recipe variables
    public GameObject recipeInfo;
    public Transform recipeList;
    public List<IOSlot> inputs;
    public List<IOSlot> outputs;
    public ProgressBar progressBar;
    public RecipeButton recipeButton;
    private List<GameObject> recipeButtons = new List<GameObject>();

    public void Start()
    {
        // Link building click event to load info
        Events.active.onBuildingClicked += LoadInfo;
    }

    public void Update()
    {
        // Update selected building info every frame
        if (constructor != null)
        {
            // Set progress bar
            if (constructor.crafter != null)
            {
                progressBar.currentPercent = ((constructor.recipe.time - constructor.crafter.time) / constructor.recipe.time) * 100;
                progressBar.UpdateUI();
            }

            // Loop through input slots that are needed and set values
            for (int i = 0; i < constructor.recipe.input.Length; i++)
                inputs[i].amount.text = constructor.inputHolding[constructor.recipe.input[i].item].ToString();

            // Loop through input slots that are needed and set values
            for (int i = 0; i < constructor.recipe.output.Length; i++)
                outputs[i].amount.text = constructor.outputHolding.ToString();
        }
    }

    public void LoadInfo(Building building)
    {
        // Open tablet
        canvasGroup.alpha = 1f;

        // Get the scriptable for the buildin
        Tile tile = ScriptableManager.active.RequestTileByName(building.name);
        if (tile == null) 
        {
            Debug.Log("Could not retrieve info on " + building.name);
            return;
        }

        // Set building info
        buildingIcon.sprite = SpritesManager.GetSprite(tile.name);
        buildingName.text = tile.name;
        buildingDesc.text = tile.description;

        // If the building has a constructor script, load it's recipes
        Constructor constructor = building.GetComponent<Constructor>();
        if (constructor != null)
        {
            this.constructor = constructor;
            recipeInfo.SetActive(true);
            LoadRecipes(constructor.machine);
        }
        else recipeInfo.SetActive(false);
    }

    // Loads recipes when a building with a machine scriptable is clicked
    public void LoadRecipes(Machine machine)
    {
        // Recycle any previous recipes
        for (int i = 0; i < recipeButtons.Count; i++)
            Destroy(recipeButtons[i]);
        recipeButtons = new List<GameObject>();

        // Loop through new building recipes and add them
        foreach(Recipe recipe in machine.recipes)
        {
            RecipeButton button = Instantiate(recipeButton.gameObject, Vector3.zero, Quaternion.identity).GetComponent<RecipeButton>();
            button.transform.SetParent(recipeList);
            button.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            button.SetRecipe(recipe);
            recipeButtons.Add(button.gameObject);
        }

        // Check if constructor has active recipe
        SetRecipe(constructor);
    }

    // Sets a recipe from a constructor
    public void SetRecipe(Constructor constructor)
    {
        /////////////////
        // INPUT SLOTS //
        /////////////////

        // Disable all inputs
        for (int i = 0; i < inputs.Count; i++)
            inputs[i].obj.SetActive(false);

        // Loop through input slots that are needed and set values
        for (int i = 0; i < constructor.recipe.input.Length; i++)
        {
            inputs[i].obj.SetActive(true);
            inputs[i].button.buttonIcon = SpritesManager.GetSprite(constructor.recipe.input[i].item.name);
            inputs[i].amount.text = constructor.inputHolding[constructor.recipe.input[i].item].ToString();
            inputs[i].button.UpdateUI();
        }

        //////////////////
        // OUTPUT SLOTS //
        //////////////////

        // Disable all outputs
        for (int i = 0; i < outputs.Count; i++)
            outputs[i].obj.SetActive(false);

        // Loop through input slots that are needed and set values
        for (int i = 0; i < constructor.recipe.output.Length; i++)
        {
            outputs[i].obj.SetActive(true);
            outputs[i].button.buttonIcon = SpritesManager.GetSprite(constructor.recipe.output[i].item.name);
            outputs[i].amount.text = constructor.outputHolding.ToString();
            outputs[i].button.UpdateUI();
        }
    }
}