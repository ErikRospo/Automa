using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Creature : NetworkBehaviour, IDamageable
{
    /// <summary>
    /// Dictionary of all <see cref="Creature"/> <see cref="Stat"/>s keyed by their <see cref="Stat.Type"/>.
    /// </summary>
    protected Dictionary<Stat.Type, Stat> stats = new Dictionary<Stat.Type, Stat>();

    // Environment the creature is in
    public EnvironmentData _environment;
    protected EnvironmentData environment;

    /// <summary>
    /// Change the <see cref="EnvironmentData"/> the <see cref="Creature"/> is in.
    /// </summary>
    /// <param name="environment">The <see cref="EnvironmentData"/> to put the <see cref="Creature"/> in.</param>
    public virtual void ChangeEnvironment(EnvironmentData environment)
    {
        // Check if environment is null
        if (environment == null) environment = _environment;
        if (this.environment == null) this.environment = _environment;

        // Assign new environment
        this.environment = environment;
    }

    /// <summary>
    /// Damage this <see cref="Creature"/> using <see cref="IDamageable"/>
    /// </summary>
    /// <param name="dmg">The <see cref="float"/> amount to damage the <see cref="Creature"/> by.</param>
    public virtual void Damage(float dmg)
    {
        Modify(Stat.Type.Health, dmg);
    }

    /// <summary>
    /// Kill this <see cref="Creature"/> using <see cref="IDamageable"/>
    /// </summary>
    public virtual void Kill()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Checks a specified stat for actions.
    /// </summary>
    /// <param name="stat">The <see cref="Stat.Type"/> to check.</param>
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

    /// <summary>
    /// Modifies the current value of a stat.
    /// </summary>
    /// <param name="type">The <see cref="Stat.Type"/> to modify.</param>
    /// <param name="amount">The <see cref="float"/> amount to modify it by.</param>
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

    /// <summary>
    /// Modifies the maximum bound of a stat.
    /// </summary>
    /// <param name="type">The <see cref="Stat.Type"/> to modify.</param>
    /// <param name="amount">The <see cref="float"/> amount to modify it by.</param>
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

    /// <summary>
    /// Modifies the minimum bound of a stat.
    /// </summary>
    /// <param name="type">The <see cref="Stat.Type"/> to modify.</param>
    /// <param name="amount">The <see cref="float"/> amount to modify it by.</param>
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

    /// <summary>
    /// Sets a <see cref="Stat"/> object, or creates a new one if it doesn't exist.
    /// </summary>
    /// <param name="stat">The stat to set.</param>
    public void SetStat(Stat stat)
    {
        if (HasStat(stat)) stats[stat.type] = stat;
        else stats.Add(stat.type, stat);
    }

    /// <summary>
    /// Returns a stat object, or returns null if it doesn't exist.
    /// </summary>
    /// <param name="type">The <see cref="Stat.Type"/> to get.</param>
    /// <returns>The <see cref="Stat"/> of type <paramref name="type"/>.</returns>
    public Stat GetStat(Stat.Type type) 
    {
        if (HasStat(type)) return stats[type];
        Debug.Log(transform.name + " does not have stat " + type.ToString());
        return null;
    }

    /// <summary>
    /// Removes a stat from a <see cref="Creature"/>.
    /// </summary>
    /// <param name="type">The <see cref="Stat.Type"/> to remove.</param>
    public void RemoveStat(Stat.Type type)
    {
        if (HasStat(type)) stats.Remove(type);
        else Debug.Log(transform.name + " does not have stat " + type.ToString());
    }

    /// <summary>
    /// Checks to see if a stat exists.
    /// </summary>
    /// <param name="stat">The <see cref="Stat"/> containing the <see cref="Stat.Type"/> to check.</param>
    /// <returns>True if stat exists on <see cref="Creature"/>.</returns>
    public bool HasStat(Stat stat) { return stats.ContainsKey(stat.type); }

    /// <summary>
    /// Checks to see if a stat exists.
    /// </summary>
    /// <param name="type">The <see cref="Stat.Type"/> to check.</param>
    /// <returns>True if stat exists on <see cref="Creature"/></returns>
    public bool HasStat(Stat.Type type) { return stats.ContainsKey(type); }
}
