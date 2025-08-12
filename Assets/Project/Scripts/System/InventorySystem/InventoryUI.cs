using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform gridParent;
    public GameObject panel;

    private void Start()
    {
        InventoryManager.Instance.OnInventoryChanged += Refresh;
        panel.SetActive(false);
    }

    private void Refresh()
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        foreach (var item in InventoryManager.Instance.Items)
        {
            GameObject slot = Instantiate(slotPrefab, gridParent);
            slot.GetComponent<ItemSlotUI>().Setup(item);
        }
    }

    public void ActiveUI()
    {
        if (panel != null)
        {
            panel.SetActive(!panel.activeSelf);
            if (panel.activeSelf) Refresh();
        }
    }
}
