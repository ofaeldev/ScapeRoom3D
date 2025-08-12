using System.Linq;
using UnityEngine;

public class InteractWithObject : MonoBehaviour, IInteractable, IItemReceiver
{
    [Header("Configuração da Porta")]
    public InventoryItemSO itemRequired;
    public Animator doorAnimator;
    public bool isOpen = false;

    // se true: só abre com o item; se false: abre mesmo sem item
    public bool requireItemToOpen = true;

    [Header("Feedback")]
    public string missingItemMessage = "Você precisa de um item para abrir esta porta.";

    // Nova flag para indicar se já foi desbloqueada
    private bool unlocked = false;

    public InteractionType InteractionType => InteractionType.InteractWithObject;
    public InteractionType PuzzleType => InteractionType.None;
    public bool StartsPuzzle => false;

    public void ExitInteract(PlayerContext context) { }

    public string GetInteractionPrompt()
    {
        if (unlocked || !requireItemToOpen)
            return isOpen ? "Fechar" : "Abrir";

        var inv = InventoryManager.Instance;
        if (inv != null && itemRequired != null &&
            inv.Items.Any(i => i.data == itemRequired))
        {
            return $"Usar {itemRequired.itemName}";
        }
        return "Usar item";
    }

    public void Interact(PlayerContext context)
    {
        // Se já desbloqueada, só abre/fecha
        if (unlocked || !requireItemToOpen)
        {
            ToggleDoor();
            return;
        }

        var inv = InventoryManager.Instance;
        if (inv == null)
        {
            Debug.LogWarning("InventoryManager.Instance is null.");
            return;
        }

        var selected = TryGetSelectedItemFromContext(context);
        if (selected != null)
        {
            var col = GetComponent<Collider>();
            if (col != null)
                inv.UseItem(selected, col);
            else if (TryUseItem(selected))
                inv.RemoveItem(selected);
            return;
        }

        if (itemRequired != null)
        {
            var itemInstance = inv.Items.FirstOrDefault(i => i.data == itemRequired);
            if (itemInstance != null)
            {
                var col = GetComponent<Collider>();
                if (col != null)
                    inv.UseItem(itemInstance, col);
                else if (TryUseItem(itemInstance))
                    inv.RemoveItem(itemInstance);
                return;
            }
        }

        UIFeedback.Instance.ShowMessageFeedback(missingItemMessage);
    }

    public bool TryUseItem(InventoryItemInstance item)
    {
        if (item != null && item.data == itemRequired)
        {
            unlocked = true; // Porta desbloqueada para sempre
            ToggleDoor();
            return true;
        }

        UIFeedback.Instance.ShowMessageFeedback(missingItemMessage);
        return false;
    }

    private void ToggleDoor()
    {
        isOpen = !isOpen;
        if (doorAnimator != null)
            doorAnimator.SetBool("IsOpen", isOpen);
        else
            Debug.Log($"Porta {(isOpen ? "abriu" : "fechou")} (sem Animator).");
    }

    private InventoryItemInstance TryGetSelectedItemFromContext(PlayerContext context)
    {
        if (context == null) return null;

        var t = context.GetType();
        string[] candidateNames = { "selectedItem", "currentItem", "heldItem", "equippedItem", "itemSelected" };

        foreach (var name in candidateNames)
        {
            var prop = t.GetProperty(name);
            if (prop != null && typeof(InventoryItemInstance).IsAssignableFrom(prop.PropertyType))
                return (InventoryItemInstance)prop.GetValue(context);

            var field = t.GetField(name);
            if (field != null && typeof(InventoryItemInstance).IsAssignableFrom(field.FieldType))
                return (InventoryItemInstance)field.GetValue(context);
        }
        return null;
    }
}
