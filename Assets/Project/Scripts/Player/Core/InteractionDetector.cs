using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    [Header("Detecção")]
    [SerializeField] private Transform detectionOrigin;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private float detectionDistance = 2.5f;

    private Collider currentTargetCollider;
    private IInteractable currentTarget;

    [Header("Debug")]
    [SerializeField] private bool debugRay = true;

    public IInteractable CurrentTarget => currentTarget;
    public bool HasInteractable => currentTarget != null;
    public Collider CurrentTargetCollider => currentTargetCollider;

    private void Update()
    {
        DetectInteractable();
    }

    public Collider GetCurrentCollider()
    {
        return currentTargetCollider;
    }

    public void DetectInteractable()
    {
        // Armazena o anterior (para evento futuro se quiser)
        var previousTarget = currentTarget;
        currentTarget = null;
        currentTargetCollider = null;

        if (Physics.SphereCast(detectionOrigin.position, 0.3f, detectionOrigin.forward, out RaycastHit hit, detectionDistance, interactableLayer))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                currentTarget = interactable;
                currentTargetCollider = hit.collider;
            }
        }

        if (debugRay)
            DrawDebugRay();

        // disparar evento se o alvo mudou
        // if (previousTarget != currentTarget)
        //     OnTargetChanged?.Invoke(currentTarget);
    }
    private void DrawDebugRay()
    {
        Color rayColor = currentTarget != null ? Color.green : Color.red;
        Debug.DrawRay(detectionOrigin.position, detectionOrigin.forward * detectionDistance, rayColor);
    }
}
