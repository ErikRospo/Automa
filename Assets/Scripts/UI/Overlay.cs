using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Overlay : MonoBehaviour
{
    // Stat UI class
    [SerializeField]
    public class StatElement
    {
        public Stat stat;
        public string name;
        public TextMeshProUGUI text;

        // Update a stat
        public void SetStat(float amount)
        {

        }
    }
    public List<StatElement> _statElements;

    // Internal dictionary
    public Dictionary<Stat, StatElement> statElements = new Dictionary<Stat, StatElement>();

    // Setup overlay
    public void Start()
    {
        // Loop through stat elements
        foreach(StatElement stat in _statElements)
        {
            
        }
    }
}
