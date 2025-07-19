using System.Collections.Generic;
using UnityEngine;

public enum AudioClipEnum
{
    CarGetSound,
    CarIgnitionSound,
    PremiumCarGetSound
}

public class AudioManager : SingletonBase<AudioManager>
{
    [SerializeField]
    private AudioSource audioSource;

    private readonly Dictionary<string, AudioClip> audioClips = new();

    protected override void Awake()
    {
        base.Awake();

        LoadAudioClips();
    }

    private void LoadAudioClips()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");
        foreach (AudioClip clip in clips)
        {
            audioClips[clip.name] = clip;
        }
    }

    public void Play(AudioClipEnum audioClipEnum)
    {
        string clipName = audioClipEnum.ToString();

        if (audioClips.TryGetValue(clipName, out var clip))
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Audio clip '{clipName}' not found!");
        }
    }

    public void Stop()
    {
        audioSource.Stop();
    }
}