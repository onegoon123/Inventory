using UnityEngine;
using InventorySystem.Controller;
using InventorySystem.Model;

namespace InventorySystem
{
    /// <summary>
    /// 인벤토리 테스트용 헬퍼 스크립트
    /// 게임 시작 시 인벤토리에 테스트 아이템을 추가합니다
    /// </summary>
    public class InventoryTester : MonoBehaviour
    {
        [Header("참조")]
        [SerializeField] private InventoryController inventoryController;
        
        [Header("테스트 아이템")]
        [SerializeField] private ItemData[] testItems;
        
        [Header("설정")]
        [SerializeField] private bool addItemsOnStart = true;
        
        private void Start()
        {
            if (addItemsOnStart && inventoryController != null && testItems != null)
            {
                AddTestItems();
            }
        }
        
        /// <summary>
        /// 테스트 아이템들을 인벤토리에 추가
        /// </summary>
        private void AddTestItems()
        {
            foreach (var item in testItems)
            {
                if (item != null)
                {
                    bool success = inventoryController.AddItem(item);
                    if (success)
                    {
                        Debug.Log($"테스트 아이템 추가됨: {item.itemName}");
                    }
                    else
                    {
                        Debug.LogWarning($"테스트 아이템 추가 실패 (인벤토리 가득 찼음): {item.itemName}");
                    }
                }
            }
        }
        
        /// <summary>
        /// 키보드 입력으로 수동 테스트
        /// </summary>
        private void Update()
        {
            // T 키: 테스트 아이템 추가
            if (Input.GetKeyDown(KeyCode.T))
            {
                AddTestItems();
            }
            
            // C 키: 인벤토리 비우기
            if (Input.GetKeyDown(KeyCode.C))
            {
                ClearInventory();
            }
        }
        
        /// <summary>
        /// 인벤토리의 모든 아이템 제거
        /// </summary>
        private void ClearInventory()
        {
            if (inventoryController != null)
            {
                for (int i = 0; i < 64; i++)
                {
                    inventoryController.RemoveItem(i);
                }
                Debug.Log("인벤토리가 비워졌습니다.");
            }
        }
    }
}
