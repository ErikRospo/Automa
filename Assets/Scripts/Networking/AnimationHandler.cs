using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public static Animator conveyorMaster;

    public void Start()
    {
        conveyorMaster = GetComponent<Animator>();
    }
}
