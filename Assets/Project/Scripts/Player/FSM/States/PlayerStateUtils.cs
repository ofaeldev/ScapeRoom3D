// PlayerStateUtils.cs
using UnityEngine;
using FSM;

public static class PlayerStateUtils
{
    public static void DetectInteractable(PlayerContext context)
    {
        context.interactionDetector.DetectInteractable();
    }

    public static bool TryInteract(PlayerContext context, StateMachine<PlayerContext> stateMachine, State<PlayerContext> interactState)
    {
        if (context.interactInput && context.interactionDetector.HasInteractable)
        {
            stateMachine.ChangeState(interactState);
            return true;
        }
        return false;
    }

    public static bool TryOpenInventory(PlayerContext context, StateMachine<PlayerContext> stateMachine, State<PlayerContext> menuState)
    {
        if (context.inventoryInput)
        {
            stateMachine.ChangeState(menuState);
            return true;
        }
        return false;
    }
    public static bool TryJump(PlayerContext context, StateMachine<PlayerContext> stateMachine, State<PlayerContext> jumpState)
    {
        if (context.jumpInput)
        {
            stateMachine.ChangeState(jumpState);
            return true;
        }
        return false;
    }
    public static bool TryFall(PlayerContext context, StateMachine<PlayerContext> stateMachine, State<PlayerContext> fallingState)
    {
        if (context.isGrounded) return false;

        float fallDistance = context.lastGroundedY - context.transform.position.y;
        float verticalSpeed = context.rb.linearVelocity.y;

        bool isFallingFast = verticalSpeed < context.minFallSpeed;
        bool fellEnough = fallDistance > context.fallStartThreshold;
        bool beenInAir = context.timeSinceLeftGround > context.minFallTime;

        // Entra em queda se 2 ou mais critérios forem atendidos
        int conditions = 0;
        if (isFallingFast) conditions++;
        if (fellEnough) conditions++;
        if (beenInAir) conditions++;

        if (conditions >= 2)
        {
            stateMachine.ChangeState(fallingState);
            return true;
        }

        return false;
    }

    public static bool TryStartMovement(PlayerContext context, StateMachine<PlayerContext> stateMachine, State<PlayerContext> movementState)
    {
        if (context.moveInput.sqrMagnitude > 0.01f)
        {
            stateMachine.ChangeState(movementState);
            return true;
        }
            return false;
    }
    public static bool TryStartIdle(PlayerContext context, StateMachine<PlayerContext> stateMachine, State<PlayerContext> idleState)
    {
        if (context.moveInput.sqrMagnitude < 0.01f && context.isGrounded)
        {
            stateMachine.ChangeState(idleState);
            return true;
        }
        return false;
    }
    public static bool TryStartSprint(PlayerContext context, StateMachine<PlayerContext> stateMachine, State<PlayerContext> sprintState, State<PlayerContext> idleState)
    {
        if (context.sprintInput)
        {
            stateMachine.ChangeState(sprintState);
            return true;
        }
        if (!context.sprintInput && context.moveInput.sqrMagnitude < 0.1f)
        {
            stateMachine.ChangeState(idleState);
        }
        return false;
    }
}