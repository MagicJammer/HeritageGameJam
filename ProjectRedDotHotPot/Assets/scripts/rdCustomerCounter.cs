using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rdCustomerCounter : rdWorkstation
{
    public override bool Interact(FoodItemTag item, rdEntity user)
    {
        if (item == rdRecipeManager.Seele.CurrentRecipe.DishName)
        {
            rdRecipeManager.Seele.OrderServed();
            Debug.Log(item + "served");

            //test only
            user.ItemOnHand = FoodItemTag.None;
            rdUIManager.UpdateStationPopups(gameObject);
            rdUIManager.UpdateOnHandItem(user.ItemOnHand, user);
        } else
            rdRecipeManager.Seele.Chat();
            return false;
    }
}
