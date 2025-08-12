using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public InventoryItemInstance ItemInUse { get; private set; }

    public event Action OnInventoryChanged;

    private List<InventoryItemInstance> items = new List<InventoryItemInstance>();
    public IReadOnlyList<InventoryItemInstance> Items => items.AsReadOnly();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(InventoryItemSO itemData)
    {
        items.Add(new InventoryItemInstance(itemData));
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(InventoryItemInstance item)
    {
        items.Remove(item);
        OnInventoryChanged?.Invoke();
    }

    public void UseItem(InventoryItemInstance item, Collider targetCollider)
    {
        var receiver = targetCollider.GetComponent<IItemReceiver>();
        if (receiver != null && receiver.TryUseItem(item))
        {
            RemoveItem(item);
        }
        else
        {
            UIFeedback.Instance.ShowMessageFeedback("It is not possible to use this item.");
        }
    }

    public bool TryCombineItems(InventoryItemInstance itemA, InventoryItemInstance itemB)
    {
        if (itemA.data.isCombinable && itemB.data.isCombinable &&
            itemA.data.combineResult != null && itemA.data.combineResult == itemB.data.combineResult)
        {
            RemoveItem(itemA);
            RemoveItem(itemB);
            AddItem(itemA.data.combineResult);
            return true;
        }
        return false;
    }
}
