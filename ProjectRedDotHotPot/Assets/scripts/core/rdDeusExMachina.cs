using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct Recipe
{
    public FoodItemTag DishName;
    public RecipeInstruction[] Instructions;
    public FoodItemTag[] IngredientsToPickup;
}
[Serializable]
public struct RecipeInstruction
{
    public FoodItemTag[] Ingredients;
    public WorkstationTag Workstation;
    public float ProcessTime;
    public TaskType Type;
    public FoodItemTag Result;
}

public enum TaskType
{
    Active,
    Automatic
}
public enum FoodItemTag
{
    None,
    //1-99 raw
    INGREDIENTS = 1,
    POULTRY,
    Beef,
    DuckMeat,
    DicedChicken,
    ChickenSkewers,
    DicedPork,
    SEAFOOD,
    Prawns,
    Fishcakes,
    SPICEGARDEN,
    Chili,
    Garlic,
    GreenOnion,
    Tapioca,
    LaksaLeaf,
    Pandanleaves,
    MintLeaves,
    Onions,
    SEASONINGSTUFF,
    Oil,
    RedBean,
    PalmSugar,
    CoconutMilk,
    CoconutShavings,
    CurryPowder,
    JellyNoodles,
    ShavedIce,
    Butter,
    DarkSoySauce,
    SpicesPowder,
    DOUGHSTUFF,
    Paranthas,
    Noodles,
    NaanBread,
    Rice,
    Yam,
    //101-199 processed menu
    PROCESSEDMENU = 100,
    MincedBeef,
    ChoppedGarlic,
    SlicedChili,
    CutOnions,
    FriedNaanBread,
    ButterChicken,
    

    //201-299 final menu
    FINALMENU = 200,
    BeefKwayTeow = 201,
    ButterChickenNaan,

    //999 plate or bowl
    //PlateOrBowl = 999,

}
public enum WorkstationTag
{
    Counter, PlateOrBowl, ChoppingBoard, Pot, FryingPan, MortarPestle, RiceCooker, None, Collect,
}
public enum StationStatus
{
    Inactive,//not used by recipe
    Ready,//used by recipe
    Cooking,
    Collect,
    Waiting,

}

[System.Serializable]
public struct StoryData {
    [TextArea(3, 5)]
    public string StoryLine;
    public float TextDelay;
}

[System.Serializable]
public struct ChatData {
    public string ChatLine;
    public float TextDelay;
}
public abstract class rdStation : MonoBehaviour
{
    public abstract void ResetRecipe();
    public abstract bool Interact(FoodItemTag item, rdEntity user);
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        rdEntity user = go.GetComponent<rdEntity>();
        user.SelectedStation = this;
    }
}