using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private InventoryItemSO itemData;
    [SerializeField] private float waitAnimation = 1f;

    public InteractionType InteractionType => InteractionType.PickupItem;
    public InteractionType PuzzleType => InteractionType.None;
    public bool StartsPuzzle => false;

    public void Interact(PlayerContext context)
    {
        if (itemData == null)
        {
            Debug.LogWarning("ItemPickup: itemData não atribuído!", this);
            return;
        }

        // Mostra o cursor do mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        InventoryManager.Instance.AddItem(itemData);

        context.playerAnimatorController.SetInteracting(2); // por ex: trigger de "pegar"
        context.inputBlocker.Block();

        StartCoroutine(ExitInteraction(context));
    }

    public void ExitInteract(PlayerContext context)
    {
        // Nada a fazer aqui por enquanto
    }

    public string GetInteractionPrompt()
    {
        return "Pegar " + itemData.itemName;
    }

    private IEnumerator ExitInteraction(PlayerContext context)
    {
        yield return new WaitForSeconds(waitAnimation);

        context.playerAnimatorController.SetInteracting(0);
        context.inputBlocker.Unblock();
        context.ChangeState(context.idleState);

        yield return new WaitForSeconds(0.2f);

        Destroy(gameObject); // Remove o item do cenário
    }
}
