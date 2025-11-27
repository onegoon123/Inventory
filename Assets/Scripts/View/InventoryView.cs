using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using InventorySystem.Model;

namespace InventorySystem.View
{
    /// <summary>
    /// 인벤토리 전체 UI를 관리하는 뷰 (MVC의 View)
    /// 8x8 그리드를 생성하고 Model의 변경사항을 UI에 반영합니다
    /// </summary>
    public class InventoryView : MonoBehaviour
    {
        [Header("UI 설정")]
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private Transform gridContainer;
        [SerializeField] private Canvas canvas;
        
        [Header("드래그 비주얼")]
        [SerializeField] private GameObject dragVisualPrefab;
        
        private List<InventorySlotView> slots = new List<InventorySlotView>();
        private DragVisual dragVisual;
        
        // Controller에 전달할 이벤트
        public event Action<int> OnSlotBeginDrag;
        public event Action<int, Vector2> OnSlotDrag;
        public event Action<int> OnSlotEndDrag;
        public event Action<int, int> OnSlotDrop; // (드롭 대상 슬롯, 드래그된 슬롯)
        
        /// <summary>
        /// 8x8 그리드 초기화
        /// </summary>
        public void Initialize(int gridSize = 64)
        {
            // 기존 슬롯 제거
            foreach (var slot in slots)
            {
                if (slot != null)
                    Destroy(slot.gameObject);
            }
            slots.Clear();
            
            // 8x8 = 64개 슬롯 생성
            for (int i = 0; i < gridSize; i++)
            {
                CreateSlot(i);
            }
            
            // 드래그 비주얼 생성
            CreateDragVisual();
        }
        
        /// <summary>
        /// 슬롯 생성
        /// </summary>
        private void CreateSlot(int index)
        {
            GameObject slotObj = Instantiate(slotPrefab, gridContainer);
            InventorySlotView slotView = slotObj.GetComponent<InventorySlotView>();
            
            if (slotView != null)
            {
                slotView.Initialize(index);
                
                // 슬롯 이벤트 구독
                slotView.OnBeginDragEvent += HandleSlotBeginDrag;
                slotView.OnDragEvent += HandleSlotDrag;
                slotView.OnEndDragEvent += HandleSlotEndDrag;
                slotView.OnDropEvent += HandleSlotDrop;
                
                slots.Add(slotView);
            }
        }
        
        /// <summary>
        /// 드래그 비주얼 생성
        /// </summary>
        private void CreateDragVisual()
        {
            if (dragVisualPrefab != null && canvas != null)
            {
                GameObject dragObj = Instantiate(dragVisualPrefab, canvas.transform);
                dragVisual = dragObj.GetComponent<DragVisual>();
                if (dragVisual != null)
                {
                    dragVisual.Setup(null, canvas);
                    dragVisual.SetVisible(false);
                }
            }
        }
        
        /// <summary>
        /// 특정 슬롯의 아이템 표시 업데이트
        /// </summary>
        public void UpdateSlot(int slotIndex, ItemData item)
        {
            if (slotIndex >= 0 && slotIndex < slots.Count)
            {
                slots[slotIndex].UpdateSlot(item);
            }
        }
        
        /// <summary>
        /// 드래그 비주얼 표시
        /// </summary>
        public void ShowDragVisual(Sprite icon, Vector2 position)
        {
            if (dragVisual != null)
            {
                dragVisual.Setup(icon, canvas);
                dragVisual.UpdatePosition(position);
                dragVisual.SetVisible(true);
            }
        }
        
        /// <summary>
        /// 드래그 비주얼 위치 업데이트
        /// </summary>
        public void UpdateDragVisualPosition(Vector2 position)
        {
            if (dragVisual != null)
            {
                dragVisual.UpdatePosition(position);
            }
        }
        
        /// <summary>
        /// 드래그 비주얼 숨김
        /// </summary>
        public void HideDragVisual()
        {
            if (dragVisual != null)
            {
                dragVisual.SetVisible(false);
            }
        }
        
        /// <summary>
        /// 슬롯 강조 표시
        /// </summary>
        public void HighlightSlot(int slotIndex, bool highlight)
        {
            if (slotIndex >= 0 && slotIndex < slots.Count)
            {
                slots[slotIndex].SetHighlight(highlight);
            }
        }
        
        #region 슬롯 이벤트 핸들러
        
        private void HandleSlotBeginDrag(int slotIndex)
        {
            OnSlotBeginDrag?.Invoke(slotIndex);
        }
        
        private void HandleSlotDrag(int slotIndex, Vector2 position)
        {
            OnSlotDrag?.Invoke(slotIndex, position);
        }
        
        private void HandleSlotEndDrag(int slotIndex)
        {
            OnSlotEndDrag?.Invoke(slotIndex);
        }
        
        private void HandleSlotDrop(int dropSlotIndex, int draggedSlotIndex)
        {
            OnSlotDrop?.Invoke(dropSlotIndex, draggedSlotIndex);
        }
        
        #endregion
        
        private void OnDestroy()
        {
            // 이벤트 구독 해제
            foreach (var slot in slots)
            {
                if (slot != null)
                {
                    slot.OnBeginDragEvent -= HandleSlotBeginDrag;
                    slot.OnDragEvent -= HandleSlotDrag;
                    slot.OnEndDragEvent -= HandleSlotEndDrag;
                    slot.OnDropEvent -= HandleSlotDrop;
                }
            }
        }
    }
}
