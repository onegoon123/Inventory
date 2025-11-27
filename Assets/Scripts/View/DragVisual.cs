using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.View
{
    /// <summary>
    /// 드래그 중에 표시되는 아이템 아이콘 (MVC의 View)
    /// 마우스 커서를 따라다니며 시각적 피드백을 제공합니다
    /// </summary>
    public class DragVisual : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private CanvasGroup canvasGroup;
        
        private RectTransform rectTransform;
        private Canvas parentCanvas;
        
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            
            // Canvas가 없으면 추가
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            
            // 레이캐스트 차단 방지 (드래그 중 다른 UI 요소와 상호작용 가능하도록)
            canvasGroup.blocksRaycasts = false;
            
            // 투명도 설정
            canvasGroup.alpha = 0.6f;
        }
        
        /// <summary>
        /// 드래그 비주얼 설정
        /// </summary>
        public void Setup(Sprite icon, Canvas canvas)
        {
            if (iconImage != null)
            {
                iconImage.sprite = icon;
            }
            
            parentCanvas = canvas;
            
            // Canvas 자식으로 설정하여 최상위에 표시
            transform.SetParent(canvas.transform, false);
            transform.SetAsLastSibling();
        }
        
        /// <summary>
        /// 위치 업데이트 (마우스 위치 추적)
        /// </summary>
        public void UpdatePosition(Vector2 screenPosition)
        {
            if (parentCanvas == null) return;
            
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.transform as RectTransform,
                screenPosition,
                parentCanvas.worldCamera,
                out localPoint
            );
            
            rectTransform.localPosition = localPoint;
        }
        
        /// <summary>
        /// 표시/숨김
        /// </summary>
        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}
