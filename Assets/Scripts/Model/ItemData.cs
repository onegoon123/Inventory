using UnityEngine;

namespace InventorySystem.Model
{
    public enum ItemType
    {
        Consumable,
        Equipment,
        Material,
        Quest
    }

    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
    public class ItemData : ScriptableObject
    {
        public string itemName;
        public Sprite icon;
        public ItemType type;
    }
}
