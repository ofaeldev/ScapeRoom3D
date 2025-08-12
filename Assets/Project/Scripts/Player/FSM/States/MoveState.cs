using FSM;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/States/Move")]
public class MoveState : BasePlayerState
{
    private PlayerStateReferences _states;
    public float moveSpeed;

    public override void Enter()
    {
        Debug.Log("Entrou no estado Move");
        _states = context.GetComponent<PlayerStateReferences>();
        if (_states == null)
            Debug.LogError("PlayerStateReferences não encontrado no contexto!");
    }

    public override void Tick()
    {
        DetectInteractable();
        HandleInputs();
    }
    public override void FixedTick()
    {
        context.GetMovement(context.moveInput, moveSpeed);
    }    

    public override void Exit()
    {
    }
    public void HandleInputs()
    {
        if (TryInteract()) return;
        if (TryOpenInventory()) return;
        if (TrySprint()) return;
        if (TryJump()) return;
        if (TryStartFalling()) return;
    }
}