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
    //TODO, restrict to put anything on unecessary on plates
    public override bool Interact(FoodItemTag item, rdEntity user) {
        switch (Status) {
            case StationStatus.Inactive:
                if (user.ItemOnHand == FoodItemTag.None)
                    return false;
                CurrentlyHolding.Add(user.ItemOnHand);
                user.DropOffItem();
                bool correctDish = CurrentlyHolding.OrderBy(x => x).SequenceEqual(_requiredMenus.OrderBy(x => x));
                //need to be checked again XD
                if (correctDish) {
                    Status = StationStatus.Collect;
                    FinalPlating = newRecipeManager.Seele._currentRecipe.DishName;
                    rdUIManager.UpdateStationPopups(this.gameObject, FinalPlating);
                    rdUIManager.UpdateOnHandItem(user.ItemOnHand, user);
                    return false;
                }
                rdUIManager.UpdateStationPopups(gameObject);
                rdUIManager.UpdateOnHandItem(user.ItemOnHand, user);
                break;
            case StationStatus.Collect:
                CurrentlyHolding.Clear();
                user.ItemOnHand = FinalPlating;
                Status = StationStatus.Inactive;
                rdUIManager.UpdateStationPopups(gameObject);
                rdUIManager.UpdateOnHandItem(user.ItemOnHand, user);
                break;
            default:
                break;
        }
        return false;
    }

    public override void ResetRecipe() {
        throw new System.NotImplementedException();
    }

    private void OnDestroy() {
        if (newRecipeManager.Seele != null) {
            newRecipeManager.Seele.OnNewRecipe -= OnNewRecipe;
            newRecipeManager.Seele.OnHoldInstructionUpdate -= OnHoldInstructionUpdate;
        }
    }

    private void Start() {
        newRecipeManager.Seele.OnNewRecipe += OnNewRecipe;
        newRecipeManager.Seele.OnHoldInstructionUpdate += OnHoldInstructionUpdate;
        OnNewRecipe();
    }

    public void OnNewRecipe() {
        CurrentlyHolding.Clear();
        _requiredMenus.Clear();
        int reqItemsIdx = newRecipeManager.Seele._currentRecipe.Instructions.Length - 1;
        print(reqItemsIdx);
        RecipeInstruction rcpIns = newRecipeManager.Seele._currentRecipe.Instructions[reqItemsIdx];
        foreach (var item in rcpIns.Ingredients) {
            _requiredMenus.Add(item);
        }
    }

    void OnHoldInstructionUpdate(FoodItemTag food) {
        foreach (var item in _requiredMenus) {
            if (item == food) {
                rdUIManager.UpdateStationPopups(this.gameObject, food);
                break;
            }
        }

        //List<FoodItemTag> fds = new List<FoodItemTag>();
        //foreach (RecipeInstruction item in RecipeMenu) {
        //    fds.Add(item.Result);
        //}

        ////to be updated later XD
        //foreach (RecipeInstruction rcp in RecipeMenu) {
        //    foreach (FoodItemTag correctFood in rcp.Ingredients) {
        //        if (food == correctFood) {
        //            rdUIManager.UpdateStationPopups(this.gameObject, correctFood);
        //            break;
        //        }
        //    }
        //}
    }
}
