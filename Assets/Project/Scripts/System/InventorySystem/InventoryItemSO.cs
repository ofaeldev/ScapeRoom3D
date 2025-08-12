using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class InventoryItemSO : ScriptableObject
{
    public string itemName;
    public bool isCombinable;
    public string description;
    public Sprite icon;
    public InventoryItemSO combineResult;
}
