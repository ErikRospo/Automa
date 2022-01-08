using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceOpener : MonoBehaviour
{
    // What type the interface is
    public InterfaceType interfaceType;

    // Player the interface is interacting with
    private Player player;

    // Interfaction flag (can be toggled)
    public bool canInteract;
    private bool isSubscribed;

    // Open the interface
    public void OpenInterface()
    {
        if (canInteract)
            UIEvents.active.OpenInterface(interfaceType, player);
    }

    // Trigger area (can work without a trigger)
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the player object
        Player newPlayer = collision.GetComponent<Player>();
        if (newPlayer == null || !player.hasAuthority) return;

        // Check if already subscribed
        if (!isSubscribed)
        {
            player = newPlayer;
            isSubscribed = true;
            InputEvents.active.onInteractPressed += OpenInterface;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        // Get the player object
        Player newPlayer = collision.GetComponent<Player>();
        if (newPlayer == null || !player.hasAuthority) return;

        // Check if subscribed
        if (isSubscribed)
        {
            player = null;
            isSubscribed = false;
            InputEvents.active.onInteractPressed -= OpenInterface;
        }
    }
}
