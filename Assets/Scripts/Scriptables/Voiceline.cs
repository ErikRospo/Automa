using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Voiceline ID's and translations
// https://docs.google.com/spreadsheets/d/19zwwKQXA00ff4Hzy6THyu9ErTiBCfEcNz8d5BabCRPg/edit?usp=sharing

[CreateAssetMenu(fileName = "New Voiceline", menuName = "Voiceline")]
public class Voiceline : IdentifiableScriptableObject
{
    // Languages
    public enum Language
    {
        English,
        French
    }

    // Audio clips
    [System.Serializable]
    public class Clip
    {
        public string name;
        public string translation;
        public AudioClip audio;
        public Language language;
    }

    public new string name;
    public string identifier;
    public List<Clip> clips;

    public Clip GetClip(Language language = Language.English)
    {
        foreach (Clip clip in clips)
            if (clip.language == language)
                return clip;

        Debug.Log("Could not retrieve clip with the specified language " + language.ToString());
        return null;
    }

    public AudioClip GetAudio(Language language = Language.English)
    {
        foreach (Clip clip in clips)
            if (clip.language == language)
                return clip.audio;

        Debug.Log("Could not retrieve audio with the specified language " + language.ToString());
        return null;
    }
}
