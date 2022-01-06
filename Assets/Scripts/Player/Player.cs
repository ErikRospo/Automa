using UnityEngine;

public class Player : Creature
{
    // Default stats for player
    [SerializeField]
    private float health, shield, stamina, oxygen, 
        temperature, radiation, hunger, thirst;

    // Start method
    public void Start()
    {
        // Check authority
        if (!hasAuthority) return;

        // Create stats
        SetDefaultStats(health, shield, stamina, oxygen, temperature, radiation, hunger, thirst);
    }

    // Placeholder setup
    public void SetDefaultStats(float health, float shield, float stamina, float oxygen,
        float temperature, float radiation, float hunger, float thirst)
    {
        // Create new dictionary instances
        SetStat(new Stat(Stat.Type.Health, 0, health, health));
        SetStat(new Stat(Stat.Type.Shield, 0, shield, shield));
        SetStat(new Stat(Stat.Type.Stamina, 0, stamina, stamina));
        SetStat(new Stat(Stat.Type.Oxygen, 0, oxygen, oxygen));
        SetStat(new Stat(Stat.Type.Temperature, 25f, temperature, 45f));
        SetStat(new Stat(Stat.Type.Radiation, 0, radiation, 1000f));
        SetStat(new Stat(Stat.Type.Hunger, 0, hunger, hunger));
        SetStat(new Stat(Stat.Type.Thirst, 0, thirst, thirst));
    }

    // TEMP UPDATE METHOD
    public void Update()
    {
        // Check authority
        if (!hasAuthority) return;


    }
}
