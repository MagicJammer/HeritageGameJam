//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class rdCustomerCounter : rdWorkstation
//{
//    public override bool Interact(FoodItemTag item, rdEntity user)
//    {
//        if (item == rdRecipeManager.Seele.CurrentRecipe.DishName)
//        {
//            rdRecipeManager.Seele.OrderServed();
//            Debug.Log(item + "served");

//            user.ItemOnHand = FoodItemTag.None;
//            rdUIManager.UpdateOnHandItem(user.ItemOnHand, user);
//        } else {
//            //random chat thingyy
//            ChatData[] chats = rdRecipeManager.Seele._currentChatdatas;
//            rdUIManager.ShowRandomChatText(chats);
//            //rdRecipeManager.Seele.Chat();
//            return false;
//        }
//        return false;
//    }

//}
