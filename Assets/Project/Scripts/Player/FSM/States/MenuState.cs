using FSM;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/States/MenuState")]
public class MenuState : State<PlayerContext>
{
    public override void Enter()
    {
        Debug.Log("Menu State");
        context.inventoryUI.ActiveUI();
        context.playerAnimatorController.StopMovement();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public override void Tick()
    {
        if (context.inventoryInput)
        {
            context.inventoryUI.ActiveUI();
            if (context.inventoryUI != null && context.inventoryUI.panel != null && !context.inventoryUI.panel.activeSelf)
            {
                stateMachine.ChangeState(context.idleState);
            }
        }
    }

    public override void Exit()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UIFeedback.Instance.descriptionText.text = "";
    }
}
