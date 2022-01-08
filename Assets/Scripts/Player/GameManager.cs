using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Game manager handles other managers
// Use this script to pick & choose what a scene needs

public class GameManager : MonoBehaviour
{
    public static GameManager active;

    [SerializeField]
    protected List<Item> startingItems;

    public void Awake()
    {
        active = this;
        Application.targetFrameRate = 1000;
        Scriptables.GenerateAllScriptables();
    }

    public List<Item> FetchStartingItems()
    {
        return startingItems;
    }
}
