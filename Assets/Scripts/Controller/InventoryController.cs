using System.Collections.Generic;
using UnityEngine;
using InventorySystem.Model;

namespace InventorySystem.Controller
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private InventoryModel inventoryModel;
        [SerializeField] private List<CraftingRecipe> recipes;

        public void OnSlotDrop(int sourceIndex, int targetIndex)
        {
            if (sourceIndex == targetIndex) return;

            ItemData sourceItem = inventoryModel.GetItem(sourceIndex);
            ItemData targetItem = inventoryModel.GetItem(targetIndex);

            if (sourceItem == null) return;

            if (targetItem == null)
            {
                // Move
                inventoryModel.SetItem(targetIndex, sourceItem);
                inventoryModel.SetItem(sourceIndex, null);
            }
            else
            {
                // Check Recipe
                CraftingRecipe recipe = FindRecipe(sourceItem, targetItem);
                if (recipe != null)
                {
                    // Craft
                    inventoryModel.SetItem(sourceIndex, null);
                    inventoryModel.SetItem(targetIndex, recipe.result);
                    Debug.Log($"Crafted {recipe.result.itemName} from {sourceItem.itemName} and {targetItem.itemName}");
                }
                else
                {
                    // Swap
                    inventoryModel.SetItem(targetIndex, sourceItem);
                    inventoryModel.SetItem(sourceIndex, targetItem);
                }
            }
        }

        private CraftingRecipe FindRecipe(ItemData itemA, ItemData itemB)
        {
            foreach (var recipe in recipes)
            {
                if ((recipe.inputA == itemA && recipe.inputB == itemB) ||
                    (recipe.inputA == itemB && recipe.inputB == itemA))
                {
                    return recipe;
                }
            }
            return null;
        }
    }
}
