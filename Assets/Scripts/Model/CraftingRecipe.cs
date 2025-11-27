using UnityEngine;

namespace InventorySystem.Model
{
    /// <summary>
    /// 아이템 조합 레시피를 정의하는 ScriptableObject
    /// A + B = C 형식의 조합을 저장합니다
    /// </summary>
    [CreateAssetMenu(fileName = "New Recipe", menuName = "Inventory/Crafting Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        [Header("조합 재료")]
        [Tooltip("첫 번째 필수 재료")]
        public ItemData ingredientA;
        
        [Tooltip("두 번째 필수 재료")]
        public ItemData ingredientB;
        
        [Header("조합 결과")]
        [Tooltip("조합으로 생성되는 아이템")]
        public ItemData result;
        
        /// <summary>
        /// 두 아이템이 이 레시피와 일치하는지 확인 (순서 무관)
        /// </summary>
        public bool Matches(ItemData item1, ItemData item2)
        {
            if (item1 == null || item2 == null) return false;
            
            return (item1 == ingredientA && item2 == ingredientB) ||
                   (item1 == ingredientB && item2 == ingredientA);
        }
    }
}
