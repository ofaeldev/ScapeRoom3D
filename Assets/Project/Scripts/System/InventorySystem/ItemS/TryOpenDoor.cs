using UnityEngine;

public class TryOpenDoor : MonoBehaviour
{
    public InventoryItemSO itemRequired;
    public bool TryUseItem(InventoryItemInstance item)
    {
        if (item.data == itemRequired)
        {
            // Abre a porta
            return true; // Remove item
        }

        UIFeedback.Instance.ShowMessageFeedback("Falta o item certo.");
        return false;
    }
}
