using System.Collections.Generic;
using InventorySystem.Model;

namespace InventorySystem.Controller
{
    /// <summary>
    /// 조합 시스템을 관리하는 클래스 (MVC의 Controller)
    /// 조합 레시피를 관리하고 조합 가능 여부를 확인합니다
    /// </summary>
    public class CraftingSystem
    {
        private List<CraftingRecipe> recipes;
        
        /// <summary>
        /// 생성자
        /// </summary>
        public CraftingSystem()
        {
            recipes = new List<CraftingRecipe>();
        }
        
        /// <summary>
        /// 레시피 추가
        /// </summary>
        public void AddRecipe(CraftingRecipe recipe)
        {
            if (recipe != null && !recipes.Contains(recipe))
            {
                recipes.Add(recipe);
            }
        }
        
        /// <summary>
        /// 여러 레시피 추가
        /// </summary>
        public void AddRecipes(List<CraftingRecipe> newRecipes)
        {
            foreach (var recipe in newRecipes)
            {
                AddRecipe(recipe);
            }
        }
        
        /// <summary>
        /// 두 아이템으로 조합 가능한 레시피 찾기 (순서 무관)
        /// </summary>
        public CraftingRecipe FindRecipe(ItemData itemA, ItemData itemB)
        {
            if (itemA == null || itemB == null)
                return null;
            
            foreach (var recipe in recipes)
            {
                if (recipe.Matches(itemA, itemB))
                {
                    return recipe;
                }
            }
            
            return null;
        }
        
        /// <summary>
        /// 조합 가능한지 확인
        /// </summary>
        public bool CanCraft(ItemData itemA, ItemData itemB)
        {
            return FindRecipe(itemA, itemB) != null;
        }
        
        /// <summary>
        /// 레시피 목록 가져오기
        /// </summary>
        public List<CraftingRecipe> GetAllRecipes()
        {
            return new List<CraftingRecipe>(recipes);
        }
    }
}
