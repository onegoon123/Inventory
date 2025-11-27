using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using InventorySystem.Model;

namespace InventorySystem.View
{
    /// <summary>
    /// 개별 인벤토리 슬롯의 UI 표현 (MVC의 View)
    /// 드래그 앤 드롭 이벤트를 처리하고 슬롯 상태를 표시합니다
    /// </summary>
    public class InventorySlotView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        [Header("UI 컴포넌트")]
        [SerializeField] private Image iconImage;
        [SerializeField] private Image backgroundImage;
        
        [Header("시각적 설정")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color highlightColor = Color.yellow;
        [SerializeField] private Color emptySlotColor = new Color(1f, 1f, 1f, 0.3f);
        
        private int slotIndex;
        private ItemData currentItem;
        
        // 드래그 앤 드롭 이벤트
        public event Action<int> OnBeginDragEvent;
        public event Action<int, Vector2> OnDragEvent;
        public event Action<int> OnEndDragEvent;
        public event Action<int, int> OnDropEvent; // (드롭된 슬롯, 드래그된 슬롯)
        
        /// <summary>
        /// 슬롯 초기화
        /// </summary>
        public void Initialize(int index)
        {
            slotIndex = index;
            UpdateSlot(null);
        }
        
        /// <summary>
        /// 슬롯에 표시할 아이템 업데이트
        /// </summary>
        public void UpdateSlot(ItemData item)
        {
            currentItem = item;
            
            if (item != null && item.icon != null)
            {
                iconImage.sprite = item.icon;
                iconImage.color = Color.white;
                iconImage.enabled = true;
            }
            else
            {
                iconImage.sprite = null;
                iconImage.enabled = false;
            }
            
            // 배경색 조정
            if (backgroundImage != null)
            {
                backgroundImage.color = item != null ? normalColor : emptySlotColor;
            }
        }
        
        /// <summary>
        /// 슬롯 강조 표시 설정 (드롭 대상 표시용)
        /// </summary>
        public void SetHighlight(bool highlight)
        {
            if (backgroundImage != null)
            {
                backgroundImage.color = highlight ? highlightColor : 
                    (currentItem != null ? normalColor : emptySlotColor);
            }
        }
        
        /// <summary>
        /// 현재 슬롯이 비어있는지 확인
        /// </summary>
        public bool IsEmpty()
        {
            return currentItem == null;
        }
        
        /// <summary>
        /// 현재 아이템 가져오기
        /// </summary>
        public ItemData GetItem()
        {
            return currentItem;
        }
        
        /// <summary>
        /// 슬롯 인덱스 가져오기
        /// </summary>
        public int GetSlotIndex()
        {
            return slotIndex;
        }
        
        #region 드래그 앤 드롭 인터페이스 구현
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (currentItem != null)
            {
                OnBeginDragEvent?.Invoke(slotIndex);
            }
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (currentItem != null)
            {
                OnDragEvent?.Invoke(slotIndex, eventData.position);
            }
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            if (currentItem != null)
            {
                OnEndDragEvent?.Invoke(slotIndex);
            }
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            // 드래그된 슬롯 찾기
            GameObject draggedObject = eventData.pointerDrag;
            if (draggedObject != null)
            {
                InventorySlotView draggedSlot = draggedObject.GetComponent<InventorySlotView>();
                if (draggedSlot != null)
                {
                    OnDropEvent?.Invoke(slotIndex, draggedSlot.GetSlotIndex());
                }
            }
        }
        
        #endregion
    }
}
