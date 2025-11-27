using UnityEngine;
using InventorySystem.Model;
using InventorySystem.Controller;
using System.Collections.Generic;

namespace InventorySystem.View
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private InventoryModel inventoryModel;
        [SerializeField] private InventoryController inventoryController;
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private Transform gridContainer;

        private List<InventorySlot> slots = new List<InventorySlot>();

        private void Start()
        {
            InitializeGrid();
            inventoryModel.OnSlotChanged += UpdateSlot;
        }

        private void OnDestroy()
        {
            if (inventoryModel != null)
            {
                inventoryModel.OnSlotChanged -= UpdateSlot;
            }
        }

        private void InitializeGrid()
        {
            // Clear existing children if any
            foreach (Transform child in gridContainer)
            {
                Destroy(child.gameObject);
            }
            slots.Clear();

            for (int i = 0; i < inventoryModel.Capacity; i++)
            {
                GameObject slotObj = Instantiate(slotPrefab, gridContainer);
                InventorySlot slot = slotObj.GetComponent<InventorySlot>();
                slot.Initialize(i, inventoryController);
                slots.Add(slot);
            }
        }

        private void UpdateSlot(int index)
        {
            if (index >= 0 && index < slots.Count)
            {
                ItemData item = inventoryModel.GetItem(index);
                slots[index].SetItem(item);
            }
        }
    }
}
