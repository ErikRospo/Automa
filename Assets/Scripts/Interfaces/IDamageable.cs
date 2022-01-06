/// <summary>
/// Interface for entities that should be able to recieve damage.
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Apply a <see cref="float"/> amount of damage.
    /// </summary>
    /// <param name="dmg">The <see cref="float"/> amount of damage to apply.</param>
    void Damage(float dmg);

    /// <summary>
    /// Apply a lethal amount of damage.
    /// </summary>
    void Kill();
}
