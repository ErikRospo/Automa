using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Game manager handles other managers
// Use this script to pick & choose what a scene needs

public class GameManager : MonoBehaviour
{
    public void Start()
    {
        ScriptableManager.active.GenerateBuildings();
        ScriptableManager.active.GenerateRecipes();
    }
}