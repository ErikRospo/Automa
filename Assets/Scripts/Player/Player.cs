using UnityEngine;

public class Player : Creature
{
    // Audio player for player sounds
    public AudioPlayer footstepAudioPlayer;

    // Get Veer AI attached to player object
    private VeerAI veer;
    
    // Default stats for player
    [SerializeField]
    private float health, shield, stamina, oxygen, startingOxygen,
        temperature, radiation, hunger, thirst;

    // Start method
    public void Start()
    {
        // Check authority
        if (!hasAuthority) return;

        // Check if environment is null
        environment = _environment;

        // Get veer instance
        veer = GetComponent<VeerAI>();

        // Create stats
        SetDefaultStats(health, shield, stamina, startingOxygen, oxygen, temperature, radiation, hunger, thirst);

        // Player spawn event
        Events.active.PlayerSpawned(this);
    }

    // TEMP UPDATE METHOD
    public void Update()
    {
        // Check authority
        if (!hasAuthority) return;

        // Check if environment is null
        if (environment == null)
            environment = _environment;

        // Check environment 
        if (!environment.isOxygenated)
            Modify(Stat.Type.Oxygen, -Time.deltaTime);
        else if (!GetStat(Stat.Type.Oxygen).IsAtMax())
        {
            Modify(Stat.Type.Oxygen, Time.deltaTime * 30);
            if (GetStat(Stat.Type.Oxygen).IsAtMax())
                veer.AddVoiceLine(Voicelines.GetLine("oxygen_full"));
        }
    }

    // Change environment 
    public override void ChangeEnvironment(EnvironmentData environment)
    {
        // Check if environment is null
        if (environment == null) environment = _environment;
        if (this.environment == null) this.environment = _environment;

        // Check if previous environment had oxygen
        if (this.environment.isOxygenated && !environment.isOxygenated)
            veer.AddVoiceLine(Voicelines.GetLine("exit_oxygen_environment"));
        else if (!this.environment.isOxygenated && environment.isOxygenated)
            veer.AddVoiceLine(Voicelines.GetLine("enter_oxygen_environment"));

        // Check for environment footstep sounds
        if (environment.footstepSounds != null)
        {
            footstepAudioPlayer.SetRandomClipList(environment.footstepSounds);
            footstepAudioPlayer.TogglePaused(false);
        }
        else footstepAudioPlayer.TogglePaused(true);

        // Assign new environment
        this.environment = environment;
    }

    // Stats method
    public override void CheckStat(Stat.Type type)
    {
        // Get stat object
        Stat stat = GetStat(type);

        // Update stat
        switch (type)
        {
            case Stat.Type.Health:

                // Check if player still has health
                if (stat.IsAtMin()) Kill();

                break;

            case Stat.Type.Oxygen:

                // Check if player has no oxygen
                if (stat.IsAtMin()) Modify(Stat.Type.Health, Time.deltaTime * 5f);

                // If player does have oxygen, check for audio queues
                // if (stat.GetPercentage() <= 0f)
                //     veer.AddVoiceLine(Voicelines.GetLine("oxygen_critical"));
                // else if (stat.GetPercentage() <= 0.1f)
                //     veer.AddVoiceLine(Voicelines.GetLine("oxygen_low"));

                break;

            default:
                break;
        }
    }

    // Placeholder setup
    public void SetDefaultStats(float health, float shield, float stamina, float startingOxygen,
        float oxygen, float temperature, float radiation, float hunger, float thirst)
    {
        // Create new dictionary instances
        SetStat(new Stat(Stat.Type.Health, 0, health, health));
        SetStat(new Stat(Stat.Type.Shield, 0, shield, shield));
        SetStat(new Stat(Stat.Type.Stamina, 0, stamina, stamina));
        SetStat(new Stat(Stat.Type.Oxygen, 0, startingOxygen, oxygen));
        SetStat(new Stat(Stat.Type.Temperature, 25f, temperature, 45f));
        SetStat(new Stat(Stat.Type.Radiation, 0, radiation, 1000f));
        SetStat(new Stat(Stat.Type.Hunger, 0, hunger, hunger));
        SetStat(new Stat(Stat.Type.Thirst, 0, thirst, thirst));
    }
}
