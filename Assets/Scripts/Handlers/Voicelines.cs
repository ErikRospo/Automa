using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Voiceline ID's and translations
// https://docs.google.com/spreadsheets/d/19zwwKQXA00ff4Hzy6THyu9ErTiBCfEcNz8d5BabCRPg/edit?usp=sharing

public class Voicelines
{
    public static Dictionary<string, Voiceline> lines = new Dictionary<string, Voiceline>();

    public static Voiceline GetLine(string id)
    {
        if (lines.ContainsKey(id)) return lines[id];
        Debug.Log("Could not find voiceline with ID " + id);
        return null;
    }
}
