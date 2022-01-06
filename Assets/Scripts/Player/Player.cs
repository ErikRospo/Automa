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

        // Player spawn event
        Events.active.PlayerSpawned(this);
    }
            
    // Change environment 
    public override void ChangeEnvironment(EnvironmentData environment)
    {
        // Check if previous environment had oxygen
        // if (this.environment.isOxygenated && !environment.isOxygenated)


        // Assign new environment
        this.environment = environment;
    }

    // Stats method
    public override void CheckStat(Stat.Type stat)
    {
        switch (stat)
        {
            case Stat.Type.Health:
                if (GetStat(stat).IsAtMin()) Kill();
                break;

            case Stat.Type.Oxygen:
                if (GetStat(Stat.Type.Oxygen).IsAtMin())
                    Modify(Stat.Type.Health, Time.deltaTime * 5f);
                break;

            default:
                break;
        }
    }

    // TEMP UPDATE METHOD
    public void Update()
    {
        // Check authority
        if (!hasAuthority) return;

        // Check environment 
        if (!environment.isOxygenated)
            Modify(Stat.Type.Oxygen, -Time.deltaTime);
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
}
