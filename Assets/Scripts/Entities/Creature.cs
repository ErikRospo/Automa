using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Creature : NetworkBehaviour, IDamageable
{
    // List of all player stats
    protected Dictionary<Stat.Type, Stat> stats = new Dictionary<Stat.Type, Stat>();

    // Damage method (IDamageable interface)
    public virtual void Damage(float dmg)
    {
        Modify(Stat.Type.Health, dmg);
    }

    // Kill method (IDamageable interface)
    public virtual void Kill()
    {
        Destroy(gameObject);
    }

    /// <summary> Checks a specified stat. </summary>
    public virtual void CheckStat(Stat.Type stat)
    {
        switch (stat)
        {
            case Stat.Type.Health:
                if (GetStat(stat).IsAtMin()) Kill();
                break;

            default:
                break;
        }
    }

    /// <summary> Modifies the current value of a stat. </summary>
    public void Modify(Stat.Type type, float amount)
    {
        // Check if stat exists
        if (!HasStat(type))
        {
            Debug.Log("Entity does not have stat " + type.ToString() + " (Modify)");
            return;
        }

        // Modify stat amount
        stats[type].current += amount;

        // Check stat bounds
        if (stats[type].IsAtMax())
            stats[type].current = stats[type].max;
        else if (stats[type].IsAtMin())
            stats[type].current = stats[type].min;

        // Update stat
        CheckStat(type);
    }

    /// <summary> Modifies the maximum bound of a stat. </summary>
    public void ModifyMax(Stat.Type type, float amount)
    {
        // Check if stat exists
        if (!HasStat(type))
        {
            Debug.Log("Entity does not have stat " + type.ToString() + " (ModifyMax)");
            return;
        }

        // Modify max stat amount
        stats[type].max += amount;

        // Check stat bounds
        if (stats[type].IsAtMax())
            stats[type].current = stats[type].max;

        // Update stat
        CheckStat(type);
    }

    /// <summary> Modifies the minimum bound of a stat. </summary>
    public void ModifyMin(Stat.Type type, float amount)
    {
        // Check if stat exists
        if (!HasStat(type))
        {
            Debug.Log("Entity does not have stat " + type.ToString() + " (ModifyMin)");
            return;
        }

        // Modify max stat amount
        stats[type].min += amount;

        // Check stat bounds
        if (stats[type].IsAtMin())
            stats[type].current = stats[type].min;

        // Update stat
        CheckStat(type);
    }

    /// <summary> Sets a stat object, or creates a new one if it doesn't exist. </summary>
    public void SetStat(Stat stat)
    {
        if (HasStat(stat)) stats[stat.type] = stat;
        else stats.Add(stat.type, stat);
    }

    /// <summary> Returns a stat object, or returns null if it doesn't exist. </summary>
    public Stat GetStat(Stat.Type type) 
    {
        if (HasStat(type)) return stats[type];
        Debug.Log(transform.name + " does not have stat " + type.ToString());
        return null;
    }

    /// <summary> Removes a stat from a creature. </summary>
    public void RemoveStat(Stat.Type type)
    {
        if (HasStat(type)) stats.Remove(type);
        else Debug.Log(transform.name + " does not have stat " + type.ToString());
    }

    /// <summary> Checks to see if a stat exists. </summary>
    public bool HasStat(Stat stat) { return stats.ContainsKey(stat.type); }
    public bool HasStat(Stat.Type type) { return stats.ContainsKey(type); }
}
