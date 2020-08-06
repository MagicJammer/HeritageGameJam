using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rdCollectStation : rdStation {
    public WorkstationTag Tag;
    public StationStatus Status;
    public FoodItemTag[] RestrictIngredients;
    //hide in inspector later
    public List<FoodItemTag> IngredientsToPickedUp = new List<FoodItemTag>();
    //List<FoodItemTag> _currentRecipeIngredients = new List<FoodItemTag>();

    //interact only true if the player is need to be working
    public override bool Interact(FoodItemTag item, rdEntity user) {
        if (user.ItemOnHand != FoodItemTag.None)
            return false;
        //TODO player will manually picked what item
        user.ItemOnHand = IngredientsToPickedUp[0];
        IngredientsToPickedUp.RemoveAt(0);
        if (IngredientsToPickedUp.Count <= 0)
            Status = StationStatus.Inactive;

        if (IngredientsToPickedUp.Count <= 0)
            rdUIManager.UpdateStationPopups(gameObject);
        else
            rdUIManager.UpdateStationPopups(gameObject, IngredientsToPickedUp);
        rdUIManager.UpdateOnHandItem(user.ItemOnHand, user);

        rdRecipeManager.UpdateInstruction(user.ItemOnHand);
        return false;
    }

    public override void ResetRecipe() {
        throw new System.NotImplementedException();
    }

    private void OnDestroy() {
        if (rdRecipeManager.Seele != null)
            rdRecipeManager.Seele.OnNewRecipe -= OnNewRecipe;
    }

    private void Start() {
        rdRecipeManager.Seele.OnNewRecipe += OnNewRecipe;
        OnNewRecipe();
    }

    public void OnNewRecipe() {
        foreach (var item in rdRecipeManager.Seele._currentRecipe.IngredientsToPickup) {
            foreach (var ing in RestrictIngredients) {
                if (ing == item) {
                    IngredientsToPickedUp.Add(ing);
                }
            }
        }
        Status = StationStatus.Ready;
        rdUIManager.UpdateStationPopups(this.gameObject, IngredientsToPickedUp);
    }
}