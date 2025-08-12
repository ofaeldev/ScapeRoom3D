using FSM;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/States/SprintState")]
public class SprintState : BasePlayerState
{
    [SerializeField] private float sprintSpeed;
    public override void Enter()
    {
        Debug.Log("Enter Sprint State");
    }

    public override void Tick()
    {
        DetectInteractable();
        HandleInputs();
    }

    public override void FixedTick()
    {
        context.GetMovement(context.moveInput, sprintSpeed);
    }
    public void HandleInputs()
    {
        if (TryInteract()) return;
        if (TryOpenInventory()) return;
        if (TrySprint()) return;
        if (TryJump()) return;
    }

    public override void Exit()
    {

    }
}