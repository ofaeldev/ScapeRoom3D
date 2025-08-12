using System;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float dampTime = 0.1f;

    public void SetMovement(Vector2 move)
    {
        _animator.SetFloat(AnimatorParams.MoveX, move.x, dampTime, Time.deltaTime);
        _animator.SetFloat(AnimatorParams.MoveZ, move.y, dampTime, Time.deltaTime);
    }
    public void SetTurnAmount(float turn)
    {
        _animator.SetFloat(AnimatorParams.TurnAmount, turn, dampTime, Time.deltaTime);
    }
    public void SetJumpingDirection1D(float direction)
    {
        _animator.SetFloat(AnimatorParams.JumpDirections, direction);
    }
    public void TriggerJump()
    {
        _animator.SetTrigger(AnimatorParams.JumpTrigger);
    }
    public float SmoothToZero(float current, float smoothSpeed)
    {
        if (Mathf.Abs(current) < 0.01f)
            return 0f; // já é praticamente zero

        return Mathf.MoveTowards(current, 0f, smoothSpeed * Time.deltaTime);
    }
    public void SetInteracting(int value)
    {
        _animator.SetInteger(AnimatorParams.IsInteracting, value);
    }
    public void SetJumping(bool isJumping)
    {
        _animator.SetBool(AnimatorParams.IsJumping, isJumping);
    }
    public void SetFalling(bool isFalling)
    {
        _animator.SetBool(AnimatorParams.IsFalling, isFalling);
    }
    public void SetTurning(bool isTurning)
    {
        _animator.SetBool(AnimatorParams.IsTurning, isTurning);
    }
    public void StopMovement()
    {
        _animator.SetFloat(AnimatorParams.MoveX, 0f);
        _animator.SetFloat(AnimatorParams.MoveZ, 0f);
    }
    public void CrossFadeTo(string stateName, float fadeDuration = 0.1f)
    {
        _animator.CrossFade(stateName, fadeDuration);
    }
}
