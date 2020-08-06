﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rdCustomerStation : rdStation
{
    [HideInInspector]
    public SpriteRenderer CustomerSprite;
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

    private void OnDestroy() {
        if (rdRecipeManager.Seele != null) {
            rdRecipeManager.Seele.OnNewRecipe -= OnNewRecipe;          
        }
    }

    private void Start() {
        rdRecipeManager.Seele.OnNewRecipe += OnNewRecipe;
        CustomerSprite = this.GetComponent<SpriteRenderer>();
        OnNewRecipe();
    }

    void OnNewRecipe() {
        CustomerSprite.sprite = rdRecipeManager.Seele._currentCustomer.CustomerSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
