using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyBench : MonoBehaviour
{
    public InventoryUI test;

    public void Start()
    {
        UIEvents.active.onOpenInterface += SetPlayer;
    }

    /// <summary>
    /// Connects the assembly bench to the player interacting with it
    /// </summary>
    /// <param name="type"></param>
    /// <param name="player"></param>
    public void SetPlayer(InterfaceType type, Player player)
    {
        if (type == InterfaceType.Assembly)
            test.SetInventory(player.inventory);
    }

    /// <summary>
    /// Toggle the interface on or off
    /// </summary>
    /// <param name="toggle"></param>
    public void ToggleUI(bool toggle)
    {

    }
}
