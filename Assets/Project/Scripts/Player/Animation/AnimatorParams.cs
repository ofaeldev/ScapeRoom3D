using UnityEngine;

public static class AnimatorParams
{
    public static readonly int MoveX = Animator.StringToHash("MoveX");
    public static readonly int MoveZ = Animator.StringToHash("MoveZ");
    public static readonly int TurnAmount = Animator.StringToHash("TurnAmount");
    public static readonly int IsMove = Animator.StringToHash("IsMove");
    public static readonly int IsTurning = Animator.StringToHash("IsTurning");

    public static readonly int IsJumping = Animator.StringToHash("IsJumping"); 
    public static readonly int JumpDirections = Animator.StringToHash("JumpDirection");
    public static readonly int JumpTrigger = Animator.StringToHash("Jump");
    public static readonly int IsFalling = Animator.StringToHash("IsFalling");

    public static readonly int IsInteracting = Animator.StringToHash("IsInteracting");
}
