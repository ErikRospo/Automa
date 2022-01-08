using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    // Audio component attached to this object
    private AudioSource audioPlayer;

    // List of clips (if random should be chosen)
    public bool useRandomClipsList;
    public List<AudioClip> randomClips;

    // Volume type this player will use
    public Settings.VolumeType volumeType;

    // Start method
    public void Start()
    {
        if (audioPlayer == null)
            audioPlayer = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Plays the clip on the <see cref="audioPlayer"/>, or chooses one from <see cref="randomClips"/> <br></br> <br></br>
    /// Whether the clip is random or not depends on <see cref="useRandomClipsList"/>
    /// </summary>
    public void PlayClip()
    {
        // Set the volume for the audio player
        audioPlayer.volume = Settings.GetVolume(volumeType);

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
