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
    //POULTRY,
    Beef,
    Butter,
    Cabbage,
    Chicken,
    Chili,
    CurryPowder,
    DuckMeat,
    DicedChicken,
    Egg,
    Fishcakes,
    ChickenSkewers,
    Garlic,
    GreenOnion,
    JellyNoodles,
    LaksaLeaf,
    CoconutMilk,
    CoconutShavings,
    MintLeaves,
    NaanBread,
    Noodles,
    Onion,
    PalmSugar,
    PandanLeaves,
    //SEAFOOD,
    Pork,
    Prawns,
    RedBean,
    Rice,
    ShavedIce,
    DarkSoySauce,
    Yam,
    SpicesPowder,

    //SPICEGARDEN,
    Tapioca,
    //SEASONINGSTUFF,
    Oil,
    //DOUGHSTUFF,
    Paranthas,
    //101-199 processed menu
    PROCESSEDMENU = 100,
    DicedPork,
    MincedBeef,
    ChoppedGarlic,
    SlicedChili,
    CutOnions,
    FriedNaanBread,
    ButterChicken,
    BoiledEgg,
    CrispyFriedPork,
    CookedPrawn,
    ChoppedDuckMeat,
    DicedYam,
    CrushedPandanLeaves,
    SlicedGreenOnions,
    SteamedYamRice,
    BoiledRedBean,
    CrushedPalmSugar,
    MeltedPalmSugar,
    GratedTapioca,
    SteamedTapioca,
    PanFriedChicken,
    FriedParanthas,
    CrushedMintleaves,
    SlicedFishCakes,
    LaksaLeafFlakes,


    //201-299 final menu
    FINALMENU = 200,
    BeefKwayTeow = 201,
    ButterChickenNaan,
    KwayChap,
    BraisedDuckRice,
    Chendol,
    KuehUbiKayu,
    ChickenKebabRoll,
    Laksa,
    //999 plate or bowl
    //PlateOrBowl = 999,

}
public enum WorkstationTag
{
    Deleted, PlateOrBowl, ChoppingBoard, Pot, FryingPan, MortarPestle, RiceCooker, None, Collect,
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
    public Speaker Speaker;
    public float TextDelay;
}

public enum Speaker {
    Customer,
    Player,
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