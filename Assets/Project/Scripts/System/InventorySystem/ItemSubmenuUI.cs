using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemSubmenuUI : MonoBehaviour
{
    public Button useButton;
    public Button combineButton;
    public Button examineButton;

    private InventoryItemInstance item;

    public void Setup(InventoryItemInstance newItem)
    {
        item = newItem;

        if (useButton != null)
        {
            useButton.onClick.RemoveAllListeners();
            useButton.onClick.AddListener(UseItem);
        }

        if (combineButton != null)
        {
            combineButton.onClick.RemoveAllListeners();
            combineButton.onClick.AddListener(Combine);
        }

        if (examineButton != null)
        {
            examineButton.onClick.RemoveAllListeners();
            examineButton.onClick.AddListener(Examine);
        }
    }


    private void UseItem()
    {
        var context = FindFirstObjectByType<PlayerContext>();
        var target = context?.interactionDetector?.CurrentTarget;

        if (target != null)
        {
            Collider col = null;

            // Garante que o target seja um Collider ou tenha um
            if (target is Collider directCol)
                col = directCol;
            else if (target is Component comp)
                col = comp.GetComponent<Collider>();

            if (col != null)
            {
                InventoryManager.Instance.UseItem(item, col);
            }
            else
            {
                Debug.LogWarning("Nenhum collider encontrado no alvo para usar o item.");
            }
        }
        else
        {
            UIFeedback.Instance.ShowMessageFeedback("Nenhum alvo para usar o item.");
        }

        Destroy(gameObject); // Fecha o submenu
    }

    private void Combine()
    {
        // Lógica de combinação de itens se você quiser
        UIFeedback.Instance.ShowMessageFeedback("Função de combinar ainda não implementada.");
    }

    private void Examine()
    {
        if (item != null && item.data != null)
        {
            UIFeedback.Instance.ShowDescription(item.data.description);
        }
        Destroy(gameObject);
    }

}
