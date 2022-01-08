using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seat
{
    public Seat(Transform position)
    {
        this.location = position;
    }

    Transform location;
    Transform rider;

    /// <summary>
    /// Sit a player into the seat.
    /// </summary>
    /// <param name="rider">The <see cref="Transform"/> of the player to sit.</param>
    public void Sit(Transform rider)
    {
        this.rider = rider;
        rider.SetParent(location);
        rider.SetPositionAndRotation(location.position, location.rotation);
    }

    /// <summary>
    /// Remove the rider from the seat.
    /// </summary>
    public void UnSit()
    {
        if (HasRider())
        {
            rider.SetParent(null);
            rider.SetPositionAndRotation(location.position, location.rotation);
        }
    }

    /// <summary>
    /// Get if the seat is occupied
    /// </summary>
    /// <returns>True if there is already a rider</returns>
    public bool HasRider()
    {
        return rider != null;
    }
}
