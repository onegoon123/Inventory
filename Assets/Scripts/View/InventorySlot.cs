using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using InventorySystem.Model;
using InventorySystem.Controller;

namespace InventorySystem.View
{
    public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        [SerializeField] private Image iconImage;
        
        private int slotIndex;
        private InventoryController controller;
        private static InventorySlot draggedSlot;
        private static GameObject dragVisual;
        private Canvas canvas;

        public void Initialize(int index, InventoryController ctrl)
        {
            slotIndex = index;
            controller = ctrl;
            canvas = GetComponentInParent<Canvas>();
        }

        public void SetItem(ItemData item)
        {
            if (item != null)
            {
                iconImage.sprite = item.icon;
                iconImage.enabled = true;
            }
            else
            {
                iconImage.sprite = null;
                iconImage.enabled = false;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (iconImage.sprite == null) return;

            draggedSlot = this;
            
            // Create visual representation for dragging
            dragVisual = new GameObject("DragVisual");
            dragVisual.transform.SetParent(canvas.transform, false);
            dragVisual.transform.SetAsLastSibling();
            
            Image img = dragVisual.AddComponent<Image>();
            img.sprite = iconImage.sprite;
            img.raycastTarget = false;
            
            RectTransform rect = dragVisual.GetComponent<RectTransform>();
            rect.sizeDelta = GetComponent<RectTransform>().sizeDelta;
            rect.position = transform.position;

            // Hide original icon while dragging
            Color c = iconImage.color;
            c.a = 0.5f;
            iconImage.color = c;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (dragVisual != null)
            {
                dragVisual.transform.position = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (dragVisual != null)
            {
                Destroy(dragVisual);
            }
            
            draggedSlot = null;
            
            // Restore original icon opacity
            Color c = iconImage.color;
            c.a = 1f;
            iconImage.color = c;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (draggedSlot != null)
            {
                controller.OnSlotDrop(draggedSlot.slotIndex, slotIndex);
            }
        }
    }
}
