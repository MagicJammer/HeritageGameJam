using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rdCollectStation : rdStation
{
    public FoodItemTag[] Ingredients;
    public List<FoodItemTag> IngredientsInUse;
    public bool CurrentInUse;
    public void SetRecipe(FoodItemTag ingredient)
    {
        IngredientsInUse.Add(ingredient);
    }
    public override void ResetRecipe()
    {
        IngredientsInUse.Clear();
    } 
    public override bool Interact(FoodItemTag item, rdEntity user)
    {
        return false;
    }
}
