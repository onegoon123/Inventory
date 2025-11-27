using UnityEngine;

namespace InventorySystem.Model
{
    /// <summary>
    /// 아이템 데이터를 정의하는 ScriptableObject
    /// 아이템의 이름, 아이콘, 타입을 저장합니다
    /// </summary>
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
    public class ItemData : ScriptableObject
    {
        [Header("아이템 정보")]
        [Tooltip("아이템의 표시 이름")]
        public string itemName;
        
        [Tooltip("아이템의 아이콘 이미지")]
        public Sprite icon;
        
        [Tooltip("아이템의 카테고리")]
        public ItemType itemType;
    }
}
