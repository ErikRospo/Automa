using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Overlay : MonoBehaviour
{
    // Stat UI class
    [System.Serializable]
    public class StatElement
    {
        public Stat.Type stat;
        public string name;
        public TextMeshProUGUI text;
    }
    public List<StatElement> debugElements;

    // Debug overlay
    public bool debugValues = false;
    private bool debugValuesActive = false;

    // Player pointer 
    private Player player = null;
    
    // Setup overlay
    public void Start()
    {
        // Subscribe to player spawn event
        Events.active.onPlayerSpawned += SetPlayerInstance;
    }

    // Sets the player instance
    public void SetPlayerInstance(Player player)
    {
        this.player = player;
    }

    // Update method
    public void Update()
    {
        if (player == null) return;

        // Displays values to screen
        if (debugValues)
        {
            if (!debugValuesActive) ToggleDebugValues();
            foreach (StatElement stat in debugElements)
            {
                Stat holder = player.GetStat(stat.stat);
                stat.text.text = "<b>" + stat.name + ":</b> " + Mathf.Round(holder.current) + " / " + Mathf.Round(holder.max);
            }
        }
        else if (debugValuesActive) ToggleDebugValues();
    }

    // Toggle debug values
    public void ToggleDebugValues()
    {
        // Check if debug values enabled
        debugValuesActive = !debugValuesActive;
        foreach (StatElement stat in debugElements)
            stat.text.gameObject.SetActive(debugValuesActive);
    }
}
