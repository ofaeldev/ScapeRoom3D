using UnityEngine;

[CreateAssetMenu(menuName = "Movement/JumpForceSettings")]
public class JumpForceSettingsSO : ScriptableObject
{
    [Header("Pulo Vertical (Parado)")]
    public float upwardForce = 5f;

    [Header("Pulo Direcional (Frente/Trás)")]
    public float directionalUpwardForce = 4f;
    public float directionalForce = 3f;
}
