using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    // Setting
    public int setting = 0;

    // Use equipment 
    public virtual void UseEquipment()
    {
        Debug.Log("This equipment has no use function");
    }

    // Change settings
    public virtual void ChangeSetting(int newSetting)
    {
        Debug.Log("Change settings to " + newSetting);
        setting = newSetting;
    }
}
