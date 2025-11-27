using UnityEngine;
using InventorySystem.Model;
using System.Collections.Generic;

namespace InventorySystem.Test
{
    public class InventoryTester : MonoBehaviour
    {
        [SerializeField] private InventoryModel inventoryModel;
        [SerializeField] private List<ItemData> testItems;

        private void Start()
        {
            // Add some random items to the inventory for testing
            if (testItems != null && testItems.Count > 0)
            {
                for (int i = 0; i < Mathf.Min(testItems.Count, inventoryModel.Capacity); i++)
                {
                    inventoryModel.SetItem(i, testItems[i]);
                }
            }
        }
    }
}
