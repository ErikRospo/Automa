using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Mirror;

public class InputController : NetworkBehaviour
{
    public void Update()
    {
        if (!hasAuthority) return;

        CheckKeybindInput();
    }

    // Check keybind input
    [Client]
    public void CheckKeybindInput()
    {
        // Interacting input check
        if (Input.GetKeyDown(Keybinds.interact))
            InputEvents.active.InteractPressed();
    }

    // Checks if for numeric input
    [Client]
    public void CheckNumberInput()
    {
        if (Input.GetKeyDown(Keybinds.hotbar_1))
            InputEvents.active.NumberInput(0);
        else if (Input.GetKeyDown(Keybinds.hotbar_2))
            InputEvents.active.NumberInput(1);
        else if (Input.GetKeyDown(Keybinds.hotbar_3))
            InputEvents.active.NumberInput(2);
        else if (Input.GetKeyDown(Keybinds.hotbar_4))
            InputEvents.active.NumberInput(3);
        else if (Input.GetKeyDown(Keybinds.hotbar_5))
            InputEvents.active.NumberInput(4);
        else if (Input.GetKeyDown(Keybinds.hotbar_6))
            InputEvents.active.NumberInput(5);
        else if (Input.GetKeyDown(Keybinds.hotbar_7))
            InputEvents.active.NumberInput(6);
        else if (Input.GetKeyDown(Keybinds.hotbar_8))
            InputEvents.active.NumberInput(7);
        else if (Input.GetKeyDown(Keybinds.hotbar_9))
            InputEvents.active.NumberInput(8);
    }
}