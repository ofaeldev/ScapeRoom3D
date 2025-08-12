// SoundListener.cs
using UnityEngine;

public class SoundListener : MonoBehaviour
{
    private void OnEnable()
    {
        InteractionEvents.OnInteractionStarted += HandleInteractionStarted;
    }

    private void OnDisable()
    {
        InteractionEvents.OnInteractionStarted -= HandleInteractionStarted;
    }

    private void HandleInteractionStarted(InteractionType type)
    {
        SoundManager.Instance.PlayInteractionSound(type);
    }
}
