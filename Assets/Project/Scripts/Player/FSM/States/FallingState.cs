using UnityEngine;
using FSM;

[CreateAssetMenu(menuName = "FSM/States/Falling")]
public class FallingState : BasePlayerState
{
    public override void Enter()
    {
        context.animator.applyRootMotion = false;
        context.playerAnimatorController.SetFalling(true);
        Debug.Log("Entrou no estado falling");
    }
    public override void Tick()
    {
        // Quando encostar no chão, volta pro Idle ou Move
        if (context.isGrounded)
        {
            if (!TryMove())
                TryIdle();
        }

    }

    public override void Exit()
    {
        context.playerAnimatorController.SetFalling(false);
        Debug.Log("Saiu do FallingState");
    }
}