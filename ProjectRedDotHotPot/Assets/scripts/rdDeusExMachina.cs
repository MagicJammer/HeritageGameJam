using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CustomerRecipe", menuName = "MagicJammer/CustomerRecipe", order = 1)]
public class CustomerRecipe : ScriptableObject
{
    public int OrderNumber;
    public Recipe DishRecipe;
}
[CreateAssetMenu(fileName = "IngredientBag", menuName = "MAGES/CustomerRecipe", order = 2)]
public class FoodItemBag : ScriptableObject
{
    public FoodItemTag Tag;
    public GameObject Prefab;
}


[Serializable]
public struct Recipe
{
    public string DishName;
    public RecipeInstruction[] Instructions;
}
public struct RecipeInstruction
{
    public FoodItemTag[] Ingredients;
    public WorkstationTag Workstation;
    public float ProcessTime;
    public TaskType Type;
    public FoodItemTag Result;
}
public struct WorkData
{
    public WorkstationTag Workstation;
    public float ProcessTime;
}
public enum TaskType
{
    Active,
    Automatic
}
public enum FoodItemTag
{
    None,
}
public enum WorkstationStatus
{
    Inactive,//not used by recipe
    Ready,//used by recipe
    Cooking,
    Collect,

}
public enum WorkstationTag
{

}
public abstract class rdStation : MonoBehaviour
{
    public abstract void ResetRecipe();
    public abstract bool Interact(FoodItemTag item, rdEntity user);
}