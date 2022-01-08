using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioSource music;

    public void Update() 
    {
        if (Input.GetKeyDown(Keybinds.map))
        {
            if (music.volume == 0.25f) music.volume = 0f;
            else music.volume = 0.25f;
        }
    }
}
