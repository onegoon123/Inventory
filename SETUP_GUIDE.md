# Unity 인벤토리 시스템 설정 가이드

이 가이드는 Unity Editor에서 인벤토리 시스템을 설정하는 방법을 단계별로 안내합니다.

## 📁 프로젝트 구조

모든 스크립트가 생성되었습니다:
```
Assets/
├── Scripts/
│   ├── Model/
│   │   ├── ItemType.cs
│   │   ├── ItemData.cs
│   │   ├── CraftingRecipe.cs
│   │   └── InventoryModel.cs
│   ├── View/
│   │   ├── InventorySlotView.cs
│   │   ├── DragVisual.cs
│   │   └── InventoryView.cs
│   └── Controller/
│       ├── CraftingSystem.cs
│       └── InventoryController.cs
└── Icons/
    ├── Wood.png
    ├── Stone.png
    ├── IronOre.png
    ├── Axe.png
    └── Pickaxe.png
```

## 🎨 1단계: 아이콘 설정

1. Unity Editor에서 `Assets/Icons` 폴더의 모든 이미지를 선택
2. Inspector에서 **Texture Type**을 `Sprite (2D and UI)`로 변경
3. **Apply** 버튼 클릭

## 📦 2단계: ItemData ScriptableObject 생성

1. Project 창에서 `Assets/Data/Items` 폴더 생성
2. 우클릭 → `Create` → `Inventory` → `Item Data`
3. 다음 아이템들을 생성:

### 나무 (Wood)
- Item Name: `나무`
- Icon: `Wood.png` 드래그
- Item Type: `Material`

### 돌 (Stone)
- Item Name: `돌`
- Icon: `Stone.png` 드래그
- Item Type: `Material`

### 철광석 (Iron Ore)
- Item Name: `철광석`
- Icon: `IronOre.png` 드래그
- Item Type: `Material`

### 도끼 (Axe)
- Item Name: `도끼`
- Icon: `Axe.png` 드래그
- Item Type: `Tool`

### 곡괭이 (Pickaxe)
- Item Name: `곡괭이`
- Icon: `Pickaxe.png` 드래그
- Item Type: `Tool`

## 🔧 3단계: CraftingRecipe ScriptableObject 생성

1. Project 창에서 `Assets/Data/Recipes` 폴더 생성
2. 우클릭 → `Create` → `Inventory` → `Crafting Recipe`
3. 다음 레시피들을 생성:

### Recipe_Axe
- Ingredient A: `나무` (Wood) 아이템 드래그
- Ingredient B: `돌` (Stone) 아이템 드래그
- Result: `도끼` (Axe) 아이템 드래그

### Recipe_Pickaxe
- Ingredient A: `돌` (Stone) 아이템 드래그
- Ingredient B: `철광석` (Iron Ore) 아이템 드래그
- Result: `곡괭이` (Pickaxe) 아이템 드래그

## 🎯 4단계: UI Prefab 생성 - InventorySlot

1. Hierarchy에서 우클릭 → `UI` → `Image` (이름: `InventorySlot`)
2. InventorySlot 선택 후 Inspector에서:
   - Width: `80`
   - Height: `80`
   - Color: 연한 회색 `#CCCCCC`

3. InventorySlot 하위에 Image 추가 (이름: `Icon`)
   - Anchor: Center-Center
   - Width: `60`
   - Height: `60`
   - **Raycast Target: OFF** (중요!)

4. InventorySlot에 `InventorySlotView` 스크립트 컴포넌트 추가
5. Inspector에서:
   - Icon Image: `Icon` 오브젝트 드래그
   - Background Image: `InventorySlot` 자신 드래그
   - Normal Color: 흰색 `#FFFFFF`
   - Highlight Color: 노란색 `#FFFF00`
   - Empty Slot Color: 연한 회색 반투명 `#FFFFFF80`

6. `Assets/Prefabs` 폴더 생성 후 **InventorySlot을 Prefab으로 저장**
7. Hierarchy에서 InventorySlot 삭제

## 📱 5단계: UI Prefab 생성 - DragVisual

1. Hierarchy에서 우클릭 → `UI` → `Image` (이름: `DragVisual`)
2. Inspector에서:
   - Width: `60`
   - Height: `60`
   - Image 컴포넌트의 Raycast Target: **OFF**

3. `Canvas Group` 컴포넌트 추가
   - Alpha: `0.6`
   - Blocks Raycasts: **OFF**

4. `DragVisual` 스크립트 컴포넌트 추가
5. Inspector에서:
   - Icon Image: DragVisual의 Image 컴포넌트 드래그
   - Canvas Group: 방금 추가한 Canvas Group 드래그

6. **DragVisual을 Prefab으로 저장** (`Assets/Prefabs/DragVisual`)
7. Hierarchy에서 DragVisual 삭제

## 🎨 6단계: UI Prefab 생성 - InventoryCanvas

1. Hierarchy에서 우클릭 → `UI` → `Canvas` (이름: `InventoryCanvas`)
2. Canvas 설정:
   - Render Mode: `Screen Space - Overlay`

3. InventoryCanvas 하위에 `UI` → `Panel` 추가 (이름: `InventoryPanel`)
4. InventoryPanel 설정:
   - Anchor: Center
   - Width: `720`
   - Height: `720`
   - Color: 어두운 배경 `#2D2D2D`

5. InventoryPanel에 `Grid Layout Group` 컴포넌트 추가:
   - Cell Size: X=`80`, Y=`80`
   - Spacing: X=`10`, Y=`10`
   - Constraint: `Fixed Column Count` = `8`
   - Child Alignment: `Middle Center`

6. InventoryPanel에 `Content Size Fitter` 컴포넌트 추가:
   - Horizontal Fit: `Preferred Size`
   - Vertical Fit: `Preferred Size`

7. InventoryCanvas에 `InventoryView` 스크립트 컴포넌트 추가
8. Inspector에서:
   - Slot Prefab: `InventorySlot` Prefab 드래그
   - Grid Container: `InventoryPanel` Transform 드래그
   - Canvas: `InventoryCanvas` 자신의 Canvas 컴포넌트 드래그
   - Drag Visual Prefab: `DragVisual` Prefab 드래그

9. **InventoryCanvas를 Prefab으로 저장** (`Assets/Prefabs/InventoryCanvas`)

## 🎮 7단계: Scene 설정

1. `Assets/Scenes/SampleScene` 열기
2. Hierarchy에 빈 GameObject 생성 (이름: `InventoryManager`)
3. `InventoryController` 스크립트 컴포넌트 추가
4. Inspector에서:
   - Inventory View: Hierarchy의 `InventoryCanvas` → `InventoryView` 컴포넌트 드래그
   - Crafting Recipes 크기: `2`로 설정
   - Element 0: `Recipe_Axe` 드래그
   - Element 1: `Recipe_Pickaxe` 드래그

5. **테스트용 아이템 추가** (선택사항):
   - `InventoryController`의 Public API를 활용하여 아이템 추가하는 간단한 테스트 스크립트 작성 가능

## ✅ 8단계: 테스트 스크립트 생성 (선택사항)

프로젝트에 테스트용 아이템을 자동으로 추가하려면 다음 스크립트를 생성하세요:

```csharp
using UnityEngine;
using InventorySystem.Controller;
using InventorySystem.Model;

public class InventoryTester : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private ItemData[] testItems;

    void Start()
    {
        // 테스트 아이템 추가
        foreach (var item in testItems)
        {
            inventoryController.AddItem(item);
        }
    }
}
```

이 스크립트를 `InventoryManager`에 추가하고 테스트할 아이템들을 드래그하세요.

## 🎉 완료!

이제 Play 버튼을 눌러 인벤토리 시스템을 테스트할 수 있습니다:

### 테스트 항목:
1. ✅ **드래그 앤 드롭**: 아이템을 드래그하여 다른 슬롯으로 이동
2. ✅ **아이템 교환**: 아이템을 다른 아이템 위로 드래그하여 위치 교환
3. ✅ **조합**: 나무를 돌 위로 드래그하여 도끼 생성
4. ✅ **조합**: 돌을 철광석 위로 드래그하여 곡괭이 생성

## 🐛 문제 해결

### 드래그가 작동하지 않는 경우:
- Canvas에 `Graphic Raycaster` 컴포넌트가 있는지 확인
- EventSystem이 Scene에 있는지 확인 (없으면 자동 생성됨)

### 아이콘이 보이지 않는 경우:
- 아이콘 이미지의 Texture Type이 `Sprite (2D and UI)`인지 확인
- ItemData의 Icon 필드에 Sprite가 할당되었는지 확인

### 조합이 작동하지 않는 경우:
- CraftingRecipe의 Ingredient A, B, Result가 모두 할당되었는지 확인
- InventoryController의 Crafting Recipes 리스트에 레시피가 추가되었는지 확인
