using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Michsky.UI.ModernUIPack;

public class InvButton : MonoBehaviour
{
    // Button variables
    [HideInInspector]
    public ButtonManagerBasicIcon button;
    public TextMeshProUGUI amount;


    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<ButtonManagerBasicIcon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
