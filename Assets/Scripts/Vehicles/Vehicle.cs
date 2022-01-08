using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : NetworkBehaviour, IDamageable
{
    /// <summary>
    /// Dictionary of all <see cref="Vehicle"/> <see cref="Stat"/>s keyed by their <see cref="Stat.Type"/>.
    /// </summary>
    protected Dictionary<Stat.Type, Stat> stats = new Dictionary<Stat.Type, Stat>();

    /// <summary>
    /// Used for adding seats in the editor.
    /// </summary>
    public List<Transform> seatTransforms;

    /// <summary>
    /// List of all seats in the <see cref="Vehicle"/>. seats[0] is the driver seat.
    /// </summary>
    protected List<Seat> seats;

    public virtual void Damage(float dmg)
    {
        throw new System.NotImplementedException();
    }

    public virtual void Kill()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Convert editor added seat <see cref="Transform"/> to <see cref="Seat"/>s, then clear the editor added list
    /// </summary>
    protected void GetSeats()
    {
        seats = new List<Seat>();
        foreach (Transform transform in seatTransforms)
            seats.Add(new Seat(transform));
        
        seatTransforms = null;
    }

}
