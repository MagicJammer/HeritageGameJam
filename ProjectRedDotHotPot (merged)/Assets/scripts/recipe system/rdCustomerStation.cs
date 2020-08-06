using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rdCustomerStation : rdStation
{
    public override bool Interact(FoodItemTag item, rdEntity user) {
        if (item == rdRecipeManager.Seele._currentRecipe.DishName) {
            rdRecipeManager.Seele.OrderServed();
            Debug.Log(item + "served");

            user.ItemOnHand = FoodItemTag.None;
            rdUIManager.UpdateOnHandItem(user.ItemOnHand, user);

        } else {
            //random chat thingyy
            ChatData[] chats = rdRecipeManager.Seele._currentChatdatas;
            rdUIManager.ShowRandomChatText(chats);
            return false;
        }
        return false;
    }

    public override void ResetRecipe() {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
