using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour, IDamageable
{
    // List of all player stats
    public Dictionary<Stat, float> stats;
    public Dictionary<Stat, float> maxStats;

    // Placeholder setup
    public void SetupStats(float health, float shield, float stamina, float oxygen,
        float temperature, float radiation, float hunger, float thirst)
    {
        // Create new dictionary instances
        stats = new Dictionary<Stat, float>();
        maxStats = new Dictionary<Stat, float>();

        // Set HEALTH to default value
        stats.Add(Stat.Health, health);
        maxStats.Add(Stat.Health, stats[Stat.Health]);

        // Set SHIELD to default value
        stats.Add(Stat.Shield, shield);
        maxStats.Add(Stat.Shield, stats[Stat.Shield]);

        // Set STAMINA to default value
        stats.Add(Stat.Stamina, stamina);
        maxStats.Add(Stat.Stamina, stats[Stat.Stamina]);

        // Set OXYGEN to default value
        stats.Add(Stat.Oxygen, oxygen);
        maxStats.Add(Stat.Oxygen, stats[Stat.Oxygen]);

        // Set TEMPERATURE to default value
        stats.Add(Stat.Temperature, temperature);
        maxStats.Add(Stat.Temperature, 35);

        // Set RADIATION to default value
        stats.Add(Stat.Radiation, radiation);
        maxStats.Add(Stat.Radiation, 1000);

        // Set HUNGER to default value
        stats.Add(Stat.Hunger, hunger);
        maxStats.Add(Stat.Hunger, stats[Stat.Hunger]);

        // Set THIRST to default value
        stats.Add(Stat.Thirst, thirst);
        maxStats.Add(Stat.Thirst, stats[Stat.Thirst]);
    }

    // Damage method (IDamageable interface)
    public virtual void Damage(float dmg)
    {
        stats[Stat.Health] -= dmg;
        if (stats[Stat.Health] <= 0) Kill();
    }

    // Kill method (IDamageable interface)
    public virtual void Kill()
    {
        Destroy(gameObject);
    }

    // Check stats
    public virtual void CheckStat(Stat stat)
    {
        switch (stat)
        {
            case Stat.Health:
                if (stats[Stat.Health] <= 0) Kill();
                break;

            default:
                break;
        }
    }

    // Add amount to stat
    public virtual void Add(Stat stat, float amount)
    {
        stats[stat] += amount;
        if (stats[stat] > maxStats[stat])
            stats[stat] = maxStats[stat];
        CheckStat(stat);
    }

    // Add max amount to stat
    public virtual void AddMax(Stat stat, float amount)
    {
        maxStats[stat] += amount;
        CheckStat(stat);
    }

    // Remove amount from stat
    public virtual void Remove(Stat stat, float amount)
    {
        stats[stat] -= amount;
        if (stats[stat] < 0)
            stats[stat] = 0;
        CheckStat(stat);
    }

    // Remove amount from stat
    public virtual void RemoveMax(Stat stat, float amount)
    {
        maxStats[stat] -= amount;
        if (stats[stat] > maxStats[stat])
            stats[stat] = maxStats[stat];
        CheckStat(stat);
    }

    // Get a stat
    public float GetStat(Stat stat) { return stats[stat]; }
    
    // Check if at max
    public bool IsAtMax(Stat stat) { return stats[stat] >= maxStats[stat]; }
}
