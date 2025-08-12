using UnityEngine;
using FSM;

[CreateAssetMenu(menuName = "FSM/States/Interact")]
public class InteractState : State<PlayerContext>
{
    private IInteractable currentTarget;

    public override void Enter()
    {
        currentTarget = context.interactionDetector.CurrentTarget;

        if (currentTarget == null)
        {
            Debug.LogWarning("Tentou interagir, mas nenhum alvo foi detectado.");
            stateMachine.ChangeState(context.idleState);
            return;
        }

        StartInteraction();
    }

    public override void Tick()
    {
        if (EndInteraction()) return;
    }

    private void StartInteraction()
    {
        // Executa a ação do objeto interativo
        currentTarget.Interact(context);

        // Mostra o cursor do mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Para o movimento e bloqueia os inputs do jogador
        context.playerAnimatorController.StopMovement();
        context.inputBlocker.Block();

        Debug.Log($"Entrou no estado Interact com {currentTarget} ({currentTarget.InteractionType})");
    }

    private bool EndInteraction()
    {
        if (currentTarget != null || currentTarget.InteractionType == InteractionType.FeedbackOnly)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Debug.Log($"Saiu da interação com {currentTarget} ({currentTarget.InteractionType})");
            context.inputBlocker.Unblock(); // Desbloqueia inputs novamente
            currentTarget.ExitInteract(context);
            context.ChangeState(context.idleState);
            return true;
        }
        else
        {
            Debug.LogWarning("Tentou sair da interação, mas o alvo era nulo.");
        }

        return false;
    }

    public override void Exit()
    {
        // Garante que o cursor e os inputs voltem ao normal mesmo se sair por outra via
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        context.inputBlocker.Unblock();
        currentTarget = null;
    }
}
