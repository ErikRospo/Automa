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
        public Language language;
        [TextArea] public string translation;
    }

    public AudioClip femaleAudio;
    public AudioClip maleAudio;
    public string identifier;
    public List<Clip> clips;

    public string GetTranslation(Language language = Language.English)
    {
        foreach (Clip clip in clips)
            if (clip.language == language)
                return clip.translation;

        Debug.Log("Could not retrieve translation for the specified language " + language.ToString());
        return null;
    }

    public AudioClip GetAudio(bool female)
    {
        if (female) return femaleAudio;
        else return maleAudio;
    }
}
