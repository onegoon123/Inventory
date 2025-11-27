using System;
using UnityEngine;

namespace InventorySystem.Model
{
    public class InventoryModel : MonoBehaviour
    {
        private ItemData[] items;
        public int Capacity { get; private set; } = 64; // 8x8 grid

        public event Action<int> OnSlotChanged;

        private void Awake()
        {
            items = new ItemData[Capacity];
        }

        public ItemData GetItem(int index)
        {
            if (IsIndexValid(index))
            {
                return items[index];
            }
            return null;
        }

        public void SetItem(int index, ItemData item)
        {
            if (IsIndexValid(index))
            {
                items[index] = item;
                OnSlotChanged?.Invoke(index);
            }
        }

        private bool IsIndexValid(int index)
        {
            return index >= 0 && index < Capacity;
        }
    }
}
