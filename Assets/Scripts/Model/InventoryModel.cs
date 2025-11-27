using System;

namespace InventorySystem.Model
{
    /// <summary>
    /// 인벤토리의 데이터 모델 (MVC의 Model)
    /// 아이템 저장소와 데이터 변경 이벤트를 관리합니다
    /// </summary>
    public class InventoryModel
    {
        // 8x8 그리드 = 64개 슬롯
        private const int INVENTORY_SIZE = 64;
        
        // 아이템 저장 배열
        private ItemData[] items;
        
        // 이벤트: 특정 슬롯의 아이템이 변경되었을 때 (슬롯 인덱스, 새 아이템)
        public event Action<int, ItemData> OnItemChanged;
        
        // 이벤트: 두 슬롯의 아이템이 교환되었을 때 (슬롯A 인덱스, 슬롯B 인덱스)
        public event Action<int, int> OnItemsSwapped;
        
        /// <summary>
        /// 생성자: 인벤토리 초기화
        /// </summary>
        public InventoryModel()
        {
            items = new ItemData[INVENTORY_SIZE];
        }
        
        /// <summary>
        /// 특정 슬롯의 아이템 가져오기
        /// </summary>
        public ItemData GetItem(int slotIndex)
        {
            if (!IsValidSlotIndex(slotIndex)) return null;
            return items[slotIndex];
        }
        
        /// <summary>
        /// 특정 슬롯에 아이템 설정하기
        /// </summary>
        public void SetItem(int slotIndex, ItemData item)
        {
            if (!IsValidSlotIndex(slotIndex)) return;
            
            items[slotIndex] = item;
            OnItemChanged?.Invoke(slotIndex, item);
        }
        
        /// <summary>
        /// 빈 슬롯에 아이템 추가하기
        /// </summary>
        public bool AddItem(ItemData item)
        {
            for (int i = 0; i < INVENTORY_SIZE; i++)
            {
                if (items[i] == null)
                {
                    SetItem(i, item);
                    return true;
                }
            }
            return false; // 빈 슬롯 없음
        }
        
        /// <summary>
        /// 특정 슬롯의 아이템 제거하기
        /// </summary>
        public void RemoveItem(int slotIndex)
        {
            SetItem(slotIndex, null);
        }
        
        /// <summary>
        /// 두 슬롯의 아이템 위치 교환하기
        /// </summary>
        public void SwapItems(int slotA, int slotB)
        {
            if (!IsValidSlotIndex(slotA) || !IsValidSlotIndex(slotB)) return;
            
            ItemData temp = items[slotA];
            items[slotA] = items[slotB];
            items[slotB] = temp;
            
            // 각 슬롯 변경 이벤트 발생
            OnItemChanged?.Invoke(slotA, items[slotA]);
            OnItemChanged?.Invoke(slotB, items[slotB]);
            
            // 교환 이벤트 발생
            OnItemsSwapped?.Invoke(slotA, slotB);
        }
        
        /// <summary>
        /// 슬롯 인덱스가 유효한지 확인
        /// </summary>
        private bool IsValidSlotIndex(int index)
        {
            return index >= 0 && index < INVENTORY_SIZE;
        }
        
        /// <summary>
        /// 인벤토리 크기 가져오기
        /// </summary>
        public int GetInventorySize()
        {
            return INVENTORY_SIZE;
        }
    }
}
