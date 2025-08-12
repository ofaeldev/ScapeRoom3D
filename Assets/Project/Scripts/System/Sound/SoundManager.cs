using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private List<InteractionSoundData> interactionSounds;

    private Dictionary<InteractionType, InteractionSoundData> soundLookup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        soundLookup = new Dictionary<InteractionType, InteractionSoundData>();
        foreach (var soundData in interactionSounds)
        {
            soundLookup[soundData.interactionType] = soundData;
        }
    }

    public void PlayInteractionSound(InteractionType type)
    {
        if (soundLookup.TryGetValue(type, out var soundData))
        {
            sfxSource.PlayOneShot(soundData.soundClip, soundData.volume);
        }
    }
}
