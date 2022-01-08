/// <summary>
/// Data-holder for statistics that have a minimum, maximum, and current value
/// </summary>
public class Stat
{
    /// <summary>
    /// Create a full <see cref="Stat"/> class. All params must be NonNull
    /// </summary>
    /// <param name="type">The <see cref="Stat.Type"/> of this satistic.</param>
    /// <param name="min">The minimum <see cref="float"/> of this statstic.</param>
    /// <param name="current">The starting <see cref="float"/> of this statistic</param>
    /// <param name="max"></param>
    public Stat(Type type, float min, float current, float max)
    {
        this.type = type;
        this.min = min;
        this.current = current;
        this.max = max;
    }

    /// <summary>
    /// The types of <see cref="Stat"/>s that are possible
    /// </summary>
    public enum Type
    {
        Health,
        Shield,
        Stamina,
        Oxygen,
        Temperature,
        Radiation,
        Hunger,
        Thirst
    }

    /// <summary>
    /// The <see cref="Stat.Type"/> of this <see cref="Stat"/>
    /// </summary>
    public Type type;

    /// <summary>
    /// The minimum value of this <see cref="Stat"/>.
    /// </summary>
    public float min;

    /// <summary>
    /// The current value of this <see cref="Stat"/>.
    /// </summary>
    public float current;

    /// <summary>
    /// The maximum value of this <see cref="Stat"/>.
    /// </summary>
    public float max;

    /// <summary>
    /// Gets normalized percentage
    /// </summary>
    /// <returns><see cref="float"/> between 0 and 1</returns>
    public float GetPercentage() {
        return (current - min) / (max - min);
    }

    /// <summary>
    /// Checks if this <see cref="Stat"/> is at the maximum value.
    /// </summary>
    /// <returns>True if this <see cref="Stat"/> is at the maximum.</returns>
    public bool IsAtMax() { return current >= max; }

    /// <summary>
    /// Checks if this <see cref="Stat"/> is at the minimum value.
    /// </summary>
    /// <returns>True if this <see cref="Stat"/> is at the minimum value.</returns>
    public bool IsAtMin() { return current <= min; }
}
