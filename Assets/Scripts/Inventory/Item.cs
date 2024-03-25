using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public Sprite ItemSprite { get; set; }
    [field: SerializeField] public int Cost { get; set; }
    [field: SerializeField] public int Value { get; set; }
    [field: SerializeField, TextArea] public string Descriptions;
    [field: SerializeField] public bool IsUnlocked { get; set; }
    [field: SerializeField] public bool IsStackable{ get; set; }
    [field: SerializeField] public int MaxStackSize{ get; set; } = 1;
    [field: SerializeField] public bool IsFuelResource { get; set; }
    [field: SerializeField] public float FuelAmount { get; set; }
    [field: SerializeField] public ItemType ItemType;
    [field: SerializeField] public List<CraftingRecipe> CraftingRecipes { get; set; }
}

[System.Serializable]
public struct CraftingRecipe
{
    [field: SerializeField] public Item NeededMaterial { get; set; }
    [field: SerializeField] public int NumberOfNeededMaterial { get; set; }
}

public enum ItemType
{
    Crop = 0,
    CropSeed,
    Mineral,
    Tools,
    SpecialItems,
    None,
};

// [CanEditMultipleObjects]
// [CustomEditor(typeof(Item), true)]
// public class ItemEditor : Editor
// {
//     public override void OnInspectorGUI()
//     {
//         var itemScript = (Item)target;
//         var allSelectedScripts = targets;

//         EditorGUI.BeginChangeCheck();

//         //Serialize the script
//         EditorGUI.BeginDisabledGroup(true);
//         EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((ScriptableObject)target), GetType(), false); 
//         EditorGUI.EndDisabledGroup();
//         EditorGUILayout.Space(5);

//         //Serialize items' names
//         itemScript.Name = EditorGUILayout.TextField("Name", itemScript.Name);

//         //Serialize items' sprites
//         EditorGUILayout.BeginHorizontal();
//         EditorGUILayout.PrefixLabel("Item Sprite");
//         itemScript.ItemSprite = (Sprite)EditorGUILayout.ObjectField(itemScript.ItemSprite, typeof(Sprite), allowSceneObjects : true);
//         EditorGUILayout.EndHorizontal();

//         //Serialize Cost & Value
//         itemScript.IsShowingCostAndValueBackground = EditorGUILayout.Foldout(itemScript.IsShowingCostAndValueBackground, "Cost & Value", true);
//         if(itemScript.IsShowingCostAndValueBackground)
//         {
//             itemScript.Cost = EditorGUILayout.IntField("Cost", itemScript.Cost);
//             itemScript.Value = EditorGUILayout.IntField("Value", itemScript.Value);
//         }
        
//         //Serialize items' descriptions
//         itemScript.Descriptions = (string)EditorGUILayout.TextField("Descriptions", itemScript.Descriptions, GUILayout.MaxHeight(60));

//         EditorGUILayout.Space(10);

//         //Serialize IsUnlocked?
//         itemScript.IsUnlocked = EditorGUILayout.Toggle("Is Unlocked", itemScript.IsUnlocked);

//         //Serialzie IsStackable?, if yes, then serialize the max stack size
//         itemScript.IsStackable = EditorGUILayout.Toggle("Is Stackable", itemScript.IsStackable);
        
//         if(itemScript.IsStackable)
//             itemScript.MaxStackSize = EditorGUILayout.IntField("Max Stack Size", itemScript.MaxStackSize);

//         //Serialzie IsFuelResource?, if yes, then serialize the fuel amount
//         itemScript.IsFuelResource = EditorGUILayout.Toggle("Is Fuel Resource", itemScript.IsFuelResource);
        
//         if(itemScript.IsFuelResource)
//             itemScript.FuelAmount = EditorGUILayout.FloatField("Fuel Amount", itemScript.FuelAmount);

//         itemScript.ItemType = (ItemType)EditorGUILayout.EnumFlagsField("Item Type", itemScript.ItemType); 

//         if(EditorGUI.EndChangeCheck())
//         {
//             foreach(var script in allSelectedScripts)
//             {
//                 ((Item)script).Name = itemScript.Name;
//                 ((Item)script).ItemSprite = itemScript.ItemSprite;
//                 ((Item)script).IsShowingCostAndValueBackground = itemScript.IsShowingCostAndValueBackground;
//                 ((Item)script).Cost = itemScript.Cost;
//                 ((Item)script).Value = itemScript.Value;
//                 ((Item)script).Descriptions = itemScript.Descriptions;
//                 ((Item)script).IsUnlocked = itemScript.IsUnlocked;
//                 ((Item)script).IsStackable = itemScript.IsStackable;
//                 ((Item)script).MaxStackSize = itemScript.MaxStackSize;
//                 ((Item)script).IsFuelResource = itemScript.IsFuelResource;
//                 ((Item)script).FuelAmount = itemScript.FuelAmount;

//                 SceneView.RepaintAll();
//             }
//         }

//         EditorGUILayout.Space(20);
//         base.OnInspectorGUI();
//     }
// }