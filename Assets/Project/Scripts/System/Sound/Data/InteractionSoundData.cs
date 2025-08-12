using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Interaction Sound")]
public class InteractionSoundData : ScriptableObject
{
    public InteractionType interactionType;
    public AudioClip soundClip;
    public float volume = 1f;
}
