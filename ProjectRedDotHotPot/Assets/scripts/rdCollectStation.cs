using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class rdCollectStation : rdStation
{
    public StationStatus Status;
    public FoodItemTag[] Ingredients;
    public List<FoodItemTag> IngredientsInUse;
    //public bool CurrentInUse;
    private void Awake()
    {
        OrderCor.Sub(0, GameStart);
    }
    private void GameStart(object sender)
    {
        foreach(FoodItemTag i in Ingredients)
        rdRecipeManager.Seele.IngredientStations[i] = this;
    }
    public void SetRecipe(FoodItemTag ingredient)
    {
        IngredientsInUse.Add(ingredient);
        Status = StationStatus.Ready;
    }
    public override void ResetRecipe()
    {
        IngredientsInUse.Clear();
        Status = StationStatus.Inactive;
    } 
    public override bool Interact(FoodItemTag item, rdEntity user)
    {
        if (user.ItemOnHand != FoodItemTag.None)
            return false;
        user.ItemOnHand = IngredientsInUse[0];
        IngredientsInUse.RemoveAt(0);
        if(IngredientsInUse.Count<=0)
            Status = StationStatus.Inactive;
        return false;
    }
}
