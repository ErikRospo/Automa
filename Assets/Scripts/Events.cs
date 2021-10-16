﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static Events active;

    // Start is called before the first frame update
    private void Awake()
    {
        active = this;
    }

    public event Action onPlaceBuilding;
    public void PlaceBuilding()
    {
        if (onPlaceBuilding != null)
            onPlaceBuilding();
    }

    public event Action<Building> onBuildingClicked;
    public void BuildingClicked(Building building)
    {
        if (onBuildingClicked != null)
            onBuildingClicked(building);
    }

    public event Action<InventorySlot> onRegisterInventorySlot;
    public void RegisterInventorySlot(InventorySlot slot)
    {
        if (onRegisterInventorySlot != null)
            onRegisterInventorySlot(slot);
    }

    public event Action<Inventory> onRequestInventorySlots;
    public void RequestInventorySlots(Inventory inventory)
    {
        if (onRequestInventorySlots != null)
            onRequestInventorySlots(inventory);
    }
}
