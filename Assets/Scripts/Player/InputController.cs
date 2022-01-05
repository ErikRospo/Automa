using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Mirror;

public class InputController : NetworkBehaviour
{
    // Check for input from device(s)
    public void Update()
    {
        if (!hasAuthority) return;

        CheckKeybindInput();
        CheckHotbarInput();
    }

    // Check keybind input
    [ClientCallback]
    public void CheckKeybindInput()
    {
        // Interacting input check
        if (Input.GetKeyDown(Keybinds.interact))
            InputEvents.active.InteractPressed();
    }

    // Checks for hotbar input
    [ClientCallback]
    private void CheckHotbarInput()
    {
        if (Input.GetKeyDown(Keybinds.hotbar_1)) InputEvents.active.NumberInput(1);
        else if (Input.GetKeyDown(Keybinds.hotbar_2)) InputEvents.active.NumberInput(2);
        else if (Input.GetKeyDown(Keybinds.hotbar_3)) InputEvents.active.NumberInput(3);
        else if (Input.GetKeyDown(Keybinds.hotbar_4)) InputEvents.active.NumberInput(4);
        else if (Input.GetKeyDown(Keybinds.hotbar_5)) InputEvents.active.NumberInput(5);
        else if (Input.GetKeyDown(Keybinds.hotbar_6)) InputEvents.active.NumberInput(6);
        else if (Input.GetKeyDown(Keybinds.hotbar_7)) InputEvents.active.NumberInput(7);
        else if (Input.GetKeyDown(Keybinds.hotbar_8)) InputEvents.active.NumberInput(8);
        else if (Input.GetKeyDown(Keybinds.hotbar_9)) InputEvents.active.NumberInput(9);
        else if (Input.GetKeyDown(Keybinds.hotbar_0)) InputEvents.active.NumberInput(0);
    }
}