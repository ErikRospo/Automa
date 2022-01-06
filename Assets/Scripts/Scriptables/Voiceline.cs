using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Voiceline", menuName = "Voiceline")]
public class Voiceline : IdentifiableScriptableObject
{
    public new string name;
    public string translation;
    public AudioClip audio;
}
