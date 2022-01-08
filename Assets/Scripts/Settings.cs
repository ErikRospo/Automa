using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public enum VolumeType
    {
        Music,
        Sound,
        Voice
    }

    [Range(0, 1)] public static float masterVolume;
    [Range(0, 1)] public static float musicVolume;
    [Range(0, 1)] public static float soundVolume;
    [Range(0, 1)] public static float voiceVolume;

    public static float GetVolume(VolumeType type)
    {
        switch (type)
        {
            case VolumeType.Music:
                return masterVolume * musicVolume;
            case VolumeType.Sound:
                return masterVolume * soundVolume;
            case VolumeType.Voice:
                return masterVolume * voiceVolume;
            default:
                return 0.5f;
        }
    }
}
