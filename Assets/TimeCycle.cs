using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCycle : MonoBehaviour
{
    public static float time = 0f;

    public void Start()
    {
        time = 0f;
    }

    public void Update()
    {
        time += Time.deltaTime;
    }


}
