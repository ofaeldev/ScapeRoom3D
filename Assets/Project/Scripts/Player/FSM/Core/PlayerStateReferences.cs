using UnityEngine;
using FSM;

public class PlayerStateReferences : MonoBehaviour
{
    [HideInInspector] public State<PlayerContext> idleState;
    [HideInInspector] public State<PlayerContext> moveState;
    [HideInInspector] public State<PlayerContext> jumpState;
    [HideInInspector] public State<PlayerContext> fallingState;
    [HideInInspector] public State<PlayerContext> interactState;
    [HideInInspector] public State<PlayerContext> menuState;
    [HideInInspector] public State<PlayerContext> sprintState;
}