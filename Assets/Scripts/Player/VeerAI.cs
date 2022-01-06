using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Voiceline ID's and translations
// https://docs.google.com/spreadsheets/d/19zwwKQXA00ff4Hzy6THyu9ErTiBCfEcNz8d5BabCRPg/edit?usp=sharing

public class VeerAI : MonoBehaviour
{
    // Audio component
    private AudioSource audioSource;

    // Queued voicelines
    public Queue<Voiceline> voicelineQueue = new Queue<Voiceline>();

    // On start method
    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update method
    public void Update()
    {
        if (voicelineQueue.Count > 0 && !audioSource.isPlaying) NextVoiceLine();
    }

    // Plays the next queued voiceline
    public void NextVoiceLine()
    {
        Voiceline voiceline = voicelineQueue.Dequeue();
        audioSource.clip = voiceline.audio;
        audioSource.Play();
    }

    // Add a voiceline to the queue
    public void AddVoiceLine(Voiceline voiceline)
    {
        if (!voicelineQueue.Contains(voiceline))
            voicelineQueue.Enqueue(voiceline);
    }
}
