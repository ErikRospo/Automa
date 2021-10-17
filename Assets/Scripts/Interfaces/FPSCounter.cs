using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class FPSCounter : MonoBehaviour
{
    private TextMeshProUGUI counter;

    public void Start()
    {
        counter = GetComponent<TextMeshProUGUI>();
        Application.targetFrameRate = 1000;
        InvokeRepeating("UpdateFPS", 1, 1);
    }

    // Update is called once per frame
    public void UpdateFPS()
    {
        counter.text = (int)(1f / Time.unscaledDeltaTime) + " FPS";
    }
}
