using FSM;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/States/Jumping")]
public class JumpingState : BasePlayerState
{
    public JumpForceSettingsSO forceSettings;

    public override void Enter()
    {
        float jumpDirZ = UpdateJump();

        context.playerAnimatorController.SetJumpingDirection1D(jumpDirZ);
        context.playerAnimatorController.TriggerJump();
    }


    public override void Tick()
    {
        if (TryStartFalling()) return;
    }

    public override void Exit()
    {
        Debug.Log("Saiu estado jump");
        context.playerAnimatorController.SetJumping(false);
        context.animator.ResetTrigger(AnimatorParams.JumpTrigger);
        context.playerAnimatorController.SetFalling(true); // Ativa estado de queda no Animator
    }
    private float UpdateJump()
    {
        context.animator.applyRootMotion = false;
        Debug.Log("Entrou no estado Jump");
        context.playerAnimatorController.SetJumping(true);

        // Define direção do pulo (eixo Z)
        float jumpDirZ = Mathf.Clamp(context.moveInput.y, -1f, 1f);
        Vector3 jumpDir = context.transform.forward * jumpDirZ;

        // Decide a força com base se é parado ou direcional
        float upward = Mathf.Abs(jumpDirZ) > 0.1f
            ? forceSettings.directionalUpwardForce
            : forceSettings.upwardForce;

        float directional = Mathf.Abs(jumpDirZ) > 0.1f
            ? forceSettings.directionalForce
            : 0f;

        Vector3 force = Vector3.up * upward + jumpDir * directional;

        context.rb.linearVelocity = Vector3.zero;
        context.rb.AddForce(force, ForceMode.Impulse);
        context.animator.applyRootMotion = false;
        return jumpDirZ;
    }
}
