using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Environment", menuName = "World/Environment")]
public class EnvironmentData : IdentifiableScriptableObject
{
    public List<AudioClip> footstepSounds;
    public bool isOxygenated;
}