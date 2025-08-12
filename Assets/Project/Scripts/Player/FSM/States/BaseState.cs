// BasePlayerState.cs
using FSM;
using UnityEngine;

public abstract class BasePlayerState : State<PlayerContext>
{
    protected bool isTurningInPlace = false;
    protected bool waitingToEnterTurnState = false; 
    protected float turnDuration = 1f; // duração aproximada da animação
    protected float turnElapsed = 0f;
    protected Quaternion targetRotation;
    protected Quaternion startRotation;
    protected void DetectInteractable() =>
        PlayerStateUtils.DetectInteractable(context);
    protected bool TryInteract() =>
        PlayerStateUtils.TryInteract(context, stateMachine, context.interactState);

    protected bool TryOpenInventory() =>
        PlayerStateUtils.TryOpenInventory(context, stateMachine, context.menuState);

    protected bool TryJump() =>
        PlayerStateUtils.TryJump(context, stateMachine, context.jumpState);

    protected bool TryStartFalling() =>
        PlayerStateUtils.TryFall(context, stateMachine, context.fallingState);

    protected bool TryMove()
    {
        if (CanMove())
            return PlayerStateUtils.TryStartMovement(context, stateMachine, context.moveState);
        return false;
    }
    protected bool TryIdle() =>
        PlayerStateUtils.TryStartIdle(context, stateMachine, context.idleState);

    protected bool TrySprint() =>
        PlayerStateUtils.TryStartSprint(context, stateMachine, context.sprintState, context.idleState);
    protected bool CanMove() => !isTurningInPlace;

    #region Turn System

    // Suaviza os parâmetros do Animator
    protected void SmoothAnimatorTransition()
    {
        float currentX = context.animator.GetFloat(AnimatorParams.MoveX);
        float currentZ = context.animator.GetFloat(AnimatorParams.MoveZ);

        float newX = context.playerAnimatorController.SmoothToZero(currentX, 5f); // 5 é a velocidade de suavização
        float newZ = context.playerAnimatorController.SmoothToZero(currentZ, 5f);

        context.animator.SetFloat(AnimatorParams.MoveX, newX);
        context.animator.SetFloat(AnimatorParams.MoveZ, newZ);
    }
    protected void StartTurn(float direction)
    {
        isTurningInPlace = true;
        waitingToEnterTurnState = true; //enquanto waitingToEnterTurnState == true, o código é ignorado: if (!isTurningInPlace || waitingToEnterTurnState) return <- Linha em TickTurnRotation
        turnElapsed = 0f;

        startRotation = context.transform.rotation;
        targetRotation = startRotation * Quaternion.Euler(0f, direction * 90f, 0f);

        context.animator.SetBool("IsTurning", true);
        context.animator.SetFloat("TurnX", direction);

        Debug.Log(isTurningInPlace);
        Debug.Log($"Iniciou giro para {(direction > 0 ? "direita" : "esquerda")}");
    }
    protected void DetectTurnInPlace()
    {
        if (isTurningInPlace)
        {
            UpdateTurnAnimationState();
            return;
        }

        if (context.moveInput.sqrMagnitude > 0.05f) return; // Só gira parado

        if (context.turnRightInput)
        {
            StartTurn(1f);
        }
        else if (context.turnLeftInput)
        {
            StartTurn(-1f);
        }
    }
    protected void UpdateTurnAnimationState()
    {
        Debug.Log("Chamou UpdateTurnAnimationState");
        AnimatorStateInfo anim = context.animator.GetCurrentAnimatorStateInfo(0);
        if (waitingToEnterTurnState)
        {
            if (anim.IsTag("Turning"))
            {
                waitingToEnterTurnState = false;
                Debug.Log("Entrou na animação de giro");
            }
            return;
        }

        if (!anim.IsTag("Turning") || anim.normalizedTime >= 0.9f)
        {
            context.animator.SetBool("IsTurning", false);
            context.animator.SetFloat("TurnX", 0f);
            context.animator.applyRootMotion = false;
            isTurningInPlace = false;
            Debug.Log("Finalizou giro");
        }
    }
    protected void TickTurnRotation()
    {
        if (!isTurningInPlace || waitingToEnterTurnState) return;

        turnElapsed += Time.deltaTime;
        float t = Mathf.Clamp01(turnElapsed / turnDuration);
        context.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
    }
    #endregion

}

