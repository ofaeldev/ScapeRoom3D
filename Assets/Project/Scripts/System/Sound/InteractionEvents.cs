using System;
using UnityEngine;

public static class InteractionEvents
{
    public static event Action<InteractionType> OnInteractionStarted;
    public static event Action<InteractionType> OnInteractionActionMoment;
    public static event Action<InteractionType> OnInteractionFinished;

    public static void RaiseInteractionStarted(InteractionType type)
    {
        OnInteractionStarted?.Invoke(type);
    }

    public static void RaiseInteractionActionMoment(InteractionType type)
    {
        OnInteractionActionMoment?.Invoke(type);
    }

    public static void RaiseInteractionFinished(InteractionType type)
    {
        OnInteractionFinished?.Invoke(type);
    }
}
