using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class newPlate : rdStation {
    public WorkstationTag Tag;
    public StationStatus Status;

    public List<FoodItemTag> CurrentlyHolding = new List<FoodItemTag>();
    public List<FoodItemTag> _requiredMenus = new List<FoodItemTag>();
    public FoodItemTag FinalPlating;

    public override bool Interact(FoodItemTag item, rdEntity user) {
        switch (Status) {
            case StationStatus.Inactive:
                if (user.ItemOnHand == FoodItemTag.None)
                    return false;
                CurrentlyHolding.Add(user.ItemOnHand);
                user.DropOffItem();
                //think of a more optimized way later XD
                bool correctDish = CurrentlyHolding.OrderBy(x => x).SequenceEqual(_requiredMenus.OrderBy(x => x));
                if (correctDish) {
                    Status = StationStatus.Collect;
                }
                break;
            case StationStatus.Collect:
                CurrentlyHolding.Clear();
                FinalPlating = newRecipeManager.Seele._currentRecipe.DishName;
                user.ItemOnHand = FinalPlating;
                Status = StationStatus.Inactive;
                break;
            default:
                break;
        }

        if (CurrentlyHolding.Count <= 0)
            rdUIManager.UpdateStationPopups(gameObject);
        else
            rdUIManager.UpdateStationPopups(gameObject, CurrentlyHolding);
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
        CurrentlyHolding.Clear();
        _requiredMenus.Clear();
        foreach (var reqMenu in newRecipeManager.Seele._currentRecipe.RequiredProcessedResults) {
            _requiredMenus.Add(reqMenu);
        }
    }
}
