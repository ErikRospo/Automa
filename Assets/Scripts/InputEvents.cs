using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEvents : MonoBehaviour
{
    public static InputEvents active;

    // Start is called before the first frame update
    public void Awake()
    {
        active = this;
    }

    public event Action onLeftMouseTapped;
    public void LeftMousePressed()
    {
        if (onLeftMouseTapped != null)
            onLeftMouseTapped();
    }

    public event Action onLeftMouseHeld;
    public void LeftMouseHeld()
    {
        if (onLeftMouseHeld != null)
            onLeftMouseHeld();
    }

    public event Action onLeftMouseReleased;
    public void LeftMouseReleased()
    {
        if (onLeftMouseReleased != null)
            onLeftMouseReleased();
    }

    public event Action onRightMouseTapped;
    public void RightMousePressed()
    {
        if (onRightMouseTapped != null)
            onRightMouseTapped();
    }

    public event Action onRightMouseHeld;
    public void RightMouseHeld()
    {
        if (onRightMouseHeld != null)
            onRightMouseHeld();
    }

    public event Action onRightMouseReleased;
    public void RightMouseReleased()
    {
        if (onRightMouseReleased != null)
            onRightMouseReleased();
    }

    public event Action onMiddleMousePressed;
    public void MiddleMousePressed()
    {
        if (onMiddleMousePressed != null)
            onMiddleMousePressed();
    }

    public event Action onEscapePressed;
    public void EscapePressed()
    {
        if (onEscapePressed != null)
            onEscapePressed();
    }

    public event Action onSpacePressed;
    public void SpacePressed()
    {
        if (onSpacePressed != null)
            onSpacePressed();
    }

    public event Action onShiftPressed;
    public void ShiftPressed()
    {
        if (onShiftPressed != null)
            onShiftPressed();
    }

    public event Action onShifReleased;
    public void ShiftReleased()
    {
        if (onShifReleased != null)
            onShifReleased();
    }

    public event Action onLeftControlPressed;
    public void LeftControlPressed()
    {
        if (onLeftControlPressed != null)
            onLeftControlPressed();
    }

    public event Action onLeftControlReleased;
    public void LeftControlReleased()
    {
        if (onLeftControlReleased != null)
            onLeftControlReleased();
    }

    public event Action onPausePressed;
    public void PausePressed()
    {
        if (onPausePressed != null)
            onPausePressed();
    }

    public event Action onInteractPressed;
    public void InteractPressed()
    {
        if (onInteractPressed != null)
            onInteractPressed();
    }

    public event Action<int> onNumberInput;
    public void NumberInput(int number)
    {
        if (onNumberInput != null)
            onNumberInput(number);
    }
}