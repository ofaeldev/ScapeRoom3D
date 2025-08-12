using FSM;
using System;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

[CreateAssetMenu(menuName = "FSM/States/Idle")]
public class IdleState : BasePlayerState
{
    public override void Enter()
    {
        Debug.Log("Entrou no estado Idle");
    }

    public override void Tick()
    {
        DetectInteractable();
        HandleInputs();
    }

    public override void FixedTick()
    {
        SmoothAnimatorTransition(); 
        TickTurnRotation(); //< -só cuida do giro físico
    }

    public override void Exit()
    {
        Debug.Log("Saiu do estado Idle");
    }
    private void HandleInputs()
    {

        if (TryInteract()) return;
        if (TryOpenInventory()) return;
        if (TryMove()) return;
        if (TrySprint()) return;
        if (TryJump()) return;
        if (TryStartFalling()) return;
        
        DetectTurnInPlace(); // <- chama StartTurn()
            // Reinicia inputs de giro após detectar
        context.turnLeftInput = false;
        context.turnRightInput = false;


    }
}