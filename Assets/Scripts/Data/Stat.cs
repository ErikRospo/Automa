public class Stat
{
    // Constructor for new stats
    public Stat(Type type, float min, float current, float max)
    {
        this.type = type;
        this.min = min;
        this.current = current;
        this.max = max;
    }

    // Stat types
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

    // Stat values
    public Type type;
    public float min;
    public float current;
    public float max;

    /// <summary>
    /// Gets normalized percentage
    /// </summary>
    /// <returns>float between 0 and 1</returns>
    public float GetPercentage() {
        return (current - min) / (max - min);
    }

    // Check if at max or min
    public bool IsAtMax() { return current >= max; }
    public bool IsAtMin() { return current <= min; }
}
