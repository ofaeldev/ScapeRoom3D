using UnityEngine;
using System.Collections;

public class FeedbackManager : MonoBehaviour
{
    [SerializeField] private UIFeedback feedbackPanel;
    [SerializeField] private float feedbackDuration = 2.5f;

    private Coroutine hideCoroutine;

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
        if (type != InteractionType.FeedbackOnly)
            return;

        // Aqui você pode mapear uma mensagem padrão ou pegar do objeto interativo.
        string message = FeedbackMessageProvider.GetLastMessage(); // 👈 ver abaixo
        feedbackPanel.ShowMessageFeedback(message);

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);
        hideCoroutine = StartCoroutine(HideFeedbackAfterDelay());
    }

    private IEnumerator HideFeedbackAfterDelay()
    {
        yield return new WaitForSeconds(feedbackDuration);
        feedbackPanel.HideDescription();
    }
}
