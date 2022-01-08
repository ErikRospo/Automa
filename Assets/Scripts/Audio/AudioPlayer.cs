using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    // Audio component attached to this object
    private AudioSource audioPlayer;

    // Determine if a random pitch should be used
    public bool useRandomPitch = false;
    [Range(-3, 3)] public float minRandomPitch;
    [Range(-3, 3)] public float maxRandomPitch;

    // List of clips (if random should be chosen)
    public bool useRandomClipsList = false;
    public List<AudioClip> randomClips;

    // Volume type this player will use
    public Settings.VolumeType volumeType;

    // Start method
    public void Start()
    {
        if (audioPlayer == null)
            audioPlayer = GetComponent<AudioSource>();

        if (minRandomPitch > maxRandomPitch) minRandomPitch = maxRandomPitch;
    }

    /// <summary>
    /// Plays the clip on the <see cref="audioPlayer"/>, or chooses one from <see cref="randomClips"/> <br></br> <br></br>
    /// Whether the clip is random or not depends on <see cref="useRandomClipsList"/>
    /// </summary>
    public void PlayClip()
    {
        // Set the volume for the audio player
        audioPlayer.volume = Settings.GetVolume(volumeType);

        // Check if random pitch is set to true
        if (useRandomPitch) audioPlayer.pitch = Random.Range(minRandomPitch, maxRandomPitch);

        // Play specified clip if not set to random
        if (useRandomClipsList)
        {
            if (audioPlayer.clip != null)
                audioPlayer.Play();
        }

        // If set to random, choose a random clip
        else if (randomClips.Count > 0)
        {
            audioPlayer.clip = randomClips[Random.Range(0, randomClips.Count)];
            audioPlayer.Play();
        }
    }

    /// <summary>
    /// Change the pitch of the audio player (between -3 and 3)
    /// </summary>
    /// <param name="pitch"></param>
    public void ChangePitch(float pitch)
    {
        // Ensure pitch is within proper bounds
        if (pitch > 3) pitch = 3;
        else if (pitch < -3) pitch = -3;
        audioPlayer.pitch = pitch;
    }

    /// <summary>
    /// Changes the random pitch range from the audio player
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void SetPitchRange(float min, float max)
    {
        // Ensure pitch is within proper bounds
        if (min > max) min = max;
        minRandomPitch = min;
        maxRandomPitch = max;
    }

    /// <summary>
    /// Toggles if the audio clip should be played continously
    /// </summary>
    /// <param name="toggle"></param>
    public void ToggleAudioLoop(bool loopAudio) { audioPlayer.loop = loopAudio; }

    /// <summary>
    /// Sets an audio clip on the audio source directly
    /// </summary>
    /// <param name="clip"></param>
    public void SetClip(AudioClip clip) { audioPlayer.clip = clip; }

    /// <summary>
    /// Adds an audio clip to <see cref="randomClips"/>
    /// </summary>
    /// <param name="clip"></param>
    public void AddRandomClip(AudioClip clip) { randomClips.Add(clip); }

    /// <summary>
    /// Removes an audio clip from <see cref="randomClips"/>
    /// </summary>
    /// <param name="clip"></param>
    public void RemoveRandomClip(AudioClip clip) { if (randomClips.Contains(clip)) randomClips.Remove(clip); }

    /// <summary>
    /// Sets a predefined list of audio clips to <see cref="randomClips"/>
    /// </summary>
    /// <param name="clips"></param>
    public void SetRandomClipList(List<AudioClip> clips) { randomClips = clips; }
}
