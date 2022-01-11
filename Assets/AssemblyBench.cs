using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyBench : MonoBehaviour
{
    public InventoryUI test;

    private CanvasGroup canvasGroup;

    public void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        UIEvents.active.onOpenInterface += SetPlayer;
        InputEvents.active.onEscapePressed += DisableUI;
    }

    /// <summary>
    /// Connects the assembly bench to the player interacting with it
    /// </summary>
    /// <param name="type"></param>
    /// <param name="player"></param>
    public void SetPlayer(InterfaceType type, Player player)
    {
        if (type == InterfaceType.Assembly)
        {
            test.SetInventory(player.inventory);
            ToggleUI(true);
        }
    }

    /// <summary>
    /// Disables the assembly bench interface
    /// </summary>
    public void DisableUI()
    {
        ToggleUI(false);
    }

    /// <summary>
    /// Toggle the interface on or off
    /// </summary>
    /// <param name="toggle"></param>
    public void ToggleUI(bool toggle)
    {
        if (toggle)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
