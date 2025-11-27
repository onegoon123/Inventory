using UnityEngine;

namespace InventorySystem.Model
{
    [CreateAssetMenu(fileName = "New Recipe", menuName = "Inventory/Crafting Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        public ItemData inputA;
        public ItemData inputB;
        public ItemData result;
    }
}
