using UnityEngine;
using System.Collections.Generic;
using InventorySystem.Model;
using InventorySystem.View;

namespace InventorySystem.Controller
{
    /// <summary>
    /// 인벤토리의 모든 로직을 관리하는 컨트롤러 (MVC의 Controller)
    /// Model과 View 사이를 중재하고 드래그앤드롭, 조합 등의 로직을 처리합니다
    /// </summary>
    public class InventoryController : MonoBehaviour
    {
        [Header("참조")]
        [SerializeField] private InventoryView inventoryView;
        
        [Header("조합 레시피")]
        [SerializeField] private List<CraftingRecipe> craftingRecipes;
        
        private InventoryModel inventoryModel;
        private CraftingSystem craftingSystem;
        
        private int currentDragSlotIndex = -1;
        
        private void Awake()
        {
            // Model 초기화
            inventoryModel = new InventoryModel();
            
            // Crafting System 초기화
            craftingSystem = new CraftingSystem();
            if (craftingRecipes != null && craftingRecipes.Count > 0)
            {
                craftingSystem.AddRecipes(craftingRecipes);
            }
            
            // View 초기화
            if (inventoryView != null)
            {
                inventoryView.Initialize(inventoryModel.GetInventorySize());
            }
        }
        
        private void Start()
        {
            // Model 이벤트 구독
            inventoryModel.OnItemChanged += HandleItemChanged;
            inventoryModel.OnItemsSwapped += HandleItemsSwapped;
            
            // View 이벤트 구독
            if (inventoryView != null)
            {
                inventoryView.OnSlotBeginDrag += HandleBeginDrag;
                inventoryView.OnSlotDrag += HandleDrag;
                inventoryView.OnSlotEndDrag += HandleEndDrag;
                inventoryView.OnSlotDrop += HandleDrop;
            }
            
            // 초기 UI 업데이트
            RefreshAllSlots();
        }
        
        private void OnDestroy()
        {
            // 이벤트 구독 해제
            if (inventoryModel != null)
            {
                inventoryModel.OnItemChanged -= HandleItemChanged;
                inventoryModel.OnItemsSwapped -= HandleItemsSwapped;
            }
            
            if (inventoryView != null)
            {
                inventoryView.OnSlotBeginDrag -= HandleBeginDrag;
                inventoryView.OnSlotDrag -= HandleDrag;
                inventoryView.OnSlotEndDrag -= HandleEndDrag;
                inventoryView.OnSlotDrop -= HandleDrop;
            }
        }
        
        #region Public API (외부에서 사용 가능한 메서드)
        
        /// <summary>
        /// 인벤토리에 아이템 추가
        /// </summary>
        public bool AddItem(ItemData item)
        {
            return inventoryModel.AddItem(item);
        }
        
        /// <summary>
        /// 특정 슬롯의 아이템 가져오기
        /// </summary>
        public ItemData GetItem(int slotIndex)
        {
            return inventoryModel.GetItem(slotIndex);
        }
        
        /// <summary>
        /// 특정 슬롯에 아이템 설정
        /// </summary>
        public void SetItem(int slotIndex, ItemData item)
        {
            inventoryModel.SetItem(slotIndex, item);
        }
        
        /// <summary>
        /// 특정 슬롯의 아이템 제거
        /// </summary>
        public void RemoveItem(int slotIndex)
        {
            inventoryModel.RemoveItem(slotIndex);
        }
        
        #endregion
        
        #region 드래그 앤 드롭 핸들러
        
        private void HandleBeginDrag(int slotIndex)
        {
            currentDragSlotIndex = slotIndex;
            
            ItemData item = inventoryModel.GetItem(slotIndex);
            if (item != null && item.icon != null && inventoryView != null)
            {
                inventoryView.ShowDragVisual(item.icon, Input.mousePosition);
            }
        }
        
        private void HandleDrag(int slotIndex, Vector2 position)
        {
            if (inventoryView != null)
            {
                inventoryView.UpdateDragVisualPosition(position);
            }
        }
        
        private void HandleEndDrag(int slotIndex)
        {
            if (inventoryView != null)
            {
                inventoryView.HideDragVisual();
            }
            
            currentDragSlotIndex = -1;
        }
        
        private void HandleDrop(int dropSlotIndex, int draggedSlotIndex)
        {
            // 같은 슬롯에 드롭하면 무시
            if (dropSlotIndex == draggedSlotIndex)
                return;
            
            ItemData draggedItem = inventoryModel.GetItem(draggedSlotIndex);
            ItemData dropTargetItem = inventoryModel.GetItem(dropSlotIndex);
            
            // 1. 먼저 조합 시도
            if (TryCraftItems(draggedSlotIndex, dropSlotIndex))
            {
                Debug.Log($"조합 성공: {draggedItem.itemName} + {dropTargetItem.itemName}");
                return;
            }
            
            // 2. 조합 실패 시 아이템 교환
            TrySwapItems(draggedSlotIndex, dropSlotIndex);
        }
        
        #endregion
        
        #region 아이템 교환 로직
        
        /// <summary>
        /// 두 슬롯의 아이템 교환 시도
        /// </summary>
        private bool TrySwapItems(int slotA, int slotB)
        {
            inventoryModel.SwapItems(slotA, slotB);
            return true;
        }
        
        #endregion
        
        #region 조합 로직
        
        /// <summary>
        /// 두 슬롯의 아이템 조합 시도
        /// </summary>
        private bool TryCraftItems(int slotA, int slotB)
        {
            ItemData itemA = inventoryModel.GetItem(slotA);
            ItemData itemB = inventoryModel.GetItem(slotB);
            
            // 둘 다 아이템이 있어야 함
            if (itemA == null || itemB == null)
                return false;
            
            // 레시피 찾기
            CraftingRecipe recipe = craftingSystem.FindRecipe(itemA, itemB);
            if (recipe == null)
                return false;
            
            // 조합 실행
            // 1. 두 재료 제거
            inventoryModel.RemoveItem(slotA);
            inventoryModel.RemoveItem(slotB);
            
            // 2. 결과 아이템을 드롭 대상 슬롯에 배치
            inventoryModel.SetItem(slotB, recipe.result);
            
            return true;
        }
        
        #endregion
        
        #region Model 이벤트 핸들러
        
        private void HandleItemChanged(int slotIndex, ItemData newItem)
        {
            // Model 변경사항을 View에 반영
            if (inventoryView != null)
            {
                inventoryView.UpdateSlot(slotIndex, newItem);
            }
        }
        
        private void HandleItemsSwapped(int slotA, int slotB)
        {
            // 교환 시 특별한 처리가 필요하면 여기에 추가
            // 현재는 OnItemChanged 이벤트로 각 슬롯이 개별 업데이트됨
        }
        
        #endregion
        
        #region 유틸리티
        
        /// <summary>
        /// 모든 슬롯 UI 새로고침
        /// </summary>
        private void RefreshAllSlots()
        {
            for (int i = 0; i < inventoryModel.GetInventorySize(); i++)
            {
                ItemData item = inventoryModel.GetItem(i);
                if (inventoryView != null)
                {
                    inventoryView.UpdateSlot(i, item);
                }
            }
        }
        
        #endregion
    }
}
