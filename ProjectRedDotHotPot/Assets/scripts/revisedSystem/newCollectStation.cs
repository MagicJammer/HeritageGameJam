using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newCollectStation : rdStation {
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
        return false;
    }

    public override void ResetRecipe() {
        throw new System.NotImplementedException();
    }

    private void OnDestroy() {
        if (newRecipeManager.Seele != null)
            newRecipeManager.Seele.OnNewRecipe -= OnNewRecipe;
    }

    private void Start() {
        newRecipeManager.Seele.OnNewRecipe += OnNewRecipe;
        OnNewRecipe();
    }

    public void OnNewRecipe() {

        //subtract the last item
        //TODO variety of food distribution
        RecipeInstruction[] instructions = newRecipeManager.Seele._currentRecipe.Instructions;
        for (int i = 0; i < instructions.Length - 1; i++) {
            foreach (FoodItemTag item in instructions[i].Ingredients) {
                FoodItemTag newIngredient = item;
                //foreach (var xFood in RestrictIngredients) {
                //    if (newIngredient != xFood) {
                //        IngredientsToPickedUp.Add(newIngredient);
                //    }
                //}

                IngredientsToPickedUp.Add(item);
            }
        }
        Status = StationStatus.Ready;
        rdUIManager.UpdateStationPopups(this.gameObject, IngredientsToPickedUp);
    }
}