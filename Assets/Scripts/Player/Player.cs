using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature
{
    // Default stats
    public float health, shield, stamina, oxygen, temperature, radiation, hunger, thirst;

    // Start method
    public void Start()
    {
        // Create stats
        SetupStats(health, shield, stamina, oxygen, temperature, radiation, hunger, thirst);
    }
}
