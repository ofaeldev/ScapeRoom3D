using UnityEngine;

public class FeedbackOnlyInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string feedbackMessage;

    public InteractionType InteractionType => InteractionType.FeedbackOnly;
    public InteractionType PuzzleType => InteractionType.None;
    public bool StartsPuzzle => false;

    public void Interact(PlayerContext context)
    {

        if (string.IsNullOrEmpty(feedbackMessage))
        {
            Debug.LogWarning("Feedback não atribuído");
            return;
        }

        // Para o movimento e bloqueia os inputs do jogador
        context.playerAnimatorController.StopMovement();
        context.inputBlocker.Block();

        // Dispara evento sonoro/UI
        InteractionEvents.RaiseInteractionStarted(InteractionType);
        UIFeedback.Instance.ShowMessageFeedback(feedbackMessage);

        // Alterna input de interação
        context.interactInput = !context.interactInput;
    }

    public void ExitInteract(PlayerContext context)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        context.inputBlocker.Unblock(); // Desbloqueia inputs novamente
        context.ChangeState(context.idleState);
    }

    public string GetInteractionPrompt()
    {
        return "Ver";
    }
}
