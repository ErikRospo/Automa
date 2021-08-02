using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static Events events;

    // Start is called before the first frame update
    private void Awake()
    {
        events = this;
    }

    public event Action onBuildingPlaced;
    public void PlaceBuilding()
    {
        if (onBuildingPlaced != null)
            PlaceBuilding();
    }
}
