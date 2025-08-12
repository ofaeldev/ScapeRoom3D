using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameText;
    public Button mainButton;
    public GameObject submenuPrefab;

    private GameObject currentSubmenu;
    private InventoryItemInstance currentItem;

    public void Setup(InventoryItemInstance item)
    {
        currentItem = item;
        icon.sprite = item.data.icon;
        nameText.text = item.data.itemName;

        mainButton.onClick.RemoveAllListeners(); // 🔥 evita múltiplos eventos
        mainButton.onClick.AddListener(OpenSubmenu);
    }

    void OpenSubmenu()
    {
        if (currentSubmenu != null)
        {
            Destroy(currentSubmenu);
            return;
        }

        currentSubmenu = Instantiate(submenuPrefab, transform.parent.parent);

        var submenuScript = currentSubmenu.GetComponent<ItemSubmenuUI>();
        if (submenuScript != null)
            submenuScript.Setup(currentItem);
        else
            Debug.LogWarning("ItemSubmenuUI não encontrado no prefab instanciado.");
    }
}

