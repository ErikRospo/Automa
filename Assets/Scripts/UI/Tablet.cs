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

    // Table enabled variable
    public static bool active;

    // Tablet tooltip system
    public GameObject tooltipRect;
    public TextMeshProUGUI tooltipDesc;
    public RecipeTooltip recipeTooltip;

    public void Start()
    {
        // Link building click event to load info
        Events.active.onBuildingClicked += LoadInfo;
        Events.active.onSetRecipe += SetRecipe;
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
            else
            {
                progressBar.currentPercent = 0f;
                progressBar.UpdateUI();
            }

            // Loop through input slots that are needed and set values
            for (int i = 0; i < constructor.recipe.input.Length; i++)
                inputs[i].amount.text = constructor.inputHolding[constructor.recipe.input[i].item].ToString();

            // Loop through input slots that are needed and set values
            for (int i = 0; i < constructor.recipe.output.Length; i++)
                outputs[i].amount.text = constructor.outputHolding.ToString();
        }

        // Check input
        if (Input.GetKeyDown(Keybinds.escape))
        {
            active = false;
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void SetRecipe(Recipe recipe)
    {
        if (constructor.recipe == recipe) return;
        constructor.SetRecipe(recipe);
        LoadRecipes();
    }

    public void LoadInfo(Building building)
    {
        // Open tablet
        active = true;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // Get the scriptable for the buildin
        BuildingData tile = building.data;
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
            LoadRecipes();
        }
        else recipeInfo.SetActive(false);
    }

    // Loads recipes when a building with a machine scriptable is clicked
    public void LoadRecipes()
    {
        
    }

    // Sets a recipe from a constructor
    public void SetInputOutputs(Constructor constructor)
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
